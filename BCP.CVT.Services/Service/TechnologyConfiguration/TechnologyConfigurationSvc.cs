using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.DTO.Grilla;
using BCP.CVT.Services.Interface.TechnologyConfiguration;
using BCP.CVT.Services.ModelDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.SqlClient;

namespace BCP.CVT.Services.Service.TechnologyConfiguration
{
    public class TechnologyConfigurationSvc : TechnologyConfigurationDAO
    {
        private readonly GestionCMDB_ProdEntities context = GestionCMDB_ProdEntities.ConnectToSqlServer();
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public override string GetNewDomainValue(int id)
        {
            return context.Dominio.Where(x => x.DominioId == id).FirstOrDefault().Nombre;
        }

        public override string GetNewSubDomainValue(int id)
        {
            return context.Subdominio.Where(x => x.SubdominioId == id).FirstOrDefault().Nombre;
        }

        public override string GetNewValueTypeLifeCycle(int? id)
        {
            var response = string.Empty;
            if (string.IsNullOrEmpty(id.ToString()))
            {
                response = string.Empty;
            } 
            else if(id == -1)
            {
                response = string.Empty;
            }
            else
            {
                response = context.TipoCicloVida.Where(x => x.TipoCicloVidaId == id).FirstOrDefault().Nombre;
            }   
            return response;
        }

        public override string GetNewValueType(int? id)
        {
            var response = string.Empty;

            if (!string.IsNullOrEmpty(id.ToString()) && id != -1)
                response = context.Tipo.Where(x => x.TipoId == (int)id).FirstOrDefault().Nombre;

            return response;
        }

        public override List<SolicitudFlujoDetalleDTO> GetRequestFlowDetail(int idApplication)
        {
            return context.SolicitudTecnologiaCampos.Join(context.ConfiguracionTecnologiaCampos, s => (int)s.ConfiguracionTecnologiaCamposId, c => (int)c.ConfiguracionTecnologiaCamposId, (s, c) =>
                    new {
                        solicitudTecnolgia = s,
                        configuracionTecnologia = c
                    }).Where(d => d.solicitudTecnolgia.SolicitudTecnologiaId == idApplication)
                        .Select(config => new SolicitudFlujoDetalleDTO
                        {
                            NombreCampo = config.configuracionTecnologia.NombreCampo.ToString(),
                            TablaProcedencia = config.configuracionTecnologia.TablaProcedencia.ToString(),
                            RolAprueba = config.configuracionTecnologia.RolAprueba.ToString(),
                            ValorAnterior = config.solicitudTecnolgia.ValorAnterior.ToString(),
                            ValorNuevo = config.solicitudTecnolgia.ValorNuevo.ToString(),
                            SolicitudTecnologiaCamposId = (int)config.solicitudTecnolgia.SolicitudTecnologiaCamposId,
                            ConfiguracionTecnologiaCamposId = config.configuracionTecnologia.CorrelativoCampo,
                            EstadoCampo = config.solicitudTecnolgia.EstadoCampo,
                            CorrelativoCampo = config.configuracionTecnologia.CorrelativoCampo,
                            FlagConfiguration = config.configuracionTecnologia.FlagConfiguracion
                        }).ToList();
        }

        public override List<FlujoSolicitudTecnologiaDTO> GetTechonologyConfigurationFields()
        {
            return context.SolicitudTecnologiaCampos.Join(context.ConfiguracionTecnologiaCampos, soltec => soltec.ConfiguracionTecnologiaCamposId, conftec => (int)conftec.ConfiguracionTecnologiaCamposId, (soltec, conftec) =>
                        new {
                            solicitudTecnolgia = soltec,
                            configuracionTecnologia = conftec
                        }).Where(x => x.configuracionTecnologia.RolAprueba == "CVT" && x.configuracionTecnologia.FlagActivo == 1)
                            .Select(config => new FlujoSolicitudTecnologiaDTO
                            {
                                SolicitudTecnologiaId = config.solicitudTecnolgia.SolicitudTecnologiaId,
                                ConfiguracionTecnologiaCamposId = config.solicitudTecnolgia.ConfiguracionTecnologiaCamposId,
                                CorrelativoCampo = config.configuracionTecnologia.CorrelativoCampo,
                                ValorNuevo = config.solicitudTecnolgia.ValorNuevo.ToString(),
                                ValorAnterior = config.solicitudTecnolgia.ValorAnterior.ToString(),
                                NombreCampo = config.configuracionTecnologia.NombreCampo.ToString(),
                                TablaProcedencia = config.configuracionTecnologia.TablaProcedencia.ToString(),
                                DescripcionCampo = config.configuracionTecnologia.DescripcionCampo.ToString(),
                                FlagConfiguracion = config.configuracionTecnologia.FlagConfiguracion
                            }).ToList();
        }

        public override Aplicacion GetNewValueApplication(int id)
        {
            return context.Aplicacion.Where(x => x.AplicacionId == id).FirstOrDefault();
        }

        public override string GetTechnologyReason(int id)
        {
            return context.Motivo.Where(x => x.MotivoId == id).FirstOrDefault().Nombre.ToString();
        }

