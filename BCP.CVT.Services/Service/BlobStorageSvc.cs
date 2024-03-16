using BCP.CVT.Cross;
using BCP.CVT.DTO;
using BCP.CVT.Services.Interface;
using log4net;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.Services.Service
{
    public class BlobStorageSvc : BlobStorageDAO
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);        

        public override byte[] DownloadFileByFilters(string filename, string containerName, string prefix)
        {
            try
            {
                byte[] response = null;
                prefix = string.IsNullOrWhiteSpace(prefix) ? null : prefix;
                CloudStorageAccount mycloudStorageAccount = CloudStorageAccount.Parse(this.GetStorageKey());
                CloudBlobClient blobClient = mycloudStorageAccount.CreateCloudBlobClient();

                CloudBlobContainer container = blobClient.GetContainerReference(containerName);
                var cloudBlockBlob = container
                                        .ListBlobs(prefix, false, BlobListingDetails.Metadata)
                                        .OfType<CloudBlockBlob>()
                                        .FirstOrDefault(x => x.Name == filename);

                //CloudBlockBlob cloudBlockBlob = container.GetBlockBlobReference(filename);
                if (cloudBlockBlob != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        cloudBlockBlob.DownloadToStream(memoryStream);
                        response = memoryStream.ToArray();
                    }
                }

                return response;
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorDominioDTO
                    , "Error en el metodo: List<BlobStorageDTO> GetBlobsByFilters(PaginationBlob pag, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorDominioDTO
                    , "Error en el metodo: List<BlobStorageDTO> GetBlobsByFilters(PaginationBlob pag, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<BlobStorageDTO> GetBlobsByFilters(PaginationBlob pag, out int totalRows)
        {
            try
            {
                totalRows = 0;
                pag.Prefix = string.IsNullOrWhiteSpace(pag.Prefix) ? null : pag.Prefix;
                //CloudBlobDirectory dir;
                //CloudBlob blob;

                CloudStorageAccount cloudStorage = CloudStorageAccount.Parse(this.GetStorageKey());
                CloudBlobClient blobClient = cloudStorage.CreateCloudBlobClient();
                CloudBlobContainer ctn = blobClient.GetContainerReference(pag.Container);
                //var blobs = ctn.ListBlobs().Select(x => (CloudBlockBlob)x)
                //    .Select(x => new BlobStorageDTO(
                //        x.Name,
                //        x.Properties.Length,
                //        x.Properties.LastModified,
                //        x.Properties.Created,
                //        x.Uri.ToString(),
                //        ctn.Name
                //        ));

                var listBlobs = new List<BlobStorageDTO>();
                BlobStorageDTO itemBlob = null;

                var blobs = ctn.ListBlobs(pag.Prefix, false, BlobListingDetails.Metadata);
                //BlobResultSegment resultSegment = ctn.ListBlobsSegmented(pag.Prefix,
                //false, BlobListingDetails.Metadata, null, null, null, null);

                foreach (var blobItem in blobs)
                {
                    if (blobItem is CloudBlobDirectory)
                    {
                        CloudBlobDirectory dir = (CloudBlobDirectory)blobItem;
                        itemBlob = new BlobStorageDTO(dir.Prefix, true, ctn.Name);
                    }
                    else
                    {
                        CloudBlob blob = (CloudBlob)blobItem;
                        
                        itemBlob = new BlobStorageDTO(
                            blob.Name,
                            blob.Properties.Length,
                            blob.Properties.LastModified,
                            blob.Properties.Created,
                            blob.Uri.ToString(),
                            ctn.Name,
                            false,
                            blob.Parent.Prefix
                        );
                    }
                    listBlobs.Add(itemBlob);
                }

                var blobsFiltered = listBlobs
                    .Where(x => !x.LastModified.HasValue || x.LastModified.Value.Date == pag.DateFilter.Value.Date)
                    .OrderBy(pag.sortName + " " + pag.sortOrder);

                totalRows = blobsFiltered.Count();
                var results = blobsFiltered.Skip((pag.pageNumber - 1) * pag.pageSize).Take(pag.pageSize).ToList();               

                return results;
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorDominioDTO
                    , "Error en el metodo: List<BlobStorageDTO> GetBlobsByFilters(PaginationBlob pag, out int totalRows)"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorDominioDTO
                    , "Error en el metodo: List<BlobStorageDTO> GetBlobsByFilters(PaginationBlob pag, out int totalRows)"
                    , new object[] { null });
            }
        }

        public override List<ContainerDTO> GetContainers()
        {
            try
            {
                CloudStorageAccount cloudStorage = CloudStorageAccount.Parse(this.GetStorageKey());
                CloudBlobClient blobClient = cloudStorage.CreateCloudBlobClient();
                var lstContainers = blobClient.ListContainers()
                    .Select(x => new ContainerDTO() {
                        id = Guid.NewGuid().ToString(),
                        text = x.Name,
                        selected = true
                    }).ToList();

                return lstContainers;
            }
            catch (DbEntityValidationException ex)
            {
                log.ErrorEntity(ex);
                throw new CVTException(CVTExceptionIds.ErrorDominioDTO
                    , "Error en el metodo: List<ContainerDTO> GetContainers()"
                    , new object[] { null });
            }
            catch (Exception ex)
            {
                log.Error("Error ", ex);
                throw new CVTException(CVTExceptionIds.ErrorDominioDTO
                    , "Error en el metodo: List<ContainerDTO> GetContainers()"
                    , new object[] { null });
            }
        }

        private string GetStorageKey()
        {
            return Constantes.CadenaConexionStorage;
        }
    }
}
