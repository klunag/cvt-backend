using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.DTO.Custom;
using BCP.CVT.Services.Interface;
using BCP.CVT.Services.ModelDB;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using IsolationLevel = System.Transactions.IsolationLevel;
using System.Data.Entity.SqlServer;
using DocumentFormat.OpenXml.Office.Word;
using BCP.CVT.Services.Log;

namespace BCP.CVT.Services.Service
{
    public class RelacionSvc : RelacionDAO
    {
      

        private void EnviarNotificacionRelacion(string CodigoAPT, string UsuarioCreacion, string Cuerpo)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    string nombreBuzon, buzon, buzonDefecto, asunto = string.Empty;

                    buzon = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("RELACION_GESTION_BUZON").Valor;
                    nombreBuzon = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("RELACION_GESTION_BUZON_NOMBRE").Valor;
                    buzonDefecto = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("RELACION_GESTION_BUZON_DEFECTO").Valor;
                    asunto = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("RELACION_GESTION_ASUNTO_NOTIFICACION").Valor;

                    NotificacionDTO notificacion = new NotificacionDTO();
                    //notificacion.TipoNotificacionId = (int)ETipoNotificacion.Relacion; // ENUM
                    notificacion.Nombre = nombreBuzon;
                    notificacion.De = buzon;
                    notificacion.Para = string.Empty;

                    var expertos = (from u in ctx.AplicacionExpertos
                                    where u.CodigoAPT == CodigoAPT
                                    && u.FlagActivo
                                    select u.Nombres).ToList();

                    if (expertos != null && expertos.Count() > 0)
                    {
                        foreach (var item in expertos)
                        {
                            notificacion.Para = notificacion.Para + item + ";";
                        }
                    }
                    else
                    {
                        notificacion.Para = buzonDefecto;
                    }

                    notificacion.CC = buzonDefecto;
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

        public override long AddOrEditRelacion(RelacionDTO objRegistro)
        {
            DbContextTransaction transaction = null;
            try
            {
                long ID = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    using (transaction = ctx.Database.BeginTransaction())
                    {
                        DateTime FECHA_ACTUAL = DateTime.Now;
                        if (objRegistro.RelacionId == 0)
                        {
                            var objExperto = ctx.AplicacionPortafolioResponsables.Where(x => x.FlagActivo
                                                                                        && x.Matricula == objRegistro.UsuarioCreacion
                                                                                        && x.CodigoAPT == objRegistro.CodigoAPT).FirstOrDefault();
                            var idestado = objExperto != null ? (int)EEstadoRelacion.Aprobado : objRegistro.EstadoId;

                            if (objExperto == null)
                            {
                                var objExperto2 = ctx.AplicacionExpertos.Where(x => x.FlagActivo
                                                                                && x.Matricula == objRegistro.UsuarioCreacion
                                                                                && x.CodigoAPT == objRegistro.CodigoAPT
                                                                                && x.TipoExpertoId == (int)ETipoExperto.ExpertoAplicacion).FirstOrDefault();

                                idestado = objExperto2 != null ? (int)EEstadoRelacion.Aprobado : objRegistro.EstadoId;
                            }

                            var equipo = ctx.Equipo.FirstOrDefault(x => x.EquipoId == objRegistro.EquipoId);

                            var entidad = new Relacion()
                            {
                                CodigoAPT = objRegistro.CodigoAPT,
                                TipoId = objRegistro.TipoId,
                                AmbienteId = objRegistro.AmbienteId,
                                EquipoId = objRegistro.EquipoId,
                                //EstadoId = objRegistro.EstadoId,
                                EstadoId = idestado,
                                FlagActivo = objRegistro.Activo,
                                FechaCreacion = FECHA_ACTUAL,
                                CreadoPor = objRegistro.UsuarioCreacion,
                                FlagRelacionAplicacion = objRegistro.FlagRelacionAplicacion,
                                Funcion = objRegistro.Funcion,
                                DiaRegistro = FECHA_ACTUAL.Day,
                                MesRegistro = FECHA_ACTUAL.Month,
                                AnioRegistro = FECHA_ACTUAL.Year
                                //Relación App-Componente
                                ,
                                TipoRelacionComponente = objRegistro.TipoRelacionComponente
                            };
                            ctx.Relacion.Add(entidad);
                            ctx.SaveChanges();

                            //string Cuerpo = "Creación de una nueva relación";
                            //this.EnviarNotificacionRelacion(entidad.CodigoAPT, objRegistro.UsuarioCreacion, Cuerpo);

                            ID = entidad.RelacionId;
                        }
                        else
                        {
                            var equipo = ctx.Equipo.FirstOrDefault(x => x.EquipoId == objRegistro.EquipoId);

                            var entidad = (from u in ctx.Relacion
                                           where u.RelacionId == objRegistro.RelacionId && u.FlagActivo
                                           select u).FirstOrDefault();
                            if (entidad != null)
                            {
                                entidad.CodigoAPT = objRegistro.CodigoAPT;
                                entidad.TipoId = objRegistro.TipoId;
                                entidad.EquipoId = objRegistro.EquipoId;
                                entidad.AmbienteId = objRegistro.AmbienteId;
                                entidad.FlagActivo = objRegistro.Activo;
                                entidad.FechaModificacion = FECHA_ACTUAL;
                                entidad.ModificadoPor = objRegistro.UsuarioModificacion;
                                entidad.DiaRegistro = FECHA_ACTUAL.Day;
                                entidad.MesRegistro = FECHA_ACTUAL.Month;
                                entidad.AnioRegistro = FECHA_ACTUAL.Year;
                                entidad.FlagRelacionAplicacion = objRegistro.FlagRelacionAplicacion;
                                entidad.Funcion = objRegistro.Funcion;
                                //Relación App-Componente
                                entidad.TipoRelacionComponente = objRegistro.TipoRelacionComponente;
                                ctx.SaveChanges();
                                ID = entidad.RelacionId;
                            }
                        }

                        //Remove items relaciondetalle
                        var relacionDetalle = (from u in ctx.RelacionDetalle
                                               where u.RelacionId == objRegistro.RelacionId
                                               select u).ToList();
                        if (relacionDetalle != null && relacionDetalle.Count > 0)
                        {
                            foreach (var item in relacionDetalle)
                            {
                                //var regItemRemove = (from u in ctx.RelacionDetalle
                                //                     where u.TecnologiaId == item
                                //                     && u.RelacionId == ID
                                //                     && u.FlagActivo
                                //                     select u).FirstOrDefault();

                                if (item != null)
                                {
                                    item.FlagActivo = false;
                                    ctx.SaveChanges();
                                }
                            }
                        }

                        //Add or Edit items relacion
                        if (objRegistro.ListRelacionDetalle != null && objRegistro.ListRelacionDetalle.Count > 0)
                        {
                            foreach (var item in objRegistro.ListRelacionDetalle)
                            {
                                var obj = new RelacionDetalle();

                                if (objRegistro.TipoId == (int)ETipoRelacion.Tecnologia || objRegistro.TipoId == (int)ETipoRelacion.ServicioNube)
                                {
                                    obj = (from u in ctx.RelacionDetalle
                                           where u.RelacionId == ID
                                           && u.TecnologiaId == item.TecnologiaId
                                           select u).FirstOrDefault();
                                }
                                else
                                {
                                    obj = (from u in ctx.RelacionDetalle
                                           where u.RelacionId == ID
                                           && u.TecnologiaId == item.TecnologiaId
                                           select u).FirstOrDefault();
                                }

                                var TecnologiaIdTemp = 0;
                                if (item.TecnologiaId != -1)
                                    TecnologiaIdTemp = item.TecnologiaId;

                                if (obj != null)
                                {
                                    obj.RelacionId = ID;
                                    obj.TecnologiaId = TecnologiaIdTemp;//item.TecnologiaId != -1 ? item.TecnologiaId : null ;
                                    obj.RelevanciaId = item.RelevanciaId;
                                    obj.Componente = item.Componente;
                                    obj.FlagActivo = item.Activo;
                                    obj.FechaModificacion = DateTime.Now;
                                    obj.ModificadoPor = item.UsuarioModificacion;
                                    obj.CodigoAPTVinculo = item.CodigoAPTVinculo;
                                    obj.DetalleVinculo = item.DetalleVinculo;
                                    ctx.SaveChanges();
                                }
                                else
                                {
                                    var objRelacionDetalle = new RelacionDetalle()
                                    {
                                        RelacionId = ID,
                                        TecnologiaId = TecnologiaIdTemp, //TODO
                                        RelevanciaId = item.RelevanciaId,
                                        Componente = item.Componente,
                                        FlagActivo = item.Activo,
                                        FechaCreacion = DateTime.Now,
                                        CreadoPor = item.UsuarioCreacion,
                                        CodigoAPTVinculo = item.CodigoAPTVinculo,
                                        DetalleVinculo = item.DetalleVinculo
                                    };
                                    ctx.RelacionDetalle.Add(objRelacionDetalle);
                                    ctx.SaveChanges();
                                }
                                if(objRegistro.EquipoId != null)
                                {
                                    ServiceManager<InstanciasBDDAO>.Provider.AddEditInstanciaBd((int)objRegistro.EquipoId, item.Componente);
                                }
                                
                                obj = null;
                            }
                        }
                        transaction.Commit();

                        this.ActualizarRelacion(objRegistro.CodigoAPT);
                        return ID;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                transaction.Rollback();
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorRelacionDTO
                    , "Error en el metodo: int AddOrEditRelacion(RelacionDTO objRegistro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorRelacionDTO
                    , "Error en el metodo: int AddOrEditRelacion(RelacionDTO objRegistro)"
                    , new object[] { null });
            }
        }

