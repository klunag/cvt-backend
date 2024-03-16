using BCP.CVT.DTO;
using BCP.CVT.DTO.Appliance;
using BCP.CVT.Services.Interface.Appliance;
using BCP.CVT.Services.ModelDB;
using System.Linq;

namespace BCP.CVT.Services.Service.Appliance
{
    public class ApplianceEquipoSvc : ApplianceEquipoDAO
    {
        private readonly GestionCMDB_ProdEntities context = GestionCMDB_ProdEntities.ConnectToSqlServer();

        //public ApplianceEquipoSvc(GestionCMDB_ProdEntities _context)
        //{
        //    context = _context;
        //}

        public override Equipo GetEquipo(int equipoId)
        {
            return context.Equipo.Where(i => i.EquipoId == equipoId).FirstOrDefault();
        }

        public override EquipoSoftwareBase GetEquipoSoftwareBase(int equipoId)
        {
            return context.EquipoSoftwareBase.Where(x => x.EquipoId == equipoId).FirstOrDefault();
        }

        public override EquipoSolicitud GetEquipoSolicitud(int id)
        { 
            return context.EquipoSolicitud.Where(x => x.EquipoSolicitudId == id).FirstOrDefault(); 
        }

        public override EquipoSolicitudDTO GetEquipoSolicitudXId(int equipoId)
        {
            var query = context.EquipoSolicitud
                        .GroupJoin(context.Equipo,
                            x => x.EquipoId, y => y.EquipoId, 
                            (x, y) =>
                            new
                            {
                                equipoSolicitud = x,
                                equipo = y
                            }
                          ).SelectMany(
                          lft => lft.equipo.DefaultIfEmpty(),
                          (lft, eq) =>
                             new
                             {
                                 equipo_solicitud = lft.equipoSolicitud,
                                 equipo_principal = eq
                             }).Where(i => i.equipo_solicitud.EquipoSolicitudId == equipoId) 
                          .OrderBy(k => k.equipo_solicitud.FechaCreacion) 
                          .Select(m => new EquipoSolicitudDTO
                          {
                              Id = m.equipo_solicitud.EquipoSolicitudId,
                              UsuarioCreacion = m.equipo_solicitud.CreadoPor,
                              FechaCreacion = m.equipo_solicitud.FechaCreacion.Value,
                              FechaModificacion = m.equipo_solicitud.FechaModificacion,
                              UsuarioModificacion = m.equipo_solicitud.ModificadoPor,
                              AprobadoRechazadoPor = m.equipo_solicitud.AprobadoRechazadoPor,
                              Comentarios = m.equipo_solicitud.Comentarios,
                              ArchivoConstancia = m.equipo_solicitud.ArchivoConstancia,
                              EquipoId = m.equipo_solicitud.EquipoId,
                              EstadoSolicitud = m.equipo_solicitud.EstadoSolicitud,
                              FechaAprobacionRechazo = m.equipo_solicitud.FechaAprobacionRechazo,
                              FechaFinSoporte = m.equipo_solicitud.FechaFinSoporte,
                              NombreUsuarioAprobadoRechazo = m.equipo_solicitud.NombreUsuarioAprobadoRechazo,
                              NombreUsuarioCreacion = m.equipo_solicitud.NombreUsuarioCreacion,
                              NombreUsuarioModificacion = m.equipo_solicitud.NombreUsuarioModificacion,
                              TipoEquipoActual = m.equipo_solicitud.TipoEquipoActual,
                              TipoEquipoSolicitado = m.equipo_solicitud.TipoEquipoSolicitado,
                              NombreEquipo = (string.IsNullOrEmpty(m.equipo_principal.Nombre) ? m.equipo_solicitud.NombreEquipo : m.equipo_principal.Nombre),
                              ComentariosAprobacionRechazo = m.equipo_solicitud.ComentariosAprobacionRechazo,
                              ComentariosDesestimacion = m.equipo_solicitud.ComentariosDesestimacion,
                              NombreArchivo = m.equipo_solicitud.NombreArchivo,
                              CorreoSolicitante = m.equipo_solicitud.CorreoSolicitante,
                              FlagRegistroEquipo = m.equipo_solicitud.FlagRegistroEquipo
                          }).FirstOrDefault();

            return query;
        }

        public override Relacion GetRealcionExistente(RelacionApplianceDTO relacionApplianceDTO)
        {
            return context.Relacion.FirstOrDefault(x => x.EquipoId    == relacionApplianceDTO.EquipoId
                                                    && x.CodigoAPT    == relacionApplianceDTO.CodigoAPT
                                                    && x.AmbienteId   == relacionApplianceDTO.AmbienteId
                                                    && x.DiaRegistro  == relacionApplianceDTO.DiaRegistro
                                                    && x.MesRegistro  == relacionApplianceDTO.MesRegistro
                                                    && x.AnioRegistro == relacionApplianceDTO.AnioRegistro);
        }

        public override TipoNotificacion GetTipoNotificacion(string notification_type)
        {
            return context.TipoNotificacion.FirstOrDefault(o => o.Nombre == notification_type.ToString());
        }
    }
}
