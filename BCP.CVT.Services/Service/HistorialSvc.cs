using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.Services.Interface;
using BCP.CVT.Services.ModelDB;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.Service
{
    public class HistorialSvc : HistorialDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public override List<HistorialDTO> GetHistorialModificacionByEntidadId(string entidad, string id)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var registros = (from u in ctx.HistorialModificacion
                                     where
                                     u.Entidad == entidad &&
                                     u.Id == id
                                     select new HistorialDTO()
                                     {
                                         Id = u.HistorialModificacionId,
                                         EntidadAsociado = u.EntidadAsociado,
                                         AsociadoId = u.AsociadoId,
                                         Data = u.Descripcion,
                                         UsuarioCreacion = u.CreadoPor,
                                         FechaCreacion = u.FechaCreacion.Value,
                                     }).ToList();

                    return registros;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorProductoDTO
                    , "Error en el metodo: List<ProductoDTO> GetProduct(string descripcion, string dominio, int? estado, int? tipoTecnologia, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorProductoDTO
                    , "Error en el metodo: List<ProductoDTO> GetProduct(string descripcion, string dominio, int? estado, int? tipoTecnologia, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }
    }
}
