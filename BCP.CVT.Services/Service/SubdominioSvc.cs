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
    public class SubdominioSvc : SubdominioDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public override int AddOrEditSubdom(SubdominioDTO objeto)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    if (objeto.Id == 0)
                    {
                        var entidad = new Subdominio()
                        {
                            Activo = objeto.Activo,
                            FechaCreacion = DateTime.Now,
                            Nombre = objeto.Nombre,
                            Descripcion = objeto.Descripcion,
                            Peso = objeto.Peso,
                            Dueno = objeto.MatriculaDueno,
                            CalObs = objeto.CalculoObs,
                            UsuarioCreacion = objeto.UsuarioCreacion,
                            DominioId = objeto.DomIdAsociado,
                            SubdominioId = objeto.Id,
                            UsuarioAsociadoPor = objeto.UsuarioCreacion, //Usuario Asociado
                            FechaAsociacion = DateTime.Now,
                            FlagIsVisible = objeto.IsVisible
                        };
                        ctx.Subdominio.Add(entidad);
                        ctx.SaveChanges();

                        return entidad.SubdominioId;
                    }
                    else
                    {
                        //var entidad = (from u in ctx.Subdominio
                        //               where u.SubdominioId == objeto.Id
                        //               select u).First();

                        var entidad = ctx.Subdominio.FirstOrDefault(x => x.SubdominioId == objeto.Id);
                        if (entidad != null)
                        {
                            entidad.DominioId = objeto.DomIdAsociado;
                            entidad.Nombre = objeto.Nombre;
                            entidad.Descripcion = objeto.Descripcion;
                            entidad.Peso = objeto.Peso;
                            entidad.Dueno = objeto.MatriculaDueno;
                            entidad.CalObs = objeto.CalculoObs;
                            entidad.FechaModificacion = DateTime.Now;
                            entidad.Activo = objeto.Activo;
                            entidad.UsuarioModificacion = objeto.UsuarioModificacion;
                            entidad.FlagIsVisible = objeto.IsVisible;

                            ctx.SaveChanges();

                            return entidad.SubdominioId;
                        }
                        else
                            return 0;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorSubdominioDTO
                    , "Error en el metodo: int AddOrEditSubdom(SubdominioDTO objeto)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorSubdominioDTO
                    , "Error en el metodo: int AddOrEditSubdom(SubdominioDTO objeto)"
                    , new object[] { null });
            }
        }

        public override bool AsociarSubdominiosEquivalentes(int id, string equivalencia, string usuario)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var itemSubdomEq = ctx.SubdominioEquivalencia.FirstOrDefault(x => x.SubdominioId == id && x.EquivalenciaNombre.ToUpper() == equivalencia.ToUpper());
                    if (itemSubdomEq == null)
                    {
                        var entidad = new SubdominioEquivalencia()
                        {
                            SubdominioId = id,
                            EquivalenciaNombre = equivalencia,
                            Activo = true,
                            FechaCreacion = DateTime.Now,
                            UsuarioCreacion = usuario
                        };
                        ctx.SubdominioEquivalencia.Add(entidad);
                        ctx.SaveChanges();
                    }
                    return true;
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

        public override bool CambiarEstado(int id, bool estado, string usuario)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var itemBD = (from u in ctx.Subdominio
                                  where u.SubdominioId == id
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
                throw new CVTException(CVTExceptionIds.ErrorSubdominioDTO
                    , "Error en el metodo: bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new CVTException(CVTExceptionIds.ErrorSubdominioDTO
                    , "Error en el metodo: bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
        }

        public override List<SubdominioDTO> GetSubdom(int dominioId, string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                totalRows = 0;
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var registros = (from u in ctx.Subdominio
                                         join s in ctx.Dominio on u.DominioId equals s.DominioId
                                         where u.DominioId == (dominioId == -1 ? u.DominioId : dominioId) &&
                                         (u.Nombre.ToUpper().Contains(nombre.ToUpper()) || string.IsNullOrEmpty(nombre))
                                         && s.Activo
                                         select new SubdominioDTO()
                                         {
                                             Id = u.SubdominioId,
                                             Nombre = u.Nombre,
                                             Descripcion = u.Descripcion,
                                             Peso = u.Peso,
                                             MatriculaDueno = u.Dueno,
                                             CalculoObs = u.CalObs,
                                             Activo = u.Activo,
                                             UsuarioCreacion = u.UsuarioCreacion,
                                             FechaCreacion = u.FechaCreacion,
                                             FechaModificacion = u.FechaModificacion,
                                             UsuarioModificacion = u.UsuarioModificacion,
                                             DomIdAsociado = u.DominioId,
                                             DomNomAsociado = s.Nombre,
                                             NumTecAsociadas = (from sq in ctx.Tecnologia
                                                                where (sq.SubdominioId == u.SubdominioId) && sq.Activo == true
                                                                select 1).Count()
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
                throw new CVTException(CVTExceptionIds.ErrorSubdominioDTO
                    , "Error en el metodo: List<SubdominioDTO> GetSubdom(int dominioId, string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new CVTException(CVTExceptionIds.ErrorSubdominioDTO
                    , "Error en el metodo: List<SubdominioDTO> GetSubdom(int dominioId, string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override SubdominioDTO GetSubdomById(int id)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var entidad = (from u in ctx.Subdominio
                                       where u.SubdominioId == id
                                       select new SubdominioDTO()
                                       {
                                           Activo = u.Activo,
                                           Nombre = u.Nombre,
                                           Descripcion = u.Descripcion,
                                           Peso = u.Peso,
                                           MatriculaDueno = u.Dueno,
                                           CalculoObs = u.CalObs,
                                           Id = u.SubdominioId,
                                           FechaCreacion = u.FechaCreacion,
                                           UsuarioCreacion = u.UsuarioCreacion,
                                           DomIdAsociado = u.DominioId,
                                           IsVisible = u.FlagIsVisible
                                       }).FirstOrDefault();
                        return entidad;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorSubdominioDTO
                    , "Error en el metodo: SubdominioDTO GetSubdomById(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new CVTException(CVTExceptionIds.ErrorSubdominioDTO
                    , "Error en el metodo: SubdominioDTO GetSubdomById(int id)"
                    , new object[] { null });
            }
        }

        public override List<SubdominioDTO> GetSubdominio()
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var entidad = (from u in ctx.Subdominio
                                       join d in ctx.Dominio on u.DominioId equals d.DominioId
                                       where u.Activo
                                       orderby u.Nombre
                                       select new SubdominioDTO()
                                       {
                                           Id = u.SubdominioId,
                                           Nombre = u.Nombre,
                                           DomIdAsociado = u.DominioId,
                                           DomNomAsociado = d.Nombre,
                                           Activo = u.Activo,
                                           FechaCreacion = u.FechaCreacion,
                                           UsuarioCreacion = u.UsuarioCreacion,
                                           FechaModificacion = u.FechaModificacion,
                                           UsuarioModificacion = u.UsuarioModificacion
                                       }).ToList();

                        return entidad;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorSubdominioDTO
                    , "Error en el metodo: List<SubdominioDTO> GetSubdominio()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new CVTException(CVTExceptionIds.ErrorSubdominioDTO
                    , "Error en el metodo: List<SubdominioDTO> GetSubdominio()"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocompleteSubdominio> GetSubdominiosEquivalentesByFiltro(string filtro, int id)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        //var subdomUsados = (from w in ctx.Subdominio
                        //                    join x in ctx.SubdominioEquivalencia on w.SubdominioId equals x.EquivalenciaSubdomId
                        //                    where w.Activo && x.Activo
                        //                    select w.SubdominioId).ToList();

                        var registros = (from u in ctx.Subdominio
                                         where u.SubdominioId != id && u.Activo
                                         && (string.IsNullOrEmpty(filtro)
                                         || (u.Nombre).ToUpper().Contains(filtro.ToUpper()))
                                         //&& !subdomUsados.Contains(u.SubdominioId)
                                         //&& u.SubdominioId != (from )
                                         select new CustomAutocompleteSubdominio()
                                         {
                                             Id = u.SubdominioId,
                                             Descripcion = u.Nombre,
                                             value = u.Nombre,
                                             Activo = u.Activo
                                         }).ToList();

                        return registros;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorSubdominioDTO
                    , "Error en el metodo: List<CustomAutocompleteSubdominio> GetSubdominiosEquivalentesByFiltro(string filtro, int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorSubdominioDTO
                    , "Error en el metodo: List<CustomAutocompleteSubdominio> GetSubdominiosEquivalentesByFiltro(string filtro, int id)"
                    , new object[] { null });
            }
        }



        public override List<SubdominioEquivalenciaDTO> GetSubdominiosEqBySubdom(int id)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var registros = (from u in ctx.Subdominio
                                         join te in ctx.SubdominioEquivalencia on u.SubdominioId equals te.SubdominioId
                                         where u.SubdominioId == id && u.Activo && te.Activo
                                         select new SubdominioEquivalenciaDTO
                                         {
                                             Id = te.SubdominioEquivalenciaId,
                                             SubdominioId = te.SubdominioId,
                                             EquivalenciaSubdomNombre = te.EquivalenciaNombre
                                         }).ToList();

                        return registros;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorSubdominioDTO
                    , "Error en el metodo: List<SubdominioEquivalenciaDTO> GetSubdominiosEqBySubdom(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorSubdominioDTO
                    , "Error en el metodo: List<SubdominioEquivalenciaDTO> GetSubdominiosEqBySubdom(int id)"
                    , new object[] { null });
            }
        }

        public override List<SubdominioEquivalenciaDTO> GetSubdominiosEquivalentes(int id, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                totalRows = 0;
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var registros = (from u in ctx.Subdominio
                                         join d in ctx.Dominio on u.DominioId equals d.DominioId
                                         join te in ctx.SubdominioEquivalencia on u.SubdominioId equals te.SubdominioId
                                         where u.SubdominioId == id && u.Activo && te.Activo
                                         select new SubdominioEquivalenciaDTO
                                         {
                                             Id = te.SubdominioEquivalenciaId,
                                             DominioId = d.DominioId,
                                             DominioNombre = d.Nombre,
                                             SubdominioId = te.SubdominioId,
                                             SubdominioNombre = u.Nombre,
                                             EquivalenciaSubdomNombre = te.EquivalenciaNombre,
                                             Activo = te.Activo
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
                throw new CVTException(CVTExceptionIds.ErrorSubdominioDTO
                    , "Error en el metodo: List<SubdominioEquivalenciaDTO> GetSubdominiosEquivalentes(int id, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorSubdominioDTO
                    , "Error en el metodo: List<SubdominioEquivalenciaDTO> GetSubdominiosEquivalentes(int id, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<TecnologiaDTO> GetTecBySubdom(int subdomId, string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
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
                                       where (u.SubdominioId == subdomId)
                                       && (u.Nombre.ToUpper().Contains(nombre.ToUpper()) || string.IsNullOrEmpty(nombre))
                                       && s.Activo && d.Activo
                                       select new TecnologiaDTO()
                                       {
                                           Activo = u.Activo,
                                           Id = u.TecnologiaId,
                                           Versiones = u.Versiones,
                                           Dueno = u.DuenoId,
                                           FechaCreacion = u.FechaCreacion,
                                           UsuarioCreacion = u.UsuarioCreacion,
                                           FechaModificacion = u.FechaModificacion,
                                           UsuarioModificacion = u.UsuarioModificacion,
                                           FechaLanzamiento = u.FechaLanzamiento,
                                           FechaAcordada = u.FechaAcordada,
                                           FechaExtendida = u.FechaExtendida,
                                           FechaFinSoporte = u.FechaFinSoporte,
                                           Nombre = u.Nombre,
                                           Descripcion = u.Descripcion,
                                           SubdominioNomb = s.Nombre,
                                           DominioNomb = d.Nombre
                                       });

                        totalRows = entidad.Count();
                        entidad = entidad.OrderBy(sortName + " " + sortOrder);
                        var resultado = entidad.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                        return resultado;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorSubdominioDTO
                    , "Error en el metodo: List<TecnologiaDTO> GetTecBySubdom(int subdomId, string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorSubdominioDTO
                    , "Error en el metodo: List<TecnologiaDTO> GetTecBySubdom(int subdomId, string nombre, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<TablaAsociada> ValidarRegistrosAsociados(int id)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var listTblAsoc = new List<TablaAsociada>();

                    var numTecnologias = (from t in ctx.Tecnologia
                                          where t.SubdominioId == id && t.Activo
                                          select t).Count();

                    var obj = new TablaAsociada();
                    obj.Nombre = "Tecnologías";
                    obj.CantidadRegistros = numTecnologias;
                    listTblAsoc.Add(obj);

                    return listTblAsoc;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorSubdominioDTO
                    , "Error en el metodo: List<TablaAsociada> ValidarRegistrosAsociados(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorSubdominioDTO
                    , "Error en el metodo: List<TablaAsociada> ValidarRegistrosAsociados(int id)"
                    , new object[] { null });
            }
        }

        public override string GetMatriculaSubDominio(int id)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var entidad = (from u in ctx.Subdominio
                                       where u.Activo && u.SubdominioId == id
                                       //orderby u.Nombre
                                       select u.Dueno).FirstOrDefault().ToString();

                        return entidad;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorSubdominioDTO
                    , "Error en el metodo: string GetMatriculaSubDominio(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorSubdominioDTO
                    , "Error en el metodo: string GetMatriculaSubDominio(int id)"
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
                        var itemBD = (from u in ctx.SubdominioEquivalencia
                                      where u.SubdominioEquivalenciaId == id
                                      select u).FirstOrDefault();

                        if (itemBD != null)
                        {
                            itemBD.Activo = false;
                            itemBD.FechaModificacion = DateTime.Now;
                            itemBD.UsuarioModificacion = usuario;
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
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorSubdominioDTO
                    , "Error en el metodo: bool CambiarFlagEquivalencia(int id, string usuario)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorSubdominioDTO
                    , "Error en el metodo: bool CambiarFlagEquivalencia(int id, string usuario)"
                    , new object[] { null });
            }
        }

        public override List<ComboSubdominioDTO> GetSubdominiosMultiSelect()
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var entidad = (from u in ctx.Subdominio
                                       join d in ctx.Dominio on u.DominioId equals d.DominioId
                                       where u.Activo
                                       orderby u.Nombre
                                       select new ComboSubdominioDTO()
                                       {
                                           Id = u.SubdominioId,
                                           Descripcion = u.Nombre,
                                       }).ToList();

                        return entidad;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorSubdominioDTO
                    , "Error en el metodo: List<ComboSubdominioDTO> GetSubdominiosMultiSelect()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorSubdominioDTO
                    , "Error en el metodo: List<ComboSubdominioDTO> GetSubdominiosMultiSelect()"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocomplete> GetAllSubdominioByFiltro(string filtro)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var entidad = (from u in ctx.Subdominio
                                       where u.Activo
                                       && (string.IsNullOrEmpty(filtro) || (u.Descripcion).ToUpper().Contains(filtro.ToUpper()))
                                       orderby u.Nombre
                                       select new CustomAutocomplete()
                                       {
                                           Id = u.SubdominioId.ToString(),
                                           Descripcion = u.Nombre,
                                           value = u.Nombre,
                                           TipoId = u.DominioId.ToString(),
                                       }).ToList();

                        return entidad;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorSubdominioDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetAllSubdominioByFiltro(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorSubdominioDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetAllSubdominioByFiltro(string filtro)"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocomplete> GetSubdominioByDominioId(int id)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var entidad = (from u in ctx.Subdominio
                                       where u.Activo && u.DominioId == id
                                       orderby u.Nombre
                                       select new CustomAutocomplete()
                                       {
                                           Id = u.SubdominioId.ToString(),
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
                throw new CVTException(CVTExceptionIds.ErrorSubdominioDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetSubdominioByDominioId(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorSubdominioDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetSubdominioByDominioId(int id)"
                    , new object[] { null });
            }
        }

        public override List<SubdominioDTO> GetAllSubdominio()
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        var entidad = (from u in ctx.Subdominio                                       
                                       where u.Activo
                                       orderby u.Nombre
                                       select new SubdominioDTO()
                                       {
                                           Id = u.SubdominioId,
                                           Nombre = u.Nombre,
                                           DomIdAsociado = u.DominioId,                                           
                                           Activo = u.Activo,
                                           Descripcion = u.Descripcion,
                                           MatriculaDueno = u.Dueno
                                       }).ToList();

                        return entidad;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorSubdominioDTO
                    , "Error en el metodo: List<SubdominioDTO> GetSubdominio()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error(ex);
                throw new CVTException(CVTExceptionIds.ErrorSubdominioDTO
                    , "Error en el metodo: List<SubdominioDTO> GetSubdominio()"
                    , new object[] { null });
            }
        }
    }
}