        public override string GetTechnologyRequest(int id)
        {
            return context.SolicitudTecnologia.Where(x => x.SolicitudTecnologiaId == id).FirstOrDefault().MotivoDesactiva.ToString();
        }

        public override Tecnologia GetTechnology(int id)
        {
            return context.Tecnologia.Where(t => t.TecnologiaId == id).FirstOrDefault();
        }

        public override Producto GetProducto(int id)
        {
            return context.Producto.Where(p => p.ProductoId == id).FirstOrDefault();
        }

        public override int GetIdTechnologyConfiguration(int id)
        {
            return (int)context.ConfiguracionTecnologiaCampos.Where(x => x.CorrelativoCampo == id).Select(d => d.ConfiguracionTecnologiaCamposId).FirstOrDefault();
        }

        public override string GetTechnologyEquivalence(int id, int idTecnologiaEquivalencia)
        {
            return context.TecnologiaEquivalencia.Where(x => x.TecnologiaId == id && x.TecnologiaEquivalenciaId == idTecnologiaEquivalencia).Select(d => d.Nombre).FirstOrDefault();
        }

        public override string GetApplicationTechnology(int id)
        {
            return context.Aplicacion.Where(x => x.AplicacionId == id).Select(d => d.Nombre).FirstOrDefault();
        }

