using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls.WebParts;

namespace BCP.CVT.DTO.Storage
{
    public class BackupDto
    {        
        public string outcode { get; set; }
        public string volser { get; set; }
        public string dsname { get; set; }
        public string jobname { get; set; }
        public DateTime crtdate { get; set; }
        public DateTime lfrdate { get; set; }   
        public int mbsize { get; set; }
        public int instances { get; set; }
        public string CodigoAPT { get; set; }
        public string InterfaceApp { get; set; }
        public string Nombre { get; set; }
        public string Frecuencia { get; set; }
        public string FrecuenciaDetalle
        {
            get
            {
                switch (Frecuencia)
                {
                    case "1":
                        return "Diaria";
                    case "2":
                        return "Semanal";
                    case "3":
                        return "Quincenal";
                    case "4":
                        return "Mensual";
                    default:
                        return "Otros";
                }                
            }
        }
        public int TotalFilas { get; set; }
        public int Tamanio { get; set; }
    }

    public class OpenDto
    {
        public string zone { get; set; }
        public DateTime backupday { get; set; }
        public DateTime startdatetime { get; set; }
        public DateTime finishdatetime { get; set; }
        public string product { get; set; }
        public string backupserver { get; set; }
        public string clientname { get; set; }
        public string target { get; set; }
        public string groupname { get; set; }
        public string levelbackup { get; set; }
        public decimal datatransferredkb { get; set; }
        public decimal performancembs { get; set; }
        public string outcome { get; set; }
        public string opensoutcome { get; set; }
        public int errocount { get; set; }
        public string errorcodes { get; set; }
        public string errorcategories { get; set; }
        public string description { get; set; }
        public string serverName { get; set; }
        public int TotalFilas { get; set; }
        public int total { get; set; }
        public int totalAppsRelacionadas { get; set; }
        public int dayBackup { get; set; }
        public int monthBackup { get; set; }
        public int yearBackup { get; set; }
        public int kpi
        {
            get
            {
                switch (outcome)
                {
                    case "Failed": return -1;
                    case "Partial": return 0;
                    case "Success": return 1;
                    default: return 0;
                }
            }
        }
    }

    public class RelacionOpenDto
    {
        public string CodigoAPT { get; set; }
        public string Nombre { get; set; }
        public string EstadoAplicacion { get; set; }
        public string Equipo { get; set; }
        public string Ambiente { get; set; }
        public string EstadoRelacion { get; set; }
        public int TotalFilas { get; set; }
    }

    public class ResumenOpenDto
    {
        public string serverName { get; set; }
        public int Dia1 { get; set; }
        public int Dia10 { get; set; }
        public int Dia11 { get; set; }
        public int Dia12 { get; set; }
        public int Dia13 { get; set; }
        public int Dia14 { get; set; }
        public int Dia15 { get; set; }
        public int Dia16 { get; set; }
        public int Dia17 { get; set; }
        public int Dia18 { get; set; }
        public int Dia19 { get; set; }
        public int Dia2 { get; set; }
        public int Dia20 { get; set; }
        public int Dia21 { get; set; }
        public int Dia22 { get; set; }
        public int Dia23 { get; set; }
        public int Dia24 { get; set; }
        public int Dia25 { get; set; }
        public int Dia26 { get; set; }
        public int Dia27 { get; set; }
        public int Dia28 { get; set; }
        public int Dia29 { get; set; }
        public int Dia3 { get; set; }
        public int Dia30 { get; set; }
        public int Dia31 { get; set; }
        public int Dia4 { get; set; }
        public int Dia5 { get; set; }
        public int Dia6 { get; set; }
        public int Dia7 { get; set; }
        public int Dia8 { get; set; }
        public int Dia9 { get; set; }
        public int TotalFilas { get; set; }
    }

    public class GraficoOpenDto
    {
        public string serverName { get; set; }
        public string outcome { get; set; }
        public int dayBackup { get; set; }
        public int total { get; set; }
        public decimal totalMB { get; set; }
    }

    public class StorageGrafico
    {
        public string Fecha { get; set; }
        public int Ejecuciones { get; set; }
        public decimal TotalMB { get; set; }
    }

    public class BackupPeriodoDto
    {
        public string DomainName { get; set; }
        public string NodeName { get; set; }
        public string ServerName { get; set; }
        public string ScheduleName { get; set; }
        public string StartTime { get; set; }
        public string Options { get; set; }
        public string Objects { get; set; }
        public string Period { get; set; }
        public string PerUnits { get; set; }
        public string DayOfWeek { get; set; }
        public string EnhMonth { get; set; }
        public string DayOfMonth { get; set; }
        public string WeekOfMonth { get; set; }
        public string Site { get; set; }
        public int TotalFilas { get; set; }
    }
}
