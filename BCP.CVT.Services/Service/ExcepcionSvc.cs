using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.Services.Interface;
using BCP.CVT.Services.ModelDB;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Data.Entity;

namespace BCP.CVT.Services.Service
{
    class ExcepcionSvc : ExcepcionDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public override int AddOrEdit(ExcepcionDTO objRegistro)
        {
            DbContextTransaction transaction = null;
            try
            {
                int ID = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    using (transaction = ctx.Database.BeginTransaction())
                    {
                        if (objRegistro.Id == 0)
                        {
                            var entidad = new Excepcion()
                            {
                                TipoExcepcionId = objRegistro.TipoExcepcionId,
                                TecnologiaId = objRegistro.TecnologiaId,
                                CodigoAPT = objRegistro.CodigoAPT,
                                TipoRiesgoId = objRegistro.TipoRiesgoId,
                                FechaFinExcepcion = objRegistro.FechaFinExcepcion,
                                UrlInformacion = objRegistro.UrlInformacion,
                                Comentario = objRegistro.Comentario,
                                FlagActivo = objRegistro.Activo,
                                FechaCreacion = DateTime.Now,
                                UsuarioCreacion = objRegistro.UsuarioCreacion
                            };
                            ctx.Excepcion.Add(entidad);
                            ctx.SaveChanges();

                            ID = entidad.ExcepcionId;
                        }
                        else
                        {
                            var entidad = (from u in ctx.Excepcion
                                           where u.ExcepcionId == objRegistro.Id && u.FlagActivo
                                           select u).FirstOrDefault();
                            if (entidad != null)
                            {
                                entidad.TipoExcepcionId = objRegistro.TipoExcepcionId;
                                entidad.CodigoAPT = objRegistro.CodigoAPT;
                                entidad.TecnologiaId = objRegistro.TecnologiaId;
                                entidad.TipoRiesgoId = objRegistro.TipoRiesgoId;
                                entidad.FechaFinExcepcion = objRegistro.FechaFinExcepcion;
                                entidad.UrlInformacion = objRegistro.UrlInformacion;
                                entidad.Comentario = objRegistro.Comentario;
                                entidad.FechaModificacion = DateTime.Now;
                                entidad.UsuarioModificacion = objRegistro.UsuarioModificacion;
                                ctx.SaveChanges();
                                ID = entidad.ExcepcionId;
                            }
                        }

                        if (objRegistro.TipoExcepcionId == (int)ETipoExcepcion.Tipo)
                        {
                            var entidad = (from u in ctx.Tecnologia
                                           where u.TecnologiaId == objRegistro.TecnologiaId
                                           select u).FirstOrDefault();
                            if (entidad != null)
                            {
                                entidad.TipoTecnologia = Utilitarios.Get<int>("Tipo.EstandarRestringido.Id");
                                entidad.FechaModificacion = DateTime.Now;
                                entidad.UsuarioModificacion = objRegistro.UsuarioModificacion;
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
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorExcepcionDTO
                    , "Error en el metodo: int AddOrEdit(ExcepcionDTO objRegistro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorExcepcionDTO
                    , "Error en el metodo: int AddOrEdit(ExcepcionDTO objRegistro)"
                    , new object[] { null });
            }
        }

        public override bool DeleteById(int id)
        {
            try
            {
                bool estado = false;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.Excepcion
                                   where u.ExcepcionId == id
                                   select u).FirstOrDefault();
                    if (entidad != null)
                    {
                        entidad.FlagActivo = false;
                        estado = true;
                        ctx.SaveChanges();
                    }
                }
                return estado;
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorExcepcionDTO
                    , "Error en el metodo: bool DeleteById(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorExcepcionDTO
                    , "Error en el metodo: bool DeleteById(int id)"
                    , new object[] { null });
            }
        }

        public override ExcepcionDTO GetById(int id)
        {
            try
            {
                ExcepcionDTO entidad = null;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    entidad = (from u in ctx.Excepcion
                               join a in ctx.Aplicacion on u.CodigoAPT equals a.CodigoAPT
                               join tp in ctx.TablaProcedencia on (int)ETablaProcedencia.CVT_Excepcion equals tp.CodigoInterno
                               join f in ctx.ArchivosCVT on new { Id = u.ExcepcionId.ToString(), TablaProcedenciaId = tp.TablaProcedenciaId, Activo = true } equals new { Id = f.EntidadId, TablaProcedenciaId = f.TablaProcedenciaId, Activo = f.Activo } into jl
                               from f in jl.DefaultIfEmpty()
                               join t in ctx.Tecnologia on u.TecnologiaId equals t.TecnologiaId
                               where u.ExcepcionId == id && u.FlagActivo
                               select new ExcepcionDTO()
                               {
                                   Id = id,
                                   TipoExcepcionId = u.TipoExcepcionId,
                                   CodigoAPT = u.CodigoAPT,
                                   AplicacionStr = a.Nombre,
                                   TecnologiaId = u.TecnologiaId,
                                   TecnologiaStr = t.Nombre,
                                   TipoRiesgoId = u.TipoRiesgoId,
                                   FechaFinExcepcion = u.FechaFinExcepcion,
                                   UrlInformacion = u.UrlInformacion,
                                   Comentario = u.Comentario,
                                   ArchivoId = (f == null) ? (int?)null : f.ArchivoId,
                                   ArchivoStr = (f == null) ? null : f.Nombre,
                               }).FirstOrDefault();
                }
                return entidad;
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorExcepcionDTO
                    , "Error en el metodo: ExcepcionDTO GetById(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorExcepcionDTO
                    , "Error en el metodo: ExcepcionDTO GetById(int id)"
                    , new object[] { null });
            }
        }

        public override List<ExcepcionDTO> GetExcepcionXTecnologiaId(int tipoExcepcionId, int tecnologiaId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                totalRows = 0;
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var registros = (from u in ctx.Excepcion
                                         join t in ctx.Tecnologia on u.TecnologiaId equals t.TecnologiaId
                                         join s in ctx.Subdominio on t.SubdominioId equals s.SubdominioId
                                         join d in ctx.Dominio on s.DominioId equals d.DominioId
                                         join a in ctx.Aplicacion on new { CodigoAPT = u.CodigoAPT, Activo = true } equals new { CodigoAPT = a.CodigoAPT, Activo = a.FlagActivo }
                                         where u.FlagActivo && t.Activo && s.Activo && d.Activo
                                         && u.TecnologiaId == tecnologiaId && u.TipoExcepcionId == tipoExcepcionId
                                         select new ExcepcionDTO()
                                         {
                                             Id = u.ExcepcionId,
                                             TipoExcepcionId = u.TipoExcepcionId,
                                             CodigoAPT = a.CodigoAPT,
                                             NombreAplicacion = a.Nombre,
                                             AplicacionStr = a.Nombre,
                                             TecnologiaId = t.TecnologiaId,
                                             TecnologiaStr = t.ClaveTecnologia,
                                             DominioStr = d.Nombre,
                                             SubdominioStr = s.Nombre,
                                             TipoRiesgoId = u.TipoRiesgoId,
                                             FechaFinExcepcion = u.FechaFinExcepcion,
                                             UrlInformacion = u.UrlInformacion,
                                             Comentario = u.Comentario,
                                             Activo = u.FlagActivo,
                                             UsuarioCreacion = u.UsuarioCreacion,
                                             FechaCreacion = u.FechaCreacion
                                         }).OrderBy(sortName + " " + sortOrder);

                        totalRows = registros.Count();
                        var resultado = registros.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                        return resultado;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorExcepcionDTO
                    , "Error en el metodo: List<ExcepcionDTO> GetExcepcionXTecnologiaId(int tipoExcepcionId, int tecnologiaId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorExcepcionDTO
                    , "Error en el metodo: List<ExcepcionDTO> GetExcepcionXTecnologiaId(int tipoExcepcionId, int tecnologiaId, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }
        public override List<ExcepcionDTO> GetList(string filtro, string username, int pageNumber, int pageSize, string sortName, string sortOrder, string codigoAPT, int? tecnologiaId, int? equipoId, int? tipoExcepcionId, out int totalRows)
        {
            try
            {
                totalRows = 0;
                List<ExcepcionDTO> resultado = null;
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var registros = (from u in ctx.Excepcion
                                         join t in ctx.Tecnologia on u.TecnologiaId equals t.TecnologiaId
                                         join s in ctx.Subdominio on t.SubdominioId equals s.SubdominioId
                                         join d in ctx.Dominio on s.DominioId equals d.DominioId
                                         join a in ctx.Aplicacion on new { CodigoAPT = u.CodigoAPT, Activo = true } equals new { CodigoAPT = a.CodigoAPT, Activo = a.FlagActivo }
                                         where u.FlagActivo && t.Activo && s.Activo && d.Activo
                                         && (u.TipoExcepcionId == tipoExcepcionId || tipoExcepcionId == null)
                                         && (u.CodigoAPT == codigoAPT || string.IsNullOrEmpty(codigoAPT))
                                         && (u.TecnologiaId == tecnologiaId || tecnologiaId == null)
                                         && (u.UsuarioCreacion == username || string.IsNullOrEmpty(username))
                                         select new ExcepcionDTO()
                                         {
                                             Id = u.ExcepcionId,
                                             TipoExcepcionId = u.TipoExcepcionId,
                                             CodigoAPT = a.CodigoAPT,
                                             NombreAplicacion = a.Nombre,
                                             AplicacionStr = a.Nombre,
                                             TecnologiaId = t.TecnologiaId,
                                             TecnologiaStr = t.ClaveTecnologia,
                                             DominioStr = d.Nombre,
                                             SubdominioStr = s.Nombre,
                                             TipoRiesgoId = u.TipoRiesgoId,
                                             FechaFinExcepcion = u.FechaFinExcepcion,
                                             UrlInformacion = u.UrlInformacion,
                                             Comentario = u.Comentario,
                                             Activo = u.FlagActivo,
                                             UsuarioCreacion = u.UsuarioCreacion,
                                             FechaCreacion = u.FechaCreacion
                                         }).OrderBy(sortName + " " + sortOrder);
                        totalRows = registros.Count();
                        resultado = registros.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                    }
                    return resultado;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorExcepcionDTO
                    , "Error en el metodo: int AddOrEdit(ExcepcionDTO objRegistro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorExcepcionDTO
                    , "Error en el metodo: int AddOrEdit(ExcepcionDTO objRegistro)"
                    , new object[] { null });
            }
        }
    }
}