        public override bool CambiarEstado(RelacionDTO objRegistro, DateTime? Fecha = null, bool FlagAdmin = false)
        {
            try
            {

                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var CURRENT_DATE = DateTime.Now;
                    var dia = CURRENT_DATE.Day;
                    var mes = CURRENT_DATE.Month;
                    var anio = CURRENT_DATE.Year;

                    string CuerpoNotificacion = string.Empty;

                    var itemBD = ctx.Relacion.FirstOrDefault(x => x.RelacionId == objRegistro.Id);

                    if (itemBD != null)
                    {
                        if (objRegistro.EquipoId > 0 && objRegistro.FlagRemoveEquipo && !FlagAdmin)
                        {
                            var diaFiltro = Fecha.Value.Day;
                            var mesFiltro = Fecha.Value.Month;
                            var anioFiltro = Fecha.Value.Year;

                            var cantRelXEquipo = (from u in ctx.Aplicacion
                                                  join a in ctx.Relacion on u.CodigoAPT equals a.CodigoAPT
                                                  join b in ctx.Equipo on a.EquipoId equals b.EquipoId
                                                  where b.EquipoId == objRegistro.EquipoId && u.FlagActivo
                                                  && (a.DiaRegistro == diaFiltro && a.MesRegistro == mesFiltro && a.AnioRegistro == anioFiltro)
                                                  select u).Count();

                            if (cantRelXEquipo < 2) return false;
                        }

                        itemBD.EstadoId = objRegistro.EstadoId;
                        itemBD.FechaModificacion = CURRENT_DATE;
                        itemBD.ModificadoPor = objRegistro.UsuarioModificacion;
                        switch (objRegistro.EstadoId)
                        {
                            case (int)EEstadoRelacion.Eliminado:
                                //CuerpoNotificacion = "Relación en estado ELIMINADO";
                                itemBD.Observacion = objRegistro.Observacion;
                                //itemBD.FlagActivo = false;
                                break;
                            case (int)EEstadoRelacion.Desaprobado:
                                //CuerpoNotificacion = "Relación en estado DESAPROBADO";
                                itemBD.Observacion = objRegistro.Observacion;
                                break;
                            case (int)EEstadoRelacion.Pendiente:
                                //CuerpoNotificacion = "Relación en estado PENDIENTE";
                                var comentario_anterior = (from u in ctx.ComentarioEliminacionRelacion
                                                           where u.FlagActivo && u.RelacionId == objRegistro.Id
                                                           orderby u.FechaCreacion descending
                                                           select u).FirstOrDefault();

                                if (comentario_anterior != null)
                                {
                                    comentario_anterior.FlagActivo = false;
                                    ctx.SaveChanges();
                                }

                                if (itemBD.TipoId == (int)ETipoRelacion.Equipo || itemBD.TipoId == (int)ETipoRelacion.ServicioNube)
                                {
                                    var relacionDetalleDB = ctx.RelacionDetalle.Where(x => x.RelacionId == objRegistro.Id);
                                    if (relacionDetalleDB != null && relacionDetalleDB.Count() > 0)
                                    {
                                        bool? estado = false;
                                        foreach (var itemRD_BD in relacionDetalleDB)
                                        {
                                            //var itemTec_BD = ctx.Tecnologia.FirstOrDefault(x => x.Activo && x.TecnologiaId == itemRD_BD.TecnologiaId);
                                            //var itemET_BD = ctx.EquipoTecnologia.FirstOrDefault(x => x.FlagActivo 
                                            //&& x.DiaRegistro == dia && x.MesRegistro == mes && x.AnioRegistro == anio
                                            //&& x.TecnologiaId == itemRD_BD.TecnologiaId);

                                            estado = (from eq in ctx.EquipoTecnologia
                                                      join t in ctx.Tecnologia on eq.TecnologiaId equals t.TecnologiaId
                                                      where eq.FlagActivo && t.Activo
                                                      && eq.DiaRegistro == dia && eq.MesRegistro == mes && eq.AnioRegistro == anio
                                                      && t.TecnologiaId == itemRD_BD.TecnologiaId
                                                      && eq.EquipoId == itemRD_BD.Relacion.EquipoId
                                                      select true).FirstOrDefault();

                                            itemRD_BD.FlagActivo = estado.Value;
                                            itemRD_BD.ModificadoPor = objRegistro.UsuarioModificacion;
                                            itemRD_BD.FechaModificacion = CURRENT_DATE;
                                        }
                                        ctx.SaveChanges();
                                    }
                                }

                                break;
                            case (int)EEstadoRelacion.PendienteEliminacion:
                                //Validacion aplicacion experto
                                var objExperto = ctx.AplicacionPortafolioResponsables.Where(x => x.FlagActivo
                                                                                        && x.Matricula == objRegistro.UsuarioCreacion
                                                                                        && x.CodigoAPT == itemBD.CodigoAPT).FirstOrDefault();
                                var idestado = objExperto != null ? (int)EEstadoRelacion.Eliminado : objRegistro.EstadoId;

                                if (objExperto == null)
                                {
                                    var objExperto2 = ctx.AplicacionExpertos.Where(x => x.FlagActivo
                                                                                    && x.Matricula == objRegistro.UsuarioCreacion
                                                                                    && x.CodigoAPT == itemBD.CodigoAPT
                                                                                    && x.TipoExpertoId == (int)ETipoExperto.ExpertoAplicacion).FirstOrDefault();

                                    idestado = objExperto2 != null ? (int)EEstadoRelacion.Eliminado : objRegistro.EstadoId;
                                }

                                itemBD.EstadoId = idestado;

                                //CuerpoNotificacion = "Relación en estado PENDIENTE DE ELIMINACIÓN";
                                var comentario = new ComentarioEliminacionRelacion()
                                {
                                    RelacionId = objRegistro.Id,
                                    Contenido = objRegistro.Observacion,
                                    FlagActivo = true,
                                    UsuarioCreacion = objRegistro.UsuarioModificacion,
                                    FechaCreacion = CURRENT_DATE
                                };
                                ctx.ComentarioEliminacionRelacion.Add(comentario);
                                ctx.SaveChanges();

                                break;
                            case (int)EEstadoRelacion.Aprobado:
                                {
                                    if (itemBD.TipoId == (int)ETipoRelacion.Equipo)
                                    {
                                        var equipo = ctx.Equipo.FirstOrDefault(x => x.EquipoId == itemBD.EquipoId);
                                        if (equipo.TipoEquipoId == (int)ETipoEquipo.Appliance)
                                        {
                                            try
                                            {
                                                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                                                {
                                                    cnx.Open();
                                                    using (var comando = new SqlCommand("[cvt].[USP_AprobarRelacionAppliance]", cnx))
                                                    {
                                                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                                                        comando.Parameters.AddWithValue("@equipo", itemBD.EquipoId);
                                                        int filasAfectadas = comando.ExecuteNonQuery();
                                                    }
                                                    cnx.Close();
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                HelperLog.Error(ex.Message);
                                            }
                                        }
                                    }

                                }
                                break;

                        }

                        ctx.SaveChanges();

                        //this.EnviarNotificacionRelacion(itemBD.CodigoAPT, objRegistro.UsuarioModificacion, CuerpoNotificacion);

                        switch (objRegistro.EstadoId)
                        {
                            case (int)EEstadoRelacion.Aprobado:
                            case (int)EEstadoRelacion.Eliminado:
                                bool seActualizoKPIAplicaciones = false;
                                try
                                {
                                    using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                                    {
                                        cnx.Open();
                                        using (var comando = new SqlCommand("[cvt].[USP_INSERT_INTO_APLICACIONCONFIGURACION_TO_APP]", cnx))
                                        {
                                            comando.CommandType = System.Data.CommandType.StoredProcedure;
                                            comando.Parameters.AddWithValue("@codigoAPTSearch", itemBD.CodigoAPT);
                                            int filasAfectadas = comando.ExecuteNonQuery();
                                            seActualizoKPIAplicaciones = true;
                                        }
                                        cnx.Close();
                                    }
                                }
                                catch (SqlException ex)
                                {
                                    HelperLog.Error(ex.Message);
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
                                break;
                        }

                        return true;
                    }
                    else
                        return false;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorRelacionDTO
                    , "Error en el metodo: bool CambiarEstado(RelacionDTO objRegistro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorRelacionDTO
                    , "Error en el metodo: bool CambiarEstado(RelacionDTO objRegistro)"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocomplete> GetAplicacion()
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.Aplicacion
                                   where u.FlagActivo && u.FlagRelacionar.Value
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
                throw new CVTException(CVTExceptionIds.ErrorRelacionDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetAplicacion()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorRelacionDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetAplicacion()"
                    , new object[] { null });
            }
        }

        public override List<RelacionDTO> GetRelacion(string codigoAPT
            , string componente
            , int pageNumber
            , int pageSize
            , string sortName
            , string sortOrder
            , string username
            , int? tipoId
            , int? estadoId
            , out int totalRows)
        {
            try
            {
                totalRows = 0;
                List<RelacionDTO> registros = null;
                int anio = DateTime.Now.Year;
                int mes = DateTime.Now.Month;
                int dia = DateTime.Now.Day;

                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        ctx.Database.CommandTimeout = 0;

                        var resultado = (from a1 in ctx.Relacion
                                         join b in ctx.Aplicacion on a1.CodigoAPT equals b.CodigoAPT
                                         join c in ctx.Equipo on a1.EquipoId equals c.EquipoId into c2
                                         from c in c2.DefaultIfEmpty()
                                         join amb in ctx.Ambiente on a1.AmbienteId equals amb.AmbienteId into c3
                                         from amb in c3.DefaultIfEmpty()
                                         join rd in ctx.RelacionDetalle on a1.RelacionId equals rd.RelacionId into c4
                                         from rd in c4.DefaultIfEmpty()
                                         join t in ctx.Tecnologia on rd.TecnologiaId equals t.TecnologiaId into lj6
                                         from t in lj6.DefaultIfEmpty()
                                         where a1.FlagActivo && a1.DiaRegistro.Value == dia && a1.MesRegistro.Value == mes && a1.AnioRegistro.Value == anio
                                         && b.FlagActivo && b.FlagRelacionar.Value
                                         && (b.CodigoAPT.ToUpper().Contains(codigoAPT.ToUpper())
                                         || string.IsNullOrEmpty(codigoAPT)
                                         || b.Nombre.ToUpper().Contains(codigoAPT.ToUpper())
                                         || (b.CodigoAPT + " - " + b.Nombre).ToUpper().Contains(codigoAPT.ToUpper()))

                                         group new { a1, b, c, amb, t } by new
                                         {
                                             a1.RelacionId,
                                             a1.CodigoAPT,
                                             b.Nombre,
                                             c.EquipoId,
                                             EquipoNombre = c.Nombre,
                                             a1.TipoId,
                                             a1.AmbienteId,
                                             amb.DetalleAmbiente,
                                             a1.EstadoId,
                                             a1.CreadoPor,
                                             a1.FechaCreacion,
                                             FechaModificacion = a1.FechaModificacion.HasValue ? a1.FechaModificacion : a1.FechaCreacion,
                                             ModificadoPor = !string.IsNullOrEmpty(a1.ModificadoPor) ? a1.ModificadoPor : a1.CreadoPor,
                                             ClaveTecnologia = a1.TipoId == (int)ETipoRelacion.Tecnologia ? t.ClaveTecnologia : c.Nombre
                                         } into grp
                                         select new RelacionDTO()
                                         {
                                             RelacionId = grp.Key.RelacionId,
                                             CodigoAPT = grp.Key.CodigoAPT,
                                             AplicacionStr = grp.Key.Nombre,
                                             EquipoId = grp.Key.EquipoId,
                                             TipoId = grp.Key.TipoId,
                                             AmbienteId = grp.Key.AmbienteId,
                                             AmbienteStr = grp.Key.DetalleAmbiente,
                                             EstadoId = grp.Key.EstadoId,
                                             UsuarioCreacion = grp.Key.CreadoPor,
                                             FechaCreacion = grp.Key.FechaCreacion,
                                             FechaModificacion = grp.Key.FechaModificacion.HasValue ? grp.Key.FechaModificacion : grp.Key.FechaCreacion,
                                             UsuarioModificacion = !string.IsNullOrEmpty(grp.Key.ModificadoPor) ? grp.Key.ModificadoPor : grp.Key.CreadoPor,
                                             EquipoTecnologiaStr = grp.Key.TipoId == (int)ETipoRelacion.Equipo ? grp.Key.EquipoNombre : grp.Key.ClaveTecnologia//grp.Key.TipoId == (int)ETipoRelacion.Tecnologia ? grp.Key.ClaveTecnologia : grp.Key.Nombre
                                         });

                        resultado = resultado.OrderBy(sortName + " " + sortOrder);
                        totalRows = resultado.Count();
                        registros = resultado.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();


                        //foreach (var item in registros)
                        //{
                        //    RelacionDetalleDTO relacionDetalle = null;

                        //    if (item.TipoId == (int)ETipoRelacion.Equipo)
                        //    {
                        //        relacionDetalle = (from a in ctx.Relacion
                        //                           join c in ctx.Equipo on a.EquipoId equals c.EquipoId
                        //                           where c.FlagActivo && a.RelacionId == item.RelacionId
                        //                           select new RelacionDetalleDTO()
                        //                           {
                        //                               TecnologiaStr = c.Nombre
                        //                           }).FirstOrDefault();

                        //    }
                        //    if (relacionDetalle != null)
                        //    {
                        //        item.EquipoTecnologiaStr = relacionDetalle.TecnologiaStr;
                        //    }
                        //}


                        return registros;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorRelacionDTO
                    , "Error en el metodo: List<RelacionDTO> GetRelacion(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, string username, int? tipoId, int? estadoId, int? dominioId, int? subdominioId, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorRelacionDTO
                    , "Error en el metodo: List<RelacionDTO> GetRelacion(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, string username, int? tipoId, int? estadoId, int? dominioId, int? subdominioId, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<RelacionDTO> GetRelacionSP(string codigoAPT
            , string componente
            , int pageNumber
            , int pageSize
            , string sortName
            , string sortOrder
            , string username
            , int? tipoId
            , int? estadoId
            , int perfil
            , out int totalRows)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<RelacionDTO>();
                totalRows = 0;
                tipoId = tipoId.HasValue ? tipoId : -1;
                estadoId = estadoId.HasValue ? estadoId : -2;

                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[CVT].[USP_VerRelacionesAplicacion]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@aplicacion", codigoAPT));
                        comando.Parameters.Add(new SqlParameter("@componente", componente));
                        comando.Parameters.Add(new SqlParameter("@tipo", tipoId));
                        comando.Parameters.Add(new SqlParameter("@estado", estadoId));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pageNumber));
                        comando.Parameters.Add(new SqlParameter("@OrderBy", sortName));
                        comando.Parameters.Add(new SqlParameter("@OrderByDirection", sortOrder));
                        comando.Parameters.Add(new SqlParameter("@usuario", username));
                        comando.Parameters.Add(new SqlParameter("@perfil", perfil));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new RelacionDTO()
                            {
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),
                                EquipoId = reader.IsDBNull(reader.GetOrdinal("EquipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EquipoId")),
                                EstadoId = reader.IsDBNull(reader.GetOrdinal("EstadoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EstadoId")),
                                RelacionId = reader.IsDBNull(reader.GetOrdinal("RelacionId")) ? 0 : reader.GetInt64(reader.GetOrdinal("RelacionId")),
                                TipoId = reader.IsDBNull(reader.GetOrdinal("TipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TipoId")),
                                UsuarioCreacion = reader.IsDBNull(reader.GetOrdinal("CreadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("CreadoPor")),
                                FechaCreacion = reader.IsDBNull(reader.GetOrdinal("FechaCreacion")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("FechaCreacion")),
                                UsuarioModificacion = reader.IsDBNull(reader.GetOrdinal("ModificadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("ModificadoPor")),
                                FechaModificacion = reader.IsDBNull(reader.GetOrdinal("FechaModificacion")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("FechaModificacion")),
                                AplicacionStr = reader.IsDBNull(reader.GetOrdinal("Aplicacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("Aplicacion")),
                                Componente = reader.IsDBNull(reader.GetOrdinal("Componente")) ? string.Empty : reader.GetString(reader.GetOrdinal("Componente")),
                                AmbienteStr = reader.IsDBNull(reader.GetOrdinal("Ambiente")) ? string.Empty : reader.GetString(reader.GetOrdinal("Ambiente")),
                                CodigoAPT = reader.IsDBNull(reader.GetOrdinal("CodigoAPT")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAPT")),
                                Aprobar = reader.IsDBNull(reader.GetOrdinal("Aprobar")) ? "false" : reader.GetString(reader.GetOrdinal("Aprobar"))
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
                    , "Error en el metodo: GetAplicacion()"
                    , new object[] { null });
            }
        }

        public override RelacionDTO GetRelacionById(long Id)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var entidad = (from u in ctx.Relacion
                                       join a in ctx.Aplicacion on u.CodigoAPT equals a.CodigoAPT
                                       join e in ctx.Equipo on u.EquipoId equals e.EquipoId into lj1
                                       from e in lj1.DefaultIfEmpty()
                                       join f in ctx.Ambiente on u.AmbienteId equals f.AmbienteId into lj2
                                       from f in lj2.DefaultIfEmpty()
                                       where u.RelacionId == Id && u.FlagActivo && a.FlagActivo //&& e.FlagActivo
                                                                                                //&& u.TipoId == (int)ETipoRelacion.Equipo ? e.FlagActivo : false 
                                       select new RelacionDTO()
                                       {
                                           RelacionId = Id,
                                           CodigoAPT = u.CodigoAPT,
                                           AplicacionStr = u.CodigoAPT + " - " + a.Nombre,
                                           TipoId = u.TipoId,
                                           AmbienteId = u.AmbienteId,
                                           AmbienteStr = (f != null) ? f.DetalleAmbiente : " - ",
                                           EquipoId = u.EquipoId,
                                           EquipoStr = e.Nombre,
                                           EstadoId = u.EstadoId,
                                           FechaModificacion = u.FechaModificacion.HasValue ? u.FechaModificacion : u.FechaCreacion,
                                           UsuarioModificacion = !string.IsNullOrEmpty(u.ModificadoPor) ? u.ModificadoPor : u.CreadoPor,
                                           FlagRelacionAplicacion = u.FlagRelacionAplicacion,
                                           Suscripcion = e != null ? e.Suscripcion : "",
                                           TipoEquipoId = e != null ? e.TipoEquipoId.Value : -1,
                                           Funcion = u.Funcion
                                           // Relación App-Componente
                                           ,
                                           TipoRelacionComponente = u.TipoRelacionComponente
                                       }).FirstOrDefault();

                        if (entidad != null)
                        {
                            var listaRelacionDetalle = (from a1 in ctx.Relacion
                                                        join a2 in ctx.RelacionDetalle on a1.RelacionId equals a2.RelacionId
                                                        join a3 in ctx.Tecnologia on a2.TecnologiaId equals a3.TecnologiaId into lj1
                                                        from a3 in lj1.DefaultIfEmpty()
                                                        join pr in ctx.Producto on a3.ProductoId equals pr.ProductoId into prd
                                                        from pr in prd.DefaultIfEmpty()
                                                        join x in ctx.Tipo on a3.TipoTecnologia equals x.TipoId into xl1
                                                        from x in xl1.DefaultIfEmpty()
                                                        join y in ctx.Subdominio on pr.SubDominioId equals y.SubdominioId into xl2
                                                        from y in xl2.DefaultIfEmpty()
                                                        join z in ctx.Dominio on y.DominioId equals z.DominioId into xl3
                                                        from z in xl3.DefaultIfEmpty()
                                                        join a4 in ctx.Relevancia on a2.RelevanciaId equals a4.RelevanciaId
                                                        join a5 in ctx.Aplicacion on a2.CodigoAPTVinculo equals a5.CodigoAPT into lj2
                                                        from a5 in lj2.DefaultIfEmpty()
                                                        where a1.RelacionId == Id && a1.FlagActivo && a2.FlagActivo //&& a4.FlagActivo.Value
                                                        select new RelacionDetalleDTO()
                                                        {
                                                            RelacionDetalleId = a2.RelacionDetalleId,
                                                            RelacionId = a1.RelacionId,
                                                            TecnologiaId = a3 != null ? a2.TecnologiaId.Value : -1,
                                                            TecnologiaStr = a3 != null ? a3.ClaveTecnologia : "",
                                                            SubdominioId = a3 != null ? pr.SubDominioId : (int?)null,
                                                            Subdominio = a3 != null ? y.Nombre : "",
                                                            TipoTecnologia = a3 != null ? x.Nombre : "",
                                                            Dominio = a3 != null ? z.Nombre : "",
                                                            FechaFinSoporte = a3 != null ? a3.FechaFinSoporte : null,
                                                            RelevanciaId = a2.RelevanciaId,
                                                            RelevanciaStr = a4.Nombre,
                                                            Componente = a2.Componente,
                                                            FechaModificacion = a2.FechaModificacion.HasValue ? a2.FechaModificacion : a2.FechaCreacion,
                                                            UsuarioModificacion = !string.IsNullOrEmpty(a2.ModificadoPor) ? a2.ModificadoPor : a2.CreadoPor,
                                                            FlagSeleccionado = true,
                                                            CodigoAPTVinculo = a5 != null ? a2.CodigoAPTVinculo : "",
                                                            CodigoAPTVinculoStr = a5 != null ? a5.CodigoAPT + " - " + a5.Nombre : "",
                                                            DetalleVinculo = a2.DetalleVinculo
                                                        }).ToList();

                            entidad.ListRelacionDetalle = listaRelacionDetalle;
                            if(entidad.EquipoId != null)
                            {
                                var instancias = new List<string>();
                                instancias = GetInstanciasBD_Relacion((int)entidad.EquipoId);
                                entidad.InstanciasBD = instancias;
                            }
                        }

                        return entidad;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorRelacionDTO
                    , "Error en el metodo: RelacionDTO GetRelacionById(int Id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorRelacionDTO
                    , "Error en el metodo: RelacionDTO GetRelacionById(int Id)"
                    , new object[] { null });
            }
        }
        public override List<string> GetInstanciasBD_Relacion(int equipoId)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;

                var lista = new List<string>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[usp_EquipoInstanciaBD_RelacionDetalle]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@equipoId", equipoId));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var ins = reader.IsDBNull(reader.GetOrdinal("InstanciaBD")) ? string.Empty : reader.GetString(reader.GetOrdinal("InstanciaBD"));
                            lista.Add(ins);
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
                    , "Error en el metodo: GetInstanciasBD_Relacion()"
                    , new object[] { null });
            }
        }

