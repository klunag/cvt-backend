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

namespace BCP.CVT.Services.Service
{
    public class ArquetipoSvc : ArquetipoDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public override int AddOrEditArquetipo(ArquetipoDTO objeto)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    if (objeto.Id == 0)
                    {
                        //if (!string.IsNullOrEmpty(objeto.DirFisicaDiag))
                        //{
                        //    objeto.DiagArqFile.SaveAs(objeto.DirFisicaDiag);
                        //}

                        var entidad = new Arquetipo()
                        {
                            Activo = objeto.Activo,
                            FechaCreacion = DateTime.Now,
                            Nombre = objeto.Nombre,
                            Descripcion = objeto.Descripcion,
                            Codigo = objeto.Codigo,
                            TipoArquetipoId = objeto.TipoArquetipoId,
                            EntornoId = objeto.EntornoId,
                            Automatizado = objeto.Automatizado,
                            UsuarioCreacion = objeto.UsuarioCreacion,
                            ArquetipoId = objeto.Id
                            //DirFisicaDiag = objeto.DirFisicaDiag,
                            //DiagIs = (string.IsNullOrEmpty(objeto.DirFisicaDiag) ? false : true),
                            //NombreDiag = objeto.NombreDiag
                        };

                        ctx.Arquetipo.Add(entidad);
                        ctx.SaveChanges();

