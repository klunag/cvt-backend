namespace BCP.CVT.Cross.Models
{
    public class VirtualMachineInfoModel
    {
        public string computerName { get; set; }
        public string osName { get; set; }
        public string osVersion { get; set; }

        //public string osNameStr => !string.IsNullOrWhiteSpace(osName) ? osName : string.Empty;
        //public string osVersionStr => !string.IsNullOrWhiteSpace(osVersion) ? osVersion : string.Empty;

        //public string fullName => $"{osNameStr} - {osVersionStr}";
        public string fullName => !string.IsNullOrWhiteSpace(osName) && !string.IsNullOrWhiteSpace(osVersion)
                                    ? $"{osName} - {osVersion}" : string.Empty;
    }

    public class VirtualMachineModel
    {
        public string name { get; set; }
        public string id { get; set; }
        public string soNew { get; set; }

        public PropertiesModel properties { get; set; }

        public string subscriptionId => !string.IsNullOrWhiteSpace(id) ? id.Split('/')[2] : string.Empty;
        public string resourceGroupName => !string.IsNullOrWhiteSpace(id) ? id.Split('/')[4] : string.Empty;

        public string so => properties.storageProfile.imageReference == null ?
            string.Empty : properties.storageProfile.imageReference.resourceSO;

        public string id_vm => properties.storageProfile.imageReference == null ?
            string.Empty : properties.storageProfile.imageReference.id;

        public string soType => properties.storageProfile.osDisk == null ?
            string.Empty : properties.storageProfile.osDisk.osType;
    }

    public class PropertiesModel
    {
        public StorageProfileModel storageProfile { get; set; }
    }

    public class StorageProfileModel
    {
        public ImageReferenceModel imageReference { get; set; }
        public OnDisk osDisk { get; set; }
    }

    public class ImageReferenceModel
    {
        public string id { get; set; }

        public string publisher { get; set; }
        public string offer { get; set; }
        public string sku { get; set; }
        public string version { get; set; }
        public string exactVersion { get; set; }

        public string publisherStr => !string.IsNullOrWhiteSpace(publisher) ? publisher : string.Empty;
        public string offerStr => !string.IsNullOrWhiteSpace(offer) ? offer : string.Empty;
        public string skuStr => !string.IsNullOrWhiteSpace(sku) ? sku : string.Empty;
        public string exactVersionStr => !string.IsNullOrWhiteSpace(exactVersion) ? exactVersion : string.Empty;

        public string resourceSO => $"{publisherStr} {offerStr} {skuStr} {exactVersionStr}";
    }

    public class OnDisk
    {
        public string osType { get; set; }
    }
}