        public override List<RelacionDetalleDTO> GetRelacionDetalle(long Id)
        {
            try
            {
                List<RelacionDetalleDTO> listaRelacionDetalle = null;
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        listaRelacionDetalle = (from a1 in ctx.RelacionDetalle
                                                join a2 in ctx.Tecnologia on new { TecnologiaId = a1.TecnologiaId.Value, Activo = true } equals new { a2.TecnologiaId, a2.Activo } into lj1
                                                from a2 in lj1.DefaultIfEmpty()
                                                join a3 in ctx.Relevancia on a1.RelevanciaId equals a3.RelevanciaId
                                                join a4 in ctx.Aplicacion on new { CodigoAPT = a1.CodigoAPTVinculo, FlagActivo = true } equals new { a4.CodigoAPT, a4.FlagActivo } into lj2
                                                from a4 in lj2.DefaultIfEmpty()
                                                where a1.RelacionId == Id && a1.FlagActivo
                                                //&& (a2.TecnologiaId == 0 || a2.Activo)
                                                orderby a2.ClaveTecnologia ascending
                                                select new RelacionDetalleDTO()
                                                {
                                                    RelacionDetalleId = a1.RelacionDetalleId,
                                                    RelacionId = a1.RelacionId,
                                                    //TecnologiaId = (a2 != null) ? a2.TecnologiaId : ,
                                                    TecnologiaStr = (a2 != null) ? a2.ClaveTecnologia : a4.CodigoAPT + " - " + a4.Nombre,
                                                    RelevanciaId = a1.RelevanciaId,
                                                    RelevanciaStr = a3.Nombre,
                                                    FechaModificacion = a1.FechaModificacion.HasValue ? a1.FechaModificacion : a1.FechaCreacion,
                                                    UsuarioModificacion = !string.IsNullOrEmpty(a1.ModificadoPor) ? a1.ModificadoPor : a1.CreadoPor,
                                                    FlagSeleccionado = true
                                                }).ToList();
                    }
                    return listaRelacionDetalle;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorRelacionDTO
                    , "Error en el metodo: List<RelacionDetalleDTO> GetRelacionDetalle(int Id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorRelacionDTO
                    , "Error en el metodo: List<RelacionDetalleDTO> GetRelacionDetalle(int Id)"
                    , new object[] { null });
            }
        }

        public override bool VerificarEliminada(string Id)
        {
            bool flag = false;
            try
            {
                List<RelacionDetalleDTO> listaRelacionDetalle = null;
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx
                        = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var app = ctx.Application.Where(x => x.applicationId == Id).Select(x => x.AppId).FirstOrDefault();

                        var solicitudesAnterioresVigentes = ctx.Solicitud.Where(x => x.AplicacionId == app && (x.EstadoSolicitud == 2 || x.EstadoSolicitud == 4 || x.EstadoSolicitud == 6) && x.TipoSolicitud == 3).Count();

                        if (solicitudesAnterioresVigentes != 0) { flag = true; }
                        else { flag = false; }

                    }
                    return flag;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorRelacionDTO
                    , "Error en el metodo: List<RelacionDetalleDTO> GetRelacionDetalle(int Id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorRelacionDTO
                    , "Error en el metodo: List<RelacionDetalleDTO> GetRelacionDetalle(int Id)"
                    , new object[] { null });
            }
        }

        public override bool ExisteRelacionTipoTecnologia(long id, string codigoAPT, int tecnologiaId, int? equipoId)
        {
            try
            {
                bool? estado = false;
                var CURRENT_DATE = DateTime.Now;
                var C_Day = CURRENT_DATE.Day;
                var C_Month = CURRENT_DATE.Month;
                var C_Year = CURRENT_DATE.Year;

                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        estado = (from a1 in ctx.Relacion
                                  join a2 in ctx.RelacionDetalle on a1.RelacionId equals a2.RelacionId
                                  where a1.FlagActivo && a2.FlagActivo && a1.TipoId == (int)ETipoRelacion.Tecnologia && a1.RelacionId != id
                                  && a1.CodigoAPT == codigoAPT && a2.TecnologiaId == tecnologiaId
                                  //&& (a1.EstadoId == (int)EEstadoRelacion.Aprobado || a1.EstadoId == (int)EEstadoRelacion.Pendiente)
                                  && a1.DiaRegistro == C_Day
                                  && a1.MesRegistro == C_Month
                                  && a1.AnioRegistro == C_Year
                                  select true).FirstOrDefault();
                    }
                }
                return estado == true;
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorRelacionDTO
                    , "Error en el metodo: bool ExisteRelacionTipoTecnologia(int id, string codigoAPT, int tecnologiaId, int? equipoId)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorRelacionDTO
                    , "Error en el metodo: bool ExisteRelacionTipoTecnologia(int id, string codigoAPT, int tecnologiaId, int? equipoId)"
                    , new object[] { null });
            }
        }

        public override bool ExisteRelacionTipoEquipo(long id, string codigoAPT, int ambienteId, int equipoId)
        {
            try
            {
                bool? estado = false;
                var CURRENT_DATE = DateTime.Now;
                var C_Day = CURRENT_DATE.Day;
                var C_Month = CURRENT_DATE.Month;
                var C_Year = CURRENT_DATE.Year;

                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        estado = (from a1 in ctx.Relacion
                                  where a1.FlagActivo && a1.TipoId == (int)ETipoRelacion.Equipo && a1.RelacionId != id
                                  && a1.CodigoAPT == codigoAPT && a1.AmbienteId == ambienteId && a1.EquipoId == equipoId
                                  //&& (a1.EstadoId == (int)EEstadoRelacion.Aprobado || a1.EstadoId == (int)EEstadoRelacion.Pendiente)
                                  && a1.DiaRegistro == C_Day
                                  && a1.MesRegistro == C_Month
                                  && a1.AnioRegistro == C_Year
                                  select true).FirstOrDefault();
                    }
                }
                return estado == true;
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorRelacionDTO
                    , "Error en el metodo: bool ExisteRelacionTipoEquipo(int id, string codigoAPT, int ambienteId, int equipoId)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorRelacionDTO
                    , "Error en el metodo: bool ExisteRelacionTipoEquipo(int id, string codigoAPT, int ambienteId, int equipoId)"
                    , new object[] { null });
            }
        }

        public override bool ExisteRelacionAplicacionEquipo(string codigoAPT, int equipoId)
        {
            try
            {
                bool? estado = false;
                var CURRENT_DATE = DateTime.Now;
                var C_Day = CURRENT_DATE.Day;
                var C_Month = CURRENT_DATE.Month;
                var C_Year = CURRENT_DATE.Year;

                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        estado = (from a1 in ctx.Relacion
                                  where a1.FlagActivo
                                  && a1.CodigoAPT == codigoAPT && a1.EquipoId == equipoId
                                  && a1.DiaRegistro == C_Day
                                  && a1.MesRegistro == C_Month
                                  && a1.AnioRegistro == C_Year
                                  select true).FirstOrDefault();
                    }
                }
                return estado == true;
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorRelacionDTO
                    , "Error en el metodo: bool ExisteRelacionAplicacionEquipo(string codigoAPT, int equipoId)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorRelacionDTO
                    , "Error en el metodo: bool ExisteRelacionAplicacionEquipo(string codigoAPT, int equipoId)"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocomplete> GetEquipo()
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var entidad = (from u in ctx.Equipo
                                       where u.FlagActivo
                                       orderby u.Nombre
                                       select new CustomAutocomplete()
                                       {
                                           Id = u.EquipoId.ToString(),
                                           Descripcion = u.Nombre,
                                           value = u.Nombre
                                       });

                        return entidad.ToList();
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorRelacionDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetEquipo()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorRelacionDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetEquipo()"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocomplete> GetEquipo_Tecnologia()
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var entidad = (from a1 in ctx.EquipoTecnologia
                                       join a2 in ctx.Tecnologia on a1.TecnologiaId equals a2.TecnologiaId
                                       join a3 in ctx.Equipo on a1.EquipoId equals a3.EquipoId
                                       where a1.FlagActivo && a2.Activo && a3.FlagActivo
                                       orderby a2.Nombre
                                       select new CustomAutocomplete()
                                       {
                                           Id = a1.TecnologiaId.ToString(),
                                           Descripcion = a2.Nombre,
                                           TipoId = a3.EquipoId.ToString(),
                                           TipoDescripcion = a3.Nombre
                                       }).ToList();

                        return entidad;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorRelacionDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetServidor_Tecnologia()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorRelacionDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetServidor_Tecnologia()"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocompleteRelacion> GetEquipoTecnologiaByEqId(int equipoId, string subdominioIds = null)
        {
            var CURRENT_DATE = DateTime.Now;
            var dia = CURRENT_DATE.Day; //5
            var mes = CURRENT_DATE.Month; //2
            var anio = CURRENT_DATE.Year; //2020

            var subdominioIntIds = new List<int>();
            subdominioIntIds.Add(36); //Add SO

            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        if (!string.IsNullOrEmpty(subdominioIds))
                        {
                            var subdominios = subdominioIds.Split('|').Select(x => int.Parse(x)).ToList();
                            subdominioIntIds.AddRange(subdominios);
                        }

                        var parametroSO = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("SUBDOMINIO_SISTEMA_OPERATIVO").Valor;
                        var subdominioId = int.Parse(parametroSO);

                        var entidad = (from a1 in ctx.EquipoTecnologia
                                       join a2 in ctx.Tecnologia on a1.TecnologiaId equals a2.TecnologiaId
                                       join a3 in ctx.Equipo on a1.EquipoId equals a3.EquipoId
                                       join a4 in ctx.Subdominio on a2.SubdominioId equals a4.SubdominioId
                                       where a1.FlagActivo && a2.Activo && a3.FlagActivo && a4.Activo
                                       && a1.EquipoId == equipoId
                                       && a1.DiaRegistro == dia
                                       && a1.MesRegistro == mes
                                       && a1.AnioRegistro == anio
                                       && (string.IsNullOrEmpty(subdominioIds) || subdominioIntIds.Contains(a4.SubdominioId))
                                       orderby a2.Nombre
                                       select new CustomAutocompleteRelacion()
                                       {
                                           Id = a1.TecnologiaId.ToString(),
                                           Descripcion = a2.ClaveTecnologia,
                                           TipoId = a3.EquipoId.ToString(),
                                           TipoDescripcion = a3.Nombre,
                                           FlagSO = a2.SubdominioId == subdominioId,
                                           FlagComponente = (a4.SubdominioId == 68 || a4.SubdominioId == 69)
                                       }).ToList();

                        return entidad;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorRelacionDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetEquipoTecnologiaByEqId(int equipoId)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorRelacionDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetEquipoTecnologiaByEqId(int equipoId)"
                    , new object[] { null });
            }
        }

        public override List<RelacionDetalleDTO> GetAplicacionRelacionadaByEquipoId(int equipoId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {


                        int DIA_REGISTRO = DateTime.Now.Day;
                        int MES_REGISTRO = DateTime.Now.Month;
                        int ANIO_REGISTRO = DateTime.Now.Year;
                        var registros = (from r in ctx.Relacion
                                         join a in ctx.Ambiente on r.AmbienteId equals a.AmbienteId
                                         join ap in ctx.Aplicacion on new { r.CodigoAPT, FlagActivo = true } equals new { ap.CodigoAPT, FlagActivo = ap.FlagActivo }
                                         join u3 in ctx.Application on ap.CodigoAPT equals u3.applicationId
                                         where r.EquipoId == equipoId && r.FlagActivo
                                         && (r.EstadoId == 0 || r.EstadoId == 2)
                                         && r.DiaRegistro == DIA_REGISTRO
                                         && r.MesRegistro == MES_REGISTRO
                                         && r.AnioRegistro == ANIO_REGISTRO
                                         group new { r, a, ap, u3 } by new { r.CodigoAPT, ap.Nombre, r.TipoId, a.DetalleAmbiente, r.EstadoId, u3.AppId } into grp
                                         select new RelacionDetalleDTO()
                                         {
                                             RelacionDetalleId = 0,
                                             Relacion = new RelacionDTO()
                                             {
                                                 CodigoAPT = grp.Key.CodigoAPT,
                                                 AplicacionStr = grp.Key.Nombre,
                                                 TipoId = grp.Key.TipoId,
                                                 AmbienteStr = grp.Key.DetalleAmbiente,
                                                 EstadoId = grp.Key.EstadoId,
                                                 ApplicationId = grp.Key.AppId,
                                                 TipoEquipoId = -1
                                             },
                                             //TecnologiaStr = grp.Key.ClaveTecnologia,
                                             //TecnologiaId = grp.Key.TecnologiaId
                                         })
                                         .Distinct()
                                         .OrderBy(sortName + " " + sortOrder);

                        var pci = (from x in ctx.ApplicationPCI
                                   where x.FlagActivo == true
                                   select new AplicacionPCISTODTO() { TipoPCIId = x.TipoPCIId, ApplicationId = x.ApplicationId }).ToList();

                        totalRows = registros.Count();
                        var resultado = registros.Skip((pageNumber - 1) * pageSize).Take(pageSize);



                        var l = resultado.ToList();

                        foreach (RelacionDetalleDTO a in l)
                        {
                            var lista = "";
                            int flag = 0;
                            var pci2 = (from x in pci
                                        where x.ApplicationId == a.Relacion.ApplicationId
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

                            a.Relacion.ListaPCI = lista;
                        }

                        return l;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorRelacionDTO
                    , "Error en el metodo: List<RelacionDTO> GetAplicacionRelacionadaByEquipoId(int equipoId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorRelacionDTO
                    , "Error en el metodo: List<RelacionDTO> GetAplicacionRelacionadaByEquipoId(int equipoId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }


        public override List<CertificadoDigitalDTO> GetCertificadoDigitalByEquipoId(int equipoId, int pageNumber, int pageSize, out int totalRows)
        {
            totalRows = 0;
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;

                var lista = new List<CertificadoDigitalDTO>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[CVT].[usp_certificado_digital_detalleEquipo]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@equipoId", equipoId));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pageNumber));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new CertificadoDigitalDTO()
                            {
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),
                                EquipoId = reader.IsDBNull(reader.GetOrdinal("EquipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EquipoId")),
                                FechaCalculoBase = reader.IsDBNull(reader.GetOrdinal("VencimientoLicencia")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("VencimientoLicencia")),
                                Nombre = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                                TipoCertificado = reader.IsDBNull(reader.GetOrdinal("TipoCertificadoCD")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoCertificadoCD")),
                                DescripcionTipoCertificado = reader.IsDBNull(reader.GetOrdinal("DescripcionTipoCD")) ? string.Empty : reader.GetString(reader.GetOrdinal("DescripcionTipoCD")),
                                FechaCreacionCertificado = reader.IsDBNull(reader.GetOrdinal("FechaCreadoCD")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaCreadoCD")),
                                FlagActivo = reader.IsDBNull(reader.GetOrdinal("FlagActivo")) ? false : reader.GetBoolean(reader.GetOrdinal("FlagActivo")),
                                // FlagActivo = reader.IsDBNull(reader.GetOrdinal("FlagActivo")) ? 0 : reader.GetInt32(reader.GetOrdinal("FlagActivo")),

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
                    , "Error en el metodo: GetCertificadosDigitalesByCodigoAPT()"
                    , new object[] { null });
            }
        }
        public override List<CustomAutocomplete> GetEquipoByAplicacion(string codigoAPT)
        {
            try
            {
                var dia = DateTime.Now.Day;
                var mes = DateTime.Now.Month;
                var anio = DateTime.Now.Year;

                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        ctx.Database.CommandTimeout = 0;

                        var entidad = (from u in ctx.Relacion
                                       join e in ctx.Equipo on u.EquipoId equals e.EquipoId
                                       where u.FlagActivo && e.FlagActivo
                                       && u.CodigoAPT == codigoAPT
                                       && u.DiaRegistro == dia
                                       && u.MesRegistro == mes
                                       && u.AnioRegistro == anio
                                       //&& (u.Nombre).ToUpper().Contains(filtro.ToUpper())
                                       orderby u.CodigoAPT
                                       select new CustomAutocomplete()
                                       {
                                           Id = u.EquipoId.ToString(),
                                           Descripcion = e.Nombre,
                                           value = e.Nombre
                                       }).Distinct().ToList(); //ToList();

                        return entidad;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetEquipoByFiltro(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetEquipoByFiltro(string filtro)"
                    , new object[] { null });
            }
        }

        public override int RegistrarAplicacionVinculada(AplicacionVinculadaDTO objeto)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = new AplicacionVinculada()
                    {
                        Activo = true,
                        UsuarioCreacion = objeto.UsuarioCreacion,
                        FechaCreacion = DateTime.Now,
                        CodigoAPT = objeto.CodigoAPT,
                        VinculoCodigoAPT = objeto.VinculoCodigoAPT,
                        EquipoId = objeto.EquipoId,
                        DetalleVinculo = objeto.DetalleVinculo,
                        AplicacionVinculadaId = objeto.Id
                    };
                    ctx.AplicacionVinculada.Add(entidad);
                    ctx.SaveChanges();

                    return entidad.AplicacionVinculadaId;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: RegistrarAplicacionVinculada(AplicacionVinculadaDTO objeto)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo:  RegistrarAplicacionVinculada(AplicacionVinculadaDTO objeto)"
                    , new object[] { null });
            }
        }

        public override List<AplicacionVinculadaDTO> GetAplicacionVinculada(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                totalRows = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.AplicacionVinculada
                                     join e in ctx.Equipo on u.EquipoId equals e.EquipoId
                                     join a1 in ctx.Aplicacion on u.CodigoAPT equals a1.CodigoAPT
                                     join a2 in ctx.Aplicacion on u.VinculoCodigoAPT equals a2.CodigoAPT
                                     where (e.Nombre.ToUpper().Contains(filtro.ToUpper())
                                     || a1.Nombre.ToUpper().Contains(filtro.ToUpper())
                                     || a2.Nombre.ToUpper().Contains(filtro.ToUpper())
                                     || string.IsNullOrEmpty(filtro)
                                     || u.DetalleVinculo.ToUpper().Contains(filtro.ToUpper()))
                                     && u.Activo && e.FlagActivo && a1.FlagActivo && a2.FlagActivo
                                     select new AplicacionVinculadaDTO()
                                     {
                                         Id = u.AplicacionVinculadaId,
                                         CodigoAPT = a1.CodigoAPT,
                                         VinculoCodigoAPT = a2.CodigoAPT,
                                         Activo = u.Activo,
                                         CodigoAPTStr = a1.Nombre,
                                         VinculoCodigoAPTStr = a2.Nombre,
                                         EquipoStr = e.Nombre,
                                         DetalleVinculo = u.DetalleVinculo,
                                         UsuarioCreacion = u.UsuarioCreacion,
                                         FechaCreacion = u.FechaCreacion,
                                         FechaModificacion = u.FechaModificacion,
                                         UsuarioModificacion = u.UsuarioModificacion
                                     }).OrderBy(sortName + " " + sortOrder);

                    totalRows = registros.Count();
                    var resultado = registros.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                    return resultado;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: List<TipoDTO> GetTipo(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: List<TipoDTO> GetTipo(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<VistaRelacionDto> GetVistaRelacion(
              string aplicacion
            , string equipo
            , string ambienteIds
            , string jefe
            , string gestionado
            , string PCIS
            , string RelacionAplicacion
            , string estadoId
            , string TipoRelacion
            , int pageNumber
            , int pageSize
            , string sortName
            , string sortOrder
            , out int totalRows)
        {
            var dia = DateTime.Now.Day;
            var mes = DateTime.Now.Month;
            var anio = DateTime.Now.Year;

            var lista = new List<VistaRelacionDto>();
            var _RelacionAplicacion = string.Empty;
            if (!string.IsNullOrEmpty(RelacionAplicacion))
            {
                if (RelacionAplicacion.Equals("1"))
                    _RelacionAplicacion = RelacionAplicacion;
                else
                    _RelacionAplicacion = "0";
            }

            try
            {
                totalRows = 0;
                var cadenaConexion = Constantes.CadenaConexion;
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_GetList_VistaRelacion]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@pAplicacion", string.IsNullOrEmpty(aplicacion) ? "" : aplicacion));
                        comando.Parameters.Add(new SqlParameter("@pEquipo", string.IsNullOrEmpty(equipo) ? "" : equipo));
                        comando.Parameters.Add(new SqlParameter("@pJefe", string.IsNullOrEmpty(jefe) ? "" : jefe));
                        comando.Parameters.Add(new SqlParameter("@pGestionado", string.IsNullOrEmpty(gestionado) ? "" : gestionado));
                        comando.Parameters.Add(new SqlParameter("@pAmbiente", string.IsNullOrEmpty(ambienteIds) ? "" : ambienteIds));
                        comando.Parameters.Add(new SqlParameter("@pAplicacionPCI", string.IsNullOrEmpty(PCIS) ? "" : PCIS));
                        comando.Parameters.Add(new SqlParameter("@pRelacionAplicacion", _RelacionAplicacion));
                        comando.Parameters.Add(new SqlParameter("@pEstado", string.IsNullOrEmpty(estadoId) ? "" : estadoId));
                        comando.Parameters.Add(new SqlParameter("@pTipoRelacion", string.IsNullOrEmpty(TipoRelacion) ? "" : TipoRelacion));
                        comando.Parameters.Add(new SqlParameter("@pDia", dia));
                        comando.Parameters.Add(new SqlParameter("@pMes", mes));
                        comando.Parameters.Add(new SqlParameter("@pAnio", anio));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pageNumber));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pageSize));
                        comando.Parameters.Add(new SqlParameter("@sortName", sortName));
                        comando.Parameters.Add(new SqlParameter("@sortOrder", sortOrder));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            lista.Add(new VistaRelacionDto
                            {
                                Aplicacion = reader.GetData<string>("Aplicacion"),
                                CodigoApt = reader.GetData<string>("CodigoApt"),
                                DetalleAmbiente = reader.GetData<string>("DetalleAmbiente"),
                                DetalleCriticidad = reader.GetData<string>("DetalleCriticidad"),
                                Equipo = reader.GetData<string>("Equipo"),
                                GestionadoPor = reader.GetData<string>("GestionadoPor"),
                                JefeEquipo = reader.GetData<string>("JefeEquipo"),
                                EstadoId = reader.IsDBNull(reader.GetOrdinal("EstadoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EstadoId")),
                                TipoActivoTI = reader.GetData<string>("TipoActivoTI"),
                                SistemaOperativo = reader.GetData<string>("SistemaOperativo"),
                                EquipoId = reader.IsDBNull(reader.GetOrdinal("EquipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EquipoId")),
                                ApplicationId = reader.IsDBNull(reader.GetOrdinal("ApplicationId")) ? 0 : reader.GetInt32(reader.GetOrdinal("ApplicationId")),
                                TipoEquipoId = reader.IsDBNull(reader.GetOrdinal("TipoEquipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TipoEquipoId")),
                                RelacionAplicacion = reader.IsDBNull(reader.GetOrdinal("RelacionAplicacion")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("RelacionAplicacion")),
                                ListaPCI = reader.GetData<string>("ListaPCI"),
                                TipoRelacion = reader.GetData<string>("TipoRelacion"),
                                TotalRows = reader.GetData<int>("TotalRows")
                            });
                        }
                        reader.Close();
                    }

                    if (lista.Count > 0)
                        totalRows = lista[0].TotalRows;


                    return lista;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorRelacionDTO
                    , "Error en el metodo: List<TipoDTO> GetVistaRelacion()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorRelacionDTO
                    , "Error en el metodo: List<TipoDTO> GetVistaRelacion()"
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
        public override List<RelacionDTO> GetRelacionXTecnologiaId(int tecnologiaId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                totalRows = 0;
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        DateTime fechaActual = DateTime.Now.Date;
                        var registros = (from a1 in ctx.Relacion
                                         join b in ctx.Aplicacion on a1.CodigoAPT equals b.CodigoAPT
                                         join c in ctx.Equipo on a1.EquipoId equals c.EquipoId into c2
                                         from c in c2.DefaultIfEmpty()
                                         join amb in ctx.Ambiente on a1.AmbienteId equals amb.AmbienteId into c3
                                         from amb in c3.DefaultIfEmpty()
                                         join rd in ctx.RelacionDetalle on a1.RelacionId equals rd.RelacionId into c4
                                         from rd in c4.DefaultIfEmpty()
                                         where a1.FlagActivo && a1.DiaRegistro.Value == fechaActual.Day && a1.MesRegistro.Value == fechaActual.Month && a1.AnioRegistro.Value == fechaActual.Year
                                         && b.FlagActivo && b.FlagRelacionar.Value && rd.FlagActivo && rd.TecnologiaId == tecnologiaId
                                         group new { a1, b, c, amb } by new
                                         {
                                             a1.RelacionId,
                                             a1.CodigoAPT,
                                             b.Nombre,
                                             c.EquipoId,
                                             a1.TipoId,
                                             a1.AmbienteId,
                                             amb.DetalleAmbiente,
                                             a1.EstadoId,
                                             a1.CreadoPor,
                                             a1.FechaCreacion,
                                             FechaModificacion = a1.FechaModificacion.HasValue ? a1.FechaModificacion : a1.FechaCreacion,
                                             ModificadoPor = !string.IsNullOrEmpty(a1.ModificadoPor) ? a1.ModificadoPor : a1.CreadoPor
                                         } into grp
                                         select new RelacionDTO()
                                         {
                                             RelacionId = grp.Key.RelacionId,
                                             CodigoAPT = grp.Key.CodigoAPT,
                                             AplicacionStr = grp.Key.Nombre,
                                             EquipoId = grp.Key.EquipoId,
                                             TipoId = grp.Key.TipoId,
                                             AmbienteId = grp.Key.AmbienteId,
                                             AmbienteStr = grp.Key.DetalleAmbiente,
                                             EstadoId = grp.Key.EstadoId,
                                             UsuarioCreacion = grp.Key.CreadoPor,
                                             FechaCreacion = grp.Key.FechaCreacion,
                                             FechaModificacion = grp.Key.FechaModificacion.HasValue ? grp.Key.FechaModificacion : grp.Key.FechaCreacion,
                                             UsuarioModificacion = !string.IsNullOrEmpty(grp.Key.ModificadoPor) ? grp.Key.ModificadoPor : grp.Key.CreadoPor
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
                throw new CVTException(CVTExceptionIds.ErrorRelacionDTO
                    , "Error en el metodo: List<RelacionDTO> GetRelacionXTecnologiaId(int tecnologiaId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorRelacionDTO
                    , "Error en el metodo: List<RelacionDTO> GetRelacionXTecnologiaId(int tecnologiaId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<VistaRelacionDto> GetVistaRelacionConsultor(string aplicacion
            , string equipo
            , List<int> ambienteIds
            , string jefe
            , string gestionado
            , string RelacionAplicacion
            , List<int> estadoId
            , string TipoRelacion
            , int pageNumber
            , int pageSize
            , string sortName
            , string sortOrder
            , string matricula
            , out int totalRows)
        {
            var dia = DateTime.Now.Day;
            var mes = DateTime.Now.Month;
            var anio = DateTime.Now.Year;

            bool? _RelacionAplicacion = null;
            if (!string.IsNullOrEmpty(RelacionAplicacion))
                _RelacionAplicacion = RelacionAplicacion.Equals("1") ? true : false;

            int? _TipoRelacion = null;
            if (!string.IsNullOrEmpty(TipoRelacion))
                _TipoRelacion = Convert.ToInt32(TipoRelacion);

            try
            {
                totalRows = 0;
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        ctx.Database.CommandTimeout = 0;
                        var registros = (from u in ctx.Relacion
                                         join a in ctx.Aplicacion on u.CodigoAPT equals a.CodigoAPT
                                         join e in ctx.Equipo on u.EquipoId equals e.EquipoId
                                         join ex in ctx.AplicacionExpertos on a.CodigoAPT equals ex.CodigoAPT
                                         join c in ctx.Criticidad on a.CriticidadId equals c.CriticidadId
                                         join am in ctx.Ambiente on u.AmbienteId equals am.AmbienteId
                                         where u.FlagActivo && u.DiaRegistro == dia && u.MesRegistro == mes && u.AnioRegistro == anio && am.Activo
                                         && (aplicacion.ToUpper().Contains(a.CodigoAPT.ToUpper())
                                         || aplicacion.ToUpper().Contains(a.Nombre.ToUpper())
                                         || string.IsNullOrEmpty(aplicacion))
                                         && (e.Nombre.ToUpper().Contains(equipo.ToUpper()) || string.IsNullOrEmpty(equipo))
                                         && (a.JefeEquipo_ExpertoAplicacionUserIT_ProductOwner.ToUpper().Contains(jefe.ToUpper()) || string.IsNullOrEmpty(jefe))
                                         && (a.GestionadoPor.ToUpper().Contains(gestionado.ToUpper()) || string.IsNullOrEmpty(gestionado))
                                         //&& (am.AmbienteId == ambiente || ambiente == -1)
                                         && (ambienteIds.Count == 0 || ambienteIds.Contains(am.AmbienteId))
                                         && ex.FlagActivo && ex.Matricula == matricula
                                         && (_RelacionAplicacion == null || u.FlagRelacionAplicacion == _RelacionAplicacion)
                                         && (estadoId.Count == 0 || estadoId.Contains(u.EstadoId))
                                         && (_RelacionAplicacion == null || u.FlagRelacionAplicacion == _RelacionAplicacion)
                                         && (_TipoRelacion == null || u.TipoRelacionComponente == _TipoRelacion)
                                         select new VistaRelacionDto()
                                         {
                                             Aplicacion = a.Nombre,
                                             CodigoApt = a.CodigoAPT,
                                             DetalleAmbiente = am.DetalleAmbiente,
                                             DetalleCriticidad = c.DetalleCriticidad,
                                             Equipo = e.Nombre,
                                             GestionadoPor = a.GestionadoPor,
                                             JefeEquipo = a.JefeEquipo_ExpertoAplicacionUserIT_ProductOwner,
                                             EstadoId = u.EstadoId,
                                             TipoActivoTI = a.TipoActivoInformacion,
                                             SistemaOperativo = "",
                                             EquipoId = e.EquipoId,
                                             TipoEquipoId = e.TipoEquipoId,
                                             RelacionAplicacion = u.FlagRelacionAplicacion,
                                             ListaPCI = null,
                                             TipoRelacionId = u.TipoRelacionComponente,
                                         }).OrderBy(sortName + " " + sortOrder);

                        totalRows = registros.Count();

                        var resultado = registros.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                        resultado.ForEach(x => { x.TipoRelacion = x.TipoRelacionDesc; });

                        return resultado;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorRelacionDTO
                    , "Error en el metodo: List<TipoDTO> GetVistaRelacion()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorRelacionDTO
                    , "Error en el metodo: List<TipoDTO> GetVistaRelacion()"
                    , new object[] { null });
            }
        }

        public override bool ExisteRelacionTipoEquipo(long id, string codigoAPT, int ambienteId, int equipoId, out long relacionId)
        {
            try
            {
                //bool? estado = false;
                RelacionDTO estado = null;
                relacionId = 0;
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        estado = (from a1 in ctx.Relacion
                                  join a2 in ctx.RelacionDetalle on a1.RelacionId equals a2.RelacionId
                                  where a1.FlagActivo && a2.FlagActivo && a1.TipoId == (int)ETipoRelacion.Equipo && a1.RelacionId != id
                                  && a1.CodigoAPT == codigoAPT && a1.AmbienteId == ambienteId && a1.EquipoId == equipoId
                                  && (a1.EstadoId == (int)EEstadoRelacion.Aprobado || a1.EstadoId == (int)EEstadoRelacion.Pendiente)
                                  select new RelacionDTO()
                                  {
                                      RelacionId = a1.RelacionId
                                  }).FirstOrDefault();
                    }
                }
                if (estado != null)
                    relacionId = estado.RelacionId;

                return estado != null;
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorRelacionDTO
                    , "Error en el metodo: bool ExisteRelacionTipoEquipo(int id, string codigoAPT, int ambienteId, int equipoId)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorRelacionDTO
                    , "Error en el metodo: bool ExisteRelacionTipoEquipo(int id, string codigoAPT, int ambienteId, int equipoId)"
                    , new object[] { null });
            }
        }

        public RespuestaDTO ValidarObjRelacionPublic(RelacionPublicDTO entidad)
        {
            if (string.IsNullOrEmpty(entidad.Equipo) || string.IsNullOrEmpty(entidad.CodigoAPT)
                || string.IsNullOrEmpty(entidad.Relevancia) || string.IsNullOrEmpty(entidad.Ambiente)
                || string.IsNullOrEmpty(entidad.UsuarioCreacion))
            {
                return new RespuestaDTO()
                {
                    Codigo = (int)ECodigoRetorno.ErrorGeneral,
                    Mensaje = Utilitarios.GetEnumDescription2((ECodigoRetorno)((int)ECodigoRetorno.ErrorGeneral))
                };
            }
            else
            {
                //Validacion por cada campo

                //1. Equipo
                if (!string.IsNullOrEmpty(entidad.Equipo))
                {
                    var nombre = entidad.Equipo.Trim();
                    var equipoId = ServiceManager<EquipoDAO>.Provider.ValidarEquipoByNombre(nombre);
                    if (equipoId == 0)
                    {
                        return new RespuestaDTO()
                        {
                            Codigo = (int)ECodigoRetorno.EquipoNoExiste,
                            Mensaje = Utilitarios.GetEnumDescription2((ECodigoRetorno)((int)ECodigoRetorno.EquipoNoExiste))
                        };
                    }
                    else
                    {
                        entidad.EquipoId = equipoId;
                        var objEquipo = ServiceManager<EquipoDAO>.Provider.GetEquipoById(equipoId);
                        if (objEquipo.TipoEquipoId == (int)ETipoEquipo.Servidor || objEquipo.TipoEquipoId == (int)ETipoEquipo.ServidorAgencia || objEquipo.TipoEquipoId == (int)ETipoEquipo.PC)
                            entidad.TipoRelacion = (int)ETipoRelacion.Equipo;
                        else
                            entidad.TipoRelacion = (int)ETipoRelacion.ServicioNube;
                    }
                }

                //2. Relevancia
                if (!string.IsNullOrEmpty(entidad.Relevancia))
                {
                    var nombre = entidad.Relevancia.Trim();
                    var entidadId = ServiceManager<EquipoDAO>.Provider.ValidarRelevanciaByNombre(nombre);
                    if (entidadId == 0)
                    {
                        return new RespuestaDTO()
                        {
                            Codigo = (int)ECodigoRetorno.Relevancia,
                            Mensaje = Utilitarios.GetEnumDescription2((ECodigoRetorno)((int)ECodigoRetorno.Relevancia))
                        };
                    }
                    else
                        entidad.RelevanciaId = entidadId;
                }

                //3. Ambiente
                if (!string.IsNullOrEmpty(entidad.Ambiente))
                {
                    var nombre = entidad.Ambiente.Trim();
                    var entidadId = ServiceManager<EquipoDAO>.Provider.ValidarAmbienteByNombre(nombre);
                    if (entidadId == 0)
                    {
                        return new RespuestaDTO()
                        {
                            Codigo = (int)ECodigoRetorno.Ambiente,
                            Mensaje = Utilitarios.GetEnumDescription2((ECodigoRetorno)((int)ECodigoRetorno.Ambiente))
                        };
                    }
                    else
                        entidad.AmbienteId = entidadId;
                }

                //4. CodigoAPT
                if (!string.IsNullOrEmpty(entidad.CodigoAPT))
                {
                    var nombre = entidad.CodigoAPT.Trim();
                    var entidadId = ServiceManager<EquipoDAO>.Provider.ValidarCodigoAPTByNombre(nombre);
                    if (string.IsNullOrEmpty(entidadId))
                    {
                        return new RespuestaDTO()
                        {
                            Codigo = (int)ECodigoRetorno.CodigoAPT,
                            Mensaje = Utilitarios.GetEnumDescription2((ECodigoRetorno)((int)ECodigoRetorno.CodigoAPT))
                        };
                    }
                    else
                        entidad.CodigoAPT = entidadId;
                }

                var relacionExiste = ServiceManager<RelacionDAO>.Provider.ExisteRelacionTipoEquipo(entidad.CodigoAPT
                    , entidad.AmbienteId
                    , entidad.EquipoId);
                if (relacionExiste)
                {
                    return new RespuestaDTO()
                    {
                        Codigo = (int)ECodigoRetorno.RelacionExiste,
                        Mensaje = Utilitarios.GetEnumDescription2((ECodigoRetorno)((int)ECodigoRetorno.RelacionExiste))
                    };
                }
            }

            return new RespuestaDTO()
            {
                Codigo = (int)ECodigoRetorno.OKCamposCorrectos,
                Mensaje = Utilitarios.GetEnumDescription2((ECodigoRetorno)((int)ECodigoRetorno.OKCamposCorrectos))
            };
        }

        public override RespuestaDTO AddOrEditRelacionPublic(RelacionPublicDTO objRegistro)
        {
            DbContextTransaction transaction = null;
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    using (transaction = ctx.Database.BeginTransaction())
                    {
                        //Validacion de entidad Relacion
                        var registro = ValidarObjRelacionPublic(objRegistro);
                        if (registro.Codigo == (int)ECodigoRetorno.OKCamposCorrectos)
                        {
                            DateTime FECHA_ACTUAL = DateTime.Now;
                            var entidad = new Relacion()
                            {
                                CodigoAPT = objRegistro.CodigoAPT,
                                TipoId = objRegistro.TipoRelacion,
                                AmbienteId = objRegistro.AmbienteId,
                                EquipoId = objRegistro.EquipoId,
                                EstadoId = (int)EEstadoRelacion.Aprobado,
                                FlagActivo = true,
                                FechaCreacion = FECHA_ACTUAL,
                                CreadoPor = objRegistro.UsuarioCreacion,
                                DiaRegistro = FECHA_ACTUAL.Day,
                                MesRegistro = FECHA_ACTUAL.Month,
                                AnioRegistro = FECHA_ACTUAL.Year
                            };

                            ctx.Relacion.Add(entidad);
                            ctx.SaveChanges();
                            transaction.Commit();

                            return new RespuestaDTO()
                            {
                                Codigo = (int)ECodigoRetorno.OK,
                                Mensaje = Utilitarios.GetEnumDescription2((ECodigoRetorno)((int)ECodigoRetorno.OK))
                            };
                        }
                        else
                            return registro;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                transaction.Rollback();
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorRelacionDTO
                    , "Error en el metodo: int AddOrEditRelacion(RelacionDTO objRegistro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorRelacionDTO
                    , "Error en el metodo: int AddOrEditRelacion(RelacionDTO objRegistro)"
                    , new object[] { null });
            }
        }

        public RespuestaDTO ValidarObjEquipoPublic(EquipoPublicDTO entidad)
        {
            if (string.IsNullOrEmpty(entidad.Nombre) || string.IsNullOrEmpty(entidad.CaracteristicaEquipo)
               || string.IsNullOrEmpty(entidad.Ambiente) || string.IsNullOrEmpty(entidad.DominioServidor)
               || string.IsNullOrEmpty(entidad.UsuarioCreacion) || string.IsNullOrEmpty(entidad.TipoEquipo)
               || string.IsNullOrEmpty(entidad.Tecnologia))
            {
                return new RespuestaDTO()
                {
                    Codigo = (int)ECodigoRetorno.ErrorGeneral,
                    Mensaje = Utilitarios.GetEnumDescription2((ECodigoRetorno)((int)ECodigoRetorno.ErrorGeneral))
                };
            }
            else
            {
                //Validacion por cada campo

                //1. Equipo
                if (!string.IsNullOrEmpty(entidad.Nombre))
                {
                    var nombre = entidad.Nombre.Trim();
                    var equipoId = ServiceManager<EquipoDAO>.Provider.ValidarEquipoByNombre(nombre);
                    if (equipoId != 0)
                    {
                        return new RespuestaDTO()
                        {
                            Codigo = (int)ECodigoRetorno.EquipoExiste,
                            Mensaje = Utilitarios.GetEnumDescription2((ECodigoRetorno)((int)ECodigoRetorno.EquipoExiste))
                        };
                    }
                }

                //2. Ambiente
                if (!string.IsNullOrEmpty(entidad.Ambiente))
                {
                    var nombre = entidad.Ambiente.Trim();
                    var entidadId = ServiceManager<EquipoDAO>.Provider.ValidarAmbienteByNombre(nombre);
                    if (entidadId == 0)
                    {
                        return new RespuestaDTO()
                        {
                            Codigo = (int)ECodigoRetorno.Ambiente,
                            Mensaje = Utilitarios.GetEnumDescription2((ECodigoRetorno)((int)ECodigoRetorno.Ambiente))
                        };
                    }
                    else
                        entidad.AmbienteId = entidadId;
                }

                //3. DominioServidor
                if (!string.IsNullOrEmpty(entidad.DominioServidor))
                {
                    var nombre = entidad.DominioServidor.Trim();
                    var entidadId = ServiceManager<EquipoDAO>.Provider.ValidarDominioServidorByNombre(nombre);
                    if (entidadId == 0)
                    {
                        return new RespuestaDTO()
                        {
                            Codigo = (int)ECodigoRetorno.DominioServidor,
                            Mensaje = Utilitarios.GetEnumDescription2((ECodigoRetorno)((int)ECodigoRetorno.DominioServidor))
                        };
                    }
                    else
                        entidad.DominioServidorId = entidadId;
                }

                //4. TipoEquipo
                if (!string.IsNullOrEmpty(entidad.TipoEquipo))
                {
                    var nombre = entidad.TipoEquipo.Trim();
                    var entidadId = ServiceManager<EquipoDAO>.Provider.ValidarTipoEquipoByNombre(nombre);
                    if (entidadId == 0)
                    {
                        return new RespuestaDTO()
                        {
                            Codigo = (int)ECodigoRetorno.TipoEquipo,
                            Mensaje = Utilitarios.GetEnumDescription2((ECodigoRetorno)((int)ECodigoRetorno.TipoEquipo))
                        };
                    }
                    else
                        entidad.TipoEquipoId = entidadId;
                }

                //5. Caracteristica
                if (!string.IsNullOrEmpty(entidad.CaracteristicaEquipo))
                {
                    var nombre = entidad.CaracteristicaEquipo.Trim();
                    var entidadId = ServiceManager<EquipoDAO>.Provider.ValidarCaracteristicaEquipoByNombre(nombre);
                    if (entidadId == 0)
                    {
                        return new RespuestaDTO()
                        {
                            Codigo = (int)ECodigoRetorno.Caracteristica,
                            Mensaje = Utilitarios.GetEnumDescription2((ECodigoRetorno)((int)ECodigoRetorno.Caracteristica))
                        };
                    }
                    else
                        entidad.CaracteristicaEquipoId = entidadId;
                }

                //6. Tecnologia
                if (!string.IsNullOrEmpty(entidad.Tecnologia))
                {
                    var nombre = entidad.Tecnologia.Trim();
                    var entidadId = ServiceManager<EquipoDAO>.Provider.ValidarTecnologiaByNombre(nombre);
                    if (entidadId == 0)
                    {
                        return new RespuestaDTO()
                        {
                            Codigo = (int)ECodigoRetorno.Tecnologia,
                            Mensaje = Utilitarios.GetEnumDescription2((ECodigoRetorno)((int)ECodigoRetorno.Tecnologia))
                        };
                    }
                    else
                        entidad.TecnologiaId = entidadId;
                }
            }

            return new RespuestaDTO()
            {
                Codigo = (int)ECodigoRetorno.OKCamposCorrectos,
                Mensaje = Utilitarios.GetEnumDescription2((ECodigoRetorno)((int)ECodigoRetorno.OKCamposCorrectos))
            };
        }

        public override RespuestaDTO AddOrEditEquipoPublic(EquipoPublicDTO objeto)
        {
            DbContextTransaction transaction = null;
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    using (transaction = ctx.Database.BeginTransaction())
                    {
                        //Validacion de entidad Equipo
                        var registro = ValidarObjEquipoPublic(objeto);
                        if (registro.Codigo == (int)ECodigoRetorno.OKCamposCorrectos)
                        {
                            DateTime FECHA_ACTUAL = DateTime.Now;
                            var entidad = new Equipo()
                            {
                                FlagActivo = true,
                                CreadoPor = objeto.UsuarioCreacion,
                                FechaCreacion = FECHA_ACTUAL,
                                Nombre = objeto.Nombre,
                                AmbienteId = objeto.AmbienteId,
                                DominioServidorId = objeto.DominioServidorId,
                                EquipoId = objeto.EquipoId,
                                ProcedenciaId = Guid.NewGuid().ToString(),
                                TipoEquipoId = objeto.TipoEquipoId,
                                CaracteristicaEquipo = objeto.CaracteristicaEquipoId,
                                TablaProcedenciaId = 5, //objeto.TablaProcedenciaId
                                FlagExcluirCalculo = false,
                                FlagTemporal = true
                            };
                            ctx.Equipo.Add(entidad);
                            ctx.SaveChanges();

                            var entidadET = new EquipoTecnologia()
                            {
                                EquipoId = entidad.EquipoId,
                                TecnologiaId = objeto.TecnologiaId,
                                FlagActivo = true,
                                FechaCreacion = FECHA_ACTUAL,
                                CreadoPor = objeto.UsuarioCreacion,
                                DiaRegistro = FECHA_ACTUAL.Day,
                                MesRegistro = FECHA_ACTUAL.Month,
                                AnioRegistro = FECHA_ACTUAL.Year
                            };

                            ctx.EquipoTecnologia.Add(entidadET);
                            ctx.SaveChanges();

                            transaction.Commit();

                            return new RespuestaDTO()
                            {
                                Codigo = (int)ECodigoRetorno.OK,
                                Mensaje = Utilitarios.GetEnumDescription2((ECodigoRetorno)((int)ECodigoRetorno.OK))
                            };
                        }
                        else
                            return registro;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                transaction.Rollback();
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: AddOrEditEquipoPublic(EquipoPublicDTO objeto)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: AddOrEditEquipoPublic(EquipoPublicDTO objeto)"
                    , new object[] { null });
            }
        }

        public override bool ExisteRelacionTipoEquipo(string codigoAPT, int ambienteId, int equipoId)
        {
            try
            {
                bool? estado = false;
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        estado = (from a1 in ctx.Relacion
                                  where a1.FlagActivo && a1.TipoId == (int)ETipoRelacion.Equipo
                                  && a1.CodigoAPT == codigoAPT && a1.AmbienteId == ambienteId && a1.EquipoId == equipoId
                                  && (a1.EstadoId == (int)EEstadoRelacion.Aprobado || a1.EstadoId == (int)EEstadoRelacion.Pendiente)
                                  select true).FirstOrDefault();
                    }
                }
                return estado == true;
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorRelacionDTO
                    , "Error en el metodo: bool ExisteRelacionTipoEquipo(int id, string codigoAPT, int ambienteId, int equipoId)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorRelacionDTO
                    , "Error en el metodo: bool ExisteRelacionTipoEquipo(int id, string codigoAPT, int ambienteId, int equipoId)"
                    , new object[] { null });
            }
        }

        public override string GetComentarioByRelacionId(int RelacionId)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    string retorno = string.Empty;

                    retorno = (from u in ctx.ComentarioEliminacionRelacion
                               where u.RelacionId == RelacionId && u.FlagActivo
                               orderby u.FechaCreacion descending
                               select u.Contenido).FirstOrDefault();

                    return retorno;
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

        private void ActualizarRelacion(string codigoAPT)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_ActualizarUsuarioRelacion]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@codigoApt", codigoAPT));

                        comando.ExecuteNonQuery();
                    }
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

        public override bool EliminarRelacionDetalle(RelacionDTO objRegistro)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    string CuerpoNotificacion = string.Empty;

                    var itemBD = (from u in ctx.RelacionDetalle
                                  where u.RelacionDetalleId == objRegistro.Id

                                  select u).FirstOrDefault();

                    if (itemBD != null)
                    {
                        itemBD.FechaModificacion = DateTime.Now;
                        itemBD.ModificadoPor = objRegistro.UsuarioModificacion;
                        itemBD.FlagActivo = false;

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
                throw new CVTException(CVTExceptionIds.ErrorRelacionDTO
                    , "Error en el metodo: bool EliminarRelacionDetalle(RelacionDTO objRegistro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorRelacionDTO
                    , "Error en el metodo: bool EliminarRelacionDetalle(RelacionDTO objRegistro)"
                    , new object[] { null });
            }
        }

        public override List<RelacionDTO> GetRelacionSP_2(PaginacionRelacion pag, out int totalRows)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<RelacionDTO>();
                totalRows = 0;
                pag.TipoRelacionId = pag.TipoRelacionId ?? -1;
                pag.EstadoId = pag.EstadoId ?? -2;
                pag.SubdominioIds = !string.IsNullOrEmpty(pag.SubdominioIds) ? pag.SubdominioIds : string.Empty;
                pag.Tecnologia = !string.IsNullOrEmpty(pag.Tecnologia) ? pag.Tecnologia : string.Empty;
                pag.AmbienteId = pag.AmbienteId ?? -1;
                int paramCD = Convert.ToInt32(ServiceManager<ParametroDAO>.Provider.ObtenerParametro("CERTIFICADO_DIGITAL_APLICACION_PARAMETRO_TIPOEQUIPO").Valor);

                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_VerRelacionesAplicacion_2]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@aplicacion", pag.CodigoAPT));
                        comando.Parameters.Add(new SqlParameter("@componente", pag.Componente));
                        comando.Parameters.Add(new SqlParameter("@tipo", pag.TipoRelacionId));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pag.pageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pag.pageNumber));
                        comando.Parameters.Add(new SqlParameter("@OrderBy", pag.sortName));
                        comando.Parameters.Add(new SqlParameter("@OrderByDirection", pag.sortOrder));
                        comando.Parameters.Add(new SqlParameter("@usuario", pag.username));
                        comando.Parameters.Add(new SqlParameter("@perfil", pag.PerfilId));
                        comando.Parameters.Add(new SqlParameter("@subdominioIds", pag.SubdominioIds));
                        comando.Parameters.Add(new SqlParameter("@tecnologia", pag.Tecnologia));

                        comando.Parameters.Add(new SqlParameter("@ambiente", pag.AmbienteIdStr));
                        comando.Parameters.Add(new SqlParameter("@estado", pag.EstadoIdStr));

                        comando.Parameters.Add(new SqlParameter("@dia_filtro", pag.Dia));
                        comando.Parameters.Add(new SqlParameter("@mes_filtro", pag.Mes));
                        comando.Parameters.Add(new SqlParameter("@anio_filtro", pag.Anio));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new RelacionDTO()
                            {
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),
                                EquipoId = reader.IsDBNull(reader.GetOrdinal("EquipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EquipoId")),
                                EstadoId = reader.IsDBNull(reader.GetOrdinal("EstadoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EstadoId")),
                                RelacionId = reader.IsDBNull(reader.GetOrdinal("RelacionId")) ? 0 : reader.GetInt64(reader.GetOrdinal("RelacionId")),
                                TipoId = reader.IsDBNull(reader.GetOrdinal("TipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TipoId")),
                                UsuarioCreacion = reader.IsDBNull(reader.GetOrdinal("CreadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("CreadoPor")),
                                FechaCreacion = reader.IsDBNull(reader.GetOrdinal("FechaCreacion")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("FechaCreacion")),
                                UsuarioModificacion = reader.IsDBNull(reader.GetOrdinal("ModificadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("ModificadoPor")),
                                FechaModificacion = reader.IsDBNull(reader.GetOrdinal("FechaModificacion")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaModificacion")),
                                AplicacionStr = reader.IsDBNull(reader.GetOrdinal("Aplicacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("Aplicacion")),
                                Componente = reader.IsDBNull(reader.GetOrdinal("Componente")) ? string.Empty : reader.GetString(reader.GetOrdinal("Componente")),
                                AmbienteStr = reader.IsDBNull(reader.GetOrdinal("Ambiente")) ? string.Empty : reader.GetString(reader.GetOrdinal("Ambiente")),
                                CodigoAPT = reader.IsDBNull(reader.GetOrdinal("CodigoAPT")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAPT")),
                                Tecnologia = reader.IsDBNull(reader.GetOrdinal("Tecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("Tecnologia")),
                                Aprobar = reader.IsDBNull(reader.GetOrdinal("Aprobar")) ? "false" : reader.GetString(reader.GetOrdinal("Aprobar")),
                                TecnologiaId = reader.IsDBNull(reader.GetOrdinal("TecnologiaId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TecnologiaId")),
                                TipoEquipoId = reader.IsDBNull(reader.GetOrdinal("TipoEquipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TipoEquipoId")),
                                TipoEquipoIdCD = paramCD,
                                FechaRegistroCuarentena = reader.IsDBNull(reader.GetOrdinal("FechaRegistroCuarentena")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("FechaRegistroCuarentena")),
                                FechaFinCuarentena = reader.IsDBNull(reader.GetOrdinal("FechaFinCuarentena")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("FechaFinCuarentena"))
                                // Relación App-Componente
                                ,
                                TipoRelacionComponente = reader.IsDBNull(reader.GetOrdinal("TipoRelacionComponente")) ? 0 : reader.GetInt32(reader.GetOrdinal("TipoRelacionComponente"))
                                ,
                                DescTipoRelacionComp = reader.IsDBNull(reader.GetOrdinal("DescTipoRelacionComp")) ? string.Empty : reader.GetString(reader.GetOrdinal("DescTipoRelacionComp"))
                            };
                            lista.Add(objeto);
                        }
                        reader.Close();
                    }
                    if (lista.Count > 0) totalRows = lista[0].TotalFilas;

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

        public override List<RelacionDTO> GetRelacionSP_3(PaginacionRelacion pag, out int totalRows)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<RelacionDTO>();
                totalRows = 0;
                pag.TipoRelacionId = pag.TipoRelacionId ?? -1;
                pag.EstadoId = pag.EstadoId ?? -2;
                pag.SubdominioIds = !string.IsNullOrEmpty(pag.SubdominioIds) ? pag.SubdominioIds : string.Empty;
                pag.Tecnologia = !string.IsNullOrEmpty(pag.Tecnologia) ? pag.Tecnologia : string.Empty;
                pag.AmbienteId = pag.AmbienteId ?? -1;

                int paramCD = Convert.ToInt32(ServiceManager<ParametroDAO>.Provider.ObtenerParametro("CERTIFICADO_DIGITAL_APLICACION_PARAMETRO_TIPOEQUIPO").Valor);

                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_VerRelacionesAplicacion_3]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@aplicacion", pag.CodigoAPT));
                        comando.Parameters.Add(new SqlParameter("@componente", pag.Componente));
                        comando.Parameters.Add(new SqlParameter("@tipo", pag.TipoRelacionId));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pag.pageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pag.pageNumber));
                        comando.Parameters.Add(new SqlParameter("@OrderBy", pag.sortName));
                        comando.Parameters.Add(new SqlParameter("@OrderByDirection", pag.sortOrder));
                        comando.Parameters.Add(new SqlParameter("@usuario", pag.username));
                        comando.Parameters.Add(new SqlParameter("@perfil", pag.PerfilId));
                        comando.Parameters.Add(new SqlParameter("@subdominioIds", pag.SubdominioIds));
                        comando.Parameters.Add(new SqlParameter("@tecnologia", pag.Tecnologia));

                        comando.Parameters.Add(new SqlParameter("@ambiente", pag.AmbienteIdStr));
                        comando.Parameters.Add(new SqlParameter("@estado", pag.EstadoIdStr));

                        comando.Parameters.Add(new SqlParameter("@dia_filtro", pag.Dia));
                        comando.Parameters.Add(new SqlParameter("@mes_filtro", pag.Mes));
                        comando.Parameters.Add(new SqlParameter("@anio_filtro", pag.Anio));
                        comando.Parameters.Add(new SqlParameter("@tipoEquipoId", pag.TipoEquipoId));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new RelacionDTO()
                            {
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),
                                EquipoId = reader.IsDBNull(reader.GetOrdinal("EquipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EquipoId")),
                                EstadoId = reader.IsDBNull(reader.GetOrdinal("EstadoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EstadoId")),
                                RelacionId = reader.IsDBNull(reader.GetOrdinal("RelacionId")) ? 0 : reader.GetInt64(reader.GetOrdinal("RelacionId")),
                                TipoId = reader.IsDBNull(reader.GetOrdinal("TipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TipoId")),
                                UsuarioCreacion = reader.IsDBNull(reader.GetOrdinal("CreadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("CreadoPor")),
                                FechaCreacion = reader.IsDBNull(reader.GetOrdinal("FechaCreacion")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("FechaCreacion")),
                                UsuarioModificacion = reader.IsDBNull(reader.GetOrdinal("ModificadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("ModificadoPor")),
                                FechaModificacion = reader.IsDBNull(reader.GetOrdinal("FechaModificacion")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("FechaModificacion")),
                                AplicacionStr = reader.IsDBNull(reader.GetOrdinal("Aplicacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("Aplicacion")),
                                Componente = reader.IsDBNull(reader.GetOrdinal("Componente")) ? string.Empty : reader.GetString(reader.GetOrdinal("Componente")),
                                AmbienteStr = reader.IsDBNull(reader.GetOrdinal("Ambiente")) ? string.Empty : reader.GetString(reader.GetOrdinal("Ambiente")),
                                CodigoAPT = reader.IsDBNull(reader.GetOrdinal("CodigoAPT")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAPT")),
                                Tecnologia = reader.IsDBNull(reader.GetOrdinal("Tecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("Tecnologia")),
                                Aprobar = reader.IsDBNull(reader.GetOrdinal("Aprobar")) ? "false" : reader.GetString(reader.GetOrdinal("Aprobar")),
                                TecnologiaId = reader.IsDBNull(reader.GetOrdinal("TecnologiaId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TecnologiaId")),
                                TipoEquipoId = reader.IsDBNull(reader.GetOrdinal("TipoEquipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TipoEquipoId")),
                                TipoEquipoIdCD = paramCD
                                // Relación App-Componente
                                ,
                                TipoRelacionComponente = reader.IsDBNull(reader.GetOrdinal("TipoRelacionComponente")) ? 0 : reader.GetInt32(reader.GetOrdinal("TipoRelacionComponente"))
                                ,
                                DescTipoRelacionComp = reader.IsDBNull(reader.GetOrdinal("DescTipoRelacionComp")) ? string.Empty : reader.GetString(reader.GetOrdinal("DescTipoRelacionComp"))
                            };
                            lista.Add(objeto);
                        }
                        reader.Close();
                    }
                    if (lista.Count > 0) totalRows = lista[0].TotalFilas;

                    return lista;
                }
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetRelacionSP_3()"
                    , new object[] { null });
            }
        }
        public override List<RelacionDTO> GetRelacionSP_2_Exportar(PaginacionRelacion pag, out int totalRows)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<RelacionDTO>();
                totalRows = 0;
                pag.TipoRelacionId = pag.TipoRelacionId ?? -1;
                pag.EstadoId = pag.EstadoId ?? -2;
                pag.SubdominioIds = !string.IsNullOrEmpty(pag.SubdominioIds) ? pag.SubdominioIds : string.Empty;
                pag.Tecnologia = !string.IsNullOrEmpty(pag.Tecnologia) ? pag.Tecnologia : string.Empty;
                pag.AmbienteId = pag.AmbienteId ?? -1;
                int paramCD = Convert.ToInt32(ServiceManager<ParametroDAO>.Provider.ObtenerParametro("CERTIFICADO_DIGITAL_APLICACION_PARAMETRO_TIPOEQUIPO").Valor);

                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_VerRelacionesAplicacion_2_Exportar]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@aplicacion", pag.CodigoAPT));
                        comando.Parameters.Add(new SqlParameter("@componente", pag.Componente));
                        comando.Parameters.Add(new SqlParameter("@tipo", pag.TipoRelacionId));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pag.pageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pag.pageNumber));
                        comando.Parameters.Add(new SqlParameter("@OrderBy", pag.sortName));
                        comando.Parameters.Add(new SqlParameter("@OrderByDirection", pag.sortOrder));
                        comando.Parameters.Add(new SqlParameter("@usuario", pag.username));
                        comando.Parameters.Add(new SqlParameter("@perfil", pag.PerfilId));
                        comando.Parameters.Add(new SqlParameter("@subdominioIds", pag.SubdominioIds));
                        comando.Parameters.Add(new SqlParameter("@tecnologia", pag.Tecnologia));

                        comando.Parameters.Add(new SqlParameter("@ambiente", pag.AmbienteIdStr));
                        comando.Parameters.Add(new SqlParameter("@estado", pag.EstadoIdStr));

                        comando.Parameters.Add(new SqlParameter("@dia_filtro", pag.Dia));
                        comando.Parameters.Add(new SqlParameter("@mes_filtro", pag.Mes));
                        comando.Parameters.Add(new SqlParameter("@anio_filtro", pag.Anio));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new RelacionDTO()
                            {
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),
                                EquipoId = reader.IsDBNull(reader.GetOrdinal("EquipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EquipoId")),
                                EstadoId = reader.IsDBNull(reader.GetOrdinal("EstadoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EstadoId")),
                                RelacionId = reader.IsDBNull(reader.GetOrdinal("RelacionId")) ? 0 : reader.GetInt64(reader.GetOrdinal("RelacionId")),
                                TipoId = reader.IsDBNull(reader.GetOrdinal("TipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TipoId")),
                                UsuarioCreacion = reader.IsDBNull(reader.GetOrdinal("CreadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("CreadoPor")),
                                FechaCreacion = reader.IsDBNull(reader.GetOrdinal("FechaCreacion")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("FechaCreacion")),
                                UsuarioModificacion = reader.IsDBNull(reader.GetOrdinal("ModificadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("ModificadoPor")),
                                FechaModificacion = reader.IsDBNull(reader.GetOrdinal("FechaModificacion")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("FechaModificacion")),
                                AplicacionStr = reader.IsDBNull(reader.GetOrdinal("Aplicacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("Aplicacion")),
                                Componente = reader.IsDBNull(reader.GetOrdinal("Componente")) ? string.Empty : reader.GetString(reader.GetOrdinal("Componente")),
                                ComponenteBD = reader.IsDBNull(reader.GetOrdinal("ComponenteBD")) ? string.Empty : reader.GetString(reader.GetOrdinal("ComponenteBD")),
                                AmbienteStr = reader.IsDBNull(reader.GetOrdinal("Ambiente")) ? string.Empty : reader.GetString(reader.GetOrdinal("Ambiente")),
                                CodigoAPT = reader.IsDBNull(reader.GetOrdinal("CodigoAPT")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAPT")),
                                Tecnologia = reader.IsDBNull(reader.GetOrdinal("Tecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("Tecnologia")),
                                Aprobar = reader.IsDBNull(reader.GetOrdinal("Aprobar")) ? "false" : reader.GetString(reader.GetOrdinal("Aprobar")),
                                TecnologiaId = reader.IsDBNull(reader.GetOrdinal("TecnologiaId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TecnologiaId")),
                                TipoEquipoId = reader.IsDBNull(reader.GetOrdinal("TipoEquipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TipoEquipoId")),
                                DominioStr = reader.IsDBNull(reader.GetOrdinal("Dominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Dominio")),
                                SubDominioStr = reader.IsDBNull(reader.GetOrdinal("SubDominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("SubDominio")),
                                TipoTecnologiaStr = reader.IsDBNull(reader.GetOrdinal("TipoTecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoTecnologia")),
                                RelevanciaStrBD = reader.IsDBNull(reader.GetOrdinal("Relevancia")) ? string.Empty : reader.GetString(reader.GetOrdinal("Relevancia")),
                                FlagActivo = reader.IsDBNull(reader.GetOrdinal("FlagActivo")) ? false : reader.GetBoolean(reader.GetOrdinal("FlagActivo")),
                                FechaInicioCuarentena = reader.IsDBNull(reader.GetOrdinal("FechaRegistroCuarentena")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaRegistroCuarentena")),
                                FechaFinCuarentena = reader.IsDBNull(reader.GetOrdinal("FechaFinCuarentena")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaFinCuarentena")),
                                TipoEquipoIdCD = paramCD,
                                // Relación App-Componente
                                DescTipoRelacionComp = reader.IsDBNull(reader.GetOrdinal("DescTipoRelacionComp")) ? string.Empty : reader.GetString(reader.GetOrdinal("DescTipoRelacionComp")),
                                EstadoIdTecnologia = reader.IsDBNull(reader.GetOrdinal("EstadoIdTecnologia")) ? 0 : reader.GetInt32(reader.GetOrdinal("EstadoIdTecnologia")),
                            };
                            lista.Add(objeto);
                        }
                        reader.Close();
                    }
                    if (lista.Count > 0) totalRows = lista[0].TotalFilas;

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

        public override List<RelacionDTO> GetRelacionSP_3_Exportar(PaginacionRelacion pag, out int totalRows)
        {
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<RelacionDTO>();
                totalRows = 0;
                pag.TipoRelacionId = pag.TipoRelacionId ?? -1;
                pag.EstadoId = pag.EstadoId ?? -2;
                pag.SubdominioIds = !string.IsNullOrEmpty(pag.SubdominioIds) ? pag.SubdominioIds : string.Empty;
                pag.Tecnologia = !string.IsNullOrEmpty(pag.Tecnologia) ? pag.Tecnologia : string.Empty;
                pag.AmbienteId = pag.AmbienteId ?? -1;

                int paramCD = Convert.ToInt32(ServiceManager<ParametroDAO>.Provider.ObtenerParametro("CERTIFICADO_DIGITAL_APLICACION_PARAMETRO_TIPOEQUIPO").Valor);

                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_VerRelacionesAplicacion_3_Exportar]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@aplicacion", pag.CodigoAPT));
                        comando.Parameters.Add(new SqlParameter("@componente", pag.Componente));
                        comando.Parameters.Add(new SqlParameter("@tipo", pag.TipoRelacionId));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pag.pageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pag.pageNumber));
                        comando.Parameters.Add(new SqlParameter("@OrderBy", pag.sortName));
                        comando.Parameters.Add(new SqlParameter("@OrderByDirection", pag.sortOrder));
                        comando.Parameters.Add(new SqlParameter("@usuario", pag.username));
                        comando.Parameters.Add(new SqlParameter("@perfil", pag.PerfilId));
                        comando.Parameters.Add(new SqlParameter("@subdominioIds", pag.SubdominioIds));
                        comando.Parameters.Add(new SqlParameter("@tecnologia", pag.Tecnologia));

                        comando.Parameters.Add(new SqlParameter("@ambiente", pag.AmbienteIdStr));
                        comando.Parameters.Add(new SqlParameter("@estado", pag.EstadoIdStr));

                        comando.Parameters.Add(new SqlParameter("@dia_filtro", pag.Dia));
                        comando.Parameters.Add(new SqlParameter("@mes_filtro", pag.Mes));
                        comando.Parameters.Add(new SqlParameter("@anio_filtro", pag.Anio));
                        comando.Parameters.Add(new SqlParameter("@tipoEquipoId", pag.TipoEquipoId));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new RelacionDTO()
                            {
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),
                                EquipoId = reader.IsDBNull(reader.GetOrdinal("EquipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EquipoId")),
                                EstadoId = reader.IsDBNull(reader.GetOrdinal("EstadoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EstadoId")),
                                RelacionId = reader.IsDBNull(reader.GetOrdinal("RelacionId")) ? 0 : reader.GetInt64(reader.GetOrdinal("RelacionId")),
                                TipoId = reader.IsDBNull(reader.GetOrdinal("TipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TipoId")),
                                UsuarioCreacion = reader.IsDBNull(reader.GetOrdinal("CreadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("CreadoPor")),
                                FechaCreacion = reader.IsDBNull(reader.GetOrdinal("FechaCreacion")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("FechaCreacion")),
                                UsuarioModificacion = reader.IsDBNull(reader.GetOrdinal("ModificadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("ModificadoPor")),
                                FechaModificacion = reader.IsDBNull(reader.GetOrdinal("FechaModificacion")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("FechaModificacion")),
                                AplicacionStr = reader.IsDBNull(reader.GetOrdinal("Aplicacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("Aplicacion")),
                                Componente = reader.IsDBNull(reader.GetOrdinal("Componente")) ? string.Empty : reader.GetString(reader.GetOrdinal("Componente")),
                                ComponenteBD = reader.IsDBNull(reader.GetOrdinal("ComponenteBD")) ? string.Empty : reader.GetString(reader.GetOrdinal("ComponenteBD")),
                                AmbienteStr = reader.IsDBNull(reader.GetOrdinal("Ambiente")) ? string.Empty : reader.GetString(reader.GetOrdinal("Ambiente")),
                                CodigoAPT = reader.IsDBNull(reader.GetOrdinal("CodigoAPT")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAPT")),
                                Tecnologia = reader.IsDBNull(reader.GetOrdinal("Tecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("Tecnologia")),
                                Aprobar = reader.IsDBNull(reader.GetOrdinal("Aprobar")) ? "false" : reader.GetString(reader.GetOrdinal("Aprobar")),
                                TecnologiaId = reader.IsDBNull(reader.GetOrdinal("TecnologiaId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TecnologiaId")),
                                TipoEquipoId = reader.IsDBNull(reader.GetOrdinal("TipoEquipoId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TipoEquipoId")),
                                DominioStr = reader.IsDBNull(reader.GetOrdinal("Dominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Dominio")),
                                SubDominioStr = reader.IsDBNull(reader.GetOrdinal("SubDominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("SubDominio")),
                                TipoTecnologiaStr = reader.IsDBNull(reader.GetOrdinal("TipoTecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoTecnologia")),
                                RelevanciaStrBD = reader.IsDBNull(reader.GetOrdinal("Relevancia")) ? string.Empty : reader.GetString(reader.GetOrdinal("Relevancia")),
                                FlagActivo = reader.IsDBNull(reader.GetOrdinal("FlagActivo")) ? false : reader.GetBoolean(reader.GetOrdinal("FlagActivo")),
                                FechaInicioCuarentena = reader.IsDBNull(reader.GetOrdinal("FechaRegistroCuarentena")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaRegistroCuarentena")),
                                FechaFinCuarentena = reader.IsDBNull(reader.GetOrdinal("FechaFinCuarentena")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaFinCuarentena")),
                                TipoEquipoIdCD = paramCD,
                                // Relación App-Componente
                                DescTipoRelacionComp = reader.IsDBNull(reader.GetOrdinal("DescTipoRelacionComp")) ? string.Empty : reader.GetString(reader.GetOrdinal("DescTipoRelacionComp")),
                                EstadoIdTecnologia = reader.IsDBNull(reader.GetOrdinal("EstadoIdTecnologia")) ? 0 : reader.GetInt32(reader.GetOrdinal("EstadoIdTecnologia")),
                            };
                            lista.Add(objeto);
                        }
                        reader.Close();
                    }
                    if (lista.Count > 0) totalRows = lista[0].TotalFilas;

                    return lista;
                }
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetRelacionSP_3()"
                    , new object[] { null });
            }
        }

        public override List<AplicacionRelacionDTO> GetAplicacionByFilter(PaginacionRelacionFilter pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                pag.pageSize = pag.All ? int.MaxValue : pag.pageSize;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.Aplicacion
                                     where (string.IsNullOrEmpty(pag.TipoActivo)
                                     || u.TipoActivoInformacion.ToUpper().Contains(pag.TipoActivo.ToUpper()))
                                     && (string.IsNullOrEmpty(pag.GestionadoPor)
                                     || u.GestionadoPor.ToUpper().Contains(pag.GestionadoPor.ToUpper()))
                                     && (string.IsNullOrEmpty(pag.EstadoAplicacion)
                                     || u.EstadoAplicacion.ToUpper().Contains(pag.EstadoAplicacion.ToUpper()))
                                     && (string.IsNullOrEmpty(pag.JefeEquipo)
                                     || u.JefeEquipo_ExpertoAplicacionUserIT_ProductOwner.ToUpper().Contains(pag.JefeEquipo.ToUpper()))
                                     && (string.IsNullOrEmpty(pag.LiderUsuario)
                                     || u.Owner_LiderUsuario_ProductOwner.ToUpper().Contains(pag.LiderUsuario.ToUpper()))
                                     && (string.IsNullOrEmpty(pag.TTL)
                                     || u.TribeTechnicalLead.ToUpper().Contains(pag.TTL.ToUpper()))
                                     && u.FlagActivo
                                     select new AplicacionRelacionDTO()
                                     {
                                         Id = u.AplicacionId,
                                         CodigoAPT = u.CodigoAPT,
                                         Nombre = u.Nombre,
                                         Estado = u.EstadoAplicacion,
                                         TipoActivo = u.TipoActivoInformacion,
                                     }).OrderBy(pag.sortName + " " + pag.sortOrder);

                    totalRows = registros.Count();
                    var resultado = registros.Skip((pag.pageNumber - 1) * pag.pageSize).Take(pag.pageSize).ToList();

                    return resultado;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: List<AplicacionRelacionDTO> GetAplicacionByFilter(PaginacionRelacionFilter pag, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: List<AplicacionRelacionDTO> GetAplicacionByFilter(PaginacionRelacionFilter pag, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override bool MassiveUpdateAplicacion(AplicacionTecnologiaDTO objRegistro)
        {
            DbContextTransaction transaction = null;
            try
            {
                //long ID = 0;
                var state = true;
                DateTime FECHA_ACTUAL = DateTime.Now;
                var arrCodigoAPT = objRegistro.CodigoAPTArr.Split('|');
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    using (transaction = ctx.Database.BeginTransaction())
                    {
                        foreach (var codigoAPT in arrCodigoAPT)
                        {
                            //var objExperto = ctx.AplicacionPortafolioResponsables.Where(x => x.FlagActivo
                            //                                                            && x.Matricula == objRegistro.UsuarioCreacion
                            //                                                            && x.CodigoAPT == objRegistro.CodigoAPT).FirstOrDefault();
                            //var idestado = objExperto != null ? (int)EEstadoRelacion.Aprobado : objRegistro.EstadoId;

                            //if (objExperto == null)
                            //{
                            //    var objExperto2 = ctx.AplicacionExpertos.Where(x => x.FlagActivo
                            //                                                    && x.Matricula == objRegistro.UsuarioCreacion
                            //                                                    && x.CodigoAPT == objRegistro.CodigoAPT
                            //                                                    && x.TipoExpertoId == (int)ETipoExperto.ExpertoAplicacion).FirstOrDefault();

                            //    idestado = objExperto2 != null ? (int)EEstadoRelacion.Aprobado : objRegistro.EstadoId;
                            //}

                            var itemRelacionFound = (from x in ctx.Relacion
                                                     join y in ctx.RelacionDetalle on x.RelacionId equals y.RelacionId
                                                     where x.FlagActivo && x.FlagActivo
                                                     && x.CodigoAPT.ToUpper().Equals(codigoAPT.ToUpper())
                                                     && y.TecnologiaId == objRegistro.TecnologiaId
                                                     && x.TipoId == (int)ETipoRelacion.Tecnologia
                                                     && x.DiaRegistro == FECHA_ACTUAL.Day
                                                     && x.MesRegistro == FECHA_ACTUAL.Month
                                                     && x.AnioRegistro == FECHA_ACTUAL.Year
                                                     && (x.EstadoId == (int)EEstadoRelacion.Aprobado || x.EstadoId == (int)EEstadoRelacion.Pendiente)
                                                     select x).FirstOrDefault();

                            if (itemRelacionFound != null)
                            {
                                itemRelacionFound.FechaModificacion = FECHA_ACTUAL;
                                itemRelacionFound.ModificadoPor = objRegistro.UsuarioModificacion;

                                var itemRDFound = (from z in ctx.RelacionDetalle
                                                   where z.FlagActivo && z.RelacionId == itemRelacionFound.RelacionId
                                                   select z).FirstOrDefault();

                                if (itemRDFound != null)
                                {
                                    itemRDFound.FechaModificacion = FECHA_ACTUAL;
                                    itemRDFound.ModificadoPor = objRegistro.UsuarioModificacion;
                                }
                            }
                            else
                            {
                                var newRelacion = new Relacion()
                                {
                                    CodigoAPT = codigoAPT,
                                    TipoId = (int)ETipoRelacion.Tecnologia,
                                    AmbienteId = null,
                                    EquipoId = null,
                                    EstadoId = (int)EEstadoRelacion.Aprobado,
                                    FlagActivo = true,
                                    FechaCreacion = FECHA_ACTUAL,
                                    CreadoPor = objRegistro.UsuarioCreacion,
                                    DiaRegistro = FECHA_ACTUAL.Day,
                                    MesRegistro = FECHA_ACTUAL.Month,
                                    AnioRegistro = FECHA_ACTUAL.Year
                                };
                                ctx.Relacion.Add(newRelacion);
                                ctx.SaveChanges();

                                var newRelacionDetalle = new RelacionDetalle()
                                {
                                    RelacionId = newRelacion.RelacionId,
                                    TecnologiaId = objRegistro.TecnologiaId,
                                    RelevanciaId = 1,
                                    FlagActivo = true,
                                    FechaCreacion = FECHA_ACTUAL,
                                    CreadoPor = objRegistro.UsuarioCreacion
                                };
                                ctx.RelacionDetalle.Add(newRelacionDetalle);
                            }
                            ctx.SaveChanges();
                        }

                        transaction.Commit();

                        return state;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                transaction.Rollback();
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorRelacionDTO
                    , "Error en el metodo: int AddOrEditRelacion(RelacionDTO objRegistro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorRelacionDTO
                    , "Error en el metodo: int AddOrEditRelacion(RelacionDTO objRegistro)"
                    , new object[] { null });
            }
        }

        public override bool ExisteTecnologiaEquipoActivaById(int id, int tipoRelacionId)
        {
            try
            {
                bool? estado = false;
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        if (tipoRelacionId == (int)ETipoRelacion.Tecnologia)
                        {
                            estado = (from a1 in ctx.Tecnologia
                                      where a1.Activo && a1.TecnologiaId == id
                                      select true).FirstOrDefault();
                        }
                        else
                        {
                            estado = (from a1 in ctx.Equipo
                                      where a1.FlagActivo && a1.EquipoId == id
                                      select true).FirstOrDefault();
                        }
                    }
                }
                return estado == true;
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorRelacionDTO
                    , "Error en el metodo: bool ExisteTecnologiaActivaById(int tecnologiaId)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorRelacionDTO
                    , "Error en el metodo: bool ExisteTecnologiaActivaById(int tecnologiaId)"
                    , new object[] { null });
            }
        }

        public override bool ValidarRelacionEquipo(int EquipoId, DateTime Fecha)
        {
            try
            {
                var dia = Fecha.Day;
                var mes = Fecha.Month;
                var anio = Fecha.Year;

                var estadosValidar = new List<int>() { (int)EEstadoRelacion.Aprobado, (int)EEstadoRelacion.Pendiente, (int)EEstadoRelacion.PendienteEliminacion, (int)EEstadoRelacion.Cuarentena };

                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    bool retorno = false;

                    retorno = (from a in ctx.Relacion
                               where a.EquipoId == EquipoId && (a.DiaRegistro == dia && a.MesRegistro == mes && a.AnioRegistro == anio)
                               && estadosValidar.Contains(a.EstadoId)
                               select a).Count() > 1;

                    return retorno;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: bool ValidarRelacionEquipo(int Id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: bool ValidarRelacionEquipo(int Id)"
                    , new object[] { null });
            }
        }

        public override int GetTipoEquipoIdByNombre(string Nombre)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    int retorno = 0;

                    retorno = (from u in ctx.TipoEquipo
                               where u.Nombre == Nombre && u.FlagActivo
                               select u.TipoEquipoId).FirstOrDefault();

                    return retorno;
                }
            }
            catch (DbEntityValidationException ex)
            {
                HelperLog.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: int GetTipoEquipoIdByNombre(string Nombre)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTipoDTO
                    , "Error en el metodo: int GetTipoEquipoIdByNombre(string Nombre)"
                    , new object[] { null });
            }
        }

        // --> Relaciones entre Apps
        public override List<RelacionDTO> GetList_DependenciaAplicacion(PaginacionRelacion pag, out int totalRows)
        {
            try
            {
                var lista = new List<RelacionDTO>();
                totalRows = 0;
                pag.CodigoAPT = String.IsNullOrEmpty(pag.CodigoAPT) ? "" : pag.CodigoAPT;

                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[DA_GetList_DependenciaAplicacion]", cnx))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@Perfil", pag.PerfilId);
                        comando.Parameters.AddWithValue("@Usuario", pag.username);
                        comando.Parameters.AddWithValue("@CodigoApt", pag.CodigoAPT);
                        comando.Parameters.AddWithValue("@estado", pag.EstadoIdStr);
                        comando.Parameters.AddWithValue("@Dia", pag.Dia);
                        comando.Parameters.AddWithValue("@Mes", pag.Mes);
                        comando.Parameters.AddWithValue("@Anio", pag.Anio);
                        comando.Parameters.AddWithValue("@PageSize", pag.pageSize);
                        comando.Parameters.AddWithValue("@PageNumber", pag.pageNumber);

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new RelacionDTO()
                            {
                                RelacionId = reader.IsDBNull(reader.GetOrdinal("RelacionId")) ? 0 : reader.GetInt64(reader.GetOrdinal("RelacionId")),
                                CodigoAptOrigen = reader.IsDBNull(reader.GetOrdinal("CodigoAptOrigen")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAptOrigen")),
                                NombreAptOrigen = reader.IsDBNull(reader.GetOrdinal("NombreAptOrigen")) ? string.Empty : reader.GetString(reader.GetOrdinal("NombreAptOrigen")),
                                CodigoAptDestino = reader.IsDBNull(reader.GetOrdinal("CodigoAptDestino")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAptDestino")),
                                NombreAptDestino = reader.IsDBNull(reader.GetOrdinal("NombreAptDestino")) ? string.Empty : reader.GetString(reader.GetOrdinal("NombreAptDestino")),
                                TipoId = reader.IsDBNull(reader.GetOrdinal("TipoRelacionId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TipoRelacionId")),
                                DescripcionTipoRelacion = reader.IsDBNull(reader.GetOrdinal("DescripcionTipoRelacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("DescripcionTipoRelacion")),
                                EstadoId = reader.IsDBNull(reader.GetOrdinal("EstadoRelacionId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EstadoRelacionId")),
                                Aprobar = reader.IsDBNull(reader.GetOrdinal("Aprobar")) ? string.Empty : reader.GetString(reader.GetOrdinal("Aprobar")),
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),
                                TipoEquipoId = -1, // Evitar conflicto con descripción de tipo de equipo
                                UsuarioCreacion = reader.IsDBNull(reader.GetOrdinal("CreadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("CreadoPor")),
                                FechaCreacion = reader.IsDBNull(reader.GetOrdinal("FechaCreacion")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("FechaCreacion")),
                                UsuarioModificacion = reader.IsDBNull(reader.GetOrdinal("ModificadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("ModificadoPor")),
                                FechaModificacion = reader.IsDBNull(reader.GetOrdinal("FechaModificacion")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("FechaModificacion"))
                            };
                            lista.Add(objeto);
                        }
                        reader.Close();
                    }

                    if (lista.Count > 0) totalRows = lista[0].TotalFilas;

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

        public override List<RelacionDTO> Get_DependenciaAplicacion(int Id)
        {
            try
            {
                var lista = new List<RelacionDTO>();

                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[DA_Get_DependenciaAplicacion]", cnx))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@RelacionId", Id);

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new RelacionDTO()
                            {
                                RelacionId = reader.IsDBNull(reader.GetOrdinal("RelacionId")) ? 0 : reader.GetInt64(reader.GetOrdinal("RelacionId")),
                                CodigoAptOrigen = reader.IsDBNull(reader.GetOrdinal("CodigoAptOrigen")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAptOrigen")),
                                NombreAptOrigen = reader.IsDBNull(reader.GetOrdinal("NombreAptOrigen")) ? string.Empty : reader.GetString(reader.GetOrdinal("NombreAptOrigen")),
                                CodigoAptDestino = reader.IsDBNull(reader.GetOrdinal("CodigoAptDestino")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAptDestino")),
                                NombreAptDestino = reader.IsDBNull(reader.GetOrdinal("NombreAptDestino")) ? string.Empty : reader.GetString(reader.GetOrdinal("NombreAptDestino")),
                                TipoId = reader.IsDBNull(reader.GetOrdinal("TipoRelacionId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TipoRelacionId")),
                                DescripcionTipoRelacion = reader.IsDBNull(reader.GetOrdinal("DescripcionTipoRelacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("DescripcionTipoRelacion")),
                                EstadoId = reader.IsDBNull(reader.GetOrdinal("EstadoRelacionId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EstadoRelacionId")),
                                TipoEquipoId = -1, // Evitar conflicto con descripción de tipo de equipo
                                UsuarioCreacion = reader.IsDBNull(reader.GetOrdinal("CreadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("CreadoPor")),
                                FechaCreacion = reader.IsDBNull(reader.GetOrdinal("FechaCreacion")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("FechaCreacion")),
                                UsuarioModificacion = reader.IsDBNull(reader.GetOrdinal("ModificadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("ModificadoPor")),
                                FechaModificacion = reader.IsDBNull(reader.GetOrdinal("FechaModificacion")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("FechaModificacion"))
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

        public override bool ExisteRelacionApp(RelacionDTO objRegistro)
        {
            try
            {
                var respuestaSP = 0;
                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[DA_Existe_DependenciaAplicacion]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@CodigoAptOrigen", objRegistro.CodigoAptOrigen));
                        comando.Parameters.Add(new SqlParameter("@CodigoAptDestino", objRegistro.CodigoAptDestino));
                        var output = comando.Parameters.Add(new SqlParameter("@Resultado", SqlDbType.Int));
                        output.Direction = ParameterDirection.Output;

                        comando.ExecuteNonQuery();
                        respuestaSP = (int)output.Value;
                    }
                }
                if (respuestaSP <= 0)
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: RelacionSvc.ExisteRelacionApp(string codigoAptOrigen, string codigoAptDestino, string aplicacionRelacion)"
                    , new object[] { null });
            }
        }

        public override bool RegistrarRelacionApp(RelacionDTO objRegistro, out int resultado)
        {
            try
            {
                var respuestaSP = "";

                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var objExperto = ctx.AplicacionPortafolioResponsables.Where(x => x.FlagActivo
                                                                                           && x.Matricula == objRegistro.UsuarioCreacion
                                                                                           && x.CodigoAPT == objRegistro.CodigoAptOrigen).FirstOrDefault();
                    var idestado = objExperto != null ? (int)EEstadoRelacion.Aprobado : objRegistro.EstadoId;

                    if (objExperto == null)
                    {
                        var objExperto2 = ctx.AplicacionExpertos.Where(x => x.FlagActivo
                                                                        && x.Matricula == objRegistro.UsuarioCreacion
                                                                        && x.CodigoAPT == objRegistro.CodigoAptOrigen
                                                                        && x.TipoExpertoId == (int)ETipoExperto.ExpertoAplicacion).FirstOrDefault();

                        idestado = objExperto2 != null ? (int)EEstadoRelacion.Aprobado : objRegistro.EstadoId;
                    }

                    using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                    {
                        cnx.Open();
                        using (var comando = new SqlCommand("[cvt].[DA_Insert_DependenciaAplicacion]", cnx))
                        {
                            comando.CommandTimeout = 0;
                            comando.CommandType = CommandType.StoredProcedure;
                            comando.Parameters.Add(new SqlParameter("@CodigoAptOrigen", objRegistro.CodigoAptOrigen));
                            comando.Parameters.Add(new SqlParameter("@CodigoAptDestino", objRegistro.CodigoAptDestino));
                            comando.Parameters.Add(new SqlParameter("@TipoRelacionId", objRegistro.TipoId));
                            comando.Parameters.Add(new SqlParameter("@EstadoRelacionId", idestado));
                            comando.Parameters.Add(new SqlParameter("@UsuarioCreacion", objRegistro.UsuarioCreacion));
                            var output = comando.Parameters.Add(new SqlParameter("@Resultado", SqlDbType.NVarChar, 100));
                            output.Direction = ParameterDirection.Output;

                            comando.ExecuteNonQuery();
                            respuestaSP = (string)output.Value;
                        }
                    }
                }

                resultado = Convert.ToInt32(respuestaSP.Substring(0, 1));
                return true;
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: RelacionSvc.RegistrarRelacionApp(string codigoAptOrigen, string codigoAptDestino, int tipoRelacionId, int estadoRelacionId, string username, out int resultado)"
                    , new object[] { null });
            }
        }

        public override bool ActualizarRelacionApp(RelacionDTO objRegistro, out int resultado)
        {
            try
            {
                var respuestaSP = "";
                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[DA_Update_DependenciaAplicacion]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@RelacionId", objRegistro.RelacionId));
                        comando.Parameters.Add(new SqlParameter("@TipoRelacionId", objRegistro.TipoId));
                        comando.Parameters.Add(new SqlParameter("@EstadoRelacionId", objRegistro.EstadoId));
                        comando.Parameters.Add(new SqlParameter("@ModificadoPor", objRegistro.UsuarioModificacion));
                        var output = comando.Parameters.Add(new SqlParameter("@Resultado", SqlDbType.NVarChar, 100));
                        output.Direction = ParameterDirection.Output;

                        comando.ExecuteNonQuery();
                        respuestaSP = (string)output.Value;
                    }
                }
                resultado = Convert.ToInt32(respuestaSP.Substring(0, 1));
                return true;
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: RelacionSvc.ActualizarRelacionApp(RelacionDTO objRegistro, out int resultado)"
                    , new object[] { null });
            }
        }

        public override List<RelacionDTO> GetList_DependenciaAplicacion_Exportar(PaginacionRelacion pag, out int totalRows)
        {
            try
            {
                var lista = new List<RelacionDTO>();
                totalRows = 0;
                pag.AplicacionRelacion = String.IsNullOrEmpty(pag.AplicacionRelacion) ? "" : pag.AplicacionRelacion;

                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[DA_GetList_DependenciaAplicacion_Exportar]", cnx))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@Perfil", pag.PerfilId);
                        comando.Parameters.AddWithValue("@Usuario", pag.username);
                        comando.Parameters.AddWithValue("@CodigoApt", pag.AplicacionRelacion);
                        comando.Parameters.AddWithValue("@estado", pag.EstadoIdStr);
                        comando.Parameters.AddWithValue("@dia", pag.Dia);
                        comando.Parameters.AddWithValue("@mes", pag.Mes);
                        comando.Parameters.AddWithValue("@anio", pag.Anio);

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new RelacionDTO()
                            {
                                CodigoAptOrigen = reader.IsDBNull(reader.GetOrdinal("CodigoAptOrigen")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAptOrigen")),
                                NombreAptOrigen = reader.IsDBNull(reader.GetOrdinal("NombreAptOrigen")) ? string.Empty : reader.GetString(reader.GetOrdinal("NombreAptOrigen")),
                                CodigoAptDestino = reader.IsDBNull(reader.GetOrdinal("CodigoAptDestino")) ? string.Empty : reader.GetString(reader.GetOrdinal("CodigoAptDestino")),
                                NombreAptDestino = reader.IsDBNull(reader.GetOrdinal("NombreAptDestino")) ? string.Empty : reader.GetString(reader.GetOrdinal("NombreAptDestino")),
                                // TipoRelacionId = TipoId
                                TipoId = reader.IsDBNull(reader.GetOrdinal("TipoRelacionId")) ? 0 : reader.GetInt32(reader.GetOrdinal("TipoRelacionId")),
                                DescripcionTipoRelacion = reader.IsDBNull(reader.GetOrdinal("DescripcionTipoRelacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("DescripcionTipoRelacion")),
                                EstadoId = reader.IsDBNull(reader.GetOrdinal("EstadoRelacionId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EstadoRelacionId")),
                                AplicacionRelacion = reader.IsDBNull(reader.GetOrdinal("AplicacionRelacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("AplicacionRelacion")),
                                Aprobar = "false",
                                TotalFilas = 0,
                                // Evitar conflicto con descripción de tipo de equipo
                                TipoEquipoId = -1,
                                UsuarioCreacion = reader.IsDBNull(reader.GetOrdinal("CreadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("CreadoPor")),
                                FechaCreacion = reader.IsDBNull(reader.GetOrdinal("FechaCreacion")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("FechaCreacion")),
                                UsuarioModificacion = reader.IsDBNull(reader.GetOrdinal("ModificadoPor")) ? string.Empty : reader.GetString(reader.GetOrdinal("ModificadoPor")),
                                FechaModificacion = reader.IsDBNull(reader.GetOrdinal("FechaModificacion")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("FechaModificacion"))
                            };
                            lista.Add(objeto);
                        }
                        reader.Close();
                    }

                    if (lista.Count > 0) totalRows = lista[0].TotalFilas;

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

        public override bool EliminarRelacionApp(RelacionDTO objRegistro, out int resultado)
        {
            try
            {
                var respuestaSP = "";
                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[DA_Eliminar_DependenciaAplicacion]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@RelacionId", objRegistro.RelacionId));
                        comando.Parameters.Add(new SqlParameter("@Comentario", objRegistro.MotivoEliminacionRelacionApp));
                        comando.Parameters.Add(new SqlParameter("@UsuarioCreacion", objRegistro.UsuarioCreacion));
                        var output = comando.Parameters.Add(new SqlParameter("@Resultado", SqlDbType.NVarChar, 100));
                        output.Direction = ParameterDirection.Output;

                        comando.ExecuteNonQuery();
                        respuestaSP = (string)output.Value;
                    }
                }
                resultado = Convert.ToInt32(respuestaSP.Substring(0, 1));
                return true;
            }
            catch (Exception ex)
            {
                HelperLog.Error(ex.Message);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: RelacionSvc.EliminarRelacionApp(RelacionDTO objRegistro, out int resultado)"
                    , new object[] { null });
            }
        }

        public override string GetComentarioEliminacionRelacionApp(int RelacionId)
        {
            try
            {
                var objeto = new RelacionDTO();
                using (SqlConnection cnx = new SqlConnection(Constantes.CadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[DA_Get_ComentarioRelacionEliminada]", cnx))
                    {
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.AddWithValue("@RelacionId", RelacionId);

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {

                            objeto.MotivoEliminacionRelacionApp = reader.IsDBNull(reader.GetOrdinal("Contenido")) ? string.Empty : reader.GetString(reader.GetOrdinal("Contenido"));
                        }
                        reader.Close();
                    }

                    return objeto.MotivoEliminacionRelacionApp;
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
        // <--
    }
}