                        return entidad.ArquetipoId;
                    }
                    else
                    {
                        var entidad = (from u in ctx.Arquetipo
                                       where u.ArquetipoId == objeto.Id
                                       select u).First();
                        if (entidad != null)
                        {
                            //if (!entidad.DiagIs.Value)
                            //{
                            //    objeto.DiagArqFile.SaveAs(objeto.DirFisicaDiag);
                            //}

                            //if (!string.IsNullOrEmpty(objeto.DirFisicaDiag) && objeto.DiagArqFile != null)
                            //{
                            //    objeto.DiagArqFile.SaveAs(objeto.DirFisicaDiag);
                            //}
                            entidad.Nombre = objeto.Nombre;
                            entidad.Descripcion = objeto.Descripcion;
                            entidad.Codigo = objeto.Codigo;
                            entidad.TipoArquetipoId = objeto.TipoArquetipoId;
                            entidad.EntornoId = objeto.EntornoId;
                            entidad.Automatizado = objeto.Automatizado;
                            //entidad.NombreDiag = objeto.NombreDiag;
                            //entidad.DirFisicaDiag = objeto.DirFisicaDiag;
                            //entidad.DiagIs = (string.IsNullOrEmpty(objeto.DirFisicaDiag) ? false : true);                          
                            entidad.FechaModificacion = DateTime.Now;
                            entidad.Activo = objeto.Activo;
                            entidad.UsuarioModificacion = objeto.UsuarioModificacion;

                            ctx.SaveChanges();
                            return entidad.ArquetipoId;
                        }
                        else
                            return 0;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorArquetipoDTO
                    , "Error en el metodo: int AddOrEditArquetipo(ArquetipoDTO objeto)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorArquetipoDTO
                    , "Error en el metodo: int AddOrEditArquetipo(ArquetipoDTO objeto)"
                    , new object[] { null });
            }
        }

        public override bool CambiarEstado(int id, bool estado, string usuario)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var itemBD = (from u in ctx.Arquetipo
                                  where u.ArquetipoId == id
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
                throw new CVTException(CVTExceptionIds.ErrorArquetipoDTO
                    , "Error en el metodo: bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorArquetipoDTO
                    , "Error en el metodo: bool CambiarEstado(int id, bool estado, string usuario)"
                    , new object[] { null });
            }
        }

        public override List<ArquetipoDTO> GetArquetipo(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
        {
            try
            {
                totalRows = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.Arquetipo
                                     join t in ctx.TipoArquetipo on u.TipoArquetipoId equals t.TipoArquetipoId
                                     join e in ctx.Entorno on u.EntornoId equals e.EntornoId
                                     where (u.Nombre.ToUpper().Contains(filtro.ToUpper())
                                     || u.Descripcion.ToUpper().Contains(filtro.ToUpper())
                                     || string.IsNullOrEmpty(filtro))
                                     && t.Activo && e.Activo
                                     select new ArquetipoDTO()
                                     {
                                         Id = u.ArquetipoId,
                                         Nombre = u.Nombre,
                                         Descripcion = u.Descripcion,
                                         Codigo = u.Codigo,
                                         TipoArquetipoId = t.TipoArquetipoId,
                                         EntornoId = e.EntornoId,
                                         TipoArquetipoStr = t.Nombre,
                                         EntornoStr = e.Nombre,
                                         Activo = u.Activo,
                                         EstadoAprob = u.EstadoAprob,
                                         UsuarioCreacion = u.UsuarioCreacion,
                                         FechaCreacion = u.FechaCreacion,
                                         FechaModificacion = u.FechaModificacion,
                                         UsuarioModificacion = u.UsuarioModificacion,
                                         NumTecAsociadas = (from sq in ctx.ArquetipoTecnologia
                                                            join t in ctx.Tecnologia on sq.TecnologiaId equals t.TecnologiaId
                                                            join f in ctx.Familia on t.FamiliaId equals f.FamiliaId
                                                            where (sq.ArquetipoId == u.ArquetipoId && u.Activo) && sq.Activo && u.Activo && t.Activo && f.Activo
                                                            select 1).Count()
                                     }).OrderBy(sortName + " " + sortOrder).ToList();

                    totalRows = registros.Count();
                    var resultado = registros.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                    return resultado;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorArquetipoDTO
                    , "Error en el metodo: List<ArquetipoDTO> GetArquetipo(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorArquetipoDTO
                    , "Error en el metodo: List<ArquetipoDTO> GetArquetipo(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override ArquetipoDTO GetArquetipoById(int id)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.Arquetipo
                                   //join tp in ctx.TablaProcedencia on (int)ETablaProcedencia.CVT_Arquetipo equals tp.CodigoInterno
                                   //join f in ctx.ArchivosCVT on new { Id = u.ArquetipoId.ToString(), TablaProcedenciaId = tp.TablaProcedenciaId } equals new { Id = f.EntidadId, TablaProcedenciaId = f.TablaProcedenciaId } into jl
                                   //from f in jl.DefaultIfEmpty()
                                   where u.ArquetipoId == id 
                                   //&& f.Activo
                                   select new ArquetipoDTO()
                                   {
                                       Id = u.ArquetipoId,
                                       Nombre = u.Nombre,
                                       Descripcion = u.Descripcion,
                                       Codigo = string.IsNullOrEmpty(u.Codigo) ? "" : u.Codigo,
                                       TipoArquetipoId = u.TipoArquetipoId,
                                       EntornoId = u.EntornoId,
                                       Automatizado = u.Automatizado.HasValue? u.Automatizado.Value : false,
                                       Activo = u.Activo,
                                       FechaCreacion = u.FechaCreacion,
                                       UsuarioCreacion = u.UsuarioCreacion,
                                       EstadoAprob = u.EstadoAprob,
                                       //ArchivoId = f.ArchivoId,
                                       //ArchivoStr = f.Nombre
                                       //NombreDiag = u.NombreDiag,
                                       //DirFisicaDiag = u.DirFisicaDiag
                                   }).FirstOrDefault();

                    int? TablaProcedenciaId = (from t in ctx.TablaProcedencia
                                               where t.CodigoInterno == (int)ETablaProcedencia.CVT_Arquetipo
                                               && t.FlagActivo
                                               select t.TablaProcedenciaId).FirstOrDefault();
                    if (TablaProcedenciaId == null) throw new Exception("TablaProcedencia no encontrado por codigo interno: " + (int)ETablaProcedencia.CVT_Arquetipo);

                    var archivo = (from u in ctx.ArchivosCVT
                                   where u.Activo && u.EntidadId == id.ToString() && u.TablaProcedenciaId == TablaProcedenciaId
                                   select new ArchivosCvtDTO()
                                   {
                                       Id = u.ArchivoId,
                                       Nombre = u.Nombre
                                   }).FirstOrDefault();

                    if(archivo != null)
                    {
                        entidad.ArchivoId = archivo.Id;
                        entidad.ArchivoStr = archivo.Nombre;
                    }

                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorArquetipoDTO
                    , "Error en el metodo: ArquetipoDTO GetArquetipoById(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorArquetipoDTO
                    , "Error en el metodo: ArquetipoDTO GetArquetipoById(int id)"
                    , new object[] { null });
            }
        }

        public override List<RelacionTecnologiaDTO> GetListTecByArquetipo(int id)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.ArquetipoTecnologia
                                     join t in ctx.Tecnologia on u.TecnologiaId equals t.TecnologiaId
                                     join f in ctx.Familia on t.FamiliaId equals f.FamiliaId
                                     join s in ctx.Subdominio on t.SubdominioId equals s.SubdominioId
                                     join d in ctx.Dominio on s.DominioId equals d.DominioId
                                     where u.ArquetipoId == id && u.Activo && d.Activo
                                     && t.Activo
                                     select new RelacionTecnologiaDTO
                                     {
                                         Id = t.TecnologiaId,
                                         Tecnologia = t.ClaveTecnologia,
                                         Subdominio = s.Nombre,
                                         Dominio = d.Nombre,
                                         Familia = f.Nombre,
                                         Activo = t.Activo
                                     }).ToList();

                    return registros;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorArquetipoDTO
                    , "Error en el metodo: List<RelacionTecnologiaDTO> GetListTecByArquetipo(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorArquetipoDTO
                    , "Error en el metodo: List<RelacionTecnologiaDTO> GetListTecByArquetipo(int id)"
                    , new object[] { null });
            }
        }

        public override bool AsociarTecByArq(List<int> itemsTecId, List<int> itemsRemoveTecId, int arqId)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    if (itemsRemoveTecId.Count > 0)
                    {
                        //Remove items tec                      
                        foreach (var itemId in itemsRemoveTecId)
                        {
                            var regItem = (from u in ctx.ArquetipoTecnologia
                                           where u.TecnologiaId == itemId && u.ArquetipoId == arqId && u.Activo
                                           select u).FirstOrDefault();

                            if (regItem != null)
                            {
                                regItem.Activo = false;
                                ctx.SaveChanges();
                            }
                        }
                    }

                    if (itemsTecId.Count > 0)
                    {
                        foreach (var itemId in itemsTecId)
                        {
                            var itemBD = (from u in ctx.ArquetipoTecnologia
                                          where (u.ArquetipoId == arqId && u.TecnologiaId == itemId && u.Activo)
                                          select u).FirstOrDefault();

                            if (itemBD != null)
                            {
                                itemBD.FechaModificacion = DateTime.Now;
                                itemBD.UsuarioModificacion = "usuario modificacion";
                                itemBD.ArquetipoId = arqId;
                                itemBD.TecnologiaId = itemId;
                            }
                            else
                            {
                                var entidad = new ArquetipoTecnologia()
                                {
                                    ArquetipoId = arqId,
                                    TecnologiaId = itemId,
                                    Activo = true,
                                    FechaCreacion = DateTime.Now,
                                    UsuarioCreacion = "usuario creacion"
                                };

                                ctx.ArquetipoTecnologia.Add(entidad);
                            }
                            ctx.SaveChanges();
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
                throw new CVTException(CVTExceptionIds.ErrorArquetipoDTO
                    , "Error en el metodo: bool AsociarTecByArq(List<int> itemsTecId, List<int> itemsRemoveTecId, int arqId)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorArquetipoDTO
                    , "Error en el metodo: bool AsociarTecByArq(List<int> itemsTecId, List<int> itemsRemoveTecId, int arqId)"
                    , new object[] { null });
            }
        }

        public override List<CustomAutocomplete> GetArquetipoByFiltro(string filtro)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from u in ctx.Arquetipo
                                   where u.Activo
                                   && (string.IsNullOrEmpty(filtro)
                                   || (u.Nombre).ToUpper().Contains(filtro.ToUpper()))
                                   orderby u.Nombre
                                   select new CustomAutocomplete()
                                   {
                                       Id = u.ArquetipoId.ToString(),
                                       Descripcion = u.Nombre,
                                       value = u.Nombre
                                   }).ToList();

                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorArquetipoDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetArquetipoByFiltro(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorArquetipoDTO
                    , "Error en el metodo: List<CustomAutocomplete> GetArquetipoByFiltro(string filtro)"
                    , new object[] { null });
            }
        }

        public override bool ExisteArquetipoById(int Id)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    bool? estado = (from u in ctx.Arquetipo
                                    where u.Activo && u.ArquetipoId == Id
                                    select true).FirstOrDefault();

                    return estado == true;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorArquetipoDTO
                    , "Error en el metodo: bool ExisteArquetipoById(int Id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorArquetipoDTO
                    , "Error en el metodo: bool ExisteArquetipoById(int Id)"
                    , new object[] { null });
            }
        }

        public override bool ExisteCodigoByFiltro(string filtro, int id)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    bool? estado = (from u in ctx.Arquetipo
                                    where u.Activo
                                    && u.ArquetipoId != id
                                    && u.Codigo.ToUpper() == filtro.ToUpper()
                                    select true).FirstOrDefault();

                    return estado == true;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorArquetipoDTO
                    , "Error en el metodo: bool ExisteCodigoByFiltro(string filtro)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorArquetipoDTO
                    , "Error en el metodo: bool ExisteCodigoByFiltro(string filtro)"
                    , new object[] { null });
            }
        }
    }
}