        public override TecnologiaDTO GetAllTechonologyXId(int id)
        {
            try
            {
                var result = context.Tecnologia.Join(context.Subdominio, t => t.SubdominioId, s => s.SubdominioId, (t, s) => new { technology = t, subdomain = s })
                                                    .Join(context.Dominio, sb => sb.subdomain.DominioId, d => d.DominioId, (sb, d) => new { sb.technology, sb.subdomain, domain = d })
                                                    .Join(context.Producto, dm => dm.technology.ProductoId, p => p.ProductoId, (dm, p) => new { dm.technology, dm.subdomain, dm.domain, product = p })
                                                   
                                                    .Where(o => o.technology.TecnologiaId == id)
                                                    .Select(tech => new TecnologiaDTO
                                                    {
                                                        Id = tech.technology.TecnologiaId,
                                                        ProductoId = tech.technology.ProductoId,
                                                        CodigoProducto = tech.technology.ProductoId == null ? tech.technology.CodigoProducto : string.IsNullOrEmpty(tech.technology.CodigoProducto) ? tech.product == null ? "" : tech.product.Codigo : tech.technology.CodigoProducto,
                                                        Fabricante = (string.IsNullOrEmpty(tech.technology.Fabricante) ? string.Empty : tech.product.Fabricante),
                                                        Nombre = (string.IsNullOrEmpty(tech.technology.Nombre) ? string.Empty : tech.technology.Nombre),
                                                        Versiones = (string.IsNullOrEmpty(tech.technology.Versiones) ? string.Empty : tech.technology.Versiones),
                                                        ClaveTecnologia = (string.IsNullOrEmpty(tech.technology.ClaveTecnologia) ? string.Empty : tech.technology.ClaveTecnologia),
                                                        Descripcion = (string.IsNullOrEmpty(tech.technology.Descripcion) ? string.Empty : tech.technology.Descripcion),
                                                        DescripcionProducto = (string.IsNullOrEmpty(tech.product.Descripcion) ? string.Empty : tech.product.Descripcion),
                                                        DominioId = tech.product.DominioId,
                                                        DominioNomb = (string.IsNullOrEmpty(tech.domain.Nombre) ? string.Empty : tech.domain.Nombre),
                                                        SubdominioId = tech.subdomain.SubdominioId,
                                                        SubdominioNomb = (string.IsNullOrEmpty(tech.subdomain.Nombre) ? string.Empty : tech.subdomain.Nombre),
                                                        TipoProductoId = tech.product.TipoProductoId,
                                                        TipoCicloVidaId = tech.product.TipoCicloVidaId,
                                                        EsqLicenciamientoId = tech.product.EsquemaLicenciamientoSuscripcionId,
                                                        FlagSiteEstandar = tech.technology.FlagSiteEstandar,
                                                        FlagTieneEquivalencias = (tech.technology.FlagTieneEquivalencias != false ? true : false),
                                                        MotivoId = tech.technology.MotivoId,
                                                        // === Tab 1 - General
                                                        TipoTecnologiaId = tech.technology.TipoTecnologia,
                                                        AutomatizacionImplementadaId = tech.technology.AutomatizacionImplementadaId,
                                                        RevisionSeguridadId = tech.technology.RevisionSeguridadId,
                                                        RevisionSeguridadDescripcion = (string.IsNullOrEmpty(tech.technology.RevisionSeguridadDescripcion) ? string.Empty : tech.technology.RevisionSeguridadDescripcion),
                                                        LineaBaseSeg = (string.IsNullOrEmpty(tech.technology.LineaBaseSeg) ? string.Empty : tech.technology.LineaBaseSeg),


                                                        FechaLanzamiento = tech.technology.FechaLanzamiento,
                                                        FlagFechaFinSoporte = tech.technology.FlagFechaFinSoporte,
                                                        // SI tiene fecha de fin de soporte
                                                        Fuente = tech.technology.FuenteId,
                                                        FechaCalculoTec = tech.technology.FechaCalculoTec,
                                                        FechaExtendida = tech.technology.FechaExtendida,
                                                        FechaFinSoporte = tech.technology.FechaFinSoporte,
                                                        FechaAcordada = tech.technology.FechaAcordada,
                                                        ComentariosFechaFin = (string.IsNullOrEmpty(tech.technology.ComentariosFechaFin) ? string.Empty : tech.technology.ComentariosFechaFin),
                                                        // NO tiene fecha de fin de soporte
                                                        SustentoMotivo = (string.IsNullOrEmpty(tech.technology.SustentoMotivo) ? string.Empty : tech.technology.SustentoMotivo),
                                                        SustentoUrl = (string.IsNullOrEmpty(tech.technology.SustentoUrl) ? string.Empty : tech.technology.SustentoUrl),

                                                        UrlConfluenceId = tech.technology.UrlConfluenceId,
                                                        UrlConfluence = (string.IsNullOrEmpty(tech.technology.UrlConfluence) ? string.Empty : tech.technology.UrlConfluence),
                                                        CasoUso = (string.IsNullOrEmpty(tech.technology.CasoUso) ? string.Empty : tech.technology.CasoUso),
                                                        Aplica = (string.IsNullOrEmpty(tech.technology.Aplica) ? string.Empty : tech.technology.Aplica),
                                                        CompatibilidadSOId = (string.IsNullOrEmpty(tech.technology.CompatibilidadSOId) ? string.Empty : tech.technology.CompatibilidadSOId),
                                                        CompatibilidadCloudId = (string.IsNullOrEmpty(tech.technology.CompatibilidadCloudId) ? string.Empty : tech.technology.CompatibilidadCloudId),
                                                        EsqMonitoreo = (string.IsNullOrEmpty(tech.technology.EsqMonitoreo) ? string.Empty : tech.technology.EsqMonitoreo),
                                                        EstadoId = tech.technology.EstadoId,
                                                        //Deprecado
                                                        FechaDeprecada = tech.technology.FechaDeprecada,                                                    
                                                        TecReemplazoDepId = tech.technology.TecReemplazoDepId,
                                                        //
                                                        // === Tab 2 - Responsabilidades

                                                        TribuCoeId = (string.IsNullOrEmpty(tech.product.TribuCoeId) ? string.Empty : tech.product.TribuCoeId),
                                                        TribuCoeDisplayName = (string.IsNullOrEmpty(tech.product.TribuCoeDisplayName) ? string.Empty : tech.product.TribuCoeDisplayName),
                                                        // CAR 1
                                                        SquadId = (string.IsNullOrEmpty(tech.product.SquadId) ? string.Empty : tech.product.SquadId),
                                                        SquadDisplayName = (string.IsNullOrEmpty(tech.product.SquadDisplayName) ? string.Empty : tech.product.SquadDisplayName),
                                                        OwnerId = (string.IsNullOrEmpty(tech.product.OwnerId) ? string.Empty : tech.product.OwnerId),
                                                        OwnerMatricula = (string.IsNullOrEmpty(tech.product.OwnerMatricula) ? string.Empty : tech.product.OwnerMatricula),
                                                        OwnerNombre = (string.IsNullOrEmpty(tech.product.OwnerDisplayName) ? string.Empty : tech.product.OwnerDisplayName),
                                                        LiderUsuarioAutorizador = "",
                                                        //ConfArqSeg = u.ConfArqSegId,
                                                        ConfArqSegId = (string.IsNullOrEmpty(tech.technology.ConfArqSegId) ? string.Empty : tech.technology.ConfArqSegId),
                                                        ConfArqSegDisplayName = "",
                                                        // CAR 2
                                                        //ConfArqTec = u.ConfArqTecId,
                                                        ConfArqTecId = (string.IsNullOrEmpty(tech.technology.ConfArqTecId) ? string.Empty : tech.technology.ConfArqTecId),
                                                        ConfArqTecDisplayName = "",
                                                        // CAR 3 
                                                        EquipoAprovisionamiento = (string.IsNullOrEmpty(tech.technology.EquipoAprovisionamiento) ? string.Empty : tech.technology.EquipoAprovisionamiento),
                                                        GrupoSoporteRemedyId = tech.product.GrupoTicketRemedyId,
                                                        GrupoSoporteRemedy = (string.IsNullOrEmpty(tech.product.GrupoTicketRemedyNombre) ? string.Empty : tech.product.GrupoTicketRemedyNombre),
                                                        LineamientoBaseSeguridadId = tech.technology.RevisionSeguridadId,
                                                        LineamientoBaseSeguridad = (string.IsNullOrEmpty(tech.technology.RevisionSeguridadDescripcion) ? string.Empty : tech.technology.RevisionSeguridadDescripcion),
                                                    }).FirstOrDefault();

                return result;
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: TecnologiaDTO GetAllTechonologyXId(int id)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message, ex);
                throw new CVTException(CVTExceptionIds.ErrorTecnologiaDTO
                    , "Error en el metodo: TecnologiaDTO GetAllTechonologyXId(int id)"
                    , new object[] { null });
            }
        }

