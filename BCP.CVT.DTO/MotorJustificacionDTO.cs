﻿using System;
using BCP.CVT.Cross;

namespace BCP.CVT.DTO
{
    public class MotorJustificacionDTO
    {
        public int MotorJustificacionId { get; set; }
        public int ProcesoId { get; set; }
        public int PuertoId { get; set; }
        public int ProtocoloId { get; set; }
        public string CodigoApt { get; set; }
        public int EquipoId { get; set; }
        public string ProcesoOrigen { get; set; }
        public string ProcesoDestino { get; set; }
        public int TipoProceso { get; set; }
        public string IpOrigen { get; set; }
        public string CodigoAptDestino { get; set; }
        public int EquipoIdDestino { get; set; }
        public string IpDestino { get; set; }
        public int ProductoId { get; set; }
        public int TecnologiaId { get; set; }
        public int CodAccion { get; set; }
        public string Accion => Utilitarios.GetEnumDescription2((MotorRelacionamientoAccion)CodAccion);
        public bool NotificadoOwner { get; set; }
        public bool NotificadoAdminCVT { get; set; }
        public long RelacionId { get; set; }
        public long RelacionDetalleId { get; set; }
        public int CantidadConexiones { get; set; }
        public DateTime FechaEscaneo { get; set; }

        public bool FlagActivo { get; set; }
        public string UsuarioCreacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string UsuarioModificacion { get; set; }
        public DateTime FechaModificacion { get; set; }
        public int TotalFilas { get; set; }
        public string FechaCreacionStr
        {
            get
            {
                return FechaCreacion.ToString("dd/MM/yyyy hh:mm:ss tt");
            }
        }
        public string FechaModificacionStr
        {
            get
            {
                return FechaModificacion.ToString("dd/MM/yyyy hh:mm:ss tt");
            }
        }
        public string FechaEscaneoStr
        {
            get
            {
                return FechaEscaneo.ToString("dd/MM/yyyy hh:mm:ss tt");
            }
        }
        public string Proceso { get; set; }
        public string Puerto { get; set; }
        public string Protocolo { get; set; }
        public string Producto { get; set; }
        public string Equipo { get; set; }
        public string EquipoDestino { get; set; }
        public string Tecnologia { get; set; }
        public int Tipo { get; set; }
        public string TipoStr => Utilitarios.GetEnumDescription2((MotorTipoRelacionamiento)Tipo);
        public string TipoProcesoStr => Utilitarios.GetEnumDescription2((MotorReglaProcesoTipo)TipoProceso);
    }

}
