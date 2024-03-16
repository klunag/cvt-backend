using BCP.CVT.Cross;
using System;
using System.Collections.Generic;

namespace BCP.CVT.DTO
{
    public class DependenciasDTO
    {
        public Int64 RelacionId { get; set; }
        public int OrigenId { get; set; }
        public string CodigoAptOrigen { get; set; }
        public string AplicacionOrigen { get; set; }
        public string CodigoAptDestino { get; set; }
        public string AplicacionDestino { get; set; }
        public string EquipoOrigen { get; set; }
        public string EquipoDestino { get; set; }
        public string IPEquipoOrigen { get; set; }
        public string IPEquipoDestino { get; set; }
        public string Puerto { get; set; }
        public string Protocolo { get; set; }
        public string TipoConexion { get; set; }
        public string TipoRelacion { get; set; }
        public string DescripcionEtiqueta { get; set; }
        public string EtiquetaAplicacionOrigen { get; set; }
        public string EtiquetaAplicacionDestino { get; set; }
        //public string EtiquetaAplicacion
        //{
        //    get
        //    {
        //        if(OrigenId == 2)
        //        {
        //            if (TipoConexion == "Origen")
        //            {
        //                if(!String.IsNullOrEmpty(EtiquetaAplicacionDestino))
        //                    return EtiquetaAplicacionDestino;
        //                else 
        //                    return "Negocio";
        //            }
                       
        //            else if (TipoConexion == "Destino")
        //            {
        //                if (!String.IsNullOrEmpty(EtiquetaAplicacionOrigen))
        //                    return EtiquetaAplicacionOrigen;
        //                else
        //                    return "Negocio";
        //            }
        //            else
        //                return "Negocio";
        //        }
        //        else
        //        {
        //            if(!String.IsNullOrEmpty(DescripcionEtiqueta))
        //                return DescripcionEtiqueta;
        //            else
        //                return "Negocio";
        //        } 
        //    }
        //}
        public int EtiquetaId { get; set; }
        public string EtiquetaAplicacion { get; set; }
        public string ProcesoOrigen { get; set; }
        public string ProcesoDestino { get; set; }
        public string AplicacionDuenaRelacion { get; set; }
        public string AplicacionDuenaRelacionNombre { get; set; }
        public string AplicacionGeneraImpacto { get; set; }
        public string AplicacionGeneraImpactoNombre { get; set; }
        public DateTime PrimeraFechaEscaneo { get; set; }
        public DateTime UltimaFechaEscaneo { get; set; }
        public int CantidadConexiones { get; set; }
        public int CantidadEscaneo { get; set; }
        public int TotalFilas { get; set; }
    }



    public class DependenciasComponentesDTO
    {
        public string CodigoApt { get; set; }
        public string Aplicacion { get; set; }
        public string Equipo { get; set; }
        public string TipoRelacionamiento { get; set; }
        public string TipoComponente { get; set; }
        public string Tecnologia { get; set; }
        public int? EstadoId { get; set; }
        public string Estado => EstadoId.HasValue ? (EstadoId.Value == 0 ? "-" : Utilitarios.GetEnumDescription2((EEstadoRelacion)EstadoId)) : "-";
        public string Dominio { get; set; }
        public string Subdominio { get; set; }
        public string Relevancia { get; set; }
        public string TipoTecnologia { get; set; }
    }

    public class CombosDependenciasDTO
    {
        public List<ItemLista2> ListaTipoConexion { get; set; }
        public List<ItemLista> ListaTipoRelacionamiento { get; set; }
        public List<ItemLista> ListaEtiquetaAplicacion { get; set; }
    }

    public class TipoRelacionamientoDTO
    {
        public int TipoRelacionamientoId { get; set; }
        public string NombreRelacionamiento { get; set; }
    }

    public class TipoConexionDTO
    {
        public int TipoConexionId { get; set; }
        public string NombreConexion { get; set; }
    }

    public class EtiquetaAplicacionDTO
    {
        public int TipoEtiquetaId { get; set; }
        public string NombreEtiqueta { get; set; }
    }

