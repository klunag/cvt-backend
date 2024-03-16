using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.DTO.Custom;
using BCP.CVT.Services.Interface;
using BCP.CVT.Services.ModelDB;
using BCP.CVT.Services.SQL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Dynamic;
using System.Transactions;
using IsolationLevel = System.Transactions.IsolationLevel;

namespace BCP.CVT.Services.Service
{
    public class TecnologiaNoRegistradaSvc : TecnologiaNoRegistradaDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public override bool AsociarTecnologiasNoRegistradas(int tecId, List<ObjTecnologiaNoRegistrada> itemsTecNoRegId, string UsuarioCreacion, string UsuarioModificacion)
        {
            string aplicacion = string.Empty;

            DbContextTransaction transaction = null;
            try
            {
                if (itemsTecNoRegId.Count > 0)
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        using (transaction = ctx.Database.BeginTransaction())
                        {

                            foreach (var item in itemsTecNoRegId)
                            {
                                var itemTecNoReg = (from u in ctx.TecnologiaNoRegistrada
                                                    where u.Activo && u.Aplicacion == item.Aplicacion
                                                    //&& u.EquipoId == item.EquipoId 
                                                    && u.FlagAsociado == false
                                                    select u).FirstOrDefault();

                                if (itemTecNoReg != null)
                                {
                                    itemTecNoReg.FlagAsociado = true;
                                    aplicacion = itemTecNoReg.Aplicacion;

                                    var entidad = new TecnologiaEquivalencia()
                                    {
                                        TecnologiaId = tecId,
                                        Nombre = itemTecNoReg.Aplicacion,
                                        FlagActivo = true,
                                        FechaCreacion = DateTime.Now,
                                        CreadoPor = UsuarioCreacion
                                    };

                                    ctx.TecnologiaEquivalencia.Add(entidad);
                                    ctx.SaveChanges();

                                    transaction.Commit();

                                    
                                }
                            }                            
                        }
                    }

                    if (!string.IsNullOrEmpty(aplicacion))
                    {
                        List<SQLParam> ListsQLParam = new List<SQLParam>();
                        ListsQLParam.Add(new SQLParam("@aplicacion", aplicacion, SqlDbType.NVarChar));
                        string SP = "[CVT].[USP_ActualizarTecnologiaNoRegistrada]";

                        new SQLManager().EjecutarStoredProcedure_2(SP, ListsQLParam);
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (DbEntityValidationException ex)
            {
                transaction.Rollback();
                log.ErrorEntity(ex);
                log.Error(ex.Message, ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaNoRegistradaDTO
                    , "Error en el metodo: bool AsociarTecNoReg(int tecId, List<int> itemsTecNoRegId, string UsuarioCreacion, string UsuarioModificacion)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                log.Error(ex.Message, ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaNoRegistradaDTO
                    , "Error en el metodo: bool AsociarTecNoReg(int tecId, List<int> itemsTecNoRegId, string UsuarioCreacion, string UsuarioModificacion))"
                    , new object[] { null });
            }
        }

