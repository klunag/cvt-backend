using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCP.CVT.DTO;
using BCP.CVT.DTO.Custom;
using BCP.CVT.DTO.Storage;
using System.Data;

namespace BCP.CVT.Services.Interface
{
    public abstract class StorageDAO : ServiceProvider
    {
        public abstract List<StorageDto> GetStorage(PaginacionStorage filtro, out int totalRows);
        public abstract List<StorageDto> GetStorageEquipo(PaginacionStorage filtro, out int totalRows);
        public abstract List<StorageDto> GetStorageAplicacion(PaginacionStorage filtro, out int totalRows);

        public abstract List<StorageNivelDto> GetStorageResumenNivel1(out int totalRows);
        public abstract List<StorageNivelDto> GetStorageResumenNivel2(PaginacionStorage filtro, out int totalRows);
        public abstract List<StorageNivelDto> GetStorageResumenNivel3(PaginacionStorage filtro, out int totalRows);
        public abstract List<StorageAplicacionDto> GetStorageResumenNivel4(PaginacionStorage filtro, out int totalRows);

        public abstract StorageEquipoDto GetStorageEquipo(int equipoId);

        public abstract List<StorageResumenDto> GetStorageResumen2Nivel1(out int totalRows);
        public abstract List<StorageResumenDto> GetStorageResumen2Nivel2(PaginacionStorage filtro, out int totalRows);
        public abstract List<StorageResumenDto> GetStorageResumen2Nivel3(PaginacionStorage filtro, out int totalRows);
        public abstract List<StorageResumenDto> GetStorageResumen2Nivel4(PaginacionStorage filtro, out int totalRows);
        public abstract List<StorageResumenDto> GetStorageResumen2Nivel5(PaginacionStorage filtro, out int totalRows);

        public abstract Dictionary<int, List<StorageIndicadorDto>> GetIndicadorGlobal();
        public abstract Dictionary<int, List<StorageIndicadorDto>> GetIndicadorTier(int tier);

        //Asociado al Backup
        public abstract List<BackupDto> GetBackupMainframe(PaginacionStorage filtro, out int totalRows);
        public abstract List<BackupDto> GetBackupMainframeDetalle(PaginacionStorage filtro, out int totalRows);
        public abstract List<OpenDto> GetBackupOpen(PaginacionStorage filtro, out int totalRows);
        public abstract List<OpenDto> GetBackupOpenDetalle(PaginacionStorage filtro, out int totalRows);
        public abstract List<RelacionOpenDto> GetBackupOpenAplicaciones(PaginacionStorage filtro, out int totalRows);
        public abstract List<ResumenOpenDto> GetBackupOpenResumen(PaginacionStorage filtro, out int totalRows);
        public abstract List<GraficoOpenDto> GetBackupOpenResumenDetalle(PaginacionStorage filtro, out int totalRows);
        public abstract List<GraficoOpenDto> GetBackupOpenResumenDetalleTrasnsferencia(PaginacionStorage filtro, out int totalRows);
        public abstract List<BackupPeriodoDto> GetBackupPeriodo(PaginacionStorage filtro, out int totalRows);
        public abstract List<OpenDto> GetBackupOpenExportar(string servidor, int mes, int anio);
    }
}
