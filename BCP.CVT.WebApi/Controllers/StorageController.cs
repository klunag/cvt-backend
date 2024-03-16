using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.Services.Exportar;
using BCP.CVT.Services.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using BCP.CVT.WebApi.Auth;
using BCP.CVT.DTO.Storage;

namespace BCP.CVT.WebApi.Controllers
{
   [Authorize]
    [RoutePrefix("api/Storage")]
    public class StorageController : BaseController
    {
        [Route("Backup/Mainframe")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostGetBackupMainframe(PaginacionStorage pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<StorageDAO>.Provider.GetBackupMainframe(pag, out totalRows);

            if (registros == null)
                return NotFound();

            var reader = new BootstrapTable<BackupDto>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("Backup/Mainframe/Detalle")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostGetBackupMainframeDetalle(PaginacionStorage pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<StorageDAO>.Provider.GetBackupMainframeDetalle(pag, out totalRows);

            if (registros == null)
                return NotFound();

            var reader = new BootstrapTable<BackupDto>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("Backup/Mainframe/Exportar")]
        [HttpGet]
        public IHttpActionResult ExportarBackupMainframe(string jobname, string app, string interfaceapp)
        {
            string nomArchivo = "";
            var data = new ExportarData().ExportarBackupMainframe(jobname, app, interfaceapp);
            nomArchivo = string.Format("BackupMainframe_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("Backup/Open")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostGetBackupOpen(PaginacionStorage pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<StorageDAO>.Provider.GetBackupOpen(pag, out totalRows);

            if (registros == null)
                return NotFound();

            var reader = new BootstrapTable<OpenDto>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("Backup/Open/Detalle")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostGetBackupOpenDetalle(PaginacionStorage pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<StorageDAO>.Provider.GetBackupOpenDetalle(pag, out totalRows);

            if (registros == null)
                return NotFound();

            var reader = new BootstrapTable<OpenDto>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("Backup/Open/Periodo")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostGetBackupOpenPeriodo(PaginacionStorage pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<StorageDAO>.Provider.GetBackupPeriodo(pag, out totalRows);

            if (registros == null)
                return NotFound();

            var reader = new BootstrapTable<BackupPeriodoDto>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("Backup/Open/Resumen")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostGetBackupOpenResumen(PaginacionStorage pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<StorageDAO>.Provider.GetBackupOpenResumen(pag, out totalRows);

            if (registros == null)
                return NotFound();

            var reader = new BootstrapTable<ResumenOpenDto>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("Backup/Open/Aplicaciones")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostGetBackupOpenAplicaciones(PaginacionStorage pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<StorageDAO>.Provider.GetBackupOpenAplicaciones(pag, out totalRows);

            if (registros == null)
                return NotFound();

            var reader = new BootstrapTable<RelacionOpenDto>()
            {
                Total = totalRows,
                Rows = registros
            };

            return Ok(reader);
        }

        [Route("Backup/Open/Exportar")]
        [HttpGet]
        public IHttpActionResult ExportarBackupMainframe(string aplicacion, string server, string backupserver, string target, string levelbackup, string outcome, string fecha)
        {
            string nomArchivo = "";
            var data = new ExportarData().ExportarBackupOpen(aplicacion, server, backupserver, target, levelbackup, outcome, fecha);
            nomArchivo = string.Format("BackupOpen_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("Backup/Open/ExportarDetalle")]
        [HttpGet]
        public IHttpActionResult ExportarBackupOpenDetalle(string server, int monthBackup, int yearBackup)
        {
            string nomArchivo = "";
            var data = new ExportarData().ExportarBackupOpenDetalle(server, monthBackup, yearBackup);
            nomArchivo = string.Format("BackupOpenDetalle_{0}.xlsx", DateTime.Now.ToString("yyyyMMdd_HHmmss"));

            return Ok(new { excel = data, name = nomArchivo });
        }

        [Route("Backup/Open/Grafico")]
        [HttpPost]
        [Authorize]
        public IHttpActionResult PostGetBackupOpenGrafico(PaginacionStorage pag)
        {
            var totalRows = 0;
            var registros = ServiceManager<StorageDAO>.Provider.GetBackupOpenResumenDetalle(pag, out totalRows);            

            var totalDias = System.DateTime.DaysInMonth(pag.yearBackup, pag.monthBackup);
            var lista = new List<StorageGrafico>();
            for(int i = 1; i <= totalDias; i++)
            {
                var item = registros.Where(x => x.dayBackup == i).ToList();
                if (item!=null)
                {
                    foreach(var index in item)
                    {
                        if (index.outcome.ToUpper().Equals("SUCCESS") || index.outcome.ToUpper().Equals("PARTIAL"))
                            lista.Add(new StorageGrafico()
                            {
                                Ejecuciones = index.total,
                                Fecha = new DateTime(pag.yearBackup, pag.monthBackup, i).ToString("yyyy-MM-dd")
                            });
                        else
                            lista.Add(new StorageGrafico()
                            {
                                Ejecuciones = index.total * -1,
                                Fecha = new DateTime(pag.yearBackup, pag.monthBackup, i).ToString("yyyy-MM-dd")
                            });
                    }
                    
                }
                else
                {
                    lista.Add(new StorageGrafico()
                    {
                        Ejecuciones = 0,
                        Fecha = new DateTime(pag.yearBackup, pag.monthBackup, i).ToString("yyyy-MM-dd")
                    });
                }
            }

            var registros2 = ServiceManager<StorageDAO>.Provider.GetBackupOpenResumenDetalleTrasnsferencia(pag, out totalRows);            

            totalDias = System.DateTime.DaysInMonth(pag.yearBackup, pag.monthBackup);
            var lista2 = new List<StorageGrafico>();
            for (int i = 1; i <= totalDias; i++)
            {
                var item = registros2.FirstOrDefault(x => x.dayBackup == i);
                if (item != null)
                {
                    lista2.Add(new StorageGrafico()
                    {
                        TotalMB = item.totalMB,
                        Fecha = new DateTime(pag.yearBackup, pag.monthBackup, i).ToString("yyyy-MM-dd")
                    });
                }
                else
                {
                    lista2.Add(new StorageGrafico()
                    {
                        TotalMB = 0,
                        Fecha = new DateTime(pag.yearBackup, pag.monthBackup, i).ToString("yyyy-MM-dd")
                    });
                }
            }

            var reader = new 
            {                
                Ejecuciones = lista,
                Transferencia = lista2
            };            

            return Ok(reader);
        }        
    }
}