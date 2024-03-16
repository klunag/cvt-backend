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

namespace BCP.CVT.Services.Service
{
    public class FamiliaSvc : FamiliaDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public override int AddOrEditFamilia(FamiliaDTO objeto)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    if (objeto.Id == 0)
                    {
                        var entidad = new Familia()
                        {
                            FamiliaId = objeto.Id,
                            Activo = objeto.Activo,
                            FechaCreacion = DateTime.Now,
                            Nombre = objeto.Nombre,
                            Descripcion = objeto.Descripcion,
                            Fabricante = objeto.Fabricante,
                        /*
                        FechaFinSoporte = objeto.FechaFinSoporte,
                        FechaExtendida = objeto.FechaExtendida,
                        FechaInterna = objeto.FechaInterna, */
                            Existencia = objeto.Existencia,
                            Facilidad = objeto.Facilidad,
                            Riesgo = objeto.Riesgo,
                            Vulnerabilidad = objeto.Vulnerabilidad,
                            UsuarioCreacion = objeto.UsuarioCreacion
                        };
                        ctx.Familia.Add(entidad);
                        ctx.SaveChanges();

                        return entidad.FamiliaId;
                    }
                    else
                    {
                        var entidad = (from u in ctx.Familia
                                       where u.FamiliaId == objeto.Id
                                       select u).First();
                        if (entidad != null)
                        {
                            entidad.Nombre = objeto.Nombre;
                            entidad.Descripcion = objeto.Descripcion;
                            entidad.Fabricante = objeto.Fabricante;
                            /*
                            entidad.FechaFinSoporte = objeto.FechaFinSoporte;
                            entidad.FechaExtendida = objeto.FechaExtendida;
                            entidad.FechaInterna = objeto.FechaInterna; */
                            entidad.Existencia = objeto.Existencia;
                            entidad.Facilidad = objeto.Facilidad;
                            entidad.Riesgo = objeto.Riesgo;
                            entidad.Vulnerabilidad = objeto.Vulnerabilidad;
                            entidad.Activo = objeto.Activo;
                            entidad.FechaModificacion = DateTime.Now;
                            entidad.UsuarioModificacion = objeto.UsuarioModificacion;

                            ctx.SaveChanges();

                            return entidad.FamiliaId;
                        }
                        else
                            return 0;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorFamiliaDTO
                    , "Error en el metodo: int AddOrEditFamilia(FamiliaDTO objeto)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorFamiliaDTO
                    , "Error en el metodo: int AddOrEditFamilia(FamiliaDTO objeto)"
                    , new object[] { null });
            }
        }

        public override bool CambiarEstado(int id, bool estado, string usuario)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var itemBD = (from u in ctx.Familia
                                  where u.FamiliaId == id
                                  select u).First();

                    if (itemBD != null)
                    {
                        itemBD.FechaModificacion = DateTime.Now;
                        itemBD.UsuarioModificacion = usuario;
                        itemBD.Activo = estado;

                        ctx.SaveChanges();

                        return true;
                    }
                    else
                        return false;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorFamiliaDTO
                    , "Error en el metodo: int bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorFamiliaDTO
                    , "Error en el metodo: int bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
        }

        public override bool ExisteFamiliaById(int? Id, string nombre)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        bool? estado = (from u in ctx.Familia
                                        where u.Activo
                                        && (Id == null || u.FamiliaId == Id)
                                        && (string.IsNullOrEmpty(nombre) || u.Nombre.ToUpper().Equals(nombre.ToUpper()))
                                        select true).FirstOrDefault();

                        return estado == true;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorFamiliaDTO
                    , "Error en el metodo: bool ExisteFamiliaById(int Id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorFamiliaDTO
                    , "Error en el metodo: bool ExisteFamiliaById(int Id)"
                    , new object[] { null });
            }
        }

        public override List<FamiliaDTO> GetAllFamilia()
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var entidad = (from u in ctx.Familia
                                       where u.Activo
                                       orderby u.Nombre
                                       select new FamiliaDTO()
                                       {
                                           Id = u.FamiliaId,
                                           Nombre = u.Nombre,
                                       }).ToList();
                        return entidad;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorFamiliaDTO
                    , "Error en el metodo: List<FamiliaDTO> GetAllFamilia()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorFamiliaDTO
                    , "Error en el metodo: List<FamiliaDTO> GetAllFamilia()"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocomplete> GetAllFamiliaByFiltro(string filtro)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var entidad = (from u in ctx.Familia
                                       where u.Activo
                                       && (string.IsNullOrEmpty(filtro)
                                       || (u.Nombre).ToUpper().Contains(filtro.ToUpper())
                                       || (u.Descripcion).ToUpper().Contains(filtro.ToUpper())
                                       )
                                       orderby u.Nombre
                                       select new CustomAutocomplete()
                                       {
                                           Id = u.FamiliaId.ToString(),
                                           Descripcion = u.Nombre,
                                           value = u.Nombre
                                       }).ToList();

                        return entidad;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorFamiliaDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetAllFamiliaByFiltro(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorFamiliaDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetAllFamiliaByFiltro(string filtro)"
                    , new object[] { null });
            }
        }

        public override List<FamiliaDTO> GetFamilia(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                totalRows = 0;
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var registros = (from u in ctx.Familia
                                         where (u.Nombre.ToUpper().Contains(filtro.ToUpper())
                                         || u.Descripcion.ToUpper().Contains(filtro.ToUpper())
                                         || string.IsNullOrEmpty(filtro))
                                         select new FamiliaDTO()
                                         {
                                             Id = u.FamiliaId,
                                             Nombre = u.Nombre,
                                             Descripcion = u.Descripcion,
                                             /*
                                             FechaFinSoporte = u.FechaFinSoporte.Value,
                                             FechaExtendida = u.FechaExtendida.Value,
                                             FechaInterna = u.FechaInterna.Value, */
                                             Existencia = u.Existencia,
                                             Facilidad = u.Facilidad,
                                             Riesgo = u.Riesgo,
                                             Vulnerabilidad = u.Vulnerabilidad,
                                             Activo = u.Activo,
                                             UsuarioCreacion = u.UsuarioCreacion,
                                             FechaCreacion = u.FechaCreacion,
                                             FechaModificacion = u.FechaModificacion,
                                             UsuarioModificacion = u.UsuarioModificacion,
                                             NumTecAsociadas = (from sq in ctx.Tecnologia
                                                                where (sq.FamiliaId == u.FamiliaId) && sq.EstadoTecnologia == (int)EstadoTecnologia.Aprobado && sq.Activo
                                                                select 1).Count()
                                         }).OrderBy(sortName + " " + sortOrder).ToList();

                        totalRows = registros.Count();
                        var resultado = registros.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                        return resultado;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorFamiliaDTO
                    , "Error en el metodo: List<FamiliaDTO> GetFamilia(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorFamiliaDTO
                    , "Error en el metodo: List<FamiliaDTO> GetFamilia(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override FamiliaDTO GetFamiliaById(int id)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var entidad = (from u in ctx.Familia
                                       where u.FamiliaId == id
                                       select new FamiliaDTO()
                                       {
                                           Id = u.FamiliaId,
                                           Nombre = u.Nombre,
                                           Descripcion = u.Descripcion,
                                           /*FechaFinSoporte = u.FechaFinSoporte.Value,
                                           FechaExtendida = u.FechaExtendida.Value,
                                           FechaInterna = u.FechaInterna.Value, */
                                           Existencia = u.Existencia,
                                           Facilidad = u.Facilidad,
                                           Riesgo = u.Riesgo,
                                           Vulnerabilidad = u.Vulnerabilidad,
                                           Activo = u.Activo,
                                           FechaCreacion = u.FechaCreacion,
                                           UsuarioCreacion = u.UsuarioCreacion,
                                           Fabricante = string.IsNullOrEmpty(u.Fabricante) ? string.Empty : u.Fabricante

                                       }).FirstOrDefault();
                        return entidad;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorFamiliaDTO
                    , "Error en el metodo: FamiliaDTO GetFamiliaById(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorFamiliaDTO
                    , "Error en el metodo: FamiliaDTO GetFamiliaById(int id)"
                    , new object[] { null });
            }
        }

        public override List<TecnologiaDTO> GetTecByFamilia(int id, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            totalRows = 0;
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var entidad = (from u in ctx.Tecnologia
                                       join s in ctx.Subdominio on u.SubdominioId equals s.SubdominioId
                                       join d in ctx.Dominio on s.DominioId equals d.DominioId
                                       join t in ctx.Tipo on u.TipoTecnologia equals t.TipoId
                                       where (u.FamiliaId == id) && s.Activo && d.Activo && u.Activo && t.Activo
                                       && u.EstadoTecnologia == (int)EstadoTecnologia.Aprobado
                                       orderby u.Nombre
                                       select new TecnologiaDTO()
                                       {
                                           Id = u.TecnologiaId,
                                           Nombre = u.Nombre,
                                           ClaveTecnologia = u.ClaveTecnologia,
                                           EstadoId = u.EstadoId,
                                           //TipoTecnologiaId = u.TipoTecnologia,
                                           Descripcion = u.Descripcion,
                                           Activo = u.Activo,
                                           FechaCreacion = u.FechaCreacion,
                                           UsuarioCreacion = u.UsuarioCreacion,
                                           FechaModificacion = u.FechaModificacion,
                                           UsuarioModificacion = u.UsuarioModificacion,
                                           SubdominioNomb = s.Nombre,
                                           DominioNomb = d.Nombre,
                                           TipoTecNomb = t.Nombre
                                       }).OrderBy(sortName + " " + sortOrder);

                        totalRows = entidad.Count();
                        var resultado = entidad.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                        return resultado;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorFamiliaDTO
                    , "Error en el metodo: List<TecnologiaDTO> GetTecByFamilia(int id, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorFamiliaDTO
                    , "Error en el metodo: List<TecnologiaDTO> GetTecByFamilia(int id, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<TecnologiaDTO> GetTecVinculadas(int id, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            totalRows = 0;
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var entidad = (from u in ctx.Tecnologia
                                       join tv in ctx.TecnologiaVinculada on u.TecnologiaId equals tv.TecnologiaId
                                       join x in ctx.Tecnologia on tv.VinculoId equals x.TecnologiaId
                                       join s in ctx.Subdominio on x.SubdominioId equals s.SubdominioId
                                       join d in ctx.Dominio on s.DominioId equals d.DominioId
                                       join t in ctx.Tipo on x.TipoTecnologia equals t.TipoId
                                       where (u.TecnologiaId == id)
                                       && u.Activo && tv.Activo && x.Activo && s.Activo && d.Activo && t.Activo
                                       orderby u.Nombre
                                       select new TecnologiaDTO()
                                       {
                                           Id = tv.TecnologiaVinculadaId,
                                           Nombre = x.Nombre,
                                           ClaveTecnologia = x.ClaveTecnologia,
                                           EstadoId = x.EstadoId,
                                           Descripcion = x.Descripcion,
                                           Activo = x.Activo,
                                           FechaCreacion = x.FechaCreacion,
                                           UsuarioCreacion = x.UsuarioCreacion,
                                           FechaModificacion = x.FechaModificacion,
                                           UsuarioModificacion = x.UsuarioModificacion,
                                           SubdominioNomb = s.Nombre,
                                           DominioNomb = d.Nombre,
                                           TipoTecNomb = t.Nombre
                                       }).OrderBy(sortName + " " + sortOrder);

                        totalRows = entidad.Count();
                        var resultado = entidad.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                        return resultado;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorFamiliaDTO
                    , "Error en el metodo: List<TecnologiaDTO> GetTecVinculadas(int id, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorFamiliaDTO
                    , "Error en el metodo: List<TecnologiaDTO> GetTecVinculadas(int id, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override bool AsociarTecnologiasByFamilia(int tecnologiaId, int familiaId, string usuario)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        //var itemSubdomEq = ctx.SubdominioEquivalencia.FirstOrDefault(x => x.SubdominioId == id && x.EquivalenciaNombre.ToUpper() == equivalencia.ToUpper());
                        var itemTecnologia = (from u in ctx.Tecnologia
                                              where u.TecnologiaId == tecnologiaId && u.Activo
                                              select u).FirstOrDefault();

                        if (itemTecnologia != null)
                        {
                            itemTecnologia.FamiliaId = familiaId;
                            itemTecnologia.UsuarioModificacion = usuario;

                            ctx.SaveChanges();
                            //var entidad = new SubdominioEquivalencia()
                            //{
                            //    SubdominioId = id,
                            //    EquivalenciaNombre = equivalencia,
                            //    Activo = true,
                            //    FechaCreacion = DateTime.Now,
                            //    UsuarioCreacion = usuario
                            //};
                            //ctx.SubdominioEquivalencia.Add(entidad);
                            return true;
                        }
                        else
                        {
                            return false;
                        }

                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorSubdominioDTO
                    , "Error en el metodo: bool AsociarSubdominiosEquivalentes(int id, string equivalencia, string usuario)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new CVTException(CVTExceptionIds.ErrorSubdominioDTO
                    , "Error en el metodo: bool AsociarSubdominiosEquivalentes(int id, string equivalencia, string usuario)"
                    , new object[] { null });
            }
        }

        public override bool ExisteFamiliaByNombre(int? Id, string nombre)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        bool? estado = (from u in ctx.Familia
                                        where u.Activo
                                        && (Id == null || u.FamiliaId != Id)
                                        && (string.IsNullOrEmpty(nombre) || u.Nombre.ToUpper().Equals(nombre.ToUpper()))
                                        select true).FirstOrDefault();

                        return estado == true;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorFamiliaDTO
                    , "Error en el metodo: bool ExisteFamiliaByNombre(int? Id, string nombre)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorFamiliaDTO
                    , "Error en el metodo: bool ExisteFamiliaByNombre(int? Id, string nombre)"
                    , new object[] { null });
            }
        }
    }
}
