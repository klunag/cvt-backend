namespace BCP.CVT.Cross.Models
{
    public class ResourceModel
    {
        public string id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string location { get; set; }
        public string provisioningState { get; set; }
        public string resourceGroup => !string.IsNullOrEmpty(id) ? id.Split('/')[4] : string.Empty;

        public string resourceSO { get; set; }
        public string aksVersion { get; set; }
    }
}
