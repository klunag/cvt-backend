using BCP.CVT.DTO;
using System.Collections.Generic;

namespace BCP.CVT.Services.Interface
{
    public abstract class AzureResourceDAO : ServiceProvider
    {        
        public abstract List<AzureResourceTypeDto> GetAzureTypes(Paginacion pag, out int totalRows);
        public abstract void UpdateActiveStatus(int resourceId, string user);
        public abstract bool UpdateVirtualMachineStatus(int resourceId, string user);

        public abstract List<AzureResourcesDTO> GetAllAzureResources(PaginationAzure pag, out int totalRows);
    }
}
