using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.EnvioCorreos
{
    public class MailDTO
    {
        public Message message { get; set; }

        public bool saveToSentItems { get; set; }


        public class Message
        {
            public string subject { get; set; }
            public Body body { get; set; }
            public List<EmailAdress> toRecipients { get; set; }
            public List<EmailAdress> ccRecipients { get; set; }
            public List<FileAttachment> attachments { get; set; }

            public class Body
            {
                public string contentType { get; set; }
                public string content { get; set; }
            }

            public class FileAttachment
            {
                public string odatatype { get; set; }
                public byte[] contentBytes { get; set; }
                public string contentType { get; set; }
                public string contentId { get; set; }
                public string name { get; set; }
            }

            public class EmailAdress
            {
                public Adress emailAddress { get; set; }
            }

            public class Adress
            {
                public string address { get; set; }
            }
        }

    }
}