        public override bool AsociarTecNoReg(int tecId, List<int> itemsTecNoRegId, string UsuarioCreacion, string UsuarioModificacion)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    if (itemsTecNoRegId.Count > 0)
                    {
                        foreach (var itemId in itemsTecNoRegId)
                        {
                            //var itemBD = (from u in ctx.TecnologiaEquivalencia
                            //              where (u.TecnologiaId == tecId && u.FlagActivo)
                            //              select u).FirstOrDefault();

                            //if (itemBD != null)
                            //{
                            //    itemBD.FechaModificacion = DateTime.Now;
                            //    itemBD.ModificadoPor = UsuarioModificacion;
                            //    // itemBD.TecnologiaId = tecId;
                            //}
                            //else
                            //{
                            var itemTecNoReg = (from u in ctx.TecnologiaNoRegistrada
                                                where u.Activo && u.TecnologiaNoRegistradaId == itemId
                                                select u).FirstOrDefault();

                            if (itemTecNoReg != null)
                            {
                                itemTecNoReg.FlagAsociado = true;

                                var entidad = new TecnologiaEquivalencia()
                                {
                                    TecnologiaId = tecId,
                                    Nombre = itemTecNoReg.Aplicacion,
                                    FlagActivo = true,
                                    FechaCreacion = DateTime.Now,
                                    CreadoPor = UsuarioCreacion
                                };

                                ctx.TecnologiaEquivalencia.Add(entidad);
                                ctx.SaveChanges();
                            }                           
                            //}
                        }
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
                log.ErrorEntity(ex);
                log.Error(ex.Message, ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaNoRegistradaDTO
                    , "Error en el metodo: bool AsociarTecNoReg(int tecId, List<int> itemsTecNoRegId, string UsuarioCreacion, string UsuarioModificacion)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {                
                log.Error(ex.Message, ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaNoRegistradaDTO
                    , "Error en el metodo: bool AsociarTecNoReg(int tecId, List<int> itemsTecNoRegId, string UsuarioCreacion, string UsuarioModificacion))"
                    , new object[] { null });
            }
        }

        public override bool ExisteClaveByNombre(string nombre)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    bool? estado = (from u in ctx.Tecnologia
                                    where u.Activo
                                    && u.ClaveTecnologia.ToUpper() == nombre.ToUpper()
                                    select true).FirstOrDefault();

