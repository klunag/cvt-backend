using BCP.CVT.Cross;
using FileHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO.Custom
{
    public class QualyConsolidadoDto
    {
        public string Leyenda { get; set; }
        public string Severidad { get; set; }
        public string TipoVulnerabilidad { get; set; }
        public bool FlagEquipoAsignado { get; set; }
        public bool FlagProductoTecnologiaAsignado { get; set; }
        public string EquipoAsignadoStr { get; set; }
        public string ProductoTecnologiaAsignadoStr { get; set; }
        public int Cantidad { get; set; }
        public bool TieneEquipo { get; set; }
        public string TieneEquipoStr { get { return TieneEquipo ? "Sí" : "No"; } }
        public bool TieneProducto { get; set; }
        public string TieneProductoStr { get { return TieneProducto ? "Sí" : "No"; } }
        public bool TieneTecnologia { get; set; }
        public string TieneTecnologiaStr { get { return TieneTecnologia ? "Sí" : "No"; } }
        public int Fila { get; set; }
        public int QID { get; set; }
        public string DNS { get; set; }
        public string IP { get; set; }
        public string NetBIOS { get; set; }
        public string SO { get; set; }
        public string Titulo { get; set; }
        public string Categoria { get; set; }
        public string Diagnostico { get; set; }
        public string Solucion { get; set; }
        public string EquipoStr { get; set; }
        public string ProductoStr { get; set; }
        public string DominioStr { get; set; }
        public string SubDominioStr { get; set; }
        public string TribuCoeStr { get; set; }
        public string SquadStr { get; set; }
        public string TecnologiaStr { get; set; }
        public string TipoTecnologiaStr { get; set; }
        public int EstadoIdTecnologia { get; set; }
        public string EstadoTecnologiaStr
        {
            get
            {
                return EstadoIdTecnologia == 0 ? null : Utilitarios.GetEnumDescription2((ETecnologiaEstado)EstadoIdTecnologia);
            }
        }

        public string TrackingMethod { get; set; }
        public string IPStatus { get; set; }
        public string Title { get; set; }
        public string VulnStatus { get; set; }
        public string Type { get; set; }
        public string Severity { get; set; }
        public string Port { get; set; }
        public string Protocol { get; set; }
        public string FQDN { get; set; }
        public string SSL { get; set; }
        public string FirstDetected { get; set; }
        public string LastDetected { get; set; }
        public string TimesDetected { get; set; }
        public string DateLastFixed { get; set; }
        public string FirstReopened { get; set; }
        public string LastReopened { get; set; }
        public string TimesReopened { get; set; }
        public string CVEId { get; set; }
        public string VendorReference { get; set; }
        public string BugtraqId { get; set; }
        public string CVSS { get; set; }
        public string CVSSBase { get; set; }
        public string CVSSTemporal { get; set; }
        public string CVSSEnvironment { get; set; }
        public string CVSS3 { get; set; }
        public string CVSS3Base { get; set; }
        public string CVSS3Temporal { get; set; }
        public string Threat { get; set; }
        public string Impact { get; set; }
        public string Solution { get; set; }
        public string Exploitability { get; set; }
        public string AssociatedMalware { get; set; }
        public string Results { get; set; }
        public string PCIVuln { get; set; }
        public string TicketState { get; set; }
        public string Instance { get; set; }
        public string Category { get; set; }
        public string AssociatedTags { get; set; }
    }

    [DelimitedRecord(";")]
    public class QualyConsolidadoCsvDto
    {

        [FieldCaption("Dominio")]
        [FieldQuoted()]
        public string DominioStr { get; set; }
        [FieldCaption("SubDominio")]
        [FieldQuoted()]
        public string SubDominioStr { get; set; }
        [FieldCaption("Producto")]
        [FieldQuoted()]
        public string ProductoStr { get; set; }
        [FieldCaption("Tecnología")]
        [FieldQuoted()]
        public string TecnologiaStr { get; set; }
        [FieldCaption("Owner")]
        [FieldQuoted()]
        public string TribuCoeStr { get; set; }
        [FieldCaption("Squad")]
        [FieldQuoted()]
        public string SquadStr { get; set; }
        [FieldCaption("Equipo")]
        [FieldQuoted()]
        public string EquipoStr { get; set; }
        [FieldCaption("EQUIPOS QUE ESTAN EN CVT")]
        [FieldQuoted()]
        public string TieneEquipoStr { get; set; }
        [FieldCaption("TECNOLOGÍAS QUE ESTAN EN CVT")]
        [FieldQuoted()]
        public string TieneTecnologiaStr { get; set; }
        [FieldCaption("PRODUCTOS QUE ESTAN EN CVT")]
        [FieldQuoted()]
        public string TieneProductoStr { get; set; }
        [FieldCaption("QID")]
        [FieldQuoted()]
        public int QID { get; set; }
        [FieldCaption("IP")]
        [FieldQuoted()]
        public string IP { get; set; }
        [FieldCaption("DNS")]
        [FieldQuoted()]
        public string DNS { get; set; }
        [FieldCaption("NetBIOS")]
        [FieldQuoted()]
        public string NetBIOS { get; set; }
        [FieldCaption("TRACKING METHOD")]
        [FieldQuoted()]
        public string TrackingMethod { get; set; }
        [FieldCaption("SO")]
        [FieldQuoted()]
        public string SO { get; set; }
        [FieldCaption("IP STATUS")]
        [FieldQuoted()]
        public string IPStatus { get; set; }
        [FieldCaption("TITLE")]
        [FieldQuoted()]
        public string Title { get; set; }
        [FieldCaption("VULN STATUS")]
        [FieldQuoted()]
        public string VulnStatus { get; set; }
        [FieldCaption("TYPE")]
        [FieldQuoted()]
        public string Type { get; set; }
        [FieldCaption("SEVERITY")]
        [FieldQuoted()]
        public string Severity { get; set; }
        [FieldCaption("PORT")]
        [FieldQuoted()]
        public string Port { get; set; }
        [FieldCaption("PROTOCOL")]
        [FieldQuoted()]
        public string Protocol { get; set; }
        [FieldCaption("FQDN")]
        [FieldQuoted()]
        public string FQDN { get; set; }
        [FieldCaption("SSL")]
        [FieldQuoted()]
        public string SSL { get; set; }
        [FieldCaption("FIRST DETECTED")]
        [FieldQuoted()]
        public string FirstDetected { get; set; }
        [FieldCaption("LAST DETECTED")]
        [FieldQuoted()]
        public string LastDetected { get; set; }
        [FieldCaption("TIMES DETECTED")]
        [FieldQuoted()]
        public string TimesDetected { get; set; }
        [FieldCaption("DATE LAST FIXED")]
        [FieldQuoted()]
        public string DateLastFixed { get; set; }
        [FieldCaption("FIRST REOPENED")]
        [FieldQuoted()]
        public string FirstReopened { get; set; }
        [FieldCaption("LAST REOPENED")]
        [FieldQuoted()]
        public string LastReopened { get; set; }
        [FieldCaption("TIMES REOPENED")]
        [FieldQuoted()]
        public string TimesReopened { get; set; }
        [FieldCaption("CVE ID")]
        [FieldQuoted()]
        public string CVEId { get; set; }
        [FieldCaption("VENDOR REFERENCE")]
        [FieldQuoted()]
        public string VendorReference { get; set; }
        [FieldCaption("BUGTRAQ ID")]
        [FieldQuoted()]
        public string BugtraqId { get; set; }
        [FieldCaption("CVSS")]
        [FieldQuoted()]
        public string CVSS { get; set; }
        [FieldCaption("CVSS BASE")]
        [FieldQuoted()]
        public string CVSSBase { get; set; }
        [FieldCaption("CVSS TEMPORAL")]
        [FieldQuoted()]
        public string CVSSTemporal { get; set; }
        [FieldCaption("CVSS ENVIROMENT")]
        [FieldQuoted()]
        public string CVSSEnvironment { get; set; }
        [FieldCaption("CVSS3")]
        [FieldQuoted()]
        public string CVSS3 { get; set; }
        [FieldCaption("CVSS3 BASE")]
        [FieldQuoted()]
        public string CVSS3Base { get; set; }
        [FieldCaption("CVSS3 TEMPORAL")]
        [FieldQuoted()]
        public string CVSS3Temporal { get; set; }
        [FieldCaption("THREAT")]
        [FieldQuoted()]
        public string Threat { get; set; }
        [FieldCaption("IMPACT")]
        [FieldQuoted()]
        public string Impact { get; set; }
        [FieldCaption("SOLUTION")]
        [FieldQuoted()]
        public string Solution { get; set; }
        [FieldCaption("EXPLOITABILITY")]
        [FieldQuoted()]
        public string Exploitability { get; set; }
        [FieldCaption("ASSOCIATED MALWARE")]
        [FieldQuoted()]
        public string AssociatedMalware { get; set; }
        [FieldCaption("RESULTS")]
        [FieldQuoted()]
        public string Results { get; set; }
        [FieldCaption("PCI VULN")]
        [FieldQuoted()]
        public string PCIVuln { get; set; }
        [FieldCaption("TICKET STATE")]
        [FieldQuoted()]
        public string TicketState { get; set; }
        [FieldCaption("INSTANCE")]
        [FieldQuoted()]
        public string Instance { get; set; }
        [FieldCaption("CATEGORY")]
        [FieldQuoted()]
        public string Category { get; set; }
        [FieldCaption("ASSOCIATED TAGS")]
        [FieldQuoted()]
        public string AssociatedTags { get; set; }
    }

    [DelimitedRecord(",")]
    public class GuardicoreCsvDto
    {

        [FieldCaption("id")]
        [FieldQuoted()]
        public string id { get; set; }

        [FieldCaption("connection_type")]
        [FieldQuoted()]
        public string connection_type { get; set; }

        [FieldCaption("action")]
        [FieldQuoted()]
        public string action { get; set; }

        [FieldCaption("policy_verdict")]
        [FieldQuoted()]
        public string policy_verdict { get; set; }

        [FieldCaption("has_mismatch_alert")]
        [FieldQuoted()]
        public string has_mismatch_alert { get; set; }

        [FieldCaption("source_ip")]
        [FieldQuoted()]
        public string source_ip { get; set; }

        [FieldCaption("source_asset_id")]
        [FieldQuoted()]
        public string source_asset_id { get; set; }

        [FieldCaption("source_asset_name")]
        [FieldQuoted()]
        public string source_asset_name { get; set; }

        [FieldCaption("source_asset_labels")]
        [FieldQuoted()]
        public string source_asset_labels { get; set; }

        [FieldCaption("source_asset_label_groups")]
        [FieldQuoted()]
        public string source_asset_label_groups { get; set; }

        [FieldCaption("source_asset_full_data")]
        [FieldQuoted()]
        public string source_asset_full_data { get; set; }

        [FieldCaption("source_username")]
        [FieldQuoted()]
        public string source_username { get; set; }

        [FieldCaption("source_process")]
        [FieldQuoted()]
        public string source_process { get; set; }

        [FieldCaption("source_windows_service")]
        [FieldQuoted()]
        public string source_windows_service { get; set; }

        [FieldCaption("source_process_full_path")]
        [FieldQuoted()]
        public string source_process_full_path { get; set; }

        [FieldCaption("destination_ip")]
        [FieldQuoted()]
        public string destination_ip { get; set; }

        [FieldCaption("destination_asset_id")]
        [FieldQuoted()]
        public string destination_asset_id { get; set; }

        [FieldCaption("destination_asset_name")]
        [FieldQuoted()]
        public string destination_asset_name { get; set; }

        [FieldCaption("destination_asset_labels")]
        [FieldQuoted()]
        public string destination_asset_labels { get; set; }

        [FieldCaption("destination_asset_label_groups")]
        [FieldQuoted()]
        public string destination_asset_label_groups { get; set; }

        [FieldCaption("destination_asset_full_data")]
        [FieldQuoted()]
        public string destination_asset_full_data { get; set; }

        [FieldCaption("destination_username")]
        [FieldQuoted()]
        public string destination_username { get; set; }

        [FieldCaption("destination_process")]
        [FieldQuoted()]
        public string destination_process { get; set; }

        [FieldCaption("destination_windows_service")]
        [FieldQuoted()]
        public string destination_windows_service { get; set; }

        [FieldCaption("destination_process_full_path")]
        [FieldQuoted()]
        public string destination_process_full_path { get; set; }

        [FieldCaption("destination_port")]
        [FieldQuoted()]
        public string destination_port { get; set; }

        [FieldCaption("ip_protocol")]
        [FieldQuoted()]
        public string ip_protocol { get; set; }

        [FieldCaption("destination_domain")]
        [FieldQuoted()]
        public string destination_domain { get; set; }

        [FieldCaption("count")]
        [FieldQuoted()]
        public string count { get; set; }

        [FieldCaption("time (utc)")]
        [FieldQuoted()]
        public string time { get; set; }

        [FieldCaption("incidents")]
        [FieldQuoted()]
        public string incidents { get; set; }

        [FieldCaption("policy_rule")]
        [FieldQuoted()]
        public string policy_rule { get; set; }

        [FieldCaption("policy_ruleset")]
        [FieldQuoted()]
        public string policy_ruleset { get; set; }

        [FieldCaption("exporting_error")]
        [FieldQuoted()]
        public string exporting_error { get; set; }
    }
}