    public class ItemLista
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
    }

    public class ItemLista2
    {
        public string Id { get; set; }
        public string Descripcion { get; set; }
    }

    #region Vista Grafica Dependencias
    public class GrafoNodeDTO
    {
        public string id { get; set; }
        public string label { get; set; }
        public string title { get; set; }
        public int level { get; set; }
        public string color { get; set; }
        public string shape => (tipoNodo == "App" ? "dot" : "image");
        public int size => (shape == "dot" ? 13 : 20);
        public string image
        {
            get
            {
                String resultado = "";

                switch (tipoNodo)
                {
                    case "Servidor":
                        resultado = "../images/server.png";
                        break;
                    case "cics":
                        resultado = "../images/cics.png";
                        break;
                    case "mainframe":
                        resultado = "../images/mainframe.png";
                        break;
                    case "Equipo":
                        resultado = "../images/server.png";
                        break;
                    case "Api":
                        resultado = "../images/api.png";
                        break;
                    case "Tec":
                        resultado = "../images/technology.png";
                        break;
                    case "Nube":
                        resultado = "../images/cloud.png";
                        break;
                    case "ServicioBroker":
                        resultado = "../images/broker.png";
                        break;
                    case "Aplicacion":
                        resultado = "../images/aplicacion.png";
                        break;
                    case "certificado-digital":
                        resultado = "../images/certificado-digital.png";
                        break;
                    case "appliance":
                        resultado = "../images/appliance.png";
                        break;
                    case "servidor-agencia":
                        resultado = "../images/servidor-agencia.png";
                        break;
                    case "pc":
                        resultado = "../images/pc.png";
                        break;
                    case "storage":
                        resultado = "../images/storage.png";
                        break;
                    case "redes-comunicaciones":
                        resultado = "../images/redes-comunicaciones.png";
                        break;
                    case "client-secret":
                        resultado = "../images/client-secret.png";
                        break;

                    case "Microsoft Azure Web Apps *":
                        resultado = "../images/App-Services.png";
                        break;
                    case "Microsoft Azure Key Vault *":
                        resultado = "../images/Key-Vaults.png";
                        break;
                    case "Microsoft Azure SQL Database *":
                        resultado = "../images/SQL-Database.png";
                        break;
                    case "Microsoft Azure Storage Account *":
                        resultado = "../images/Storage-Accounts.png";
                        break;
                    case "Microsoft Azure Kubernetes Service (AKS) *":
                        resultado = "../images/Kubernetes-Services.png";
                        break;
                    case "Microsoft Azure Active Directory *":
                        resultado = "../images/Azure-Active-Directory.png";
                        break;
                    case "Microsoft Azure Service Plan *":
                        resultado = "../images/App-Service-Plans.png";
                        break;
                    case "Microsoft Azure Monitor *":
                        resultado = "../images/Monitor.png";
                        break;
                    case "Microsoft Azure Portal Dashboard *":
                        resultado = "../images/Dashboard.png";
                        break;
                    case "Microsoft Azure Virtual Networks *":
                        resultado = "../images/Virtual-Networks.png";
                        break;
                    case "Microsoft Azure Log Analytics *":
                        resultado = "../images/Log-Analytics-Workspaces.png";
                        break;
                    case "Microsoft Azure API Management stv2":
                        resultado = "../images/API-Management-Services.png";
                        break;
                    default:
                        resultado = "../images/azure_logo.png";
                        break;
                }
                return resultado;
            }
        }
        public string tipoNodo { get; set; }
        public string group { get; set; }
    }

    public class GrafoEdgeDTO
    {
        public string from { get; set; }
        public string to { get; set; }
        public string label { get; set; }
        public string color { get; set; }
        public string arrows { get; set; } = "to";
        public string length { get; set; }
    }

    public class GrafoDTO
    {
        public List<GrafoNodeDTO> nodes { get; set; }
        public List<GrafoEdgeDTO> edges { get; set; }
        public dynamic options { get; set; }
    }

    public class DataComponente
    {
        public int ComponenteId { get; set; }
        public int TipoEquipoId { get; set; }
        public string Nombre { get; set; }
        public string CodigoAptOwner { get; set; }
        public string NombreAptOwner { get; set; }
        public string Descripcion { get; set; }
    }

    public class ApsRelacionComponente
    {
        public string CodigoApt { get; set; }
        public string NombreApt { get; set; }
        public int TipoRelacionId { get; set; }
        public string TipoRelacionDesc { get; set; }
    }

    public class AppsDependenciaImpacto
    {
        public string CodigoAptOrigen { get; set; }
        public string NombreAptOrigen { get; set; }
        public string CodigoAptDestino { get; set; }
        public string NombreAptDestino { get; set; }
        public int TipoRelacionId { get; set; }
        public string TipoRelacionDesc { get; set; }
    }

    public class RelacionAppToApp
    {
        public int RelacionId { get; set; }
        public string CodigoAptOrigen { get; set; }
        public string NombreAptOrigen { get; set; }
        public int EquipoOrigen { get; set; }
        public string NomEquipoOrigen { get; set; }
        public int TipoEquipoIdOrigen { get; set; }
        public string EqOrigen_SO { get; set; }
        public int TipoRelAppEqOrigen_Id { get; set; }
        public string TipoRelAppEqOrigen_Desc { get; set; }
        public string CodigoAptDestino { get; set; }
        public string NombreAptDestino { get; set; }
        public int EquipoDestino { get; set; }
        public string NomEquipoDestino { get; set; }
        public int TipoEquipoIdDestino { get; set; }
        public string EqDestino_SO { get; set; }
        public int TipoRelAppEqDestino_Id { get; set; }
        public string TipoRelAppEqDestino_Desc { get; set; }
        public int TipoRelId_App { get; set; }
        public string DescTipoRel_App { get; set; }
    }
    #endregion

    #region Diagrama de Infraestructura
    public class ServidoresRelacionadosDTO
    {
        public string CodAppOrigen { get; set; }
        public string NombreAppOrigen { get; set; }
        public int Relacionado { get; set; }
        public int EquipoId { get; set; }
        public string NombreEquipo { get; set; }
        public string Funcion { get; set; }
        public string SistemaOperativoEquipo { get; set; }
        public int EquipoId_Relacion { get; set; }
        public string NombreEquipoRelacionado { get; set; }
        public string SistemaOperativoEquipoRelacionado { get; set; }
        public string CodAppRelacionada { get; set; }
        public string NombreAppRelacionada { get; set; }
    }
    public class ApisRelacionadasDTO
    {
        public string CodAppOrigen { get; set; }
        public string NombreAppOrigen { get; set; }
        public int EquipoId { get; set; }
        public string NombreApi { get; set; }
        public string Owner { get; set; }
        public string NombreOwner { get; set; }
    }
    public class ServiciosNubeRelacionadosDTO
    {
        public string CodAppOrigen { get; set; }
        public string NombreAppOrigen { get; set; }
        public int EquipoId { get; set; }
        public string Nombre { get; set; }
        public string Tecnologia { get; set; }
        public string Suscripcion { get; set; }
        public string GrupoRecursos { get; set; }
    }
    public class RelacionReglasGeneralesDTO
    {
        public int RelacionReglasGeneralesId { get; set; }
        public int AplicaEn { get; set; }
        public string AplicaEnStr { get; set; }
        public string Origen { get; set; }
        public string Destino { get; set; }
    }
    public class RelacionReglasPorAppTablaDTO
    {
        public int RelacionReglasPorAppId { get; set; }
        public string CodigoApt { get; set; }
        public string Funcion { get; set; }
        public int EquipoId { get; set; }
        public string Nombre { get; set; }
        public string TecPrincipal { get; set; }
        public int EquipoId_Relacion { get; set; }
        public string Nombre_Relacion { get; set; }
        public string TecPrincipal_Relacion { get; set; }
        public bool Relacionado { get; set; }
        public bool FlagActivo { get; set; }
        public string CreadoPor { get; set; }
        public string FechaCreacionStr { get; set; }
        public string Estado { get; set; }
        public string ListaIpEquipo { get; set; }
        public string ListaIpEquipo_Relacion { get; set; }
        public int TipoEquipoId { get; set; }
        public int TipoEquipoId_Relacion { get; set; }
        public string ClaveTecnologia_Componente { get; set; }
        public string ClaveTecnologia_ConectaCon { get; set; }
        public string NombreAplicacion_Componente { get; set; }
        public string NombreAplicacion_ConectaCon { get; set; }
    }
    public class RelacionReglasPorAppDiagramaDTO
    {
        public string CodigoApt { get; set; }
        public string Funcion { get; set; }
        public int EquipoId { get; set; }
        public string Nombre { get; set; }
        public string TecPrincipal { get; set; }
        public int EquipoId_Relacion { get; set; }
        public string Nombre_Relacion { get; set; }
        public string TecPrincipal_Relacion { get; set; }
        public bool Relacionado { get; set; }
        public string ListaIpEquipo { get; set; }
        public string ListaIpEquipo_Relacion { get; set; }
        public int TipoEquipoId { get; set; }
        public int TipoEquipoId_Relacion { get; set; }
        public bool FlagIncluir_Componente { get; set; }
        public bool FlagIncluir_ConectaCon { get; set; }
        public string ClaveTecnologia_Componente { get; set; }
        public string ClaveTecnologia_ConectaCon { get; set; }
        public string NombreAplicacion_Componente { get; set; }
        public string NombreAplicacion_ConectaCon { get; set; }
    }
    #endregion
}