        public override IEnumerable<TecnologiaG> GetAllTechnologyDesactivated(List<int> domIds, List<int> subdomIds, string casoUso, string filtro, List<int> estadoIds, string famId, int fecId, string aplica, string codigo, string dueno,
            string equipo, List<int> tipoTecIds, List<int> estObsIds, string sortName, string sortOrder, int? flagActivo)
        {
            var technologyIds = new List<int>();
            var idsEquivalences = context.TecnologiaEquivalencia.Where(o => o.FlagActivo && o.Nombre.ToUpper().Contains(filtro.ToUpper())).Select(h => h.TecnologiaId).ToList();

            var description_tech = context.TecnologiaDetalleMaestra.Where(x => x.KeyCampo == "MDN" && x.Activo).ToList();

            if (!string.IsNullOrEmpty(equipo))
                technologyIds = context.EquipoTecnologia.Join(context.Equipo, e => e.EquipoId, p => p.EquipoId, (e, p) => new { technology_team = e, team = p })
                   .Where(k => k.technology_team.FlagActivo && k.team.FlagActivo && (k.team.Nombre.ToUpper().Contains(equipo.ToUpper()))).Select(y => y.technology_team.TecnologiaId).ToList();

            var dataTecnologiaGs = (from u in context.Tecnologia
                                    join t in context.Tipo on u.TipoTecnologia equals t.TipoId into lj1
                                    from t in lj1.DefaultIfEmpty()
                                    join f in context.Familia on u.FamiliaId equals f.FamiliaId into lj2
                                    from f in lj2.DefaultIfEmpty()
                                    join s in context.Subdominio on u.SubdominioId equals s.SubdominioId
                                    join d in context.Dominio on s.DominioId equals d.DominioId
                                    where (u.Activo == false) &&
                                    (u.Nombre.ToUpper().Contains(filtro.ToUpper())
                                    || u.Descripcion.ToUpper().Contains(filtro.ToUpper())
                                    || string.IsNullOrEmpty(filtro)
                                    || u.ClaveTecnologia.ToUpper().Contains(filtro.ToUpper())
                                    || idsEquivalences.Contains(u.TecnologiaId))
                                    && (domIds.Count == 0 || domIds.Contains(s.DominioId))
                                    && (subdomIds.Count == 0 || subdomIds.Contains(u.SubdominioId))
                                    && (estadoIds.Count == 0 || estadoIds.Contains(u.EstadoTecnologia))
                                    && (estObsIds.Count == 0 || estObsIds.Contains(u.EstadoId.HasValue ? u.EstadoId.Value : 0))
                                    && (tipoTecIds.Count == 0 || tipoTecIds.Contains(u.TipoTecnologia.HasValue ? u.TipoTecnologia.Value : 0))
                                    && (string.IsNullOrEmpty(famId) || f == null || f.Nombre.ToUpper().Equals(famId.ToUpper()))
                                    && (fecId == -1 || u.FechaFinSoporte.HasValue == (fecId == 1))
                                    && (u.Aplica.ToUpper().Contains(aplica.ToUpper()) || string.IsNullOrEmpty(aplica))
                                    && (u.CodigoTecnologiaAsignado.ToUpper().Contains(codigo.ToUpper()) || string.IsNullOrEmpty(codigo))
                                    && (u.DuenoId.ToUpper().Contains(dueno.ToUpper()) || string.IsNullOrEmpty(dueno))
                                    && (string.IsNullOrEmpty(equipo) || technologyIds.Contains(u.TecnologiaId))
                                    orderby u.FechaModificacion
                                    select new TecnologiaG()
                                    {
                                        Id = u.TecnologiaId,
                                        TipoTecnologiaId = u.TipoTecnologia.HasValue ? u.TipoTecnologia.Value : 4,
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
                                        FechaAprobacion = u.FechaAprobacion,
                                        UsuarioAprobacion = u.UsuarioAprobacion,
                                        ClaveTecnologia = u.ClaveTecnologia,
                                        EstadoId = u.EstadoId,
                                        FechaFinSoporte = u.FechaFinSoporte,
                                        FechaAcordada = u.FechaAcordada,
                                        FechaExtendida = u.FechaExtendida,
                                        FechaCalculoTec = u.FechaCalculoTec,
                                        FlagSiteEstandar = u.FlagSiteEstandar,
                                        FlagFechaFinSoporte = u.FlagFechaFinSoporte,
                                        FlagTieneEquivalencias = (context.TecnologiaEquivalencia.Where(e => e.TecnologiaId == u.TecnologiaId && e.FlagActivo).Select(h => true).FirstOrDefault() == true),
                                        MotivoDesactiva = (context.SolicitudTecnologia.Join(context.TecnologiaDetalleMaestra, s => s.MotivoDesactivaId, t => t.TecnologiaDetalleMaestraId, (s, t) => new { solicitud_tencologia = s, maestro = t }).Where(y => y.solicitud_tencologia.TecnologiaId == u.TecnologiaId).OrderByDescending(hj => hj.solicitud_tencologia.SolicitudTecnologiaId).Select(q => new { Descripcion = q.maestro.Descripcion + (string.IsNullOrEmpty(q.solicitud_tencologia.MotivoDesactiva) ? string.Empty : " - " + q.solicitud_tencologia.MotivoDesactiva) }).FirstOrDefault().Descripcion),
                                        FechaDesactiva = u.FechaModificacion,
                                        UsuarioDesactiva = u.UsuarioModificacion
                                    }).OrderByDescending(i => i.FechaModificacion)
                                    .OrderBy(j => sortName + " " + sortOrder);
            return dataTecnologiaGs;
        }

