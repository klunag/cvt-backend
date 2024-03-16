using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.DTO
{
    public class BlobStorageDTO
    {
        public string Name { get; set; }
        public long? Size { get; set; }
        public DateTimeOffset? LastModified { get; set; }
        public DateTimeOffset? Created { get; set; }
        public string Uri { get; set; }
        public string ContainerName { get; set; }
        public bool IsDirectory { get; set; }
        public string Prefix { get; set; }

        public BlobStorageDTO()
        {

        }

        public BlobStorageDTO(string _Prefix, bool _IsDirectory, string _ContainerName)
        {
            Prefix = _Prefix;
            IsDirectory = _IsDirectory;
            ContainerName = _ContainerName;
        }

        public BlobStorageDTO(string _Name, 
            long _Size, 
            DateTimeOffset? _LastModified, 
            DateTimeOffset? _Created, 
            string _Uri, 
            string _ContainerName,
            bool _IsDirectory,
            string _Prefix)
        {
            Name = _Name;
            Size = _Size;
            LastModified = _LastModified;
            Created = _Created;
            Uri = _Uri;
            ContainerName = _ContainerName;
            IsDirectory = _IsDirectory;
            Prefix = _Prefix;
        }

        public string SizeStr => Size.HasValue ? $"{Size.Value / 1000} KB" : string.Empty;
        public string LastModifiedStr => LastModified.HasValue? LastModified.Value.ToString("dd/MM/yyyy") : string.Empty;
    }

    public class ContainerDTO
    {
        public string id { get; set; }
        public string text { get; set; }
        public bool selected { get; set; }
    }
}
