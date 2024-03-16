using BCP.CVT.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.Interface
{
    public abstract partial class BlobStorageDAO : ServiceProvider
    {
        public abstract List<ContainerDTO> GetContainers();
        public abstract List<BlobStorageDTO> GetBlobsByFilters(PaginationBlob pag, out int totalRows);
        public abstract byte[] DownloadFileByFilters(string filename, string containerName, string prefix);
    }
}