        public override List<MasterDetail> GetMasterDetail(string code)
        {
            var listMasterDetail = context.TecnologiaDetalleMaestra.Where(m => m.KeyCampo == code && m.Activo)
                .Select(x => new MasterDetail
                {
                    Id = x.TecnologiaDetalleMaestraId,
                    Descripcion = x.Descripcion.ToString()
                }).ToList();

            return listMasterDetail;
        }

        public override MasterDetail GetValidListData(string key, string value)
        {
            var response = new MasterDetail();

            switch (key)
            {
                case "domain":
                    #region Dominio
                    if (!string.IsNullOrEmpty(value) || value != "-1" || value != "null")
                    {
                        if (value != "null")
                        {
                            var dominioid = int.Parse(value);
                            var domain = context.Dominio.Where(x => x.Activo && x.DominioId == dominioid)
                                .Select(u => new MasterDetail
                                {
                                    Id = (string.IsNullOrEmpty(u.DominioId.ToString()) ? -1 : u.DominioId),
                                    Descripcion = u.Descripcion,
                                    Active = true
                                }).FirstOrDefault();

                            if (ReferenceEquals(null, domain))
                            {
                                response.Id = 0;
                                response.Descripcion = string.Empty;
                                response.Active = false;
                            }
                            else
                            {
                                response.Id = domain.Id;
                                response.Descripcion = domain.Descripcion;
                                response.Active = domain.Active;
                            }
                        }
                        else
                        {
                            response.Id = 0;
                            response.Descripcion = string.Empty;
                            response.Active = false;
                        }
                    }
                    else
                    {
                        response.Id = 0;
                        response.Descripcion = string.Empty;
                        response.Active = false;
                    }
                    break;
                #endregion
                case "subdomain":
                    #region SubDominio
                    if (!string.IsNullOrEmpty(value) || value != "-1" || value != "null")
                    {
                        if (value != "null")
                        {
                            var subdomainId = int.Parse(value);
                            var subdomain = context.Subdominio.Where(x => x.SubdominioId == subdomainId && x.Activo)
                                .Select(u => new MasterDetail
                                {
                                    Id = (string.IsNullOrEmpty(u.SubdominioId.ToString()) ? -1 : u.SubdominioId),
                                    Descripcion = u.Descripcion,
                                    TipoId = u.DominioId.ToString(),
                                    Active = true
                                }).FirstOrDefault();

                            if (ReferenceEquals(null, subdomain))
                            {
                                response.Id = 0;
                                response.Descripcion = string.Empty;
                                response.Active = false;
                            }
                            else
                            {
                                response.Id = subdomain.Id;
                                response.Descripcion = subdomain.Descripcion;
                                response.Active = subdomain.Active;
                            }
                        }
                        else
                        {
                            response.Id = 0;
                            response.Descripcion = string.Empty;
                            response.Active = false;
                        }
                    }
                    else
                    {
                        response.Id = 0;
                        response.Descripcion = string.Empty;
                        response.Active = false;
                    }
                    break;
                #endregion
                case "productType":
                    #region ProductType 
                    if (!string.IsNullOrEmpty(value) || value != "-1" || value != "null")
                    {
                        if (value != "null")
                        {
                            var idtype = 0;
                            var subdomain = new MasterDetail();
                            if (Regex.IsMatch(value, @"^[0-9]+$"))
                            {
                                idtype = int.Parse(value);
                                subdomain = context.Tipo.Where(x => x.Activo && x.TipoId == idtype)
                                    .Select(u => new MasterDetail
                                    {
                                        Id = (string.IsNullOrEmpty(u.TipoId.ToString()) ? -1 : u.TipoId),
                                        Descripcion = u.Descripcion,
                                        Active = true
                                    }).FirstOrDefault();
                            }
                            else
                            {
                                subdomain = context.Tipo.Where(x => x.Activo && x.Descripcion == value.ToString())
                                    .Select(u => new MasterDetail
                                    {
                                        Id = (string.IsNullOrEmpty(u.TipoId.ToString()) ? -1 : u.TipoId),
                                        Descripcion = u.Descripcion,
                                        Active = true
                                    }).FirstOrDefault();
                            }

                            if (ReferenceEquals(null, subdomain))
                            {
                                response.Id = 0;
                                response.Descripcion = string.Empty;
                                response.Active = false;
                            }
                            else
                            {
                                response.Id = subdomain.Id;
                                response.Descripcion = subdomain.Descripcion;
                                response.Active = subdomain.Active;
                            }
                        }
                        else
                        {
                            response.Id = 0;
                            response.Descripcion = string.Empty;
                            response.Active = false;
                        }
                    }
                    else
                    {
                        response.Id = 0;
                        response.Descripcion = string.Empty;
                        response.Active = false;
                    }
                    break;
                #endregion
                case "licensingScheme":
                    #region LicensingScheme 
                    if (!string.IsNullOrEmpty(value) || value != "-1")
                    {
                        if (value != "null")
                        {
                            var idLicensingScheme = 0;
                            var licensingScheme = new MasterDetail();
                            if (Regex.IsMatch(value, @"^[0-9]+$"))
                            {
                                idLicensingScheme = int.Parse(value);
                                licensingScheme = Utilitarios.EnumToList<EEsquemaLicenciamientoSuscripcion>()
                                    .Where(o => (int)o == idLicensingScheme)
                                    .Select(x => new MasterDetail
                                    {
                                        Id = (int)x,
                                        Descripcion = Utilitarios.GetEnumDescription3(x) //CORRECCION ENUM
                                    })
                                    .FirstOrDefault();
                            }
                            else
                            {
                                licensingScheme = Utilitarios.EnumToList<EEsquemaLicenciamientoSuscripcion>()
                                  .Where(o => Utilitarios.GetEnumDescription3(o) == value.ToString()) //CORRECCION ENUM
                                  .Select(x => new MasterDetail
                                  {
                                      Id = (int)x,
                                      Descripcion = Utilitarios.GetEnumDescription3(x) //CORRECCION ENUM
                                  })
                                  .FirstOrDefault();
                            }

                            if (ReferenceEquals(null, licensingScheme))
                            {
                                response.Id = 0;
                                response.Descripcion = string.Empty;
                                response.Active = false;
                            }
                            else
                            {
                                response.Id = licensingScheme.Id;
                                response.Descripcion = licensingScheme.Descripcion;
                                response.Active = licensingScheme.Active;
                            }
                        }
                        else
                        {
                            response.Id = 0;
                            response.Descripcion = string.Empty;
                            response.Active = false;
                        }
                    }
                    else
                    {
                        response.Id = 0;
                        response.Descripcion = string.Empty;
                        response.Active = false;
                    }
                    break;
                #endregion
                case "techonologytype":
                    #region Techonologytype 
                    if (!string.IsNullOrEmpty(value) || value != "-1")
                    {
                        if (value != "null")
                        {
                            var idtechonologytype = 0;
                            var techonologytype = new MasterDetail();
                            if (Regex.IsMatch(value, @"^[0-9]+$"))
                            {
                                idtechonologytype = int.Parse(value);
                                techonologytype = context.Tipo.Where(x => x.Activo && x.TipoId == idtechonologytype)
                                    .Select(u => new MasterDetail
                                    {
                                        Id = (string.IsNullOrEmpty(u.TipoId.ToString()) ? -1 : u.TipoId),
                                        Descripcion = u.Descripcion,
                                        Active = true
                                    }).FirstOrDefault();
                            }
                            else
                            {

                                techonologytype = context.Tipo.Where(x => x.Activo && x.Descripcion == value.ToString())
                                    .Select(u => new MasterDetail
                                    {
                                        Id = (string.IsNullOrEmpty(u.TipoId.ToString()) ? -1 : u.TipoId),
                                        Descripcion = u.Descripcion,
                                        Active = true
                                    }).FirstOrDefault();
                            }

                            if (ReferenceEquals(null, techonologytype))
                            {
                                response.Id = 0;
                                response.Descripcion = string.Empty;
                                response.Active = false;
                            }
                            else
                            {
                                response.Id = techonologytype.Id;
                                response.Descripcion = techonologytype.Descripcion;
                                response.Active = techonologytype.Active;
                            }
                        }
                        else
                        {
                            response.Id = 0;
                            response.Descripcion = string.Empty;
                            response.Active = false;
                        }
                    }
                    else
                    {
                        response.Id = 0;
                        response.Descripcion = string.Empty;
                        response.Active = false;
                    }
                    break;
                #endregion
                case "automationImplemented":
                    #region AutomationImplemented 
                    if (!string.IsNullOrEmpty(value) || value != "-1")
                    {
                        if (value != "null")
                        {
                            var idautomationImplemented = 0;
                            var automationImplemented = new MasterDetail();
                            if (Regex.IsMatch(value, @"^[0-9]+$"))
                            {
                                idautomationImplemented = int.Parse(value);
                                automationImplemented = Utilitarios.EnumToList<EAutomatizacionImplementada>()
                                        .Where(o => (int)o == idautomationImplemented)
                                        .Select(x => new MasterDetail
                                        {
                                            Id = (int)x,
                                            Descripcion = Utilitarios.GetEnumDescription3(x) //CORRECCION ENUM
                                        })
                                        .FirstOrDefault();
                            }
                            else
                            {
                                automationImplemented = Utilitarios.EnumToList<EAutomatizacionImplementada>()
                                        .Where(o => Utilitarios.GetEnumDescription3(o) == value.ToString()) //CORRECCION ENUM
                                        .Select(x => new MasterDetail
                                        {
                                            Id = (int)x,
                                            Descripcion = Utilitarios.GetEnumDescription3(x) //CORRECCION ENUM
                                        })
                                        .FirstOrDefault();
                            }

                            if (ReferenceEquals(null, automationImplemented))
                            {
                                response.Id = 0;
                                response.Descripcion = string.Empty;
                                response.Active = false;
                            }
                            else
                            {
                                response.Id = automationImplemented.Id;
                                response.Descripcion = automationImplemented.Descripcion;
                                response.Active = automationImplemented.Active;
                            }
                        }
                        else
                        {
                            response.Id = 0;
                            response.Descripcion = string.Empty;
                            response.Active = false;
                        }
                    }
                    else
                    {
                        response.Id = 0;
                        response.Descripcion = string.Empty;
                        response.Active = false;
                    }
                    break;
                #endregion
                case "motiveSustenance":
                    #region MotiveSustenance 
                    if (!string.IsNullOrEmpty(value) || value != "-1")
                    {
                        if (value != "null")
                        {
                            var motiveSustenance = new MasterDetail();
                            if (!Regex.IsMatch(value, @"^[0-9]+$"))
                            {
                                motiveSustenance = context.TecnologiaDetalleMaestra.Where(x => x.Activo && x.Descripcion == value.ToString() && x.KeyCampo == "MFI")
                                .Select(u => new MasterDetail
                                {
                                    Id = (string.IsNullOrEmpty(u.TecnologiaDetalleMaestraId.ToString()) ? -1 : u.TecnologiaDetalleMaestraId),
                                    Descripcion = u.Descripcion,
                                    Active = true
                                }).FirstOrDefault();
                            }

                            if (ReferenceEquals(null, motiveSustenance))
                            {
                                response.Id = 0;
                                response.Descripcion = string.Empty;
                                response.Active = false;
                            }
                            else
                            {
                                response.Id = motiveSustenance.Id;
                                response.Descripcion = motiveSustenance.Descripcion;
                                response.Active = motiveSustenance.Active;
                            }
                        }
                        else
                        {
                            response.Id = 0;
                            response.Descripcion = string.Empty;
                            response.Active = false;
                        }
                    }
                    else
                    {
                        response.Id = 0;
                        response.Descripcion = string.Empty;
                        response.Active = false;
                    }
                    break;
                #endregion
                case "fuente":
                    #region Fuente 
                    if (!string.IsNullOrEmpty(value) || value != "-1")
                    {
                        if (value != "null")
                        {
                            var idfuente = 0;
                            var fuente = new MasterDetail();

                            if (Regex.IsMatch(value, @"^[0-9]+$"))
                            {
                                idfuente = int.Parse(value);
                                fuente = Utilitarios.EnumToList<Fuente>()
                                  .Where(o => (int)o == idfuente)
                                  .Select(x => new MasterDetail
                                  {
                                      Id = (int)x,
                                      Descripcion = Utilitarios.GetEnumDescription3(x) //CORRECCION ENUM
                                  })
                                  .FirstOrDefault();
                            }

                            if (ReferenceEquals(null, fuente))
                            {
                                response.Id = 0;
                                response.Descripcion = string.Empty;
                                response.Active = false;
                            }
                            else
                            {
                                response.Id = fuente.Id;
                                response.Descripcion = fuente.Descripcion;
                                response.Active = fuente.Active;
                            }
                        }
                        else
                        {
                            response.Id = 0;
                            response.Descripcion = string.Empty;
                            response.Active = false;
                        }
                    }
                    else
                    {
                        response.Id = 0;
                        response.Descripcion = string.Empty;
                        response.Active = false;
                    }
                    break;
                #endregion
                case "calculationdate":
                    #region Calculationdate 
                    if (!string.IsNullOrEmpty(value) || value != "-1")
                    {
                        //FechaCalculoTecnologia
                        if (value != "null")
                        {
                            var idcalculofecha = 0;
                            var fechacalculoObj = new MasterDetail();
                            if (Regex.IsMatch(value, @"^[0-9]+$"))
                            {
                                idcalculofecha = int.Parse(value);
                                fechacalculoObj = Utilitarios.EnumToList<FechaCalculoTecnologia>()
                                  .Where(o => (int)o == idcalculofecha)
                                  .Select(x => new MasterDetail
                                  {
                                      Id = (int)x,
                                      Descripcion = Utilitarios.GetEnumDescription3(x) //CORRECCION ENUM
                                  })
                                  .FirstOrDefault();
                            }
                            else
                            {
                                fechacalculoObj = Utilitarios.EnumToList<FechaCalculoTecnologia>()
                                  .Where(o => Utilitarios.GetEnumDescription3(o) == value.ToString()) //CORRECCION ENUM
                                  .Select(x => new MasterDetail
                                  {
                                      Id = (int)x,
                                      Descripcion = Utilitarios.GetEnumDescription3(x) //CORRECCION ENUM
                                  })
                                  .FirstOrDefault();
                            }

                            if (ReferenceEquals(null, fechacalculoObj))
                            {
                                response.Id = 0;
                                response.Descripcion = string.Empty;
                                response.Active = false;
                            }
                            else
                            {
                                response.Id = fechacalculoObj.Id;
                                response.Descripcion = fechacalculoObj.Descripcion;
                                response.Active = fechacalculoObj.Active;
                            }
                        }
                        else
                        {
                            response.Id = 0;
                            response.Descripcion = string.Empty;
                            response.Active = false;
                        }
                    }
                    else
                    {
                        response.Id = 0;
                        response.Descripcion = string.Empty;
                        response.Active = false;
                    }
                    break;
                #endregion
                case "platformapplies":
                    #region Platformapplies 
                    if (!string.IsNullOrEmpty(value) || value != "-1")
                    {
                        if (value != "null")
                        {
                            var oPlataformaAplica = new MasterDetail();
                            if (!Regex.IsMatch(value, @"^[0-9]+$"))
                            {
                                oPlataformaAplica = Utilitarios.EnumToList<EAplicaATecnologia>()
                                  .Where(o => Utilitarios.GetEnumDescription3(o) == value.ToString()) //CORRECCION ENUM
                                  .Select(x => new MasterDetail
                                  {
                                      Id = (int)x,
                                      Descripcion = Utilitarios.GetEnumDescription3(x) //CORRECCION ENUM
                                  })
                                  .FirstOrDefault();
                            }

                            if (ReferenceEquals(null, oPlataformaAplica))
                            {
                                response.Id = 0;
                                response.Descripcion = string.Empty;
                                response.Active = false;
                            }
                            else
                            {
                                response.Id = oPlataformaAplica.Id;
                                response.Descripcion = oPlataformaAplica.Descripcion;
                                response.Active = oPlataformaAplica.Active;
                            }
                        }
                        else
                        {
                            response.Id = 0;
                            response.Descripcion = string.Empty;
                            response.Active = false;
                        }
                    }
                    else
                    {
                        response.Id = 0;
                        response.Descripcion = string.Empty;
                        response.Active = false;
                    }
                    break;
                #endregion
                default:
                    break;
            }

            return response;
        }

