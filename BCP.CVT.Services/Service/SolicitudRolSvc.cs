using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.Services.Email;
using BCP.CVT.Services.Interface;
using BCP.CVT.Services.Interface.PortafolioAplicaciones;
using BCP.CVT.Services.ModelDB;
using BCP.PAPP.Common.Cross;
using BCP.PAPP.Common.Dto;
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
using BCP.PAPP.Common.Custom;

namespace BCP.CVT.Services.Service
{
    public class SolicitudRolSvc : SolicitudRolDAO
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public override List<SolicitudRolDTO> GetFuncionSolicitudes(PaginacionSolicitud pag, out int totalRows)
        {
            try
            {
                totalRows = 0;

                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        ctx.Database.CommandTimeout = 0;

                        if (pag.Perfil.Contains("E195_Administrador"))
                        {
                            var registros = (from u in ctx.Producto
                                             join u4 in ctx.RolesProducto on u.ProductoId equals u4.ProductoId
                                             into ps
                                             from pco in ps.DefaultIfEmpty()
                                             join r1 in ctx.ChapterFuncionesRoles on pco.RolesProductoId equals r1.RolesProductoId
                                             join r2 in ctx.SolicitudRolFuncion on r1.FuncionProductoId equals r2.FuncionProductoId
                                             join r3 in ctx.SolRolFuncionCabecera on r2.IdSolicitud equals r3.IdSolicitud

                                             join u2 in ctx.Dominio on u.DominioId equals u2.DominioId
                                             join u3 in ctx.Subdominio on u.SubDominioId equals u3.SubdominioId
                                             where
                                             //(u.Nombre.ToUpper().Contains(pag.Producto.ToUpper()) || string.IsNullOrEmpty(pag.Producto)) &&
                                             (u.Nombre.ToUpper().Contains(pag.Producto.ToUpper())
                                             || u.Fabricante.ToUpper().Contains(pag.Producto.ToUpper())
                                             || string.Concat(u.Fabricante.ToUpper(), " ", u.Nombre.ToUpper()).Contains(pag.Producto.ToUpper())
                                             || string.IsNullOrEmpty(pag.Producto))
                                             && u.FlagActivo == true &&
                                             (u.DominioId == pag.DominioId || pag.DominioId == -1) &&
                                             (u.SubDominioId == pag.SubDominioId || pag.SubDominioId == -1) &&
                                             //(r3.EstadoSolicitudCabecera == pag.EstadoAsignacion || pag.EstadoAsignacion == -1) &&
                                             (pag.EstadoAsignacion.Count == 0 || pag.EstadoAsignacion.Contains((int)r3.EstadoSolicitudCabecera)) &&
                                             (pag.FechaRegistroSolicitud2 == null
                                             || DbFunctions.TruncateTime(r3.FechaCreacion) == DbFunctions.TruncateTime(pag.FechaRegistroSolicitud).Value) &&
                                             (pag.FechaAtencionSolicitud2 == null
                                             || DbFunctions.TruncateTime(r3.FechaRevision) == DbFunctions.TruncateTime(pag.FechaAtencionSolicitud).Value)
                                             //&& ((r3.CreadoPor.ToUpper().Contains(pag.Matricula.ToUpper()))
                                             //|| (r3.OwnerProducto.ToUpper().Contains(pag.Matricula.ToUpper()))
                                             //|| (r3.AnalistaSeguridad.ToUpper().Contains(pag.Matricula.ToUpper())))
                                             && (u.Codigo.ToUpper().Contains(pag.CodigoApt.ToUpper()) || string.IsNullOrEmpty(pag.CodigoApt))
                                             select new SolicitudRolDTO()
                                             {
                                                 Id = (int)r3.IdSolicitud,
                                                 SolicitudId = r3.IdSolicitud,
                                                 Fabricante = u.Fabricante,
                                                 //Producto = u.Nombre,
                                                 Producto = string.Concat(u.Fabricante, " ", u.Nombre), 
                                                 ProductoId = u.ProductoId,
                                                 Dominio = u2.Nombre,
                                                 DominioId = u.DominioId,
                                                 SubDominio = u3.Nombre,
                                                 SubDominioId = u3.SubdominioId,
                                                 //CantRoles = 0,
                                                 FechaRevision = r3.FechaRevision,
                                                 RevisadoPor = r3.RevisadoPor,
                                                 FechaCreacion = r3.FechaCreacion,
                                                 EstadoSolicitud = r3.EstadoSolicitudCabecera,
                                                 IdTipoSolicitud = r3.IdTipoSolicitud,
                                                 Codigo = u.Codigo,
                                                 Owner = (string.IsNullOrEmpty(r3.OwnerProducto) ? string.Empty : r3.OwnerProducto),
                                                 Seguridad = (string.IsNullOrEmpty(r3.AnalistaSeguridad) ? string.Empty : r3.AnalistaSeguridad),
                                                 EsOwner = false,
                                                 EsSeguridad = false,
                                                 EsRegistroMio = ((r3.CreadoPor.ToUpper() == pag.Matricula.ToUpper()) ? true : false),
                                                 EsPendienteMio = 0,
                                                 CreadoPor = u.CreadoPor.ToUpper(),
                                                 OwnerNombre = r3.NombreOwner,
                                                 SeguridadNombre = r3.NombreAnalista
                                                 //}).Distinct().OrderBy("FechaCreacion" + " " + pag.sortOrder);
                                             }).Distinct().OrderBy(pag.sortName + " " + pag.sortOrder);
                            totalRows = registros.Count();

                            //var resultado = registros.Skip((pag.pageNumber - 1) * pag.pageSize).Take(pag.pageSize).ToList();
                            var resultado = registros.ToList();

                            foreach (var item in resultado)
                            {

                                // Obtener cantidad roles
                                //var cantidad = (from u in ctx.SolicitudRolFuncion
                                //                join u1 in ctx.ChapterFuncionesRoles on u.FuncionProductoId equals u1.FuncionProductoId
                                //                join u2 in ctx.RolesProducto on u1.RolesProductoId equals u2.RolesProductoId
                                //                where
                                //                u.IdSolicitud == item.SolicitudId && u.FlagEliminado == false
                                //                select u2.RolesProductoId
                                //                ).Distinct().ToList();

                                //if (cantidad != null)
                                //    item.CantRoles = cantidad.Count();
                                //else if (cantidad == null)
                                //    item.CantRoles = 0;

                                // Verificación de estado pendiente
                                if (item.EstadoSolicitud == (int)EstadoSolicitudRolFuncion.Pendiente)
                                {
                                    item.FechaRevision = null;
                                    item.RevisadoPor = "";
                                }

                                // Verificar si es Owner
                                if (item.Owner.ToUpper() == pag.Matricula.ToUpper())
                                {
                                    if ((item.Owner.ToUpper() == pag.Matricula.ToUpper()) &&
                                    (item.EstadoSolicitud == (int)EstadoSolicitudRolFuncion.Pendiente || item.EstadoSolicitud == (int)EstadoSolicitudRolFuncion.AprobadoSeguridad))
                                    {
                                        item.EsPendienteMio = 1;
                                    }

                                    item.EsOwner = true;
                                }

                                // Verificar si es Analista de Seguridad
                                if (item.Seguridad.ToUpper() == pag.Matricula.ToUpper())
                                {
                                    if ((item.Seguridad.ToUpper() == pag.Matricula.ToUpper()) &&
                                    (item.IdTipoSolicitud == (int)TipoSolicitudRolFunciones.Registro) &&
                                    (item.EstadoSolicitud == (int)EstadoSolicitudRolFuncion.Pendiente || item.EstadoSolicitud == (int)EstadoSolicitudRolFuncion.AprobadoOwner))
                                    {
                                        item.EsPendienteMio = 1;
                                    }

                                    item.EsSeguridad = true;
                                }

                            }

                            // Ordenamiendo por pendiente de aprobación y de mayor a menor
                            resultado = resultado.OrderByDescending(x => x.EsPendienteMio).ThenByDescending(x => x.SolicitudId).Skip((pag.pageNumber - 1) * pag.pageSize).Take(pag.pageSize).ToList();

                            return resultado;
                        }
                        else 
                        {
                            var registros = (from u in ctx.Producto
                                             join u4 in ctx.RolesProducto on u.ProductoId equals u4.ProductoId
                                             into ps
                                             from pco in ps.DefaultIfEmpty()
                                             join r1 in ctx.ChapterFuncionesRoles on pco.RolesProductoId equals r1.RolesProductoId
                                             join r2 in ctx.SolicitudRolFuncion on r1.FuncionProductoId equals r2.FuncionProductoId
                                             join r3 in ctx.SolRolFuncionCabecera on r2.IdSolicitud equals r3.IdSolicitud

                                             join u2 in ctx.Dominio on u.DominioId equals u2.DominioId
                                             join u3 in ctx.Subdominio on u.SubDominioId equals u3.SubdominioId
                                             where
                                             //(u.Nombre.ToUpper().Contains(pag.Producto.ToUpper()) || string.IsNullOrEmpty(pag.Producto)) &&
                                             (u.Nombre.ToUpper().Contains(pag.Producto.ToUpper())
                                             || u.Fabricante.ToUpper().Contains(pag.Producto.ToUpper())
                                             || string.Concat(u.Fabricante.ToUpper(), " ", u.Nombre.ToUpper()).Contains(pag.Producto.ToUpper())
                                             || string.IsNullOrEmpty(pag.Producto))
                                             && u.FlagActivo == true &&
                                             (u.DominioId == pag.DominioId || pag.DominioId == -1) &&
                                             (u.SubDominioId == pag.SubDominioId || pag.SubDominioId == -1) &&
                                             //(r3.EstadoSolicitudCabecera == pag.EstadoAsignacion || pag.EstadoAsignacion == -1) &&
                                             (pag.EstadoAsignacion.Count == 0 || pag.EstadoAsignacion.Contains((int)r3.EstadoSolicitudCabecera)) &&
                                             (pag.FechaRegistroSolicitud2 == null
                                             || DbFunctions.TruncateTime(r3.FechaCreacion) == DbFunctions.TruncateTime(pag.FechaRegistroSolicitud).Value) &&
                                             (pag.FechaAtencionSolicitud2 == null
                                             || DbFunctions.TruncateTime(r3.FechaRevision) == DbFunctions.TruncateTime(pag.FechaAtencionSolicitud).Value)
                                             && ((r3.CreadoPor.ToUpper().Contains(pag.Matricula.ToUpper()))
                                             || (r3.OwnerProducto.ToUpper().Contains(pag.Matricula.ToUpper()))
                                             || (r3.AnalistaSeguridad.ToUpper().Contains(pag.Matricula.ToUpper())))
                                             && (u.Codigo.ToUpper().Contains(pag.CodigoApt.ToUpper()) || string.IsNullOrEmpty(pag.CodigoApt))
                                             select new SolicitudRolDTO()
                                             {
                                                 Id = (int)r3.IdSolicitud,
                                                 SolicitudId = r3.IdSolicitud,
                                                 Fabricante = u.Fabricante,
                                                 //Producto = u.Nombre,
                                                 Producto = string.Concat(u.Fabricante, " ", u.Nombre),
                                                 ProductoId = u.ProductoId,
                                                 Dominio = u2.Nombre,
                                                 DominioId = u.DominioId,
                                                 SubDominio = u3.Nombre,
                                                 SubDominioId = u3.SubdominioId,
                                                 //CantRoles = 0,
                                                 FechaRevision = r3.FechaRevision,
                                                 RevisadoPor = r3.RevisadoPor,
                                                 FechaCreacion = r3.FechaCreacion,
                                                 EstadoSolicitud = r3.EstadoSolicitudCabecera,
                                                 IdTipoSolicitud = r3.IdTipoSolicitud,
                                                 Codigo = u.Codigo,
                                                 Owner = (string.IsNullOrEmpty(r3.OwnerProducto) ? string.Empty : r3.OwnerProducto),
                                                 Seguridad = (string.IsNullOrEmpty(r3.AnalistaSeguridad) ? string.Empty : r3.AnalistaSeguridad),
                                                 EsOwner = false,
                                                 EsSeguridad = false,
                                                 EsRegistroMio = ((r3.CreadoPor.ToUpper() == pag.Matricula.ToUpper()) ? true : false),
                                                 EsPendienteMio = 0,
                                                 CreadoPor = u.CreadoPor.ToUpper(),
                                                 OwnerNombre = r3.NombreOwner,
                                                 SeguridadNombre = r3.NombreAnalista
                                                 //}).Distinct().OrderBy("FechaCreacion" + " " + pag.sortOrder);
                                             }).Distinct().OrderBy(pag.sortName + " " + pag.sortOrder);
                            totalRows = registros.Count();

                            //var resultado = registros.Skip((pag.pageNumber - 1) * pag.pageSize).Take(pag.pageSize).ToList();
                            var resultado = registros.ToList();

                            foreach (var item in resultado)
                            {

                                // Obtener cantidad roles
                                //var cantidad = (from u in ctx.SolicitudRolFuncion
                                //                join u1 in ctx.ChapterFuncionesRoles on u.FuncionProductoId equals u1.FuncionProductoId
                                //                join u2 in ctx.RolesProducto on u1.RolesProductoId equals u2.RolesProductoId
                                //                where
                                //                u.IdSolicitud == item.SolicitudId && u.FlagEliminado == false
                                //                select u2.RolesProductoId
                                //                ).Distinct().ToList();

                                //if (cantidad != null)
                                //    item.CantRoles = cantidad.Count();
                                //else if (cantidad == null)
                                //    item.CantRoles = 0;

                                // Verificación de estado pendiente
                                if (item.EstadoSolicitud == (int)EstadoSolicitudRolFuncion.Pendiente)
                                {
                                    item.FechaRevision = null;
                                    item.RevisadoPor = "";
                                }

                                // Verificar si es Owner
                                if (item.Owner.ToUpper() == pag.Matricula.ToUpper())
                                {
                                    if ((item.Owner.ToUpper() == pag.Matricula.ToUpper()) &&
                                    (item.EstadoSolicitud == (int)EstadoSolicitudRolFuncion.Pendiente || item.EstadoSolicitud == (int)EstadoSolicitudRolFuncion.AprobadoSeguridad))
                                    {
                                        item.EsPendienteMio = 1;
                                    }

                                    item.EsOwner = true;
                                }

                                // Verificar si es Analista de Seguridad
                                if (item.Seguridad.ToUpper() == pag.Matricula.ToUpper())
                                {
                                    if ((item.Seguridad.ToUpper() == pag.Matricula.ToUpper()) &&
                                    (item.IdTipoSolicitud == (int)TipoSolicitudRolFunciones.Registro) &&
                                    (item.EstadoSolicitud == (int)EstadoSolicitudRolFuncion.Pendiente || item.EstadoSolicitud == (int)EstadoSolicitudRolFuncion.AprobadoOwner))
                                    {
                                        item.EsPendienteMio = 1;
                                    }

                                    item.EsSeguridad = true;
                                }

                            }

                            // Ordenamiendo por pendiente de aprobación y de mayor a menor
                            resultado = resultado.OrderByDescending(x => x.EsPendienteMio).ThenByDescending(x => x.SolicitudId).Skip((pag.pageNumber - 1) * pag.pageSize).Take(pag.pageSize).ToList();

                            return resultado;
                        }
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<SolicitudDto> GetSolicitudes(PaginacionSolicitud pag, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<SolicitudDto> GetSolicitudes(PaginacionSolicitud pag, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override DataResultAplicacion AprobarSolicitudOwner(int id, string matricula, string nombre, string comentario)
        {

            try
            {
                var dataResult = new DataResultAplicacion()
                {
                    AplicacionId = 0,
                    SolicitudId = 0,
                    EstadoTransaccion = true
                };


                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    // 1. Actualizar abecera
                    var cabecera = ctx.SolRolFuncionCabecera.FirstOrDefault(x => x.IdSolicitud == id);
                    cabecera.EstadoSolicitudCabecera = (int)EstadoSolicitudRolFuncion.AprobadoOwner;
                    cabecera.FechaRevision = DateTime.Now;
                    cabecera.RevisadoPor = nombre.ToUpper();

                    // 2. Registro de historial
                    var historial = new SolRolFuncionHistorial()
                    {
                        IdSolRolFuncionCabecera = cabecera.IdSolRolFuncionCabecera,
                        Comentario = string.Empty,
                        ObservacionOwner = comentario,
                        EstadoSolicitudHistorial = ((int)EstadoSolicitudRolFuncion.AprobadoOwner),
                        FechaCreacion = DateTime.Now,
                        CreadoPor = matricula,
                        FechaRevision = DateTime.Now,
                        RevisadoPor = nombre.ToUpper()
                    };
                    ctx.SolRolFuncionHistorial.Add(historial);
                    ctx.SaveChanges();

                    // 3. Verificar si hay una aprobación previa
                    ActualizarEstadoAtendido(id, cabecera, ctx, nombre, matricula);

                    return dataResult;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: DataResultAplicacion AprobarSolicitudOwner(int id, string matricula, string nombre)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: DataResultAplicacion AprobarSolicitudOwner(int id, string matricula, string nombre)"
                    , new object[] { null });
            }
        }
        public override DataResultAplicacion ObservarSolicitudOwner(int id, string matricula, string nombre, string comentario, int idProducto)
        {

            try
            {
                var dataResult = new DataResultAplicacion()
                {
                    AplicacionId = 0,
                    SolicitudId = 0,
                    EstadoTransaccion = true
                };

                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {

                    // 1. Actualizar abecera
                    var cabecera = ctx.SolRolFuncionCabecera.FirstOrDefault(x => x.IdSolicitud == id);
                    cabecera.EstadoSolicitudCabecera = (int)EstadoSolicitudRolFuncion.ObservadoOwner;//May2022:Solo existe estado Rechazado
                    //cabecera.EstadoSolicitudCabecera = (int)EstadoSolicitudRolFuncion.Rechazado; // Mejora_Estado_Rechazado

                    cabecera.FechaRevision = DateTime.Now;
                    cabecera.RevisadoPor = nombre.ToUpper();

                    // 2. Registro de historial
                    var historial = new SolRolFuncionHistorial()
                    {
                        IdSolRolFuncionCabecera = cabecera.IdSolRolFuncionCabecera,
                        ObservacionOwner = comentario,
                        EstadoSolicitudHistorial = ((int)EstadoSolicitudRolFuncion.ObservadoOwner),////May2022:Solo existe estado Rechazado
                        //EstadoSolicitudHistorial = ((int)EstadoSolicitudRolFuncion.Rechazado), // Mejora_Estado_Rechazado
                        FechaCreacion = DateTime.Now,
                        CreadoPor = matricula,
                        FechaRevision = DateTime.Now,
                        RevisadoPor = nombre.ToUpper()
                    };

                    ctx.SolRolFuncionHistorial.Add(historial);
                    ctx.SaveChanges();

                    // 3. Envio de correo por observación
                    try
                    {
                        var producto = (from u in ctx.Producto
                                        where u.ProductoId == idProducto
                                        select new ProductoDTO()
                                        {
                                            Id = u.ProductoId,
                                            Codigo = u.Codigo,
                                            Nombre = string.Concat(u.Fabricante, " ", u.Nombre)
                                        }).FirstOrDefault();

                        string NombreCorreo = "NOTIFICACION_APROBACION_SOLICITUD_OBSERVACION";
                        string CodigoProducto = producto.Codigo;
                        string NombreProducto = producto.Nombre;

                        var mailManager = new MailingManager();
                        var diccionario = new Dictionary<string, string>();
                        diccionario.Add("[Motivo]", comentario);

                        mailManager.ProcesarEnvioNotificacionesObservacion(NombreCorreo, diccionario, CodigoProducto, NombreProducto, id, (int)cabecera.IdTipoSolicitud);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.Message, ex);
                    }

                    // Actualizar a atendido si se trata de una solicitud de eliminación observada
                    ActualizarEstadoAtendido_PorObservacion(id, cabecera, ctx, nombre, matricula);

                    return dataResult;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: DataResultAplicacion ObservarSolicitudOwner(int id, string matricula, string nombre, string comentario)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: DataResultAplicacion ObservarSolicitudOwner(int id, string matricula, string nombre, string comentario)"
                    , new object[] { null });
            }
        }
        public override DataResultAplicacion AprobarSolicitudSeguridad(int id, string matricula, string nombre, string comentario)
        {

            try
            {
                var dataResult = new DataResultAplicacion()
                {
                    AplicacionId = 0,
                    SolicitudId = 0,
                    EstadoTransaccion = true
                };


                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    // 1. Actualizar cabecera
                    var cabecera = ctx.SolRolFuncionCabecera.FirstOrDefault(x => x.IdSolicitud == id);
                    cabecera.EstadoSolicitudCabecera = (int)EstadoSolicitudRolFuncion.AprobadoSeguridad;
                    cabecera.FechaRevision = DateTime.Now;
                    cabecera.RevisadoPor = nombre.ToUpper();

                    // 2. Registro de historial
                    var historial = new SolRolFuncionHistorial()
                    {
                        IdSolRolFuncionCabecera = cabecera.IdSolRolFuncionCabecera,
                        Comentario = string.Empty,
                        ObservacionSeguridad = comentario,
                        EstadoSolicitudHistorial = ((int)EstadoSolicitudRolFuncion.AprobadoSeguridad),
                        FechaCreacion = DateTime.Now,
                        CreadoPor = matricula,
                        FechaRevision = DateTime.Now,
                        RevisadoPor = nombre.ToUpper()
                    };
                    ctx.SolRolFuncionHistorial.Add(historial);

                    ctx.SaveChanges();

                    // 3. Verificar si hay una aprobación previa
                    ActualizarEstadoAtendido(id,cabecera, ctx, nombre, matricula);

                    return dataResult;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: DataResultAplicacion AprobarSolicitudSeguridad(int id, string matricula, string nombre)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: DataResultAplicacion AprobarSolicitudSeguridad(int id, string matricula, string nombre)"
                    , new object[] { null });
            }
        }
        public override DataResultAplicacion ObservarSolicitudSeguridad(int id, string matricula, string nombre, string comentario, int idProducto)
        {

            try
            {
                var dataResult = new DataResultAplicacion()
                {
                    AplicacionId = 0,
                    SolicitudId = 0,
                    EstadoTransaccion = true
                };

                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {

                    // 1. Actualizar abecera
                    var cabecera = ctx.SolRolFuncionCabecera.FirstOrDefault(x => x.IdSolicitud == id);
                    cabecera.EstadoSolicitudCabecera = (int)EstadoSolicitudRolFuncion.ObservadoSeguridad;////May2022:Solo existe estado Rechazado
                    //cabecera.EstadoSolicitudCabecera = (int)EstadoSolicitudRolFuncion.Rechazado; // Mejora_Estado_Rechazado
                    cabecera.FechaRevision = DateTime.Now;
                    cabecera.RevisadoPor = nombre.ToUpper();

                    // 2. Registro de historial
                    var historial = new SolRolFuncionHistorial()
                    {
                        IdSolRolFuncionCabecera = cabecera.IdSolRolFuncionCabecera,
                        ObservacionSeguridad = comentario,
                        EstadoSolicitudHistorial = ((int)EstadoSolicitudRolFuncion.ObservadoSeguridad),////May2022:Solo existe estado Rechazado
                        //EstadoSolicitudHistorial = ((int)EstadoSolicitudRolFuncion.Rechazado), // Mejora_Estado_Rechazado
                        FechaCreacion = DateTime.Now,
                        CreadoPor = matricula,
                        FechaRevision = DateTime.Now,
                        RevisadoPor = nombre.ToUpper()
                    };

                    ctx.SolRolFuncionHistorial.Add(historial);
                    ctx.SaveChanges();

                    // 3. Envio de correo por observación
                    try
                    {
                        var producto = (from u in ctx.Producto
                                        where u.ProductoId == idProducto
                                        select new ProductoDTO()
                                        {
                                            Id = u.ProductoId,
                                            Codigo = u.Codigo,
                                            Nombre = string.Concat(u.Fabricante, " ", u.Nombre)
                                        }).FirstOrDefault();

                        string NombreCorreo = "NOTIFICACION_APROBACION_SOLICITUD_OBSERVACION";
                        string CodigoProducto = producto.Codigo;
                        string NombreProducto = producto.Nombre;

                        var mailManager = new MailingManager();
                        var diccionario = new Dictionary<string, string>();
                        diccionario.Add("[Motivo]", comentario);

                        mailManager.ProcesarEnvioNotificacionesObservacion(NombreCorreo, diccionario, CodigoProducto, NombreProducto, id, (int)cabecera.IdTipoSolicitud);
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex.Message, ex);
                    }

                    return dataResult;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: DataResultAplicacion ObservarSolicitudSeguridad(int id, string matricula, string nombre, string comentario)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: DataResultAplicacion ObservarSolicitudSeguridad(int id, string matricula, string nombre, string comentario)"
                    , new object[] { null });
            }
        }
        public void ActualizarEstadoAtendido(int idSolicitud, SolRolFuncionCabecera cabecera, GestionCMDB_ProdEntities ctx, string nombre, string matricula)
        {
            List<SolRolHistorialDTO> historialReg = new List<SolRolHistorialDTO>();
            bool registro = false;

            if (cabecera.IdTipoSolicitud == (int)TipoSolicitudRolFunciones.Eliminacion)
            {
                historialReg = (from u in ctx.SolRolFuncionHistorial
                                where u.IdSolRolFuncionCabecera == cabecera.IdSolRolFuncionCabecera &&
                                      (u.EstadoSolicitudHistorial == (int)EstadoSolicitudRolFuncion.AprobadoOwner ||
                                      u.EstadoSolicitudHistorial == (int)EstadoSolicitudRolFuncion.Pendiente)
                                select new SolRolHistorialDTO()
                                {
                                    HistorialId = u.IdSolRolFuncionHistorial,
                                    EstadoSolicitud = u.EstadoSolicitudHistorial
                                }).ToList();

                historialReg = historialReg.Where(x => x.HistorialId > historialReg.Where(p => p.EstadoSolicitud == (int)EstadoSolicitudRolFuncion.Pendiente).Max(p => p.HistorialId)
                            ).ToList();

                if (historialReg.Count == 1) 
                {
                    registro = true;
                }
            }
            else
            {
                historialReg = (from u in ctx.SolRolFuncionHistorial
                                where u.IdSolRolFuncionCabecera == cabecera.IdSolRolFuncionCabecera &&
                                      (u.EstadoSolicitudHistorial == (int)EstadoSolicitudRolFuncion.AprobadoOwner ||
                                      u.EstadoSolicitudHistorial == (int)EstadoSolicitudRolFuncion.AprobadoSeguridad ||
                                      u.EstadoSolicitudHistorial == (int)EstadoSolicitudRolFuncion.Pendiente)
                                select new SolRolHistorialDTO()
                                {
                                    HistorialId = u.IdSolRolFuncionHistorial,
                                    EstadoSolicitud = u.EstadoSolicitudHistorial
                                }).ToList();

                historialReg = historialReg.Where(x => x.HistorialId > historialReg.Where(p => p.EstadoSolicitud == (int)EstadoSolicitudRolFuncion.Pendiente).Max(p => p.HistorialId)
                            ).ToList();

                if (historialReg.Count == 2)
                {
                    registro = true;
                }
            }

            //historialReg = historialReg.Where(x => x.HistorialId > historialReg.Where(p => p.EstadoSolicitud == (int)EstadoSolicitudRolFuncion.Pendiente).Max(p => p.HistorialId)
            //                ).ToList();

            //if (historialReg.Count == 2)
            if (registro)
            {
                // 2. Actualizar cabecera
                cabecera.EstadoSolicitudCabecera = (int)EstadoSolicitudRolFuncion.Atendido;
                cabecera.FechaRevision = DateTime.Now;
                cabecera.RevisadoPor = nombre.ToUpper();
                
                // 3. Actualizar historial
                var historial = new SolRolFuncionHistorial()
                {
                    IdSolRolFuncionCabecera = cabecera.IdSolRolFuncionCabecera,
                    Comentario = string.Empty,
                    EstadoSolicitudHistorial = ((int)EstadoSolicitudRolFuncion.Atendido),
                    FechaCreacion = DateTime.Now,
                    CreadoPor = matricula,
                    FechaRevision = DateTime.Now,
                    RevisadoPor = nombre.ToUpper()
                };
                ctx.SolRolFuncionHistorial.Add(historial);
                ctx.SaveChanges();

                // 4. Actualizar tabla ChapterFuncionesRoles
                var listRolFunciones = ctx.SolicitudRolFuncion.Where(x => x.FlagEliminado == false &&
                                                                         x.IdSolicitud == idSolicitud).ToList();

                if (listRolFunciones != null && listRolFunciones.Count > 0)
                {
                    foreach (var itemFunc in listRolFunciones)
                    {
                        itemFunc.ModificadoPor = matricula;
                        itemFunc.FechaModificacion = DateTime.Now;

                        var chapterFunc = (from u in ctx.ChapterFuncionesRoles
                                            where u.FuncionProductoId == itemFunc.FuncionProductoId
                                            select u).FirstOrDefault();
                        //chapterFunc.EstadoFuncion = true;
                        if (cabecera.IdTipoSolicitud == (int)TipoSolicitudRolFunciones.Eliminacion)
                        {
                            chapterFunc.EstadoFuncion = (int)EstadoRolesChapter.AprobadoEliminacion;
                            chapterFunc.FlagActivo = false;
                            chapterFunc.FlagEliminado = true;
                        }
                        else 
                        {
                            chapterFunc.EstadoFuncion = (int)EstadoRolesChapter.AprobadoRegistro;
                        }
                        
                        chapterFunc.FechaModificacion = DateTime.Now;
                        chapterFunc.ModificadoPor = matricula;
                        ctx.SaveChanges();
                    }
                }

                // 5. Actualizar tabla RolesProducto
                var listRolProductos = (from u in ctx.SolicitudRolFuncion
                                        join a in ctx.ChapterFuncionesRoles on u.FuncionProductoId equals a.FuncionProductoId
                                        join b in ctx.RolesProducto on a.RolesProductoId equals b.RolesProductoId
                                        where u.IdSolicitud == idSolicitud
                                        select b.RolesProductoId
                                        ).Distinct().ToList();

                if (listRolProductos != null && listRolProductos.Count > 0)
                {
                    foreach (var itemRol in listRolProductos)
                    {
                        int total = (from u in ctx.ChapterFuncionesRoles
                                         where u.RolesProductoId == itemRol
                                         select u).Count();

                        int elim = (from u in ctx.ChapterFuncionesRoles
                                     where u.RolesProductoId == itemRol
                                     && u.FlagActivo == false && u.FlagEliminado == true
                                     select u).Count();
                        
                        var rolesProd = (from u in ctx.RolesProducto
                                         where u.RolesProductoId == itemRol
                                         select u).FirstOrDefault();

                        if (total == elim && cabecera.IdTipoSolicitud == (int)TipoSolicitudRolFunciones.Eliminacion) 
                        {
                            // Jul2022: Aunque sea la unica funcion, el rol ya no se elimina
                            //rolesProd.EstadoRol = (int)EstadoRolesChapter.AprobadoEliminacion;
                            //rolesProd.FlagActivo = false;
                            //rolesProd.FlagEliminado = true;
                            //rolesProd.EliminadoPor = matricula;
                            //rolesProd.FechaEliminacion = DateTime.Now;
                        }
                        else
                        {
                            rolesProd.EstadoRol = (int)EstadoRolesChapter.AprobadoRegistro;
                        }
                        
                        rolesProd.FechaModificacion = DateTime.Now;
                        rolesProd.ModificadoPor = matricula;
                        ctx.SaveChanges();

                    }
                }
            }
        }
        public void ActualizarEstadoAtendido_PorObservacion(int idSolicitud, SolRolFuncionCabecera cabecera, GestionCMDB_ProdEntities ctx, string nombre, string matricula)
        {
            List<SolRolHistorialDTO> historialReg = new List<SolRolHistorialDTO>();
            bool registro = false;

            if (cabecera.IdTipoSolicitud == (int)TipoSolicitudRolFunciones.Eliminacion)
            {
                historialReg = (from u in ctx.SolRolFuncionHistorial
                                where u.IdSolRolFuncionCabecera == cabecera.IdSolRolFuncionCabecera &&
                                      (u.EstadoSolicitudHistorial == (int)EstadoSolicitudRolFuncion.AprobadoOwner ||
                                      u.EstadoSolicitudHistorial == (int)EstadoSolicitudRolFuncion.Pendiente)
                                select new SolRolHistorialDTO()
                                {
                                    HistorialId = u.IdSolRolFuncionHistorial,
                                    EstadoSolicitud = u.EstadoSolicitudHistorial
                                }).ToList();

                //historialReg = historialReg.Where(x => x.HistorialId > historialReg.Where(p => p.EstadoSolicitud == (int)EstadoSolicitudRolFuncion.Pendiente).Max(p => p.HistorialId)
                //            ).ToList();

                //if (historialReg.Count == 1)
                //{
                //    registro = true;
                //}

                if (historialReg.Count > 0)
                {
                    registro = true;
                }
            }
            //else
            //{
            //    historialReg = (from u in ctx.SolRolFuncionHistorial
            //                    where u.IdSolRolFuncionCabecera == cabecera.IdSolRolFuncionCabecera &&
            //                          (u.EstadoSolicitudHistorial == (int)EstadoSolicitudRolFuncion.AprobadoOwner ||
            //                          u.EstadoSolicitudHistorial == (int)EstadoSolicitudRolFuncion.AprobadoSeguridad ||
            //                          u.EstadoSolicitudHistorial == (int)EstadoSolicitudRolFuncion.Pendiente)
            //                    select new SolRolHistorialDTO()
            //                    {
            //                        HistorialId = u.IdSolRolFuncionHistorial,
            //                        EstadoSolicitud = u.EstadoSolicitudHistorial
            //                    }).ToList();

            //    historialReg = historialReg.Where(x => x.HistorialId > historialReg.Where(p => p.EstadoSolicitud == (int)EstadoSolicitudRolFuncion.Pendiente).Max(p => p.HistorialId)
            //                ).ToList();

            //    if (historialReg.Count == 2)
            //    {
            //        registro = true;
            //    }
            //}

            if (registro)
            {
                // 2. Actualizar cabecera
                cabecera.EstadoSolicitudCabecera = (int)EstadoSolicitudRolFuncion.Atendido;
                cabecera.FechaRevision = DateTime.Now;
                cabecera.RevisadoPor = nombre.ToUpper();

                // 3. Actualizar historial
                var historial = new SolRolFuncionHistorial()
                {
                    IdSolRolFuncionCabecera = cabecera.IdSolRolFuncionCabecera,
                    Comentario = string.Empty,
                    EstadoSolicitudHistorial = ((int)EstadoSolicitudRolFuncion.Atendido),
                    FechaCreacion = DateTime.Now,
                    CreadoPor = matricula,
                    FechaRevision = DateTime.Now,
                    RevisadoPor = nombre.ToUpper()
                };
                ctx.SolRolFuncionHistorial.Add(historial);
                ctx.SaveChanges();

                // 4. Actualizar tabla ChapterFuncionesRoles
                var listRolFunciones = ctx.SolicitudRolFuncion.Where(x => x.FlagEliminado == false &&
                                                                         x.IdSolicitud == idSolicitud).ToList();

                if (listRolFunciones != null && listRolFunciones.Count > 0)
                {
                    foreach (var itemFunc in listRolFunciones)
                    {
                        itemFunc.ModificadoPor = matricula;
                        itemFunc.FechaModificacion = DateTime.Now;

                        var chapterFunc = (from u in ctx.ChapterFuncionesRoles
                                           where u.FuncionProductoId == itemFunc.FuncionProductoId
                                           select u).FirstOrDefault();
                        if (cabecera.IdTipoSolicitud == (int)TipoSolicitudRolFunciones.Eliminacion)
                        {
                            chapterFunc.EstadoFuncion = (int)EstadoRolesChapter.AprobadoRegistro;
                        }
                        //if (cabecera.IdTipoSolicitud == (int)TipoSolicitudRolFunciones.Eliminacion)
                        //{
                        //    chapterFunc.EstadoFuncion = (int)EstadoRolesChapter.AprobadoEliminacion;
                        //    chapterFunc.FlagActivo = false;
                        //    chapterFunc.FlagEliminado = true;
                        //}
                        //else
                        //{
                        //    chapterFunc.EstadoFuncion = (int)EstadoRolesChapter.AprobadoRegistro;
                        //}

                        chapterFunc.FechaModificacion = DateTime.Now;
                        chapterFunc.ModificadoPor = matricula;
                        ctx.SaveChanges();
                    }
                }

                // 5. Actualizar tabla RolesProducto
                var listRolProductos = (from u in ctx.SolicitudRolFuncion
                                        join a in ctx.ChapterFuncionesRoles on u.FuncionProductoId equals a.FuncionProductoId
                                        join b in ctx.RolesProducto on a.RolesProductoId equals b.RolesProductoId
                                        where u.IdSolicitud == idSolicitud
                                        select b.RolesProductoId
                                        ).Distinct().ToList();

                if (listRolProductos != null && listRolProductos.Count > 0)
                {
                    foreach (var itemRol in listRolProductos)
                    {
                        int total = (from u in ctx.ChapterFuncionesRoles
                                     where u.RolesProductoId == itemRol
                                     select u).Count();

                        int elim = (from u in ctx.ChapterFuncionesRoles
                                    where u.RolesProductoId == itemRol
                                    && u.FlagActivo == false && u.FlagEliminado == true
                                    select u).Count();

                        var rolesProd = (from u in ctx.RolesProducto
                                         where u.RolesProductoId == itemRol
                                         select u).FirstOrDefault();

                        //if (total == elim && cabecera.IdTipoSolicitud == (int)TipoSolicitudRolFunciones.Eliminacion)
                        //{
                        //    rolesProd.EstadoRol = (int)EstadoRolesChapter.AprobadoEliminacion;
                        //    rolesProd.FlagActivo = false;
                        //    rolesProd.FlagEliminado = true;
                        //    rolesProd.EliminadoPor = matricula;
                        //    rolesProd.FechaEliminacion = DateTime.Now;
                        //}
                        //else
                        //{
                        //    rolesProd.EstadoRol = (int)EstadoRolesChapter.AprobadoRegistro;
                        //}

                        //if (total == elim && cabecera.IdTipoSolicitud == (int)TipoSolicitudRolFunciones.Eliminacion)
                        if (cabecera.IdTipoSolicitud == (int)TipoSolicitudRolFunciones.Eliminacion)
                        {
                            rolesProd.EstadoRol = (int)EstadoRolesChapter.AprobadoRegistro;
                        }

                        rolesProd.FechaModificacion = DateTime.Now;
                        rolesProd.ModificadoPor = matricula;
                        ctx.SaveChanges();

                    }
                }
            }
        }
        public override List<SolRolHistorialDTO> GetHistorialSolicitud(PaginacionSolicitud pag, out int totalRows)
        {
            try
            {
                totalRows = 0;

                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        ctx.Database.CommandTimeout = 0;

                        var registros = (from u in ctx.SolRolFuncionCabecera
                                         join a in ctx.SolRolFuncionHistorial on u.IdSolRolFuncionCabecera equals a.IdSolRolFuncionCabecera
                                         where u.IdSolicitud == pag.SolicitudAplicacionId
                                         select new SolRolHistorialDTO()
                                         {
                                             HistorialId = a.IdSolRolFuncionHistorial,
                                             CabeceraId = u.IdSolRolFuncionCabecera,
                                             SolicitudId = u.IdSolicitud,
                                             EstadoSolicitud = a.EstadoSolicitudHistorial,
                                             CreadoPor = a.CreadoPor,
                                             RevisadoPor = a.RevisadoPor,
                                             Responsable = a.RevisadoPor,
                                             FechaCreacion = a.FechaCreacion,
                                             FechaRevision = a.FechaRevision,
                                             Comentario = a.Comentario,
                                             ObservacionOwner = a.ObservacionOwner,
                                             ObservacionSeguridad = a.ObservacionSeguridad


                                         }).OrderBy(pag.sortName + " " + pag.sortOrder).ToList();

                        totalRows = registros.Count();
                        var resultado = registros.Skip((pag.pageNumber - 1) * pag.pageSize).Take(pag.pageSize).ToList();

                        foreach (var item in resultado)
                        {
                            // Observado Owner
                            if (item.EstadoSolicitud == ((int)EstadoSolicitudRolFuncion.ObservadoOwner) || item.EstadoSolicitud == ((int)EstadoSolicitudRolFuncion.AprobadoOwner)) {
                                item.Comentario = item.ObservacionOwner;
                            } 
                            else if (item.EstadoSolicitud == ((int)EstadoSolicitudRolFuncion.ObservadoSeguridad) || item.EstadoSolicitud == ((int)EstadoSolicitudRolFuncion.AprobadoSeguridad))
                            {
                                item.Comentario = item.ObservacionSeguridad;
                            }

                        }

                        return resultado;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<SolRolHistorialDTO> GetHistorialSolicitud(PaginacionSolicitud pag, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<SolRolHistorialDTO> GetHistorialSolicitud(PaginacionSolicitud pag, out int totalRows)"
                    , new object[] { null });
            }
        }
        public override SolicitudRolDTO GetProductoObservadoById(int id)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var entidad = (from c in ctx.SolRolFuncionCabecera
                                            join u in ctx.SolicitudRolFuncion on c.IdSolicitud equals u.IdSolicitud
                                            join a in ctx.ChapterFuncionesRoles on u.FuncionProductoId equals a.FuncionProductoId
                                            join b in ctx.RolesProducto on a.RolesProductoId equals b.RolesProductoId
                                            where b.ProductoId == id &&
                                            (c.EstadoSolicitudCabecera == ((int)EstadoSolicitudRolFuncion.ObservadoOwner) ||
                                            c.EstadoSolicitudCabecera == ((int)EstadoSolicitudRolFuncion.ObservadoSeguridad))
                                            //select b.RolesProductoId
                                            select new SolicitudRolDTO()
                                            {
                                                Owner = c.OwnerProducto,
                                                OwnerNombre = c.NombreOwner,
                                                Seguridad = c.AnalistaSeguridad + "-" + c.BuzonSeguridad,
                                                SeguridadNombre = c.NombreAnalista,
                                                ProductoId = b.ProductoId,
                                                EstadoSolicitud = c.EstadoSolicitudCabecera,
                                                IdTipoSolicitud = c.IdTipoSolicitud
                                            }).Distinct().FirstOrDefault();

                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorProductoDTO
                    , "Error en el metodo: ProductoDTO GetProductoById(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorProductoDTO
                    , "Error en el metodo: ProductoDTO GetProductoById(int id)"
                    , new object[] { null });
            }
        }

        public override List<RolesProductoDTO> GetProductoRolesDetalle(PaginacionSolicitud pag, out int totalRows)
        {
            try
            {
                totalRows = 0;

                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        ctx.Database.CommandTimeout = 0;

                        var registros = (from b in ctx.Producto
                                         join u in ctx.RolesProducto on b.ProductoId equals u.ProductoId
                                         join a in ctx.ChapterFuncionesRoles on u.RolesProductoId equals a.RolesProductoId

                                         join c in ctx.SolicitudRolFuncion on a.FuncionProductoId equals c.FuncionProductoId
                                         where 
                                         u.ProductoId == pag.ProductoId && 
                                         c.FlagEliminado == false && c.IdSolicitud == pag.SolicitudAplicacionId
                                         select new RolesProductoDTO()
                                         {
                                             Id = u.RolesProductoId,
                                             SolicitudId = pag.SolicitudAplicacionId,
                                             Descripcion = u.Descripcion,
                                             TipoCuenta= u.TipoCuenta,
                                             ProductoId = u.ProductoId,
                                             Rol = u.Rol,
                                             GrupoRed = u.GrupoRed,
                                             Producto = b.Nombre,
                                             //EstadoRol = (u.EstadoRol == true) ? "Aprobado" : "Pendiente"
                                             EstadoRol = (u.EstadoRol == (int)EstadoRolesChapter.AprobadoRegistro || u.EstadoRol == (int)EstadoRolesChapter.AprobadoEliminacion) ? "Aprobado" : "Pendiente"

                                         }).OrderBy(pag.sortName + " " + pag.sortOrder).Distinct().ToList();

                        totalRows = registros.Count();
                        var resultado = registros.Skip((pag.pageNumber - 1) * pag.pageSize).Take(pag.pageSize).ToList();

                        foreach (var item in resultado)
                        {
                            var cantidad = (from c in ctx.SolicitudRolFuncion 
                                             join a in ctx.ChapterFuncionesRoles on c.FuncionProductoId equals a.FuncionProductoId
                                             where
                                             c.FlagEliminado == false && c.IdSolicitud == pag.SolicitudAplicacionId &&
                                             a.RolesProductoId == item.Id
                                             select c.FuncionProductoId).ToList();

                            //var cantidad = ctx.ChapterFuncionesRoles.Where(x => x.RolesProductoId == item.Id && x.FlagActivo == true && x.FlagEliminado == false).ToList();
                            if (cantidad != null)
                                item.FuncionesRelacionadas = cantidad.Count();
                            else if (cantidad == null)
                                item.FuncionesRelacionadas = 0;

                        }

                        return resultado;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<SolicitudDto> GetSolicitudes(PaginacionSolicitud pag, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<SolicitudDto> GetSolicitudes(PaginacionSolicitud pag, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<FuncionDTO> GetDetalleFuncionesProductosRoles(PaginacionSolicitud pag, out int totalRows)
        {
            try
            {
                totalRows = 0;

                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        ctx.Database.CommandTimeout = 0;

                        // 1. Obtener tipo de solicitud
                        int? tipo = (from d in ctx.SolRolFuncionCabecera
                                    where d.IdSolicitud == pag.SolicitudAplicacionId
                                    select d.IdTipoSolicitud).FirstOrDefault();

                        List<FuncionDTO> registros = new List<FuncionDTO>();

                        if (tipo == (int)TipoSolicitudRolFunciones.Registro)
                        {
                            registros = (from d in ctx.SolRolFuncionCabecera
                                             join c in ctx.SolicitudRolFuncion on d.IdSolicitud equals c.IdSolicitud
                                             join u in ctx.ChapterFuncionesRoles on c.FuncionProductoId equals u.FuncionProductoId
                                             join a in ctx.RolesProducto on u.RolesProductoId equals a.RolesProductoId
                                             join b in ctx.Producto on a.ProductoId equals b.ProductoId
                                             where
                                             c.IdSolicitud == pag.SolicitudAplicacionId && c.FlagEliminado == false &&
                                             u.RolesProductoId == pag.ProductoId //&& u.FlagActivo == true && u.FlagEliminado == false
                                             select new FuncionDTO()
                                             {
                                                 Id = u.FuncionProductoId,
                                                 Tribu = u.Tribu,
                                                 Chapter = u.Chapter,
                                                 Funcion = u.Funcion,
                                                 Producto = b.Nombre,
                                                 EstadoFuncion = (int)u.EstadoFuncion,
                                                 IdSolicitud = d.IdSolicitud,
                                                 IdEstadoSolicitud = d.EstadoSolicitudCabecera,
                                                 IdTipoSolicitud = d.IdTipoSolicitud
                                             }).OrderBy(pag.sortName + " " + pag.sortOrder).ToList();
                        }
                        else
                        {
                            registros = (from d in ctx.SolRolFuncionCabecera
                                         join c in ctx.SolicitudRolFuncion on d.IdSolicitud equals c.IdSolicitud
                                         join u in ctx.ChapterFuncionesRoles on c.FuncionProductoId equals u.FuncionProductoId
                                         join a in ctx.RolesProducto on u.RolesProductoId equals a.RolesProductoId
                                         join b in ctx.Producto on a.ProductoId equals b.ProductoId
                                         where
                                         c.IdSolicitud == pag.SolicitudAplicacionId && c.FlagEliminado == false &&
                                         u.RolesProductoId == pag.ProductoId //&& u.FlagActivo == true && u.FlagEliminado == false
                                         select new FuncionDTO()
                                         {
                                             Id = u.FuncionProductoId,
                                             Tribu = u.Tribu,
                                             Chapter = u.Chapter,
                                             Funcion = u.Funcion,
                                             Producto = b.Nombre,
                                             EstadoFuncion = (int)u.EstadoFuncion,
                                             IdSolicitud = d.IdSolicitud,
                                             IdEstadoSolicitud = d.EstadoSolicitudCabecera,
                                             IdTipoSolicitud = d.IdTipoSolicitud
                                         }).OrderBy(pag.sortName + " " + pag.sortOrder).ToList();
                        }

                        totalRows = registros.Count();
                        var resultado = registros.Skip((pag.pageNumber - 1) * pag.pageSize).Take(pag.pageSize).ToList();

                        int idEstadoTemp = 0;

                        if (registros[0].IdTipoSolicitud == (int)TipoSolicitudRolFunciones.Eliminacion && registros[0].IdEstadoSolicitud == (int)EstadoSolicitudRolFuncion.Atendido)
                        {
                            idEstadoTemp = (int)EstadoRolesChapter.AprobadoEliminacion;
                        }
                        //else if ((registros[0].IdTipoSolicitud == (int)TipoSolicitudRolFunciones.Registro || registros[0].IdTipoSolicitud == null) && registros[0].IdEstadoSolicitud == (int)EstadoSolicitudRolFuncion.Atendido) 
                        else if (registros[0].IdTipoSolicitud == (int)TipoSolicitudRolFunciones.Registro && registros[0].IdEstadoSolicitud == (int)EstadoSolicitudRolFuncion.Atendido)
                        {
                            idEstadoTemp = (int)EstadoRolesChapter.AprobadoRegistro;
                        }

                        if (idEstadoTemp > 0) 
                        {
                            foreach (var item in resultado) 
                            {
                                item.EstadoFuncion = idEstadoTemp;
                            }
                        }

                        return resultado;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<SolicitudDto> GetSolicitudes(PaginacionSolicitud pag, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<SolicitudDto> GetSolicitudes(PaginacionSolicitud pag, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<SolicitudRolCorreosDTO> ObtenerCorreoSolicitud(int idSolicitud, int idTipo)
        {
            List<SolicitudRolCorreosDTO> lista = new List<SolicitudRolCorreosDTO>();

            try
            {

                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var cabecera = ctx.SolRolFuncionCabecera.FirstOrDefault(x => x.IdSolicitud == idSolicitud);

                    if (idTipo == 1) 
                    {
                        // Especialista
                        var datEspecialista = new SolicitudRolCorreosDTO()
                        {
                            Matricula = cabecera.CreadoPor,
                            Mail = cabecera.BuzonEspecialista
                        };
                        lista.Add(datEspecialista);
                    }

                    if (idTipo == 2) // Analista de seguridad y Owner
                    {
                        // Seguridad
                        var regSeguridad = new SolicitudRolCorreosDTO()
                        {
                            Matricula = cabecera.AnalistaSeguridad,
                            Mail = cabecera.BuzonSeguridad
                        };
                        lista.Add(regSeguridad);

                        // Owner
                        var regOwner = new SolicitudRolCorreosDTO()
                        {
                            Matricula = cabecera.OwnerProducto,
                            Mail = cabecera.BuzonOwnerProducto
                        };
                        lista.Add(regOwner);
                    }

                    if (idTipo == 3) //  Owner
                    {
                        // Owner
                        var regOwner = new SolicitudRolCorreosDTO()
                        {
                            Matricula = cabecera.OwnerProducto,
                            Mail = cabecera.BuzonOwnerProducto
                        };
                        lista.Add(regOwner);
                    }

                    return lista;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: List<SolicitudRolCorreosDTO> ObtenerCorreoSolicitud(int idSolicitud)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: List<SolicitudRolCorreosDTO> ObtenerCorreoSolicitud(int idSolicitud)"
                    , new object[] { null });
            }
        }

        public override List<SolicitudRolResponsablesDTO> GetResponsablesPorSolicitud(PaginacionSolicitud pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {

                    List<SolicitudRolResponsablesDTO> registros = new List<SolicitudRolResponsablesDTO>();
                    SolicitudRolResponsablesDTO responsable = new SolicitudRolResponsablesDTO();

                    responsable = (from u in ctx.SolRolFuncionCabecera
                                   where u.IdSolicitud == pag.SolicitudAplicacionId
                                   select new SolicitudRolResponsablesDTO()
                                   {
                                       Responsable = u.NombreOwner.ToUpper(),
                                       BuzonResponsable = u.BuzonOwnerProducto,
                                       ResponsableId = (int)TipoResponsableSolicitud.Owner,
                                       TipoSolicitudId = (int)u.IdTipoSolicitud,
                                       SolicitudCabeceraId = (int)u.IdSolRolFuncionCabecera,
                                       EstadoSolicitudId = (int)u.EstadoSolicitudCabecera
                                   }).FirstOrDefault();

                    registros.Add(responsable);

                    if (pag.TipoSolicitud == (int)TipoSolicitudRolFunciones.Registro)
                    {
                        responsable = (from u in ctx.SolRolFuncionCabecera
                                       where u.IdSolicitud == pag.SolicitudAplicacionId
                                       select new SolicitudRolResponsablesDTO()
                                       {
                                           Responsable = u.NombreAnalista.ToUpper(),
                                           BuzonResponsable = u.BuzonSeguridad,
                                           ResponsableId = (int)TipoResponsableSolicitud.Seguridad,
                                           TipoSolicitudId = (int)u.IdTipoSolicitud,
                                           SolicitudCabeceraId = (int)u.IdSolRolFuncionCabecera,
                                           EstadoSolicitudId = (int)u.EstadoSolicitudCabecera
                                       }).FirstOrDefault();

                        registros.Add(responsable);
                    }

                    foreach (var item in registros) 
                    {
                        if (item.EstadoSolicitudId == (int)EstadoSolicitudRolFuncion.ObservadoOwner || item.EstadoSolicitudId == (int)EstadoSolicitudRolFuncion.ObservadoSeguridad)
                        {
                            item.EsAprobado = false;
                        }
                        else 
                        {
                            item.EsAprobado = SolicitudAprobadoResponsable(item.SolicitudCabeceraId, item.TipoSolicitudId, item.ResponsableId);
                        }
                    }

                    totalRows = registros.Count();
                    return registros;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: List<SolicitudRolDTO> GetResponsablesPorSolicitud(int idSolicitud, int idTipoSolicitud, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: List<SolicitudRolDTO> GetResponsablesPorSolicitud(int idSolicitud, int idTipoSolicitud, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override bool SolicitudAprobadoResponsable(int idCabecera, int idTipoSolicitud, int idTipoResponsable)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    List<SolRolHistorialDTO> historialReg = new List<SolRolHistorialDTO>();
                    bool registro = false;

                    if (idTipoSolicitud == (int)TipoSolicitudRolFunciones.Eliminacion) // Eliminación
                    {
                        historialReg = (from u in ctx.SolRolFuncionHistorial
                                        where u.IdSolRolFuncionCabecera == idCabecera &&
                                              (u.EstadoSolicitudHistorial == (int)EstadoSolicitudRolFuncion.AprobadoOwner ||
                                              u.EstadoSolicitudHistorial == (int)EstadoSolicitudRolFuncion.Pendiente) 
                                        select new SolRolHistorialDTO()
                                        {
                                            HistorialId = u.IdSolRolFuncionHistorial,
                                            EstadoSolicitud = u.EstadoSolicitudHistorial
                                        }).ToList();
                    }
                    else // Registro
                    {
                        if (idTipoResponsable == 1) // Owner
                        {
                            historialReg = (from u in ctx.SolRolFuncionHistorial
                                            where u.IdSolRolFuncionCabecera == idCabecera &&
                                                  (u.EstadoSolicitudHistorial == (int)EstadoSolicitudRolFuncion.AprobadoOwner ||
                                                  u.EstadoSolicitudHistorial == (int)EstadoSolicitudRolFuncion.Pendiente)
                                            select new SolRolHistorialDTO()
                                            {
                                                HistorialId = u.IdSolRolFuncionHistorial,
                                                EstadoSolicitud = u.EstadoSolicitudHistorial
                                            }).ToList();
                        }
                        else // Seguridad
                        {
                            historialReg = (from u in ctx.SolRolFuncionHistorial
                                            where u.IdSolRolFuncionCabecera == idCabecera &&
                                                  (u.EstadoSolicitudHistorial == (int)EstadoSolicitudRolFuncion.AprobadoSeguridad ||
                                                  u.EstadoSolicitudHistorial == (int)EstadoSolicitudRolFuncion.Pendiente)
                                            select new SolRolHistorialDTO()
                                            {
                                                HistorialId = u.IdSolRolFuncionHistorial,
                                                EstadoSolicitud = u.EstadoSolicitudHistorial
                                            }).ToList();
                        }
                    }

                    historialReg = historialReg.Where(x => x.HistorialId > historialReg.Where(p => p.EstadoSolicitud == (int)EstadoSolicitudRolFuncion.Pendiente).Max(p => p.HistorialId)
                                   ).ToList();

                    if (historialReg.Count == 1)
                    {
                        registro = true;
                    }

                    return registro;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: bool SolicitudAprobadoResponsable(int idSolicitud, int idTipoSolicitud, int idTipoResponsable, string matricula)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: bool SolicitudAprobadoResponsable(int idSolicitud, int idTipoSolicitud, int idTipoResponsable, string matricula)"
                    , new object[] { null });
            }
        }

        public override SolicitudRolDTO GetSolicitudPorId(int id)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {

                    //var ambientes = (from srf in ctx.SolicitudRolFuncion
                    //                 join cfr in ctx.ChapterFuncionesRoles on srf.FuncionProductoId equals cfr.FuncionProductoId
                    //                 join rpa in ctx.RolesProductoAmbiente on cfr.RolesProductoId equals rpa.RolesProductoId
                    //                 where srf.IdSolicitud == id
                    //                 select new RolesProductoAmbienteDTO()
                    //                 {
                    //                     RolesProductoAmbienteId = rpa.RolesProductoAmbienteId,
                    //                     RolesProductoId = rpa.RolesProductoId,
                    //                     Ambiente = rpa.Ambiente,
                    //                     GrupoRed = rpa.GrupoRed,
                    //                     FlagActivo = rpa.FlagActivo
                    //                 }).ToList();
                    var entidad = (from u in ctx.SolRolFuncionCabecera
                                   join a in ctx.Producto on u.IdProducto equals a.ProductoId
                                   //from a in lja.DefaultIfEmpty()
                                       //join tp in ctx.TablaProcedencia on (int)ETablaProcedencia.CVT_Producto equals tp.CodigoInterno
                                       //join f in ctx.ArchivosCVT on new { Id = u.ProductoId.ToString(), TablaProcedenciaId = tp.TablaProcedenciaId } equals new { Id = f.EntidadId, TablaProcedenciaId = f.TablaProcedenciaId } into jl
                                       //from f in jl.DefaultIfEmpty()
                                   where u.IdSolicitud == id
                                   //&& f.Activo
                                   select new SolicitudRolDTO()
                                   {
                                       SolicitudId = u.IdSolicitud,
                                       IdTipoSolicitud = u.IdTipoSolicitud,
                                       ProductoId = u.IdProducto,
                                       Codigo = a.Codigo,
                                       Producto = string.Concat(a.Fabricante, " ", a.Nombre),
                                       EstadoSolicitud = u.EstadoSolicitudCabecera,
                                       Owner = u.OwnerProducto,
                                       OwnerNombre = u.NombreOwner,
                                       Seguridad = u.AnalistaSeguridad,
                                       SeguridadNombre = u.NombreAnalista,
                                       //Fabricante = u.Fabricante,
                                       //Nombre = u.Nombre,
                                       //Descripcion = u.Descripcion,
                                       //DominioId = u.DominioId,
                                       
                                   }).FirstOrDefault();
                    return entidad;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorProductoDTO
                    , "Error en el metodo: ProductoDTO GetProductoById(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorProductoDTO
                    , "Error en el metodo: ProductoDTO GetProductoById(int id)"
                    , new object[] { null });
            }
        }

        public override List<FuncionDTO> GetDetalleSolicitudRoles(PaginacionSolicitud pag, out int totalRows)
        {
            try
            {
                totalRows = 0;

                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        ctx.Database.CommandTimeout = 0;

                        // 1. Obtener tipo de solicitud
                        int? tipo = (from d in ctx.SolRolFuncionCabecera
                                     where d.IdSolicitud == pag.SolicitudAplicacionId
                                     select d.IdTipoSolicitud).FirstOrDefault();

                        List<FuncionDTO> registros = new List<FuncionDTO>();

                        if (tipo == (int)TipoSolicitudRolFunciones.Registro)
                        {
                            registros = (from d in ctx.SolRolFuncionCabecera
                                         join c in ctx.SolicitudRolFuncion on d.IdSolicitud equals c.IdSolicitud
                                         join u in ctx.ChapterFuncionesRoles on c.FuncionProductoId equals u.FuncionProductoId
                                         join a in ctx.RolesProducto on u.RolesProductoId equals a.RolesProductoId
                                         join b in ctx.Producto on a.ProductoId equals b.ProductoId
                                         where
                                         c.IdSolicitud == pag.SolicitudAplicacionId && c.FlagEliminado == false 
                                         //&& u.RolesProductoId == pag.ProductoId 
                                         select new FuncionDTO()
                                         {
                                             Id = u.FuncionProductoId,
                                             Rol = a.Rol,
                                             DescripcionRol = a.Descripcion,
                                             TipoCuenta = a.TipoCuenta,
                                             GrupoRed = a.GrupoRed,
                                             Tribu = u.Tribu,
                                             Chapter = u.Chapter,
                                             Funcion = u.Funcion,
                                             Producto = b.Nombre,
                                             EstadoFuncion = (int)u.EstadoFuncion,
                                             IdSolicitud = d.IdSolicitud,
                                             IdEstadoSolicitud = d.EstadoSolicitudCabecera,
                                             IdTipoSolicitud = d.IdTipoSolicitud
                                         })
                                         .OrderBy(pag.sortName + " " + pag.sortOrder)
                                         .ToList();
                        }
                        else
                        {
                            registros = (from d in ctx.SolRolFuncionCabecera
                                         join c in ctx.SolicitudRolFuncion on d.IdSolicitud equals c.IdSolicitud
                                         join u in ctx.ChapterFuncionesRoles on c.FuncionProductoId equals u.FuncionProductoId
                                         join a in ctx.RolesProducto on u.RolesProductoId equals a.RolesProductoId
                                         join b in ctx.Producto on a.ProductoId equals b.ProductoId
                                         where
                                         c.IdSolicitud == pag.SolicitudAplicacionId && c.FlagEliminado == false 
                                         //&& u.RolesProductoId == pag.ProductoId 
                                         select new FuncionDTO()
                                         {
                                             Id = u.FuncionProductoId,
                                             Rol = a.Rol,
                                             DescripcionRol = a.Descripcion,
                                             TipoCuenta = a.TipoCuenta,
                                             GrupoRed = a.GrupoRed,
                                             Tribu = u.Tribu,
                                             Chapter = u.Chapter,
                                             Funcion = u.Funcion,
                                             Producto = b.Nombre,
                                             EstadoFuncion = (int)u.EstadoFuncion,
                                             IdSolicitud = d.IdSolicitud,
                                             IdEstadoSolicitud = d.EstadoSolicitudCabecera,
                                             IdTipoSolicitud = d.IdTipoSolicitud
                                         })
                                         .OrderBy(pag.sortName + " " + pag.sortOrder)
                                         .ToList();
                        }

                        totalRows = registros.Count();
                        if (totalRows > 0)
                        {
                            var resultado = registros.Skip((pag.pageNumber - 1) * pag.pageSize).Take(pag.pageSize).ToList();

                            int idEstadoTemp = 0;

                            if (registros[0].IdTipoSolicitud == (int)TipoSolicitudRolFunciones.Eliminacion && registros[0].IdEstadoSolicitud == (int)EstadoSolicitudRolFuncion.Atendido)
                            {
                                idEstadoTemp = (int)EstadoRolesChapter.AprobadoEliminacion;
                            }

                            else if (registros[0].IdTipoSolicitud == (int)TipoSolicitudRolFunciones.Registro && registros[0].IdEstadoSolicitud == (int)EstadoSolicitudRolFuncion.Atendido)
                            {
                                idEstadoTemp = (int)EstadoRolesChapter.AprobadoRegistro;
                            }

                            if (idEstadoTemp > 0)
                            {
                                foreach (var item in resultado)
                                {
                                    item.EstadoFuncion = idEstadoTemp;
                                }
                            }
                            foreach (var item in resultado)
                            {
                                //var ambientes = (from a in ctx.RolesProductoAmbiente
                                //                 where a.RolesProductoId == item.Id && a.FlagActivo == true
                                //                 select new RolesProductoAmbienteDTO()
                                //                 {
                                //                     RolesProductoAmbienteId = a.RolesProductoAmbienteId,
                                //                     RolesProductoId = a.RolesProductoId,
                                //                     Ambiente = a.Ambiente,
                                //                     GrupoRed = a.GrupoRed,
                                //                     FlagActivo = a.FlagActivo
                                //                 }).ToList();
                                var ambientes = (from cfr in ctx.ChapterFuncionesRoles
                                                 join rpa in ctx.RolesProductoAmbiente on cfr.RolesProductoId equals rpa.RolesProductoId
                                                 where cfr.FuncionProductoId == item.Id && rpa.FlagActivo == true
                                                 select new RolesProductoAmbienteDTO()
                                                 {
                                                     RolesProductoAmbienteId = rpa.RolesProductoAmbienteId,
                                                     RolesProductoId = rpa.RolesProductoId,
                                                     Ambiente = rpa.Ambiente,
                                                     GrupoRed = rpa.GrupoRed,
                                                     FlagActivo = rpa.FlagActivo
                                                 }).ToList();
                                item.Ambiente = ambientes;
                            }
                            return resultado;
                        }
                        else
                        {
                            return registros;
                        }
                    }  
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<FuncionDTO> GetDetalleSolicitudRoles(PaginacionSolicitud pag, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<FuncionDTO> GetDetalleSolicitudRoles(PaginacionSolicitud pag, out int totalRows)"
                    , new object[] { null });
            }
        }
        public override List<RolesProductoAmbienteDTO> GetAmbienteRolProductoByIdSolicitud(int id)
        {
            try
            {
                using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                {
                    var ambientes = (from srf in ctx.SolicitudRolFuncion
                                     join cfr in ctx.ChapterFuncionesRoles on srf.FuncionProductoId equals cfr.FuncionProductoId
                                     join rpa in ctx.RolesProductoAmbiente on cfr.RolesProductoId equals rpa.RolesProductoId
                                     where srf.IdSolicitud == id
                                     select new RolesProductoAmbienteDTO()
                                     {
                                         RolesProductoAmbienteId = rpa.RolesProductoAmbienteId,
                                         RolesProductoId = rpa.RolesProductoId,
                                         Ambiente = rpa.Ambiente,
                                         GrupoRed = rpa.GrupoRed,
                                         FlagActivo = rpa.FlagActivo
                                     }).ToList();
                    return ambientes;
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: List<ActivosDTO> GetApplicationByUser(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorActivosDTO
                    , "Error en el metodo: List<ActivosDTO> GetApplicationByUser(string filtro, int pageNumber, int pageSize, string sortName, string sortOrder, out int totalRows)"
                    , new object[] { null });
            }
        }

        #region Catálogo de Tecnologías
        public override List<RolesProductoDTO> GetProductoRolesDetalleCatalogo(PaginacionSolicitud pag, out int totalRows)
        {
            try
            {
                totalRows = 0;

                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        ctx.Database.CommandTimeout = 0;

                        var registros = (from b in ctx.Producto
                                         join u in ctx.RolesProducto on b.ProductoId equals u.ProductoId
                                         where 
                                         u.ProductoId == pag.ProductoId 
                                         && u.FlagActivo == true && u.FlagEliminado == false && u.EstadoRol == (int)EstadoRolesChapter.AprobadoRegistro
                                         select new RolesProductoDTO()
                                         {
                                             Id = u.RolesProductoId,
                                             ProductoId = u.ProductoId,
                                             Descripcion = u.Descripcion,
                                             TipoCuenta = u.TipoCuenta,
                                             Rol = u.Rol,
                                             GrupoRed = u.GrupoRed

                                         }).OrderBy(pag.sortName + " " + pag.sortOrder).ToList();

                        totalRows = registros.Count();
                        var resultado = registros.Skip((pag.pageNumber - 1) * pag.pageSize).Take(pag.pageSize).ToList();

                        foreach (var item in resultado)
                        {
                            var ambientes = (from a in ctx.RolesProductoAmbiente
                                             where a.RolesProductoId == item.Id && a.FlagActivo == true
                                             select new RolesProductoAmbienteDTO()
                                             {
                                                 RolesProductoAmbienteId = a.RolesProductoAmbienteId,
                                                 RolesProductoId = a.RolesProductoId,
                                                 Ambiente = a.Ambiente,
                                                 GrupoRed = a.GrupoRed,
                                                 FlagActivo = a.FlagActivo
                                             }).ToList();
                            item.Ambiente = ambientes;

                        }
                        return resultado;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<RolesProductoDTO> GetProductoRolesDetalleCatalogo(PaginacionSolicitud pag, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<RolesProductoDTO> GetProductoRolesDetalleCatalogo(PaginacionSolicitud pag, out int totalRows)"
                    , new object[] { null });
            }
        }
        public override List<FuncionDTO> GetProductoFuncionesDetalleCatalogo(PaginacionSolicitud pag, out int totalRows)
        {
            try
            {
                totalRows = 0;

                using (var scope = new TransactionScope(TransactionScopeOption.RequiresNew, new TransactionOptions { IsolationLevel = IsolationLevel.ReadUncommitted }))
                {
                    using (var ctx = GestionCMDB_ProdEntities.ConnectToSqlServer())
                    {
                        ctx.Database.CommandTimeout = 0;

                        List<FuncionDTO> registros = new List<FuncionDTO>();

                        
                            registros = (from u in ctx.RolesProducto //on b.ProductoId equals u.ProductoId
                                         join c in ctx.ChapterFuncionesRoles on u.RolesProductoId equals c.RolesProductoId
                                         where
                                         u.ProductoId == pag.ProductoId
                                         && u.FlagActivo == true && u.FlagEliminado == false //&& u.EstadoRol == (int)EstadoRolesChapter.AprobadoRegistro
                                         && c.FlagActivo == true && c.FlagEliminado == false && (c.EstadoFuncion == (int)EstadoRolesChapter.AprobadoRegistro || c.EstadoFuncion == 2)
                                         select new FuncionDTO()
                                         {
                                             Rol = u.Rol,
                                             Tribu = c.Tribu,
                                             Chapter = c.Chapter,
                                             Funcion = c.Funcion                                             
                                         }).OrderBy(pag.sortName + " " + pag.sortOrder).ToList();

                        totalRows = registros.Count();
                        var resultado = registros.Skip((pag.pageNumber - 1) * pag.pageSize).Take(pag.pageSize).ToList();

                        return resultado;
                    }
                }
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<FuncionDTO> GetProductoFuncionesDetalleCatalogo(PaginacionSolicitud pag, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorAplicacionDTO
                    , "Error en el metodo: List<FuncionDTO> GetProductoFuncionesDetalleCatalogo(PaginacionSolicitud pag, out int totalRows)"
                    , new object[] { null });
            }
        }
        #endregion
    }
}
