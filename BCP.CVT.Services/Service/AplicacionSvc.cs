﻿using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.DTO.Custom;
using BCP.CVT.Services.Interface;
using BCP.CVT.Services.ModelDB;
using BCP.PAPP.Common.Cross;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Dynamic;
using System.Transactions;
using IsolationLevel = System.Transactions.IsolationLevel;
using System.Data.Entity.SqlServer;
using RestSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RestSharp.Authenticators;
using System.Net;
using BCP.CVT.Services.Log;

namespace BCP.CVT.Services.Service
{
    public class AplicacionSvc : AplicacionDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void CallPipelineJenkins(string codApp)
        {
            var objURL = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("URL_CLIENTE_JENKINS");
            var objUsername = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("USERNAME_CLIENTE_JENKINS");
            var objPassword = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("PASSWORD_CLIENTE_JENKINS");

            System.Net.ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);

            log.Debug("Iniciando conexión");
            RestClient client = null;
            RestRequest request = null;
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            log.DebugFormat("Intentando conexión al host: {0}", objURL.Valor);
            client = new RestClient(objURL.Valor + codApp);
            client.Authenticator = new HttpBasicAuthenticator(objUsername.Valor, objPassword.Valor);
            string arg = objUsername.Valor + ":" + objPassword.Valor;
            log.DebugFormat("Generando token de autorización: {0}", arg);

            request = new RestRequest(Method.POST);

            request.RequestFormat = DataFormat.Json;
            request.AddHeader("Content-Type", "application/json;charset=utf-8");
            log.DebugFormat("Código de aplicación enviada: {0}", codApp);

            var response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var jsonRetorno = response.Content;
                log.Debug("HTTP Status code response: 200");
                log.DebugFormat("Respuesta: {0}", jsonRetorno);
            }
            else if (response.StatusCode == HttpStatusCode.Created)
            {
                var jsonRetorno = response.Content;
                log.Debug("HTTP Status code response: 201");
                log.DebugFormat("Respuesta: {0}", jsonRetorno);
            }
            else
            {
                log.DebugFormat("HTTP Status code response: {0}", response.StatusCode.ToString());
                log.DebugFormat("HTTP Status Message: {0}", response.StatusDescription);
                log.DebugFormat("HTTP Error Message: {0}", response.ErrorMessage);
            }
        }

        public override int AddOrEdit(AplicacionDTO objRegistro)
        {
            try
            {
                int ID = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    if (objRegistro.Id == 0)
                    {
                        var entidad = new Aplicacion()
                        {
                            CodigoAPT = objRegistro.CodigoAPT,
                            Nombre = objRegistro.Nombre,
                            TipoActivoInformacion = objRegistro.TipoActivoInformacion,
                            GerenciaCentral = objRegistro.GerenciaCentral,
                            Division = objRegistro.Division,
                            Area = objRegistro.Area,
                            Unidad = objRegistro.Unidad,
                            DescripcionAplicacion = objRegistro.DescripcionAplicacion,
                            //EstadoAplicacionId = objRegistro.EstadoAplicacionProcedencia,
                            FechaRegistroProcedencia = objRegistro.FechaRegistroProcedencia,
                            FechaCreacionProcedencia = objRegistro.FechaRegistroProcedencia,
                            AreaBIAN = objRegistro.AreaBIAN,
                            DominioBIAN = objRegistro.DominioBIAN,
                            JefaturaATI = objRegistro.JefaturaATI,
                            TribeTechnicalLead = objRegistro.TribeTechnicalLead,
                            JefeEquipo_ExpertoAplicacionUserIT_ProductOwner = objRegistro.JefeEquipo_ExpertoAplicacionUserIT_ProductOwner,
                            NombreEquipo_Squad = objRegistro.NombreEquipo_Squad,
                            GestionadoPor = objRegistro.GestionadoPor,
                            Owner_LiderUsuario_ProductOwner = objRegistro.Owner_LiderUsuario_ProductOwner,
                            Gestor_UsuarioAutorizador_ProductOwner = objRegistro.Gestor_UsuarioAutorizador_ProductOwner,
                            EntidadResponsable = objRegistro.EntidadResponsable,

                            CriticidadId = objRegistro.CriticidadId,
                            CategoriaTecnologica = objRegistro.CategoriaTecnologica,
                            RoadMapId = objRegistro.RoadMapId,
                            FlagActivo = true,
                            FechaCreacion = DateTime.Now,
                            CreadoPor = objRegistro.UsuarioCreacion
                        };
                        ctx.Aplicacion.Add(entidad);
                        ctx.SaveChanges();

                        ID = entidad.AplicacionId;
                    }
                    else
                    {
                        var entidad = (from u in ctx.Aplicacion
                                       where u.CodigoAPT == objRegistro.CodigoAPT
                                       && u.FlagActivo
                                       select u).First();
                        if (entidad != null)
                        {
                            entidad.Nombre = objRegistro.Nombre;
                            entidad.TipoActivoInformacion = objRegistro.TipoActivoInformacion;
                            entidad.GerenciaCentral = objRegistro.GerenciaCentral;
                            entidad.Division = objRegistro.Division;
                            entidad.Area = objRegistro.Area;
                            entidad.Unidad = objRegistro.Unidad;
                            entidad.DescripcionAplicacion = objRegistro.DescripcionAplicacion;
                            //EstadoAplicacionId = objRegistro.EstadoAplicacionProcedencia,

                            entidad.FechaCreacionProcedencia = objRegistro.FechaRegistroProcedencia;
                            entidad.AreaBIAN = objRegistro.AreaBIAN;
                            entidad.DominioBIAN = objRegistro.DominioBIAN;
                            entidad.JefaturaATI = objRegistro.JefaturaATI;
                            entidad.TribeTechnicalLead = objRegistro.TribeTechnicalLead;  //3
                            entidad.JefeEquipo_ExpertoAplicacionUserIT_ProductOwner = objRegistro.JefeEquipo_ExpertoAplicacionUserIT_ProductOwner;
                            entidad.NombreEquipo_Squad = objRegistro.NombreEquipo_Squad;
                            entidad.GestionadoPor = objRegistro.GestionadoPor;
                            entidad.Owner_LiderUsuario_ProductOwner = objRegistro.Owner_LiderUsuario_ProductOwner; //1
                            entidad.Gestor_UsuarioAutorizador_ProductOwner = objRegistro.Gestor_UsuarioAutorizador_ProductOwner; //2
                            entidad.EntidadResponsable = objRegistro.EntidadResponsable;

                            entidad.CriticidadId = objRegistro.CriticidadId;
                            entidad.CategoriaTecnologica = objRegistro.CategoriaTecnologica;
                            entidad.RoadMapId = objRegistro.RoadMapId;
                            entidad.FlagActivo = objRegistro.Activo;
                            entidad.FechaModificacion = DateTime.Now;
                            entidad.ModificadoPor = objRegistro.UsuarioModificacion;
                            ctx.SaveChanges();
                            ID = entidad.AplicacionId;
                        }
                    }
                }
                return ID;
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: int AddOrEdit(AplicacionDTO objRegistro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: int AddOrEdit(AplicacionDTO objRegistro)"
                    , new object[] { null });
            }
        }

        public override int AddOrEditAPR(AplicacionPortafolioResponsablesDTO objeto, bool _flagSolicitud = false)
        {
            try
            {
                int ID = 0;
                DateTime FECHA_ACTUAL = DateTime.Now;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    if (objeto.Id == 0)
                    {
                        var entidad = new AplicacionPortafolioResponsables()
                        {
                            FlagActivo = true,
                            CreadoPor = objeto.UsuarioCreacion,
                            FechaCreacion = FECHA_ACTUAL,
                            CodigoAPT = objeto.CodigoAPT,
                            Matricula = objeto.Matricula,
                            Colaborador = objeto.Colaborador,
                            PortafolioResponsableId = objeto.PortafolioResponsableId,
                            FlagSolicitud = _flagSolicitud
                        };
                        ctx.AplicacionPortafolioResponsables.Add(entidad);
                        ctx.SaveChanges();

                        ID = entidad.AplicacionPortafolioResponsableId;
                    }
                    else
                    {
                        var entidad = (from u in ctx.AplicacionPortafolioResponsables
                                       where u.AplicacionPortafolioResponsableId == objeto.Id
                                       && u.FlagSolicitud == _flagSolicitud
                                       select u).FirstOrDefault();

                        if (entidad != null)
                        {
                            entidad.CodigoAPT = objeto.CodigoAPT;
                            entidad.Matricula = objeto.Matricula;
                            entidad.FechaModificacion = FECHA_ACTUAL;
                            entidad.ModificadoPor = objeto.UsuarioModificacion;

                            ctx.SaveChanges();

                            ID = entidad.AplicacionPortafolioResponsableId;
                        }
                    }

                    return ID;
                }
            }
            catch (DbEntityValidationException ex)
            {
                //transaction.Rollback();
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: int AddOrEditTipo(TipoDTO objeto)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                //transaction.Rollback();
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: int AddOrEditTipo(TipoDTO objeto)"
                    , new object[] { null });
            }
        }

        public override bool AddOrEditAplicacionExperto(ParametroList objRegistro)
        {
            try
            {
                bool estado = false;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    if (objRegistro.ListIdsEliminar != null)
                    {
                        foreach (var item in objRegistro.ListIdsEliminar)
                        {
                            var existeBd = (from u in ctx.AplicacionExpertos
                                            where u.AplicacionExpertoId == item.AppExpId && u.TipoExpertoId == item.TipExpId && u.CodigoAPT == objRegistro.CodigoAPT
                                            && u.FlagActivo == true
                                            select u).FirstOrDefault();
                            if (existeBd != null)
                            {
                                existeBd.FlagActivo = false;
                                existeBd.FechaModificacion = DateTime.Now;
                                existeBd.ModificadoPor = objRegistro.UsuarioModificacion;
                            }
                            ctx.SaveChanges();
                        }
                    }

                    if (objRegistro.ListIdsRegistrar != null)
                    {
                        foreach (var item in objRegistro.ListIdsRegistrar)
                        {
                            var existeBd = (from u in ctx.AplicacionExpertos
                                            where u.Matricula == item.Matricula && u.CodigoAPT == objRegistro.CodigoAPT && u.TipoExpertoId == item.TipoExpertoId.Value
                                            select u).FirstOrDefault();
                            if (existeBd != null)
                            {
                                existeBd.FlagActivo = true;
                                existeBd.FechaModificacion = DateTime.Now;
                                existeBd.ModificadoPor = objRegistro.UsuarioModificacion;
                            }
                            else
                            {
                                var objRegistroBd = new AplicacionExpertos()
                                {
                                    CodigoAPT = objRegistro.CodigoAPT,
                                    Matricula = item.Matricula,
                                    FlagActivo = true,
                                    FechaCreacion = DateTime.Now,
                                    CreadoPor = objRegistro.UsuarioModificacion,
                                    TipoExpertoId = item.TipoExpertoId.Value,
                                    Nombres = item.Nombres
                                };
                                ctx.AplicacionExpertos.Add(objRegistroBd);
                            }
                            ctx.SaveChanges();
                        }
                    }
                    estado = true;
                }
                return estado;
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: bool AddOrEditAplicacionExperto(ParametroList objRegistro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: bool AddOrEditAplicacionExperto(ParametroList objRegistro)"
                    , new object[] { null });
            }
        }
        public override bool AddOrEditComponentesApp(ComponenteConectaCon objRegistro)
        {
            try
            {
                bool estado = false;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    if (objRegistro.ListIdsEliminar != null)
                    {
                        foreach (var item in objRegistro.ListIdsEliminar)
                        {
                            using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                            {
                                cnx.Open();
                                using (var comando = new SqlCommand("[cvt].[USP_DiagramaInfraestructura_EliminarReglasPorApp]", cnx))
                                {
                                    comando.CommandType = System.Data.CommandType.StoredProcedure;
                                    comando.Parameters.AddWithValue("@RelacionReglasPorAppId", item.RelacionReglasPorAppId);
                                    comando.Parameters.AddWithValue("@ModificadoPor", objRegistro.UsuarioModificacion);

                                    var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);
                                    reader.Close();
                                }
                                cnx.Close();
                            }
                        }
                    }
                    if (objRegistro.ListIdsActivar != null)
                    {
                        foreach (var item in objRegistro.ListIdsActivar)
                        {
                            using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                            {
                                cnx.Open();
                                using (var comando = new SqlCommand("[cvt].[USP_DiagramaInfraestructura_ActivarReglasPorApp]", cnx))
                                {
                                    comando.CommandType = System.Data.CommandType.StoredProcedure;
                                    comando.Parameters.AddWithValue("@RelacionReglasPorAppId", item.RelacionReglasPorAppId);
                                    comando.Parameters.AddWithValue("@ModificadoPor", objRegistro.UsuarioModificacion);

                                    var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);
                                    reader.Close();
                                }
                                cnx.Close();
                            }
                        }
                    }

                    if (objRegistro.ListIdsRegistrar != null)
                    {
                        foreach (var item in objRegistro.ListIdsRegistrar)
                        {
                            if (item.RelacionReglasPorAppId==0)
                            {
                                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                                {
                                    cnx.Open();
                                    using (var comando = new SqlCommand("[cvt].[USP_DiagramaInfraestructura_InsertarReglasPorApp]", cnx))
                                    {
                                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                                        comando.Parameters.AddWithValue("@CodigoAPT", item.CodigoApt);
                                        comando.Parameters.AddWithValue("@EquipoId", item.EquipoId);
                                        comando.Parameters.AddWithValue("@TipoEquipoId", item.TipoEquipoId);
                                        comando.Parameters.AddWithValue("@EquipoId_Relacion", item.EquipoId_Relacion);
                                        comando.Parameters.AddWithValue("@TipoEquipoId_Relacion", item.TipoEquipoId_Relacion);
                                        comando.Parameters.AddWithValue("@CreadoPor", objRegistro.UsuarioModificacion);

                                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);
                                        reader.Close();
                                    }
                                    cnx.Close();
                                }
                            }
                        }
                    }
                    estado = true;
                }
                return estado;
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: bool AddOrEditComponentesApp(ComponenteConectaCon objRegistro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: bool AddOrEditComponentesApp(ComponenteConectaCon objRegistro)"
                    , new object[] { null });
            }
        }

        public override bool CambiarFlagExperto(int Id, string user, int fuente = 0)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    if (fuente == 0)
                    {
                        var itemBD = (from u in ctx.AplicacionExpertos
                                      where u.AplicacionExpertoId == Id
                                      select u).FirstOrDefault();

                        if (itemBD != null)
                        {
                            itemBD.FlagActivo = false;
                            itemBD.FechaModificacion = DateTime.Now;
                            itemBD.ModificadoPor = user;
                            ctx.SaveChanges();
                            return true;
                        }
                        else
                            return false;
                    }
                    else
                    {
                        var itemBD = (from u in ctx.AplicacionPortafolioResponsables
                                      where u.AplicacionPortafolioResponsableId == Id
                                      select u).FirstOrDefault();

                        if (itemBD != null)
                        {
                            itemBD.FlagActivo = false;
                            itemBD.FechaModificacion = DateTime.Now;
                            itemBD.ModificadoPor = user;
                            ctx.SaveChanges();
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
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: bool CambiarFlagRelacionar(AplicacionDTO objRegistro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: bool CambiarFlagRelacionar(AplicacionDTO objRegistro)"
                    , new object[] { null });
            }
        }

        public override bool CambiarFlagRelacionar(AplicacionDTO objRegistro)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var itemBD = (from u in ctx.Aplicacion
                                  where u.CodigoAPT == objRegistro.CodigoAPT
                                  && u.FlagActivo
                                  select u).FirstOrDefault();

                    if (itemBD != null)
                    {
                        itemBD.FlagRelacionar = objRegistro.FlagRelacionar;
                        itemBD.FechaModificacion = DateTime.Now;
                        itemBD.ModificadoPor = objRegistro.UsuarioModificacion;
                        ctx.SaveChanges();
                        return true;
                    }
                    else
                        return false;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: bool CambiarFlagRelacionar(AplicacionDTO objRegistro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: bool CambiarFlagRelacionar(AplicacionDTO objRegistro)"
                    , new object[] { null });
            }
        }

        public override List<TipoExpertoDTO> GetTipoExperto()
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.TipoExperto
                                     where u.FlagActivo
                                     select new TipoExpertoDTO()
                                     {
                                         Id = u.TipoExpertoId,
                                         Nombre = u.Nombres,
                                         Activo = u.FlagActivo
                                     }).ToList();

                    return registros;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: bool ExisteAplicacion(string nombre, string Id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: bool ExisteAplicacion(string nombre, string Id)"
                    , new object[] { null });
            }
        }

        public override bool ExisteAplicacionById(string Id)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    bool? estado = (from u in ctx.Aplicacion
                                    where u.FlagActivo
                                    //&& u.FlagRelacionar.Value
                                    && u.CodigoAPT.ToUpper().Equals(Id.ToUpper())
                                    orderby u.Nombre
                                    select true).FirstOrDefault();

                    return estado == true;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: bool ExisteAplicacion(string nombre, string Id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: bool ExisteAplicacion(string nombre, string Id)"
                    , new object[] { null });
            }
        }

        public override bool ExisteAplicacionById(string Id, bool FlagRelacionar)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    bool? estado = (from u in ctx.Aplicacion
                                    where u.FlagActivo && u.FlagRelacionar.Value == FlagRelacionar
                                    && u.CodigoAPT == Id
                                    orderby u.Nombre
                                    select true).FirstOrDefault();

                    return estado == true;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: bool ExisteAplicacion(string nombre, string Id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: bool ExisteAplicacion(string nombre, string Id)"
                    , new object[] { null });
            }
        }

        public override List<AplicacionDTO> GetAplicacion(PaginacionAplicacion pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                var fechaConsulta = DateTime.Now;

                // >> Certificados Digitales: Parámetro de Tipo de Equipo (cvt.TipoEquipo)
                var TipoEquipo_CD = Int32.Parse(ServiceManager<ParametroDAO>.Provider.ObtenerParametro("CERTIFICADO_DIGITAL_APLICACION_PARAMETRO_TIPOEQUIPO").Valor);
                // <<

                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        ctx.Database.CommandTimeout = 0;

                        var aplicacionExpertos = (from x in ctx.AplicacionExpertos
                                                  where x.Matricula == pag.username && x.FlagActivo
                                                  select x.CodigoAPT).ToList(); ;




                        var aplicacionPCI = (from x in ctx.ApplicationPCI

                                             where (pag.PCIS.Count == 0 || pag.PCIS.Contains(SqlFunctions.StringConvert((double)x.TipoPCIId).Trim())) && x.FlagActivo == true
                                             select x.ApplicationId).ToList();

                        //var aplicacionPCI = (from x in aplicacionPCI2 select x).Distinct();

                        var totalServidoresRelacionados = (from a in ctx.Relacion
                                                               //join b in ctx.Tecnologia on a.TecnologiaId equals b.TecnologiaId
                                                               // >> Excluyendo Certificados Digitales
                                                           join e in ctx.Equipo on a.EquipoId equals e.EquipoId
                                                           join te in ctx.TipoEquipo on e.TipoEquipoId equals te.TipoEquipoId
                                                           // <<
                                                           where a.AnioRegistro == fechaConsulta.Year
                                                           && a.MesRegistro == fechaConsulta.Month
                                                           && a.DiaRegistro == fechaConsulta.Day
                                                           && a.FlagActivo && a.TipoId == (int)ETipoRelacion.Equipo
                                                           && (a.EstadoId == (int)EEstadoRelacion.Aprobado || a.EstadoId == (int)EEstadoRelacion.PendienteEliminacion)
                                                           // >> Excluyendo Certificados Digitales
                                                           && te.TipoEquipoId != TipoEquipo_CD
                                                           // <<
                                                           group a by a.CodigoAPT into grp
                                                           select new
                                                           {
                                                               CodigoAPT = grp.Key,
                                                               NroTecnologias = grp.Count()
                                                           }).Distinct();

                        var registros = (from u in ctx.Aplicacion
                                         join u3 in ctx.Application on u.CodigoAPT equals u3.applicationId
                                         join u2 in ctx.Criticidad on u.CriticidadId equals u2.CriticidadId into cri
                                         from u2 in cri.DefaultIfEmpty()
                                         join r in ctx.RoadMap on u.RoadMapId equals r.RoadMapId into lj1
                                         from r in lj1.DefaultIfEmpty()
                                         join s in totalServidoresRelacionados on new { u.CodigoAPT } equals new { s.CodigoAPT } into lj2
                                         from s in lj2.DefaultIfEmpty()

                                         where (u.CodigoAPT.ToUpper().Contains(pag.Aplicacion.ToUpper())
                                         //|| u.Nombre.ToUpper().Contains(pag.Aplicacion.ToUpper())
                                         || (u.CodigoAPT + " - " + u.Nombre).ToUpper().Contains(pag.Aplicacion.ToUpper())
                                         || string.IsNullOrEmpty(pag.Aplicacion))
                                         //&& (u.EstadoAplicacion.ToUpper().Contains(pag.Estado.ToUpper()) || pag.Estado == null)
                                         //&& (u.GerenciaCentral.ToUpper().Contains(pag.Gerencia.ToUpper()) || pag.Gerencia == null)
                                         //&& (u.Division.ToUpper().Contains(pag.Division.ToUpper()) || pag.Division == null)
                                         //&& (u.Unidad.ToUpper().Contains(pag.Unidad.ToUpper()) || pag.Unidad == null)
                                         //&& (u.Area.ToUpper().Contains(pag.Area.ToUpper()) || pag.Area == null)

                                         && (pag.Estados.Count == 0 || pag.Estados.Contains(u.EstadoAplicacion.ToUpper()))
                                         && (pag.Gerencias.Count == 0 || pag.Gerencias.Contains(u.GerenciaCentral.ToUpper()))
                                         && (pag.Divisiones.Count == 0 || pag.Divisiones.Contains(u.Division.ToUpper()))
                                         && (pag.Unidades.Count == 0 || pag.Unidades.Contains(u.Unidad.ToUpper()))
                                         && (pag.Areas.Count == 0 || pag.Areas.Contains(u.Area.ToUpper()))
                                         && (pag.PCIS.Count == 0 || aplicacionPCI.Contains(u3.AppId))
                                         && (u.JefeEquipo_ExpertoAplicacionUserIT_ProductOwner.ToUpper().Contains(pag.JefeEquipo.ToUpper()) || string.IsNullOrEmpty(pag.JefeEquipo))
                                         && (u.Owner_LiderUsuario_ProductOwner.ToUpper().Contains(pag.Owner.ToUpper()) || string.IsNullOrEmpty(pag.Owner))
                                         && u.FlagActivo
                                         && (pag.PerfilId == (int)EPerfilBCP.Administrador || aplicacionExpertos.Contains(u.CodigoAPT))
                                         orderby u.Nombre
                                         select new AplicacionDTO()
                                         {
                                             Id = u.AplicacionId,
                                             CodigoAPT = u.CodigoAPT,
                                             CodigoAPTStr = u.CodigoAPT,
                                             Nombre = u.Nombre,
                                             GestionadoPor = u.GestionadoPor,
                                             CriticidadId = u.CriticidadId,
                                             RoadMapId = u.RoadMapId,
                                             RoadMapToString = r.Nombre,
                                             Matricula = u.Matricula,
                                             Obsolescente = u.Obsolescente,
                                             //MesAnio = u.MesAnio,
                                             Activo = u.FlagActivo,
                                             UsuarioCreacion = u.CreadoPor,
                                             FechaCreacion = u.FechaCreacion,
                                             FechaRegistroProcedencia = u.FechaRegistroProcedencia,
                                             FechaModificacion = u.FechaModificacion,
                                             UsuarioModificacion = u.ModificadoPor,
                                             FlagRelacionar = u.FlagRelacionar,
                                             CriticidadToString = u2.DetalleCriticidad,
                                             TipoActivoInformacion = u.TipoActivoInformacion,
                                             GerenciaCentral = u.GerenciaCentral,
                                             Division = u.Division,
                                             Unidad = u.Unidad,
                                             Area = u.Area,
                                             NombreEquipo_Squad = u.NombreEquipo_Squad,
                                             Owner_LiderUsuario_ProductOwner = u.Owner_LiderUsuario_ProductOwner,
                                             JefeEquipo_ExpertoAplicacionUserIT_ProductOwner = u.JefeEquipo_ExpertoAplicacionUserIT_ProductOwner,
                                             Experto_Especialista = u.Experto_Especialista,
                                             TotalEquiposRelacionados = s == null ? 0 : s.NroTecnologias,
                                             EstadoAplicacion = u.EstadoAplicacion,
                                             ApplicationId = u3.AppId
                                         }).OrderBy(pag.sortName + " " + pag.sortOrder);
                        var arr = registros.Count();

                        var final = (from a in registros
                                     where (pag.PCIS.Count == 0 || aplicacionPCI.Contains(a.ApplicationId))
                                     select a);

                        totalRows = registros.Count();

                        var pci = (from x in ctx.ApplicationPCI
                                   where x.FlagActivo == true
                                   select new AplicacionPCISTODTO() { TipoPCIId = x.TipoPCIId, ApplicationId = x.ApplicationId }).ToList();



                        var resultado = registros.Skip((pag.pageNumber - 1) * pag.pageSize).Take(pag.pageSize).ToList();

                        foreach (AplicacionDTO a in resultado)
                        {
                            var lista = "";
                            int flag = 0;
                            var pci2 = (from x in pci
                                        where x.ApplicationId == a.ApplicationId
                                        select x.TipoPCIId).ToList();
                            foreach (int i in pci2)
                            {



                                if (flag == (pci.Count()) - 1)
                                {
                                    lista = lista + GetPCIDSS(i);
                                }
                                else lista = lista + GetPCIDSS(i) + ",";

                                flag++;
                            }

                            a.ListaPCI = lista;
                        }



                        return resultado;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<AplicacionDTO> GetAplicacion(string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<AplicacionDTO> GetAplicacion(string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public string GetPCIDSS(int id)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.TipoPCI
                                   where u.FlagActivo == true && u.TipoPCIId == id

                                   select u.Nombre).FirstOrDefault();

                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<AplicacionDTO> GetAplicacion()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<AplicacionDTO> GetAplicacion()"
                    , new object[] { null });
            }
        }

        public override List<AplicacionDTO> GetAplicacion()
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.Aplicacion
                                   where u.FlagActivo
                                   orderby u.Nombre
                                   select new AplicacionDTO()
                                   {
                                       //Id = u.AplicacionId,
                                       CodigoAPT = u.CodigoAPT,
                                       Nombre = u.Nombre
                                   }).ToList();

                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<AplicacionDTO> GetAplicacion()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<AplicacionDTO> GetAplicacion()"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocomplete> GetAplicacionByFiltro(string filtro)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.Aplicacion
                                   where u.FlagActivo
                                   && (u.CodigoAPT + " - " + u.Nombre).ToUpper().Contains(filtro.ToUpper())
                                   orderby u.Nombre
                                   select new CustomAutocomplete()
                                   {
                                       Id = u.CodigoAPT,
                                       Descripcion = u.CodigoAPT + " - " + u.Nombre,
                                       value = u.CodigoAPT + " - " + u.Nombre
                                   }).ToList();

                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetAplicacionByFiltro(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetAplicacionByFiltro(string filtro)"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocomplete> GetNombreAplicacionByFiltro(string filtro)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.Aplicacion
                                   where u.FlagActivo
                                   && (u.CodigoAPT + " - " + u.Nombre).ToUpper().Contains(filtro.ToUpper())
                                   orderby u.Nombre
                                   select new CustomAutocomplete()
                                   {
                                       Id = u.Nombre,
                                       Descripcion = u.CodigoAPT + " - " + u.Nombre,
                                       value = u.CodigoAPT + " - " + u.Nombre
                                   }).ToList();

                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetAplicacionByFiltro(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetAplicacionByFiltro(string filtro)"
                    , new object[] { null });
            }
        }
        public override List<AplicacionExpertoDTO> GetAplicacionExperto(string Id)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var entidad = (from u in ctx.Aplicacion
                                       join b in ctx.AplicacionExpertos on u.CodigoAPT equals b.CodigoAPT
                                       join e in ctx.TipoExperto on b.TipoExpertoId equals e.TipoExpertoId
                                       where u.FlagActivo && b.FlagActivo && b.Matricula.Trim() != "NO APLICA"
                                       && b.CodigoAPT == Id
                                       select new AplicacionExpertoDTO()
                                       {
                                           AplicacionExpertoId = b.AplicacionExpertoId,
                                           CodApp = u.CodigoAPT,
                                           Matricula = b.Matricula,
                                           Nombres = b.Nombres,
                                           Activo = b.FlagActivo,
                                           TipoExpertoId = b.TipoExpertoId,
                                           TipoExpertoToString = e.Nombres,
                                           FechaCreacion = b.FechaCreacion,
                                           UsuarioCreacion = b.CreadoPor
                                       }).ToList();

                        return entidad;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<AplicacionExpertoDTO> GetAplicacionExperto(string Id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<AplicacionExpertoDTO> GetAplicacionExperto(string Id)"
                    , new object[] { null });
            }
        }
        #region Dashboard Aplicacion
        public override FiltrosDashboardAplicacion ListFiltros()
        {
            try
            {
                FiltrosDashboardAplicacion arr_data = null;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    arr_data = new FiltrosDashboardAplicacion();
                    var dataPrincipal = (from u in ctx.Aplicacion
                                         where u.FlagActivo
                                         select u);

                    arr_data.GestionadoPor = (from u in dataPrincipal
                                              where !string.IsNullOrEmpty(u.GestionadoPor)
                                              select u.GestionadoPor).Distinct().OrderBy(x => x).ToArray();

                    arr_data.EstadoAplicacion = (from u in dataPrincipal
                                                 where !string.IsNullOrEmpty(u.EstadoAplicacion)
                                                 select u.EstadoAplicacion).Distinct().OrderBy(x => x).ToArray();

                    arr_data.TipoActivo = (from u in dataPrincipal
                                           where !string.IsNullOrEmpty(u.TipoActivoInformacion)
                                           select u.TipoActivoInformacion).Distinct().OrderBy(x => x).ToArray();

                    arr_data.Gerencia = (from u in dataPrincipal
                                         where !string.IsNullOrEmpty(u.GerenciaCentral)
                                         select u.GerenciaCentral).Distinct().OrderBy(x => x).ToArray();
                    arr_data.ClasificacionTecnica = (from u in dataPrincipal
                                                     where !string.IsNullOrEmpty(u.ClasificacionTecnica)
                                                     select u.ClasificacionTecnica).Distinct().OrderBy(x => x).ToArray();

                    arr_data.SubclasificacionTecnica = (from u in dataPrincipal
                                                        where !string.IsNullOrEmpty(u.SubclasificacionTecnica)
                                                        select u.SubclasificacionTecnica).Distinct().OrderBy(x => x).ToArray();
                    //nuevos
                    arr_data.JefeEquipo = (from u in dataPrincipal
                                           where !string.IsNullOrEmpty(u.JefeEquipo_ExpertoAplicacionUserIT_ProductOwner)
                                           select u.JefeEquipo_ExpertoAplicacionUserIT_ProductOwner).Distinct().OrderBy(x => x).ToArray();

                    arr_data.LiderUsuario = (from u in dataPrincipal
                                             where !string.IsNullOrEmpty(u.Owner_LiderUsuario_ProductOwner)
                                             select u.Owner_LiderUsuario_ProductOwner).Distinct().OrderBy(x => x).ToArray();

                    arr_data.Division = (from u in dataPrincipal
                                         where !string.IsNullOrEmpty(u.Division)
                                         select u.Division).Distinct().OrderBy(x => x).ToArray();

                    arr_data.Experto = (from u in dataPrincipal
                                        where !string.IsNullOrEmpty(u.Experto_Especialista)
                                        select u.Experto_Especialista).Distinct().OrderBy(x => x).ToArray();

                    arr_data.Gestor = (from u in dataPrincipal
                                       where !string.IsNullOrEmpty(u.Gestor_UsuarioAutorizador_ProductOwner)
                                       select u.Gestor_UsuarioAutorizador_ProductOwner).Distinct().ToArray();

                    var gestor = new List<string>();
                    if (arr_data.Gestor != null)
                    {
                        foreach (string item in arr_data.Gestor)
                        {
                            string[] data = item.Split(';');
                            gestor.AddRange(data.Select(x => x.Trim()).ToArray());
                        }
                        arr_data.Gestor = gestor.Distinct().OrderBy(x => x).ToArray();
                    }

                    arr_data.TTL = (from u in dataPrincipal
                                    where !string.IsNullOrEmpty(u.TribeTechnicalLead)
                                    select u.TribeTechnicalLead).Distinct().OrderBy(x => x).ToArray();

                    arr_data.Area = (from u in dataPrincipal
                                     where !string.IsNullOrEmpty(u.Area)
                                     select u.Area).Distinct().OrderBy(x => x).ToArray();

                    arr_data.Unidad = (from u in dataPrincipal
                                       where !string.IsNullOrEmpty(u.Unidad)
                                       select u.Unidad).Distinct().OrderBy(x => x).ToArray();

                    arr_data.Broker = (from u in dataPrincipal
                                       where !string.IsNullOrEmpty(u.BrokerSistemas)
                                       select u.BrokerSistemas).Distinct().OrderBy(x => x).ToArray();
                    arr_data.UnidadFondeo = ServiceManager<AplicacionDAO>.Provider.GetListUnidadFondeo();
                }
                return arr_data;
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: FiltrosDashboardAplicacion ListFiltros()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: FiltrosDashboardAplicacion ListFiltros()"
                    , new object[] { null });
            }
        }

        public override FiltrosDashboardAplicacion ListFiltrosXGestionadoPor(List<string> gestionadoPor)
        {
            try
            {
                FiltrosDashboardAplicacion arr_data = null;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    arr_data = new FiltrosDashboardAplicacion();
                    if (gestionadoPor.Count > 0)
                    {
                        arr_data.UsuarioLider = (from u in ctx.Aplicacion
                                                 where u.FlagActivo
                                                 && gestionadoPor.Contains(u.GestionadoPor)
                                                 orderby u.Owner_LiderUsuario_ProductOwner
                                                 select u.Owner_LiderUsuario_ProductOwner).Distinct().ToArray();

                        arr_data.UsuarioAutorizador = (from u in ctx.Aplicacion
                                                       where u.FlagActivo
                                                       && gestionadoPor.Contains(u.GestionadoPor)
                                                       orderby u.Gestor_UsuarioAutorizador_ProductOwner
                                                       select u.Gestor_UsuarioAutorizador_ProductOwner).Distinct().ToArray();

                        arr_data.ExpertoEspecialista = (from u in ctx.Aplicacion
                                                        where u.FlagActivo
                                                        && gestionadoPor.Contains(u.GestionadoPor)
                                                        orderby u.Experto_Especialista
                                                        select u.Experto_Especialista).Distinct().ToArray();
                    }

                }
                return arr_data;
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: ListFiltrosXGestionadoPor()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: ListFiltrosXGestionadoPor()"
                    , new object[] { null });
            }
        }

        public override FiltrosDashboardAplicacion ListAplicacionesXFiltros(FiltrosDashboardAplicacion filtros)
        {
            try
            {
                FiltrosDashboardAplicacion arr_data = null;

                filtros.GestionadoPorFiltrar = filtros.GestionadoPorFiltrar == null ? new List<string>() : filtros.GestionadoPorFiltrar;
                filtros.UsuarioLiderFiltrar = filtros.UsuarioLiderFiltrar == null ? new List<string>() : filtros.UsuarioLiderFiltrar;
                filtros.UsuarioAutorizadorFiltrar = filtros.UsuarioAutorizadorFiltrar == null ? new List<string>() : filtros.UsuarioAutorizadorFiltrar;
                filtros.ExpertoEspecialistaFiltrar = filtros.ExpertoEspecialistaFiltrar == null ? new List<string>() : filtros.ExpertoEspecialistaFiltrar;

                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    arr_data = new FiltrosDashboardAplicacion();

                    var dataAplicaciones = (from u in ctx.Aplicacion
                                            where u.FlagActivo
                                            //&& u.GestionadoPor == filtros.GestionadoPorFiltrar
                                            && (filtros.GestionadoPorFiltrar.Contains(u.GestionadoPor) || filtros.GestionadoPorFiltrar.Count() == 0)
                                            && (filtros.UsuarioLiderFiltrar.Contains(u.Owner_LiderUsuario_ProductOwner) || filtros.UsuarioLiderFiltrar.Count() == 0)
                                            && (filtros.UsuarioAutorizadorFiltrar.Contains(u.Gestor_UsuarioAutorizador_ProductOwner) || filtros.UsuarioAutorizadorFiltrar.Count() == 0)
                                            && (filtros.ExpertoEspecialistaFiltrar.Contains(u.Experto_Especialista) || filtros.ExpertoEspecialistaFiltrar.Count() == 0)
                                            select new
                                            {
                                                u.CodigoAPT,
                                                u.EstadoAplicacion
                                            }).ToList();

                    arr_data.CodigoAPT = dataAplicaciones.Select(x => new CustomAutocomplete { Id = x.CodigoAPT, Descripcion = x.CodigoAPT, TipoDescripcion = x.EstadoAplicacion }).Distinct().ToArray();

                    arr_data.EstadoAplicacion = (from u in dataAplicaciones
                                                 orderby u.EstadoAplicacion
                                                 select u.EstadoAplicacion).Distinct().OrderBy(x => x).ToArray();

                }
                return arr_data;
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: ListAplicacionesXFiltros()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: ListAplicacionesXFiltros()"
                    , new object[] { null });
            }
        }

        public override FiltrosAplicacion GetFiltros()
        {
            try
            {
                FiltrosAplicacion arr_data = null;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    ctx.Database.CommandTimeout = 0;


                    arr_data = new FiltrosAplicacion();

                    var parametro = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("FILTRO_GERENCIA_CENTRAL_APLICACIONES");
                    arr_data.Filtro = parametro != null ? parametro.Valor : "";

                    arr_data.TipoExperto = ServiceManager<AplicacionDAO>.Provider.GetTipoExpertoByFiltro(null);
                    arr_data.TipoExpertoPortafolio = ServiceManager<AplicacionDAO>.Provider.GetTipoExpertoPortafolioByFiltro(null);
                    arr_data.UnidadFondeo = ServiceManager<AplicacionDAO>.Provider.GetListUnidadFondeo();
                    arr_data.Criticidad = (from u in ctx.Aplicacion
                                           join c in ctx.Criticidad on u.CriticidadId equals c.CriticidadId
                                           where u.FlagActivo && c.Activo
                                           group u by new { u.CriticidadId, c.DetalleCriticidad } into grp
                                           select new CustomAutocomplete()
                                           {
                                               Id = grp.Key.CriticidadId.ToString(),
                                               Descripcion = grp.Key.DetalleCriticidad
                                           }).OrderBy(x => x.Descripcion).ToList();

                    arr_data.Gerencia = (from u in ctx.Aplicacion
                                         where u.FlagActivo
                                         //&& (!string.IsNullOrEmpty(u.GerenciaCentral))
                                         group u by u.GerenciaCentral into grp
                                         select (grp.Key ?? "")).OrderBy(x => x).ToArray();
                    //arr_data.Gerencia = (from u in ctx.Gerencia
                    //                     where u.FlagActivo
                    //                     group u by u.Nombre into grp
                    //                     select grp.Key).OrderBy(x => x).ToArray();

                    arr_data.Division = (from u in ctx.Aplicacion
                                         where u.FlagActivo
                                         //&& (!string.IsNullOrEmpty(u.Division))
                                         group u by u.Division into grp
                                         select (grp.Key ?? "")).OrderBy(x => x).ToArray();


                    //arr_data.Division = (from u in ctx.Division
                    //                     where u.FlagActivo
                    //                     group u by u.Nombre into grp
                    //                     select grp.Key).OrderBy(x => x).ToArray();

                    arr_data.Unidad = (from u in ctx.Aplicacion
                                       where u.FlagActivo
                                       //&& (!string.IsNullOrEmpty(u.Unidad))
                                       group u by u.Unidad into grp
                                       select (grp.Key ?? "")).OrderBy(x => x).ToArray();

                    //arr_data.Unidad = (from u in ctx.Unidad
                    //                   where u.FlagActivo
                    //                   group u by u.Nombre into grp
                    //                   select grp.Key).OrderBy(x => x).ToArray();

                    arr_data.Area = (from u in ctx.Aplicacion
                                     where u.FlagActivo //&& u.Area != ""
                                     //&& (!string.IsNullOrEmpty(u.Area))
                                     group u by u.Area into grp
                                     select (grp.Key ?? "")).OrderBy(x => x).ToArray();

                    //arr_data.Area = (from u in ctx.Area
                    //                 where u.FlagActivo 
                    //                 group u by u.Nombre into grp
                    //                 select grp.Key).OrderBy(x => x).ToArray();

                    arr_data.Estado = (from u in ctx.Aplicacion
                                       where u.EstadoAplicacion != "Eliminada"
                                       group u by u.EstadoAplicacion into grp
                                       select (grp.Key ?? "")).OrderBy(x => x).ToArray();

                    //arr_data.Estado = (from u in ctx.Aplicacion
                    //                   where u.EstadoAplicacion != "Eliminada"
                    //                   group u by u.EstadoAplicacion into grp
                    //                   select grp.Key).OrderBy(x => x).ToArray();

                    //arr_data.Estado = (new DescriptionAttributes<ApplicationState>().Descriptions.ToList()).ToArray();


                    //var estado = ServiceManager<ParametricasDAO>.Provider.GetParametricasByTabla(Utilitarios.GetEnumDescription2(EEntidadPortafolio.EstadoAplicacion), (int)EEntidadParametrica.PORTAFOLIO);
                    //var estadoFiltro = ServiceManager<ParametroDAO>.Provider.ObtenerParametroApp(Utilitarios.ESTADOS_APLICACION);
                    //if (estadoFiltro != null)
                    //{
                    //    var lista = estadoFiltro.Valor.Split('|');
                    //    var listaRetorno = new List<string>();
                    //    foreach (var item in lista)
                    //    {
                    //        var filtro = estado.FirstOrDefault(x => x.Id == item);
                    //        if (filtro != null)
                    //        {
                    //            listaRetorno.Add(filtro.Descripcion);
                    //        }
                    //        arr_data.Estado = listaRetorno.ToArray();
                    //    }
                    //}
                    //else
                    //{
                    //    arr_data.Estado = ServiceManager<ParametricasDAO>.Provider.GetParametricasByTabla(Utilitarios.GetEnumDescription2(EEntidadPortafolio.EstadoAplicacion),
                    //            (int)EEntidadParametrica.PORTAFOLIO
                    //            ).Select(x => x.Descripcion).ToArray();
                    //}

                    //arr_data.EstadoAll = ServiceManager<ParametricasDAO>.Provider.GetParametricasByTabla(Utilitarios.GetEnumDescription2(EEntidadPortafolio.EstadoAplicacion),
                    //            (int)EEntidadParametrica.PORTAFOLIO
                    //            ).Select(x => x.Descripcion).ToArray();

                    arr_data.ClasificacionTecnica = (from u in ctx.Aplicacion
                                                     where u.EstadoAplicacion != "Eliminada"
                                                     group u by u.ClasificacionTecnica into grp
                                                     select grp.Key).OrderBy(x => x).ToArray();

                    arr_data.SubclasificacionTecnica = (from u in ctx.Aplicacion
                                                        where u.EstadoAplicacion != "Eliminada"
                                                        group u by u.SubclasificacionTecnica into grp
                                                        select grp.Key).OrderBy(x => x).ToArray();

                    arr_data.TipoActivoInformacion = (from u in ctx.Aplicacion
                                                      where u.EstadoAplicacion != "Eliminada"
                                                      group u by u.TipoActivoInformacion into grp
                                                      select grp.Key).OrderBy(x => x).ToArray();

                    arr_data.TipoPCI = (from u in ctx.TipoPCI
                                        where u.FlagActivo == true && !u.FlagEliminado.Value
                                        orderby u.Nombre
                                        select new CustomAutocomplete()
                                        {
                                            Id = u.TipoPCIId.ToString(),
                                            Descripcion = u.Nombre,
                                            value = u.TipoPCIId.ToString()
                                        }).ToList();

                    var lEstadoSolicitudApp = Utilitarios.EnumToList<EEstadoSolicitudAplicacion>();
                    arr_data.EstadoSolicitud = lEstadoSolicitudApp.Select(x => new CustomAutocompleteConsulta { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) }).ToList();

                    arr_data.CapaFuncional = ServiceManager<ParametricasDAO>.Provider.GetParametricasByTabla(Utilitarios.GetEnumDescription2(EEntidadPortafolio.CapaFuncional),
                    (int)EEntidadParametrica.PORTAFOLIO).Select(x => x.Descripcion).ToArray();


                }
                return arr_data;
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: FiltrosAplicacion GetFiltros()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: FiltrosAplicacion GetFiltros()"
                    , new object[] { null });
            }
        }

        public override FiltrosAplicacion GetFiltros2()
        {
            try
            {
                FiltrosAplicacion arr_data = null;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    ctx.Database.CommandTimeout = 0;


                    arr_data = new FiltrosAplicacion();

                    var parametro = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("FILTRO_GERENCIA_CENTRAL_APLICACIONES");
                    arr_data.Filtro = parametro != null ? parametro.Valor : "";

                    arr_data.TipoExperto = ServiceManager<AplicacionDAO>.Provider.GetTipoExpertoByFiltro(null);
                    arr_data.TipoExpertoPortafolio = ServiceManager<AplicacionDAO>.Provider.GetTipoExpertoPortafolioByFiltro(null);

                    arr_data.Criticidad = (from u in ctx.Aplicacion
                                           join c in ctx.Criticidad on u.CriticidadId equals c.CriticidadId
                                           where u.FlagActivo && c.Activo
                                           group u by new { u.CriticidadId, c.DetalleCriticidad } into grp
                                           select new CustomAutocomplete()
                                           {
                                               Id = grp.Key.CriticidadId.ToString(),
                                               Descripcion = grp.Key.DetalleCriticidad
                                           }).OrderBy(x => x.Descripcion).ToList();

                    //arr_data.Gerencia = (from u in ctx.Aplicacion
                    //                     where u.FlagActivo
                    //                     //&& (!string.IsNullOrEmpty(u.GerenciaCentral))
                    //                     group u by u.GerenciaCentral into grp
                    //                     select grp.Key).OrderBy(x => x).ToArray();
                    arr_data.Gerencia = (from u in ctx.Gerencia
                                         where u.FlagActivo
                                         group u by u.Nombre into grp
                                         select grp.Key).OrderBy(x => x).ToArray();

                    //arr_data.Division = (from u in ctx.Aplicacion
                    //                     where u.FlagActivo
                    //                     //&& (!string.IsNullOrEmpty(u.Division))
                    //                     group u by u.Division into grp
                    //                     select grp.Key).OrderBy(x => x).ToArray();


                    arr_data.Division = (from u in ctx.Division
                                         where u.FlagActivo
                                         group u by u.Nombre into grp
                                         select grp.Key).OrderBy(x => x).ToArray();

                    //arr_data.Unidad = (from u in ctx.Aplicacion
                    //                   where u.FlagActivo
                    //                   //&& (!string.IsNullOrEmpty(u.Unidad))
                    //                   group u by u.Unidad into grp
                    //                   select grp.Key).OrderBy(x => x).ToArray();

                    arr_data.Unidad = (from u in ctx.Unidad
                                       where u.FlagActivo
                                       group u by u.Nombre into grp
                                       select grp.Key).OrderBy(x => x).ToArray();

                    //arr_data.Area = (from u in ctx.Aplicacion
                    //                 where u.FlagActivo //&& u.Area != ""
                    //                 //&& (!string.IsNullOrEmpty(u.Area))
                    //                 group u by u.Area into grp
                    //                 select grp.Key).OrderBy(x => x).ToArray();

                    arr_data.Area = (from u in ctx.Area
                                     where u.FlagActivo
                                     group u by u.Nombre into grp
                                     select grp.Key).OrderBy(x => x).ToArray();

                    //arr_data.Estado = (from u in ctx.Aplicacion
                    //                    where u.EstadoAplicacion!="Eliminada"
                    //                   group u by u.EstadoAplicacion into grp
                    //                   select grp.Key).OrderBy(x => x).ToArray();

                    //arr_data.Estado = (from u in ctx.Aplicacion
                    //                   where u.EstadoAplicacion != "Eliminada"
                    //                   group u by u.EstadoAplicacion into grp
                    //                   select grp.Key).OrderBy(x => x).ToArray();

                    arr_data.Estado = (new DescriptionAttributes<ApplicationState>().Descriptions.ToList()).ToArray();


                    //var estado = ServiceManager<ParametricasDAO>.Provider.GetParametricasByTabla(Utilitarios.GetEnumDescription2(EEntidadPortafolio.EstadoAplicacion), (int)EEntidadParametrica.PORTAFOLIO);
                    //var estadoFiltro = ServiceManager<ParametroDAO>.Provider.ObtenerParametroApp(Utilitarios.ESTADOS_APLICACION);
                    //if (estadoFiltro != null)
                    //{
                    //    var lista = estadoFiltro.Valor.Split('|');
                    //    var listaRetorno = new List<string>();
                    //    foreach (var item in lista)
                    //    {
                    //        var filtro = estado.FirstOrDefault(x => x.Id == item);
                    //        if (filtro != null)
                    //        {
                    //            listaRetorno.Add(filtro.Descripcion);
                    //        }
                    //        arr_data.Estado = listaRetorno.ToArray();
                    //    }
                    //}
                    //else
                    //{
                    //    arr_data.Estado = ServiceManager<ParametricasDAO>.Provider.GetParametricasByTabla(Utilitarios.GetEnumDescription2(EEntidadPortafolio.EstadoAplicacion),
                    //            (int)EEntidadParametrica.PORTAFOLIO
                    //            ).Select(x => x.Descripcion).ToArray();
                    //}

                    arr_data.EstadoAll = ServiceManager<ParametricasDAO>.Provider.GetParametricasByTabla(Utilitarios.GetEnumDescription2(EEntidadPortafolio.EstadoAplicacion),
                                (int)EEntidadParametrica.PORTAFOLIO
                                ).Select(x => x.Descripcion).ToArray();

                    arr_data.ClasificacionTecnica = (from u in ctx.ClasificacionTecnica
                                                     where u.FlagActivo //&& u.Area != ""
                                                                        //&& (!string.IsNullOrEmpty(u.Area))
                                                     group u by u.Nombre into grp
                                                     select grp.Key).OrderBy(x => x).ToArray();

                    arr_data.SubclasificacionTecnica = (from u in ctx.SubClasificacionTecnica
                                                        where u.FlagActivo //&& u.Area != ""
                                                                           //&& (!string.IsNullOrEmpty(u.Area))
                                                        group u by u.Nombre into grp
                                                        select grp.Key).OrderBy(x => x).ToArray();

                    arr_data.TipoActivoInformacion = (from u in ctx.TipoActivoInformacion
                                                      where u.FlagActivo
                                                      group u by u.Nombre into grp
                                                      select grp.Key).OrderBy(x => x).ToArray();

                    var lEstadoSolicitudApp = Utilitarios.EnumToList<EEstadoSolicitudAplicacion>();
                    arr_data.EstadoSolicitud = lEstadoSolicitudApp.Select(x => new CustomAutocompleteConsulta { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) }).ToList();
                }
                return arr_data;
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: FiltrosAplicacion GetFiltros()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: FiltrosAplicacion GetFiltros()"
                    , new object[] { null });
            }
        }

        public override FiltrosReporteAplicacion GetFiltrosReporteAplicacion()
        {
            try
            {
                FiltrosReporteAplicacion arr_data = null;

                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    arr_data = new FiltrosReporteAplicacion();

                    arr_data.EstadoAplicacion = (from u in ctx.Aplicacion
                                                 orderby u.EstadoAplicacion
                                                 where !string.IsNullOrEmpty(u.EstadoAplicacion)
                                                 select u.EstadoAplicacion).Distinct().OrderBy(x => x).ToArray();

                }
                return arr_data;
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: GetFiltrosReporteAplicacion()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: GetFiltrosReporteAplicacion()"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocompleteAplicacion> GetAplicacionByFiltro(string filtro, bool flagRelacionar)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.Aplicacion
                                   where u.FlagActivo && u.FlagRelacionar.Value == flagRelacionar
                                   && (u.CodigoAPT + " - " + u.Nombre).ToUpper().Contains(filtro.ToUpper())
                                   orderby u.Nombre
                                   select new CustomAutocompleteAplicacion()
                                   {
                                       Id = u.CodigoAPT,
                                       Descripcion = u.CodigoAPT + " - " + u.Nombre,
                                       value = u.CodigoAPT + " - " + u.Nombre,
                                       Nombre = u.Nombre,
                                       CategoriaTecnologica = u.CategoriaTecnologica,
                                       UsuarioLider = u.Owner_LiderUsuario_ProductOwner,
                                       TipoActivo = u.TipoActivoInformacion,
                                       EstadoAplicacion = u.EstadoAplicacion,
                                       IdAplicacion = u.AplicacionId
                                   }).ToList();

                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetAplicacionByFiltro(string filtro, bool flagRelacionar)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetAplicacionByFiltro(string filtro, bool flagRelacionar)"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocomplete> GetJefeEquipoByFiltro(string filtro)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.Aplicacion
                                   where u.FlagActivo
                                   && (string.IsNullOrEmpty(filtro) || u.JefeEquipo_ExpertoAplicacionUserIT_ProductOwner.ToUpper().Contains(filtro.ToUpper()))
                                   orderby u.JefeEquipo_ExpertoAplicacionUserIT_ProductOwner
                                   group u by u.JefeEquipo_ExpertoAplicacionUserIT_ProductOwner into grp
                                   select new CustomAutocomplete()
                                   {
                                       Id = grp.Key,
                                       Descripcion = grp.Key,
                                       value = grp.Key
                                   }).ToList();

                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetJefeEquipoByFiltro(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetJefeEquipoByFiltro(string filtro)"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocomplete> GetOwnerByFiltro(string filtro)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.Aplicacion
                                   where u.FlagActivo
                                   && (string.IsNullOrEmpty(filtro) || u.Owner_LiderUsuario_ProductOwner.ToUpper().Contains(filtro.ToUpper()))
                                   orderby u.Owner_LiderUsuario_ProductOwner
                                   group u by u.Owner_LiderUsuario_ProductOwner into grp
                                   select new CustomAutocomplete()
                                   {
                                       Id = grp.Key,
                                       Descripcion = grp.Key,
                                       value = grp.Key
                                   }).ToList();

                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetOwnerByFiltro(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetOwnerByFiltro(string filtro)"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocomplete> GetExpertoByFiltro(string filtro)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.Aplicacion
                                   where u.FlagActivo
                                   && (string.IsNullOrEmpty(filtro) || u.Experto_Especialista.ToUpper().Contains(filtro.ToUpper()))
                                   orderby u.Experto_Especialista
                                   group u by u.Experto_Especialista into grp
                                   select new CustomAutocomplete()
                                   {
                                       Id = grp.Key,
                                       Descripcion = grp.Key,
                                       value = grp.Key
                                   }).ToList();

                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetExpertoByFiltro(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetExpertoByFiltro(string filtro)"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocomplete> GetGerenciaByFiltro(string filtro)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.Aplicacion
                                   where u.FlagActivo
                                   && (string.IsNullOrEmpty(filtro) || u.GerenciaCentral.ToUpper().Contains(filtro.ToUpper()))
                                   orderby u.GerenciaCentral
                                   group u by u.GerenciaCentral into grp
                                   select new CustomAutocomplete()
                                   {
                                       Id = grp.Key,
                                       Descripcion = grp.Key,
                                       value = grp.Key
                                   }).ToList();

                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetGerenciaByFiltro(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetGerenciaByFiltro(string filtro)"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocomplete> GetDivisionByFiltro(string filtro)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.Aplicacion
                                   where u.FlagActivo
                                   && (string.IsNullOrEmpty(filtro) || u.Division.ToUpper().Contains(filtro.ToUpper()))
                                   orderby u.Division
                                   group u by u.Division into grp
                                   select new CustomAutocomplete()
                                   {
                                       Id = grp.Key,
                                       Descripcion = grp.Key,
                                       value = grp.Key
                                   }).ToList();

                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetDivisionByFiltro(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetDivisionByFiltro(string filtro)"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocomplete> GetGestionadoByFiltro(string filtro)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.Aplicacion
                                   where u.FlagActivo
                                   && (string.IsNullOrEmpty(filtro) || u.GestionadoPor.ToUpper().Contains(filtro.ToUpper()))
                                   orderby u.GestionadoPor
                                   group u by u.GestionadoPor into grp
                                   select new CustomAutocomplete()
                                   {
                                       Id = grp.Key,
                                       Descripcion = grp.Key,
                                       value = grp.Key
                                   }).ToList();

                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetGestionadoByFiltro(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetGestionadoByFiltro(string filtro)"
                    , new object[] { null });
            }
        }

        public override List<AplicacionDTO> GetPublicacionAplicacion3(PaginacionReporteAplicacion pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                var data = new List<AplicacionDTO>();


                data = ReportePublicacionAplicacion3(pag);



                if (data != null && data.Count() > 0) totalRows = Convert.ToInt16(data.Count());

                return data;
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetTecnologias()"
                    , new object[] { null });
            }
        }

        public List<AplicacionDTO> ReportePublicacionAplicacion3(PaginacionReporteAplicacion pag)
        {
            DataSet resultado = null;
            var cadenaConexion = Constantes.CadenaConexion;


            List<AplicacionDTO> lista = new List<AplicacionDTO>();

            using (SqlConnection cnx = new SqlConnection(cadenaConexion))
            {
                cnx.Open();
                using (var comando = new SqlCommand("[CVT].[USP_Reporte_Publicacion_Catalogo_Carga_Masiva]", cnx))
                {
                    comando.CommandTimeout = 0;
                    comando.CommandType = System.Data.CommandType.StoredProcedure;
                    comando.Parameters.Add(new SqlParameter("@gerencia", pag.Gerencia));
                    comando.Parameters.Add(new SqlParameter("@division", pag.Division));
                    comando.Parameters.Add(new SqlParameter("@area", pag.Area));
                    comando.Parameters.Add(new SqlParameter("@unidad", pag.Unidad));
                    comando.Parameters.Add(new SqlParameter("@estado", pag.Estado));
                    comando.Parameters.Add(new SqlParameter("@clasificacionTecnica", pag.ClasificacionTecnica));
                    comando.Parameters.Add(new SqlParameter("@subclasificacionTecnica", pag.SubclasificacionTecnica));
                    comando.Parameters.Add(new SqlParameter("@aplicacion", string.IsNullOrWhiteSpace(pag.Aplicacion) ? string.Empty : pag.Aplicacion));
                    comando.Parameters.Add(new SqlParameter("@Columnas", ""));
                    comando.Parameters.Add(new SqlParameter("@TablaProcedencia", pag.Procedencia));
                    comando.Parameters.Add(new SqlParameter("@PageSize", pag.pageSize));
                    comando.Parameters.Add(new SqlParameter("@PageNumber", pag.pageNumber));
                    comando.Parameters.Add(new SqlParameter("@tipoActivo", pag.TipoActivo));
                    comando.Parameters.Add(new SqlParameter("@exportar", pag.Exportar));
                    comando.Parameters.Add(new SqlParameter("@EstadoReactivacion", -1));


                    var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                    while (reader.Read())
                    {
                        lista.Add(new AplicacionDTO()
                        {
                            CodigoAPT = reader.GetString(reader.GetOrdinal("applicationId")),
                            Nombre = reader.GetString(reader.GetOrdinal("applicationName")),
                            interfaceId = reader.GetString(reader.GetOrdinal("interfaceId")),
                            DescripcionAplicacion = reader.GetString(reader.GetOrdinal("description")),
                            EstadoAplicacion = reader.IsDBNull(reader.GetOrdinal("status")) ? "" : reader.GetString(reader.GetOrdinal("status")),
                            implementationType = reader.GetString(reader.GetOrdinal("implementationType")),
                            assetType = reader.GetString(reader.GetOrdinal("assetType")),
                            Gerencia = reader.GetString(reader.GetOrdinal("centralManagement")),
                            Division = reader.GetString(reader.GetOrdinal("Division")),
                            area = reader.GetString(reader.GetOrdinal("area")),
                            unit = reader.GetString(reader.GetOrdinal("unit")),
                            Lider_Usuario = reader.GetString(reader.GetOrdinal("Lider_Usuario")),
                            Usuario_Autorizador_Gestor = reader.GetString(reader.GetOrdinal("Usuario_Autorizador_Gestor")),
                            managed = reader.GetString(reader.GetOrdinal("managed")),
                            teamName = reader.IsDBNull(reader.GetOrdinal("teamName")) ? "" : reader.GetString(reader.GetOrdinal("teamName")),
                            Tribe_Lead = reader.GetString(reader.GetOrdinal("Tribe_Lead")),
                            Tribe_Technical_Lead = reader.GetString(reader.GetOrdinal("Tribe_Technical_Lead")),
                            Jefe_Equipo = reader.GetString(reader.GetOrdinal("Jefe_Equipo")),
                            Broker_Sistemas = reader.GetString(reader.GetOrdinal("Broker_Sistemas")),
                            Experto_Lider_tecnico = reader.GetString(reader.GetOrdinal("Experto_Lider_tecnico")),
                            BIANarea = reader.GetString(reader.GetOrdinal("BIANarea")),
                            BIANdomain = reader.GetString(reader.GetOrdinal("BIANdomain")),
                            tobe = reader.GetString(reader.GetOrdinal("tobe")),
                            mainOffice = reader.GetString(reader.GetOrdinal("mainOffice")),
                            architect = reader.GetString(reader.GetOrdinal("architectId")),
                            userEntity = reader.GetString(reader.GetOrdinal("userEntity")),
                            applicationCriticalityBIA = reader.GetString(reader.GetOrdinal("applicationCriticalityBIA")),
                            classification = reader.GetString(reader.GetOrdinal("classification")),
                            finalCriticality = reader.GetString(reader.GetOrdinal("finalCriticality")),
                            starProduct = reader.GetString(reader.GetOrdinal("starProduct")),
                            shorterApplicationResponseTime = reader.GetString(reader.GetOrdinal("shorterApplicationResponseTime")),
                            highestDegreeInterruption = reader.GetString(reader.GetOrdinal("highestDegreeInterruption")),
                            tierPreProduction = reader.GetString(reader.GetOrdinal("tierPreProduction")),
                            tierProduction = reader.GetString(reader.GetOrdinal("tierProduction")),
                            deploymentType = reader.GetString(reader.GetOrdinal("deploymentType")),
                            technologyCategory = reader.GetString(reader.GetOrdinal("technologyCategory")),
                            webDomain = reader.GetString(reader.GetOrdinal("webDomain")),
                            technicalClassification = reader.GetString(reader.GetOrdinal("technicalClassification")),
                            technicalSubclassification = reader.GetString(reader.GetOrdinal("technicalSubclassification")),
                            developmentType = reader.GetString(reader.GetOrdinal("developmentType")),
                            developmentProvider = reader.GetString(reader.GetOrdinal("developmentProvider")),
                            infrastructure = reader.GetString(reader.GetOrdinal("infrastructure")),

                            authenticationMethod = reader.GetString(reader.GetOrdinal("authenticationMethod")),
                            authorizationMethod = reader.GetString(reader.GetOrdinal("authorizationMethod")),
                            groupTicketRemedy = reader.GetString(reader.GetOrdinal("groupTicketRemedy")),
                            summaryStandard = reader.GetString(reader.GetOrdinal("summaryStandard")),
                            complianceLevel = reader.IsDBNull(reader.GetOrdinal("complianceLevel")) ? "" : reader.GetString(reader.GetOrdinal("complianceLevel")),
                            isFormalApplication = reader.IsDBNull(reader.GetOrdinal("isFormalApplication")) ? "" : reader.GetString(reader.GetOrdinal("isFormalApplication")),
                            regularizationDate = reader.IsDBNull(reader.GetOrdinal("regularizationDate")) ? "" : reader.GetString(reader.GetOrdinal("regularizationDate")),
                            parentAPTCode = reader.GetString(reader.GetOrdinal("parentAPTCode")),
                            replacementApplication = reader.GetString(reader.GetOrdinal("replacementApplication")),
                            solicitante = reader.GetString(reader.GetOrdinal("solicitante")),
                            registerDate = reader.IsDBNull(reader.GetOrdinal("registerDate")) ? "" : reader.GetString(reader.GetOrdinal("registerDate")),
                            registrationSituation = reader.IsDBNull(reader.GetOrdinal("registrationSituation")) ? "" : reader.GetString(reader.GetOrdinal("registrationSituation")),
                            dateFirstRelease = reader.IsDBNull(reader.GetOrdinal("dateFirstRelease")) ? "" : reader.GetString(reader.GetOrdinal("dateFirstRelease")),
                            ListaPCI = reader.IsDBNull(reader.GetOrdinal("TipoPCI")) ? "" : reader.GetString(reader.GetOrdinal("TipoPCI"))
                            //FechaEliminacion = reader.IsDBNull(reader.GetOrdinal("fechaeliminacion")) ? "" : reader.GetString(reader.GetOrdinal("fechaeliminacion")),
                            //TipoEliminacion = reader.IsDBNull(reader.GetOrdinal("TipoEliminacion")) ? "" : reader.GetString(reader.GetOrdinal("TipoEliminacion")),
                            //UsuarioEliminacion = reader.IsDBNull(reader.GetOrdinal("UsuarioEliminacion")) ? "" : reader.GetString(reader.GetOrdinal("UsuarioEliminacion"))

                        });
                    }

                }

                cnx.Close();

                return lista;
            }
        }
        public override List<AplicacionDTO> GetPublicacionAplicacion4(PaginacionReporteAplicacion pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                var data = new List<AplicacionDTO>();
                data = ReportePublicacionAplicacion4(pag);

                if (data != null && data.Count() > 0) totalRows = Convert.ToInt16(data.Count());

                return data;
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetTecnologias()"
                    , new object[] { null });
            }
        }

        public List<AplicacionDTO> ReportePublicacionAplicacion4(PaginacionReporteAplicacion pag)
        {
            var cadenaConexion = Constantes.CadenaConexion;
            List<AplicacionDTO> lista = new List<AplicacionDTO>();

            using (SqlConnection cnx = new SqlConnection(cadenaConexion))
            {
                cnx.Open();
                using (var comando = new SqlCommand("[CVT].[USP_Reporte_Publicacion_Catalogo_3]", cnx))
                {
                    comando.CommandTimeout = 0;
                    comando.CommandType = System.Data.CommandType.StoredProcedure;
                    comando.Parameters.Add(new SqlParameter("@gerencia", pag.Gerencia));
                    comando.Parameters.Add(new SqlParameter("@division", pag.Division));
                    comando.Parameters.Add(new SqlParameter("@area", pag.Area));
                    comando.Parameters.Add(new SqlParameter("@unidad", pag.Unidad));
                    comando.Parameters.Add(new SqlParameter("@estado", pag.Estado));
                    comando.Parameters.Add(new SqlParameter("@clasificacionTecnica", pag.ClasificacionTecnica));
                    comando.Parameters.Add(new SqlParameter("@subclasificacionTecnica", pag.SubclasificacionTecnica));
                    comando.Parameters.Add(new SqlParameter("@aplicacion", string.IsNullOrWhiteSpace(pag.Aplicacion) ? string.Empty : pag.Aplicacion));
                    comando.Parameters.Add(new SqlParameter("@TipoPCI", pag.TipoPCI));
                    comando.Parameters.Add(new SqlParameter("@GestionadoPor", pag.GestionadoPor));
                    comando.Parameters.Add(new SqlParameter("@LiderUsuario", pag.LiderUsuario));
                    comando.Parameters.Add(new SqlParameter("@Columnas", ""));
                    comando.Parameters.Add(new SqlParameter("@TablaProcedencia", pag.Procedencia));
                    comando.Parameters.Add(new SqlParameter("@PageSize", pag.pageSize));
                    comando.Parameters.Add(new SqlParameter("@PageNumber", pag.pageNumber));
                    comando.Parameters.Add(new SqlParameter("@tipoActivo", pag.TipoActivo));
                    comando.Parameters.Add(new SqlParameter("@exportar", pag.Exportar));
                    comando.Parameters.Add(new SqlParameter("@EstadoReactivacion", -1));

                    var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                    while (reader.Read())
                    {
                        lista.Add(new AplicacionDTO()
                        {
                            CodigoAPT = reader.IsDBNull(reader.GetOrdinal("applicationId")) ? string.Empty : reader.GetString(reader.GetOrdinal("applicationId")),
                            Nombre = reader.IsDBNull(reader.GetOrdinal("applicationName")) ? string.Empty : reader.GetString(reader.GetOrdinal("applicationName")),
                            interfaceId = reader.IsDBNull(reader.GetOrdinal("interfaceId")) ? string.Empty : reader.GetString(reader.GetOrdinal("interfaceId")),
                            DescripcionAplicacion = reader.IsDBNull(reader.GetOrdinal("description")) ? string.Empty : reader.GetString(reader.GetOrdinal("description")),
                            EstadoAplicacion = reader.IsDBNull(reader.GetOrdinal("status")) ? string.Empty : reader.GetString(reader.GetOrdinal("status")),
                            implementationType = reader.IsDBNull(reader.GetOrdinal("implementationType")) ? string.Empty : reader.GetString(reader.GetOrdinal("implementationType")),
                            assetType = reader.IsDBNull(reader.GetOrdinal("assetType")) ? string.Empty : reader.GetString(reader.GetOrdinal("assetType")),
                            Gerencia = reader.IsDBNull(reader.GetOrdinal("centralManagement")) ? string.Empty : reader.GetString(reader.GetOrdinal("centralManagement")),
                            Division = reader.IsDBNull(reader.GetOrdinal("Division")) ? string.Empty : reader.GetString(reader.GetOrdinal("Division")),
                            area = reader.IsDBNull(reader.GetOrdinal("area")) ? string.Empty : reader.GetString(reader.GetOrdinal("area")),
                            unit = reader.IsDBNull(reader.GetOrdinal("unit")) ? string.Empty : reader.GetString(reader.GetOrdinal("unit")),
                            Lider_Usuario = reader.IsDBNull(reader.GetOrdinal("Lider_Usuario")) ? string.Empty : reader.GetString(reader.GetOrdinal("Lider_Usuario")),
                            Usuario_Autorizador_Gestor = reader.IsDBNull(reader.GetOrdinal("Usuario_Autorizador_Gestor")) ? string.Empty : reader.GetString(reader.GetOrdinal("Usuario_Autorizador_Gestor")),
                            managed = reader.IsDBNull(reader.GetOrdinal("managed")) ? string.Empty : reader.GetString(reader.GetOrdinal("managed")),
                            teamName = reader.IsDBNull(reader.GetOrdinal("teamName")) ? string.Empty : reader.GetString(reader.GetOrdinal("teamName")),
                            Tribe_Lead = reader.IsDBNull(reader.GetOrdinal("Tribe_Lead")) ? string.Empty : reader.GetString(reader.GetOrdinal("Tribe_Lead")),
                            Tribe_Technical_Lead = reader.IsDBNull(reader.GetOrdinal("Tribe_Technical_Lead")) ? string.Empty : reader.GetString(reader.GetOrdinal("Tribe_Technical_Lead")),
                            Jefe_Equipo = reader.IsDBNull(reader.GetOrdinal("Jefe_Equipo")) ? string.Empty : reader.GetString(reader.GetOrdinal("Jefe_Equipo")),
                            Broker_Sistemas = reader.IsDBNull(reader.GetOrdinal("Broker_Sistemas")) ? string.Empty : reader.GetString(reader.GetOrdinal("Broker_Sistemas")),
                            Experto_Lider_tecnico = reader.IsDBNull(reader.GetOrdinal("Experto_Lider_tecnico")) ? string.Empty : reader.GetString(reader.GetOrdinal("Experto_Lider_tecnico")),
                            BIANarea = reader.IsDBNull(reader.GetOrdinal("BIANarea")) ? string.Empty : reader.GetString(reader.GetOrdinal("BIANarea")),
                            BIANdomain = reader.IsDBNull(reader.GetOrdinal("BIANdomain")) ? string.Empty : reader.GetString(reader.GetOrdinal("BIANdomain")),
                            tobe = reader.IsDBNull(reader.GetOrdinal("tobe")) ? string.Empty : reader.GetString(reader.GetOrdinal("tobe")),
                            mainOffice = reader.IsDBNull(reader.GetOrdinal("mainOffice")) ? string.Empty : reader.GetString(reader.GetOrdinal("mainOffice")),
                            architect = reader.IsDBNull(reader.GetOrdinal("architectId")) ? string.Empty : reader.GetString(reader.GetOrdinal("architectId")),
                            userEntity = reader.IsDBNull(reader.GetOrdinal("userEntity")) ? string.Empty : reader.GetString(reader.GetOrdinal("userEntity")),
                            applicationCriticalityBIA = reader.IsDBNull(reader.GetOrdinal("applicationCriticalityBIA")) ? string.Empty : reader.GetString(reader.GetOrdinal("applicationCriticalityBIA")),
                            classification = reader.IsDBNull(reader.GetOrdinal("classification")) ? string.Empty : reader.GetString(reader.GetOrdinal("classification")),
                            finalCriticality = reader.IsDBNull(reader.GetOrdinal("finalCriticality")) ? string.Empty : reader.GetString(reader.GetOrdinal("finalCriticality")),
                            starProduct = reader.IsDBNull(reader.GetOrdinal("starProduct")) ? string.Empty : reader.GetString(reader.GetOrdinal("starProduct")),
                            shorterApplicationResponseTime = reader.IsDBNull(reader.GetOrdinal("shorterApplicationResponseTime")) ? string.Empty : reader.GetString(reader.GetOrdinal("shorterApplicationResponseTime")),
                            highestDegreeInterruption = reader.IsDBNull(reader.GetOrdinal("highestDegreeInterruption")) ? string.Empty : reader.GetString(reader.GetOrdinal("highestDegreeInterruption")),
                            tierPreProduction = reader.IsDBNull(reader.GetOrdinal("tierPreProduction")) ? string.Empty : reader.GetString(reader.GetOrdinal("tierPreProduction")),
                            tierProduction = reader.IsDBNull(reader.GetOrdinal("tierProduction")) ? string.Empty : reader.GetString(reader.GetOrdinal("tierProduction")),
                            deploymentType = reader.IsDBNull(reader.GetOrdinal("deploymentType")) ? string.Empty : reader.GetString(reader.GetOrdinal("deploymentType")),
                            technologyCategory = reader.IsDBNull(reader.GetOrdinal("technologyCategory")) ? string.Empty : reader.GetString(reader.GetOrdinal("technologyCategory")),
                            webDomain = reader.IsDBNull(reader.GetOrdinal("webDomain")) ? string.Empty : reader.GetString(reader.GetOrdinal("webDomain")),
                            technicalClassification = reader.IsDBNull(reader.GetOrdinal("technicalClassification")) ? string.Empty : reader.GetString(reader.GetOrdinal("technicalClassification")),
                            technicalSubclassification = reader.IsDBNull(reader.GetOrdinal("technicalSubclassification")) ? string.Empty : reader.GetString(reader.GetOrdinal("technicalSubclassification")),
                            developmentType = reader.IsDBNull(reader.GetOrdinal("developmentType")) ? string.Empty : reader.GetString(reader.GetOrdinal("developmentType")),
                            developmentProvider = reader.IsDBNull(reader.GetOrdinal("developmentProvider")) ? string.Empty : reader.GetString(reader.GetOrdinal("developmentProvider")),
                            infrastructure = reader.IsDBNull(reader.GetOrdinal("infrastructure")) ? string.Empty : reader.GetString(reader.GetOrdinal("infrastructure")),
                            authenticationMethod = reader.IsDBNull(reader.GetOrdinal("authenticationMethod")) ? string.Empty : reader.GetString(reader.GetOrdinal("authenticationMethod")),
                            authorizationMethod = reader.IsDBNull(reader.GetOrdinal("authorizationMethod")) ? string.Empty : reader.GetString(reader.GetOrdinal("authorizationMethod")),
                            groupTicketRemedy = reader.IsDBNull(reader.GetOrdinal("groupTicketRemedy")) ? string.Empty : reader.GetString(reader.GetOrdinal("groupTicketRemedy")),
                            summaryStandard = reader.IsDBNull(reader.GetOrdinal("summaryStandard")) ? string.Empty : reader.GetString(reader.GetOrdinal("summaryStandard")),
                            complianceLevel = reader.IsDBNull(reader.GetOrdinal("complianceLevel")) ? string.Empty : reader.GetString(reader.GetOrdinal("complianceLevel")),
                            isFormalApplication = reader.IsDBNull(reader.GetOrdinal("isFormalApplication")) ? string.Empty : reader.GetString(reader.GetOrdinal("isFormalApplication")),
                            regularizationDate = reader.IsDBNull(reader.GetOrdinal("regularizationDate")) ? string.Empty : reader.GetString(reader.GetOrdinal("regularizationDate")),
                            parentAPTCode = reader.IsDBNull(reader.GetOrdinal("parentAPTCode")) ? string.Empty : reader.GetString(reader.GetOrdinal("parentAPTCode")),
                            replacementApplication = reader.IsDBNull(reader.GetOrdinal("replacementApplication")) ? string.Empty : reader.GetString(reader.GetOrdinal("replacementApplication")),
                            solicitante = reader.IsDBNull(reader.GetOrdinal("solicitante")) ? string.Empty : reader.GetString(reader.GetOrdinal("solicitante")),
                            registerDate = reader.IsDBNull(reader.GetOrdinal("registerDate")) ? string.Empty : reader.GetString(reader.GetOrdinal("registerDate")),
                            registrationSituation = reader.IsDBNull(reader.GetOrdinal("registrationSituation")) ? string.Empty : reader.GetString(reader.GetOrdinal("registrationSituation")),
                            dateFirstRelease = reader.IsDBNull(reader.GetOrdinal("dateFirstRelease")) ? string.Empty : reader.GetString(reader.GetOrdinal("dateFirstRelease")),
                            FechaEliminacion = reader.IsDBNull(reader.GetOrdinal("fechaeliminacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("fechaeliminacion")),
                            TipoEliminacion = reader.IsDBNull(reader.GetOrdinal("TipoEliminacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoEliminacion")),
                            UsuarioEliminacion = reader.IsDBNull(reader.GetOrdinal("UsuarioEliminacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("UsuarioEliminacion")),
                            FechaReactivacion = reader.IsDBNull(reader.GetOrdinal("FechaReactivacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("FechaReactivacion")),
                            EstadoReactivacion = reader.IsDBNull(reader.GetOrdinal("EstadoReactivacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("EstadoReactivacion")),
                            NombreSolicitanteReactivacion = reader.IsDBNull(reader.GetOrdinal("nombresolicitante")) ? string.Empty : reader.GetString(reader.GetOrdinal("nombresolicitante")),
                            ListaPCI = reader.IsDBNull(reader.GetOrdinal("TipoPCI")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoPCI")),
                            architectSolution = reader.IsDBNull(reader.GetOrdinal("architectSolution")) ? string.Empty : reader.GetString(reader.GetOrdinal("architectSolution")),
                            functionalLayer = reader.IsDBNull(reader.GetOrdinal("functionalLayer")) ? string.Empty : reader.GetString(reader.GetOrdinal("functionalLayer")),
                            operatingHours = reader.IsDBNull(reader.GetOrdinal("operatingHours")) ? string.Empty : reader.GetString(reader.GetOrdinal("operatingHours")),
                            FlagComponenteCross = reader.IsDBNull(reader.GetOrdinal("FlagComponenteCross")) ? string.Empty : reader.GetString(reader.GetOrdinal("FlagComponenteCross")),
                            FechaAprobacionRegistroCompleto = reader.IsDBNull(reader.GetOrdinal("FechaAprobacionRegistroCompleto")) ? string.Empty : reader.GetString(reader.GetOrdinal("FechaAprobacionRegistroCompleto"))
                        });
                    }
                }

                return lista;
            }
        }


        public override DashboardAplicacionData GetReporte(FiltrosDashboardAplicacion filtros)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var rpta = new DashboardAplicacionData();

                #region Tecnologias obsoletas de las aplicaciones

                List<DashboardBase> dataGraficoBarras = new List<DashboardBase>();

                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("CVT.USP_Dashboard_Aplicaciones_Tecnologias_Obsoletas", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@strAplicaciones", string.Join("|", filtros.CodigoAPTFiltrar)));
                        //comando.Parameters.Add(new SqlParameter("@fechaDia", DateTime.Now));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            dataGraficoBarras.Add(new DashboardBase()
                            {
                                Descripcion = reader.IsDBNull(reader.GetOrdinal("Grupo")) ? "" : reader.GetString(reader.GetOrdinal("Grupo")),
                                Valor = reader.IsDBNull(reader.GetOrdinal("NroAplicaciones")) ? 0 : reader.GetInt32(reader.GetOrdinal("NroAplicaciones")),
                                Color = reader.IsDBNull(reader.GetOrdinal("Color")) ? "" : reader.GetString(reader.GetOrdinal("Color"))
                            });
                        }
                        reader.Close();
                    }

                    rpta.DataBarras = dataGraficoBarras;




                }

                #endregion

                #region Aplicaciones 

                var paramProyeccion1 = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES");
                var proyeccionMeses1 = paramProyeccion1 != null ? paramProyeccion1.Valor : "12";
                var paramProyeccion2 = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES_2");
                var proyeccionMeses2 = paramProyeccion2 != null ? paramProyeccion2.Valor : "24";

                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    int DIA = DateTime.Now.Day;
                    int MES = DateTime.Now.Month;
                    int ANIO = DateTime.Now.Year;

                    if (filtros.CodigoAPTFiltrar == null) filtros.CodigoAPTFiltrar = new List<string>();

                    var paramColorXDefecto = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("COLOR_ROADMAP_POR_DEFECTO");
                    var colorXDefecto = paramColorXDefecto != null ? paramColorXDefecto.Valor : "#1f77b4";


                    var aplicaciones = (from a in ctx.AplicacionConfiguracion
                                        join b in ctx.Aplicacion on a.AplicacionId equals b.AplicacionId
                                        join c in ctx.RoadMap on b.RoadMapId equals c.RoadMapId
                                        where b.FlagActivo
                                        && a.AnioRegistro == ANIO && a.MesRegistro == MES && a.DiaRegistro == DIA
                                        && (filtros.CodigoAPTFiltrar.Contains(b.CodigoAPT))
                                        select new
                                        {
                                            b.CodigoAPT,
                                            //SituacionActual
                                            a.IndiceObsolescencia,
                                            a.RiesgoAplicacion,
                                            a.Priorizacion,
                                            //Proyeccion 1
                                            a.IndiceObsolescencia_Proyeccion1,
                                            a.RiesgoAplicacion_Proyeccion1,
                                            a.Priorizacion_Proyeccion1,
                                            //Proyeccion 2
                                            a.IndiceObsolescencia_Proyeccion2,
                                            a.RiesgoAplicacion_Proyeccion2,
                                            a.Priorizacion_Proyeccion2,
                                            Color = c.Color == null || c.Color == "" ? colorXDefecto : c.Color
                                        }).Distinct().ToList();

                    #region PROMEDIOS
                    PlotlyDataDTO promedioAplicacionesSituacionActual = null;
                    PlotlyDataDTO promedioAplicacionesProyeccion1 = null;
                    PlotlyDataDTO promedioAplicacionesProyeccion2 = null;

                    if (aplicaciones != null && aplicaciones.Count != 0)
                    {
                        promedioAplicacionesSituacionActual = new PlotlyDataDTO
                        {
                            Id = "1",
                            Text = "Situación actual",
                            X = Math.Round(aplicaciones.Average(x => x.RiesgoAplicacion.HasValue ? x.RiesgoAplicacion.Value : 0), 3),
                            Y = Math.Round(aplicaciones.Average(x => x.IndiceObsolescencia.HasValue ? x.IndiceObsolescencia.Value : 0), 3),
                            Valor = Math.Round(aplicaciones.Average(x => x.Priorizacion.HasValue ? x.Priorizacion.Value : 0), 3),
                        };
                        promedioAplicacionesProyeccion1 = new PlotlyDataDTO
                        {
                            Id = "2",
                            Text = string.Format("Proyección {0} meses", proyeccionMeses1),
                            X = Math.Round(aplicaciones.Average(x => x.RiesgoAplicacion_Proyeccion1.HasValue ? x.RiesgoAplicacion_Proyeccion1.Value : 0), 3),
                            Y = Math.Round(aplicaciones.Average(x => x.IndiceObsolescencia_Proyeccion1.HasValue ? x.IndiceObsolescencia_Proyeccion1.Value : 0), 3),
                            Valor = Math.Round(aplicaciones.Average(x => x.Priorizacion_Proyeccion1.HasValue ? x.Priorizacion_Proyeccion1.Value : 0), 3),
                        };
                        promedioAplicacionesProyeccion2 = new PlotlyDataDTO
                        {
                            Id = "3",
                            Text = string.Format("Proyección {0} meses", proyeccionMeses2),
                            X = Math.Round(aplicaciones.Average(x => x.RiesgoAplicacion_Proyeccion2.HasValue ? x.RiesgoAplicacion_Proyeccion2.Value : 0), 3),
                            Y = Math.Round(aplicaciones.Average(x => x.IndiceObsolescencia_Proyeccion2.HasValue ? x.IndiceObsolescencia_Proyeccion2.Value : 0), 3),
                            Valor = Math.Round(aplicaciones.Average(x => x.Priorizacion_Proyeccion2.HasValue ? x.Priorizacion_Proyeccion2.Value : 0), 3),
                        };

                    }

                    rpta.DataPromedioAplicaciones = new List<PlotlyDataDTO>
                        { promedioAplicacionesSituacionActual
                        , promedioAplicacionesProyeccion1
                        , promedioAplicacionesProyeccion2
                    };
                    #endregion

                    List<PlotlyDataDTO> aplicacionesSituacionActual = null;
                    List<PlotlyDataDTO> aplicacionesProyeccion1 = null;
                    List<PlotlyDataDTO> aplicacionesProyeccion2 = null;

                    if (aplicaciones != null && aplicaciones.Count != 0)
                    {


                        aplicacionesSituacionActual = (from a in aplicaciones
                                                       select new PlotlyDataDTO
                                                       {
                                                           Id = "1",
                                                           Text = "Situación actual",
                                                           Value = a.CodigoAPT,
                                                           Color = a.Color,
                                                           X = a.RiesgoAplicacion.HasValue ? Math.Round(a.RiesgoAplicacion.Value, 3) : 0,
                                                           Y = a.IndiceObsolescencia.HasValue ? Math.Round(a.IndiceObsolescencia.Value, 3) : 0,
                                                           Valor = a.Priorizacion.HasValue ? Math.Round(a.Priorizacion.Value, 3) : 0
                                                       }).ToList();

                        aplicacionesProyeccion1 = (from a in aplicaciones
                                                   select new PlotlyDataDTO
                                                   {
                                                       Id = "2",
                                                       Text = string.Format("Proyección {0} meses", proyeccionMeses1),
                                                       Value = a.CodigoAPT,
                                                       Color = a.Color,
                                                       X = a.RiesgoAplicacion_Proyeccion1.HasValue ? Math.Round(a.RiesgoAplicacion_Proyeccion1.Value, 3) : 0,
                                                       Y = a.IndiceObsolescencia_Proyeccion1.HasValue ? Math.Round(a.IndiceObsolescencia_Proyeccion1.Value, 3) : 0,
                                                       Valor = a.Priorizacion_Proyeccion1.HasValue ? Math.Round(a.Priorizacion_Proyeccion1.Value, 3) : 0
                                                   }).ToList();

                        aplicacionesProyeccion2 = (from a in aplicaciones
                                                   select new PlotlyDataDTO
                                                   {

                                                       Id = "3",
                                                       Text = string.Format("Proyección {0} meses", proyeccionMeses2),
                                                       Value = a.CodigoAPT,
                                                       Color = a.Color,
                                                       X = a.RiesgoAplicacion_Proyeccion2.HasValue ? Math.Round(a.RiesgoAplicacion_Proyeccion2.Value, 3) : 0,
                                                       Y = a.IndiceObsolescencia_Proyeccion2.HasValue ? Math.Round(a.IndiceObsolescencia_Proyeccion2.Value, 3) : 0,
                                                       Valor = a.Priorizacion_Proyeccion2.HasValue ? Math.Round(a.Priorizacion_Proyeccion2.Value, 3) : 0
                                                   }).ToList();

                    }

                    rpta.DataAplicaciones = aplicacionesSituacionActual.Concat(aplicacionesProyeccion1).Concat(aplicacionesProyeccion2).ToList();
                }

                #endregion

                rpta.Proyeccion1Meses = proyeccionMeses1;
                rpta.Proyeccion2Meses = proyeccionMeses2;
                return rpta;
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: GetReporte(FiltrosDashboardAplicacion filtros)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: GetReporte(FiltrosDashboardAplicacion filtros)"
                    , new object[] { null });
            }
        }

        public override AplicacionDTO GetAplicacionDetalleById(string codigoAPT)
        {
            string listaPCI = "";
            int flag = 0;
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    int DIA = DateTime.Now.Day;
                    int MES = DateTime.Now.Month;
                    int ANIO = DateTime.Now.Year;

                    var entidad = (from a in ctx.Aplicacion
                                       //join r in ctx.RoadMap on new { RoadMapId = a.RoadMapId.HasValue ? a.RoadMapId.Value : 0, FlagActivo = true } equals new { RoadMapId = r.RoadMapId, FlagActivo = r.FlagActivo } into lj1
                                       //from r in lj1.DefaultIfEmpty()
                                   join c in ctx.Criticidad on a.CriticidadId equals c.CriticidadId
                                   //KPIS
                                   join z in ctx.AplicacionConfiguracion on a.AplicacionId equals z.AplicacionId
                                   where a.CodigoAPT == codigoAPT && a.FlagActivo && c.Activo
                                   && z.AnioRegistro == ANIO && z.MesRegistro == MES && z.DiaRegistro == DIA
                                   select new AplicacionDTO()
                                   {
                                       Id = a.AplicacionId,
                                       CodigoAPT = a.CodigoAPT,
                                       Nombre = a.Nombre,
                                       CriticidadToString = c.DetalleCriticidad,
                                       GestionadoPor = a.GestionadoPor,
                                       //RoadMapToString = r.Nombre,
                                       EstadoAplicacion = a.EstadoAplicacion,
                                       //SituwacionActual
                                       // z.IndiceObsolescencia,
                                       //z.RiesgoAplicacion,




                                       KPIObsolescencia = Math.Round((decimal)z.IndiceObsolescencia_ForwardLooking, 4),
                                       KPIObsolescenciaReal = Math.Round((decimal)z.IndiceObsolescencia, 4),
                                       //Proyeccion 1
                                       //z.IndiceObsolescencia_Proyeccion1,
                                       //z.RiesgoAplicacion_Proyeccion1,
                                       KPIObsolescenciaProyeccion1 = Math.Round((decimal)z.IndiceObsolescencia_ForwardLooking_Proyeccion1, 4),
                                       //Proyeccion 2
                                       //z.IndiceObsolescencia_Proyeccion2,
                                       //z.RiesgoAplicacion_Proyeccion2,
                                       KPIObsolescenciaProyeccion2 = Math.Round((decimal)z.IndiceObsolescencia_ForwardLooking_Proyeccion2, 4),
                                       ListaPCI = ""
                                   }).FirstOrDefault();

                    var entidad2 = (from a in ctx.Aplicacion

                                    join b in ctx.Application on a.CodigoAPT equals b.applicationId
                                    //KPIS
                                    join c in ctx.ApplicationPCI on b.AppId equals c.ApplicationId
                                    join d in ctx.TipoPCI on c.TipoPCIId equals d.TipoPCIId
                                    where a.CodigoAPT == codigoAPT && a.FlagActivo && c.FlagActivo == true && b.isActive == true

                                    select new AplicacionDTO()
                                    {
                                        PCI = d.Nombre
                                    }).ToList();

                    foreach (AplicacionDTO a in entidad2)
                    {
                        if (flag != entidad2.Count() - 1)
                        {
                            listaPCI = listaPCI + a.PCI + ", ";
                        }
                        else listaPCI = listaPCI + a.PCI;
                    }

                    if (entidad != null)
                        entidad.ListaPCI = listaPCI;

                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: AplicacionDTO GetAplicacionDetalleById(int Id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: AplicacionDTO GetAplicacionDetalleById(int Id)"
                    , new object[] { null });
            }
        }





        public override List<AplicacionExpertoDTO> GetAplicacionExpertoByCodigoAPT(string codigoAPT, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                totalRows = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.Aplicacion
                                     join b in ctx.AplicacionExpertos on u.CodigoAPT equals b.CodigoAPT
                                     join e in ctx.TipoExperto on b.TipoExpertoId equals e.TipoExpertoId
                                     where u.FlagActivo && b.FlagActivo
                                     && b.CodigoAPT == codigoAPT
                                     select new AplicacionExpertoDTO()
                                     {
                                         Matricula = b.Matricula,
                                         Nombres = b.Nombres,
                                         Activo = b.FlagActivo,
                                         TipoExpertoId = b.TipoExpertoId,
                                         TipoExpertoToString = e.Nombres,
                                         FechaCreacion = e.FechaCreacion,
                                         UsuarioCreacion = e.CreadoPor
                                     }).OrderBy(sortName + " " + sortOrder).ToList();

                    totalRows = registros.Count();
                    var resultado = registros.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                    return resultado;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<AplicacionExpertoDTO> GetAplicacionExpertoByCodigoAPT(string codigoAPT, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<AplicacionExpertoDTO> GetAplicacionExpertoByCodigoAPT(string codigoAPT, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        #endregion

        public override List<CustomAutocomplete> GetTipoExpertoByFiltro(string filtro)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.TipoExperto
                                   where u.FlagActivo
                                   && (string.IsNullOrEmpty(filtro) || u.Nombres.ToUpper().Contains(filtro.ToUpper()))
                                   select new CustomAutocomplete()
                                   {
                                       Id = u.TipoExpertoId.ToString(),
                                       Descripcion = u.Nombres,
                                       value = u.Nombres
                                   }).ToList();

                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetJefeEquipoByFiltro(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetJefeEquipoByFiltro(string filtro)"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocomplete> GetUnidadByFiltro(string filtro)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.Aplicacion
                                   where u.FlagActivo
                                   && (string.IsNullOrEmpty(filtro) || u.Unidad.ToUpper().Contains(filtro.ToUpper()))
                                   orderby u.Unidad
                                   group u by u.Unidad into grp
                                   select new CustomAutocomplete()
                                   {
                                       Id = grp.Key,
                                       Descripcion = grp.Key,
                                       value = grp.Key
                                   }).ToList();

                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetGerenciaByFiltro(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetGerenciaByFiltro(string filtro)"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocomplete> GetAreaByFiltro(string filtro)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.Aplicacion
                                   where u.FlagActivo
                                   && (string.IsNullOrEmpty(filtro) || u.Area.ToUpper().Contains(filtro.ToUpper()))
                                   orderby u.Area
                                   group u by u.Area into grp
                                   select new CustomAutocomplete()
                                   {
                                       Id = grp.Key,
                                       Descripcion = grp.Key,
                                       value = grp.Key
                                   }).ToList();

                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetGerenciaByFiltro(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetGerenciaByFiltro(string filtro)"
                    , new object[] { null });
            }
        }

        public override List<AplicacionDTO> GetAplicacionesExperto(string matricula)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.AplicacionPortafolioResponsables
                                   where u.FlagActivo
                                   && u.Matricula == matricula
                                   select new AplicacionDTO()
                                   {
                                       CodigoAPT = u.CodigoAPT
                                   }).ToList();

                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetGerenciaByFiltro(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetGerenciaByFiltro(string filtro)"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocomplete> GetGestorByFiltro(string filtro)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.Aplicacion
                                   where u.FlagActivo
                                   && (string.IsNullOrEmpty(filtro) || u.Gestor_UsuarioAutorizador_ProductOwner.ToUpper().Contains(filtro.ToUpper()))
                                   orderby u.Gestor_UsuarioAutorizador_ProductOwner
                                   group u by u.Gestor_UsuarioAutorizador_ProductOwner into grp
                                   select new CustomAutocomplete()
                                   {
                                       Id = grp.Key,
                                       Descripcion = grp.Key,
                                       value = grp.Key
                                   }).ToList();

                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetGestionadoByFiltro(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetGestionadoByFiltro(string filtro)"
                    , new object[] { null });
            }
        }

        public override List<AplicacionDTO> GetAplicacionConsultor(PaginacionAplicacion pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                var fechaConsulta = DateTime.Now;
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        ctx.Database.CommandTimeout = 0;

                        var aplicacionExpertos = (from x in ctx.AplicacionExpertos
                                                  where x.Matricula == pag.username && x.FlagActivo
                                                  select x.CodigoAPT).ToList();

                        var totalServidoresRelacionados = (from a in ctx.Relacion
                                                               //join b in ctx.Tecnologia on a.TecnologiaId equals b.TecnologiaId
                                                           where a.AnioRegistro == fechaConsulta.Year
                                                           && a.MesRegistro == fechaConsulta.Month
                                                           && a.DiaRegistro == fechaConsulta.Day
                                                           && a.FlagActivo && a.TipoId == (int)ETipoRelacion.Equipo
                                                           && (a.EstadoId == (int)EEstadoRelacion.Aprobado || a.EstadoId == (int)EEstadoRelacion.PendienteEliminacion)
                                                           group a by a.CodigoAPT into grp
                                                           select new
                                                           {
                                                               CodigoAPT = grp.Key,
                                                               NroTecnologias = grp.Count()
                                                           }).Distinct();

                        var registros = (from u in ctx.Aplicacion
                                         join u2 in ctx.Criticidad on u.CriticidadId equals u2.CriticidadId
                                         join e in ctx.AplicacionExpertos on u.CodigoAPT equals e.CodigoAPT
                                         join r in ctx.RoadMap on u.RoadMapId equals r.RoadMapId into lj1
                                         from r in lj1.DefaultIfEmpty()
                                         join s in totalServidoresRelacionados on new { CodigoAPT = u.CodigoAPT } equals new { CodigoAPT = s.CodigoAPT } into lj2
                                         from s in lj2.DefaultIfEmpty()
                                         where (u.CodigoAPT.ToUpper().Contains(pag.Aplicacion.ToUpper())
                                         //|| u.Nombre.ToUpper().Contains(pag.Aplicacion.ToUpper())
                                         || (u.CodigoAPT + " - " + u.Nombre).ToUpper().Contains(pag.Aplicacion.ToUpper())
                                         || string.IsNullOrEmpty(pag.Aplicacion))
                                         && (u.EstadoAplicacion.ToUpper().Contains(pag.Estado.ToUpper()) || pag.Estado == null)
                                         && (u.GerenciaCentral.ToUpper().Contains(pag.Gerencia.ToUpper()) || pag.Gerencia == null)
                                         && (u.Division.ToUpper().Contains(pag.Division.ToUpper()) || pag.Division == null)
                                         && (u.Unidad.ToUpper().Contains(pag.Unidad.ToUpper()) || pag.Unidad == null)
                                         && (u.Area.ToUpper().Contains(pag.Area.ToUpper()) || pag.Area == null)
                                         && (u.JefeEquipo_ExpertoAplicacionUserIT_ProductOwner.ToUpper().Contains(pag.JefeEquipo.ToUpper()) || string.IsNullOrEmpty(pag.JefeEquipo))
                                         && (u.Owner_LiderUsuario_ProductOwner.ToUpper().Contains(pag.Owner.ToUpper()) || string.IsNullOrEmpty(pag.Owner))
                                         && u.FlagActivo && e.FlagActivo && e.Matricula == pag.Matricula
                                         //&& (pag.PerfilId == (int)EPerfilBCP.Administrador || aplicacionExpertos.Contains(u.CodigoAPT))
                                         orderby u.Nombre
                                         select new AplicacionDTO()
                                         {
                                             Id = u.AplicacionId,
                                             CodigoAPT = u.CodigoAPT,
                                             CodigoAPTStr = u.CodigoAPT,
                                             Nombre = u.Nombre,
                                             GestionadoPor = u.GestionadoPor,
                                             CriticidadId = u.CriticidadId,
                                             RoadMapId = u.RoadMapId,
                                             RoadMapToString = r.Nombre,
                                             Matricula = u.Matricula,
                                             Obsolescente = u.Obsolescente,
                                             //MesAnio = u.MesAnio,
                                             Activo = u.FlagActivo,
                                             UsuarioCreacion = u.CreadoPor,
                                             FechaCreacion = u.FechaCreacion,
                                             FechaRegistroProcedencia = u.FechaRegistroProcedencia,
                                             FechaModificacion = u.FechaModificacion,
                                             UsuarioModificacion = u.ModificadoPor,
                                             FlagRelacionar = u.FlagRelacionar,
                                             CriticidadToString = u2.DetalleCriticidad,
                                             TipoActivoInformacion = u.TipoActivoInformacion,
                                             GerenciaCentral = u.GerenciaCentral,
                                             Division = u.Division,
                                             Unidad = u.Unidad,
                                             Area = u.Area,
                                             NombreEquipo_Squad = u.NombreEquipo_Squad,
                                             Owner_LiderUsuario_ProductOwner = u.Owner_LiderUsuario_ProductOwner,
                                             JefeEquipo_ExpertoAplicacionUserIT_ProductOwner = u.JefeEquipo_ExpertoAplicacionUserIT_ProductOwner,
                                             Experto_Especialista = u.Experto_Especialista,
                                             TotalEquiposRelacionados = s == null ? 0 : s.NroTecnologias,
                                             EstadoAplicacion = u.EstadoAplicacion,
                                             TipoExperto = e.TipoExpertoId
                                         }).Distinct().OrderBy(pag.sortName + " " + pag.sortOrder);

                        totalRows = registros.Count();
                        var resultado = registros.Skip((pag.pageNumber - 1) * pag.pageSize).Take(pag.pageSize).ToList();

                        return resultado;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<AplicacionDTO> GetAplicacion(string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<AplicacionDTO> GetAplicacion(string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<AplicacionDTO> GetAplicacionResponsables(PaginacionAplicacion pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                var fechaConsulta = DateTime.Now;
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        ctx.Database.CommandTimeout = 0;

                        var registros = (from u in ctx.Aplicacion
                                         join u2 in ctx.AplicacionExpertos on u.CodigoAPT equals u2.CodigoAPT
                                         join u3 in ctx.TipoExperto on u2.TipoExpertoId equals u3.TipoExpertoId
                                         where (u.CodigoAPT.ToUpper().Contains(pag.Aplicacion.ToUpper())
                                         || (u.CodigoAPT + " - " + u.Nombre).ToUpper().Contains(pag.Aplicacion.ToUpper())
                                         || string.IsNullOrEmpty(pag.Aplicacion))
                                         && (u.EstadoAplicacion.ToUpper().Contains(pag.Estado.ToUpper()) || pag.Estado == null)
                                         && (u.GerenciaCentral.ToUpper().Contains(pag.Gerencia.ToUpper()) || pag.Gerencia == null)
                                         && (u.Division.ToUpper().Contains(pag.Division.ToUpper()) || pag.Division == null)
                                         && (u.Unidad.ToUpper().Contains(pag.Unidad.ToUpper()) || pag.Unidad == null)
                                         && (u.Area.ToUpper().Contains(pag.Area.ToUpper()) || pag.Area == null)
                                         && (u.JefeEquipo_ExpertoAplicacionUserIT_ProductOwner.ToUpper().Contains(pag.JefeEquipo.ToUpper()) || string.IsNullOrEmpty(pag.JefeEquipo))
                                         && (u.Owner_LiderUsuario_ProductOwner.ToUpper().Contains(pag.Owner.ToUpper()) || string.IsNullOrEmpty(pag.Owner))
                                         && u.FlagActivo
                                         orderby u.Nombre
                                         select new AplicacionDTO()
                                         {
                                             Id = u.AplicacionId,
                                             CodigoAPT = u.CodigoAPT,
                                             CodigoAPTStr = u.CodigoAPT,
                                             Nombre = u.Nombre,
                                             GestionadoPor = u.GestionadoPor,
                                             CriticidadId = u.CriticidadId,
                                             RoadMapId = u.RoadMapId,
                                             Matricula = u.Matricula,
                                             Obsolescente = u.Obsolescente,
                                             //MesAnio = u.MesAnio,
                                             Activo = u.FlagActivo,
                                             UsuarioCreacion = u2.CreadoPor,
                                             FechaCreacion = u2.FechaCreacion,
                                             FechaRegistroProcedencia = u.FechaRegistroProcedencia,
                                             FechaModificacion = u.FechaModificacion,
                                             UsuarioModificacion = u.ModificadoPor,
                                             FlagRelacionar = u.FlagRelacionar,
                                             TipoActivoInformacion = u.TipoActivoInformacion,
                                             GerenciaCentral = u.GerenciaCentral,
                                             Division = u.Division,
                                             Unidad = u.Unidad,
                                             Area = u.Area,
                                             NombreEquipo_Squad = u.NombreEquipo_Squad,
                                             Owner_LiderUsuario_ProductOwner = u.Owner_LiderUsuario_ProductOwner,
                                             JefeEquipo_ExpertoAplicacionUserIT_ProductOwner = u.JefeEquipo_ExpertoAplicacionUserIT_ProductOwner,
                                             Experto_Especialista = u.Experto_Especialista,
                                             ResponsableMatricula = u2.Matricula,
                                             ResponsableNombre = u2.Nombres,
                                             ResponsableTipo = u3.Nombres
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
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<AplicacionDTO> GetAplicacion(string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<AplicacionDTO> GetAplicacion(string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<AplicacionDTO> GetGestionAplicacion(PaginacionAplicacion pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                var fechaConsulta = DateTime.Now;
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        ctx.Database.CommandTimeout = 0;

                        var aplicacionExpertos = (from x in ctx.AplicacionExpertos
                                                  where x.Matricula == pag.username && x.FlagActivo
                                                  select x.CodigoAPT).ToList();

                        var totalServidoresRelacionados = (from a in ctx.Relacion
                                                               //join b in ctx.Tecnologia on a.TecnologiaId equals b.TecnologiaId
                                                           where a.AnioRegistro == fechaConsulta.Year
                                                           && a.MesRegistro == fechaConsulta.Month
                                                           && a.DiaRegistro == fechaConsulta.Day
                                                           && a.FlagActivo && a.TipoId == (int)ETipoRelacion.Equipo
                                                           && (a.EstadoId == (int)EEstadoRelacion.Aprobado || a.EstadoId == (int)EEstadoRelacion.PendienteEliminacion)
                                                           group a by a.CodigoAPT into grp
                                                           select new
                                                           {
                                                               CodigoAPT = grp.Key,
                                                               NroTecnologias = grp.Count()
                                                           }).Distinct();

                        var registros = (from u in ctx.Aplicacion
                                         join u2 in ctx.Criticidad on u.CriticidadId equals u2.CriticidadId
                                         join r in ctx.RoadMap on u.RoadMapId equals r.RoadMapId into lj1
                                         from r in lj1.DefaultIfEmpty()
                                         join s in totalServidoresRelacionados on new { CodigoAPT = u.CodigoAPT } equals new { CodigoAPT = s.CodigoAPT } into lj2
                                         from s in lj2.DefaultIfEmpty()
                                         join ad in ctx.AplicacionDetalle on u.AplicacionId equals ad.AplicacionId into lj3
                                         from ad in lj3.DefaultIfEmpty()
                                         where (string.IsNullOrEmpty(pag.Aplicacion)
                                         || u.CodigoAPT.ToUpper().Contains(pag.Aplicacion.ToUpper())
                                         || (u.CodigoAPT + " - " + u.Nombre).ToUpper().Contains(pag.Aplicacion.ToUpper()))
                                         && (u.EstadoAplicacion.ToUpper().Contains(pag.Estado.ToUpper()) || pag.Estado == null)
                                         && (u.GerenciaCentral.ToUpper().Contains(pag.Gerencia.ToUpper()) || pag.Gerencia == null)
                                         && (u.Division.ToUpper().Contains(pag.Division.ToUpper()) || pag.Division == null)
                                         && (u.Unidad.ToUpper().Contains(pag.Unidad.ToUpper()) || pag.Unidad == null)
                                         && (u.Area.ToUpper().Contains(pag.Area.ToUpper()) || pag.Area == null)
                                         && (u.JefeEquipo_ExpertoAplicacionUserIT_ProductOwner.ToUpper().Contains(pag.JefeEquipo.ToUpper()) || string.IsNullOrEmpty(pag.JefeEquipo))
                                         && (u.Owner_LiderUsuario_ProductOwner.ToUpper().Contains(pag.Owner.ToUpper()) || string.IsNullOrEmpty(pag.Owner))
                                         && (pag.PerfilId == (int)EPerfilBCP.Administrador || aplicacionExpertos.Contains(u.CodigoAPT))
                                         && u.FlagActivo
                                         && u.FlagCargaManual.Value
                                         && (!pag.EstadoSolicitud.Any() || pag.EstadoSolicitud.Contains(ad.EstadoSolicitudId.Value))
                                         orderby u.Nombre
                                         select new AplicacionDTO()
                                         {
                                             Id = u.AplicacionId,
                                             CodigoAPT = u.CodigoAPT,
                                             CodigoAPTStr = u.CodigoAPT,
                                             Nombre = u.Nombre,
                                             GestionadoPor = u.GestionadoPor,
                                             CriticidadId = u.CriticidadId,
                                             RoadMapId = u.RoadMapId,
                                             RoadMapToString = r.Nombre,
                                             Matricula = u.Matricula,
                                             Obsolescente = u.Obsolescente,
                                             //MesAnio = u.MesAnio,
                                             Activo = u.FlagActivo,
                                             UsuarioCreacion = u.CreadoPor,
                                             FechaCreacion = u.FechaCreacion,
                                             FechaRegistroProcedencia = u.FechaRegistroProcedencia,
                                             FechaModificacion = u.FechaModificacion,
                                             UsuarioModificacion = u.ModificadoPor,
                                             FlagRelacionar = u.FlagRelacionar,
                                             CriticidadToString = u2.DetalleCriticidad,
                                             TipoActivoInformacion = u.TipoActivoInformacion,
                                             GerenciaCentral = u.GerenciaCentral,
                                             Division = u.Division,
                                             Unidad = u.Unidad,
                                             Area = u.Area,
                                             NombreEquipo_Squad = u.NombreEquipo_Squad,
                                             Owner_LiderUsuario_ProductOwner = u.Owner_LiderUsuario_ProductOwner,
                                             JefeEquipo_ExpertoAplicacionUserIT_ProductOwner = u.JefeEquipo_ExpertoAplicacionUserIT_ProductOwner,
                                             Experto_Especialista = u.Experto_Especialista,
                                             TotalEquiposRelacionados = s == null ? 0 : s.NroTecnologias,
                                             EstadoAplicacion = u.EstadoAplicacion,
                                             AplicacionDetalle = new AplicacionDetalleDTO()
                                             {
                                                 EstadoSolicitudId = ad.EstadoSolicitudId.Value
                                             }
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
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<AplicacionDTO> GetGestionAplicacion(PaginacionAplicacion pag, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<AplicacionDTO> GetGestionAplicacion(PaginacionAplicacion pag, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override FiltrosAplicacion GetFiltrosGestionAplicacion()
        {
            try
            {
                FiltrosAplicacion arr_data = null;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    ctx.Database.CommandTimeout = 0;

                    arr_data = new FiltrosAplicacion();

                    arr_data.DataTooltip = ServiceManager<ActivosDAO>.Provider.GetAllToolTipPortafolio(null, false);
                    arr_data.DataListBox = ServiceManager<ActivosDAO>.Provider.GetDataListBoxNewByFiltro();

                    //arr_data.Criticidad = ServiceManager<CriticidadDAO>.Provider.GetCriticidadByFiltro(null);
                    arr_data.Criticidad = ServiceManager<ParametricasDAO>.Provider.GetParametricasByTabla(Utilitarios.GetEnumDescription2(EEntidadPortafolio.CriticidadAplicacion),
                    (int)EEntidadParametrica.PORTAFOLIO);

                    arr_data.Gerencia = ServiceManager<ActivosDAO>.Provider.GetGerenciaByFiltro(null).Select(x => x.Descripcion).ToArray();
                    arr_data.ProcesoClave = ServiceManager<ActivosDAO>.Provider.GetProcesoVitalByFiltro(null).Select(x => x.Descripcion).ToArray();

                    arr_data.CategoriaTecnologica = ServiceManager<ParametricasDAO>.Provider.GetParametricasByTabla(Utilitarios.GetEnumDescription2(EEntidadPortafolio.CategoriaTecnologica),
                                (int)EEntidadParametrica.PORTAFOLIO
                                ).Select(x => x.Descripcion).ToArray();

                    var estado = ServiceManager<ParametricasDAO>.Provider.GetParametricasByTabla(Utilitarios.GetEnumDescription2(EEntidadPortafolio.EstadoAplicacion), (int)EEntidadParametrica.PORTAFOLIO);
                    var estadoFiltro = ServiceManager<ParametroDAO>.Provider.ObtenerParametroApp(Utilitarios.ESTADOS_APLICACION);
                    if (estadoFiltro != null)
                    {
                        var lista = estadoFiltro.Valor.Split('|');
                        var listaRetorno = new List<string>();
                        foreach (var item in lista)
                        {
                            var filtro = estado.FirstOrDefault(x => x.Id == item);
                            if (filtro != null)
                            {
                                listaRetorno.Add(filtro.Descripcion);
                            }
                            arr_data.Estado = listaRetorno.ToArray();
                        }
                    }
                    else
                    {
                        arr_data.Estado = ServiceManager<ParametricasDAO>.Provider.GetParametricasByTabla(Utilitarios.GetEnumDescription2(EEntidadPortafolio.EstadoAplicacion),
                                (int)EEntidadParametrica.PORTAFOLIO
                                ).Select(x => x.Descripcion).ToArray();
                    }

                    //arr_data.TipoActivoInformacion = ServiceManager<ActivosDAO>.Provider.GetActivosByFiltro(null).Select(x => x.Descripcion).ToArray();
                    arr_data.TipoActivoInformacionAll = ServiceManager<ActivosDAO>.Provider.GetActivosByFiltro(null);

                    arr_data.MotivoCreacion = ServiceManager<ParametricasDAO>.Provider.GetParametricasByTabla(Utilitarios.GetEnumDescription2(EEntidadPortafolio.MotivoCreacion),
                                                    (int)EEntidadParametrica.PORTAFOLIO
                                                    ).Select(x => x.Descripcion).ToArray();

                    //var lModeloEntrega = Utilitarios.EnumToList<EModeloEntrega>();
                    //arr_data.ModeloEntrega = lModeloEntrega.Select(x => Utilitarios.GetEnumDescription2(x)).ToArray();

                    arr_data.ModeloEntrega = ServiceManager<ParametricasDAO>.Provider.GetParametricasByTabla(Utilitarios.GetEnumDescription2(EEntidadPortafolio.ModeloEntrega),
                                (int)EEntidadParametrica.PORTAFOLIO
                                ).Select(x => x.Descripcion).ToArray();

                    arr_data.AreaBIAN = ServiceManager<ActivosDAO>.Provider.GetAreaBianByFiltro(null).Select(x => x.Descripcion).ToArray();
                    arr_data.JefaturaEquipoATI = ServiceManager<ActivosDAO>.Provider.GetJefaturaAtiByFiltro(null).Select(x => x.Descripcion).ToArray();

                    arr_data.EntidadResponsable = ServiceManager<ParametricasDAO>.Provider.GetParametricasByTabla(Utilitarios.GetEnumDescription2(EEntidadPortafolio.EntidadResponsable),
                                (int)EEntidadParametrica.PORTAFOLIO
                                ).Select(x => x.Descripcion).ToArray();

                    arr_data.GestionadoPor = ServiceManager<ActivosDAO>.Provider.GetGestionadoPorByFiltro(null).Select(x => x.Descripcion).ToArray();

                    arr_data.ClasificacionTecnica = ServiceManager<ParametricasDAO>.Provider.GetParametricasByTabla(Utilitarios.GetEnumDescription2(EEntidadPortafolio.ClasificacionTecnica),
                                (int)EEntidadParametrica.PORTAFOLIO
                                ).Select(x => x.Descripcion).ToArray();

                    arr_data.OOR = ServiceManager<ParametricasDAO>.Provider.GetParametricasByTabla(Utilitarios.GetEnumDescription2(EEntidadPortafolio.OOR),
                        (int)EEntidadParametrica.PORTAFOLIO
                        ).Select(x => x.Descripcion).ToArray();
                    //).Select(x => new CustomAutocompleteConsulta { Id = int.Parse(x.Id), Descripcion = x.Descripcion }).ToList();

                    arr_data.AmbienteInstalacion = ServiceManager<ParametricasDAO>.Provider.GetParametricasByTabla(Utilitarios.GetEnumDescription2(EEntidadPortafolio.AmbienteInstalacion),
                        (int)EEntidadParametrica.PORTAFOLIO
                        ).Select(x => x.Descripcion).ToArray();

                    arr_data.MetodoAutenticacion = ServiceManager<ParametricasDAO>.Provider.GetParametricasByTabla(Utilitarios.GetEnumDescription2(EEntidadPortafolio.MetodoAutenticacion),
                        (int)EEntidadParametrica.PORTAFOLIO
                        ).Select(x => x.Descripcion).ToArray();

                    arr_data.MetodoAutorizacion = ServiceManager<ParametricasDAO>.Provider.GetParametricasByTabla(Utilitarios.GetEnumDescription2(EEntidadPortafolio.MetodoAutorizacion),
                        (int)EEntidadParametrica.PORTAFOLIO
                        ).Select(x => x.Descripcion).ToArray();

                    arr_data.Contingencia = ServiceManager<ParametricasDAO>.Provider.GetParametricasByTabla(Utilitarios.GetEnumDescription2(EEntidadPortafolio.Contingencia),
                        (int)EEntidadParametrica.PORTAFOLIO
                        ).Select(x => x.Descripcion).ToArray();

                    arr_data.Ubicacion = ServiceManager<ParametricasDAO>.Provider.GetParametricasByTabla(Utilitarios.GetEnumDescription2(EEntidadPortafolio.Ubicacion),
                        (int)EEntidadParametrica.PORTAFOLIO
                        ).Select(x => x.Descripcion).ToArray();

                    arr_data.TipoDesarrollo = ServiceManager<ParametricasDAO>.Provider.GetParametricasByTabla(Utilitarios.GetEnumDescription2(EEntidadPortafolio.TipoDesarrollo),
                        (int)EEntidadParametrica.PORTAFOLIO
                        ).Select(x => x.Descripcion).ToArray();

                    arr_data.InfraestructuraAplicacion = ServiceManager<ParametricasDAO>.Provider.GetParametricasByTabla(Utilitarios.GetEnumDescription2(EEntidadPortafolio.InfraestructuraAplicacion),
                        (int)EEntidadParametrica.PORTAFOLIO
                        ).Select(x => x.Descripcion).ToArray();

                    var lEstadoSolicitudApp = Utilitarios.EnumToList<EEstadoSolicitudAplicacion>();
                    arr_data.EstadoSolicitud = lEstadoSolicitudApp.Select(x => new CustomAutocompleteConsulta { Id = (int)x, Descripcion = Utilitarios.GetEnumDescription2(x) }).ToList();

                    arr_data.Generico = ServiceManager<ParametricasDAO>.Provider.GetParametricasByTabla(Utilitarios.GetEnumDescription2(EEntidadPortafolio.Generico),
                        (int)EEntidadParametrica.PORTAFOLIO
                        ).Select(x => x.Descripcion).ToArray();

                    arr_data.CompatibilidadNavegador = ServiceManager<ParametricasDAO>.Provider.GetParametricasByTabla(Utilitarios.GetEnumDescription2(EEntidadPortafolio.CompatibilidadNavegador),
                        (int)EEntidadParametrica.PORTAFOLIO
                        ).Select(x => x.Descripcion).ToArray();

                    arr_data.GrupoTicketRemedy = ServiceManager<ParametricasDAO>.Provider.GetParametricasByTabla(Utilitarios.GetEnumDescription2(EEntidadPortafolio.GrupoTicketRemedy),
                       (int)EEntidadParametrica.PORTAFOLIO
                       ).Select(x => x.Descripcion).ToArray();

                    arr_data.ClasificacionCriticidad = ServiceManager<ParametricasDAO>.Provider.GetParametricasByTabla(Utilitarios.GetEnumDescription2(EEntidadPortafolio.ClasificacionCriticidad),
                       (int)EEntidadParametrica.PORTAFOLIO
                       ).Select(x => x.Descripcion).ToArray();

                    arr_data.EstadoRoadmap = ServiceManager<ParametricasDAO>.Provider.GetParametricasByTabla(Utilitarios.GetEnumDescription2(EEntidadPortafolio.EstadoRoadmap),
                       (int)EEntidadParametrica.PORTAFOLIO
                       ).Select(x => x.Descripcion).ToArray();

                    arr_data.EstadoCriticidad = ServiceManager<ParametricasDAO>.Provider.GetParametricasByTabla(Utilitarios.GetEnumDescription2(EEntidadPortafolio.EstadoCriticidad),
                       (int)EEntidadParametrica.PORTAFOLIO
                       ).Select(x => x.Descripcion).ToArray();

                    arr_data.TipoExpertoPortafolio = ServiceManager<AplicacionDAO>.Provider.GetTipoExpertoPortafolioByFiltro(null);
                }
                return arr_data;
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: FiltrosAplicacion GetFiltros()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: FiltrosAplicacion GetFiltros()"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocomplete> GetTTLByFiltro(string filtro)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.Aplicacion
                                   where u.FlagActivo
                                   && (string.IsNullOrEmpty(filtro) || u.TribeTechnicalLead.ToUpper().Contains(filtro.ToUpper()))
                                   orderby u.TribeTechnicalLead
                                   group u by u.TribeTechnicalLead into grp
                                   select new CustomAutocomplete()
                                   {
                                       Id = grp.Key,
                                       Descripcion = grp.Key,
                                       value = grp.Key
                                   }).ToList();

                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetOwnerByFiltro(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetOwnerByFiltro(string filtro)"
                    , new object[] { null });
            }
        }

        private void AddOrEditResponsableAprobacion(GestionCMDB_ProdEntities ctx, AplicacionDTO objeto, long ID_APLICACION)
        {
            var lBandejaAprobadores = new int[]
            {
                (int)EBandejaAprobadorAplicacion.ClasificacionTecnica,
                (int)EBandejaAprobadorAplicacion.DevSecOps,
            };

            DateTime FECHA_ACTUAL = DateTime.Now;
            //var lBandejaArquitectoTI = new List<CustomAutocompleteAprobador>();
            CustomAutocompleteAprobador lBandejaArquitectoTI = null;
            CustomAutocompleteAprobador lBandejaTTL = null;
            CustomAutocompleteAprobador lBandejaPO = null;
            CustomAutocompleteAprobador lBandejaGestorUserIT = null;
            string matArquitecto = !string.IsNullOrEmpty(objeto.ArquitectoTI) ? objeto.ArquitectoTI.Split('-')[0].Trim() : "";
            if (objeto.AplicacionDetalle.EstadoSolicitudId == (int)EEstadoSolicitudAplicacion.EnRevision)
            {
                //Buscando la solicitud de la aplicacion
                var idAPP = Convert.ToInt32(ID_APLICACION);
                var solicitudCreacion = ctx.Solicitud.FirstOrDefault(x => x.AplicacionId == idAPP
                && x.TipoSolicitud == (int)ETipoSolicitudAplicacion.CreacionAplicacion);
                if (solicitudCreacion != null)
                {
                    //Asignando un nuevo responsable a la bandeja Gestor User IT
                    if (objeto.TipoFlujoId == (int)EFlujoRegistro.PAE)
                    {
                        //var mGestor = objeto.GestorUserIT;
                        var mGestor = objeto.Gestor_UsuarioAutorizador_ProductOwner;
                        if (!string.IsNullOrEmpty(mGestor) && mGestor != Utilitarios.NO_APLICA)
                        {
                            lBandejaGestorUserIT = new CustomAutocompleteAprobador()
                            {
                                Id = (int)EBandejaAprobadorAplicacion.GestorUserIT,
                                MatriculaList = mGestor
                            };
                        }
                    }

                    //Asignando un nuevo responsable a la bandeja TTL
                    if (!string.IsNullOrEmpty(objeto.GestionadoPor))
                    {
                        var iGestionadoPor = ctx.GestionadoPor.FirstOrDefault(x => x.FlagActivo
                        && x.Nombre.ToUpper().Equals(objeto.GestionadoPor.ToUpper()) && x.FlagEquipoAgil.Value);
                        if (iGestionadoPor != null)
                        {
                            var mTTL = objeto.TribeTechnicalLead;
                            if (!string.IsNullOrEmpty(mTTL) && mTTL != Utilitarios.NO_APLICA)
                            {
                                lBandejaTTL = new CustomAutocompleteAprobador()
                                {
                                    Id = (int)EBandejaAprobadorAplicacion.TTL,
                                    MatriculaList = mTTL
                                };
                            }
                        }
                    }

                    //Asignando un nuevo responsable a la bandeja Lider Usuario /PO
                    var mPO = objeto.Owner_LiderUsuario_ProductOwner;
                    if (objeto.TipoFlujoId == (int)EFlujoRegistro.FNA)
                    {
                        if (!string.IsNullOrEmpty(mPO) && mPO != Utilitarios.NO_APLICA)
                        {
                            lBandejaPO = new CustomAutocompleteAprobador()
                            {
                                Id = (int)EBandejaAprobadorAplicacion.PO,
                                MatriculaList = mPO
                            };
                        }
                    }
                    else
                        mPO = string.Empty;


                    //Asignando un nuevo responsable a la bandeja Arquitectura de TI
                    if (!string.IsNullOrEmpty(objeto.ArquitectoTI))
                    {
                        //lBandejaArquitectoTI = (from x in ctx.ArquitectoTI
                        //                        join y in ctx.JefaturaAti on x.JefaturaAtiId equals y.JefaturaAtiId
                        //                        where x.FlagActivo && y.FlagActivo
                        //                        && y.Nombre.ToUpper().Equals(objeto.JefaturaATI.ToUpper())
                        //                        group x by new { x.JefaturaAtiId } into grp
                        //                        select grp).AsEnumerable().Select(grp =>
                        //                        new CustomAutocompleteAprobador()
                        //                        {
                        //                            Id = (int)EBandejaAprobadorAplicacion.ArquitecturaTI,
                        //                            MatriculaList = string.Join("|", grp.Select(m => m.Matricula))
                        //                        }).ToList();
                        lBandejaArquitectoTI = new CustomAutocompleteAprobador()
                        {
                            Id = (int)EBandejaAprobadorAplicacion.ArquitecturaTI,
                            MatriculaList = matArquitecto
                        };
                    }

                    //Creando la solicitud aprobadores detalle
                    var lBandejas = (from a in ctx.BandejaAprobacion
                                     where a.FlagActivo
                                     //&& a.BandejaId != (int)EBandejaAprobadorAplicacion.ArquitecturaTI
                                     && lBandejaAprobadores.Contains(a.BandejaId)
                                     group a by new { a.BandejaId } into grp
                                     select grp).AsEnumerable().Select(grp =>
                                     new CustomAutocompleteAprobador()
                                     {
                                         Id = grp.Key.BandejaId,
                                         MatriculaList = string.Join("|", grp.Select(x => x.MatriculaAprobador))
                                     }).ToList();

                    if (lBandejas != null && lBandejas.Count > 0)
                    {
                        if (lBandejaArquitectoTI != null)
                            lBandejas.Add(lBandejaArquitectoTI);

                        if (lBandejaPO != null)
                            lBandejas.Add(lBandejaPO);

                        if (lBandejaTTL != null)
                            lBandejas.Add(lBandejaTTL);

                        if (lBandejaGestorUserIT != null)
                            lBandejas.Add(lBandejaGestorUserIT);

                        foreach (var item in lBandejas)
                        {
                            bool createInsert = false;

                            switch (item.Id)
                            {
                                case (int)EBandejaAprobadorAplicacion.ArquitecturaTI:
                                    createInsert = true;
                                    break;
                                case (int)EBandejaAprobadorAplicacion.ClasificacionTecnica:
                                    createInsert = true;
                                    break;
                                case (int)EBandejaAprobadorAplicacion.DevSecOps:
                                    var dataDSO = objeto.AplicacionDetalle.ModeloEntrega;
                                    createInsert = !string.IsNullOrEmpty(dataDSO) && (dataDSO == "DEVSECOPS" || dataDSO == "HIBRIDO");
                                    break;
                                case (int)EBandejaAprobadorAplicacion.PO:
                                    createInsert = !string.IsNullOrEmpty(mPO) && mPO != Utilitarios.NO_APLICA;
                                    break;
                                case (int)EBandejaAprobadorAplicacion.TTL:
                                    var dataTTL = objeto.TribeTechnicalLead;
                                    createInsert = !string.IsNullOrEmpty(dataTTL) && dataTTL != Utilitarios.NO_APLICA;
                                    break;
                                case (int)EBandejaAprobadorAplicacion.GestorUserIT:
                                    //var dataGestor = objeto.GestorUserIT;
                                    var dataGestor = objeto.Gestor_UsuarioAutorizador_ProductOwner;
                                    createInsert = objeto.TipoFlujoId == (int)EFlujoRegistro.PAE && !string.IsNullOrEmpty(dataGestor) && dataGestor != Utilitarios.NO_APLICA;
                                    break;
                            }

                            if (createInsert)
                            {
                                var iSA = ctx.SolicitudAprobadores.FirstOrDefault(x => x.FlagActivo
                                && x.SolicitudAplicacionId == solicitudCreacion.SolicitudAplicacionId && x.BandejaId == item.Id);
                                if (iSA != null)
                                {
                                    if (!iSA.FlagAprobado.Value)
                                    {
                                        iSA.Matricula = item.MatriculaList;
                                        iSA.UsuarioModificacion = objeto.UsuarioCreacion;
                                        iSA.FechaModificacion = FECHA_ACTUAL;
                                        ctx.SaveChanges();
                                    }
                                }
                                else
                                {
                                    var solicitudAprobador = new SolicitudAprobadores()
                                    {
                                        SolicitudAplicacionId = solicitudCreacion.SolicitudAplicacionId,
                                        BandejaId = item.Id,
                                        Matricula = item.MatriculaList,
                                        FlagAprobado = false,
                                        EstadoAprobacion = (int)EEstadoSolicitudAplicacion.EnRevision,
                                        FlagActivo = true,
                                        UsuarioCreacion = objeto.UsuarioCreacion,
                                        FechaCreacion = FECHA_ACTUAL
                                    };
                                    ctx.SolicitudAprobadores.Add(solicitudAprobador);
                                    ctx.SaveChanges();
                                }
                            }
                        }
                    }
                }
            }
        }

        public override DataResultAplicacion AddOrEditAplicacion(AplicacionDTO objeto)
        {
            DbContextTransaction transaction = null;
            AplicacionDTO objetoAuditoria = null;
            try
            {
                long ID = 0;
                long SOLICITUD_ID = 0;
                DateTime FECHA_ACTUAL = DateTime.Now;
                var dataResult = new DataResultAplicacion()
                {
                    AplicacionId = 0,
                    SolicitudId = 0,
                    EstadoTransaccion = true
                };
                string matArquitecto = !string.IsNullOrEmpty(objeto.ArquitectoTI) ? objeto.ArquitectoTI.Split('-')[0].Trim() : "";
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    ctx.Database.CommandTimeout = 0;

                    using (transaction = ctx.Database.BeginTransaction())
                    {
                        if (objeto.TipoSolicitudId == (int)ETipoSolicitudAplicacion.CreacionAplicacion)
                        {
                            //Revalidar el codigoAPT
                            bool existsCodigoAPT = ServiceManager<AplicacionDAO>.Provider.ExisteAplicacionById(objeto.CodigoAPT, objeto.Id);
                            if (existsCodigoAPT) dataResult.EstadoTransaccion = false;
                        }

                        if (dataResult.EstadoTransaccion)
                        {
                            if (objeto.Id == 0)
                            {
                                AddOrEditResponsablesAplicacion(objeto);

                                var entidad = new Aplicacion()
                                {
                                    CodigoAPT = objeto.CodigoAPT,
                                    Nombre = objeto.Nombre,
                                    TipoActivoInformacion = objeto.TipoActivoInformacion,
                                    GerenciaCentral = objeto.GerenciaCentral,
                                    Division = objeto.Division,
                                    Area = objeto.Area,
                                    Unidad = objeto.Unidad,
                                    DescripcionAplicacion = objeto.DescripcionAplicacion,
                                    AreaBIAN = objeto.AreaBIAN,
                                    DominioBIAN = objeto.DominioBIAN,
                                    JefaturaATI = objeto.JefaturaATI,
                                    TribeTechnicalLead = objeto.TribeTechnicalLead,
                                    JefeEquipo_ExpertoAplicacionUserIT_ProductOwner = objeto.JefeEquipo_ExpertoAplicacionUserIT_ProductOwner,
                                    NombreEquipo_Squad = objeto.NombreEquipo_Squad,
                                    GestionadoPor = objeto.GestionadoPor,
                                    Owner_LiderUsuario_ProductOwner = objeto.Owner_LiderUsuario_ProductOwner,
                                    Gestor_UsuarioAutorizador_ProductOwner = objeto.Gestor_UsuarioAutorizador_ProductOwner,
                                    Resp_Owner = objeto.MatriculaOwner,
                                    Resp_Gestor = objeto.MatriculaGestor,
                                    Resp_TTL = objeto.MatriculaTTL,
                                    Resp_Experto = objeto.MatriculaExperto,
                                    Resp_Broker = objeto.MatriculaBroker,
                                    Resp_JDE = objeto.MatriculaJDE,
                                    EntidadResponsable = objeto.EntidadResponsable,
                                    CriticidadId = objeto.CriticidadId,
                                    CategoriaTecnologica = objeto.CategoriaTecnologica,
                                    RoadMapId = null,
                                    FechaRegistroProcedencia = FECHA_ACTUAL,
                                    Matricula = null,
                                    Obsolescente = null,
                                    FlagActivo = true,
                                    FechaCreacion = FECHA_ACTUAL,
                                    CreadoPor = objeto.UsuarioCreacion,
                                    FlagRelacionar = true,
                                    Experto_Especialista = objeto.Experto_Especialista,
                                    Mes = FECHA_ACTUAL.Month,
                                    Anio = FECHA_ACTUAL.Year,
                                    EstadoAplicacion = (objeto.EstadoAplicacion == "-" || string.IsNullOrWhiteSpace(objeto.EstadoAplicacion)) ? "Eliminada" : objeto.EstadoAplicacion,
                                    BrokerSistemas = objeto.BrokerSistemas,
                                    FlagCargaManual = true,
                                    FlagAprobado = false,
                                    ClasificacionTecnica = objeto.ClasificacionTecnica,
                                    SubclasificacionTecnica = objeto.SubclasificacionTecnica,
                                    ArquitectoTI = matArquitecto,
                                    GestorUserIT = objeto.GestorUserIT,
                                    FechaCreacionAplicacion = objeto.FechaCreacionAplicacion
                                };

                                ctx.Aplicacion.Add(entidad);
                                ctx.SaveChanges();

                                ID = entidad.AplicacionId;

                                //Creacion de la solicitud de tipo creacion
                                var solicitudCreacion = new Solicitud()
                                {
                                    TipoSolicitud = (int)ETipoSolicitudAplicacion.CreacionAplicacion,
                                    UsuarioCreacion = objeto.UsuarioCreacion,
                                    FechaCreacion = FECHA_ACTUAL,
                                    AplicacionId = Convert.ToInt32(ID),
                                    EstadoSolicitud = objeto.AplicacionDetalle.EstadoSolicitudId,
                                    FlagAprobacion = false,
                                    Observaciones = objeto.MotivoComentario
                                };
                                ctx.Solicitud.Add(solicitudCreacion);
                                ctx.SaveChanges();

                                SOLICITUD_ID = solicitudCreacion.SolicitudAplicacionId;

                                //Actualizando el parametro ULTIMO_CODIGO_APT_PAE
                                if (objeto.TipoFlujoId == (int)EFlujoRegistro.PAE)
                                {
                                    var updateParametro = new ParametroDTO()
                                    {
                                        Codigo = Utilitarios.ULTIMO_CODIGOAPT_PAE_PORTAFOLIO,
                                        Valor = objeto.CodigoAPT,
                                        UsuarioModificacion = objeto.UsuarioModificacion
                                    };
                                    ServiceManager<ParametroDAO>.Provider.ActualizarParametroAppByCodigo(updateParametro, ctx);
                                }

                                objetoAuditoria = objeto;
                            }
                            else
                            {
                                ID = RegistroEdicionSolicitudAplicacion(ctx, objeto, matArquitecto, out objetoAuditoria, out SOLICITUD_ID);
                            }

                            //Add or Edit Responsables Aprobadores
                            if (objeto.CargaResponsables) AddOrEditResponsableAprobacion(ctx, objeto, ID);

                            //Add or Edit nuevos campos
                            var dataNF = objeto.NewInputs;
                            if (dataNF != null && dataNF.Count > 0)
                            {
                                foreach (var item in dataNF)
                                {
                                    var newObject = new NuevoCampoPortafolioDTO()
                                    {
                                        Codigo = item.Id,
                                        Valor = item.Valor,
                                        CodigoAPT = objeto.CodigoAPT,
                                        UsuarioCreacion = objeto.UsuarioCreacion,
                                        UsuarioModificacion = objeto.UsuarioCreacion
                                    };
                                    ServiceManager<ActivosDAO>.Provider.AddOrEditNuevoCampoPortafolio(newObject, ctx);
                                }
                            }

                            //Add or Edit AplicacionDetalle
                            RegistroEdicionSolicitudAplicacionDetalle(ctx, objeto, ID);

                            transaction.Commit();

                            //Registro de eventos en la bitácora
                            RegistrarDataBitacora(objetoAuditoria, ID);
                        }

                        dataResult.AplicacionId = ID;
                        dataResult.SolicitudId = SOLICITUD_ID;

                        return dataResult;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                transaction.Rollback();
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: int AddOrEditTipo(TipoDTO objeto)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: int AddOrEditTipo(TipoDTO objeto)"
                    , new object[] { null });
            }
        }

        private long RegistroEdicionSolicitudAplicacion(GestionCMDB_ProdEntities ctx, AplicacionDTO objeto, string matArquitecto, out AplicacionDTO objetoAuditoria, out long SOLICITUD_ID)
        {
            try
            {
                DateTime FECHA_ACTUAL = DateTime.Now;
                objetoAuditoria = new AplicacionDTO();
                long ID = 0;
                SOLICITUD_ID = 0;

                switch (objeto.TipoSolicitudId)
                {
                    case (int)ETipoSolicitudAplicacion.CreacionAplicacion:
                        objetoAuditoria = ServiceManager<AplicacionDAO>.Provider.GetAplicacionById(objeto.Id, objeto.TipoSolicitudId);
                        var entidad = ctx.Aplicacion.FirstOrDefault(x => x.AplicacionId == objeto.Id);

                        if (entidad != null)
                        {
                            entidad.Nombre = objeto.Nombre;
                            entidad.DescripcionAplicacion = objeto.DescripcionAplicacion;
                            entidad.FechaModificacion = FECHA_ACTUAL;
                            entidad.ModificadoPor = objeto.UsuarioModificacion;

                            entidad.Owner_LiderUsuario_ProductOwner = objeto.Owner_LiderUsuario_ProductOwner;
                            entidad.Gestor_UsuarioAutorizador_ProductOwner = objeto.Gestor_UsuarioAutorizador_ProductOwner;
                            entidad.TribeTechnicalLead = objeto.TribeTechnicalLead;
                            entidad.Experto_Especialista = objeto.Experto_Especialista;
                            entidad.BrokerSistemas = objeto.BrokerSistemas;
                            entidad.JefeEquipo_ExpertoAplicacionUserIT_ProductOwner = objeto.JefeEquipo_ExpertoAplicacionUserIT_ProductOwner;

                            entidad.Resp_Owner = objeto.MatriculaOwner;
                            entidad.Resp_Gestor = objeto.MatriculaGestor;
                            entidad.Resp_TTL = objeto.MatriculaTTL;
                            entidad.Resp_Experto = objeto.MatriculaExperto;
                            entidad.Resp_Broker = objeto.MatriculaBroker;
                            entidad.Resp_JDE = objeto.MatriculaJDE;

                            AddOrEditResponsablesAplicacion(objeto);

                            entidad.TipoActivoInformacion = objeto.TipoActivoInformacion;
                            entidad.GerenciaCentral = objeto.GerenciaCentral;
                            entidad.Division = objeto.Division;
                            entidad.Area = objeto.Area;
                            entidad.Unidad = objeto.Unidad;
                            entidad.CodigoAPT = objeto.CodigoAPT;
                            entidad.DescripcionAplicacion = objeto.DescripcionAplicacion;
                            entidad.EstadoAplicacion = objeto.EstadoAplicacion; //ToDo
                            entidad.CategoriaTecnologica = objeto.CategoriaTecnologica;
                            entidad.AreaBIAN = objeto.AreaBIAN;
                            entidad.DominioBIAN = objeto.DominioBIAN;
                            entidad.JefaturaATI = objeto.JefaturaATI;
                            entidad.CriticidadId = objeto.CriticidadId;
                            entidad.ClasificacionTecnica = objeto.ClasificacionTecnica;
                            entidad.SubclasificacionTecnica = objeto.SubclasificacionTecnica;
                            entidad.ArquitectoTI = matArquitecto;
                            entidad.GestionadoPor = objeto.GestionadoPor;
                            entidad.EntidadResponsable = objeto.EntidadResponsable;
                            entidad.GestorUserIT = objeto.GestorUserIT;
                            entidad.FechaCreacionAplicacion = objeto.FechaCreacionAplicacion;
                            entidad.NombreEquipo_Squad = objeto.NombreEquipo_Squad;

                            ctx.SaveChanges();
                            ID = entidad.AplicacionId;
                        }
                        break;
                    case (int)ETipoSolicitudAplicacion.ModificacionAplicacion:
                        objetoAuditoria = ServiceManager<AplicacionDAO>.Provider.GetAplicacionById(objeto.Id, objeto.TipoSolicitudId);
                        var objSolMod = ctx.SolicitudAplicacion.FirstOrDefault(x => x.AplicacionId == objeto.Id); //ToDo FlagAprobado

                        if (objSolMod != null)
                        {
                            objSolMod.Nombre = objeto.Nombre;
                            objSolMod.DescripcionAplicacion = objeto.DescripcionAplicacion;
                            objSolMod.FechaModificacion = FECHA_ACTUAL;
                            objSolMod.ModificadoPor = objeto.UsuarioModificacion;

                            objSolMod.Owner_LiderUsuario_ProductOwner = objeto.Owner_LiderUsuario_ProductOwner;
                            objSolMod.Gestor_UsuarioAutorizador_ProductOwner = objeto.Gestor_UsuarioAutorizador_ProductOwner;
                            objSolMod.TribeTechnicalLead = objeto.TribeTechnicalLead;
                            objSolMod.Experto_Especialista = objeto.Experto_Especialista;
                            objSolMod.BrokerSistemas = objeto.BrokerSistemas;
                            objSolMod.JefeEquipo_ExpertoAplicacionUserIT_ProductOwner = objeto.JefeEquipo_ExpertoAplicacionUserIT_ProductOwner;

                            AddOrEditResponsablesAplicacion(objeto); //ToDo

                            objSolMod.TipoActivoInformacion = objeto.TipoActivoInformacion;
                            objSolMod.GerenciaCentral = objeto.GerenciaCentral;
                            objSolMod.Division = objeto.Division;
                            objSolMod.Area = objeto.Area;
                            objSolMod.Unidad = objeto.Unidad;
                            objSolMod.CodigoAPT = objeto.CodigoAPT;
                            objSolMod.DescripcionAplicacion = objeto.DescripcionAplicacion;
                            objSolMod.EstadoAplicacion = objeto.EstadoAplicacion;
                            objSolMod.CategoriaTecnologica = objeto.CategoriaTecnologica;
                            objSolMod.AreaBIAN = objeto.AreaBIAN;
                            objSolMod.DominioBIAN = objeto.DominioBIAN;
                            objSolMod.JefaturaATI = objeto.JefaturaATI;
                            objSolMod.CriticidadId = objeto.CriticidadId;
                            objSolMod.ClasificacionTecnica = objeto.ClasificacionTecnica;
                            objSolMod.SubclasificacionTecnica = objeto.SubclasificacionTecnica;
                            objSolMod.ArquitectoTI = matArquitecto;
                            objSolMod.GestionadoPor = objeto.GestionadoPor;
                            objSolMod.EntidadResponsable = objeto.EntidadResponsable;
                            objSolMod.GestorUserIT = objeto.GestorUserIT;
                            objSolMod.FechaCreacionAplicacion = objeto.FechaCreacionAplicacion;
                            objSolMod.NombreEquipo_Squad = objeto.NombreEquipo_Squad;

                            ctx.SaveChanges();
                            ID = objSolMod.AplicacionId;
                        }
                        else
                        {
                            AddOrEditResponsablesAplicacion(objeto);
                            var objNewSolMod = new SolicitudAplicacion()
                            {
                                AplicacionId = objeto.Id,
                                CodigoAPT = objeto.CodigoAPT,
                                Nombre = objeto.Nombre,
                                TipoActivoInformacion = objeto.TipoActivoInformacion,
                                GerenciaCentral = objeto.GerenciaCentral,
                                Division = objeto.Division,
                                Area = objeto.Area,
                                Unidad = objeto.Unidad,
                                DescripcionAplicacion = objeto.DescripcionAplicacion,
                                AreaBIAN = objeto.AreaBIAN,
                                DominioBIAN = objeto.DominioBIAN,
                                JefaturaATI = objeto.JefaturaATI,
                                TribeTechnicalLead = objeto.TribeTechnicalLead,
                                JefeEquipo_ExpertoAplicacionUserIT_ProductOwner = objeto.JefeEquipo_ExpertoAplicacionUserIT_ProductOwner,
                                NombreEquipo_Squad = objeto.NombreEquipo_Squad,
                                GestionadoPor = objeto.GestionadoPor,
                                Owner_LiderUsuario_ProductOwner = objeto.Owner_LiderUsuario_ProductOwner,
                                Gestor_UsuarioAutorizador_ProductOwner = objeto.Gestor_UsuarioAutorizador_ProductOwner,
                                EntidadResponsable = objeto.EntidadResponsable,
                                CriticidadId = objeto.CriticidadId,
                                CategoriaTecnologica = objeto.CategoriaTecnologica,
                                RoadMapId = null,
                                FechaRegistroProcedencia = FECHA_ACTUAL,
                                Matricula = null,
                                Obsolescente = null,
                                FlagActivo = true,
                                FechaCreacion = FECHA_ACTUAL,
                                CreadoPor = objeto.UsuarioCreacion,
                                FlagRelacionar = true,
                                Experto_Especialista = objeto.Experto_Especialista,
                                Mes = FECHA_ACTUAL.Month,
                                Anio = FECHA_ACTUAL.Year,
                                EstadoAplicacion = (objeto.EstadoAplicacion == "-" || string.IsNullOrWhiteSpace(objeto.EstadoAplicacion)) ? "Eliminada" : objeto.EstadoAplicacion,
                                BrokerSistemas = objeto.BrokerSistemas,
                                FlagCargaManual = true,
                                FlagAprobado = true,
                                ClasificacionTecnica = objeto.ClasificacionTecnica,
                                SubclasificacionTecnica = objeto.SubclasificacionTecnica,
                                ArquitectoTI = matArquitecto,
                                GestorUserIT = objeto.GestorUserIT,
                                FechaCreacionAplicacion = objeto.FechaCreacionAplicacion,
                                FlagAprobacionSol = false
                            };

                            ctx.SolicitudAplicacion.Add(objNewSolMod);
                            ctx.SaveChanges();

                            ID = objNewSolMod.AplicacionId;

                            //Creacion de la solicitud de tipo modificacion
                            var newSolMod = new Solicitud()
                            {
                                TipoSolicitud = (int)ETipoSolicitudAplicacion.ModificacionAplicacion,
                                UsuarioCreacion = objeto.UsuarioCreacion,
                                FechaCreacion = FECHA_ACTUAL,
                                AplicacionId = objeto.Id,
                                EstadoSolicitud = objeto.AplicacionDetalle.EstadoSolicitudId,
                                FlagAprobacion = false,
                                Observaciones = objeto.MotivoComentario
                            };
                            ctx.Solicitud.Add(newSolMod);
                            ctx.SaveChanges();

                            SOLICITUD_ID = newSolMod.SolicitudAplicacionId;
                        }
                        break;
                }

                return ID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RegistroEdicionSolicitudAplicacionDetalle(GestionCMDB_ProdEntities ctx, AplicacionDTO objeto, long ID)
        {
            try
            {
                DateTime FECHA_ACTUAL = DateTime.Now;
                switch (objeto.TipoSolicitudId)
                {
                    case (int)ETipoSolicitudAplicacion.CreacionAplicacion:
                        if (objeto.AplicacionDetalle != null)
                        {
                            var objAD = objeto.AplicacionDetalle;
                            if (objAD.Id == 0)
                            {
                                var entidadAD = new AplicacionDetalle()
                                {
                                    AplicacionId = Convert.ToInt32(ID),
                                    MotivoCreacion = objAD.MotivoCreacion,
                                    PersonaSolicitud = objAD.PersonaSolicitud,
                                    ModeloEntrega = objAD.ModeloEntrega,
                                    PlataformaBCP = objAD.PlataformaBCP,
                                    EntidadUso = objAD.EntidadUso,
                                    Proveedor = objAD.Proveedor,
                                    Ubicacion = objAD.Ubicacion,
                                    Infraestructura = objAD.Infraestructura,
                                    RutaRepositorio = objAD.RutaRepositorio,
                                    Contingencia = objAD.Contingencia,
                                    MetodoAutenticacion = objAD.MetodoAutenticacion,
                                    MetodoAutorizacion = objAD.MetodoAutorizacion,
                                    AmbienteInstalacion = objAD.AmbienteInstalacion,
                                    GrupoServiceDesk = objAD.GrupoServiceDesk,
                                    FlagOOR = objAD.FlagOOR,
                                    FlagRatificaOOR = objAD.FlagRatificaOOR,
                                    AplicacionReemplazo = objAD.AplicacionReemplazo,
                                    TipoDesarrollo = objAD.TipoDesarrollo,
                                    FechaSolicitud = FECHA_ACTUAL,//objAD.FechaSolicitud,
                                    EstadoSolicitudId = objAD.EstadoSolicitudId,
                                    FlagActivo = true,
                                    UsuarioCreacion = objAD.UsuarioCreacion,
                                    FechaCreacion = FECHA_ACTUAL,
                                    //campos nuevos
                                    CodigoInterfaz = objAD.CodigoInterfaz,
                                    InterfazApp = objAD.InterfazApp,
                                    NombreServidor = objAD.NombreServidor,
                                    CompatibilidadWindows = objAD.CompatibilidadWindows,
                                    CompatibilidadNavegador = objAD.CompatibilidadNavegador,
                                    CompatibilidadHV = objAD.CompatibilidadHV,
                                    InstaladaDesarrollo = objAD.InstaladaDesarrollo,
                                    InstaladaCertificacion = objAD.InstaladaCertificacion,
                                    InstaladaProduccion = objAD.InstaladaProduccion,
                                    GrupoTicketRemedy = objAD.GrupoTicketRemedy,
                                    NCET = objAD.NCET,
                                    NCLS = objAD.NCLS,
                                    NCG = objAD.NCG,
                                    ResumenSeguridadInformacion = objAD.ResumenSeguridadInformacion,
                                    ProcesoClave = objAD.ProcesoClave,
                                    Confidencialidad = objAD.Confidencialidad,
                                    Integridad = objAD.Integridad,
                                    Disponibilidad = objAD.Disponibilidad,
                                    Privacidad = objAD.Privacidad,
                                    Clasificacion = objAD.Clasificacion,
                                    RoadmapPlanificado = objAD.RoadmapPlanificado,
                                    DetalleEstrategia = objAD.DetalleEstrategia,
                                    EstadoRoadmap = objAD.EstadoRoadmap,
                                    EtapaAtencion = objAD.EtapaAtencion,
                                    RoadmapEjecutado = objAD.RoadmapEjecutado,
                                    FechaInicioRoadmap = objAD.FechaInicioRoadmap,
                                    FechaFinRoadmap = objAD.FechaFinRoadmap,
                                    CodigoAppReemplazo = objAD.CodigoAppReemplazo,
                                    SWBase_SO = objAD.SWBase_SO,
                                    SWBase_HP = objAD.SWBase_HP,
                                    SWBase_LP = objAD.SWBase_LP,
                                    SWBase_BD = objAD.SWBase_BD,
                                    SWBase_Framework = objAD.SWBase_Framework,
                                    RET = objAD.RET,
                                    CriticidadAplicacionBIA = objAD.CriticidadAplicacionBIA,
                                    ProductoMasRepresentativo = objAD.ProductoMasRepresentativo,
                                    MenorRTO = objAD.MenorRTO,
                                    MayorGradoInterrupcion = objAD.MayorGradoInterrupcion,
                                    FlagFileCheckList = objAD.FlagFileCheckList,
                                    FlagFileMatriz = objAD.FlagFileMatriz,
                                    GestorAplicacionCTR = objAD.GestorAplicacionCTR,
                                    ConsultorCTR = objAD.ConsultorCTR,
                                    ValorL_NC = objAD.ValorL_NC,
                                    ValorM_NC = objAD.ValorM_NC,
                                    ValorN_NC = objAD.ValorN_NC,
                                    ValorPC_NC = objAD.ValorPC_NC,
                                    UnidadUsuario = objAD.UnidadUsuario
                                };

                                ctx.AplicacionDetalle.Add(entidadAD);
                                ctx.SaveChanges();
                            }
                            else
                            {
                                var entidadAD = (from u in ctx.AplicacionDetalle
                                                 where u.AplicacionDetalleId == objAD.Id
                                                 select u).FirstOrDefault();

                                if (entidadAD != null)
                                {
                                    entidadAD.FechaModificacion = FECHA_ACTUAL;
                                    entidadAD.UsuarioModificacion = objAD.UsuarioModificacion;
                                    entidadAD.MotivoCreacion = objAD.MotivoCreacion;
                                    entidadAD.PersonaSolicitud = objAD.PersonaSolicitud;
                                    entidadAD.ModeloEntrega = objAD.ModeloEntrega;
                                    entidadAD.PlataformaBCP = objAD.PlataformaBCP;
                                    entidadAD.EntidadUso = objAD.EntidadUso;
                                    entidadAD.Proveedor = objAD.Proveedor;
                                    entidadAD.Ubicacion = objAD.Ubicacion;
                                    entidadAD.Infraestructura = objAD.Infraestructura;
                                    entidadAD.RutaRepositorio = objAD.RutaRepositorio;
                                    entidadAD.Contingencia = objAD.Contingencia;
                                    entidadAD.MetodoAutenticacion = objAD.MetodoAutenticacion;
                                    entidadAD.MetodoAutorizacion = objAD.MetodoAutorizacion;
                                    entidadAD.AmbienteInstalacion = objAD.AmbienteInstalacion;
                                    entidadAD.GrupoServiceDesk = objAD.GrupoServiceDesk;
                                    entidadAD.FlagOOR = objAD.FlagOOR;
                                    entidadAD.FlagRatificaOOR = objAD.FlagRatificaOOR;
                                    entidadAD.AplicacionReemplazo = objAD.AplicacionReemplazo;
                                    entidadAD.TipoDesarrollo = objAD.TipoDesarrollo;

                                    entidadAD.CodigoInterfaz = objAD.CodigoInterfaz;
                                    entidadAD.InterfazApp = objAD.InterfazApp;
                                    entidadAD.NombreServidor = objAD.NombreServidor;
                                    entidadAD.CompatibilidadWindows = objAD.CompatibilidadWindows;
                                    entidadAD.CompatibilidadNavegador = objAD.CompatibilidadNavegador;
                                    entidadAD.CompatibilidadHV = objAD.CompatibilidadHV;
                                    entidadAD.InstaladaDesarrollo = objAD.InstaladaDesarrollo;
                                    entidadAD.InstaladaCertificacion = objAD.InstaladaCertificacion;
                                    entidadAD.InstaladaProduccion = objAD.InstaladaProduccion;
                                    entidadAD.GrupoTicketRemedy = objAD.GrupoTicketRemedy;
                                    entidadAD.NCET = objAD.NCET;
                                    entidadAD.NCLS = objAD.NCLS;
                                    entidadAD.NCG = objAD.NCG;
                                    entidadAD.ResumenSeguridadInformacion = objAD.ResumenSeguridadInformacion;
                                    entidadAD.ProcesoClave = objAD.ProcesoClave;
                                    entidadAD.Confidencialidad = objAD.Confidencialidad;
                                    entidadAD.Integridad = objAD.Integridad;
                                    entidadAD.Disponibilidad = objAD.Disponibilidad;
                                    entidadAD.Privacidad = objAD.Privacidad;
                                    entidadAD.Clasificacion = objAD.Clasificacion;
                                    entidadAD.RoadmapPlanificado = objAD.RoadmapPlanificado;
                                    entidadAD.DetalleEstrategia = objAD.DetalleEstrategia;
                                    entidadAD.EstadoRoadmap = objAD.EstadoRoadmap;
                                    entidadAD.EtapaAtencion = objAD.EtapaAtencion;
                                    entidadAD.RoadmapEjecutado = objAD.RoadmapEjecutado;
                                    entidadAD.FechaInicioRoadmap = objAD.FechaInicioRoadmap;
                                    entidadAD.FechaFinRoadmap = objAD.FechaFinRoadmap;
                                    entidadAD.CodigoAppReemplazo = objAD.CodigoAppReemplazo;

                                    entidadAD.SWBase_SO = objAD.SWBase_SO;
                                    entidadAD.SWBase_HP = objAD.SWBase_HP;
                                    entidadAD.SWBase_LP = objAD.SWBase_LP;
                                    entidadAD.SWBase_BD = objAD.SWBase_BD;
                                    entidadAD.SWBase_Framework = objAD.SWBase_Framework;
                                    entidadAD.RET = objAD.RET;

                                    entidadAD.CriticidadAplicacionBIA = objAD.CriticidadAplicacionBIA;
                                    entidadAD.ProductoMasRepresentativo = objAD.ProductoMasRepresentativo;
                                    entidadAD.MenorRTO = objAD.MenorRTO;
                                    entidadAD.MayorGradoInterrupcion = objAD.MayorGradoInterrupcion;

                                    entidadAD.FlagFileCheckList = objAD.FlagFileCheckList;
                                    entidadAD.FlagFileMatriz = objAD.FlagFileMatriz;

                                    entidadAD.GestorAplicacionCTR = objAD.GestorAplicacionCTR;
                                    entidadAD.ConsultorCTR = objAD.ConsultorCTR;
                                    entidadAD.ValorL_NC = objAD.ValorL_NC;
                                    entidadAD.ValorM_NC = objAD.ValorM_NC;
                                    entidadAD.ValorN_NC = objAD.ValorN_NC;
                                    entidadAD.ValorPC_NC = objAD.ValorPC_NC;
                                    entidadAD.UnidadUsuario = objAD.UnidadUsuario;

                                    ctx.SaveChanges();
                                }
                            }
                        }

                        break;
                    case (int)ETipoSolicitudAplicacion.ModificacionAplicacion:
                        if (objeto.AplicacionDetalle != null)
                        {
                            var objAD = objeto.AplicacionDetalle;
                            var entidadAD = (from u in ctx.SolicitudAplicacionDetalle
                                             where u.AplicacionDetalleId == objAD.Id
                                             select u).FirstOrDefault();

                            if (entidadAD != null)
                            {
                                entidadAD.FechaModificacion = FECHA_ACTUAL;
                                entidadAD.UsuarioModificacion = objAD.UsuarioModificacion;
                                entidadAD.MotivoCreacion = objAD.MotivoCreacion;
                                entidadAD.PersonaSolicitud = objAD.PersonaSolicitud;
                                entidadAD.ModeloEntrega = objAD.ModeloEntrega;
                                entidadAD.PlataformaBCP = objAD.PlataformaBCP;
                                entidadAD.EntidadUso = objAD.EntidadUso;
                                entidadAD.Proveedor = objAD.Proveedor;
                                entidadAD.Ubicacion = objAD.Ubicacion;
                                entidadAD.Infraestructura = objAD.Infraestructura;
                                entidadAD.RutaRepositorio = objAD.RutaRepositorio;
                                entidadAD.Contingencia = objAD.Contingencia;
                                entidadAD.MetodoAutenticacion = objAD.MetodoAutenticacion;
                                entidadAD.MetodoAutorizacion = objAD.MetodoAutorizacion;
                                entidadAD.AmbienteInstalacion = objAD.AmbienteInstalacion;
                                entidadAD.GrupoServiceDesk = objAD.GrupoServiceDesk;
                                entidadAD.FlagOOR = objAD.FlagOOR;
                                entidadAD.FlagRatificaOOR = objAD.FlagRatificaOOR;
                                entidadAD.AplicacionReemplazo = objAD.AplicacionReemplazo;
                                entidadAD.TipoDesarrollo = objAD.TipoDesarrollo;

                                entidadAD.CodigoInterfaz = objAD.CodigoInterfaz;
                                entidadAD.InterfazApp = objAD.InterfazApp;
                                entidadAD.NombreServidor = objAD.NombreServidor;
                                entidadAD.CompatibilidadWindows = objAD.CompatibilidadWindows;
                                entidadAD.CompatibilidadNavegador = objAD.CompatibilidadNavegador;
                                entidadAD.CompatibilidadHV = objAD.CompatibilidadHV;
                                entidadAD.InstaladaDesarrollo = objAD.InstaladaDesarrollo;
                                entidadAD.InstaladaCertificacion = objAD.InstaladaCertificacion;
                                entidadAD.InstaladaProduccion = objAD.InstaladaProduccion;
                                entidadAD.GrupoTicketRemedy = objAD.GrupoTicketRemedy;
                                entidadAD.NCET = objAD.NCET;
                                entidadAD.NCLS = objAD.NCLS;
                                entidadAD.NCG = objAD.NCG;
                                entidadAD.ResumenSeguridadInformacion = objAD.ResumenSeguridadInformacion;
                                entidadAD.ProcesoClave = objAD.ProcesoClave;
                                entidadAD.Confidencialidad = objAD.Confidencialidad;
                                entidadAD.Integridad = objAD.Integridad;
                                entidadAD.Disponibilidad = objAD.Disponibilidad;
                                entidadAD.Privacidad = objAD.Privacidad;
                                entidadAD.Clasificacion = objAD.Clasificacion;
                                entidadAD.RoadmapPlanificado = objAD.RoadmapPlanificado;
                                entidadAD.DetalleEstrategia = objAD.DetalleEstrategia;
                                entidadAD.EstadoRoadmap = objAD.EstadoRoadmap;
                                entidadAD.EtapaAtencion = objAD.EtapaAtencion;
                                entidadAD.RoadmapEjecutado = objAD.RoadmapEjecutado;
                                entidadAD.FechaInicioRoadmap = objAD.FechaInicioRoadmap;
                                entidadAD.FechaFinRoadmap = objAD.FechaFinRoadmap;
                                entidadAD.CodigoAppReemplazo = objAD.CodigoAppReemplazo;

                                entidadAD.SWBase_SO = objAD.SWBase_SO;
                                entidadAD.SWBase_HP = objAD.SWBase_HP;
                                entidadAD.SWBase_LP = objAD.SWBase_LP;
                                entidadAD.SWBase_BD = objAD.SWBase_BD;
                                entidadAD.SWBase_Framework = objAD.SWBase_Framework;
                                entidadAD.RET = objAD.RET;

                                entidadAD.CriticidadAplicacionBIA = objAD.CriticidadAplicacionBIA;
                                entidadAD.ProductoMasRepresentativo = objAD.ProductoMasRepresentativo;
                                entidadAD.MenorRTO = objAD.MenorRTO;
                                entidadAD.MayorGradoInterrupcion = objAD.MayorGradoInterrupcion;

                                entidadAD.FlagFileCheckList = objAD.FlagFileCheckList;
                                entidadAD.FlagFileMatriz = objAD.FlagFileMatriz;

                                entidadAD.GestorAplicacionCTR = objAD.GestorAplicacionCTR;
                                entidadAD.ConsultorCTR = objAD.ConsultorCTR;
                                entidadAD.ValorL_NC = objAD.ValorL_NC;
                                entidadAD.ValorM_NC = objAD.ValorM_NC;
                                entidadAD.ValorN_NC = objAD.ValorN_NC;
                                entidadAD.ValorPC_NC = objAD.ValorPC_NC;
                                entidadAD.UnidadUsuario = objAD.UnidadUsuario;

                                ctx.SaveChanges();
                            }
                            else
                            {
                                var newEntidadAD = new SolicitudAplicacionDetalle()
                                {
                                    AplicacionDetalleId = objAD.Id,
                                    AplicacionId = Convert.ToInt32(ID),
                                    MotivoCreacion = objAD.MotivoCreacion,
                                    PersonaSolicitud = objAD.PersonaSolicitud,
                                    ModeloEntrega = objAD.ModeloEntrega,
                                    PlataformaBCP = objAD.PlataformaBCP,
                                    EntidadUso = objAD.EntidadUso,
                                    Proveedor = objAD.Proveedor,
                                    Ubicacion = objAD.Ubicacion,
                                    Infraestructura = objAD.Infraestructura,
                                    RutaRepositorio = objAD.RutaRepositorio,
                                    Contingencia = objAD.Contingencia,
                                    MetodoAutenticacion = objAD.MetodoAutenticacion,
                                    MetodoAutorizacion = objAD.MetodoAutorizacion,
                                    AmbienteInstalacion = objAD.AmbienteInstalacion,
                                    GrupoServiceDesk = objAD.GrupoServiceDesk,
                                    FlagOOR = objAD.FlagOOR,
                                    FlagRatificaOOR = objAD.FlagRatificaOOR,
                                    AplicacionReemplazo = objAD.AplicacionReemplazo,
                                    TipoDesarrollo = objAD.TipoDesarrollo,
                                    FechaSolicitud = FECHA_ACTUAL,//objAD.FechaSolicitud,
                                    EstadoSolicitudId = objAD.EstadoSolicitudId,
                                    FlagActivo = true,
                                    UsuarioCreacion = objAD.UsuarioCreacion,
                                    FechaCreacion = FECHA_ACTUAL,
                                    //campos nuevos
                                    CodigoInterfaz = objAD.CodigoInterfaz,
                                    InterfazApp = objAD.InterfazApp,
                                    NombreServidor = objAD.NombreServidor,
                                    CompatibilidadWindows = objAD.CompatibilidadWindows,
                                    CompatibilidadNavegador = objAD.CompatibilidadNavegador,
                                    CompatibilidadHV = objAD.CompatibilidadHV,
                                    InstaladaDesarrollo = objAD.InstaladaDesarrollo,
                                    InstaladaCertificacion = objAD.InstaladaCertificacion,
                                    InstaladaProduccion = objAD.InstaladaProduccion,
                                    GrupoTicketRemedy = objAD.GrupoTicketRemedy,
                                    NCET = objAD.NCET,
                                    NCLS = objAD.NCLS,
                                    NCG = objAD.NCG,
                                    ResumenSeguridadInformacion = objAD.ResumenSeguridadInformacion,
                                    ProcesoClave = objAD.ProcesoClave,
                                    Confidencialidad = objAD.Confidencialidad,
                                    Integridad = objAD.Integridad,
                                    Disponibilidad = objAD.Disponibilidad,
                                    Privacidad = objAD.Privacidad,
                                    Clasificacion = objAD.Clasificacion,
                                    RoadmapPlanificado = objAD.RoadmapPlanificado,
                                    DetalleEstrategia = objAD.DetalleEstrategia,
                                    EstadoRoadmap = objAD.EstadoRoadmap,
                                    EtapaAtencion = objAD.EtapaAtencion,
                                    RoadmapEjecutado = objAD.RoadmapEjecutado,
                                    FechaInicioRoadmap = objAD.FechaInicioRoadmap,
                                    FechaFinRoadmap = objAD.FechaFinRoadmap,
                                    CodigoAppReemplazo = objAD.CodigoAppReemplazo,
                                    SWBase_SO = objAD.SWBase_SO,
                                    SWBase_HP = objAD.SWBase_HP,
                                    SWBase_LP = objAD.SWBase_LP,
                                    SWBase_BD = objAD.SWBase_BD,
                                    SWBase_Framework = objAD.SWBase_Framework,
                                    RET = objAD.RET,
                                    CriticidadAplicacionBIA = objAD.CriticidadAplicacionBIA,
                                    ProductoMasRepresentativo = objAD.ProductoMasRepresentativo,
                                    MenorRTO = objAD.MenorRTO,
                                    MayorGradoInterrupcion = objAD.MayorGradoInterrupcion,
                                    FlagFileCheckList = objAD.FlagFileCheckList,
                                    FlagFileMatriz = objAD.FlagFileMatriz,
                                    GestorAplicacionCTR = objAD.GestorAplicacionCTR,
                                    ConsultorCTR = objAD.ConsultorCTR,
                                    ValorL_NC = objAD.ValorL_NC,
                                    ValorM_NC = objAD.ValorM_NC,
                                    ValorN_NC = objAD.ValorN_NC,
                                    ValorPC_NC = objAD.ValorPC_NC,
                                    UnidadUsuario = objAD.UnidadUsuario
                                };

                                ctx.SolicitudAplicacionDetalle.Add(newEntidadAD);
                                ctx.SaveChanges();
                            }
                        }

                        break;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void RegistrarDataBitacora(AplicacionDTO objeto, long ID)
        {
            if (objeto != null)
            {
                if (objeto.Id == 0)
                {
                    ServiceManager<SolicitudAplicacionDAO>.Provider.AddBitacora(ID, objeto.UsuarioCreacion, (int)ETipoBitacora.SolicitudRegistroAplicacion);
                }
                else
                {
                    //Actualización
                    ServiceManager<SolicitudAplicacionDAO>.Provider.AddBitacora(ID, objeto.UsuarioCreacion, (int)ETipoBitacora.ModificacionAplicacion, "creación", null, 0, objeto);
                }
            }
        }

        public override AplicacionDTO GetAplicacionById(int Id, int TipoSolicitudId = (int)ETipoSolicitudAplicacion.CreacionAplicacion)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var entidad = new AplicacionDTO();

                        switch (TipoSolicitudId)
                        {
                            case (int)ETipoSolicitudAplicacion.CreacionAplicacion:
                                entidad = (from u in ctx.Aplicacion
                                           join s in ctx.Solicitud on u.AplicacionId equals s.AplicacionId into lj0
                                           from s in lj0.DefaultIfEmpty()
                                           join ad in ctx.AplicacionDetalle on u.AplicacionId equals ad.AplicacionId into lj1
                                           from ad in lj1.DefaultIfEmpty()
                                           join u2 in ctx.Criticidad on u.CriticidadId equals u2.CriticidadId
                                           join r in ctx.RoadMap on u.RoadMapId equals r.RoadMapId into lj2
                                           from r in lj2.DefaultIfEmpty()
                                           where u.AplicacionId == Id
                                           select new AplicacionDTO()
                                           {
                                               Id = u.AplicacionId,
                                               CodigoAPT = u.CodigoAPT,
                                               CodigoAPTStr = u.CodigoAPT,
                                               Nombre = u.Nombre,
                                               DescripcionAplicacion = u.DescripcionAplicacion,
                                               GestionadoPor = u.GestionadoPor,
                                               CriticidadId = u.CriticidadId,
                                               RoadMapId = u.RoadMapId,
                                               RoadMapToString = r.Nombre,
                                               Matricula = u.Matricula,
                                               Obsolescente = u.Obsolescente,
                                               CategoriaTecnologica = u.CategoriaTecnologica,
                                               //MesAnio = u.MesAnio,
                                               Activo = u.FlagActivo,
                                               UsuarioCreacion = u.CreadoPor,
                                               FechaCreacion = u.FechaCreacion,
                                               FechaRegistroProcedencia = u.FechaRegistroProcedencia,
                                               FechaModificacion = u.FechaModificacion,
                                               UsuarioModificacion = u.ModificadoPor,
                                               FlagRelacionar = u.FlagRelacionar,
                                               CriticidadToString = u2.DetalleCriticidad,
                                               TipoActivoInformacion = u.TipoActivoInformacion,
                                               GerenciaCentral = u.GerenciaCentral,
                                               Division = u.Division,
                                               Unidad = u.Unidad,
                                               Area = u.Area,
                                               AreaBIAN = u.AreaBIAN,
                                               DominioBIAN = u.DominioBIAN,
                                               JefaturaATI = u.JefaturaATI,
                                               NombreEquipo_Squad = u.NombreEquipo_Squad,
                                               TribeTechnicalLead = u.TribeTechnicalLead,
                                               BrokerSistemas = u.BrokerSistemas,
                                               Owner_LiderUsuario_ProductOwner = u.Owner_LiderUsuario_ProductOwner,
                                               JefeEquipo_ExpertoAplicacionUserIT_ProductOwner = u.JefeEquipo_ExpertoAplicacionUserIT_ProductOwner,
                                               Gestor_UsuarioAutorizador_ProductOwner = u.Gestor_UsuarioAutorizador_ProductOwner,
                                               Experto_Especialista = u.Experto_Especialista,
                                               EstadoAplicacion = u.EstadoAplicacion,
                                               EntidadResponsable = u.EntidadResponsable,
                                               SolicitudId = s != null ? s.SolicitudAplicacionId : 0,
                                               FlagAprobado = u.FlagAprobado,
                                               ClasificacionTecnica = u.ClasificacionTecnica,
                                               SubclasificacionTecnica = u.SubclasificacionTecnica,
                                               ArquitectoTI = u.ArquitectoTI,
                                               GestorUserIT = u.GestorUserIT,
                                               FechaCreacionAplicacion = u.FechaCreacionAplicacion,
                                               AplicacionDetalle = new AplicacionDetalleDTO()
                                               {
                                                   Id = ad != null ? ad.AplicacionDetalleId : 0,
                                                   EstadoSolicitudId = ad != null ? (ad.EstadoSolicitudId.HasValue ? ad.EstadoSolicitudId.Value : (int)EEstadoSolicitudAplicacion.Aprobado) : (int)EEstadoSolicitudAplicacion.Aprobado,
                                                   MotivoCreacion = ad != null ? ad.MotivoCreacion : "-1",
                                                   AmbienteInstalacion = ad != null ? ad.AmbienteInstalacion : "",
                                                   AplicacionReemplazo = ad != null ? ad.AplicacionReemplazo : "",
                                                   Contingencia = ad != null ? ad.Contingencia : "-1",
                                                   EntidadUso = ad != null ? ad.EntidadUso : "",
                                                   FechaSolicitud = ad != null ? ad.FechaSolicitud : u.FechaCreacion,
                                                   FlagOOR = ad != null ? ad.FlagOOR : null,
                                                   FlagRatificaOOR = ad.FlagRatificaOOR,
                                                   GrupoServiceDesk = ad != null ? ad.GrupoServiceDesk : "",
                                                   Infraestructura = ad != null ? ad.Infraestructura : "-1",
                                                   MetodoAutenticacion = ad != null ? ad.MetodoAutenticacion : "-1",
                                                   MetodoAutorizacion = ad != null ? ad.MetodoAutorizacion : "-1",
                                                   ModeloEntrega = ad != null ? ad.ModeloEntrega : "-1",
                                                   PersonaSolicitud = ad != null ? ad.PersonaSolicitud : "",
                                                   PlataformaBCP = ad != null ? ad.PlataformaBCP : "-1",
                                                   Proveedor = ad != null ? ad.Proveedor : "",
                                                   RutaRepositorio = ad != null ? ad.RutaRepositorio : "",
                                                   TipoDesarrollo = ad != null ? ad.TipoDesarrollo : "-1",
                                                   Ubicacion = ad != null ? ad.Ubicacion : "-1",
                                                   //campos nuevos
                                                   CodigoInterfaz = ad != null ? ad.CodigoInterfaz : "",
                                                   InterfazApp = ad != null ? ad.InterfazApp : "",
                                                   NombreServidor = ad != null ? ad.NombreServidor : "",
                                                   CompatibilidadWindows = ad != null ? ad.CompatibilidadWindows : "-1",
                                                   CompatibilidadNavegador = ad != null ? ad.CompatibilidadNavegador : "-1",
                                                   CompatibilidadHV = ad != null ? ad.CompatibilidadHV : "-1",
                                                   InstaladaDesarrollo = ad != null ? ad.InstaladaDesarrollo : "-1",
                                                   InstaladaCertificacion = ad != null ? ad.InstaladaCertificacion : "-1",
                                                   InstaladaProduccion = ad != null ? ad.InstaladaProduccion : "-1",
                                                   GrupoTicketRemedy = ad != null ? ad.GrupoTicketRemedy : "",
                                                   NCET = ad != null ? ad.NCET : "",
                                                   NCLS = ad != null ? ad.NCLS : "",
                                                   NCG = ad != null ? ad.NCG : "",
                                                   ResumenSeguridadInformacion = ad != null ? ad.ResumenSeguridadInformacion : "",
                                                   ProcesoClave = ad != null ? ad.ProcesoClave : "",
                                                   Confidencialidad = ad != null ? ad.Confidencialidad : "-1",
                                                   Integridad = ad != null ? ad.Integridad : "-1",
                                                   Disponibilidad = ad != null ? ad.Disponibilidad : "-1",
                                                   Privacidad = ad != null ? ad.Privacidad : "-1",
                                                   Clasificacion = ad != null ? ad.Clasificacion : "-1",
                                                   RoadmapPlanificado = ad != null ? ad.RoadmapPlanificado : "",
                                                   DetalleEstrategia = ad != null ? ad.DetalleEstrategia : "",
                                                   EstadoRoadmap = ad != null ? ad.EstadoRoadmap : "-1",
                                                   EtapaAtencion = ad != null ? ad.EtapaAtencion : "",
                                                   RoadmapEjecutado = ad != null ? ad.RoadmapEjecutado : "",
                                                   FechaInicioRoadmap = ad != null ? ad.FechaInicioRoadmap : "",
                                                   FechaFinRoadmap = ad != null ? ad.FechaFinRoadmap : "",
                                                   CodigoAppReemplazo = ad != null ? ad.CodigoAppReemplazo : "",
                                                   SWBase_SO = ad != null ? ad.SWBase_SO : "",
                                                   SWBase_HP = ad != null ? ad.SWBase_HP : "",
                                                   SWBase_BD = ad != null ? ad.SWBase_BD : "",
                                                   SWBase_LP = ad != null ? ad.SWBase_LP : "",
                                                   SWBase_Framework = ad != null ? ad.SWBase_Framework : "",
                                                   RET = ad != null ? ad.RET : "",
                                                   CriticidadAplicacionBIA = ad != null ? ad.CriticidadAplicacionBIA : "",
                                                   ProductoMasRepresentativo = ad != null ? ad.ProductoMasRepresentativo : "",
                                                   MenorRTO = ad != null ? ad.MenorRTO : "",
                                                   MayorGradoInterrupcion = ad != null ? ad.MayorGradoInterrupcion : "",
                                                   FlagFileCheckList = ad != null ? ad.FlagFileCheckList : false,
                                                   FlagFileMatriz = ad != null ? ad.FlagFileMatriz : false,

                                                   GestorAplicacionCTR = ad != null ? ad.GestorAplicacionCTR : "",
                                                   ConsultorCTR = ad != null ? ad.ConsultorCTR : "",
                                                   ValorL_NC = ad != null ? ad.ValorL_NC : "|",
                                                   ValorM_NC = ad != null ? ad.ValorM_NC : "|",
                                                   ValorN_NC = ad != null ? ad.ValorN_NC : "",
                                                   ValorPC_NC = ad != null ? ad.ValorPC_NC : "",
                                                   UnidadUsuario = ad != null ? ad.UnidadUsuario : "",
                                               }
                                           }).FirstOrDefault();
                                break;
                            case (int)ETipoSolicitudAplicacion.ModificacionAplicacion:
                                entidad = (from u in ctx.SolicitudAplicacion
                                           join s in ctx.Solicitud on u.AplicacionId equals s.AplicacionId into lj0
                                           from s in lj0.DefaultIfEmpty()
                                           join ad in ctx.SolicitudAplicacionDetalle on u.AplicacionId equals ad.AplicacionId into lj1
                                           from ad in lj1.DefaultIfEmpty()
                                           join u2 in ctx.Criticidad on u.CriticidadId equals u2.CriticidadId
                                           join r in ctx.RoadMap on u.RoadMapId equals r.RoadMapId into lj2
                                           from r in lj2.DefaultIfEmpty()
                                           where u.AplicacionId == Id
                                           select new AplicacionDTO()
                                           {
                                               Id = u.AplicacionId,
                                               CodigoAPT = u.CodigoAPT,
                                               CodigoAPTStr = u.CodigoAPT,
                                               Nombre = u.Nombre,
                                               DescripcionAplicacion = u.DescripcionAplicacion,
                                               GestionadoPor = u.GestionadoPor,
                                               CriticidadId = u.CriticidadId,
                                               RoadMapId = u.RoadMapId,
                                               RoadMapToString = r.Nombre,
                                               Matricula = u.Matricula,
                                               Obsolescente = u.Obsolescente,
                                               CategoriaTecnologica = u.CategoriaTecnologica,
                                               //MesAnio = u.MesAnio,
                                               Activo = u.FlagActivo,
                                               UsuarioCreacion = u.CreadoPor,
                                               FechaCreacion = u.FechaCreacion,
                                               FechaRegistroProcedencia = u.FechaRegistroProcedencia,
                                               FechaModificacion = u.FechaModificacion,
                                               UsuarioModificacion = u.ModificadoPor,
                                               FlagRelacionar = u.FlagRelacionar,
                                               CriticidadToString = u2.DetalleCriticidad,
                                               TipoActivoInformacion = u.TipoActivoInformacion,
                                               GerenciaCentral = u.GerenciaCentral,
                                               Division = u.Division,
                                               Unidad = u.Unidad,
                                               Area = u.Area,
                                               AreaBIAN = u.AreaBIAN,
                                               DominioBIAN = u.DominioBIAN,
                                               JefaturaATI = u.JefaturaATI,
                                               NombreEquipo_Squad = u.NombreEquipo_Squad,
                                               TribeTechnicalLead = u.TribeTechnicalLead,
                                               BrokerSistemas = u.BrokerSistemas,
                                               Owner_LiderUsuario_ProductOwner = u.Owner_LiderUsuario_ProductOwner,
                                               JefeEquipo_ExpertoAplicacionUserIT_ProductOwner = u.JefeEquipo_ExpertoAplicacionUserIT_ProductOwner,
                                               Gestor_UsuarioAutorizador_ProductOwner = u.Gestor_UsuarioAutorizador_ProductOwner,
                                               Experto_Especialista = u.Experto_Especialista,
                                               EstadoAplicacion = u.EstadoAplicacion,
                                               EntidadResponsable = u.EntidadResponsable,
                                               SolicitudId = s != null ? s.SolicitudAplicacionId : 0,
                                               FlagAprobado = u.FlagAprobado,
                                               ClasificacionTecnica = u.ClasificacionTecnica,
                                               SubclasificacionTecnica = u.SubclasificacionTecnica,
                                               ArquitectoTI = u.ArquitectoTI,
                                               GestorUserIT = u.GestorUserIT,
                                               FechaCreacionAplicacion = u.FechaCreacionAplicacion,
                                               AplicacionDetalle = new AplicacionDetalleDTO()
                                               {
                                                   Id = ad != null ? ad.AplicacionDetalleId : 0,
                                                   EstadoSolicitudId = ad != null ? (ad.EstadoSolicitudId.HasValue ? ad.EstadoSolicitudId.Value : (int)EEstadoSolicitudAplicacion.Aprobado) : (int)EEstadoSolicitudAplicacion.Aprobado,
                                                   MotivoCreacion = ad != null ? ad.MotivoCreacion : "-1",
                                                   AmbienteInstalacion = ad != null ? ad.AmbienteInstalacion : "",
                                                   AplicacionReemplazo = ad != null ? ad.AplicacionReemplazo : "",
                                                   Contingencia = ad != null ? ad.Contingencia : "-1",
                                                   EntidadUso = ad != null ? ad.EntidadUso : "",
                                                   FechaSolicitud = ad != null ? ad.FechaSolicitud : u.FechaCreacion,
                                                   FlagOOR = ad != null ? ad.FlagOOR : null,
                                                   FlagRatificaOOR = ad.FlagRatificaOOR,
                                                   GrupoServiceDesk = ad != null ? ad.GrupoServiceDesk : "",
                                                   Infraestructura = ad != null ? ad.Infraestructura : "-1",
                                                   MetodoAutenticacion = ad != null ? ad.MetodoAutenticacion : "-1",
                                                   MetodoAutorizacion = ad != null ? ad.MetodoAutorizacion : "-1",
                                                   ModeloEntrega = ad != null ? ad.ModeloEntrega : "-1",
                                                   PersonaSolicitud = ad != null ? ad.PersonaSolicitud : "",
                                                   PlataformaBCP = ad != null ? ad.PlataformaBCP : "-1",
                                                   Proveedor = ad != null ? ad.Proveedor : "",
                                                   RutaRepositorio = ad != null ? ad.RutaRepositorio : "",
                                                   TipoDesarrollo = ad != null ? ad.TipoDesarrollo : "-1",
                                                   Ubicacion = ad != null ? ad.Ubicacion : "-1",
                                                   //campos nuevos
                                                   CodigoInterfaz = ad != null ? ad.CodigoInterfaz : "",
                                                   InterfazApp = ad != null ? ad.InterfazApp : "",
                                                   NombreServidor = ad != null ? ad.NombreServidor : "",
                                                   CompatibilidadWindows = ad != null ? ad.CompatibilidadWindows : "-1",
                                                   CompatibilidadNavegador = ad != null ? ad.CompatibilidadNavegador : "-1",
                                                   CompatibilidadHV = ad != null ? ad.CompatibilidadHV : "-1",
                                                   InstaladaDesarrollo = ad != null ? ad.InstaladaDesarrollo : "-1",
                                                   InstaladaCertificacion = ad != null ? ad.InstaladaCertificacion : "-1",
                                                   InstaladaProduccion = ad != null ? ad.InstaladaProduccion : "-1",
                                                   GrupoTicketRemedy = ad != null ? ad.GrupoTicketRemedy : "",
                                                   NCET = ad != null ? ad.NCET : "",
                                                   NCLS = ad != null ? ad.NCLS : "",
                                                   NCG = ad != null ? ad.NCG : "",
                                                   ResumenSeguridadInformacion = ad != null ? ad.ResumenSeguridadInformacion : "",
                                                   ProcesoClave = ad != null ? ad.ProcesoClave : "",
                                                   Confidencialidad = ad != null ? ad.Confidencialidad : "-1",
                                                   Integridad = ad != null ? ad.Integridad : "-1",
                                                   Disponibilidad = ad != null ? ad.Disponibilidad : "-1",
                                                   Privacidad = ad != null ? ad.Privacidad : "-1",
                                                   Clasificacion = ad != null ? ad.Clasificacion : "-1",
                                                   RoadmapPlanificado = ad != null ? ad.RoadmapPlanificado : "",
                                                   DetalleEstrategia = ad != null ? ad.DetalleEstrategia : "",
                                                   EstadoRoadmap = ad != null ? ad.EstadoRoadmap : "-1",
                                                   EtapaAtencion = ad != null ? ad.EtapaAtencion : "",
                                                   RoadmapEjecutado = ad != null ? ad.RoadmapEjecutado : "",
                                                   FechaInicioRoadmap = ad != null ? ad.FechaInicioRoadmap : "",
                                                   FechaFinRoadmap = ad != null ? ad.FechaFinRoadmap : "",
                                                   CodigoAppReemplazo = ad != null ? ad.CodigoAppReemplazo : "",
                                                   SWBase_SO = ad != null ? ad.SWBase_SO : "",
                                                   SWBase_HP = ad != null ? ad.SWBase_HP : "",
                                                   SWBase_BD = ad != null ? ad.SWBase_BD : "",
                                                   SWBase_LP = ad != null ? ad.SWBase_LP : "",
                                                   SWBase_Framework = ad != null ? ad.SWBase_Framework : "",
                                                   RET = ad != null ? ad.RET : "",
                                                   CriticidadAplicacionBIA = ad != null ? ad.CriticidadAplicacionBIA : "",
                                                   ProductoMasRepresentativo = ad != null ? ad.ProductoMasRepresentativo : "",
                                                   MenorRTO = ad != null ? ad.MenorRTO : "",
                                                   MayorGradoInterrupcion = ad != null ? ad.MayorGradoInterrupcion : "",
                                                   FlagFileCheckList = ad != null ? ad.FlagFileCheckList : false,
                                                   FlagFileMatriz = ad != null ? ad.FlagFileMatriz : false,

                                                   GestorAplicacionCTR = ad != null ? ad.GestorAplicacionCTR : "",
                                                   ConsultorCTR = ad != null ? ad.ConsultorCTR : "",
                                                   ValorL_NC = ad != null ? ad.ValorL_NC : "|",
                                                   ValorM_NC = ad != null ? ad.ValorM_NC : "|",
                                                   ValorN_NC = ad != null ? ad.ValorN_NC : "",
                                                   ValorPC_NC = ad != null ? ad.ValorPC_NC : "",
                                                   UnidadUsuario = ad != null ? ad.UnidadUsuario : "",
                                               }
                                           }).FirstOrDefault();
                                break;
                        }

                        if (entidad != null)
                        {
                            //Get estado estandares
                            //SO
                            var SW_SO = entidad.AplicacionDetalle.SWBase_SO;
                            if (!string.IsNullOrEmpty(SW_SO))
                            {
                                var itemSO = ctx.Tecnologia.FirstOrDefault(x => x.Activo && x.ClaveTecnologia.ToUpper().Equals(SW_SO.ToUpper()));
                                if (itemSO != null) entidad.AplicacionDetalle.EstadoId_SO = itemSO.EstadoId;
                            }

                            //HP
                            var SW_HP = entidad.AplicacionDetalle.SWBase_HP;
                            if (!string.IsNullOrEmpty(SW_HP))
                            {
                                var itemHP = ctx.Tecnologia.FirstOrDefault(x => x.Activo && x.ClaveTecnologia.ToUpper().Equals(SW_HP.ToUpper()));
                                if (itemHP != null) entidad.AplicacionDetalle.EstadoId_HP = itemHP.EstadoId;
                            }

                            //LP
                            var SW_LP = entidad.AplicacionDetalle.SWBase_LP;
                            if (!string.IsNullOrEmpty(SW_LP))
                            {
                                var itemLP = ctx.Tecnologia.FirstOrDefault(x => x.Activo && x.ClaveTecnologia.ToUpper().Equals(SW_LP.ToUpper()));
                                if (itemLP != null) entidad.AplicacionDetalle.EstadoId_LP = itemLP.EstadoId;
                            }

                            //BD
                            var SW_BD = entidad.AplicacionDetalle.SWBase_BD;
                            if (!string.IsNullOrEmpty(SW_BD))
                            {
                                var itemBD = ctx.Tecnologia.FirstOrDefault(x => x.Activo && x.ClaveTecnologia.ToUpper().Equals(SW_BD.ToUpper()));
                                if (itemBD != null) entidad.AplicacionDetalle.EstadoId_BD = itemBD.EstadoId;
                            }

                            //FW
                            var SW_FW = entidad.AplicacionDetalle.SWBase_Framework;
                            if (!string.IsNullOrEmpty(SW_FW))
                            {
                                var itemFW = ctx.Tecnologia.FirstOrDefault(x => x.Activo && x.ClaveTecnologia.ToUpper().Equals(SW_FW.ToUpper()));
                                if (itemFW != null) entidad.AplicacionDetalle.EstadoId_FW = itemFW.EstadoId;
                            }

                            //Update responsables
                            var responsables = (from x in ctx.AplicacionPortafolioResponsables
                                                where x.CodigoAPT == entidad.CodigoAPT
                                                && x.FlagActivo //&& x.PortafolioResponsableId == (int)EPortafolioResponsable.Experto
                                                select x).ToList();

                            if (responsables != null && responsables.Count > 0)
                            {
                                foreach (var item in responsables)
                                {
                                    switch (item.PortafolioResponsableId)
                                    {
                                        case (int)EPortafolioResponsable.Owner:
                                            if (!string.IsNullOrEmpty(entidad.Owner_LiderUsuario_ProductOwner)
                                                && entidad.Owner_LiderUsuario_ProductOwner != Utilitarios.NO_APLICA)
                                            {
                                                entidad.HdOwner = item.AplicacionPortafolioResponsableId;
                                                entidad.MatriculaOwner = item.Matricula;
                                                entidad.Owner_LiderUsuario_ProductOwner = item.Colaborador;
                                            }
                                            break;
                                        case (int)EPortafolioResponsable.Gestor:
                                            if (!string.IsNullOrEmpty(entidad.Gestor_UsuarioAutorizador_ProductOwner)
                                                && entidad.Gestor_UsuarioAutorizador_ProductOwner != Utilitarios.NO_APLICA)
                                            {
                                                entidad.HdGestor = item.AplicacionPortafolioResponsableId;
                                                entidad.MatriculaGestor = item.Matricula;
                                                entidad.Gestor_UsuarioAutorizador_ProductOwner = item.Colaborador;
                                            }
                                            break;
                                        case (int)EPortafolioResponsable.TTL:
                                            if (!string.IsNullOrEmpty(entidad.TribeTechnicalLead)
                                                && entidad.TribeTechnicalLead != Utilitarios.NO_APLICA)
                                            {
                                                entidad.HdTTL = item.AplicacionPortafolioResponsableId;
                                                entidad.MatriculaTTL = item.Matricula;
                                                entidad.TribeTechnicalLead = item.Colaborador;
                                            }

                                            break;
                                        case (int)EPortafolioResponsable.Experto:
                                            if (!string.IsNullOrEmpty(entidad.Experto_Especialista)
                                                && entidad.Experto_Especialista != Utilitarios.NO_APLICA)
                                            {
                                                entidad.HdExperto = item.AplicacionPortafolioResponsableId;
                                                entidad.MatriculaExperto = item.Matricula;
                                                entidad.Experto_Especialista = item.Colaborador;
                                            }
                                            break;
                                        case (int)EPortafolioResponsable.Broker:
                                            if (!string.IsNullOrEmpty(entidad.BrokerSistemas)
                                                && entidad.BrokerSistemas != Utilitarios.NO_APLICA)
                                            {
                                                entidad.HdBroker = item.AplicacionPortafolioResponsableId;
                                                entidad.MatriculaBroker = item.Matricula;
                                                entidad.BrokerSistemas = item.Colaborador;
                                            }
                                            break;
                                        case (int)EPortafolioResponsable.JdE:
                                            if (!string.IsNullOrEmpty(entidad.JefeEquipo_ExpertoAplicacionUserIT_ProductOwner)
                                                && entidad.JefeEquipo_ExpertoAplicacionUserIT_ProductOwner != Utilitarios.NO_APLICA)
                                            {
                                                entidad.HdJDE = item.AplicacionPortafolioResponsableId;
                                                entidad.MatriculaJDE = item.Matricula;
                                                entidad.JefeEquipo_ExpertoAplicacionUserIT_ProductOwner = item.Colaborador;
                                            }
                                            break;
                                    }
                                }
                            }


                            //Get files
                            int? TablaProcedenciaId = (from t in ctx.TablaProcedencia
                                                       where t.CodigoInterno == (int)ETablaProcedencia.CVT_Aplicacion
                                                       && t.FlagActivo
                                                       select t.TablaProcedenciaId).FirstOrDefault();
                            if (TablaProcedenciaId != null)
                            {
                                var lFiles = (from u in ctx.ArchivosCVT
                                              where u.Activo
                                              && u.EntidadId == entidad.Id.ToString() && u.TablaProcedenciaId == TablaProcedenciaId
                                              select new ArchivosCvtDTO()
                                              {
                                                  Id = u.ArchivoId,
                                                  Nombre = u.Nombre,
                                                  NombreRef = u.NombreRef
                                              }).ToList();

                                if (lFiles != null && lFiles.Count > 0)
                                    entidad.Files = lFiles;
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

        public override int GetEstadoSolicitudAppById(int Id)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {

                        var entidad = (from u in ctx.Aplicacion
                                           //join ad in ctx.AplicacionDetalle on u.AplicacionId equals ad.AplicacionId into lj1
                                           //from ad in lj1.DefaultIfEmpty()
                                       join s in ctx.Solicitud on u.AplicacionId equals s.AplicacionId into lj1
                                       from s in lj1.DefaultIfEmpty()
                                       where u.AplicacionId == Id
                                       && u.FlagActivo
                                       select (int?)s.EstadoSolicitud).FirstOrDefault() ?? 0;
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

        public override bool ExisteAplicacionById(string codigoAPT, int idAplicacion)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    bool? estado = (from u in ctx.Aplicacion
                                    where u.FlagActivo
                                    && u.CodigoAPT.ToUpper().Equals(codigoAPT.ToUpper())
                                    && u.AplicacionId != idAplicacion
                                    orderby u.Nombre
                                    select true).FirstOrDefault();

                    return estado == true;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: bool ExisteAplicacion(string nombre, string Id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: bool ExisteAplicacion(string nombre, string Id)"
                    , new object[] { null });
            }
        }

        public override bool CambiarEstadoApp(int id, int estadoTec, string obs, string usuario)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    //var itemBD = (from u in ctx.Aplicacion
                    //              join ad in ctx.AplicacionDetalle on u.AplicacionId equals ad.AplicacionId into lj1
                    //              from ad in lj1.DefaultIfEmpty()
                    //              where u.AplicacionId == id
                    //              select u).FirstOrDefault();
                    var itemBD = (from u in ctx.AplicacionDetalle
                                  where u.AplicacionId == id
                                  && u.FlagActivo
                                  select u).FirstOrDefault();

                    if (itemBD != null)
                    {
                        itemBD.EstadoSolicitudId = estadoTec;
                        switch (estadoTec)
                        {
                            case (int)EstadoTecnologia.Aprobado:
                                itemBD.FechaAprobacion = DateTime.Now;
                                itemBD.AprobadoPor = usuario;
                                break;

                            case (int)EstadoTecnologia.Observado:
                                itemBD.Observacion = obs;
                                itemBD.ObservadoPor = usuario;
                                break;
                        }

                        itemBD.FechaModificacion = DateTime.Now;
                        itemBD.UsuarioModificacion = usuario;
                        ctx.SaveChanges();

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

        public override bool CambiarEstado(int id, string estado, string usuario)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var itemBD = (from u in ctx.Aplicacion
                                  where u.AplicacionId == id
                                  select u).FirstOrDefault();

                    if (itemBD != null)
                    {
                        itemBD.FechaModificacion = DateTime.Now;
                        itemBD.ModificadoPor = usuario;
                        itemBD.EstadoAplicacion = estado;

                        ctx.SaveChanges();

                        return true;
                    }
                    else
                        return false;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
        }

        public override List<ConfiguracionColumnaAplicacionDTO> GetColumnaAplicacion(PaginacionAplicacion pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                var lstTablaProcedencia = new List<int>();
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        ctx.Database.CommandTimeout = 0;
                        if (!string.IsNullOrEmpty(pag.TablaProcedencia))
                            lstTablaProcedencia = pag.TablaProcedencia.Split(';').Select(int.Parse).ToList();

                        var registros = (from u in ctx.ConfiguracionColumnaAplicacion
                                         where !u.FlagEliminado.Value
                                         && (pag.ColumnaId == -1 || u.ConfiguracionColumnaAplicacionId == pag.ColumnaId)
                                         && (pag.FlagEditar == -1 || u.FlagEdicion.Value == (pag.FlagEditar == 1))
                                         && (pag.FlagVerExportar == -1 || u.FlagVerExportar.Value == (pag.FlagVerExportar == 1))
                                         && (pag.TipoRegistro == 0 || u.TipoRegistro.Value == pag.TipoRegistro)
                                         && (pag.ActivoAplica == 0 || u.ActivoAplica.Value == pag.ActivoAplica)
                                         && (pag.ModoLlenado == 0 || u.ModoLlenado.Value == pag.ModoLlenado)
                                         && (pag.NivelConfiabilidad == 0 || u.NivelConfiabilidad.Value == pag.NivelConfiabilidad)
                                         && (string.IsNullOrEmpty(pag.TablaProcedencia) || lstTablaProcedencia.Contains(u.TablaProcedencia.Value))
                                         && (string.IsNullOrEmpty(pag.TTLFiltro) || u.NombreExcel.ToUpper().Contains(pag.TTLFiltro.ToUpper()))
                                         select new ConfiguracionColumnaAplicacionDTO()
                                         {
                                             Id = u.ConfiguracionColumnaAplicacionId,
                                             NombreBD = u.NombreBD,
                                             NombreExcel = u.NombreExcel,
                                             TablaProcedenciaId = u.TablaProcedencia.Value,
                                             FlagEdicion = u.FlagEdicion.HasValue ? u.FlagEdicion.Value : false,
                                             FlagVerExportar = u.FlagVerExportar.HasValue ? u.FlagVerExportar.Value : false,
                                             OrdenColumna = u.OrdenColumna.HasValue ? u.OrdenColumna.Value : 0,
                                             FlagCampoNuevo = u.FlagCampoNuevo,
                                             FlagModificable = u.FlagModificable,
                                             FlagMostrarCampo = u.FlagMostrarCampo,
                                             FechaCreacion = u.FechaCreacion,
                                             FechaModificacion = u.FechaModificacion,
                                             UsuarioCreacion = u.UsuarioCreacion,
                                             UsuarioModificacion = u.UsuarioModificacion,
                                             ActivoAplica = u.ActivoAplica,
                                             ModoLlenado = u.ModoLlenado,
                                             NivelConfiabilidad = u.NivelConfiabilidad,
                                             TipoRegistro = u.TipoRegistro,
                                             RolAprueba = u.RolAprueba,
                                             RolRegistra = u.RolRegistra,
                                             DescripcionCampo = u.DescripcionCampo,
                                             RolResponsableActualizacion = u.RolResponsableActualizacion,
                                             NombreBDEntidadRelacion = u.NombreBDEntidadRelacion
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
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<AplicacionDTO> GetGestionAplicacion(PaginacionAplicacion pag, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<AplicacionDTO> GetGestionAplicacion(PaginacionAplicacion pag, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override int AddOrEditColumnaApp(ConfiguracionColumnaAplicacionDTO objRegistro)
        {
            DbContextTransaction transaction = null;
            try
            {
                int ID = 0;
                DateTime FECHA_ACTUAL = DateTime.Now;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    ctx.Database.CommandTimeout = 0;

                    using (transaction = ctx.Database.BeginTransaction())
                    {
                        if (objRegistro.Id == 0)
                        {
                            var entidad = new ConfiguracionColumnaAplicacion()
                            {
                                NombreBD = objRegistro.NombreBD,
                                NombreExcel = objRegistro.NombreExcel,
                                TablaProcedencia = objRegistro.TablaProcedenciaId,
                                FlagEdicion = objRegistro.FlagEdicion,
                                FechaCreacion = FECHA_ACTUAL,
                                UsuarioCreacion = objRegistro.UsuarioCreacion,
                                FlagActivo = true,
                                FlagEliminado = false,
                                FlagVerExportar = objRegistro.FlagVerExportar,
                                OrdenColumna = objRegistro.OrdenColumna,
                                FlagCampoNuevo = objRegistro.FlagCampoNuevo,
                                FlagObligatorio = objRegistro.FlagObligatorio,
                                FlagModificable = true,

                                //ActivoAplica = objRegistro.ActivoAplica,
                                //ModoLlenado = objRegistro.ModoLlenado,
                                //RolRegistra = objRegistro.RolRegistra,
                                //RolAprueba = objRegistro.RolAprueba,

                                //DescripcionCampo = objRegistro.DescripcionCampo,
                                //NivelConfiabilidad = objRegistro.NivelConfiabilidad,
                                //RolResponsableActualizacion = objRegistro.RolResponsableActualizacion,
                                //TipoRegistro = objRegistro.TipoRegistro
                            };
                            ctx.ConfiguracionColumnaAplicacion.Add(entidad);
                            ctx.SaveChanges();

                            ID = entidad.ConfiguracionColumnaAplicacionId;
                        }
                        else
                        {
                            var entidad = (from u in ctx.ConfiguracionColumnaAplicacion
                                           where u.ConfiguracionColumnaAplicacionId == objRegistro.Id && u.FlagActivo
                                           select u).FirstOrDefault();
                            if (entidad != null)
                            {
                                entidad.NombreExcel = objRegistro.NombreExcel;
                                entidad.FlagEdicion = objRegistro.FlagEdicion;
                                entidad.FlagVerExportar = objRegistro.FlagVerExportar;
                                entidad.OrdenColumna = objRegistro.OrdenColumna;
                                entidad.FlagObligatorio = objRegistro.FlagObligatorio;
                                //if (objRegistro.FlagVerExportar.Value)
                                //    entidad.OrdenColumna = objRegistro.OrdenColumna;
                                //else
                                //    entidad.OrdenColumna = 0;
                                entidad.FechaModificacion = FECHA_ACTUAL;
                                entidad.UsuarioModificacion = objRegistro.UsuarioModificacion;

                                entidad.ActivoAplica = objRegistro.ActivoAplica;
                                entidad.ModoLlenado = objRegistro.ModoLlenado;
                                entidad.RolRegistra = objRegistro.RolRegistra;
                                entidad.RolAprueba = objRegistro.RolAprueba;

                                entidad.DescripcionCampo = objRegistro.DescripcionCampo;
                                entidad.NivelConfiabilidad = objRegistro.NivelConfiabilidad;
                                entidad.RolResponsableActualizacion = objRegistro.RolResponsableActualizacion;
                                entidad.TipoRegistro = objRegistro.TipoRegistro;
                                entidad.NombreBDEntidadRelacion = objRegistro.UsuarioCreacion;
                                ctx.SaveChanges();
                                ID = entidad.ConfiguracionColumnaAplicacionId;
                            }
                        }

                        //Save InfoCampoPortafolio
                        objRegistro.InfoCampoPortafolio.ConfiguracionColumnaAplicacionId = ID;
                        objRegistro.InfoCampoPortafolio.UsuarioCreacion = objRegistro.UsuarioCreacion;
                        ServiceManager<ActivosDAO>.Provider.AddOrEditInfoCampoPortafolio(objRegistro.InfoCampoPortafolio, ctx);

                        transaction.Commit();
                        return ID;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                transaction.Rollback();
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: int AddOrEdit(AplicacionDTO objRegistro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: int AddOrEdit(AplicacionDTO objRegistro)"
                    , new object[] { null });
            }
        }

        public override DataTable GetPublicacionAplicacion(PaginacionReporteAplicacion pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                var data = new DataTable();

                var columnas = GetColumnasPublicacionAplicacionToBD(pag.TablaProcedencia);
                pag.Columnas = columnas;
                switch (pag.Procedencia)
                {
                    case (int)ETablaProcedenciaAplicacion.Aplicacion:
                    case (int)ETablaProcedenciaAplicacion.AplicacionDetalle:
                        data = ReportePublicacionAplicacion(pag);
                        break;
                    case (int)ETablaProcedenciaAplicacion.AplicacionData:
                        data = ReportePublicacionAplicacionData(pag);
                        break;
                    case (int)ETablaProcedenciaAplicacion.InfoCampoPortafolio:
                        data = ReportePublicacionAplicacion2(pag);
                        break;
                }

                if (data != null && data.Rows.Count > 0) totalRows = Convert.ToInt16(data.Rows[0]["TotalFilas"]);

                return data;
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetTecnologias()"
                    , new object[] { null });
            }
        }

        public override DataTable GetPublicacionAplicacion2(PaginacionReporteAplicacion pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                var data = new DataTable();

                var columnas = GetColumnasPublicacionAplicacionToBD2(pag.TablaProcedencia);
                pag.Columnas = columnas;
                data = ReportePublicacionAplicacionPortafolioAplicaciones(pag);

                if (data != null && data.Rows.Count > 0) totalRows = Convert.ToInt16(data.Rows[0]["TotalFilas"]);

                return data;
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetTecnologias()"
                    , new object[] { null });
            }
        }

        public override DataTable GetPublicacionAplicacionAsignada(PaginacionReporteAplicacion pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                var data = new DataTable();

                var columnas = GetColumnasPublicacionAplicacionToBD2(pag.TablaProcedencia);
                pag.Columnas = columnas;
                data = ReportePublicacionAplicacionAsignada(pag);

                if (data != null && data.Rows.Count > 0) totalRows = Convert.ToInt16(data.Rows[0]["TotalFilas"]);

                return data;
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetTecnologias()"
                    , new object[] { null });
            }
        }

        public DataTable ReportePublicacionAplicacionAsignada(PaginacionReporteAplicacion pag)
        {
            DataSet resultado = null;
            var cadenaConexion = Constantes.CadenaConexion;

            using (SqlConnection cnx = new SqlConnection(cadenaConexion))
            {
                cnx.Open();
                using (var comando = new SqlCommand("[CVT].[USP_Reporte_Publicacion_Catalogo_4]", cnx))
                {
                    comando.CommandTimeout = 0;
                    comando.CommandType = System.Data.CommandType.StoredProcedure;
                    comando.Parameters.Add(new SqlParameter("@aplicacion", string.IsNullOrWhiteSpace(pag.Aplicacion) ? string.Empty : pag.Aplicacion));
                    comando.Parameters.Add(new SqlParameter("@Matricula", pag.Matricula));

                    IDbDataAdapter adapter = new SqlDataAdapter(comando);
                    resultado = new DataSet();
                    adapter.Fill(resultado);
                }

                cnx.Close();

                return resultado.Tables[0];
            }
        }

        public DataTable ReportePublicacionAplicacion2(PaginacionReporteAplicacion pag)
        {
            DataSet resultado = null;
            var cadenaConexion = Constantes.CadenaConexion;

            using (SqlConnection cnx = new SqlConnection(cadenaConexion))
            {
                cnx.Open();
                using (var comando = new SqlCommand("[CVT].[USP_Reporte_Publicacion_Catalogo_3]", cnx))
                {
                    comando.CommandTimeout = 0;
                    comando.CommandType = System.Data.CommandType.StoredProcedure;
                    comando.Parameters.Add(new SqlParameter("@gerencia", pag.Gerencia));
                    comando.Parameters.Add(new SqlParameter("@division", pag.Division));
                    comando.Parameters.Add(new SqlParameter("@area", pag.Area));
                    comando.Parameters.Add(new SqlParameter("@unidad", pag.Unidad));
                    comando.Parameters.Add(new SqlParameter("@estado", pag.Estado));
                    comando.Parameters.Add(new SqlParameter("@clasificacionTecnica", pag.ClasificacionTecnica));
                    comando.Parameters.Add(new SqlParameter("@subclasificacionTecnica", pag.SubclasificacionTecnica));
                    comando.Parameters.Add(new SqlParameter("@aplicacion", string.IsNullOrWhiteSpace(pag.Aplicacion) ? string.Empty : pag.Aplicacion));
                    comando.Parameters.Add(new SqlParameter("@TipoPCI", string.IsNullOrWhiteSpace(pag.TipoPCI) ? string.Empty : pag.TipoPCI));
                    comando.Parameters.Add(new SqlParameter("@GestionadoPor", string.IsNullOrWhiteSpace(pag.GestionadoPor) ? string.Empty : pag.GestionadoPor));
                    comando.Parameters.Add(new SqlParameter("@LiderUsuario", string.IsNullOrWhiteSpace(pag.LiderUsuario) ? string.Empty : pag.LiderUsuario));
                    comando.Parameters.Add(new SqlParameter("@Columnas", pag.Columnas));
                    comando.Parameters.Add(new SqlParameter("@TablaProcedencia", pag.Procedencia));
                    comando.Parameters.Add(new SqlParameter("@PageSize", pag.pageSize));
                    comando.Parameters.Add(new SqlParameter("@PageNumber", pag.pageNumber));
                    comando.Parameters.Add(new SqlParameter("@tipoActivo", pag.TipoActivo));
                    comando.Parameters.Add(new SqlParameter("@exportar", pag.Exportar));
                    comando.Parameters.Add(new SqlParameter("@EstadoReactivacion", pag.AppReactivadas));

                    IDbDataAdapter adapter = new SqlDataAdapter(comando);
                    resultado = new DataSet();
                    adapter.Fill(resultado);
                }

                cnx.Close();

                return resultado.Tables[0];
            }
        }

        public DataTable ReportePublicacionAplicacionPortafolioAplicaciones(PaginacionReporteAplicacion pag)
        {
            DataSet resultado = null;
            var cadenaConexion = Constantes.CadenaConexion;

            using (SqlConnection cnx = new SqlConnection(cadenaConexion))
            {
                cnx.Open();
                using (var comando = new SqlCommand("[CVT].[USP_Reporte_Publicacion_Catalogo_PortafolioAplicaciones]", cnx))
                {
                    comando.CommandTimeout = 0;
                    comando.CommandType = System.Data.CommandType.StoredProcedure;
                    comando.Parameters.Add(new SqlParameter("@gerencia", pag.Gerencia));
                    comando.Parameters.Add(new SqlParameter("@division", pag.Division));
                    comando.Parameters.Add(new SqlParameter("@area", pag.Area));
                    comando.Parameters.Add(new SqlParameter("@unidad", pag.Unidad));
                    comando.Parameters.Add(new SqlParameter("@estado", pag.Estado));
                    comando.Parameters.Add(new SqlParameter("@clasificacionTecnica", pag.ClasificacionTecnica));
                    comando.Parameters.Add(new SqlParameter("@subclasificacionTecnica", pag.SubclasificacionTecnica));
                    comando.Parameters.Add(new SqlParameter("@aplicacion", string.IsNullOrWhiteSpace(pag.Aplicacion) ? string.Empty : pag.Aplicacion));
                    comando.Parameters.Add(new SqlParameter("@TipoPCI", string.IsNullOrWhiteSpace(pag.TipoPCI) ? string.Empty : pag.TipoPCI));
                    comando.Parameters.Add(new SqlParameter("@GestionadoPor", string.IsNullOrWhiteSpace(pag.GestionadoPor) ? string.Empty : pag.GestionadoPor));
                    comando.Parameters.Add(new SqlParameter("@LiderUsuario", string.IsNullOrWhiteSpace(pag.LiderUsuario) ? string.Empty : pag.LiderUsuario));
                    comando.Parameters.Add(new SqlParameter("@Columnas", pag.Columnas));
                    comando.Parameters.Add(new SqlParameter("@TablaProcedencia", pag.Procedencia));
                    comando.Parameters.Add(new SqlParameter("@PageSize", pag.pageSize));
                    comando.Parameters.Add(new SqlParameter("@PageNumber", pag.pageNumber));
                    comando.Parameters.Add(new SqlParameter("@tipoActivo", pag.TipoActivo));
                    comando.Parameters.Add(new SqlParameter("@exportar", pag.Exportar));

                    IDbDataAdapter adapter = new SqlDataAdapter(comando);
                    resultado = new DataSet();
                    adapter.Fill(resultado);
                }

                cnx.Close();

                return resultado.Tables[0];
            }
        }

        public DataTable ReportePublicacionAplicacion(PaginacionReporteAplicacion pag)
        {
            DataSet resultado = null;
            var cadenaConexion = Constantes.CadenaConexion;

            using (SqlConnection cnx = new SqlConnection(cadenaConexion))
            {
                cnx.Open();
                using (var comando = new SqlCommand("[CVT].[USP_Reporte_Publicacion_Aplicacion]", cnx))
                {
                    comando.CommandTimeout = 0;
                    comando.CommandType = System.Data.CommandType.StoredProcedure;
                    comando.Parameters.Add(new SqlParameter("@gerencia", pag.Gerencia));
                    comando.Parameters.Add(new SqlParameter("@division", pag.Division));
                    comando.Parameters.Add(new SqlParameter("@area", pag.Area));
                    comando.Parameters.Add(new SqlParameter("@unidad", pag.Unidad));
                    comando.Parameters.Add(new SqlParameter("@estado", pag.Estado));
                    comando.Parameters.Add(new SqlParameter("@clasificacionTecnica", pag.ClasificacionTecnica));
                    comando.Parameters.Add(new SqlParameter("@subclasificacionTecnica", pag.SubclasificacionTecnica));
                    comando.Parameters.Add(new SqlParameter("@aplicacion", string.IsNullOrWhiteSpace(pag.Aplicacion) ? string.Empty : pag.Aplicacion));
                    comando.Parameters.Add(new SqlParameter("@Columnas", pag.Columnas));
                    comando.Parameters.Add(new SqlParameter("@TablaProcedencia", pag.Procedencia));
                    comando.Parameters.Add(new SqlParameter("@PageSize", pag.pageSize));
                    comando.Parameters.Add(new SqlParameter("@PageNumber", pag.pageNumber));
                    comando.Parameters.Add(new SqlParameter("@tipoActivo", pag.TipoActivo));

                    IDbDataAdapter adapter = new SqlDataAdapter(comando);
                    resultado = new DataSet();
                    adapter.Fill(resultado);
                }

                cnx.Close();

                return resultado.Tables[0];
            }
        }


        public DataTable ReportePublicacionAplicacionData(PaginacionReporteAplicacion pag) //TODO
        {
            DataSet resultado = null;
            var cadenaConexion = Constantes.CadenaConexion;

            using (SqlConnection cnx = new SqlConnection(cadenaConexion))
            {
                cnx.Open();
                using (var comando = new SqlCommand("[CVT].[USP_Reporte_Publicacion_Aplicacion_Data]", cnx))
                {
                    comando.CommandTimeout = 0;
                    comando.CommandType = System.Data.CommandType.StoredProcedure;
                    comando.Parameters.Add(new SqlParameter("@Columnas", pag.Columnas));
                    comando.Parameters.Add(new SqlParameter("@gerencia", pag.Gerencia));
                    comando.Parameters.Add(new SqlParameter("@division", pag.Division));
                    comando.Parameters.Add(new SqlParameter("@area", pag.Area));
                    comando.Parameters.Add(new SqlParameter("@unidad", pag.Unidad));
                    comando.Parameters.Add(new SqlParameter("@estado", pag.Estado));
                    comando.Parameters.Add(new SqlParameter("@clasificacionTecnica", pag.ClasificacionTecnica));
                    comando.Parameters.Add(new SqlParameter("@subclasificacionTecnica", pag.SubclasificacionTecnica));
                    comando.Parameters.Add(new SqlParameter("@aplicacion", pag.Aplicacion));
                    comando.Parameters.Add(new SqlParameter("@tipoActivo", (string.IsNullOrWhiteSpace(pag.TipoActivo) ? string.Empty : pag.TipoActivo)));
                    comando.Parameters.Add(new SqlParameter("@PageSize", pag.pageSize));
                    comando.Parameters.Add(new SqlParameter("@PageNumber", pag.pageNumber));

                    IDbDataAdapter adapter = new SqlDataAdapter(comando);
                    resultado = new DataSet();
                    adapter.Fill(resultado);
                }

                cnx.Close();

                return resultado.Tables[0];
            }
        }

        public DataTable ReportePublicacionAplicacionData2(PaginacionReporteAplicacion pag) //TODO
        {
            DataSet resultado = null;
            var cadenaConexion = Constantes.CadenaConexion;

            using (SqlConnection cnx = new SqlConnection(cadenaConexion))
            {
                cnx.Open();
                using (var comando = new SqlCommand("[CVT].[USP_Reporte_Publicacion_Aplicacion_Data]", cnx))
                {
                    comando.CommandTimeout = 0;
                    comando.CommandType = System.Data.CommandType.StoredProcedure;
                    comando.Parameters.Add(new SqlParameter("@Columnas", pag.Columnas));
                    //comando.Parameters.Add(new SqlParameter("@TablaProcedencia", pag.Procedencia));
                    comando.Parameters.Add(new SqlParameter("@gerencia", pag.Gerencia));
                    comando.Parameters.Add(new SqlParameter("@division", pag.Division));
                    comando.Parameters.Add(new SqlParameter("@area", pag.Area));
                    comando.Parameters.Add(new SqlParameter("@unidad", pag.Unidad));
                    comando.Parameters.Add(new SqlParameter("@estado", pag.Estado));
                    comando.Parameters.Add(new SqlParameter("@clasificacionTecnica", pag.ClasificacionTecnica));
                    comando.Parameters.Add(new SqlParameter("@subclasificacionTecnica", pag.SubclasificacionTecnica));
                    comando.Parameters.Add(new SqlParameter("@aplicacion", pag.Aplicacion));
                    comando.Parameters.Add(new SqlParameter("@PageSize", pag.pageSize));
                    comando.Parameters.Add(new SqlParameter("@PageNumber", pag.pageNumber));

                    IDbDataAdapter adapter = new SqlDataAdapter(comando);
                    resultado = new DataSet();
                    adapter.Fill(resultado);
                }

                cnx.Close();

                return resultado.Tables[0];
            }
        }

        public string GetColumnasPublicacionAplicacionToBD(string tablaProcedencia)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var lstTablaProcedencia = new List<int>();
                        string columnas = string.Empty;

                        if (!string.IsNullOrEmpty(tablaProcedencia))
                            lstTablaProcedencia = tablaProcedencia.Split(';').Select(int.Parse).ToList();

                        string[] ldata = (from u in ctx.ConfiguracionColumnaAplicacion
                                          join b in ctx.TablaProcedenciaAplicacion on u.TablaProcedencia equals b.TablaProcedenciaAplicacionId
                                          where u.FlagActivo && u.FlagVerExportar.Value
                                          && (string.IsNullOrEmpty(tablaProcedencia) || lstTablaProcedencia.Contains(u.TablaProcedencia.Value))
                                          orderby u.OrdenColumna.Value ascending
                                          select (b.Alias + "." + u.NombreBD)
                                     ).ToArray();



                        columnas = string.Join(",", ldata);

                        return columnas;
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

        public string GetColumnasPublicacionAplicacionToBD2(string tablaProcedencia)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var lstTablaProcedencia = new List<int>();
                        string columnas = string.Empty;

                        if (!string.IsNullOrEmpty(tablaProcedencia))
                            lstTablaProcedencia = tablaProcedencia.Split(';').Select(int.Parse).ToList();

                        string[] ldata = (from u in ctx.ConfiguracionColumnaAplicacion
                                          join b in ctx.TablaProcedenciaAplicacion on u.TablaProcedencia equals b.TablaProcedenciaAplicacionId
                                          where u.FlagActivo && u.FlagVerExportar.Value
                                          && (string.IsNullOrEmpty(tablaProcedencia) || lstTablaProcedencia.Contains(u.TablaProcedencia.Value))
                                          orderby u.OrdenColumna.Value ascending
                                          select (b.Alias + "." + u.NombreBD)

                                     ).ToArray();



                        columnas = string.Join(",", ldata);

                        return columnas;
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


        public void AddOrEditResponsablesAplicacion(AplicacionDTO objeto)
        {
            string NO_APLICA = "NO APLICA";
            bool flagSolicitud = objeto.TipoSolicitudId == (int)ETipoSolicitudAplicacion.ModificacionAplicacion;

            if (!string.IsNullOrEmpty(objeto.Owner_LiderUsuario_ProductOwner)
                && objeto.Owner_LiderUsuario_ProductOwner != NO_APLICA) //1
            {
                var objetoDTO = new AplicacionPortafolioResponsablesDTO()
                {
                    Id = objeto.HdOwner,
                    UsuarioCreacion = objeto.UsuarioCreacion,
                    CodigoAPT = objeto.CodigoAPT,
                    Matricula = objeto.Owner_LiderUsuario_ProductOwner,
                    Colaborador = objeto.MatriculaOwner,
                    PortafolioResponsableId = (int)EPortafolioResponsable.Owner
                };
                ServiceManager<AplicacionDAO>.Provider.AddOrEditAPR(objetoDTO, flagSolicitud);
            }

            if (!string.IsNullOrEmpty(objeto.Gestor_UsuarioAutorizador_ProductOwner)
                && objeto.Gestor_UsuarioAutorizador_ProductOwner != NO_APLICA) //2
            {
                var objetoDTO = new AplicacionPortafolioResponsablesDTO()
                {
                    Id = objeto.HdGestor,
                    UsuarioCreacion = objeto.UsuarioCreacion,
                    CodigoAPT = objeto.CodigoAPT,
                    Matricula = objeto.Gestor_UsuarioAutorizador_ProductOwner,
                    Colaborador = objeto.MatriculaGestor,
                    PortafolioResponsableId = (int)EPortafolioResponsable.Gestor
                };
                ServiceManager<AplicacionDAO>.Provider.AddOrEditAPR(objetoDTO, flagSolicitud);
            }

            if (!string.IsNullOrEmpty(objeto.TribeTechnicalLead)
                && objeto.TribeTechnicalLead != NO_APLICA) //3
            {
                var objetoDTO = new AplicacionPortafolioResponsablesDTO()
                {
                    Id = objeto.HdTTL,
                    UsuarioCreacion = objeto.UsuarioCreacion,
                    CodigoAPT = objeto.CodigoAPT,
                    Matricula = objeto.TribeTechnicalLead,
                    Colaborador = objeto.MatriculaTTL,
                    PortafolioResponsableId = (int)EPortafolioResponsable.TTL
                };
                ServiceManager<AplicacionDAO>.Provider.AddOrEditAPR(objetoDTO, flagSolicitud);
            }

            if (!string.IsNullOrEmpty(objeto.Experto_Especialista)
                && objeto.Experto_Especialista != NO_APLICA) //4
            {
                var objetoDTO = new AplicacionPortafolioResponsablesDTO()
                {
                    Id = objeto.HdExperto,
                    UsuarioCreacion = objeto.UsuarioCreacion,
                    CodigoAPT = objeto.CodigoAPT,
                    Matricula = objeto.Experto_Especialista,
                    Colaborador = objeto.MatriculaExperto,
                    PortafolioResponsableId = (int)EPortafolioResponsable.Experto
                };
                ServiceManager<AplicacionDAO>.Provider.AddOrEditAPR(objetoDTO, flagSolicitud);
            }

            if (!string.IsNullOrEmpty(objeto.BrokerSistemas)
                && objeto.BrokerSistemas != NO_APLICA) //5
            {
                var objetoDTO = new AplicacionPortafolioResponsablesDTO()
                {
                    Id = objeto.HdBroker,
                    UsuarioCreacion = objeto.UsuarioCreacion,
                    CodigoAPT = objeto.CodigoAPT,
                    Matricula = objeto.BrokerSistemas,
                    Colaborador = objeto.MatriculaBroker,
                    PortafolioResponsableId = (int)EPortafolioResponsable.Broker
                };
                ServiceManager<AplicacionDAO>.Provider.AddOrEditAPR(objetoDTO, flagSolicitud);
            }

            if (!string.IsNullOrEmpty(objeto.JefeEquipo_ExpertoAplicacionUserIT_ProductOwner)
                && objeto.JefeEquipo_ExpertoAplicacionUserIT_ProductOwner != NO_APLICA) //6
            {
                var objetoDTO = new AplicacionPortafolioResponsablesDTO()
                {
                    Id = objeto.HdJDE,
                    UsuarioCreacion = objeto.UsuarioCreacion,
                    CodigoAPT = objeto.CodigoAPT,
                    Matricula = objeto.JefeEquipo_ExpertoAplicacionUserIT_ProductOwner,
                    Colaborador = objeto.MatriculaJDE,
                    PortafolioResponsableId = (int)EPortafolioResponsable.JdE
                };
                ServiceManager<AplicacionDAO>.Provider.AddOrEditAPR(objetoDTO, flagSolicitud);
            }
        }

        public override List<ItemColumnaAppJS> GetColumnasPublicacionAplicacionToJS(string tablaProcedencia)
        {
            try
            {
                var lstTablaProcedencia = new List<int>();
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        if (!string.IsNullOrEmpty(tablaProcedencia))
                            lstTablaProcedencia = tablaProcedencia.Split(';').Select(int.Parse).ToList();

                        var ldata = (from u in ctx.ConfiguracionColumnaAplicacion
                                     where u.FlagActivo && u.FlagVerExportar.Value
                                     && (string.IsNullOrEmpty(tablaProcedencia) || lstTablaProcedencia.Contains(u.TablaProcedencia.Value))
                                     orderby u.OrdenColumna.Value ascending
                                     select new ItemColumnaAppJS()
                                     {
                                         ConfiguracionId = u.ConfiguracionColumnaAplicacionId,
                                         title = u.NombreExcel,
                                         field = u.NombreBD,
                                         formatter = u.ConfiguracionColumnaAplicacionId == 9 ? "descripcionFormatter" : "generalFormatter"
                                     }).ToList();

                        if (ldata.Any())
                        {
                            var itemOwner = ldata.FirstOrDefault(x => x.ConfiguracionId == (int)EIdResponsablePortafolio.Owner_LiderUsuario_ProductOwner);
                            if (itemOwner != null)
                            {
                                var indexOwner = ldata.FindIndex(x => x.ConfiguracionId == (int)EIdResponsablePortafolio.Owner_LiderUsuario_ProductOwner);
                                ldata.Insert(indexOwner + 1, new ItemColumnaAppJS()
                                {
                                    ConfiguracionId = itemOwner.ConfiguracionId,
                                    title = string.Format("Responsable {0}", Utilitarios.GetEnumDescription2((EIdResponsablePortafolio)itemOwner.ConfiguracionId)),
                                    field = "Resp_Owner",
                                    formatter = "generalFormatter"
                                });
                            }

                            var itemGestor = ldata.FirstOrDefault(x => x.ConfiguracionId == (int)EIdResponsablePortafolio.Gestor_UsuarioAutorizador_ProductOwner);
                            if (itemGestor != null)
                            {
                                var indexGestor = ldata.FindIndex(x => x.ConfiguracionId == (int)EIdResponsablePortafolio.Gestor_UsuarioAutorizador_ProductOwner);
                                ldata.Insert(indexGestor + 1, new ItemColumnaAppJS()
                                {
                                    ConfiguracionId = itemGestor.ConfiguracionId,
                                    title = string.Format("Responsable {0}", Utilitarios.GetEnumDescription2((EIdResponsablePortafolio)itemGestor.ConfiguracionId)),
                                    field = "Resp_Gestor",
                                    formatter = "generalFormatter"
                                });
                            }

                            var itemTTL = ldata.FirstOrDefault(x => x.ConfiguracionId == (int)EIdResponsablePortafolio.TribeTechnicalLead);
                            if (itemTTL != null)
                            {
                                var indexTTL = ldata.FindIndex(x => x.ConfiguracionId == (int)EIdResponsablePortafolio.TribeTechnicalLead);
                                ldata.Insert(indexTTL + 1, new ItemColumnaAppJS()
                                {
                                    ConfiguracionId = itemTTL.ConfiguracionId,
                                    title = string.Format("Responsable {0}", Utilitarios.GetEnumDescription2((EIdResponsablePortafolio)itemTTL.ConfiguracionId)),
                                    field = "Resp_TTL",
                                    formatter = "generalFormatter"
                                });
                            }

                            var itemExperto = ldata.FirstOrDefault(x => x.ConfiguracionId == (int)EIdResponsablePortafolio.Experto_Especialista);
                            if (itemExperto != null)
                            {
                                var indexExperto = ldata.FindIndex(x => x.ConfiguracionId == (int)EIdResponsablePortafolio.Experto_Especialista);
                                ldata.Insert(indexExperto + 1, new ItemColumnaAppJS()
                                {
                                    ConfiguracionId = itemExperto.ConfiguracionId,
                                    title = string.Format("Responsable {0}", Utilitarios.GetEnumDescription2((EIdResponsablePortafolio)itemExperto.ConfiguracionId)),
                                    field = "Resp_Experto",
                                    formatter = "generalFormatter"
                                });
                            }

                            var itemBroker = ldata.FirstOrDefault(x => x.ConfiguracionId == (int)EIdResponsablePortafolio.BrokerSistemas);
                            if (itemBroker != null)
                            {
                                var indexBroker = ldata.FindIndex(x => x.ConfiguracionId == (int)EIdResponsablePortafolio.BrokerSistemas);
                                ldata.Insert(indexBroker + 1, new ItemColumnaAppJS()
                                {
                                    ConfiguracionId = itemBroker.ConfiguracionId,
                                    title = string.Format("Responsable {0}", Utilitarios.GetEnumDescription2((EIdResponsablePortafolio)itemBroker.ConfiguracionId)),
                                    field = "Resp_Broker",
                                    formatter = "generalFormatter"
                                });
                            }

                            var itemJDE = ldata.FirstOrDefault(x => x.ConfiguracionId == (int)EIdResponsablePortafolio.JefeEquipo_ExpertoAplicacionUserIT_ProductOwner);
                            if (itemJDE != null)
                            {
                                var indexJDE = ldata.FindIndex(x => x.ConfiguracionId == (int)EIdResponsablePortafolio.JefeEquipo_ExpertoAplicacionUserIT_ProductOwner);
                                ldata.Insert(indexJDE + 1, new ItemColumnaAppJS()
                                {
                                    ConfiguracionId = itemJDE.ConfiguracionId,
                                    title = string.Format("Responsable {0}", Utilitarios.GetEnumDescription2((EIdResponsablePortafolio)itemJDE.ConfiguracionId)),
                                    field = "Resp_JDE",
                                    formatter = "generalFormatter"
                                });
                            }
                        }

                        foreach (var item in ldata)
                        {
                            item.field = string.IsNullOrWhiteSpace(item.field) ? item.title.Replace(" ", "_") : item.field;
                        }

                        return ldata;
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

        public override List<ItemColumnaAppJS> GetColumnasPublicacionAplicacionToJS2(string tablaProcedencia)
        {
            try
            {
                var lstTablaProcedencia = new List<int>();
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        if (!string.IsNullOrEmpty(tablaProcedencia))
                            lstTablaProcedencia = tablaProcedencia.Split(';').Select(int.Parse).ToList();

                        var ldata = (from u in ctx.ConfiguracionColumnaAplicacion
                                     where u.FlagActivo && u.FlagVerExportar.Value
                                     && (string.IsNullOrEmpty(tablaProcedencia) || lstTablaProcedencia.Contains(u.TablaProcedencia.Value))
                                     orderby u.OrdenColumna.Value ascending
                                     select new ItemColumnaAppJS()
                                     {
                                         ConfiguracionId = u.ConfiguracionColumnaAplicacionId,
                                         title = u.NombreExcel,
                                         field = u.NombreBD,
                                         formatter = u.ConfiguracionColumnaAplicacionId == 9 ? "descripcionFormatter" : "generalFormatter"
                                     }).ToList();

                        //if (ldata.Any())
                        //{
                        //    var itemOwner = ldata.FirstOrDefault(x => x.ConfiguracionId == (int)EIdResponsablePortafolio.Owner_LiderUsuario_ProductOwner);
                        //    if (itemOwner != null)
                        //    {
                        //        var indexOwner = ldata.FindIndex(x => x.ConfiguracionId == (int)EIdResponsablePortafolio.Owner_LiderUsuario_ProductOwner);
                        //        ldata.Insert(indexOwner + 1, new ItemColumnaAppJS()
                        //        {
                        //            ConfiguracionId = itemOwner.ConfiguracionId,
                        //            title = string.Format("Responsable {0}", Utilitarios.GetEnumDescription2((EIdResponsablePortafolio)itemOwner.ConfiguracionId)),
                        //            field = "Resp_Owner",
                        //            formatter = "generalFormatter"
                        //        });
                        //    }

                        //    var itemGestor = ldata.FirstOrDefault(x => x.ConfiguracionId == (int)EIdResponsablePortafolio.Gestor_UsuarioAutorizador_ProductOwner);
                        //    if (itemGestor != null)
                        //    {
                        //        var indexGestor = ldata.FindIndex(x => x.ConfiguracionId == (int)EIdResponsablePortafolio.Gestor_UsuarioAutorizador_ProductOwner);
                        //        ldata.Insert(indexGestor + 1, new ItemColumnaAppJS()
                        //        {
                        //            ConfiguracionId = itemGestor.ConfiguracionId,
                        //            title = string.Format("Responsable {0}", Utilitarios.GetEnumDescription2((EIdResponsablePortafolio)itemGestor.ConfiguracionId)),
                        //            field = "Resp_Gestor",
                        //            formatter = "generalFormatter"
                        //        });
                        //    }

                        //    var itemTTL = ldata.FirstOrDefault(x => x.ConfiguracionId == (int)EIdResponsablePortafolio.TribeTechnicalLead);
                        //    if (itemTTL != null)
                        //    {
                        //        var indexTTL = ldata.FindIndex(x => x.ConfiguracionId == (int)EIdResponsablePortafolio.TribeTechnicalLead);
                        //        ldata.Insert(indexTTL + 1, new ItemColumnaAppJS()
                        //        {
                        //            ConfiguracionId = itemTTL.ConfiguracionId,
                        //            title = string.Format("Responsable {0}", Utilitarios.GetEnumDescription2((EIdResponsablePortafolio)itemTTL.ConfiguracionId)),
                        //            field = "Resp_TTL",
                        //            formatter = "generalFormatter"
                        //        });
                        //    }

                        //    var itemExperto = ldata.FirstOrDefault(x => x.ConfiguracionId == (int)EIdResponsablePortafolio.Experto_Especialista);
                        //    if (itemExperto != null)
                        //    {
                        //        var indexExperto = ldata.FindIndex(x => x.ConfiguracionId == (int)EIdResponsablePortafolio.Experto_Especialista);
                        //        ldata.Insert(indexExperto + 1, new ItemColumnaAppJS()
                        //        {
                        //            ConfiguracionId = itemExperto.ConfiguracionId,
                        //            title = string.Format("Responsable {0}", Utilitarios.GetEnumDescription2((EIdResponsablePortafolio)itemExperto.ConfiguracionId)),
                        //            field = "Resp_Experto",
                        //            formatter = "generalFormatter"
                        //        });
                        //    }

                        //    var itemBroker = ldata.FirstOrDefault(x => x.ConfiguracionId == (int)EIdResponsablePortafolio.BrokerSistemas);
                        //    if (itemBroker != null)
                        //    {
                        //        var indexBroker = ldata.FindIndex(x => x.ConfiguracionId == (int)EIdResponsablePortafolio.BrokerSistemas);
                        //        ldata.Insert(indexBroker + 1, new ItemColumnaAppJS()
                        //        {
                        //            ConfiguracionId = itemBroker.ConfiguracionId,
                        //            title = string.Format("Responsable {0}", Utilitarios.GetEnumDescription2((EIdResponsablePortafolio)itemBroker.ConfiguracionId)),
                        //            field = "Resp_Broker",
                        //            formatter = "generalFormatter"
                        //        });
                        //    }

                        //    var itemJDE = ldata.FirstOrDefault(x => x.ConfiguracionId == (int)EIdResponsablePortafolio.JefeEquipo_ExpertoAplicacionUserIT_ProductOwner);
                        //    if (itemJDE != null)
                        //    {
                        //        var indexJDE = ldata.FindIndex(x => x.ConfiguracionId == (int)EIdResponsablePortafolio.JefeEquipo_ExpertoAplicacionUserIT_ProductOwner);
                        //        ldata.Insert(indexJDE + 1, new ItemColumnaAppJS()
                        //        {
                        //            ConfiguracionId = itemJDE.ConfiguracionId,
                        //            title = string.Format("Responsable {0}", Utilitarios.GetEnumDescription2((EIdResponsablePortafolio)itemJDE.ConfiguracionId)),
                        //            field = "Resp_JDE",
                        //            formatter = "generalFormatter"
                        //        });
                        //    }
                        //}

                        foreach (var item in ldata)
                        {
                            item.field = string.IsNullOrWhiteSpace(item.field) ? item.title.Replace(" ", "_") : item.field;
                        }

                        return ldata;
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



        public override bool ExisteOrden(int ordenNuevo, int ordenActual)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        bool? estado = (from u in ctx.ConfiguracionColumnaAplicacion
                                        where u.FlagActivo
                                        && u.OrdenColumna == ordenNuevo
                                        && u.OrdenColumna != ordenActual
                                        select true).FirstOrDefault();

                        return estado == true;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: bool ExisteOrden(int ordenNuevo, int ordenActual)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: bool ExisteOrden(int ordenNuevo, int ordenActual)"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocomplete> GetTipoExpertoPortafolioByFiltro(string filtro)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.PortafolioResponsables
                                   where //u.FlagActivo
                                   (string.IsNullOrEmpty(filtro) || u.Nombre.ToUpper().Contains(filtro.ToUpper()))
                                   select new CustomAutocomplete()
                                   {
                                       Id = u.PortafolioResponsableId.ToString(),
                                       Descripcion = u.Nombre,
                                       value = u.Nombre
                                   }).ToList();

                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetJefeEquipoByFiltro(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetJefeEquipoByFiltro(string filtro)"
                    , new object[] { null });
            }
        }

        public override bool AddOrEditAplicacionExpertoPortafolio(ParametroList objRegistro)
        {
            try
            {
                bool estado = false;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    //remove items
                    var lIdsEliminar = objRegistro.ListIdsEliminar;
                    if (lIdsEliminar != null && lIdsEliminar.Count > 0)
                    {
                        foreach (var item in lIdsEliminar)
                        {
                            var existeBd = (from u in ctx.AplicacionPortafolioResponsables
                                            where u.Matricula == item.Matricula && u.CodigoAPT == objRegistro.CodigoAPT
                                            select u).FirstOrDefault();
                            if (existeBd != null)
                            {
                                existeBd.FlagActivo = false;
                                existeBd.FechaModificacion = DateTime.Now;
                                existeBd.ModificadoPor = objRegistro.UsuarioModificacion;
                            }
                            ctx.SaveChanges();
                        }
                    }

                    //add items
                    var lIdsRegistrar = objRegistro.ListIdsRegistrar;
                    if (lIdsRegistrar != null && lIdsRegistrar.Count > 0)
                    {
                        foreach (var item in lIdsRegistrar)
                        {
                            var aplicacionBD = ctx.Aplicacion.FirstOrDefault(x => x.CodigoAPT == objRegistro.CodigoAPT);

                            if (item.TipoExpertoId.Value != (int)EPortafolioResponsable.Experto)
                            {
                                var existeBd = (from u in ctx.AplicacionPortafolioResponsables
                                                where
                                                //u.Matricula == item.Matricula && 
                                                u.CodigoAPT == objRegistro.CodigoAPT &&
                                                u.PortafolioResponsableId == item.TipoExpertoId.Value &&
                                                u.FlagActivo
                                                select u).FirstOrDefault();
                                if (existeBd != null)
                                {
                                    var adUsuario = new ADUsuario().ObtenerADUsuario(item.Matricula);
                                    existeBd.FlagActivo = true;
                                    existeBd.FechaModificacion = DateTime.Now;
                                    existeBd.ModificadoPor = objRegistro.UsuarioModificacion;
                                    existeBd.Matricula = item.Matricula;
                                    existeBd.Colaborador = adUsuario != null ? adUsuario.Nombre : string.Empty;
                                }
                                else
                                {
                                    var adUsuario = new ADUsuario().ObtenerADUsuario(item.Matricula);

                                    var objRegistroBd = new AplicacionPortafolioResponsables()
                                    {
                                        CodigoAPT = objRegistro.CodigoAPT,
                                        Matricula = item.Matricula,
                                        FlagActivo = true,
                                        FechaCreacion = DateTime.Now,
                                        CreadoPor = objRegistro.UsuarioModificacion,
                                        PortafolioResponsableId = item.TipoExpertoId.Value,
                                        Colaborador = adUsuario != null ? adUsuario.Nombre : string.Empty
                                    };
                                    ctx.AplicacionPortafolioResponsables.Add(objRegistroBd);
                                }
                            }
                            else
                            {
                                var existeBd = (from u in ctx.AplicacionPortafolioResponsables
                                                where
                                                u.Matricula == item.Matricula &&
                                                u.CodigoAPT == objRegistro.CodigoAPT &&
                                                u.PortafolioResponsableId == item.TipoExpertoId.Value
                                                select u).FirstOrDefault();
                                if (existeBd != null)
                                {
                                    existeBd.FlagActivo = true;
                                    existeBd.FechaModificacion = DateTime.Now;
                                    existeBd.ModificadoPor = objRegistro.UsuarioModificacion;
                                }
                                else
                                {
                                    var adUsuario = new ADUsuario().ObtenerADUsuario(item.Matricula);

                                    var objRegistroBd = new AplicacionPortafolioResponsables()
                                    {
                                        CodigoAPT = objRegistro.CodigoAPT,
                                        Matricula = item.Matricula,
                                        FlagActivo = true,
                                        FechaCreacion = DateTime.Now,
                                        CreadoPor = objRegistro.UsuarioModificacion,
                                        PortafolioResponsableId = item.TipoExpertoId.Value,
                                        Colaborador = adUsuario != null ? adUsuario.Nombre : string.Empty
                                    };
                                    ctx.AplicacionPortafolioResponsables.Add(objRegistroBd);
                                }
                            }

                            //Actualizar en la información del portafolio
                            if (aplicacionBD != null)
                            {
                                switch (item.TipoExpertoId.Value)
                                {
                                    case (int)EPortafolioResponsable.Broker:
                                        aplicacionBD.BrokerSistemas = item.Matricula;
                                        break;
                                    case (int)EPortafolioResponsable.Gestor:
                                        aplicacionBD.Gestor_UsuarioAutorizador_ProductOwner = item.Matricula;
                                        break;
                                    case (int)EPortafolioResponsable.JdE:
                                        aplicacionBD.JefeEquipo_ExpertoAplicacionUserIT_ProductOwner = item.Matricula;
                                        break;
                                    case (int)EPortafolioResponsable.Owner:
                                        aplicacionBD.Owner_LiderUsuario_ProductOwner = item.Matricula;
                                        break;
                                    case (int)EPortafolioResponsable.TTL:
                                        aplicacionBD.TribeTechnicalLead = item.Matricula;
                                        break;
                                }
                            }

                            ctx.SaveChanges();
                        }
                    }
                    estado = true;
                }
                return estado;
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: bool AddOrEditAplicacionExperto(ParametroList objRegistro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: bool AddOrEditAplicacionExperto(ParametroList objRegistro)"
                    , new object[] { null });
            }
        }

        public override List<AplicacionExpertoDTO> GetAplicacionExpertoPortafolio(string Id)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var entidad = (from u in ctx.Aplicacion
                                       join b in ctx.AplicacionPortafolioResponsables on u.CodigoAPT equals b.CodigoAPT
                                       join e in ctx.PortafolioResponsables on b.PortafolioResponsableId equals e.PortafolioResponsableId
                                       where u.FlagActivo && b.FlagActivo
                                       && b.CodigoAPT == Id
                                       select new AplicacionExpertoDTO()
                                       {
                                           CodApp = u.CodigoAPT,
                                           Matricula = b.Matricula,
                                           Nombres = b.Colaborador,
                                           Activo = b.FlagActivo,
                                           TipoExpertoId = b.PortafolioResponsableId,
                                           TipoExpertoToString = e.Nombre,
                                           FechaCreacion = b.FechaCreacion,
                                           UsuarioCreacion = b.CreadoPor,
                                           Id = b.AplicacionPortafolioResponsableId,
                                           CorreoElectronico = b.CorreoElectronico

                                       }).ToList();

                        return entidad;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<AplicacionExpertoDTO> GetAplicacionExperto(string Id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<AplicacionExpertoDTO> GetAplicacionExperto(string Id)"
                    , new object[] { null });
            }
        }

        public override List<ModuloAplicacionDTO> GetModuloAplicacion(PaginacionAplicacion pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        //ctx.Database.CommandTimeout = 0;
                        var registros = (from u in ctx.ModuloAplicacion
                                         where (string.IsNullOrEmpty(pag.CodigoAPT) || u.CodigoAPT == pag.CodigoAPT)
                                         //u.FlagActivo
                                         select new ModuloAplicacionDTO()
                                         {
                                             Id = u.ModuloAplicacionId,
                                             CodigoAPT = u.CodigoAPT,
                                             Codigo = u.Codigo,
                                             Nombre = u.Nombre,
                                             Descripcion = u.Descripcion,
                                             FechaCreacion = u.FechaCreacion,
                                             FechaModificacion = u.FechaModificacion,
                                             UsuarioCreacion = u.UsuarioCreacion,
                                             FlagRegistrado = true,
                                             Activo = u.FlagActivo
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
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<AplicacionDTO> GetGestionAplicacion(PaginacionAplicacion pag, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<AplicacionDTO> GetGestionAplicacion(PaginacionAplicacion pag, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override bool CambiarEstadoModulo(int Id, bool estado, string usuario)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var itemBD = (from u in ctx.ModuloAplicacion
                                  where u.ModuloAplicacionId == Id
                                  select u).FirstOrDefault();

                    if (itemBD != null)
                    {
                        itemBD.FechaModificacion = DateTime.Now;
                        itemBD.UsuarioModificacion = usuario;
                        itemBD.FlagActivo = estado;

                        ctx.SaveChanges();

                        return true;
                    }
                    else
                        return false;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorDominioDTO
                    , "Error en el metodo: bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorDominioDTO
                    , "Error en el metodo: bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
        }

        public override string GetCodigoModulo(string codigoAPT)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    string retorno = string.Empty;
                    int codigo_next = 0;
                    string codigo_format = string.Empty;

                    var entidad = (from u in ctx.ModuloAplicacion
                                   where u.CodigoAPT == codigoAPT
                                   orderby u.FechaCreacion descending
                                   select u).FirstOrDefault();

                    if (entidad != null)
                    {
                        codigo_next = entidad.ModuloAplicacionId++;
                        codigo_format = codigo_next < 10 ? string.Format("0{0}", codigo_next) : codigo_next.ToString();

                        retorno = string.Format("{0}-{1}", codigoAPT, codigo_format);
                    }
                    else
                    {
                        retorno = string.Format("{0}-{1}", codigoAPT, "01");
                    }

                    return retorno;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorDominioDTO
                    , "Error en el metodo: bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorDominioDTO
                    , "Error en el metodo: bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocomplete> GetBrokerByFiltro(string filtro)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.Aplicacion
                                   where u.FlagActivo
                                   && (string.IsNullOrEmpty(filtro) || u.BrokerSistemas.ToUpper().Contains(filtro.ToUpper()))
                                   orderby u.BrokerSistemas
                                   group u by u.BrokerSistemas into grp
                                   select new CustomAutocomplete()
                                   {
                                       Id = grp.Key,
                                       Descripcion = grp.Key,
                                       value = grp.Key
                                   }).ToList();

                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetOwnerByFiltro(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetOwnerByFiltro(string filtro)"
                    , new object[] { null });
            }
        }

        public override List<ParametroDTO> GetParametroAplicacion(PaginacionAplicacion pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var registros = (from u in ctx.ParametroApp
                                         where (string.IsNullOrEmpty(pag.nombre) || u.Codigo.ToUpper().Contains(pag.nombre.ToUpper())) &&
                                         u.FlagActivo
                                         select new ParametroDTO()
                                         {
                                             Id = u.ParametroAppId,
                                             TipoParametroId = u.TipoParametroId,
                                             Codigo = u.Codigo,
                                             Descripcion = u.Descripcion,
                                             Valor = u.Valor,
                                             FechaCreacion = u.FechaCreacion,
                                             FechaModificacion = u.FechaModificacion,
                                             UsuarioCreacion = u.UsuarioCreacion,
                                             Activo = u.FlagActivo
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
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<AplicacionDTO> GetGestionAplicacion(PaginacionAplicacion pag, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<AplicacionDTO> GetGestionAplicacion(PaginacionAplicacion pag, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override int AddOrEditParametro(ParametroDTO objRegistro)
        {
            try
            {
                int ID = 0;
                DateTime FECHA_ACTUAL = DateTime.Now;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    if (objRegistro.Id == 0)
                    {
                        var entidad = new ParametroApp()
                        {
                            TipoParametroId = 1,
                            Codigo = objRegistro.Codigo,
                            Descripcion = objRegistro.Descripcion,
                            Valor = objRegistro.Valor,
                            FechaCreacion = FECHA_ACTUAL,
                            UsuarioCreacion = objRegistro.UsuarioCreacion,
                            FlagActivo = true
                        };
                        ctx.ParametroApp.Add(entidad);
                        ctx.SaveChanges();

                        ID = entidad.ParametroAppId;
                    }
                    else
                    {
                        var entidad = (from u in ctx.ParametroApp
                                       where u.ParametroAppId == objRegistro.Id
                                       && u.FlagActivo
                                       select u).FirstOrDefault();

                        if (entidad != null)
                        {
                            entidad.Valor = objRegistro.Valor;
                            entidad.FechaModificacion = FECHA_ACTUAL;
                            entidad.UsuarioModificacion = objRegistro.UsuarioModificacion;
                            ctx.SaveChanges();

                            ID = entidad.ParametroAppId;
                        }
                    }
                }
                return ID;
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: int AddOrEdit(AplicacionDTO objRegistro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: int AddOrEdit(AplicacionDTO objRegistro)"
                    , new object[] { null });
            }
        }

        public override ModuloAplicacionDTO GetModuloById(int Id)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.ModuloAplicacion
                                   where u.ModuloAplicacionId == Id
                                   select new ModuloAplicacionDTO()
                                   {
                                       CodigoAPT = u.CodigoAPT,
                                       Codigo = u.Codigo,
                                       CodigoInterfaz = u.CodigoInterfaz,
                                       Nombre = u.Nombre,
                                       Descripcion = u.Descripcion,
                                       TipoDesarrollo = u.TipoDesarrollo,
                                       InfraestructuraModulo = u.InfraestructuraModulo,
                                       CategoriaTecnologica = u.CategoriaTecnologica,
                                       ModeloEntrega = u.ModeloEntrega,
                                       CriticidadId = u.CriticidadId,
                                       EstadoModulo = u.EstadoModulo,
                                       InstaladaDesarrollo = u.InstaladaDesarrollo,
                                       InstaladaCertificacion = u.InstaladaCertificacion,
                                       InstaladaProduccion = u.InstaladaProduccion,
                                       Contingencia = u.Contingencia,
                                       MetodoAutenticacion = u.MetodoAutenticacion,
                                       MetodoAutorizacion = u.MetodoAutorizacion,
                                       FlagOOR = u.FlagOOR,
                                       FlagRatificaOOR = u.FlagRatificaOOR,
                                       NombreServidor = u.NombreServidor,
                                       CompatibilidadWindows = u.CompatibilidadWindows,
                                       CompatibilidadNavegador = u.CompatibilidadNavegador,
                                       CompatibilidadHV = u.CompatibilidadHV,
                                       NCET = u.NCET,
                                       NCG = u.NCG,
                                       NCLS = u.NCLS,
                                       ResumenSeguridadInformacion = u.ResumenSeguridadInformacion,
                                       Id = u.ModuloAplicacionId,
                                       FechaCreacion = u.FechaCreacion,
                                       UsuarioCreacion = u.UsuarioCreacion,
                                       Activo = u.FlagActivo
                                   }).FirstOrDefault();
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

        public override int AddOrEditModuloAplicacion(ModuloAplicacionDTO item)
        {
            try
            {
                int ID = 0;
                DateTime FECHA_ACTUAL = DateTime.Now;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    if (item.Id == 0)
                    {
                        var entidad = new ModuloAplicacion()
                        {
                            CodigoAPT = item.CodigoAPT,
                            Codigo = item.Codigo,
                            Nombre = item.Nombre,
                            Descripcion = item.Descripcion,
                            TipoDesarrollo = item.TipoDesarrollo,
                            InfraestructuraModulo = item.InfraestructuraModulo,
                            MetodoAutenticacion = item.MetodoAutenticacion,
                            MetodoAutorizacion = item.MetodoAutorizacion,
                            CategoriaTecnologica = item.CategoriaTecnologica,
                            ModeloEntrega = item.ModeloEntrega,
                            Contingencia = item.Contingencia,
                            FlagOOR = item.FlagOOR,
                            FlagRatificaOOR = item.FlagRatificaOOR,
                            CodigoInterfaz = item.CodigoInterfaz,
                            CompatibilidadHV = item.CompatibilidadHV,
                            NombreServidor = item.NombreServidor,
                            CompatibilidadWindows = item.CompatibilidadWindows,
                            CompatibilidadNavegador = item.CompatibilidadNavegador,
                            InstaladaDesarrollo = item.InstaladaDesarrollo,
                            InstaladaCertificacion = item.InstaladaCertificacion,
                            InstaladaProduccion = item.InstaladaProduccion,
                            NCET = item.NCET,
                            NCLS = item.NCLS,
                            NCG = item.NCG,
                            ResumenSeguridadInformacion = item.ResumenSeguridadInformacion,
                            EstadoModulo = item.EstadoModulo,
                            CriticidadId = item.CriticidadId,
                            FlagActivo = true,
                            FechaCreacion = FECHA_ACTUAL,
                            UsuarioCreacion = item.UsuarioCreacion
                        };

                        ctx.ModuloAplicacion.Add(entidad);
                        ctx.SaveChanges();

                        ID = entidad.ModuloAplicacionId;
                    }
                    else
                    {
                        var entidad = (from u in ctx.ModuloAplicacion
                                       where u.ModuloAplicacionId == item.Id
                                       //&& u.FlagActivo
                                       select u).FirstOrDefault();

                        if (entidad != null)
                        {
                            entidad.FechaModificacion = FECHA_ACTUAL;
                            entidad.UsuarioModificacion = item.UsuarioModificacion;

                            //CodigoAPT = item.CodigoAPT,
                            //Codigo = item.Codigo,
                            entidad.Nombre = item.Nombre;
                            entidad.Descripcion = item.Descripcion;
                            entidad.TipoDesarrollo = item.TipoDesarrollo;
                            entidad.InfraestructuraModulo = item.InfraestructuraModulo;
                            entidad.MetodoAutenticacion = item.MetodoAutenticacion;
                            entidad.MetodoAutorizacion = item.MetodoAutorizacion;
                            entidad.CategoriaTecnologica = item.CategoriaTecnologica;
                            entidad.ModeloEntrega = item.ModeloEntrega;
                            entidad.Contingencia = item.Contingencia;
                            entidad.FlagOOR = item.FlagOOR;
                            entidad.FlagRatificaOOR = item.FlagRatificaOOR;
                            entidad.CodigoInterfaz = item.CodigoInterfaz;
                            entidad.CompatibilidadHV = item.CompatibilidadHV;
                            entidad.NombreServidor = item.NombreServidor;
                            entidad.CompatibilidadWindows = item.CompatibilidadWindows;
                            entidad.CompatibilidadNavegador = item.CompatibilidadNavegador;
                            entidad.InstaladaDesarrollo = item.InstaladaDesarrollo;
                            entidad.InstaladaCertificacion = item.InstaladaCertificacion;
                            entidad.InstaladaProduccion = item.InstaladaProduccion;
                            entidad.NCET = item.NCET;
                            entidad.NCLS = item.NCLS;
                            entidad.NCG = item.NCG;
                            entidad.ResumenSeguridadInformacion = item.ResumenSeguridadInformacion;
                            entidad.EstadoModulo = item.EstadoModulo;
                            entidad.CriticidadId = item.CriticidadId;

                            ctx.SaveChanges();

                            ID = entidad.ModuloAplicacionId;
                        }
                    }
                }

                return ID;
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: int AddOrEdit(AplicacionDTO objRegistro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: int AddOrEdit(AplicacionDTO objRegistro)"
                    , new object[] { null });
            }
        }

        public override List<AplicacionDTO> GetAplicacionPortafolio(PaginacionAplicacion pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                var fechaConsulta = DateTime.Now;
                var lMatriculas = new List<string>();
                string lMatriculaAprobadores = Settings.Get<string>("Responsables.Portafolio");
                if (!string.IsNullOrEmpty(lMatriculaAprobadores))
                    lMatriculas = lMatriculaAprobadores.Split('|').ToList();
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        ctx.Database.CommandTimeout = 0;

                        var registros = (from u in ctx.Aplicacion
                                         join ad in ctx.AplicacionDetalle on u.AplicacionId equals ad.AplicacionId into lj0
                                         from ad in lj0.DefaultIfEmpty()
                                         join u2 in ctx.Criticidad on u.CriticidadId equals u2.CriticidadId
                                         join r in ctx.RoadMap on u.RoadMapId equals r.RoadMapId into lj1
                                         from r in lj1.DefaultIfEmpty()
                                         where (string.IsNullOrEmpty(pag.Aplicacion)
                                         || u.CodigoAPT.ToUpper().Contains(pag.Aplicacion.ToUpper())
                                         || (u.CodigoAPT + " - " + u.Nombre).ToUpper().Contains(pag.Aplicacion.ToUpper()))
                                         && (pag.Estado == null || u.EstadoAplicacion.ToUpper().Contains(pag.Estado.ToUpper()))
                                         && (pag.Gerencia == null || u.GerenciaCentral.ToUpper().Contains(pag.Gerencia.ToUpper()))
                                         && (pag.Division == null || u.Division.ToUpper().Contains(pag.Division.ToUpper()))
                                         && (pag.Unidad == null || u.Unidad.ToUpper().Contains(pag.Unidad.ToUpper()))
                                         && (pag.Area == null || u.Area.ToUpper().Contains(pag.Area.ToUpper()))
                                         && (string.IsNullOrEmpty(pag.JefeEquipo) || u.JefeEquipo_ExpertoAplicacionUserIT_ProductOwner.ToUpper().Contains(pag.JefeEquipo.ToUpper()))
                                         && (string.IsNullOrEmpty(pag.Owner) || u.Owner_LiderUsuario_ProductOwner.ToUpper().Contains(pag.Owner.ToUpper()))
                                         && (string.IsNullOrEmpty(lMatriculaAprobadores) || lMatriculas.Contains(pag.username))
                                         && u.FlagAprobado.Value
                                         orderby u.Nombre
                                         select new AplicacionDTO()
                                         {
                                             Id = u.AplicacionId,
                                             CodigoAPT = u.CodigoAPT,
                                             CodigoAPTStr = u.CodigoAPT,
                                             CategoriaTecnologica = u.CategoriaTecnologica,
                                             Nombre = u.Nombre,
                                             DescripcionAplicacion = u.DescripcionAplicacion,
                                             TipoActivoInformacion = u.TipoActivoInformacion,
                                             EstadoAplicacion = u.EstadoAplicacion,
                                             GestionadoPor = u.GestionadoPor,
                                             CriticidadId = u.CriticidadId,
                                             RoadMapId = u.RoadMapId,
                                             RoadMapToString = r.Nombre,
                                             Matricula = u.Matricula,
                                             Obsolescente = u.Obsolescente,
                                             //MesAnio = u.MesAnio,
                                             Activo = u.FlagActivo,
                                             UsuarioCreacion = u.CreadoPor,
                                             FechaCreacion = u.FechaCreacion,
                                             FechaRegistroProcedencia = u.FechaRegistroProcedencia,
                                             FechaModificacion = u.FechaModificacion,
                                             UsuarioModificacion = u.ModificadoPor,
                                             FlagRelacionar = u.FlagRelacionar,
                                             CriticidadToString = u2.DetalleCriticidad,
                                             GerenciaCentral = u.GerenciaCentral,
                                             Division = u.Division,
                                             Unidad = u.Unidad,
                                             Area = u.Area,
                                             AreaBIAN = u.AreaBIAN,
                                             DominioBIAN = u.DominioBIAN,
                                             JefaturaATI = u.JefaturaATI,
                                             NombreEquipo_Squad = u.NombreEquipo_Squad,
                                             Owner_LiderUsuario_ProductOwner = u.Owner_LiderUsuario_ProductOwner,
                                             Gestor_UsuarioAutorizador_ProductOwner = u.Gestor_UsuarioAutorizador_ProductOwner,
                                             JefeEquipo_ExpertoAplicacionUserIT_ProductOwner = u.JefeEquipo_ExpertoAplicacionUserIT_ProductOwner,
                                             BrokerSistemas = u.BrokerSistemas,
                                             TribeTechnicalLead = u.TribeTechnicalLead,
                                             Experto_Especialista = u.Experto_Especialista,
                                             FechaCreacionAplicacion = u.FechaCreacionAplicacion,
                                             ArquitectoTI = u.ArquitectoTI,
                                             EntidadResponsable = u.EntidadResponsable,
                                             ClasificacionTecnica = u.ClasificacionTecnica,
                                             SubclasificacionTecnica = u.SubclasificacionTecnica,
                                             //TotalEquiposRelacionados = s == null ? 0 : s.NroTecnologias
                                             AplicacionDetalle = new AplicacionDetalleDTO()
                                             {
                                                 Id = ad != null ? ad.AplicacionDetalleId : 0,
                                                 EstadoSolicitudId = ad != null ? ad.EstadoSolicitudId.Value : (int)EEstadoSolicitudAplicacion.Aprobado,
                                                 MotivoCreacion = ad != null ? ad.MotivoCreacion : "-1",
                                                 AmbienteInstalacion = ad != null ? ad.AmbienteInstalacion : "",
                                                 EntidadUso = ad != null ? ad.EntidadUso : "",
                                                 FechaSolicitud = ad != null ? ad.FechaSolicitud : u.FechaCreacion,
                                                 FlagOOR = ad != null ? ad.FlagOOR : null,
                                                 FlagRatificaOOR = ad.FlagRatificaOOR,
                                                 AplicacionReemplazo = ad != null ? ad.AplicacionReemplazo : "",
                                                 GrupoServiceDesk = ad != null ? ad.GrupoServiceDesk : "",
                                                 ModeloEntrega = ad != null ? ad.ModeloEntrega : "-1",
                                                 PersonaSolicitud = ad != null ? ad.PersonaSolicitud : "",
                                                 PlataformaBCP = ad != null ? ad.PlataformaBCP : "-1",
                                                 RutaRepositorio = ad != null ? ad.RutaRepositorio : "",
                                                 TipoDesarrollo = ad != null ? ad.TipoDesarrollo : "-1",
                                                 Proveedor = ad != null ? ad.Proveedor : "",
                                                 Ubicacion = ad != null ? ad.Ubicacion : "-1",
                                                 Infraestructura = ad != null ? ad.Infraestructura : "-1",
                                                 Contingencia = ad != null ? ad.Contingencia : "-1",
                                                 MetodoAutenticacion = ad != null ? ad.MetodoAutenticacion : "-1",
                                                 MetodoAutorizacion = ad != null ? ad.MetodoAutorizacion : "-1",
                                                 CodigoInterfaz = ad != null ? ad.CodigoInterfaz : "",
                                                 InterfazApp = ad != null ? ad.InterfazApp : "",
                                                 NombreServidor = ad != null ? ad.NombreServidor : "",
                                                 CompatibilidadWindows = ad != null ? ad.CompatibilidadWindows : "-1",
                                                 CompatibilidadNavegador = ad != null ? ad.CompatibilidadNavegador : "-1",
                                                 CompatibilidadHV = ad != null ? ad.CompatibilidadHV : "-1",
                                                 InstaladaDesarrollo = ad != null ? ad.InstaladaDesarrollo : "-1",
                                                 InstaladaCertificacion = ad != null ? ad.InstaladaCertificacion : "-1",
                                                 InstaladaProduccion = ad != null ? ad.InstaladaProduccion : "-1",
                                                 GrupoTicketRemedy = ad != null ? ad.GrupoTicketRemedy : "-1",
                                                 NCET = ad != null ? ad.NCET : "",
                                                 NCLS = ad != null ? ad.NCLS : "",
                                                 NCG = ad != null ? ad.NCG : "",
                                                 ResumenSeguridadInformacion = ad != null ? ad.ResumenSeguridadInformacion : "",
                                                 ProcesoClave = ad != null ? ad.ProcesoClave : "",
                                                 Confidencialidad = ad != null ? ad.Confidencialidad : "-1",
                                                 Integridad = ad != null ? ad.Integridad : "-1",
                                                 Disponibilidad = ad != null ? ad.Disponibilidad : "-1",
                                                 Privacidad = ad != null ? ad.Privacidad : "-1",
                                                 Clasificacion = ad != null ? ad.Clasificacion : "-1",
                                                 RoadmapPlanificado = ad != null ? ad.RoadmapPlanificado : "",
                                                 DetalleEstrategia = ad != null ? ad.DetalleEstrategia : "",
                                                 EstadoRoadmap = ad != null ? ad.EstadoRoadmap : "-1",
                                                 EtapaAtencion = ad != null ? ad.EtapaAtencion : "",
                                                 RoadmapEjecutado = ad != null ? ad.RoadmapEjecutado : "",
                                                 FechaInicioRoadmap = ad != null ? ad.FechaInicioRoadmap : "",
                                                 FechaFinRoadmap = ad != null ? ad.FechaFinRoadmap : "",
                                                 CodigoAppReemplazo = ad != null ? ad.CodigoAppReemplazo : "",
                                                 SWBase_SO = ad != null ? ad.SWBase_SO : "",
                                                 SWBase_HP = ad != null ? ad.SWBase_HP : "",
                                                 SWBase_BD = ad != null ? ad.SWBase_BD : "",
                                                 SWBase_LP = ad != null ? ad.SWBase_LP : "",
                                                 SWBase_Framework = ad != null ? ad.SWBase_Framework : "",
                                                 RET = ad != null ? ad.RET : "",
                                                 CriticidadAplicacionBIA = ad != null ? ad.CriticidadAplicacionBIA : "",
                                                 ProductoMasRepresentativo = ad != null ? ad.ProductoMasRepresentativo : "",
                                                 MenorRTO = ad != null ? ad.MenorRTO : "",
                                                 MayorGradoInterrupcion = ad != null ? ad.MayorGradoInterrupcion : "",
                                                 FlagFileCheckList = ad != null ? ad.FlagFileCheckList : false,
                                                 FlagFileMatriz = ad != null ? ad.FlagFileMatriz : false,
                                                 GestorAplicacionCTR = ad != null ? ad.GestorAplicacionCTR : "",
                                                 ConsultorCTR = ad != null ? ad.ConsultorCTR : "",
                                                 ValorL_NC = ad != null ? ad.ValorL_NC : "|",
                                                 ValorM_NC = ad != null ? ad.ValorM_NC : "|",
                                                 ValorN_NC = ad != null ? ad.ValorN_NC : "",
                                                 ValorPC_NC = ad != null ? ad.ValorPC_NC : ""
                                             }
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
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<AplicacionDTO> GetAplicacion(string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<AplicacionDTO> GetAplicacion(string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<AplicacionDTO> GetAplicacionConfiguracion(PaginacionAplicacion pag, out int totalRows, out string arrCodigoAPTs)
        {
            try
            {
                totalRows = 0;
                arrCodigoAPTs = string.Empty;
                var fechaConsulta = DateTime.Now;

                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<AplicacionDTO>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();

                    using (var comando = new SqlCommand("[cvt].[Usp_GetAplicacionConfiguracion]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@aplicacion", pag.Aplicacion));
                        comando.Parameters.Add(new SqlParameter("@estado", pag.Estado == null ? "" : pag.Estado));
                        comando.Parameters.Add(new SqlParameter("@UnidadFondeoId", pag.UnidadFondeoId));
                        comando.Parameters.Add(new SqlParameter("@matricula", pag.username == null ? "" : pag.username));
                        comando.Parameters.Add(new SqlParameter("@gerenciaCentral", pag.Gerencia == null ? "" : pag.Gerencia));
                        comando.Parameters.Add(new SqlParameter("@division", pag.Division == null ? "" : pag.Division));
                        comando.Parameters.Add(new SqlParameter("@unidad", pag.Unidad == null ? "" : pag.Unidad));
                        comando.Parameters.Add(new SqlParameter("@area", pag.Area == null ? "" : pag.Area));
                        comando.Parameters.Add(new SqlParameter("@jefeEquipo", pag.JefeEquipo == null ? "" : pag.JefeEquipo));
                        comando.Parameters.Add(new SqlParameter("@owner", pag.Owner == null ? "" : pag.Owner));
                        comando.Parameters.Add(new SqlParameter("@perfilId", pag.PerfilId));
                        comando.Parameters.Add(new SqlParameter("@anioRegistro", fechaConsulta.Year));
                        comando.Parameters.Add(new SqlParameter("@mesRegistro", fechaConsulta.Month));
                        comando.Parameters.Add(new SqlParameter("@diaRegistro", fechaConsulta.Day));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new AplicacionDTO()
                            {
                                Id = reader.IsDBNull(reader.GetOrdinal("Id")) ? 0 : reader.GetInt32(reader.GetOrdinal("Id")),
                                CodigoAPT = reader.IsDBNull(reader.GetOrdinal("CodigoAPT")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAPT")),
                                CodigoAPTStr = reader.IsDBNull(reader.GetOrdinal("CodigoAPTStr")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAPTStr")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                GestionadoPor = reader.IsDBNull(reader.GetOrdinal("GestionadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("GestionadoPor")),
                                CriticidadId = reader.IsDBNull(reader.GetOrdinal("CriticidadId")) ? 0 : reader.GetInt32(reader.GetOrdinal("CriticidadId")),
                                RoadMapId = reader.IsDBNull(reader.GetOrdinal("RoadMapId")) ? 0 : reader.GetInt32(reader.GetOrdinal("RoadMapId")),
                                RoadMapToString = reader.IsDBNull(reader.GetOrdinal("RoadMapToString")) ? string.Empty : reader.GetString(reader.GetOrdinal("RoadMapToString")),
                                Matricula = reader.IsDBNull(reader.GetOrdinal("Matricula")) ? string.Empty : reader.GetString(reader.GetOrdinal("Matricula")),
                                Obsolescente = reader.IsDBNull(reader.GetOrdinal("Obsolescente")) ? 0 : reader.GetInt32(reader.GetOrdinal("Obsolescente")),
                                Activo = reader.GetBoolean(reader.GetOrdinal("Activo")),
                                UsuarioCreacion = reader.IsDBNull(reader.GetOrdinal("UsuarioCreacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("UsuarioCreacion")),
                                FechaCreacion = reader.IsDBNull(reader.GetOrdinal("FechaCreacion")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("FechaCreacion")),
                                FechaRegistroProcedencia = reader.IsDBNull(reader.GetOrdinal("FechaRegistroProcedencia")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("FechaRegistroProcedencia")),
                                FechaModificacion = reader.IsDBNull(reader.GetOrdinal("FechaModificacion")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("FechaModificacion")),
                                UsuarioModificacion = reader.IsDBNull(reader.GetOrdinal("UsuarioModificacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("UsuarioModificacion")),
                                FlagRelacionar = reader.IsDBNull(reader.GetOrdinal("FlagRelacionar")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("FlagRelacionar")),
                                CriticidadToString = reader.IsDBNull(reader.GetOrdinal("CriticidadToString")) ? string.Empty : reader.GetString(reader.GetOrdinal("CriticidadToString")),
                                TipoActivoInformacion = reader.IsDBNull(reader.GetOrdinal("TipoActivoInformacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoActivoInformacion")),
                                GerenciaCentral = reader.IsDBNull(reader.GetOrdinal("GerenciaCentral")) ? string.Empty : reader.GetString(reader.GetOrdinal("GerenciaCentral")),
                                Division = reader.IsDBNull(reader.GetOrdinal("Division")) ? string.Empty : reader.GetString(reader.GetOrdinal("Division")),
                                Unidad = reader.IsDBNull(reader.GetOrdinal("Unidad")) ? string.Empty : reader.GetString(reader.GetOrdinal("Unidad")),
                                Area = reader.IsDBNull(reader.GetOrdinal("Area")) ? string.Empty : reader.GetString(reader.GetOrdinal("Area")),
                                NombreEquipo_Squad = reader.IsDBNull(reader.GetOrdinal("NombreEquipo_Squad")) ? string.Empty : reader.GetString(reader.GetOrdinal("NombreEquipo_Squad")),
                                Owner_LiderUsuario_ProductOwner = reader.IsDBNull(reader.GetOrdinal("Owner_LiderUsuario_ProductOwner")) ? string.Empty : reader.GetString(reader.GetOrdinal("Owner_LiderUsuario_ProductOwner")),
                                JefeEquipo_ExpertoAplicacionUserIT_ProductOwner = reader.IsDBNull(reader.GetOrdinal("JefeEquipo_ExpertoAplicacionUserIT_ProductOwner")) ? string.Empty : reader.GetString(reader.GetOrdinal("JefeEquipo_ExpertoAplicacionUserIT_ProductOwner")),
                                Experto_Especialista = reader.IsDBNull(reader.GetOrdinal("Experto_Especialista")) ? string.Empty : reader.GetString(reader.GetOrdinal("Experto_Especialista")),
                                TotalEquiposRelacionados = reader.IsDBNull(reader.GetOrdinal("TotalEquiposRelacionados")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalEquiposRelacionados")),
                                EstadoAplicacion = reader.IsDBNull(reader.GetOrdinal("EstadoAplicacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("EstadoAplicacion")),
                                Agrupacion = reader.IsDBNull(reader.GetOrdinal("Agrupacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("Agrupacion")),
                                UnidadFondeo = reader.IsDBNull(reader.GetOrdinal("UnidadFondeo")) ? string.Empty : reader.GetString(reader.GetOrdinal("UnidadFondeo")),
                                TipoExperto = reader.IsDBNull(reader.GetOrdinal("TipoExpertoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TipoExpertoId"))
                            };

                            lista.Add(objeto);
                        }
                        reader.Close();
                    }

                    arrCodigoAPTs = string.Join("|", lista.Select(x => x.CodigoAPT).ToArray());
                    totalRows = lista.Count();
                    var resultado = lista.Skip((pag.pageNumber - 1) * pag.pageSize).Take(pag.pageSize).ToList();

                    return resultado;

                }

            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<AplicacionDTO> GetAplicacion(string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<AplicacionDTO> GetAplicacion(string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<AplicacionDTO> GetAplicacionConfiguracionExportar(PaginacionAplicacion pag, out int totalRows)
        {
            try
            {
                var fechaConsulta = DateTime.Now;
                totalRows = 0;
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<AplicacionDTO>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();

                    using (var comando = new SqlCommand("[cvt].[Usp_GetAplicacionConfiguracion]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@aplicacion", pag.Aplicacion));
                        comando.Parameters.Add(new SqlParameter("@estado", pag.Estado == null ? "" : pag.Estado));
                        comando.Parameters.Add(new SqlParameter("@UnidadFondeoId", pag.UnidadFondeoId));
                        comando.Parameters.Add(new SqlParameter("@matricula", pag.username == null ? "" : pag.username));
                        comando.Parameters.Add(new SqlParameter("@gerenciaCentral", pag.Gerencia == null ? "" : pag.Gerencia));
                        comando.Parameters.Add(new SqlParameter("@division", pag.Division == null ? "" : pag.Division));
                        comando.Parameters.Add(new SqlParameter("@unidad", pag.Unidad == null ? "" : pag.Unidad));
                        comando.Parameters.Add(new SqlParameter("@area", pag.Area == null ? "" : pag.Area));
                        comando.Parameters.Add(new SqlParameter("@jefeEquipo", pag.JefeEquipo == null ? "" : pag.JefeEquipo));
                        comando.Parameters.Add(new SqlParameter("@owner", pag.Owner == null ? "" : pag.Owner));
                        comando.Parameters.Add(new SqlParameter("@perfilId", pag.PerfilId));
                        comando.Parameters.Add(new SqlParameter("@anioRegistro", fechaConsulta.Year));
                        comando.Parameters.Add(new SqlParameter("@mesRegistro", fechaConsulta.Month));
                        comando.Parameters.Add(new SqlParameter("@diaRegistro", fechaConsulta.Day));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new AplicacionDTO()
                            {
                                Id = reader.IsDBNull(reader.GetOrdinal("Id")) ? 0 : reader.GetInt32(reader.GetOrdinal("Id")),
                                CodigoAPT = reader.IsDBNull(reader.GetOrdinal("CodigoAPT")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAPT")),
                                CodigoAPTStr = reader.IsDBNull(reader.GetOrdinal("CodigoAPTStr")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAPTStr")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                GestionadoPor = reader.IsDBNull(reader.GetOrdinal("GestionadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("GestionadoPor")),
                                CriticidadId = reader.IsDBNull(reader.GetOrdinal("CriticidadId")) ? 0 : reader.GetInt32(reader.GetOrdinal("CriticidadId")),
                                RoadMapId = reader.IsDBNull(reader.GetOrdinal("RoadMapId")) ? 0 : reader.GetInt32(reader.GetOrdinal("RoadMapId")),
                                RoadMapToString = reader.IsDBNull(reader.GetOrdinal("RoadMapToString")) ? string.Empty : reader.GetString(reader.GetOrdinal("RoadMapToString")),
                                Matricula = reader.IsDBNull(reader.GetOrdinal("Matricula")) ? string.Empty : reader.GetString(reader.GetOrdinal("Matricula")),
                                Obsolescente = reader.IsDBNull(reader.GetOrdinal("Obsolescente")) ? 0 : reader.GetInt32(reader.GetOrdinal("Obsolescente")),
                                Activo = reader.GetBoolean(reader.GetOrdinal("Activo")),
                                UsuarioCreacion = reader.IsDBNull(reader.GetOrdinal("UsuarioCreacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("UsuarioCreacion")),
                                FechaCreacion = reader.IsDBNull(reader.GetOrdinal("FechaCreacion")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("FechaCreacion")),
                                FechaRegistroProcedencia = reader.IsDBNull(reader.GetOrdinal("FechaRegistroProcedencia")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("FechaRegistroProcedencia")),
                                FechaModificacion = reader.IsDBNull(reader.GetOrdinal("FechaModificacion")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("FechaModificacion")),
                                UsuarioModificacion = reader.IsDBNull(reader.GetOrdinal("UsuarioModificacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("UsuarioModificacion")),
                                FlagRelacionar = reader.IsDBNull(reader.GetOrdinal("FlagRelacionar")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("FlagRelacionar")),
                                CriticidadToString = reader.IsDBNull(reader.GetOrdinal("CriticidadToString")) ? string.Empty : reader.GetString(reader.GetOrdinal("CriticidadToString")),
                                TipoActivoInformacion = reader.IsDBNull(reader.GetOrdinal("TipoActivoInformacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoActivoInformacion")),
                                GerenciaCentral = reader.IsDBNull(reader.GetOrdinal("GerenciaCentral")) ? string.Empty : reader.GetString(reader.GetOrdinal("GerenciaCentral")),
                                Division = reader.IsDBNull(reader.GetOrdinal("Division")) ? string.Empty : reader.GetString(reader.GetOrdinal("Division")),
                                Unidad = reader.IsDBNull(reader.GetOrdinal("Unidad")) ? string.Empty : reader.GetString(reader.GetOrdinal("Unidad")),
                                Area = reader.IsDBNull(reader.GetOrdinal("Area")) ? string.Empty : reader.GetString(reader.GetOrdinal("Area")),
                                NombreEquipo_Squad = reader.IsDBNull(reader.GetOrdinal("NombreEquipo_Squad")) ? string.Empty : reader.GetString(reader.GetOrdinal("NombreEquipo_Squad")),
                                Owner_LiderUsuario_ProductOwner = reader.IsDBNull(reader.GetOrdinal("Owner_LiderUsuario_ProductOwner")) ? string.Empty : reader.GetString(reader.GetOrdinal("Owner_LiderUsuario_ProductOwner")),
                                JefeEquipo_ExpertoAplicacionUserIT_ProductOwner = reader.IsDBNull(reader.GetOrdinal("JefeEquipo_ExpertoAplicacionUserIT_ProductOwner")) ? string.Empty : reader.GetString(reader.GetOrdinal("JefeEquipo_ExpertoAplicacionUserIT_ProductOwner")),
                                Experto_Especialista = reader.IsDBNull(reader.GetOrdinal("Experto_Especialista")) ? string.Empty : reader.GetString(reader.GetOrdinal("Experto_Especialista")),
                                TotalEquiposRelacionados = reader.IsDBNull(reader.GetOrdinal("TotalEquiposRelacionados")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalEquiposRelacionados")),
                                EstadoAplicacion = reader.IsDBNull(reader.GetOrdinal("EstadoAplicacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("EstadoAplicacion")),
                                Agrupacion = reader.IsDBNull(reader.GetOrdinal("Agrupacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("Agrupacion")),
                                UnidadFondeo = reader.IsDBNull(reader.GetOrdinal("UnidadFondeo")) ? string.Empty : reader.GetString(reader.GetOrdinal("UnidadFondeo"))
                            };

                            lista.Add(objeto);
                        }
                        reader.Close();
                    }
                    totalRows = lista.Count();
                    return lista;

                }

            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<AplicacionDTO> GetAplicacion(string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<AplicacionDTO> GetAplicacion(string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<AplicacionCargaDto> GetReporteAplicacionData(PaginacionAplicacion pag, out int totalRows)
        {
            pag.Gerencia = (pag.Gerencia == "-1" || pag.Gerencia == null) ? "" : pag.Gerencia;
            pag.Division = (pag.Division == "-1" || pag.Division == null) ? "" : pag.Division;
            pag.Unidad = (pag.Unidad == "-1" || pag.Unidad == null) ? "" : pag.Unidad;
            pag.Area = (pag.Area == "-1" || pag.Area == null) ? "" : pag.Area;
            pag.Estado = (pag.Estado == "-1" || pag.Estado == null) ? "" : pag.Estado;

            totalRows = 0;

            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<AplicacionCargaDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[CVT].[USP_Reporte_Data_Aplicacion]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@gerencia", pag.Gerencia));
                        comando.Parameters.Add(new SqlParameter("@division", pag.Division));
                        //comando.Parameters.Add(new SqlParameter("@aplicacion", pag.Aplicacion));
                        comando.Parameters.Add(new SqlParameter("@area", pag.Area));
                        comando.Parameters.Add(new SqlParameter("@unidad", pag.Unidad));
                        comando.Parameters.Add(new SqlParameter("@estado", pag.Estado));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pag.pageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pag.pageNumber));
                        comando.Parameters.Add(new SqlParameter("@OrderBy", pag.sortName));
                        comando.Parameters.Add(new SqlParameter("@OrderByDirection", pag.sortOrder));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new AplicacionCargaDto()
                            {
                                CodigoAPT = reader.IsDBNull(reader.GetOrdinal("CodigoAPT")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAPT")), //Grilla
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")), //Grilla
                                CodigoInterfaz = reader.IsDBNull(reader.GetOrdinal("CodigoInterfaz")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoInterfaz")),
                                TipoActivoInformacion = reader.IsDBNull(reader.GetOrdinal("TipoActivoInformacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoActivoInformacion")),
                                GerenciaCentral = reader.IsDBNull(reader.GetOrdinal("GerenciaCentral")) ? string.Empty : reader.GetString(reader.GetOrdinal("GerenciaCentral")), //Grilla
                                Division = reader.IsDBNull(reader.GetOrdinal("Division")) ? string.Empty : reader.GetString(reader.GetOrdinal("Division")), //Grilla
                                Area = reader.IsDBNull(reader.GetOrdinal("Area")) ? string.Empty : reader.GetString(reader.GetOrdinal("Area")), //Grilla
                                Unidad = reader.IsDBNull(reader.GetOrdinal("Unidad")) ? string.Empty : reader.GetString(reader.GetOrdinal("Unidad")), //Grilla
                                DescripcionAplicacion = reader.IsDBNull(reader.GetOrdinal("DescripcionAplicacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("DescripcionAplicacion")),
                                EstadoAplicacion = reader.IsDBNull(reader.GetOrdinal("EstadoAplicacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("EstadoAplicacion")), //Grilla
                                FechaCreacionProcedencia = reader.IsDBNull(reader.GetOrdinal("FechaCreacionProcedencia")) ? string.Empty : reader.GetString(reader.GetOrdinal("FechaCreacionProcedencia")),

                                MotivoCreacion = reader.IsDBNull(reader.GetOrdinal("MotivoCreacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("MotivoCreacion")),
                                AreaBIAN = reader.IsDBNull(reader.GetOrdinal("AreaBIAN")) ? string.Empty : reader.GetString(reader.GetOrdinal("AreaBIAN")),
                                DominioBIAN = reader.IsDBNull(reader.GetOrdinal("DominioBIAN")) ? string.Empty : reader.GetString(reader.GetOrdinal("DominioBIAN")),
                                PlataformaBCP = reader.IsDBNull(reader.GetOrdinal("PlataformaBCP")) ? string.Empty : reader.GetString(reader.GetOrdinal("PlataformaBCP")),
                                JefaturaATI = reader.IsDBNull(reader.GetOrdinal("JefaturaATI")) ? string.Empty : reader.GetString(reader.GetOrdinal("JefaturaATI")),
                                TribeTechnicalLead = reader.IsDBNull(reader.GetOrdinal("TribeTechnicalLead")) ? string.Empty : reader.GetString(reader.GetOrdinal("TribeTechnicalLead")),
                                MatriculaTTL = reader.IsDBNull(reader.GetOrdinal("MatriculaTTL")) ? string.Empty : reader.GetString(reader.GetOrdinal("MatriculaTTL")),
                                JefeEquipo_ExpertoAplicacionUserIT_ProductOwner = reader.IsDBNull(reader.GetOrdinal("JefeEquipo_ExpertoAplicacionUserIT_ProductOwner")) ? string.Empty : reader.GetString(reader.GetOrdinal("JefeEquipo_ExpertoAplicacionUserIT_ProductOwner")),
                                MatriculaJDE = reader.IsDBNull(reader.GetOrdinal("MatriculaJDE")) ? string.Empty : reader.GetString(reader.GetOrdinal("MatriculaJDE")),
                                BrokerSistemas = reader.IsDBNull(reader.GetOrdinal("BrokerSistemas")) ? string.Empty : reader.GetString(reader.GetOrdinal("BrokerSistemas")),

                                MatriculaBroker = reader.IsDBNull(reader.GetOrdinal("MatriculaBroker")) ? string.Empty : reader.GetString(reader.GetOrdinal("MatriculaBroker")),
                                NombreEquipo_Squad = reader.IsDBNull(reader.GetOrdinal("NombreEquipo_Squad")) ? string.Empty : reader.GetString(reader.GetOrdinal("NombreEquipo_Squad")),
                                GestionadoPor = reader.IsDBNull(reader.GetOrdinal("GestionadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("GestionadoPor")), //Grilla
                                Owner_LiderUsuario_ProductOwner = reader.IsDBNull(reader.GetOrdinal("Owner_LiderUsuario_ProductOwner")) ? string.Empty : reader.GetString(reader.GetOrdinal("Owner_LiderUsuario_ProductOwner")),
                                MatriculaOwner = reader.IsDBNull(reader.GetOrdinal("MatriculaOwner")) ? string.Empty : reader.GetString(reader.GetOrdinal("MatriculaOwner")),
                                Gestor_UsuarioAutorizador_ProductOwner = reader.IsDBNull(reader.GetOrdinal("Gestor_UsuarioAutorizador_ProductOwner")) ? string.Empty : reader.GetString(reader.GetOrdinal("Gestor_UsuarioAutorizador_ProductOwner")),
                                MatriculaGestor = reader.IsDBNull(reader.GetOrdinal("MatriculaGestor")) ? string.Empty : reader.GetString(reader.GetOrdinal("MatriculaGestor")),
                                Experto_Especialista = reader.IsDBNull(reader.GetOrdinal("Experto_Especialista")) ? string.Empty : reader.GetString(reader.GetOrdinal("Experto_Especialista")),
                                MatriculaExperto = reader.IsDBNull(reader.GetOrdinal("MatriculaExperto")) ? string.Empty : reader.GetString(reader.GetOrdinal("MatriculaExperto")),
                                EntidadResponsable = reader.IsDBNull(reader.GetOrdinal("EntidadResponsable")) ? string.Empty : reader.GetString(reader.GetOrdinal("EntidadResponsable")),

                                EntidadUsuaria = reader.IsDBNull(reader.GetOrdinal("EntidadUsuaria")) ? string.Empty : reader.GetString(reader.GetOrdinal("EntidadUsuaria")),
                                NombreEntidadUsuaria = reader.IsDBNull(reader.GetOrdinal("NombreEntidadUsuaria")) ? string.Empty : reader.GetString(reader.GetOrdinal("NombreEntidadUsuaria")),
                                ProcesoClave = reader.IsDBNull(reader.GetOrdinal("ProcesoClave")) ? string.Empty : reader.GetString(reader.GetOrdinal("ProcesoClave")),
                                Clasificacion = reader.IsDBNull(reader.GetOrdinal("Clasificacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("Clasificacion")),
                                Criticidad = reader.IsDBNull(reader.GetOrdinal("Criticidad")) ? string.Empty : reader.GetString(reader.GetOrdinal("Criticidad")), //Grilla
                                CategoriaTecnologica = reader.IsDBNull(reader.GetOrdinal("CategoriaTecnologica")) ? string.Empty : reader.GetString(reader.GetOrdinal("CategoriaTecnologica")),
                                TipoDesarrollo = reader.IsDBNull(reader.GetOrdinal("TipoDesarrollo")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoDesarrollo")),
                                ModeloEntrega = reader.IsDBNull(reader.GetOrdinal("ModeloEntrega")) ? string.Empty : reader.GetString(reader.GetOrdinal("ModeloEntrega")),
                                ProveedorDesarrollo = reader.IsDBNull(reader.GetOrdinal("ProveedorDesarrollo")) ? string.Empty : reader.GetString(reader.GetOrdinal("ProveedorDesarrollo")),
                                Ubicacion = reader.IsDBNull(reader.GetOrdinal("Ubicacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("Ubicacion")),

                                Infraestructura = reader.IsDBNull(reader.GetOrdinal("Infraestructura")) ? string.Empty : reader.GetString(reader.GetOrdinal("Infraestructura")),
                                ClasificacionTecnica = reader.IsDBNull(reader.GetOrdinal("ClasificacionTecnica")) ? string.Empty : reader.GetString(reader.GetOrdinal("ClasificacionTecnica")),
                                NombreInterface = reader.IsDBNull(reader.GetOrdinal("NombreInterface")) ? string.Empty : reader.GetString(reader.GetOrdinal("NombreInterface")),
                                NombreServidor = reader.IsDBNull(reader.GetOrdinal("NombreServidor")) ? string.Empty : reader.GetString(reader.GetOrdinal("NombreServidor")),
                                Repositorio = reader.IsDBNull(reader.GetOrdinal("Repositorio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Repositorio")),
                                Contingencia = reader.IsDBNull(reader.GetOrdinal("Contingencia")) ? string.Empty : reader.GetString(reader.GetOrdinal("Contingencia")),
                                MetodoAutenticacion = reader.IsDBNull(reader.GetOrdinal("MetodoAutenticacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("MetodoAutenticacion")),
                                MetodoAutorizacion = reader.IsDBNull(reader.GetOrdinal("MetodoAutorizacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("MetodoAutorizacion")),
                                CompatibleWindows7 = reader.IsDBNull(reader.GetOrdinal("CompatibleWindows7")) ? string.Empty : reader.GetString(reader.GetOrdinal("CompatibleWindows7")),
                                CompatibleNavegador = reader.IsDBNull(reader.GetOrdinal("CompatibleNavegador")) ? string.Empty : reader.GetString(reader.GetOrdinal("CompatibleNavegador")),
                                CompatibleHV = reader.IsDBNull(reader.GetOrdinal("CompatibleHV")) ? string.Empty : reader.GetString(reader.GetOrdinal("CompatibleHV")),

                                InstaladaDesarrollo = reader.IsDBNull(reader.GetOrdinal("InstaladaDesarrollo")) ? string.Empty : reader.GetString(reader.GetOrdinal("InstaladaDesarrollo")),
                                InstaladaCertificacion = reader.IsDBNull(reader.GetOrdinal("InstaladaCertificacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("InstaladaCertificacion")),
                                InstaladaProduccion = reader.IsDBNull(reader.GetOrdinal("InstaladaProduccion")) ? string.Empty : reader.GetString(reader.GetOrdinal("InstaladaProduccion")),
                                GTR = reader.IsDBNull(reader.GetOrdinal("GTR")) ? string.Empty : reader.GetString(reader.GetOrdinal("GTR")),
                                OOR = reader.IsDBNull(reader.GetOrdinal("OOR")) ? string.Empty : reader.GetString(reader.GetOrdinal("OOR")),
                                RatificaOOR = reader.IsDBNull(reader.GetOrdinal("RatificaOOR")) ? string.Empty : reader.GetString(reader.GetOrdinal("RatificaOOR")),
                                SWBaseSO = reader.IsDBNull(reader.GetOrdinal("SWBaseSO")) ? string.Empty : reader.GetString(reader.GetOrdinal("SWBaseSO")),
                                SWBaseHP = reader.IsDBNull(reader.GetOrdinal("SWBaseHP")) ? string.Empty : reader.GetString(reader.GetOrdinal("SWBaseHP")),
                                SWBaseLP = reader.IsDBNull(reader.GetOrdinal("SWBaseLP")) ? string.Empty : reader.GetString(reader.GetOrdinal("SWBaseLP")),
                                SWBaseBD = reader.IsDBNull(reader.GetOrdinal("SWBaseBD")) ? string.Empty : reader.GetString(reader.GetOrdinal("SWBaseBD")),

                                SWBaseFramework = reader.IsDBNull(reader.GetOrdinal("SWBaseFramework")) ? string.Empty : reader.GetString(reader.GetOrdinal("SWBaseFramework")),
                                RET = reader.IsDBNull(reader.GetOrdinal("RET")) ? string.Empty : reader.GetString(reader.GetOrdinal("RET")),
                                NCET = reader.IsDBNull(reader.GetOrdinal("NCET")) ? string.Empty : reader.GetString(reader.GetOrdinal("NCET")),
                                RLSI = reader.IsDBNull(reader.GetOrdinal("RLSI")) ? string.Empty : reader.GetString(reader.GetOrdinal("RLSI")),
                                NCLS = reader.IsDBNull(reader.GetOrdinal("NCLS")) ? string.Empty : reader.GetString(reader.GetOrdinal("NCLS")),
                                NCG = reader.IsDBNull(reader.GetOrdinal("NCG")) ? string.Empty : reader.GetString(reader.GetOrdinal("NCG")),
                                Roadmap = reader.IsDBNull(reader.GetOrdinal("Roadmap")) ? string.Empty : reader.GetString(reader.GetOrdinal("Roadmap")),
                                DetalleEstrategiaRoadmapPlanificado = reader.IsDBNull(reader.GetOrdinal("DetalleEstrategiaRoadmapPlanificado")) ? string.Empty : reader.GetString(reader.GetOrdinal("DetalleEstrategiaRoadmapPlanificado")),
                                EstadoRoadmap = reader.IsDBNull(reader.GetOrdinal("EstadoRoadmap")) ? string.Empty : reader.GetString(reader.GetOrdinal("EstadoRoadmap")),
                                EtapaAtencionRoadmap = reader.IsDBNull(reader.GetOrdinal("EtapaAtencionRoadmap")) ? string.Empty : reader.GetString(reader.GetOrdinal("EtapaAtencionRoadmap")),

                                RoadmapEjecutado = reader.IsDBNull(reader.GetOrdinal("RoadmapEjecutado")) ? string.Empty : reader.GetString(reader.GetOrdinal("RoadmapEjecutado")),
                                FechaInicioRoadmapEjecutado = reader.IsDBNull(reader.GetOrdinal("FechaInicioRoadmapEjecutado")) ? string.Empty : reader.GetString(reader.GetOrdinal("FechaInicioRoadmapEjecutado")),
                                FechaFinRoadmapEjecutado = reader.IsDBNull(reader.GetOrdinal("FechaFinRoadmapEjecutado")) ? string.Empty : reader.GetString(reader.GetOrdinal("FechaFinRoadmapEjecutado")),
                                CodigoAplicacionReemplazo = reader.IsDBNull(reader.GetOrdinal("CodigoAplicacionReemplazo")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAplicacionReemplazo")),
                                AplicativoReemplazo = reader.IsDBNull(reader.GetOrdinal("AplicativoReemplazo")) ? string.Empty : reader.GetString(reader.GetOrdinal("AplicativoReemplazo")),
                                FechaRegistroProcedencia = reader.IsDBNull(reader.GetOrdinal("FechaRegistroProcedencia")) ? string.Empty : reader.GetString(reader.GetOrdinal("FechaRegistroProcedencia")), //Grilla

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
                    , "Error en el metodo: GetGerenciaDivision()"
                    , new object[] { null });
            }
        }

        public override bool ExisteAplicacionByNombre(string nombre, int id)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    bool? estado = (from u in ctx.Aplicacion
                                    where u.FlagActivo
                                    && u.Nombre.ToUpper().Equals(nombre.ToUpper())
                                    && u.AplicacionId != id
                                    orderby u.Nombre
                                    select true).FirstOrDefault();

                    return estado == true;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: bool ExisteAplicacion(string nombre, string Id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: bool ExisteAplicacion(string nombre, string Id)"
                    , new object[] { null });
            }
        }

        public override bool ExisteCodigoInterfaz(string filtro, int id)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    bool? estado = (from u in ctx.AplicacionDetalle
                                    where u.FlagActivo
                                    && u.CodigoInterfaz.ToUpper().Equals(filtro.ToUpper())
                                    && u.AplicacionId != id
                                    orderby u.CodigoInterfaz
                                    select true).FirstOrDefault();

                    return estado == true;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: bool ExisteAplicacion(string nombre, string Id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: bool ExisteAplicacion(string nombre, string Id)"
                    , new object[] { null });
            }
        }

        public override List<string> GetCodigoInterfazUsados(int id)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var lista = (from u in ctx.AplicacionDetalle
                                 where u.FlagActivo
                                 && u.AplicacionId != id
                                 orderby u.CodigoInterfaz
                                 select u.CodigoInterfaz.ToUpper()).ToList();

                    return lista;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: bool ExisteAplicacion(string nombre, string Id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: bool ExisteAplicacion(string nombre, string Id)"
                    , new object[] { null });
            }
        }



        public override ConfiguracionColumnaAplicacionDTO GetColumnaAppById(int id)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.ConfiguracionColumnaAplicacion
                                   where u.ConfiguracionColumnaAplicacionId == id
                                   select new ConfiguracionColumnaAplicacionDTO()
                                   {
                                       Id = u.ConfiguracionColumnaAplicacionId,
                                       TablaProcedenciaId = u.TablaProcedencia.Value,
                                       FlagEdicion = u.FlagEdicion,
                                       FlagVerExportar = u.FlagVerExportar,
                                       OrdenColumna = u.OrdenColumna.Value,
                                       FlagCampoNuevo = u.FlagCampoNuevo,
                                       FlagModificable = u.FlagModificable,
                                       FlagObligatorio = u.FlagObligatorio,
                                       FechaCreacion = u.FechaCreacion,
                                       UsuarioCreacion = u.UsuarioCreacion,
                                       Activo = u.FlagActivo,
                                       NombreExcel = u.NombreExcel,

                                       ActivoAplica = u.ActivoAplica,
                                       ModoLlenado = u.ModoLlenado,
                                       RolRegistra = u.RolRegistra,
                                       RolAprueba = u.RolAprueba,

                                       DescripcionCampo = u.DescripcionCampo,
                                       NivelConfiabilidad = u.NivelConfiabilidad,
                                       RolResponsableActualizacion = u.RolResponsableActualizacion,
                                       TipoRegistro = u.TipoRegistro
                                   }).FirstOrDefault();

                    if (entidad != null)
                    {
                        var infoCampo = ctx.InfoCampoPortafolio.FirstOrDefault(x => x.FlagActivo
                        && x.ConfiguracionColumnaAplicacionId == entidad.Id);
                        if (infoCampo != null)
                        {
                            var itemDTO = new InfoCampoPortafolioDTO()
                            {
                                Id = infoCampo.InfoCampoPortafolioId,
                                ToolTip = infoCampo.ToolTip,
                                TipoFlujoId = infoCampo.TipoFlujoId,
                                Nombre = infoCampo.Nombre,
                                TipoInputId = infoCampo.TipoInputId,
                                ParametricaDescripcion = infoCampo.ParametricaDescripcion,
                                MantenimientoPortafolioId = infoCampo.MantenimientoPortafolioId
                            };
                            entidad.InfoCampoPortafolio = itemDTO;

                            if (infoCampo.TipoInputId == (int)ETipoInputHTML.ListBox)
                            {
                                if (infoCampo.MantenimientoPortafolioId == (int)EConfiguracionPortafolio.Parametrica)
                                {
                                    var dataListBox = ServiceManager<ActivosDAO>.Provider.GetDataListBoxByConfiguracion(entidad.Id);
                                    entidad.InfoCampoPortafolio.DataListBoxDetalle = dataListBox;
                                }
                            }
                        }
                    }

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

        public override FiltrosDashboardAplicacion ListFiltrosResumen()
        {
            try
            {
                FiltrosDashboardAplicacion arr_data = null;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    arr_data = new FiltrosDashboardAplicacion();
                    arr_data.EstadoAplicacion = (from u in ctx.Aplicacion
                                                 where u.FlagActivo
                                                 select u.EstadoAplicacion).Distinct().OrderBy(x => x).ToArray();

                    arr_data.JefeEquipo = (from u in ctx.Aplicacion
                                           where u.FlagActivo && u.JefeEquipo_ExpertoAplicacionUserIT_ProductOwner != ""
                                           select u.JefeEquipo_ExpertoAplicacionUserIT_ProductOwner).Distinct().OrderBy(x => x).ToArray();

                    arr_data.LiderUsuario = (from u in ctx.Aplicacion
                                             where u.FlagActivo && u.Owner_LiderUsuario_ProductOwner != ""
                                             select u.Owner_LiderUsuario_ProductOwner).Distinct().OrderBy(x => x).ToArray();

                    arr_data.TTL = (from u in ctx.Aplicacion
                                    where u.FlagActivo && u.TribeTechnicalLead != ""
                                    select u.TribeTechnicalLead).Distinct().OrderBy(x => x).ToArray();

                }
                return arr_data;
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: FiltrosDashboardAplicacion ListFiltros()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: FiltrosDashboardAplicacion ListFiltros()"
                    , new object[] { null });
            }
        }

        //public override AplicacionPortafolioResponsablesDTO ObtenerAplicacionPortafolioResponsableXMatricula(string matricula)
        //{
        //    try
        //    {
        //        using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
        //        {
        //            var entidad = (from u in ctx.AplicacionPortafolioResponsables
        //                           where //u.FlagActivo
        //                           (u.Matricula.ToUpper() == matricula.ToUpper())
        //                           select new AplicacionPortafolioResponsablesDTO()
        //                           {
        //                               Colaborador = u.Colaborador,
        //                               CorreoElectronico = u.CorreoElectronico,

        //                           }).FirstOrDefault();

        //            return entidad;
        //        }
        //    }
        //    catch (DbEntityValidationException ex)
        //    {
        //        HelperLog.ErrorEntity(ex);
        //        throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
        //            , "Error en el metodo: ObtenerAplicacionPortafolioResponsableXMatricula(string matricula)"
        //            , new object[] { null });
        //    }
        //    catch (Exception ex)
        //    {
        //        HelperLog.Error(ex.Message);
        //        throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
        //            , "Error en el metodo: ObtenerAplicacionPortafolioResponsableXMatricula(string matricula)"
        //            , new object[] { null });
        //    }
        //}


        public override List<AplicacionPortafolioResponsablesDTO> ObtenerAplicacionPortafolioResponsableXListaMatricula(List<string> listaMatriculas)
        {
            try
            {

                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.AplicacionPortafolioResponsables
                                   where //u.FlagActivo
                                   (listaMatriculas.Contains(u.Matricula))
                                   select new AplicacionPortafolioResponsablesDTO()
                                   {
                                       Colaborador = u.Colaborador,
                                       CorreoElectronico = u.CorreoElectronico,

                                   }).ToList();

                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: ObtenerAplicacionPortafolioResponsableXListaMatricula(List<string> listaMatriculas)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: ObtenerAplicacionPortafolioResponsableXListaMatricula(List<string> listaMatriculas)"
                    , new object[] { null });
            }
        }

        public override AplicacionPublicDto GetAplicacionByCodigo(string CodigoAPT)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.Aplicacion
                                   where u.CodigoAPT == CodigoAPT
                                   select new AplicacionPublicDto()
                                   {
                                       Area = u.Area,
                                       AreaBIAN = u.AreaBIAN,
                                       ClasificacionTecnica = u.ClasificacionTecnica,
                                       CodigoAPT = u.CodigoAPT,
                                       Descripcion = u.DescripcionAplicacion,
                                       Division = u.Division,
                                       DominioBIAN = u.DominioBIAN,
                                       EstadoAplicacion = u.EstadoAplicacion,
                                       GerenciaCentral = u.GerenciaCentral,
                                       JefaturaATI = u.JefaturaATI,
                                       Nombre = u.Nombre,
                                       TipoActivoInformacion = u.TipoActivoInformacion,
                                       Unidad = u.Unidad,
                                       SubclasificacionTecnica = u.SubclasificacionTecnica
                                   }).FirstOrDefault();
                    if (entidad != null)
                    {
                        entidad.ResponsablesPortafolio = this.GetAplicacionExpertoPortafolio(CodigoAPT);
                    }
                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: GetAplicacionByCodigo()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: GetAplicacionByCodigo()"
                    , new object[] { null });
            }
        }

        public override List<AplicacionDTO> GetAplicacionVistaConsultor(PaginacionAplicacion pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                var fechaConsulta = DateTime.Now;

                // >> Certificados Digitales: Parámetro de Tipo de Equipo (cvt.TipoEquipo)
                var TipoEquipo_CD = Int32.Parse(ServiceManager<ParametroDAO>.Provider.ObtenerParametro("CERTIFICADO_DIGITAL_APLICACION_PARAMETRO_TIPOEQUIPO").Valor);
                // <<

                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        ctx.Database.CommandTimeout = 0;

                        var aplicacionExpertos = (from x in ctx.AplicacionExpertos
                                                  where x.Matricula == pag.username && x.FlagActivo
                                                  select x.CodigoAPT).ToList(); ;

                        var totalServidoresRelacionados = (from a in ctx.Relacion
                                                               //join b in ctx.Tecnologia on a.TecnologiaId equals b.TecnologiaId
                                                               // >> Excluyendo Certificados Digitales
                                                           join e in ctx.Equipo on a.EquipoId equals e.EquipoId
                                                           join te in ctx.TipoEquipo on e.TipoEquipoId equals te.TipoEquipoId
                                                           // <<
                                                           where a.AnioRegistro == fechaConsulta.Year
                                                           && a.MesRegistro == fechaConsulta.Month
                                                           && a.DiaRegistro == fechaConsulta.Day
                                                           && a.FlagActivo && a.TipoId == (int)ETipoRelacion.Equipo
                                                           && (a.EstadoId == (int)EEstadoRelacion.Aprobado || a.EstadoId == (int)EEstadoRelacion.PendienteEliminacion)
                                                           // >> Excluyendo Certificados Digitales
                                                           && te.TipoEquipoId != TipoEquipo_CD
                                                           // <<
                                                           group a by a.CodigoAPT into grp
                                                           select new
                                                           {
                                                               CodigoAPT = grp.Key,
                                                               NroTecnologias = grp.Count()
                                                           }).Distinct();

                        var registros = (from u in ctx.Aplicacion
                                         join u2 in ctx.Criticidad on u.CriticidadId equals u2.CriticidadId
                                         join e in ctx.AplicacionExpertos on u.CodigoAPT equals e.CodigoAPT
                                         join r in ctx.RoadMap on u.RoadMapId equals r.RoadMapId into lj1
                                         from r in lj1.DefaultIfEmpty()
                                         join s in totalServidoresRelacionados on new { u.CodigoAPT } equals new { s.CodigoAPT } into lj2
                                         from s in lj2.DefaultIfEmpty()
                                         where (u.CodigoAPT.ToUpper().Contains(pag.Aplicacion.ToUpper())
                                         //|| u.Nombre.ToUpper().Contains(pag.Aplicacion.ToUpper())
                                         || (u.CodigoAPT + " - " + u.Nombre).ToUpper().Contains(pag.Aplicacion.ToUpper())
                                         || string.IsNullOrEmpty(pag.Aplicacion))
                                         //&& (u.EstadoAplicacion.ToUpper().Contains(pag.Estado.ToUpper()) || pag.Estado == null)
                                         //&& (u.GerenciaCentral.ToUpper().Contains(pag.Gerencia.ToUpper()) || pag.Gerencia == null)
                                         //&& (u.Division.ToUpper().Contains(pag.Division.ToUpper()) || pag.Division == null)
                                         //&& (u.Unidad.ToUpper().Contains(pag.Unidad.ToUpper()) || pag.Unidad == null)
                                         //&& (u.Area.ToUpper().Contains(pag.Area.ToUpper()) || pag.Area == null)

                                         && (pag.Estados.Count == 0 || pag.Estados.Contains(u.EstadoAplicacion.ToUpper()))
                                         && (pag.Gerencias.Count == 0 || pag.Gerencias.Contains(u.GerenciaCentral.ToUpper()))
                                         && (pag.Divisiones.Count == 0 || pag.Divisiones.Contains(u.Division.ToUpper()))
                                         && (pag.Unidades.Count == 0 || pag.Unidades.Contains(u.Unidad.ToUpper()))
                                         && (pag.Areas.Count == 0 || pag.Areas.Contains(u.Area.ToUpper()))

                                         && (u.JefeEquipo_ExpertoAplicacionUserIT_ProductOwner.ToUpper().Contains(pag.JefeEquipo.ToUpper()) || string.IsNullOrEmpty(pag.JefeEquipo))
                                         && (u.Owner_LiderUsuario_ProductOwner.ToUpper().Contains(pag.Owner.ToUpper()) || string.IsNullOrEmpty(pag.Owner))
                                         && u.FlagActivo && e.FlagActivo && e.Matricula == pag.Matricula
                                         //&& (pag.PerfilId == (int)EPerfilBCP.Administrador || aplicacionExpertos.Contains(u.CodigoAPT))
                                         orderby u.Nombre
                                         select new AplicacionDTO()
                                         {
                                             Id = u.AplicacionId,
                                             CodigoAPT = u.CodigoAPT,
                                             CodigoAPTStr = u.CodigoAPT,
                                             Nombre = u.Nombre,
                                             GestionadoPor = u.GestionadoPor,
                                             CriticidadId = u.CriticidadId,
                                             RoadMapId = u.RoadMapId,
                                             RoadMapToString = r.Nombre,
                                             Matricula = u.Matricula,
                                             Obsolescente = u.Obsolescente,
                                             //MesAnio = u.MesAnio,
                                             Activo = u.FlagActivo,
                                             UsuarioCreacion = u.CreadoPor,
                                             FechaCreacion = u.FechaCreacion,
                                             FechaRegistroProcedencia = u.FechaRegistroProcedencia,
                                             FechaModificacion = u.FechaModificacion,
                                             UsuarioModificacion = u.ModificadoPor,
                                             FlagRelacionar = u.FlagRelacionar,
                                             CriticidadToString = u2.DetalleCriticidad,
                                             TipoActivoInformacion = u.TipoActivoInformacion,
                                             GerenciaCentral = u.GerenciaCentral,
                                             Division = u.Division,
                                             Unidad = u.Unidad,
                                             Area = u.Area,
                                             NombreEquipo_Squad = u.NombreEquipo_Squad,
                                             Owner_LiderUsuario_ProductOwner = u.Owner_LiderUsuario_ProductOwner,
                                             JefeEquipo_ExpertoAplicacionUserIT_ProductOwner = u.JefeEquipo_ExpertoAplicacionUserIT_ProductOwner,
                                             Experto_Especialista = u.Experto_Especialista,
                                             TotalEquiposRelacionados = s == null ? 0 : s.NroTecnologias,
                                             EstadoAplicacion = u.EstadoAplicacion
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
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<AplicacionDTO> GetAplicacion(string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<AplicacionDTO> GetAplicacion(string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<ConfiguracionColumnaAplicacionDTO> GetConfiguracionColumnaAplicacion()
        {
            try
            {

                var fechaConsulta = DateTime.Now;
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        ctx.Database.CommandTimeout = 0;



                        var resultado = (from a in ctx.ConfiguracionColumnaAplicacion
                                         where a.TablaProcedencia == 4
                                         && a.FlagActivo
                                         orderby a.OrdenColumna
                                         select new ConfiguracionColumnaAplicacionDTO
                                         {
                                             NombreExcel = a.NombreExcel,
                                             ActivoAplica = a.ActivoAplica,
                                             ModoLlenado = a.ModoLlenado,
                                             RolRegistra = a.RolRegistra,
                                             RolAprueba = a.RolAprueba,
                                             DescripcionCampo = a.DescripcionCampo,
                                             NivelConfiabilidad = a.NivelConfiabilidad,
                                             RolResponsableActualizacion = a.RolResponsableActualizacion,
                                             TipoRegistro = a.TipoRegistro,
                                         }).ToList();


                        return resultado;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<AplicacionDTO> GetAplicacion(string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<AplicacionDTO> GetAplicacion(string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }


        public override List<GrupoTicketRemedyDto> GetGrupo(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                totalRows = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.GrupoRemedy
                                     where (u.Nombre.ToUpper().Contains(filtro.ToUpper())
                                     || string.IsNullOrEmpty(filtro))
                                     && u.FlagEliminado == false
                                     orderby u.Nombre ascending
                                     select new GrupoTicketRemedyDto()
                                     {
                                         Id = u.GrupoRemedyId,
                                         Descripcion = u.Descripcion,
                                         Nombre = u.Nombre,
                                         Activo = u.FlagActivo,
                                         UsuarioCreacion = u.UsuarioCreacion,
                                         FechaCreacion = u.FechaCreacion,
                                         UsuarioModificacion = u.UsuarioModificacion,
                                         FechaModificacion = u.FechaModificacion,

                                     });

                    totalRows = registros.Count();
                    registros = registros.OrderBy(sortName + " " + sortOrder);
                    var resultado = registros.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                    return resultado;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: List<ActivosDTO> GetActivos(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: List<ActivosDTO> GetActivos(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<CodigoReservadoDTO> GetCodigoReservado(string codigo, int tipoCodigo, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                totalRows = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.CodigoReservado
                                     where (u.Codigo.ToUpper().Contains(codigo.ToUpper())
                                     || string.IsNullOrEmpty(codigo))
                                     && u.FlagEliminado == false
                                     && (tipoCodigo == -1 || u.TipoCodigo == tipoCodigo)
                                     orderby u.Codigo ascending
                                     select new CodigoReservadoDTO()
                                     {
                                         Id = u.CodigoReservadoId,
                                         TipoCodigo = u.TipoCodigo,
                                         Codigo = u.Codigo,
                                         FlagActivo = u.FlagActivo,
                                         Comentarios = u.Comentarios,


                                     });

                    totalRows = registros.Count();
                    registros = registros.OrderBy(sortName + " " + sortOrder);
                    var resultado = registros.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                    return resultado;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: List<CodigoReservadoDTO> GetCodigoReservado(string codigo, int tipoCodigo, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: List<CodigoReservadoDTO> GetCodigoReservado(string codigo, int tipoCodigo, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<CodigoReservadoDTO> GetCodigoReutilizado(string codigo, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                List<CodigoReservadoDTO> registros = new List<CodigoReservadoDTO>();
                totalRows = 0;
                var cadenaConexion = Constantes.CadenaConexion;
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();

                    using (var comando = new SqlCommand("[app].[USP_ListarCodigoReutilizado]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@Codigo", codigo));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pageNumber));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pageSize));
                        comando.Parameters.Add(new SqlParameter("@sortName", sortName));
                        comando.Parameters.Add(new SqlParameter("@sortOrder", sortOrder));
                        var dr = comando.ExecuteReader();
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var item = new CodigoReservadoDTO();
                                item.Id = dr.GetData<int>("CodigoReutilizadoId");
                                item.Codigo = dr.GetData<string>("Codigo");
                                item.Comentarios = dr.GetData<string>("Comentarios");
                                item.FlagActivo = dr.GetData<bool>("FlagActivo");
                                item.FlagEliminado = dr.GetData<bool>("FlagEliminado");
                                item.TipoCodigo = -1;
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
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<CodigoReservadoDTO> GetCodigoReutilizado(string codigo, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<CodigoReservadoDTO> GetCodigoReutilizado(string codigo, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override GrupoTicketRemedyDto GetGrupoById(int id)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.GrupoRemedy

                                   where u.GrupoRemedyId == id
                                   select new GrupoTicketRemedyDto()
                                   {
                                       Id = u.GrupoRemedyId,
                                       Descripcion = u.Descripcion,
                                       Nombre = u.Nombre,

                                       Activo = u.FlagActivo,
                                       //FechaCreacion = u.FechaCreacion.HasValue? u.FechaCreacion.Value : DateTime.Now,
                                       UsuarioCreacion = u.UsuarioCreacion,

                                   }).FirstOrDefault();
                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: ActivosDTO GetActivosById(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: ParametricasDTO GetActivosById(int id)"
                    , new object[] { null });
            }
        }

        public override CodigoReservadoDTO GetCodigoReservadoById(int id)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.CodigoReservado

                                   where u.CodigoReservadoId == id
                                   select new CodigoReservadoDTO()
                                   {
                                       Id = u.CodigoReservadoId,
                                       Comentarios = u.Comentarios,
                                       TipoCodigo = u.TipoCodigo,
                                       FlagActivo = u.FlagActivo,
                                       Codigo = u.Codigo,

                                   }).FirstOrDefault();
                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: CodigoReservadoDTO GetCodigoReservadoById(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: CodigoReservadoDTO GetCodigoReservadoById(int id)"
                    , new object[] { null });
            }
        }
        public override CodigoReservadoDTO GetCodigoReutilizadoById(int id)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var item = new CodigoReservadoDTO();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();

                    using (var comando = new SqlCommand("[app].[USP_CodigoReutilizadoById]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@codigoReutilizado", id));
                        var dr = comando.ExecuteReader();
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                item.Id = dr.GetData<int>("CodigoReutilizadoId");
                                item.Codigo = dr.GetData<string>("Codigo");
                                item.Comentarios = dr.GetData<string>("Comentarios");
                                item.FlagActivo = dr.GetData<bool>("FlagActivo");
                                item.FlagEliminado = dr.GetData<bool>("FlagEliminado");
                                item.TipoCodigo = -1; // corregir modelo el campo TipoCodigoStr
                            }
                        }
                    }
                    cnx.Close();
                    return item;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: CodigoReservadoDTO GetCodigoReutilizadoById(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: CodigoReservadoDTO GetCodigoReutilizadoById(int id)"
                    , new object[] { null });
            }
        }

        public override bool GetCodigoReutilizadoExists(string codigo)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                bool item = false;
                var parametroFechaCheckList = ServiceManager<ParametroDAO>.Provider.ObtenerParametroApp("FECHA_CORTE_CHECKLIST_ELIMINAR_APP");
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();

                    using (var comando = new SqlCommand("[app].[USP_VerificarExisteCodigoReutilizado]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@codigoReservado", codigo));
                        comando.Parameters.Add(new SqlParameter("@fechaCorte", parametroFechaCheckList.Valor));
                        var dr = comando.ExecuteReader();
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                item = dr.GetData<bool>("codExiste");
                            }
                        }
                    }
                    cnx.Close();
                    return item;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: CodigoReservadoDTO GetCodigoReutilizadoExists(string codigo)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: CodigoReservadoDTO GetCodigoReutilizadoExists(string codigo)"
                    , new object[] { null });
            }
        }
        
        public override bool CambiarEstadoGrupo(int id, bool estado, string usuario)
        {
            try
            {
                bool retorno = false;
                DateTime FECHA_ACTUAL = DateTime.Now;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var itemBD = (from u in ctx.GrupoRemedy
                                  where u.GrupoRemedyId == id
                                  select u).FirstOrDefault();

                    if (itemBD != null)
                    {
                        itemBD.FechaModificacion = FECHA_ACTUAL;
                        itemBD.UsuarioModificacion = usuario;
                        itemBD.FlagActivo = estado;
                        ctx.SaveChanges();

                        retorno = !retorno;
                    }

                    return retorno;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
        }

        public override bool CambiarEstadoCodigo(int id, bool? estado, string usuario)
        {
            try
            {
                bool retorno = false;
                DateTime FECHA_ACTUAL = DateTime.Now;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var itemBD = (from u in ctx.CodigoReservado
                                  where u.CodigoReservadoId == id
                                  select u).FirstOrDefault();

                    if (itemBD != null)
                    {

                        itemBD.FlagActivo = estado;
                        ctx.SaveChanges();

                        retorno = !retorno;
                    }

                    return retorno;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: bool CambiarEstadoCodigo(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: bool CambiarEstadoCodigo(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
        }
        public override bool GetCambiarEstadoCodigoReutilizado(int id, bool? estado, bool? eliminado,string codigo,string comentarios, string usuario)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                int item = 0;
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();

                    using (var comando = new SqlCommand("[app].[USP_InsertUpdateCodigoReutilizado]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@CodigoReutilizadoId", id));
                        comando.Parameters.Add(new SqlParameter("@Codigo", codigo));
                        comando.Parameters.Add(new SqlParameter("@Comentarios", comentarios));
                        comando.Parameters.Add(new SqlParameter("@FlagActivo", estado));
                        comando.Parameters.Add(new SqlParameter("@FlagEliminado", eliminado));
                        var dr = comando.ExecuteReader();
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                item = dr.GetData<int>("CodigoReutilizadoId");
                            }
                        }
                    }
                    cnx.Close();
                    return item != 0 ? true : false;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: bool GetCambiarEstadoCodigoReutilizado(int id, bool? estado, bool? eliminado,string codigo,string comentarios, string usuario)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: bool GetCambiarEstadoCodigoReutilizado(int id, bool? estado, bool? eliminado,string codigo,string comentarios, string usuario)"
                    , new object[] { null });
            }
        }
        public override bool GetEliminarCodigoReutilizado(int id, bool? estado, bool? eliminado, string codigo, string comentarios, string usuario)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                int item = 0;
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();

                    using (var comando = new SqlCommand("[app].[USP_InsertUpdateCodigoReutilizado]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@CodigoReutilizadoId", id));
                        comando.Parameters.Add(new SqlParameter("@Codigo", codigo));
                        comando.Parameters.Add(new SqlParameter("@Comentarios", comentarios));
                        comando.Parameters.Add(new SqlParameter("@FlagActivo", estado));
                        comando.Parameters.Add(new SqlParameter("@FlagEliminado", eliminado));
                        var dr = comando.ExecuteReader();
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                item = dr.GetData<int>("CodigoReutilizadoId");
                            }
                        }
                    }
                    cnx.Close();
                    return item != 0 ? true : false;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: bool GetEliminarCodigoReutilizado(int id, bool? estado, bool? eliminado, string codigo, string comentarios, string usuario)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: bool GetEliminarCodigoReutilizado(int id, bool? estado, bool? eliminado, string codigo, string comentarios, string usuario)"
                    , new object[] { null });
            }
        }
        
        public override int AddOrEditGrupo(GrupoTicketRemedyDto objeto)
        {
            try
            {
                DateTime FECHA_ACTUAL = DateTime.Now;
                int ID = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    if (objeto.Id == -1)
                    {
                        var entidad = new GrupoRemedy()
                        {

                            Descripcion = objeto.Descripcion,
                            Nombre = objeto.Nombre,
                            UsuarioCreacion = objeto.UsuarioCreacion,
                            UsuarioModificacion = objeto.UsuarioModificacion,
                            FechaCreacion = FECHA_ACTUAL,
                            FechaModificacion = FECHA_ACTUAL,
                            FlagEliminado = false,

                        };
                        ctx.GrupoRemedy.Add(entidad);


                        ctx.SaveChanges();

                        ID = entidad.GrupoRemedyId;
                    }
                    else
                    {
                        var entidad = (from u in ctx.GrupoRemedy
                                       where u.GrupoRemedyId == objeto.Id
                                       select u).FirstOrDefault();
                        if (entidad != null)
                        {



                            entidad.Descripcion = objeto.Descripcion;
                            entidad.Nombre = objeto.Nombre;
                            entidad.FechaModificacion = FECHA_ACTUAL;
                            entidad.UsuarioModificacion = objeto.UsuarioModificacion;

                            ctx.SaveChanges();

                            ID = entidad.GrupoRemedyId;
                        }
                    }

                    return ID;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: int AddOrEditActivos(ActivosDTO objeto)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: int AddOrEditActivos(ActivosDTO objeto)"
                    , new object[] { null });
            }
        }

        public override int AddOrEditCodigo(CodigoReservadoDTO objeto)
        {
            try
            {
                DateTime FECHA_ACTUAL = DateTime.Now;
                int ID = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    if (objeto.Id == -1)
                    {
                        var entidad = new CodigoReservado()
                        {

                            Comentarios = objeto.Comentarios,
                            TipoCodigo = objeto.TipoCodigo,
                            Codigo = objeto.Codigo,
                            FlagActivo = true,
                            FlagEliminado = false,

                        };
                        ctx.CodigoReservado.Add(entidad);


                        ctx.SaveChanges();

                        ID = entidad.CodigoReservadoId;
                    }
                    else
                    {
                        var entidad = (from u in ctx.CodigoReservado
                                       where u.CodigoReservadoId == objeto.Id
                                       select u).FirstOrDefault();
                        if (entidad != null)
                        {



                            entidad.Codigo = objeto.Codigo;
                            entidad.Comentarios = objeto.Comentarios;
                            entidad.TipoCodigo = objeto.TipoCodigo;


                            ctx.SaveChanges();

                            ID = entidad.CodigoReservadoId;
                        }
                    }

                    return ID;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: int AddOrEditActivos(ActivosDTO objeto)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: int AddOrEditActivos(ActivosDTO objeto)"
                    , new object[] { null });
            }
        }
        public override int AddOrEditCodigoReutilizado(CodigoReservadoDTO objeto)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                int item = 0;
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();

                    using (var comando = new SqlCommand("[app].[USP_InsertUpdateCodigoReutilizado]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@CodigoReutilizadoId", objeto.Id));
                        comando.Parameters.Add(new SqlParameter("@Codigo", objeto.Codigo));
                        comando.Parameters.Add(new SqlParameter("@Comentarios", objeto.Comentarios));
                        comando.Parameters.Add(new SqlParameter("@FlagActivo", objeto.FlagActivo));
                        comando.Parameters.Add(new SqlParameter("@FlagEliminado", objeto.FlagEliminado));
                        var dr = comando.ExecuteReader();
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                item = dr.GetData<int>("CodigoReutilizadoId");
                            }
                        }
                    }
                    cnx.Close();
                    return item;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: CodigoReservadoDTO GetCodigoReservadoExists(string codigo)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: CodigoReservadoDTO GetCodigoReservadoExists(string codigo)"
                    , new object[] { null });
            }
        }
        
        public override List<CustomAutocomplete> GetAplicacionAprobadaByFiltro(string filtro, bool? flagAprobado)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.Application
                                       //join y in ctx.TipoActivoInformacion on u.TipoActivoInformacion.ToUpper() equals y.Nombre.ToUpper()
                                   where u.isActive == true
                                   && (u.applicationId + " - " + u.applicationName).ToUpper().Contains(filtro.ToUpper())
                                   //&& (flagAprobado == null || u.FlagAprobado.Value == flagAprobado.Value)
                                   orderby u.applicationId
                                   select new CustomAutocomplete()
                                   {
                                       Id = u.applicationId,
                                       IdAplicacion = u.AppId,
                                       Descripcion = u.applicationId + " - " + u.applicationName,
                                       value = u.applicationId + " - " + u.applicationName
                                       //TipoFlujoId = y.FlujoRegistro.Value.ToString()
                                   }).ToList();

                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetAplicacionByFiltro(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetAplicacionByFiltro(string filtro)"
                    , new object[] { null });
            }
        }

        public override bool ExisteAplicacionByCodigoNombre(string filtro, int idAplicacion)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    bool? estado = (from u in ctx.Aplicacion
                                    where u.FlagActivo && u.FlagAprobado.Value
                                    && (u.CodigoAPT + " - " + u.Nombre).ToUpper().Equals(filtro.ToUpper())
                                    && u.AplicacionId != idAplicacion
                                    orderby u.Nombre
                                    select true).FirstOrDefault();

                    return estado == true;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: bool ExisteAplicacion(string nombre, string Id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: bool ExisteAplicacion(string nombre, string Id)"
                    , new object[] { null });
            }
        }

        public override List<AplicacionDTO> GetAplicacionPortafolioUpdate(PaginacionAplicacion pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                var fechaConsulta = DateTime.Now;

                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        ctx.Database.CommandTimeout = 0;

                        var registros = (from u in ctx.Aplicacion
                                             //join s in ctx.Solicitud on u.AplicacionId equals s.AplicacionId into lj0
                                             //from s in lj0.DefaultIfEmpty()
                                         join ad in ctx.AplicacionDetalle on u.AplicacionId equals ad.AplicacionId into lj1
                                         from ad in lj1.DefaultIfEmpty()
                                         join u2 in ctx.Criticidad on u.CriticidadId equals u2.CriticidadId
                                         join r in ctx.RoadMap on u.RoadMapId equals r.RoadMapId into lj2
                                         from r in lj2.DefaultIfEmpty()
                                             //where u.FlagActivo && u.FlagAprobado.Value && u.EstadoAplicacion != "Eliminada"
                                         orderby u.CodigoAPT ascending
                                         select new AplicacionDTO()
                                         {
                                             Id = u.AplicacionId,
                                             CodigoAPT = u.CodigoAPT,
                                             Nombre = u.Nombre,
                                             DescripcionAplicacion = u.DescripcionAplicacion,
                                             EstadoAplicacion = u.EstadoAplicacion,
                                             GestionadoPor = u.GestionadoPor,
                                             CriticidadId = u.CriticidadId,
                                             RoadMapId = u.RoadMapId,
                                             RoadMapToString = r.Nombre,
                                             Matricula = u.Matricula,
                                             Obsolescente = u.Obsolescente,
                                             CategoriaTecnologica = u.CategoriaTecnologica,
                                             Activo = u.FlagActivo,
                                             UsuarioCreacion = u.CreadoPor,
                                             FechaCreacion = u.FechaCreacion,
                                             FechaRegistroProcedencia = u.FechaRegistroProcedencia,
                                             FechaModificacion = u.FechaModificacion,
                                             UsuarioModificacion = u.ModificadoPor,
                                             FlagRelacionar = u.FlagRelacionar,
                                             CriticidadToString = u2.DetalleCriticidad,
                                             TipoActivoInformacion = u.TipoActivoInformacion,
                                             GerenciaCentral = u.GerenciaCentral,
                                             Division = u.Division,
                                             Unidad = u.Unidad,
                                             Area = u.Area,
                                             AreaBIAN = u.AreaBIAN,
                                             DominioBIAN = u.DominioBIAN,
                                             JefaturaATI = u.JefaturaATI,
                                             NombreEquipo_Squad = u.NombreEquipo_Squad,
                                             TribeTechnicalLead = string.IsNullOrEmpty(u.TribeTechnicalLead) ? Utilitarios.NO_APLICA : u.TribeTechnicalLead,
                                             BrokerSistemas = string.IsNullOrEmpty(u.BrokerSistemas) ? Utilitarios.NO_APLICA : u.BrokerSistemas,
                                             Owner_LiderUsuario_ProductOwner = string.IsNullOrEmpty(u.Owner_LiderUsuario_ProductOwner) ? Utilitarios.NO_APLICA : u.Owner_LiderUsuario_ProductOwner,
                                             JefeEquipo_ExpertoAplicacionUserIT_ProductOwner = string.IsNullOrEmpty(u.JefeEquipo_ExpertoAplicacionUserIT_ProductOwner) ? Utilitarios.NO_APLICA : u.JefeEquipo_ExpertoAplicacionUserIT_ProductOwner,
                                             Gestor_UsuarioAutorizador_ProductOwner = string.IsNullOrEmpty(u.Gestor_UsuarioAutorizador_ProductOwner) ? Utilitarios.NO_APLICA : u.Gestor_UsuarioAutorizador_ProductOwner,
                                             Experto_Especialista = string.IsNullOrEmpty(u.Experto_Especialista) ? Utilitarios.NO_APLICA : u.Experto_Especialista,
                                             EntidadResponsable = u.EntidadResponsable,
                                             ClasificacionTecnica = u.ClasificacionTecnica,
                                             SubclasificacionTecnica = u.SubclasificacionTecnica,
                                             ArquitectoTI = u.ArquitectoTI,
                                             GestorUserIT = u.GestorUserIT,
                                             FechaCreacionAplicacion = u.FechaCreacionAplicacion,
                                             AplicacionDetalle = new AplicacionDetalleDTO()
                                             {
                                                 Id = ad != null ? ad.AplicacionDetalleId : 0,
                                                 EstadoSolicitudId = ad != null ? (ad.EstadoSolicitudId.HasValue ? ad.EstadoSolicitudId.Value : (int)EEstadoSolicitudAplicacion.Aprobado) : (int)EEstadoSolicitudAplicacion.Aprobado,
                                                 MotivoCreacion = ad != null ? ad.MotivoCreacion : "-1",
                                                 AmbienteInstalacion = ad != null ? ad.AmbienteInstalacion : "",
                                                 AplicacionReemplazo = ad != null ? ad.AplicacionReemplazo : "",
                                                 Contingencia = ad != null ? ad.Contingencia : "-1",
                                                 EntidadUso = ad != null ? ad.EntidadUso : "",
                                                 FechaSolicitud = ad != null ? ad.FechaSolicitud : u.FechaCreacion,
                                                 FlagOOR = ad != null ? ad.FlagOOR : null,
                                                 FlagRatificaOOR = ad.FlagRatificaOOR,
                                                 GrupoServiceDesk = ad != null ? ad.GrupoServiceDesk : "",
                                                 Infraestructura = ad != null ? ad.Infraestructura : "-1",
                                                 MetodoAutenticacion = ad != null ? ad.MetodoAutenticacion : "-1",
                                                 MetodoAutorizacion = ad != null ? ad.MetodoAutorizacion : "-1",
                                                 ModeloEntrega = ad != null ? ad.ModeloEntrega : "-1",
                                                 PersonaSolicitud = ad != null ? ad.PersonaSolicitud : "",
                                                 PlataformaBCP = ad != null ? ad.PlataformaBCP : "-1",
                                                 Proveedor = ad != null ? ad.Proveedor : "",
                                                 RutaRepositorio = ad != null ? ad.RutaRepositorio : "",
                                                 TipoDesarrollo = ad != null ? ad.TipoDesarrollo : "-1",
                                                 Ubicacion = ad != null ? ad.Ubicacion : "-1",
                                                 CodigoInterfaz = ad != null ? ad.CodigoInterfaz : "",
                                                 InterfazApp = ad != null ? ad.InterfazApp : "",
                                                 NombreServidor = ad != null ? ad.NombreServidor : "",
                                                 CompatibilidadWindows = ad != null ? ad.CompatibilidadWindows : "-1",
                                                 CompatibilidadNavegador = ad != null ? ad.CompatibilidadNavegador : "-1",
                                                 CompatibilidadHV = ad != null ? ad.CompatibilidadHV : "-1",
                                                 InstaladaDesarrollo = ad != null ? ad.InstaladaDesarrollo : "-1",
                                                 InstaladaCertificacion = ad != null ? ad.InstaladaCertificacion : "-1",
                                                 InstaladaProduccion = ad != null ? ad.InstaladaProduccion : "-1",
                                                 GrupoTicketRemedy = ad != null ? ad.GrupoTicketRemedy : "-1",
                                                 NCET = ad != null ? ad.NCET : "",
                                                 NCLS = ad != null ? ad.NCLS : "",
                                                 NCG = ad != null ? ad.NCG : "",
                                                 ResumenSeguridadInformacion = ad != null ? ad.ResumenSeguridadInformacion : "",
                                                 ProcesoClave = ad != null ? ad.ProcesoClave : "",
                                                 Confidencialidad = ad != null ? ad.Confidencialidad : "-1",
                                                 Integridad = ad != null ? ad.Integridad : "-1",
                                                 Disponibilidad = ad != null ? ad.Disponibilidad : "-1",
                                                 Privacidad = ad != null ? ad.Privacidad : "-1",
                                                 Clasificacion = ad != null ? ad.Clasificacion : "-1",
                                                 RoadmapPlanificado = ad != null ? ad.RoadmapPlanificado : "",
                                                 DetalleEstrategia = ad != null ? ad.DetalleEstrategia : "",
                                                 EstadoRoadmap = ad != null ? ad.EstadoRoadmap : "-1",
                                                 EtapaAtencion = ad != null ? ad.EtapaAtencion : "",
                                                 RoadmapEjecutado = ad != null ? ad.RoadmapEjecutado : "",
                                                 FechaInicioRoadmap = ad != null ? ad.FechaInicioRoadmap : "",
                                                 FechaFinRoadmap = ad != null ? ad.FechaFinRoadmap : "",
                                                 CodigoAppReemplazo = ad != null ? ad.CodigoAppReemplazo : "",
                                                 SWBase_SO = ad != null ? ad.SWBase_SO : "",
                                                 SWBase_HP = ad != null ? ad.SWBase_HP : "",
                                                 SWBase_BD = ad != null ? ad.SWBase_BD : "",
                                                 SWBase_LP = ad != null ? ad.SWBase_LP : "",
                                                 SWBase_Framework = ad != null ? ad.SWBase_Framework : "",
                                                 RET = ad != null ? ad.RET : "",
                                                 CriticidadAplicacionBIA = ad != null ? ad.CriticidadAplicacionBIA : "",
                                                 ProductoMasRepresentativo = ad != null ? ad.ProductoMasRepresentativo : "",
                                                 MenorRTO = ad != null ? ad.MenorRTO : "",
                                                 MayorGradoInterrupcion = ad != null ? ad.MayorGradoInterrupcion : "",
                                                 FlagFileCheckList = ad != null ? ad.FlagFileCheckList : false,
                                                 FlagFileMatriz = ad != null ? ad.FlagFileMatriz : false,
                                                 GestorAplicacionCTR = ad != null ? ad.GestorAplicacionCTR : "",
                                                 ConsultorCTR = ad != null ? ad.ConsultorCTR : "",
                                                 ValorL_NC = ad != null ? ad.ValorL_NC : "|",
                                                 ValorM_NC = ad != null ? ad.ValorM_NC : "|",
                                                 ValorN_NC = ad != null ? ad.ValorN_NC : "",
                                                 ValorPC_NC = ad != null ? ad.ValorPC_NC : ""
                                             }
                                         })
                                         .OrderBy(pag.sortName + " " + pag.sortOrder);



                        totalRows = registros.Count();
                        var resultado = registros.Skip((pag.pageNumber - 1) * pag.pageSize).Take(pag.pageSize).ToList();

                        return resultado;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<AplicacionDTO> GetAplicacionPortafolioUpdate(PaginacionAplicacion pag, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<AplicacionDTO> GetAplicacionPortafolioUpdate(PaginacionAplicacion pag, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override DataResults GetResultsCargaMasivaPortafolio()
        {
            try
            {
                var dataResults = new DataResults();
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var errores = (from x in ctx.ErrorCargaMasiva
                                   where x.FlagActivo
                                   orderby x.UsuarioCreacion
                                   select new ErrorCargaMasivaDTO()
                                   {
                                       TipoErrorId = x.TipoErrorId,
                                       Detalle = x.Detalle,
                                       FilaExcel = x.FilaExcel
                                   }).ToList();

                    if (errores != null)
                        dataResults.Errores = errores;

                    var registros = (from y in ctx.ApplicationCargaMasiva select y);

                    if (registros != null)
                    {
                        var totalRegistros = registros.Count();
                        var totalActualizados = registros.Where(x => x.FlagRegistroValido.Value && x.FlagRegistroDistinto.Value).Count();
                        dataResults.TotalRegistros = totalRegistros;
                        dataResults.TotalActualizados = totalActualizados;
                    }

                    return dataResults;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<ErrorCargaMasivaDTO> GetErrorCargaMasivaPortafolio()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<ErrorCargaMasivaDTO> GetErrorCargaMasivaPortafolio()"
                    , new object[] { null });
            }
        }

        public override DataResults GetResultsCargaMasivaPortafolio2()
        {
            try
            {
                var dataResults = new DataResults();
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var errores = (from x in ctx.ErrorCargaMasiva
                                   where x.FlagActivo
                                   orderby x.UsuarioCreacion
                                   select new ErrorCargaMasivaDTO()
                                   {
                                       TipoErrorId = x.TipoErrorId,
                                       Detalle = x.Detalle,
                                       FilaExcel = x.FilaExcel
                                   }).ToList();

                    if (errores != null)
                        dataResults.Errores = errores;

                    var registros = (from y in ctx.ApplicationHistorico select y);

                    if (registros != null)
                    {
                        var totalRegistros = registros.Count();
                        var totalActualizados = registros.Where(x => x.FlagRegistroValido.Value).Count();
                        dataResults.TotalRegistros = totalRegistros;
                        dataResults.TotalActualizados = totalActualizados;
                    }

                    return dataResults;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<ErrorCargaMasivaDTO> GetErrorCargaMasivaPortafolio()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<ErrorCargaMasivaDTO> GetErrorCargaMasivaPortafolio()"
                    , new object[] { null });
            }
        }

        public override int ReordenarColumnaApp(List<ConfiguracionColumnaAplicacionOrdenDTO> listaOrdenada, string username)
        {
            try
            {
                var dataResults = new DataResults();
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var ids = listaOrdenada.Select(x => x.Id).ToList();
                    var listaActualizar = (from u in ctx.ConfiguracionColumnaAplicacion
                                           where !u.FlagEliminado.Value
                                           && ids.Contains(u.ConfiguracionColumnaAplicacionId)
                                           select u).ToList();

                    listaActualizar = listaActualizar.Select(x =>
                            {
                                x.OrdenColumna = listaOrdenada.Where(y => y.Id == x.ConfiguracionColumnaAplicacionId).FirstOrDefault().OrdenColumna;
                                x.FechaModificacion = DateTime.Now;
                                x.UsuarioModificacion = username;
                                return x;
                            }).ToList();

                    ctx.SaveChanges();
                    return 1;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: ReordenarColumnaApp(List<ConfiguracionColumnaAplicacionDTO> registros)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: ReordenarColumnaApp(List<ConfiguracionColumnaAplicacionDTO> registros)"
                    , new object[] { null });
            }
        }


        public override void GetResultsCargaMasivaPortafolio(PortafolioBackupDTO objRegistro)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;

                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();

                    using (var comando = new SqlCommand("[app].[USP_INSERT_PORTAFOLIO_BACKUP]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@Comentario", objRegistro.Comentario));
                        comando.Parameters.Add(new SqlParameter("@BackupBytes", objRegistro.BackupBytes));
                        comando.Parameters.Add(new SqlParameter("@UsuarioCreacion", objRegistro.UsuarioCreacion));
                        comando.Parameters.Add(new SqlParameter("@FechaCreacion", objRegistro.FechaCreacion));
                        comando.ExecuteNonQuery();

                    }

                    cnx.Close();
                }

            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: GetResultsCargaMasivaPortafolio(string comentario, byte[] data)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: GetResultsCargaMasivaPortafolio(string comentario, byte[] data)"
                    , new object[] { null });
            }
        }

        public override List<PortafolioBackupDTO> GetAplicacionPortafolioBackups(PaginacionPortafolioBackup pag, out int totalRows)
        {
            try
            {
                totalRows = 0;

                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var registros = (from u in ctx.PortafolioBackup
                                         where (u.UsuarioCreacion.ToUpper().Contains(pag.Usuario.ToUpper()) || string.IsNullOrEmpty(pag.Usuario))
                                         && (u.FechaCreacion >= pag.FechaDesde || !pag.FechaDesde.HasValue)
                                         && (u.FechaCreacion <= pag.FechaHasta || !pag.FechaHasta.HasValue)
                                         orderby u.FechaCreacion ascending
                                         select new PortafolioBackupDTO()
                                         {
                                             PortafolioBackupId = u.PortafolioBackupId,
                                             UsuarioCreacion = u.UsuarioCreacion,
                                             FechaCreacion = u.FechaCreacion,
                                             Comentario = u.Comentario,
                                             //BackupBytes = u.BackupBytes

                                         });


                        totalRows = registros.Count();
                        var resultado = registros.Skip((pag.pageNumber - 1) * pag.pageSize).Take(pag.pageSize).ToList();

                        return resultado;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo:  GetAplicacionPortafolioBackups(PaginacionPortafolioBackup pag, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: GetAplicacionPortafolioBackups(PaginacionPortafolioBackup pag, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override PortafolioBackupDTO GetAplicacionPortafolioBackupById(int idBackup)
        {
            try
            {

                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var resultado = (from u in ctx.PortafolioBackup
                                     where u.PortafolioBackupId == idBackup
                                     select new PortafolioBackupDTO
                                     {
                                         PortafolioBackupId = u.PortafolioBackupId,
                                         UsuarioCreacion = u.UsuarioCreacion,
                                         FechaCreacion = u.FechaCreacion,
                                         Comentario = u.Comentario,
                                         BackupBytes = u.BackupBytes

                                     }).FirstOrDefault();

                    return resultado;
                }

            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: PortafolioBackupDTO GetAplicacionPortafolioBackupsById(int idBackup)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: PortafolioBackupDTO GetAplicacionPortafolioBackupsById(int idBackup)"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocomplete> GetAplicacionAprobadaCatalogoByFiltro(string filtro)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.Aplicacion
                                   where u.FlagAprobado.Value
                                   && (u.CodigoAPT + " - " + u.Nombre).ToUpper().Contains(filtro.ToUpper())
                                   orderby u.Nombre
                                   select new CustomAutocomplete()
                                   {
                                       Id = u.CodigoAPT,
                                       IdAplicacion = u.AplicacionId,
                                       Descripcion = u.CodigoAPT + " - " + u.Nombre,
                                       value = u.CodigoAPT + " - " + u.Nombre
                                   }).ToList();

                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetAplicacionByFiltro(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetAplicacionByFiltro(string filtro)"
                    , new object[] { null });
            }
        }

        public override List<ItemColumnaAppJS> GetColumnasPublicacionAplicacionToJSCatalogo(string tablaProcedencia)
        {
            try
            {
                var lstTablaProcedencia = new List<int>();
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        if (!string.IsNullOrEmpty(tablaProcedencia))
                            lstTablaProcedencia = tablaProcedencia.Split(';').Select(int.Parse).ToList();

                        var ldataPrincipal = new List<ItemColumnaAppJS>();
                        ldataPrincipal.Add(new ItemColumnaAppJS()
                        {
                            title = "#",
                            field = string.Empty,
                            formatter = "rowNumFormatterServer"
                        });
                        ldataPrincipal.Add(new ItemColumnaAppJS()
                        {
                            title = "Acciones",
                            field = "CodigoAPT",
                            formatter = "opcionesFormatter"
                        });

                        var ldata = (from u in ctx.ConfiguracionColumnaAplicacion
                                     where u.FlagActivo
                                     //&& u.FlagVerExportar.Value
                                     && (string.IsNullOrEmpty(tablaProcedencia) || lstTablaProcedencia.Contains(u.TablaProcedencia.Value))
                                     orderby u.OrdenColumna.Value ascending
                                     select new ItemColumnaAppJS()
                                     {
                                         title = u.NombreExcel,
                                         field = u.NombreBD,
                                         formatter = u.ConfiguracionColumnaAplicacionId == 9 ? "descripcionFormatter" : "generalFormatter"
                                     }).ToList();

                        foreach (var item in ldata)
                        {
                            item.field = string.IsNullOrWhiteSpace(item.field) ? item.title.Replace(" ", "_") : item.field;
                        }

                        ldataPrincipal.AddRange(ldata);

                        return ldataPrincipal;
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

        public override DataTable GetPublicacionAplicacionCatalogo(PaginacionReporteAplicacion pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                var data = new DataTable();

                var columnas = GetColumnasPublicacionAplicacionToBD(pag.TablaProcedencia);
                pag.Columnas = columnas;
                data = ReportePublicacionAplicacion2(pag);

                if (data != null && data.Rows.Count > 0) totalRows = Convert.ToInt16(data.Rows[0]["TotalFilas"]);

                return data;
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetTecnologias()"
                    , new object[] { null });
            }
        }

        public override bool ExisteNombre(string nombre, int id)
        {
            try
            {
                var listaProcedencia = new List<int>() { 1, 2, 4 };
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        bool? estado = (from u in ctx.ConfiguracionColumnaAplicacion
                                        where u.FlagActivo
                                        && u.NombreExcel.ToUpper().Equals(nombre.ToUpper())
                                        && u.ConfiguracionColumnaAplicacionId != id
                                        && listaProcedencia.Contains(u.TablaProcedencia.Value)
                                        select true).FirstOrDefault();

                        return estado == true;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: bool ExisteOrden(int ordenNuevo, int ordenActual)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: bool ExisteOrden(int ordenNuevo, int ordenActual)"
                    , new object[] { null });
            }
        }

        public override List<AplicacionDTO> GetAplicacionConsultorByFilter(PaginacionAplicacion pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                var fechaConsulta = DateTime.Now;
                var arrExpertoCodigoAPTs = new List<string>();
                //var arrMatriculas = new string[]
                //{
                //    pag.Matricula,
                //    pag.MatriculaExperto
                //};
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        ctx.Database.CommandTimeout = 0;

                        if (!string.IsNullOrEmpty(pag.MatriculaExperto))
                        {
                            arrExpertoCodigoAPTs = ctx.AplicacionExpertos
                                                    .Where(x => x.FlagActivo
                                                    && x.Matricula.ToUpper().Equals(pag.MatriculaExperto.ToUpper())
                                                    && x.TipoExpertoId == pag.TipoExpertoId)
                                                    .Select(x => x.CodigoAPT).ToList();
                        }

                        var registros = (from u in ctx.Aplicacion
                                         join u2 in ctx.Criticidad on u.CriticidadId equals u2.CriticidadId
                                         join e in ctx.AplicacionExpertos on u.CodigoAPT equals e.CodigoAPT
                                         join te in ctx.TipoExperto on e.TipoExpertoId equals te.TipoExpertoId
                                         where u.FlagActivo && e.FlagActivo
                                         && e.Matricula == pag.Matricula
                                         //&& (pag.TipoExpertoId == -1 || e.TipoExpertoId == pag.TipoExpertoId)
                                         //&& arrMatriculas.Contains(e.Matricula)
                                         orderby u.Nombre
                                         select new AplicacionDTO()
                                         {
                                             //Id = u.AplicacionId,
                                             Id = e.AplicacionExpertoId,
                                             CodigoAPT = u.CodigoAPT,
                                             CodigoAPTStr = u.CodigoAPT,
                                             Nombre = u.Nombre,
                                             GestionadoPor = u.GestionadoPor,
                                             CriticidadId = u.CriticidadId,
                                             Matricula = u.Matricula,
                                             Obsolescente = u.Obsolescente,
                                             Activo = u.FlagActivo,
                                             UsuarioCreacion = u.CreadoPor,
                                             FechaCreacion = u.FechaCreacion,
                                             FechaRegistroProcedencia = u.FechaRegistroProcedencia,
                                             FechaModificacion = u.FechaModificacion,
                                             UsuarioModificacion = u.ModificadoPor,
                                             FlagRelacionar = u.FlagRelacionar,
                                             CriticidadToString = u2.DetalleCriticidad,
                                             TipoActivoInformacion = u.TipoActivoInformacion,
                                             GerenciaCentral = u.GerenciaCentral,
                                             Division = u.Division,
                                             Unidad = u.Unidad,
                                             Area = u.Area,
                                             NombreEquipo_Squad = u.NombreEquipo_Squad,
                                             Owner_LiderUsuario_ProductOwner = u.Owner_LiderUsuario_ProductOwner,
                                             JefeEquipo_ExpertoAplicacionUserIT_ProductOwner = u.JefeEquipo_ExpertoAplicacionUserIT_ProductOwner,
                                             Experto_Especialista = u.Experto_Especialista,
                                             EstadoAplicacion = u.EstadoAplicacion,
                                             TipoExperto = e.TipoExpertoId,
                                             TipoExpertoToString = te.Nombres,
                                             //ItemSelected = e.Matricula == pag.MatriculaExperto
                                             ItemSelected = string.IsNullOrEmpty(pag.MatriculaExperto) ? false : arrExpertoCodigoAPTs.Contains(u.CodigoAPT)
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
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<AplicacionDTO> GetAplicacion(string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<AplicacionDTO> GetAplicacion(string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override bool AddOrEditAplicacionExpertoConsultor(ParametroConsultor objRegistro)
        {
            try
            {
                var rowsAffected = 0;
                var CURRENT_DATE = DateTime.Now;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    if (!string.IsNullOrEmpty(objRegistro.ArrIdsAplicacionExpertos))
                    {
                        var arrIdsAplicacionExpertos = objRegistro.ArrIdsAplicacionExpertos.Split('|');
                        ctx.AplicacionExpertos
                            .Where(x => arrIdsAplicacionExpertos.Contains(x.AplicacionExpertoId.ToString()) && x.FlagActivo)
                            .ToList().ForEach(y =>
                            {
                                y.FlagActivo = false;
                                y.FechaModificacion = CURRENT_DATE;
                                y.ModificadoPor = objRegistro.UsuarioModificacion;
                            });

                        ctx.SaveChanges();
                    }

                    if (!string.IsNullOrEmpty(objRegistro.ArrCodigoAPT))
                    {
                        var arrCodigoAPT = objRegistro.ArrCodigoAPT.Split('|');
                        foreach (var codigoAPT in arrCodigoAPT)
                        {
                            var existsItem = ctx.AplicacionExpertos.FirstOrDefault(x =>
                                                x.CodigoAPT.ToUpper().Equals(codigoAPT.ToUpper())
                                                && x.Matricula.ToUpper().Equals(objRegistro.MatriculaExperto.ToUpper())
                                                && x.TipoExpertoId == objRegistro.TipoExpertoId && x.FlagActivo);

                            if (existsItem != null)
                            {
                                existsItem.FechaModificacion = CURRENT_DATE;
                                existsItem.ModificadoPor = objRegistro.UsuarioModificacion;
                            }
                            else
                            {
                                var objRegistroBd = new AplicacionExpertos()
                                {
                                    CodigoAPT = codigoAPT,
                                    Matricula = objRegistro.MatriculaExperto,
                                    FlagActivo = true,
                                    FechaCreacion = CURRENT_DATE,
                                    CreadoPor = objRegistro.UsuarioModificacion,
                                    TipoExpertoId = objRegistro.TipoExpertoId,
                                    Nombres = objRegistro.Nombres
                                };
                                ctx.AplicacionExpertos.Add(objRegistroBd);
                            }
                            rowsAffected = ctx.SaveChanges();
                        }
                    }
                }
                return rowsAffected > 0;
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: bool AddOrEditAplicacionExperto(ParametroList objRegistro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: bool AddOrEditAplicacionExperto(ParametroList objRegistro)"
                    , new object[] { null });
            }
        }

        public override List<ItemColumnaAppJS> GetColumnasPublicacionAplicacionToJSAppsDesestimadas(string tablaProcedencia)
        {
            try
            {
                var lstTablaProcedencia = new List<int>();
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        if (!string.IsNullOrEmpty(tablaProcedencia))
                            lstTablaProcedencia = tablaProcedencia.Split(';').Select(int.Parse).ToList();

                        var ldataPrincipal = new List<ItemColumnaAppJS>();
                        ldataPrincipal.Add(new ItemColumnaAppJS()
                        {
                            title = "#",
                            field = string.Empty,
                            formatter = "rowNumFormatterServer"
                        });
                        ldataPrincipal.Add(new ItemColumnaAppJS()
                        {
                            title = "Acciones",
                            field = "CodigoAPT",
                            formatter = "opcionesFormatter"
                        });

                        var ldata = (from u in ctx.ConfiguracionColumnaAplicacion
                                     where u.FlagActivo
                                     //&& u.FlagVerExportar.Value
                                     && (string.IsNullOrEmpty(tablaProcedencia) || lstTablaProcedencia.Contains(u.TablaProcedencia.Value))
                                     orderby u.OrdenColumna.Value ascending
                                     select new ItemColumnaAppJS()
                                     {
                                         title = u.NombreExcel,
                                         field = u.NombreBD,
                                         formatter = u.ConfiguracionColumnaAplicacionId == 9 ? "descripcionFormatter" : "generalFormatter"
                                     }).ToList();

                        foreach (var item in ldata)
                        {
                            item.field = string.IsNullOrWhiteSpace(item.field) ? item.title.Replace(" ", "_") : item.field;
                        }

                        ldataPrincipal.AddRange(ldata);

                        return ldataPrincipal;
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

        public override DataTable GetPublicacionAplicacionDesestimada(PaginacionReporteAplicacion pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                var data = new DataTable();

                var columnas = GetColumnasPublicacionAplicacionToBD(pag.TablaProcedencia);
                pag.Columnas = columnas;
                data = ReportePublicacionAplicacionDesestimadas2(pag);

                if (data != null && data.Rows.Count > 0) totalRows = Convert.ToInt16(data.Rows[0]["TotalFilas"]);

                return data;
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetTecnologias()"
                    , new object[] { null });
            }
        }


        public DataTable ReportePublicacionAplicacionDesestimadas2(PaginacionReporteAplicacion pag)
        {
            DataSet resultado = null;
            var cadenaConexion = Constantes.CadenaConexion;

            using (SqlConnection cnx = new SqlConnection(cadenaConexion))
            {
                cnx.Open();
                using (var comando = new SqlCommand("[CVT].[USP_Reporte_Publicacion_APP_Desestimada]", cnx))
                {
                    comando.CommandTimeout = 0;
                    comando.CommandType = System.Data.CommandType.StoredProcedure;
                    comando.Parameters.Add(new SqlParameter("@gerencia", pag.Gerencia));
                    comando.Parameters.Add(new SqlParameter("@division", pag.Division));
                    comando.Parameters.Add(new SqlParameter("@area", pag.Area));
                    comando.Parameters.Add(new SqlParameter("@unidad", pag.Unidad));
                    comando.Parameters.Add(new SqlParameter("@estado", pag.Estado));
                    comando.Parameters.Add(new SqlParameter("@clasificacionTecnica", pag.ClasificacionTecnica));
                    comando.Parameters.Add(new SqlParameter("@subclasificacionTecnica", pag.SubclasificacionTecnica));
                    comando.Parameters.Add(new SqlParameter("@aplicacion", string.IsNullOrWhiteSpace(pag.Aplicacion) ? string.Empty : pag.Aplicacion));
                    comando.Parameters.Add(new SqlParameter("@Columnas", pag.Columnas));
                    comando.Parameters.Add(new SqlParameter("@TablaProcedencia", pag.Procedencia));
                    comando.Parameters.Add(new SqlParameter("@PageSize", pag.pageSize));
                    comando.Parameters.Add(new SqlParameter("@PageNumber", pag.pageNumber));
                    comando.Parameters.Add(new SqlParameter("@tipoActivo", pag.TipoActivo));

                    IDbDataAdapter adapter = new SqlDataAdapter(comando);
                    resultado = new DataSet();
                    adapter.Fill(resultado);
                }

                cnx.Close();

                return resultado.Tables[0];
            }
        }

        public DataTable ReportePublicacionFormatosRegistro(PaginacionReporteAplicacion pag)
        {
            DataSet resultado = null;
            var cadenaConexion = Constantes.CadenaConexion;

            using (SqlConnection cnx = new SqlConnection(cadenaConexion))
            {
                cnx.Open();
                using (var comando = new SqlCommand("[CVT].[USP_Reporte_Publicacion_Formato_Registro]", cnx))
                {
                    comando.CommandTimeout = 0;
                    comando.CommandType = System.Data.CommandType.StoredProcedure;
                    comando.Parameters.Add(new SqlParameter("@gerencia", pag.Gerencia));
                    comando.Parameters.Add(new SqlParameter("@division", pag.Division));
                    comando.Parameters.Add(new SqlParameter("@area", pag.Area));
                    comando.Parameters.Add(new SqlParameter("@unidad", pag.Unidad));
                    comando.Parameters.Add(new SqlParameter("@estado", pag.Estado));
                    comando.Parameters.Add(new SqlParameter("@clasificacionTecnica", pag.ClasificacionTecnica));
                    comando.Parameters.Add(new SqlParameter("@subclasificacionTecnica", pag.SubclasificacionTecnica));
                    comando.Parameters.Add(new SqlParameter("@aplicacion", string.IsNullOrWhiteSpace(pag.Aplicacion) ? string.Empty : pag.Aplicacion));
                    comando.Parameters.Add(new SqlParameter("@Columnas", pag.Columnas));
                    comando.Parameters.Add(new SqlParameter("@TablaProcedencia", pag.Procedencia));
                    comando.Parameters.Add(new SqlParameter("@PageSize", pag.pageSize));
                    comando.Parameters.Add(new SqlParameter("@PageNumber", pag.pageNumber));
                    comando.Parameters.Add(new SqlParameter("@tipoActivo", pag.TipoActivo));

                    IDbDataAdapter adapter = new SqlDataAdapter(comando);
                    resultado = new DataSet();
                    adapter.Fill(resultado);
                }

                cnx.Close();

                return resultado.Tables[0];
            }
        }

        public override DataTable GetAplicacionesDesestimadas(PaginacionReporteAplicacion pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                var data = new DataTable();

                var columnas = GetColumnasPublicacionAplicacionToBD(pag.TablaProcedencia);
                pag.Columnas = columnas;
                switch (pag.Procedencia)
                {
                    /*case (int)ETablaProcedenciaAplicacion.Aplicacion:
                    case (int)ETablaProcedenciaAplicacion.AplicacionDetalle:
                        data = ReportePublicacionAplicacion(pag);
                        break;
                    case (int)ETablaProcedenciaAplicacion.AplicacionData:
                        data = ReportePublicacionAplicacionData(pag);
                        break;*/
                    case (int)ETablaProcedenciaAplicacion.InfoCampoPortafolio:
                        data = ReportePublicacionAplicacionDesestimadas2(pag);
                        break;
                }

                if (data != null && data.Rows.Count > 0) totalRows = Convert.ToInt16(data.Rows[0]["TotalFilas"]);

                return data;
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetTecnologias()"
                    , new object[] { null });
            }
        }

        public override DataTable GetFormatosRegistro(PaginacionReporteAplicacion pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                var data = new DataTable();

                var columnas = GetColumnasPublicacionAplicacionToBD(pag.TablaProcedencia);
                pag.Columnas = columnas;
                switch (pag.Procedencia)
                {
                    /*case (int)ETablaProcedenciaAplicacion.Aplicacion:
                    case (int)ETablaProcedenciaAplicacion.AplicacionDetalle:
                        data = ReportePublicacionAplicacion(pag);
                        break;
                    case (int)ETablaProcedenciaAplicacion.AplicacionData:
                        data = ReportePublicacionAplicacionData(pag);
                        break;*/
                    case (int)ETablaProcedenciaAplicacion.InfoCampoPortafolio:
                        data = ReportePublicacionFormatosRegistro(pag);
                        break;
                }

                if (data != null && data.Rows.Count > 0) totalRows = Convert.ToInt16(data.Rows[0]["TotalFilas"]);

                return data;
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetTecnologias()"
                    , new object[] { null });
            }
        }

        public override bool ExisteCodigoSIGAByFilter(string codigo)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    bool? estado = (from u in ctx.BCP_CATG_GH_UNIDADES
                                    where u.COD_UNIDAD == codigo
                                    orderby u.COD_UNIDAD
                                    select true).FirstOrDefault();

                    return estado == true;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: bool ExisteCodigoSIGAByFilter(string codigo)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: bool ExisteCodigoSIGAByFilter(string codigo)"
                    , new object[] { null });
            }
        }

        public override DataTable GetPublicacionAplicacionPortafolioAplicaciones(PaginacionReporteAplicacion pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                var data = new DataTable();

                var columnas = GetColumnasPublicacionAplicacionToBD(pag.TablaProcedencia);
                pag.Columnas = columnas;
                data = ReportePublicacionAplicacionPortafolioAplicaciones(pag);

                if (data != null && data.Rows.Count > 0) totalRows = Convert.ToInt16(data.Rows[0]["TotalFilas"]);

                return data;
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetTecnologias()"
                    , new object[] { null });
            }
        }

        public override List<AplicacionDTO> GetResponsablePortafolio(out int totalRows)
        {
            try
            {
                List<AplicacionDTO> registros = new List<AplicacionDTO>();
                totalRows = 0;
                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_RESPONSABLES_PORTAFOLIO_LISTAR_EXPORTAR]", cnx))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;

                        var dr = comando.ExecuteReader();

                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                var item = new AplicacionDTO();
                                item.CodigoAPT = dr.GetData<string>("AplicacionId");
                                item.NombreRol = dr.GetData<string>("NombreRol");
                                item.Nombre = dr.GetData<string>("Nombre");
                                item.Email = dr.GetData<string>("Email");
                                item.Matricula = dr.GetData<string>("Matricula");

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
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<AplicacionDTO> GetResponsablePortafolio(out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<AplicacionDTO> GetResponsablePortafolio(out int totalRows)"
                    , new object[] { null });
            }
        }
        public override List<CustomAutocomplete> GetProductoManagerFiltro(string filtro)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.ProductoManager
                                   where
                                   (string.IsNullOrEmpty(filtro) || u.Nombre.ToUpper().Contains(filtro.ToUpper()))
                                   select new CustomAutocomplete()
                                   {
                                       Id = u.ProductoManagerId.ToString(),
                                       Descripcion = u.Nombre,
                                       value = u.Nombre
                                   }).ToList();

                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetProductoManagerFiltro(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetProductoManagerFiltro(string filtro)"
                    , new object[] { null });
            }
        }

        public override List<EtiquetaAplicacionDTO> GetListEtiquetas()
        {
            try
            {
                var listaEtiquetaAplicacion = new List<EtiquetaAplicacionDTO>();

                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();

                    using (var comando = new SqlCommand("[cvt].[DA_GetListTipoEtiqueta]", cnx))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new EtiquetaAplicacionDTO()
                            {
                                TipoEtiquetaId = reader.IsDBNull(reader.GetOrdinal("TipoEtiquetaId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TipoEtiquetaId")),
                                NombreEtiqueta = reader.IsDBNull(reader.GetOrdinal("NombreEtiqueta")) ? string.Empty : reader.GetString(reader.GetOrdinal("NombreEtiqueta"))
                            };
                            listaEtiquetaAplicacion.Add(objeto);
                        }
                        reader.Close();
                    }

                    cnx.Close();

                    return listaEtiquetaAplicacion;

                }
            }
            catch (SqlException ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: List<EtiquetaAplicacionDTO> GetListEtiquetas()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: List<EtiquetaAplicacionDTO> GetListEtiquetas()"
                    , new object[] { null });
            }
        }

        public override EtiquetaAplicacionDTO GetAplicacionEtiqueta(string codigoAPT)
        {
            try
            {
                var etiquetaAplicacion = new EtiquetaAplicacionDTO();

                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();

                    using (var comando = new SqlCommand("[cvt].[DA_GetAplicacionEtiqueta]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@codigoAPT", codigoAPT));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new EtiquetaAplicacionDTO()
                            {
                                TipoEtiquetaId = reader.IsDBNull(reader.GetOrdinal("EtiquetaId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EtiquetaId")),
                                NombreEtiqueta = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? string.Empty : reader.GetString(reader.GetOrdinal("Descripcion"))
                            };
                            etiquetaAplicacion = objeto;
                        }
                        reader.Close();
                    }

                    cnx.Close();

                    return etiquetaAplicacion;

                }
            }
            catch (SqlException ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: List<EtiquetaAplicacionDTO> GetListEtiquetas()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: List<EtiquetaAplicacionDTO> GetListEtiquetas()"
                    , new object[] { null });
            }
        }

        public override UnidadFondeoDTO GetAplicacionUnidadFondeo(string codigoAPT)
        {
            try
            {
                var uf = new UnidadFondeoDTO();

                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();

                    using (var comando = new SqlCommand("[cvt].[USP_Unidad_Fondeo_GetAplicacion]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@codigoAPT", codigoAPT));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new UnidadFondeoDTO()
                            {
                                UnidadFondeoId = reader.IsDBNull(reader.GetOrdinal("UnidadFondeoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("UnidadFondeoId")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre"))
                            };
                            uf = objeto;
                        }
                        reader.Close();
                    }

                    cnx.Close();

                    return uf;

                }
            }
            catch (SqlException ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: List<EtiquetaAplicacionDTO> GetListEtiquetas()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: List<EtiquetaAplicacionDTO> GetListEtiquetas()"
                    , new object[] { null });
            }
        }

        public override List<UnidadFondeoDTO> GetListUnidadFondeo()
        {
            try
            {
                var lista = new List<UnidadFondeoDTO>();

                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();

                    using (var comando = new SqlCommand("[cvt].[USP_Unidad_Fondeo_Combo]", cnx))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new UnidadFondeoDTO()
                            {
                                UnidadFondeoId = reader.IsDBNull(reader.GetOrdinal("UnidadFondeoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("UnidadFondeoId")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                Id = reader.IsDBNull(reader.GetOrdinal("UnidadFondeoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("UnidadFondeoId")),
                                Descripcion = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre"))
                            };
                            lista.Add(objeto);
                        }
                        reader.Close();
                    }

                    cnx.Close();

                    return lista;

                }
            }
            catch (SqlException ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: List<EtiquetaAplicacionDTO> GetListEtiquetas()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: List<EtiquetaAplicacionDTO> GetListEtiquetas()"
                    , new object[] { null });
            }
        }

        public override List<SegundoNivelDTO> GetListSegundoNivel()
        {
            try
            {
                var lista = new List<SegundoNivelDTO>();

                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();

                    using (var comando = new SqlCommand("[cvt].[USP_SegundoNivel_List]", cnx))
                    {
                        comando.CommandType = System.Data.CommandType.StoredProcedure;

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new SegundoNivelDTO()
                            {
                                Id = reader.IsDBNull(reader.GetOrdinal("Id")) ? 0 : reader.GetInt32(reader.GetOrdinal("Id")),
                                Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? string.Empty : reader.GetString(reader.GetOrdinal("Descripcion"))
                            };
                            lista.Add(objeto);
                        }
                        reader.Close();
                    }

                    cnx.Close();

                    return lista;

                }
            }
            catch (SqlException ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: List<EtiquetaAplicacionDTO> GetListEtiquetas()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorDependencias
                    , "Error en el metodo: List<EtiquetaAplicacionDTO> GetListEtiquetas()"
                    , new object[] { null });
            }
        }

        public override bool GuardarUnidadFondeo(GuardarUnidadFondeoAplicacionDTO item)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;

                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();

                    using (var comando = new SqlCommand("[cvt].[USP_Unidad_Fondeo_Aplicacion_Guardar]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@UnidadFondeoId", item.UnidadFondeoId));
                        comando.Parameters.Add(new SqlParameter("@CodigoAPT", item.CodigoAPT));
                        comando.Parameters.Add(new SqlParameter("@UsuarioModificacion", item.UsuarioModificacion));
                        comando.ExecuteNonQuery();
                    }

                    cnx.Close();
                    return true;
                }

            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: GetResultsCargaMasivaPortafolio(string comentario, byte[] data)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: GetResultsCargaMasivaPortafolio(string comentario, byte[] data)"
                    , new object[] { null });
            }
        }

        public override bool GuardarEtiqueta(GuardarEtiquetaAplicacionDTO item)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;

                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();

                    using (var comando = new SqlCommand("[cvt].[usp_guardarEtiquetaAplicacion]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@EtiquetaId", item.EtiquetaId));
                        comando.Parameters.Add(new SqlParameter("@CodigoAPT", item.CodigoAPT));
                        comando.Parameters.Add(new SqlParameter("@UsuarioModificacion", item.UsuarioModificacion));
                        comando.ExecuteNonQuery();
                    }

                    cnx.Close();
                    return true;
                }

            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: GetResultsCargaMasivaPortafolio(string comentario, byte[] data)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: GetResultsCargaMasivaPortafolio(string comentario, byte[] data)"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocompleteAplicacion> ListAplicaciones_ByPerfil(int perfil, string aplicacion, string usuario)
        {
            try
            {
                var lista = new List<CustomAutocompleteAplicacion>();

                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_ListAplicaciones_ByPerfil]", cnx))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@perfil", perfil);
                        comando.Parameters.AddWithValue("@aplicacion", aplicacion);
                        comando.Parameters.AddWithValue("@usuario", usuario);

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new CustomAutocompleteAplicacion()
                            {
                                Id = reader.IsDBNull(reader.GetOrdinal("Id")) ? string.Empty : reader.GetString(reader.GetOrdinal("Id")),
                                Descripcion = reader.IsDBNull(reader.GetOrdinal("Descripcion")) ? string.Empty : reader.GetString(reader.GetOrdinal("Descripcion")),
                                value = reader.IsDBNull(reader.GetOrdinal("value")) ? string.Empty : reader.GetString(reader.GetOrdinal("value")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                CategoriaTecnologica = reader.IsDBNull(reader.GetOrdinal("CategoriaTecnologica")) ? string.Empty : reader.GetString(reader.GetOrdinal("CategoriaTecnologica")),
                                UsuarioLider = reader.IsDBNull(reader.GetOrdinal("UsuarioLider")) ? string.Empty : reader.GetString(reader.GetOrdinal("UsuarioLider")),
                                TipoActivo = reader.IsDBNull(reader.GetOrdinal("TipoActivo")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoActivo")),
                                EstadoAplicacion = reader.IsDBNull(reader.GetOrdinal("EstadoAplicacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("EstadoAplicacion")),
                                IdAplicacion = reader.IsDBNull(reader.GetOrdinal("IdAplicacion")) ? 0 : reader.GetInt32(reader.GetOrdinal("IdAplicacion"))
                            };
                            lista.Add(objeto);
                        }
                        reader.Close();
                    }

                    return lista;
                }
            }
            catch (SqlException ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorRelacionesApps
                    , "Error en el metodo: RelacionScv.GetList_DependenciaAplicacion(int perfil, string username, string codigoAPT, int estadoId, int pageNumber, int pageSize)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorRelacionesApps
                    , "Error en el metodo: RelacionScv.GetList_DependenciaAplicacion(int perfil, string username, string codigoAPT, int estadoId, int pageNumber, int pageSize)"
                    , new object[] { null });
            }
        }

        public override List<AplicacionDTO> DiagramaInfra_GetAplicacionConfiguracion(PaginacionAplicacion pag, out int totalRows, out string arrCodigoAPTs)
        {
            try
            {
                totalRows = 0;
                arrCodigoAPTs = string.Empty;
                var fechaConsulta = DateTime.Now;

                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<AplicacionDTO>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();

                    using (var comando = new SqlCommand("[cvt].[Usp_DiagramaInfraestructura_GetAplicacionConfiguracion]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@aplicacion", pag.Aplicacion));
                        comando.Parameters.Add(new SqlParameter("@estado", pag.Estado == null ? "" : pag.Estado));
                        comando.Parameters.Add(new SqlParameter("@UnidadFondeoId", pag.UnidadFondeoId));
                        comando.Parameters.Add(new SqlParameter("@matricula", pag.username == null ? "" : pag.username));
                        comando.Parameters.Add(new SqlParameter("@gerenciaCentral", pag.Gerencia == null ? "" : pag.Gerencia));
                        comando.Parameters.Add(new SqlParameter("@division", pag.Division == null ? "" : pag.Division));
                        comando.Parameters.Add(new SqlParameter("@unidad", pag.Unidad == null ? "" : pag.Unidad));
                        comando.Parameters.Add(new SqlParameter("@area", pag.Area == null ? "" : pag.Area));
                        comando.Parameters.Add(new SqlParameter("@jefeEquipo", pag.JefeEquipo == null ? "" : pag.JefeEquipo));
                        comando.Parameters.Add(new SqlParameter("@owner", pag.Owner == null ? "" : pag.Owner));
                        comando.Parameters.Add(new SqlParameter("@perfilId", pag.PerfilId));
                        comando.Parameters.Add(new SqlParameter("@anioRegistro", fechaConsulta.Year));
                        comando.Parameters.Add(new SqlParameter("@mesRegistro", fechaConsulta.Month));
                        comando.Parameters.Add(new SqlParameter("@diaRegistro", fechaConsulta.Day));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new AplicacionDTO()
                            {
                                Id = reader.IsDBNull(reader.GetOrdinal("Id")) ? 0 : reader.GetInt32(reader.GetOrdinal("Id")),
                                CodigoAPT = reader.IsDBNull(reader.GetOrdinal("CodigoAPT")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAPT")),
                                CodigoAPTStr = reader.IsDBNull(reader.GetOrdinal("CodigoAPTStr")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAPTStr")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                GestionadoPor = reader.IsDBNull(reader.GetOrdinal("GestionadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("GestionadoPor")),
                                CriticidadId = reader.IsDBNull(reader.GetOrdinal("CriticidadId")) ? 0 : reader.GetInt32(reader.GetOrdinal("CriticidadId")),
                                RoadMapId = reader.IsDBNull(reader.GetOrdinal("RoadMapId")) ? 0 : reader.GetInt32(reader.GetOrdinal("RoadMapId")),
                                RoadMapToString = reader.IsDBNull(reader.GetOrdinal("RoadMapToString")) ? string.Empty : reader.GetString(reader.GetOrdinal("RoadMapToString")),
                                Matricula = reader.IsDBNull(reader.GetOrdinal("Matricula")) ? string.Empty : reader.GetString(reader.GetOrdinal("Matricula")),
                                Obsolescente = reader.IsDBNull(reader.GetOrdinal("Obsolescente")) ? 0 : reader.GetInt32(reader.GetOrdinal("Obsolescente")),
                                Activo = reader.GetBoolean(reader.GetOrdinal("Activo")),
                                UsuarioCreacion = reader.IsDBNull(reader.GetOrdinal("UsuarioCreacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("UsuarioCreacion")),
                                FechaCreacion = reader.IsDBNull(reader.GetOrdinal("FechaCreacion")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("FechaCreacion")),
                                FechaRegistroProcedencia = reader.IsDBNull(reader.GetOrdinal("FechaRegistroProcedencia")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("FechaRegistroProcedencia")),
                                FechaModificacion = reader.IsDBNull(reader.GetOrdinal("FechaModificacion")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("FechaModificacion")),
                                UsuarioModificacion = reader.IsDBNull(reader.GetOrdinal("UsuarioModificacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("UsuarioModificacion")),
                                FlagRelacionar = reader.IsDBNull(reader.GetOrdinal("FlagRelacionar")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("FlagRelacionar")),
                                CriticidadToString = reader.IsDBNull(reader.GetOrdinal("CriticidadToString")) ? string.Empty : reader.GetString(reader.GetOrdinal("CriticidadToString")),
                                TipoActivoInformacion = reader.IsDBNull(reader.GetOrdinal("TipoActivoInformacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoActivoInformacion")),
                                GerenciaCentral = reader.IsDBNull(reader.GetOrdinal("GerenciaCentral")) ? string.Empty : reader.GetString(reader.GetOrdinal("GerenciaCentral")),
                                Division = reader.IsDBNull(reader.GetOrdinal("Division")) ? string.Empty : reader.GetString(reader.GetOrdinal("Division")),
                                Unidad = reader.IsDBNull(reader.GetOrdinal("Unidad")) ? string.Empty : reader.GetString(reader.GetOrdinal("Unidad")),
                                Area = reader.IsDBNull(reader.GetOrdinal("Area")) ? string.Empty : reader.GetString(reader.GetOrdinal("Area")),
                                NombreEquipo_Squad = reader.IsDBNull(reader.GetOrdinal("NombreEquipo_Squad")) ? string.Empty : reader.GetString(reader.GetOrdinal("NombreEquipo_Squad")),
                                Owner_LiderUsuario_ProductOwner = reader.IsDBNull(reader.GetOrdinal("Owner_LiderUsuario_ProductOwner")) ? string.Empty : reader.GetString(reader.GetOrdinal("Owner_LiderUsuario_ProductOwner")),
                                JefeEquipo_ExpertoAplicacionUserIT_ProductOwner = reader.IsDBNull(reader.GetOrdinal("JefeEquipo_ExpertoAplicacionUserIT_ProductOwner")) ? string.Empty : reader.GetString(reader.GetOrdinal("JefeEquipo_ExpertoAplicacionUserIT_ProductOwner")),
                                Experto_Especialista = reader.IsDBNull(reader.GetOrdinal("Experto_Especialista")) ? string.Empty : reader.GetString(reader.GetOrdinal("Experto_Especialista")),
                                TotalEquiposRelacionados = reader.IsDBNull(reader.GetOrdinal("TotalEquiposRelacionados")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalEquiposRelacionados")),
                                EstadoAplicacion = reader.IsDBNull(reader.GetOrdinal("EstadoAplicacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("EstadoAplicacion")),
                                Agrupacion = reader.IsDBNull(reader.GetOrdinal("Agrupacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("Agrupacion")),
                                UnidadFondeo = reader.IsDBNull(reader.GetOrdinal("UnidadFondeo")) ? string.Empty : reader.GetString(reader.GetOrdinal("UnidadFondeo")),
                                TipoExperto = reader.IsDBNull(reader.GetOrdinal("TipoExpertoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TipoExpertoId"))
                            };

                            lista.Add(objeto);
                        }
                        reader.Close();
                    }

                    arrCodigoAPTs = string.Join("|", lista.Select(x => x.CodigoAPT).ToArray());
                    totalRows = lista.Count();
                    var resultado = lista.Skip((pag.pageNumber - 1) * pag.pageSize).Take(pag.pageSize).ToList();

                    return resultado;

                }

            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<AplicacionDTO> GetAplicacion(string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<AplicacionDTO> GetAplicacion(string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }
    }
}



public class DescriptionAttributes<T>
{
    protected List<DescriptionAttribute> Attributes = new List<DescriptionAttribute>();
    public List<string> Descriptions { get; set; }

    public DescriptionAttributes()
    {
        RetrieveAttributes();
        Descriptions = Attributes.Select(x => x.Description).ToList();
    }

    private void RetrieveAttributes()
    {
        foreach (var attribute in typeof(T).GetMembers().SelectMany(member => member.GetCustomAttributes(typeof(DescriptionAttribute), true).Cast<DescriptionAttribute>()))
        {
            Attributes.Add(attribute);
        }
    }
}