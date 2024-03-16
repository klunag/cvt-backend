using BCP.CVT.Cross;
using BCP.CVT.DTO.Grilla;
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
    public class VistaSvc : VistaDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public override List<TecnologiaG> GetVistaTecnologia(int domId, int subdomId, string nombre, string aplica, string codigo, string dueno, string equipo, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)
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
                                             && (e.Nombre.ToUpper().Contains(equipo.ToUpper()) || string.IsNullOrEmpty(equipo))
                                             select et.TecnologiaId).ToList();

                        var registros = (from u in ctx.Tecnologia
                                         join t in ctx.Tipo on u.TipoTecnologia equals t.TipoId into lj1
                                         from t in lj1.DefaultIfEmpty()
                                         join s in ctx.Subdominio on u.SubdominioId equals s.SubdominioId
                                         join d in ctx.Dominio on s.DominioId equals d.DominioId
                                         where u.SubdominioId == (subdomId == -1 ? u.SubdominioId : subdomId) &&
                                         (u.Nombre.ToUpper().Contains(nombre.ToUpper())
                                         || string.IsNullOrEmpty(nombre)
                                         || u.Descripcion.ToUpper().Contains(nombre.ToUpper())
                                         || u.ClaveTecnologia.ToUpper().Contains(nombre.ToUpper())
                                         ) &&
                                         (s.DominioId == (domId == -1 ? s.DominioId : domId))
                                         && (u.Aplica.ToUpper().Contains(aplica.ToUpper()) || string.IsNullOrEmpty(aplica))
                                         && (u.CodigoTecnologiaAsignado.ToUpper().Contains(codigo.ToUpper()) || string.IsNullOrEmpty(codigo))
                                         && (u.DuenoId.ToUpper().Contains(dueno.ToUpper()) || string.IsNullOrEmpty(dueno))
                                         && (string.IsNullOrEmpty(equipo) || tecnologiaIds.Contains(u.TecnologiaId))

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
                                             ClaveTecnologia = u.ClaveTecnologia
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
                throw new CVTException(CVTExceptionIds.ErrorVistaDTO
                    , "Error en el metodo: List<TecnologiaG> GetVistaTecnologia(int domId, int subdomId, string nombre, string aplica, string codigo, string dueno, string equipo, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorVistaDTO
                    , "Error en el metodo: List<TecnologiaG> GetVistaTecnologia(int domId, int subdomId, string nombre, string aplica, string codigo, string dueno, string equipo, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }
    }
}
