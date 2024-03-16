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
	public class ParametricasSvc : ParametricasDAO
	{
		private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

		public override int AddOrEditParametricas(ParametricasDTO objeto)
		{
			try
			{
                DateTime FECHA_ACTUAL = DateTime.Now;
				using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
				{
					if (objeto.Id == -1)
					{
						var entidad = new ParametricaDetalle()
						{
							ParametricaId = objeto.ParametricaId,
							Descripcion = objeto.Descripcion,
							Valor = objeto.Valor,
							CreadoPor = objeto.UsuarioCreacion,
                            ModificadoPor = objeto.UsuarioCreacion,
							FechaCreacion = FECHA_ACTUAL,
                            FechaModificacion = FECHA_ACTUAL,
                            FlagActivo = true,
                            FlagEliminado = false
						};
						ctx.ParametricaDetalle.Add(entidad);
						ctx.SaveChanges();

						return entidad.ParametricaDetalleId;
					}
					else
					{
						var entidad = (from u in ctx.ParametricaDetalle
									   where u.ParametricaDetalleId == objeto.Id
									   select u).FirstOrDefault();
						if (entidad != null)
						{
							//entidad.ParametricaId = objeto.ParametricaId;
							//entidad.Descripcion = objeto.Descripcion;
							entidad.Valor = objeto.Valor;
                            entidad.FechaModificacion = FECHA_ACTUAL;
                            entidad.ModificadoPor = objeto.UsuarioModificacion;

							ctx.SaveChanges();

							return entidad.ParametricaDetalleId;
						}
						else
							return 0;
					}
				}
			}
			catch (DbEntityValidationException ex)
			{
				log.ErrorEntity(ex);
				throw new CVTException(CVTExceptionIds.ErrorParametricasDTO
					, "Error en el metodo: int AddOrEditParametricas(ParametricasDTO objeto)"
					, new object[] { null });
			}
			catch (Exception ex)
			{
				log.Error("Error ", ex);
				throw new CVTException(CVTExceptionIds.ErrorParametricasDTO
					, "Error en el metodo: int AddOrEditParametricas(ParametricasDTO objeto)"
					, new object[] { null });
			}
		}

		public override bool CambiarEstado(int id, bool estado, string usuario)
		{
			try
			{
				using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
				{
					var itemBD = (from u in ctx.ParametricaDetalle
								  where u.ParametricaDetalleId == id
								  select u).First();

					if (itemBD != null)
					{
						itemBD.FechaModificacion = DateTime.Now;
						itemBD.ModificadoPor = usuario;
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
				log.ErrorEntity(ex);
				throw new CVTException(CVTExceptionIds.ErrorParametricasDTO
					, "Error en el metodo: bool CambiarEstado(int id, bool estado, string usuario)"
					, new object[] { null });
			}
			catch (Exception ex)
			{
				log.Error("Error ", ex);
				throw new CVTException(CVTExceptionIds.ErrorParametricasDTO
					, "Error en el metodo: bool CambiarEstado(int id, bool estado, string usuario)"
					, new object[] { null });
			}
		}


		public override List<ParametricasDTO> GetParametricas(Paginacion pag, out int totalRows)
		{
			try
			{
				totalRows = 0;
				using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
				{
					var registros = (from u in ctx.ParametricaDetalle
									 join b in ctx.Parametrica on u.ParametricaId equals b.ParametricaId
									 where (string.IsNullOrEmpty(pag.nombre) || u.Valor.ToUpper().Contains(pag.nombre.ToUpper())) 
                                     && (string.IsNullOrEmpty(pag.tabla) || u.Descripcion.ToUpper().Contains(pag.tabla.ToUpper())) 
                                     && (pag.ParametricaId == 0 || u.ParametricaId == pag.ParametricaId)
                                     && !u.FlagEliminado.Value
									 orderby b.Valor ascending
									 select new ParametricasDTO()
									 {
										 Id = u.ParametricaDetalleId,
										 ParametricaId = u.ParametricaId,
										 Descripcion = u.Descripcion,
										 Valor = u.Valor,
										 Tabla = b.Valor,
										 Activo = u.FlagActivo,
										 UsuarioCreacion = u.CreadoPor,
										 FechaCreacion = u.FechaCreacion,
										 FechaModificacion = u.FechaModificacion,
										 UsuarioModificacion = u.ModificadoPor
									 });

					totalRows = registros.Count();
					registros = registros.OrderBy(pag.sortName + " " + pag.sortOrder);
					var resultado = registros.Skip((pag.pageNumber - 1) * pag.pageSize).Take(pag.pageSize).ToList();

					return resultado;
				}
			}
			catch (DbEntityValidationException ex)
			{
				log.ErrorEntity(ex);
				throw new CVTException(CVTExceptionIds.ErrorParametricasDTO
					, "Error en el metodo: List<ParametricasDTO> GetParametricas(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
					, new object[] { null });
			}
			catch (Exception ex)
			{
				log.Error("Error ", ex);
				throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
					, "Error en el metodo: List<ParametricasDTO> GetParametricas(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
					, new object[] { null });
			}
		}

		public override List<ParametricasDTO> GetParametricas()
		{
			try
			{
				using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
				{
					var registros = (from u in ctx.ParametricaDetalle
									 join b in ctx.Parametrica
									 on u.ParametricaId equals b.ParametricaId
									 where u.FlagActivo
									 select new ParametricasDTO()
									 {
										 Id = u.ParametricaDetalleId,
										 ParametricaId = u.ParametricaId,
										 Descripcion = u.Descripcion,
										 Valor = u.Valor
									 }).ToList();

					return registros;
				}
			}
			catch (DbEntityValidationException ex)
			{
				log.ErrorEntity(ex);
				throw new CVTException(CVTExceptionIds.ErrorParametricasDTO
					, "Error en el metodo: List<ParametricasDTO> GetParametricas()"
					, new object[] { null });
			}
			catch (Exception ex)
			{
				log.Error("Error ", ex);
				throw new CVTException(CVTExceptionIds.ErrorParametricasDTO
					, "Error en el metodo: List<ParametricasDTO> GetParametricas()"
					, new object[] { null });
			}
		}

		public override List<CustomAutocomplete> GetParametricasByTabla(string filtro, int? entidadId)
		{
			try
			{
				using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
				{
					var entidad = (from u in ctx.ParametricaDetalle
								   where u.FlagActivo
								   && (string.IsNullOrEmpty(filtro) || (u.Descripcion).ToUpper().Equals(filtro.ToUpper()))
                                   && (!entidadId.HasValue || u.ParametricaId == entidadId.Value)
								   orderby u.Descripcion
								   select new CustomAutocomplete()
								   {
									   Id = string.IsNullOrEmpty(u.CodigoOpcional) ? u.ParametricaDetalleId.ToString() : u.CodigoOpcional,
									   Descripcion = u.Valor,
									   value = u.Valor
                                   }).ToList();

					return entidad;
				}
			}
			catch (DbEntityValidationException ex)
			{
				log.ErrorEntity(ex);
				throw new CVTException(CVTExceptionIds.ErrorParametricasDTO
					, "Error en el metodo: List<CustomAutocomplete> GetParametricasByFiltro(string filtro)"
					, new object[] { null });
			}
			catch (Exception ex)
			{
				log.Error("Error ", ex);
				throw new CVTException(CVTExceptionIds.ErrorParametricasDTO
					, "Error en el metodo: List<CustomAutocomplete> GetParametricasByFiltro(string filtro)"
					, new object[] { null });
			}
		}

		public override ParametricasDTO GetParametricasById(int id)
		{
			try
			{
				using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
				{
					var entidad = (from u in ctx.ParametricaDetalle

								   where u.ParametricaDetalleId == id
								   select new ParametricasDTO()
								   {
									   Id = u.ParametricaDetalleId,
									   Descripcion = u.Descripcion,
									   Valor = u.Valor,
									   ParametricaId = u.ParametricaId,
									   Activo = u.FlagActivo,
									   //FechaCreacion = u.FechaCreacion.HasValue? u.FechaCreacion.Value : DateTime.Now,
									   UsuarioCreacion = u.CreadoPor,

								   }).FirstOrDefault();
					return entidad;
				}
			}
			catch (DbEntityValidationException ex)
			{
				log.ErrorEntity(ex);
				throw new CVTException(CVTExceptionIds.ErrorAmbienteDTO
					, "Error en el metodo: ParametricasDTO GetParametricasById(int id)"
					, new object[] { null });
			}
			catch (Exception ex)
			{
				log.Error("Error ", ex);
				throw new CVTException(CVTExceptionIds.ErrorParametricasDTO
					, "Error en el metodo: ParametricasDTO GetParametricasById(int id)"
					, new object[] { null });
			}
		}


		public override List<CustomAutocomplete> GetEntidades(int idParametrica)
		{
			try
			{
				
				using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
				{
					
					var registros = (from u in ctx.Parametrica
                                     where (idParametrica == 0 || u.ParametricaId == idParametrica)
									 select new CustomAutocomplete
                                     {
										 Id = u.ParametricaId.ToString(),
										 Descripcion = u.Valor
									 }).ToList();

					return registros;
				}
			}
			catch (DbEntityValidationException ex)
			{
				log.ErrorEntity(ex);
				throw new CVTException(CVTExceptionIds.ErrorParametricasDTO
					, "Error en el metodo: List<ParametricasDTO> GetParametricas()"
					, new object[] { null });
			}
			catch (Exception ex)
			{
				log.Error("Error ", ex);
				throw new CVTException(CVTExceptionIds.ErrorParametricasDTO
					, "Error en el metodo: List<ParametricasDTO> GetParametricas()"
					, new object[] { null });
			}
		}

		public override List<CustomAutocomplete> GetTablas(int idParametrica)
		{
			try
			{

				using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
				{

					var registros = (from u in ctx.ParametricaDetalle
                                     where (idParametrica == 0 || u.ParametricaId == idParametrica)
									 group u.Descripcion by u.Descripcion into g
									 orderby g.Key
									 select new CustomAutocomplete
									 {
										 Descripcion = g.Key
									 }).ToList();

					return registros;
				}
			}
			catch (DbEntityValidationException ex)
			{
				log.ErrorEntity(ex);
				throw new CVTException(CVTExceptionIds.ErrorParametricasDTO
					, "Error en el metodo: List<ParametricasDTO> GetParametricas()"
					, new object[] { null });
			}
			catch (Exception ex)
			{
				log.Error("Error ", ex);
				throw new CVTException(CVTExceptionIds.ErrorParametricasDTO
					, "Error en el metodo: List<ParametricasDTO> GetParametricas()"
					, new object[] { null });
			}
		}

	}
}