        public override MasterDetail GetTechnologyDescription(string id)
        {
            var idTecnologiaDetalleMaestra = int.Parse(id);
            var query = context.TecnologiaDetalleMaestra.Where(x => x.TecnologiaDetalleMaestraId.Equals(idTecnologiaDetalleMaestra))
                .Select(u => new MasterDetail { Id = u.TecnologiaDetalleMaestraId, Descripcion = u.Descripcion.ToString(), Active = u.Activo })
                .FirstOrDefault();

            return query;
        }

        public override MasterDetail GetMasterDetailXId(string code, string keycampo)
        {
            var listMasterDetail = context.TecnologiaDetalleMaestra.Where(m => m.Descripcion == code && m.KeyCampo == keycampo && m.Activo)
                .Select(x => new MasterDetail
                {
                    Id = x.TecnologiaDetalleMaestraId,
                    Descripcion = x.Descripcion.ToString()
                }).FirstOrDefault();

            return listMasterDetail;
        }
        public override List<MasterDetail> EstadoResolucionCambioBajaOwner()
        {
            var listMasterDetail = new List<MasterDetail>();
            try
            {
                var cadenaConexion = Constantes.CadenaConexion;
                using (SqlConnection cnx = new SqlConnection(cadenaConexion))
                {
                    cnx.Open();
                    using (var comando = new SqlCommand("[cvt].[USP_ListarEstadoResolucion_BajaCambioOwnerTecnologia]", cnx))
                    {
                        comando.CommandTimeout = 0;
                        comando.CommandType = System.Data.CommandType.StoredProcedure;
                        var reader = comando.ExecuteReader(CommandBehavior.CloseConnection);

                        while (reader.Read())
                        {
                            var objeto = new MasterDetail()
                            {
                                Id = reader.IsDBNull(reader.GetOrdinal("EstadoResolucion_BajaCambioOwnerTecnologiaId")) ? 0 : reader.GetInt32(reader.GetOrdinal("EstadoResolucion_BajaCambioOwnerTecnologiaId")),
                                Descripcion = reader.IsDBNull(reader.GetOrdinal("Nombre")) ? string.Empty : reader.GetString(reader.GetOrdinal("Nombre")),
                            };
                            listMasterDetail.Add(objeto);
                        }
                        reader.Close();
                    }

                    return listMasterDetail;
                }
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorEquipoDTO
                    , "Error en el metodo: EstadoResolucionCambioBajaOwner()"
                    , new object[] { null });
            }
        }
    }
}