                    return estado == true;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: bool ExisteEquipoByNombre(string nombre)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: bool ExisteEquipoByNombre(string nombre)"
                    , new object[] { null });
            }
        }

        public override int ExisteDominioByNombre(string nombre)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    int? id = null;
                    id = (from u in ctx.Dominio
                                  where u.Activo
                                  && u.Nombre.ToUpper().Equals(nombre.ToUpper())
                                  select u.DominioId).FirstOrDefault();

                    return id == null ? 0 : id.Value;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: bool ExisteEquipoByNombre(string nombre)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: bool ExisteEquipoByNombre(string nombre)"
                    , new object[] { null });
            }
        }

        public override bool ExisteDominioBySubdominio(int iddominio, int idsubdominio)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    bool? estado = (from s in ctx.Subdominio
                                    join d in ctx.Dominio on s.DominioId equals d.DominioId
                                    where s.Activo && d.Activo
                                    //&& u.ClaveTecnologia.ToUpper() == nombre.ToUpper()
                                    && s.SubdominioId == idsubdominio 
                                    && d.DominioId == iddominio
                                    select true).FirstOrDefault();

                    return estado == true;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: bool ExisteEquipoByNombre(string nombre)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: bool ExisteEquipoByNombre(string nombre)"
                    , new object[] { null });
            }
        }

        public override int ExisteFamiliaByNombre(string nombre)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    int? id = null;
                    id = (from u in ctx.Familia
                          where u.Activo
                          && u.Nombre.ToUpper().Equals(nombre.ToUpper())
                          select u.FamiliaId).FirstOrDefault();

                    return id == null ? 0 : id.Value;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: bool ExisteEquipoByNombre(string nombre)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: bool ExisteEquipoByNombre(string nombre)"
                    , new object[] { null });
            }
        }

        public override int ExisteSubdominioByNombre(string nombre)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    int? id = null;
                    id = (from u in ctx.Subdominio
                          where u.Activo
                          && u.Nombre.ToUpper().Equals(nombre.ToUpper())
                          select u.SubdominioId).FirstOrDefault();

                    return id == null ? 0 : id.Value;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: bool ExisteEquipoByNombre(string nombre)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: bool ExisteEquipoByNombre(string nombre)"
                    , new object[] { null });
            }
        }

        public override int ExisteTipoByNombre(string nombre)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    int? id = null;
                    id = (from u in ctx.Tipo
                          where u.Activo
                          && u.Nombre.ToUpper().Equals(nombre.ToUpper())
                          select u.TipoId).FirstOrDefault();

                    return id == null ? 0 : id.Value;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: bool ExisteEquipoByNombre(string nombre)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: bool ExisteEquipoByNombre(string nombre)"
                    , new object[] { null });
            }
        }

        public override int GetDominioIdBySubdominioId(int SubdominioId)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        int DominioId = 0;
                        DominioId = (from u in ctx.Subdominio
                                     where u.SubdominioId == SubdominioId
                                     && u.Activo
                                     select u.DominioId).FirstOrDefault();

                        return DominioId;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                log.Error(ex.Message, ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaNoRegistradaDTO
                    , "Error en el metodo: int GetDominioIdBySubdominioId(int SubdominioId)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaNoRegistradaDTO
                    , "Error en el metodo: int GetDominioIdBySubdominioId(int SubdominioId)"
                    , new object[] { null });
            }
        }

        public override List<int> GetSubdominiosSugeridos(string nombre)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var data = new List<int>();

                        var ids = (from u in ctx.SubdominioEquivalencia
                                   join s in ctx.Subdominio on u.SubdominioId equals s.SubdominioId
                                   where u.EquivalenciaNombre == nombre
                                   && u.Activo && s.Activo
                                   select u.SubdominioId).ToList();

                        if (ids != null)
                            data = ids;

                        return data;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                log.Error(ex.Message, ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaNoRegistradaDTO
                    , "Error en el metodo: List<int> GetSubdominiosSugeridos(string nombre)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {                
                log.Error(ex.Message, ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaNoRegistradaDTO
                    , "Error en el metodo: List<int> GetSubdominiosSugeridos(string nombre)"
                    , new object[] { null });
            }
        }

        public override List<TecnologiaNoRegistradaDTO> GetTecnologiaNoRegistradaByEquipoId(int equipoId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                var paramProyeccion1 = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES");
                var proyeccionMeses1 = int.Parse(paramProyeccion1 != null ? paramProyeccion1.Valor : "12");
                var paramProyeccion2 = ServiceManager<ParametroDAO>.Provider.ObtenerParametro("NRO_MESES_PROYECCIONES_2");
                var proyeccionMeses2 = int.Parse(paramProyeccion2 != null ? paramProyeccion2.Valor : "24");

                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var registros = (from a in ctx.TecnologiaNoRegistrada
                                             //join te in ctx.TipoEquipo on a.TipoEquipoId equals te.TipoEquipoId
                                         where a.EquipoId == equipoId
                                         && a.Activo && a.FlagAsociado==false
                                         //group new { a, te } by new { a.Aplicacion, a.Equipo, te.Nombre } into grp
                                         select new TecnologiaNoRegistradaDTO()
                                         {
                                             Aplicacion = a.Aplicacion,
                                             FechaEscaneo = a.FechaEscaneo,
                                             FechaFinSoporte = a.FechaFinSoporte,
                                             Obsoleto = a.Obsoleto,
                                             Meses = proyeccionMeses1,
                                             IndicadorMeses1 = proyeccionMeses1,
                                             IndicadorMeses2 = proyeccionMeses2
                                         }).OrderBy(sortName + " " + sortOrder);

                        totalRows = registros.Count();
                        var resultado = registros.Skip((pageNumber - 1) * pageSize).Take(pageSize);
                        return resultado.ToList();
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                log.Error(ex.Message, ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaNoRegistradaDTO
                    , "Error en el metodo: List<TecnologiaNoRegistradaDTO> GetTecnologiaNoRegistradaByEquipoId(int equipoId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);

                throw new CVTException(CVTExceptionIds.ErrorTecnologiaNoRegistradaDTO
                    , "Error en el metodo: List<TecnologiaNoRegistradaDTO> GetTecnologiaNoRegistradaByEquipoId(int equipoId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocomplete> GetTecnologiasUnicas()
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var ids = (from u in ctx.TecnologiaNoRegistrada
                                   where !u.FlagAsociado && u.Activo
                                   select new CustomAutocomplete()
                                   {
                                       Id = u.ClasificacionSugerida ?? "-",
                                       Descripcion = u.ClasificacionSugerida ?? "-",
                                       value = u.ClasificacionSugerida ?? "-"
                                   }).Distinct().ToList();

                        return ids;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                log.Error(ex.Message, ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaNoRegistradaDTO
                    , "Error en el metodo: List<int> GetTecnologiasUnicas()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaNoRegistradaDTO
                    , "Error en el metodo: List<int> GetTecnologiasUnicas()"
                    , new object[] { null });
            }
        }

        public override List<TecnologiaNoRegistradaDTO> GetTecNoReg(string filtro, int tipoEquipoId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                totalRows = 0;
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var registros = (from u in ctx.TecnologiaNoRegistrada
                                             //join u2 in ctx.Equipo on u.EquipoId equals u2.EquipoId
                                         join u3 in ctx.TipoEquipo on u.TipoEquipoId equals u3.TipoEquipoId
                                         where (
                                         u.Aplicacion.ToUpper().Contains(filtro.ToUpper())
                                         || u.Equipo.ToUpper().Contains(filtro.ToUpper())
                                         || string.IsNullOrEmpty(filtro)
                                         || u.ClasificacionSugerida.ToUpper().Contains(filtro.ToUpper()))
                                         && (u.TipoEquipoId == (tipoEquipoId == -1 ? u.TipoEquipoId : tipoEquipoId))
                                         && !u.FlagAsociado
                                         select new TecnologiaNoRegistradaDTO()
                                         {
                                             Id = u.TecnologiaNoRegistradaId,
                                             Aplicacion = u.Aplicacion,
                                             Equipo = u.Equipo,
                                             EquipoId = u.EquipoId,
                                             TipoEquipoId = u3.TipoEquipoId,
                                             ClasificacionSugerida = u.ClasificacionSugerida,
                                             Activo = u.Activo,
                                             UsuarioCreacion = u.UsuarioCreacion,
                                             FechaCreacion = u.FechaCreacion,
                                             FechaModificacion = u.FechaModificacion,
                                             UsuarioModificacion = u.UsuarioModificacion,
                                             TipoEquipoToString = u3.Nombre
                                             //FechaEscaneo = u.FechaEscaneo.value
                                         });

                        //var registros = (from u in ctx.TecnologiaNoRegistrada
                        //                 join u3 in ctx.TipoEquipo on u.TipoEquipoId equals u3.TipoEquipoId
                        //                 where (
                        //                 u.Aplicacion.ToUpper().Contains(filtro.ToUpper())
                        //                 || u.Equipo.ToUpper().Contains(filtro.ToUpper())
                        //                 || string.IsNullOrEmpty(filtro)
                        //                 || u.ClasificacionSugerida.ToUpper().Contains(filtro.ToUpper()))
                        //                 && (u.TipoEquipoId == tipoEquipoId || tipoEquipoId == -1)
                        //                 && !u.FlagAsociado
                        //                 group new { u, u3 } by new
                        //                 {
                        //                     //u.TecnologiaNoRegistradaId,
                        //                     u.Aplicacion,
                        //                     u.Equipo,
                        //                     u.EquipoId,
                        //                     u3.TipoEquipoId,
                        //                     u3.Nombre,
                        //                     u.ClasificacionSugerida//,
                        //                     //u.FechaCreacion
                        //                 } into grp
                        //                 select new TecnologiaNoRegistradaDTO()
                        //                 {
                        //                     //Id = u.TecnologiaNoRegistradaId,
                        //                     Aplicacion = grp.Key.Aplicacion,
                        //                     Equipo = grp.Key.Equipo,
                        //                     EquipoId = grp.Key.EquipoId,
                        //                     TipoEquipoId = grp.Key.TipoEquipoId,
                        //                     ClasificacionSugerida = grp.Key.ClasificacionSugerida,
                        //                     TipoEquipoToString = grp.Key.Nombre//,
                        //                     //FechaCreacion = grp.Key.FechaCreacion
                        //                 }).ToArray();

                        totalRows = registros.Count();
                        registros = registros.OrderBy(sortName + " " + sortOrder);
                        var resultado = registros.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                        return resultado;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                log.Error(ex.Message, ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaNoRegistradaDTO
                    , "Error en el metodo: List<TecnologiaNoRegistradaDTO> GetTecNoReg(string filtro, int tipoEquipoId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);                
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaNoRegistradaDTO
                    , "Error en el metodo: List<TecnologiaNoRegistradaDTO> GetTecNoReg(string filtro, int tipoEquipoId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<TecnologiaNoRegistradaDTO> GetTecNoRegSP(string filtro, string equivalencia, int tipoEquipoId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            totalRows = 0;
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<TecnologiaNoRegistradaDTO>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[CVT].[USP_VerTecnologiasNoRegistradas]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@tecnologia", filtro));
                        comando.Parameters.Add(new SqlParameter("@clasificacion", equivalencia));
                        comando.Parameters.Add(new SqlParameter("@tipo", tipoEquipoId));                        
                        comando.Parameters.Add(new SqlParameter("@PageSize", pageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pageNumber));
                        comando.Parameters.Add(new SqlParameter("@OrderBy", sortName));
                        comando.Parameters.Add(new SqlParameter("@OrderByDirection", sortOrder));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new TecnologiaNoRegistradaDTO()
                            {
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),
                                Aplicacion = reader.IsDBNull(reader.GetOrdinal("Aplicacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("Aplicacion")),
                                ClasificacionSugerida = reader.IsDBNull(reader.GetOrdinal("ClasificacionSugerida")) ? string.Empty : reader.GetString(reader.GetOrdinal("ClasificacionSugerida")),
                                TipoEquipoToString = reader.IsDBNull(reader.GetOrdinal("Tipo")) ? string.Empty : reader.GetString(reader.GetOrdinal("Tipo")),
                                Total = reader.IsDBNull(reader.GetOrdinal("Total")) ? 0 : reader.GetInt32(reader.GetOrdinal("Total")),
                                FechaFinSoporte = reader.IsDBNull(reader.GetOrdinal("FechaFinSoporte")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaFinSoporte")),
                                FechaFinSoporteExtendido = reader.IsDBNull(reader.GetOrdinal("FechaFinSoporteExtendido")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaFinSoporteExtendido")),
                                FechaCreacion = reader.IsDBNull(reader.GetOrdinal("FechaCreacion")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("FechaCreacion")),
                                UsuarioCreacion = reader.IsDBNull(reader.GetOrdinal("UsuarioCreacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("UsuarioCreacion"))
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
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetAplicacion()"
                    , new object[] { null });
            }
        }

        public override List<TecnologiaNoRegistradaVistaDTO> GetTecNoRegVista(string tecnologia, int tipoEquipoId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            totalRows = 0;
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<TecnologiaNoRegistradaVistaDTO>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[CVT].[USP_VerTecnologiasNoRegistradasEnriquecidaVista]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@Tecnologia", tecnologia));
                        comando.Parameters.Add(new SqlParameter("@Tipo", tipoEquipoId));
                        comando.Parameters.Add(new SqlParameter("@PageSize", pageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pageNumber));
                        comando.Parameters.Add(new SqlParameter("@OrderBy", sortName));
                        comando.Parameters.Add(new SqlParameter("@OrderByDirection", sortOrder));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new TecnologiaNoRegistradaVistaDTO()
                            {
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),
                                Tecnologia = reader.IsDBNull(reader.GetOrdinal("Tecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("Tecnologia")),
                                Producto = reader.IsDBNull(reader.GetOrdinal("Producto")) ? string.Empty : reader.GetString(reader.GetOrdinal("Producto")),
                                Dominio = reader.IsDBNull(reader.GetOrdinal("Dominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Dominio")),
                                Subdominio = reader.IsDBNull(reader.GetOrdinal("Subdominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Subdominio")),
                                TribuCoe = reader.IsDBNull(reader.GetOrdinal("TribuCoe")) ? string.Empty : reader.GetString(reader.GetOrdinal("TribuCoe")),
                                Squad = reader.IsDBNull(reader.GetOrdinal("Squad")) ? string.Empty : reader.GetString(reader.GetOrdinal("Squad")),
                                Owner = reader.IsDBNull(reader.GetOrdinal("Owner")) ? string.Empty : reader.GetString(reader.GetOrdinal("Owner")),
                                MatriculaOwner = reader.IsDBNull(reader.GetOrdinal("Matricula")) ? string.Empty : reader.GetString(reader.GetOrdinal("Matricula")),
                                Correo = reader.IsDBNull(reader.GetOrdinal("Correo")) ? string.Empty : reader.GetString(reader.GetOrdinal("Correo")),
                                TipoEnriquecido = reader.IsDBNull(reader.GetOrdinal("TipoEnriquecido")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoEnriquecido")),
                                TecnologiaRecomendada = reader.IsDBNull(reader.GetOrdinal("TecnologiaRecomendada")) ? string.Empty : reader.GetString(reader.GetOrdinal("TecnologiaRecomendada")),
                                TipoEquipoToString = reader.IsDBNull(reader.GetOrdinal("Tipo")) ? string.Empty : reader.GetString(reader.GetOrdinal("Tipo")),
                                Total = reader.IsDBNull(reader.GetOrdinal("Total")) ? 0 : reader.GetInt32(reader.GetOrdinal("Total")),
                                FechaFinSoporte = reader.IsDBNull(reader.GetOrdinal("FechaFinSoporte")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaFinSoporte")),
                                FechaFinSoporteExtendido = reader.IsDBNull(reader.GetOrdinal("FechaFinSoporteExtendido")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaFinSoporteExtendido")),
                                //FechaCreacion = reader.IsDBNull(reader.GetOrdinal("FechaCreacion")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("FechaCreacion")),
                                //UsuarioCreacion = reader.IsDBNull(reader.GetOrdinal("UsuarioCreacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("UsuarioCreacion"))
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
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetAplicacion()"
                    , new object[] { null });
            }
        }

        public override List<TecnologiaNoRegistradaVistaDTO> GetTecNoRegSP_Detalle(string tecnologia, int tipoEquipoId)
        {
            //totalRows = 0;
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<TecnologiaNoRegistradaVistaDTO>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_VerTecnologiasNoRegistradasEnriquecidaDetalladoVista]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@Tecnologia", tecnologia));
                        comando.Parameters.Add(new SqlParameter("@Tipo", tipoEquipoId));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new TecnologiaNoRegistradaVistaDTO()
                            {
                                //TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),
                                Tecnologia = reader.IsDBNull(reader.GetOrdinal("Tecnologia")) ? string.Empty : reader.GetString(reader.GetOrdinal("Tecnologia")),
                                Producto = reader.IsDBNull(reader.GetOrdinal("Producto")) ? string.Empty : reader.GetString(reader.GetOrdinal("Producto")),
                                Dominio = reader.IsDBNull(reader.GetOrdinal("Dominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Dominio")),
                                Subdominio = reader.IsDBNull(reader.GetOrdinal("Subdominio")) ? string.Empty : reader.GetString(reader.GetOrdinal("Subdominio")),
                                TribuCoe = reader.IsDBNull(reader.GetOrdinal("TribuCoe")) ? string.Empty : reader.GetString(reader.GetOrdinal("TribuCoe")),
                                Squad = reader.IsDBNull(reader.GetOrdinal("Squad")) ? string.Empty : reader.GetString(reader.GetOrdinal("Squad")),
                                Owner = reader.IsDBNull(reader.GetOrdinal("Owner")) ? string.Empty : reader.GetString(reader.GetOrdinal("Owner")),
                                MatriculaOwner = reader.IsDBNull(reader.GetOrdinal("Matricula")) ? string.Empty : reader.GetString(reader.GetOrdinal("Matricula")),
                                Correo = reader.IsDBNull(reader.GetOrdinal("Correo")) ? string.Empty : reader.GetString(reader.GetOrdinal("Correo")),
                                TipoEnriquecido = reader.IsDBNull(reader.GetOrdinal("TipoEnriquecido")) ? string.Empty : reader.GetString(reader.GetOrdinal("TipoEnriquecido")),
                                TecnologiaRecomendada = reader.IsDBNull(reader.GetOrdinal("TecnologiaRecomendada")) ? string.Empty : reader.GetString(reader.GetOrdinal("TecnologiaRecomendada")),
                                Equipo = reader.IsDBNull(reader.GetOrdinal("Equipo")) ? string.Empty : reader.GetString(reader.GetOrdinal("Equipo")),
                                TipoEquipoToString = reader.IsDBNull(reader.GetOrdinal("Tipo")) ? string.Empty : reader.GetString(reader.GetOrdinal("Tipo")),
                                //Total = reader.IsDBNull(reader.GetOrdinal("Total")) ? 0 : reader.GetInt32(reader.GetOrdinal("Total")),
                                FechaFinSoporte = reader.IsDBNull(reader.GetOrdinal("FechaFinSoporte")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaFinSoporte")),
                                FechaFinSoporteExtendido = reader.IsDBNull(reader.GetOrdinal("FechaFinSoporteExtendido")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaFinSoporteExtendido")),
                                //FechaCreacion = reader.IsDBNull(reader.GetOrdinal("FechaCreacion")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("FechaCreacion")),
                                //UsuarioCreacion = reader.IsDBNull(reader.GetOrdinal("UsuarioCreacion")) ? string.Empty : reader.GetString(reader.GetOrdinal("UsuarioCreacion"))
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
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: List<TecnologiaNoRegistradaDTO> GetTecNoRegSP_Detalle(string tecnologia, int tipoEquipoId, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<TecnologiaDTO> GetTecSugeridas(string filtro, List<int> idSubSugeridos, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
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
                                         where (u.ClaveTecnologia.ToUpper().Contains(filtro.ToUpper())
                                         || u.Descripcion.ToUpper().Contains(filtro.ToUpper())
                                         || string.IsNullOrEmpty(filtro))
                                         && u.Activo && s.Activo && d.Activo
                                         && idSubSugeridos.Contains(u.SubdominioId)
                                         && u.EstadoTecnologia == (int)EstadoTecnologia.Aprobado
                                         select new TecnologiaDTO()
                                         {
                                             Id = u.TecnologiaId,
                                             Nombre = u.Nombre,
                                             SubdominioId = u.SubdominioId,
                                             SubdominioNomb = s.Nombre,
                                             DominioId = d.DominioId,
                                             DominioNomb = d.Nombre,
                                             Activo = u.Activo,
                                             ClaveTecnologia = u.ClaveTecnologia
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
                log.ErrorEntity(ex);
                log.Error(ex.Message, ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaNoRegistradaDTO
                    , "Error en el metodo: List<TecnologiaDTO> GetTecSugeridas(string filtro, List<int> idSubSugeridos, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaNoRegistradaDTO
                    , "Error en el metodo: List<TecnologiaDTO> GetTecSugeridas(string filtro, List<int> idSubSugeridos, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<EquipoNoRegistradoDto> GetEquipoNoRegSP (string nombre, int motivo, int origen, int? flagAprobado, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            totalRows = 0;
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<EquipoNoRegistradoDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[CVT].[USP_VerEquipossNoRegistrados]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@nombre", ((object)nombre ?? DBNull.Value)));
                        comando.Parameters.Add(new SqlParameter("@motivo", motivo));
                        comando.Parameters.Add(new SqlParameter("@origen", origen));                        
                        comando.Parameters.Add(new SqlParameter("@flagAprobado", ((object)flagAprobado ?? DBNull.Value)));                        
                        comando.Parameters.Add(new SqlParameter("@PageSize", pageSize));
                        comando.Parameters.Add(new SqlParameter("@PageNumber", pageNumber));
                        comando.Parameters.Add(new SqlParameter("@OrderBy", sortName));
                        comando.Parameters.Add(new SqlParameter("@OrderByDirection", sortOrder));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new EquipoNoRegistradoDto()
                            {
                                EquipoNoRegistradoId = reader.GetData<int>("EquipoNoRegistradoId"),
                                TotalFilas = reader.IsDBNull(reader.GetOrdinal("TotalFilas")) ? 0 : reader.GetInt32(reader.GetOrdinal("TotalFilas")),
                                EquipoId = reader.IsDBNull(reader.GetOrdinal("EquipoId")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("EquipoId")),
                                Estado = reader.IsDBNull(reader.GetOrdinal("Estado")) ? 1 : reader.GetInt32(reader.GetOrdinal("Estado")),
                                FechaDescubrimiento = reader.IsDBNull(reader.GetOrdinal("FechaDescubrimiento")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("FechaDescubrimiento")),
                                FechaRegistro = reader.IsDBNull(reader.GetOrdinal("FechaRegistro")) ? DateTime.Now : reader.GetDateTime(reader.GetOrdinal("FechaRegistro")),
                                Fuente = reader.IsDBNull(reader.GetOrdinal("Fuente")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("Fuente")),
                                IPEquipo = reader.IsDBNull(reader.GetOrdinal("IPEquipo")) ? string.Empty : reader.GetString(reader.GetOrdinal("IPEquipo")),
                                Motivo = reader.IsDBNull(reader.GetOrdinal("Motivo")) ? 1 : reader.GetInt32(reader.GetOrdinal("Motivo")),
                                NombreEquipo = reader.IsDBNull(reader.GetOrdinal("NombreEquipo")) ? string.Empty : reader.GetString(reader.GetOrdinal("NombreEquipo")),
                                Origen = reader.IsDBNull(reader.GetOrdinal("Origen")) ? 1 : reader.GetInt32(reader.GetOrdinal("Origen")),
                                SistemaOperativo = reader.IsDBNull(reader.GetOrdinal("SistemaOperativo")) ? string.Empty : reader.GetString(reader.GetOrdinal("SistemaOperativo")),
                                FlagAprobado = reader.GetData<bool?>("FlagAprobado")
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
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetAplicacion()"
                    , new object[] { null });
            }
        }

        public override List<TecnologiaNoRegistradaQualysDto> GetTecnologiaNoRegQualysXEquipoSP(int equipoId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            totalRows = 0;
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<TecnologiaNoRegistradaQualysDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[CVT].[usp_equipo_vulnerabilidades_listar]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@equipoId", equipoId));
                        comando.Parameters.Add(new SqlParameter("@pageSize", pageSize));
                        comando.Parameters.Add(new SqlParameter("@pageNumber", pageNumber));
                        comando.Parameters.Add(new SqlParameter("@sortName", sortName));
                        comando.Parameters.Add(new SqlParameter("@sortOrder", sortOrder));

                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new TecnologiaNoRegistradaQualysDto()
                            {
                                Id = reader.GetData<int>("Id"),
                                QualyId = reader.GetData<int?>("QualyId"),
                                ProductoId = reader.GetData<int?>("ProductoId"),
                                ProductoStr = reader.GetData<string>("ProductoStr"),                                
                                Title = reader.GetData<string>("Titulo"),
                                VulnStatusId = reader.GetData<int>("Vuln_Status"),
                                Solucion = reader.GetData<string>("Solucion"),
                                NivelSeveridad = reader.GetData<int>("NivelSeveridad"),
                                Categoria = reader.GetData<string>("Categoria"),
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
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetAplicacion()"
                    , new object[] { null });
            }
        }

        public override int GetCantidadVulnerabilidadesXEquipoSP(int equipoId)
        {
            try
            {
                int cantidad = 0;
                var cadenaConexion = Constantes.CadenaConexion;
                var lista = new List<TecnologiaNoRegistradaQualysDto>();
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[CVT].[usp_equipo_cantidadvulnerabilidades]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        comando.Parameters.Add(new SqlParameter("@equipoId", equipoId));

                        cantidad = (int)comando.ExecuteScalar();
                    }

                    return cantidad;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: GetAplicacion()"
                    , new object[] { null });
            }
        }        
    }
}
