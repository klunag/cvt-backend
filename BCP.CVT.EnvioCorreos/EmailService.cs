using Microsoft.Exchange.WebServices.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BCP.CVT.EnvioCorreos
{
    public class EmailService
    {
        const string CHAR_SPLIT = ";";
        static string CORREO;
        static string PASS;
        static ExchangeService service;
        public EmailService(string USER_CORREO, string USER_PASS)
        {
            CORREO = USER_CORREO;
            PASS = USER_PASS;
            service = ServiceEWS.ConectarServicio(CORREO, PASS);
        }
        public void SendEmail(string correoFrom, string nombreMostrarFrom, string correoTo, string correoCC, string correoBCC
            , MailPriority prioridad, string asunto, string cuerpo, bool EsHtml, Dictionary<string, MemoryStream> listaAdjuntos = null)
        {

            var listaTo = correoTo.Split(Convert.ToChar(CHAR_SPLIT)).ToList();
            var listaCC = (!string.IsNullOrEmpty(correoCC) ? correoCC.Split(Convert.ToChar(CHAR_SPLIT)).ToList() : new List<string>());
            var listaBCC = (!string.IsNullOrEmpty(correoBCC) ? correoBCC.Split(Convert.ToChar(CHAR_SPLIT)).ToList() : new List<string>());

            EmailMessage mensaje = new EmailMessage(service);
            mensaje.From = new EmailAddress(nombreMostrarFrom, correoFrom);

            foreach (var to in listaTo)
                mensaje.ToRecipients.Add(new EmailAddress(to));
            foreach (var cc in listaCC)
                mensaje.CcRecipients.Add(new EmailAddress(cc));
            foreach (var bcc in listaBCC)
                mensaje.BccRecipients.Add(new EmailAddress(bcc));


            if (listaAdjuntos != null && listaAdjuntos.Count != 0)
            {
                var i = 0;
                foreach (var adjunto in listaAdjuntos)
                {
                    mensaje.Attachments.AddFileAttachment(adjunto.Key, adjunto.Value);
                    mensaje.Attachments[i].ContentId = adjunto.Key;
                    mensaje.Attachments[i].IsInline = false;
                    i = i + 1;
                }
            }
            mensaje.Subject = asunto;
            mensaje.Body = new MessageBody(BodyType.HTML, cuerpo);
            mensaje.SendAndSaveCopy();
        }


        public void EmailDraft(List<EmailAddress> listaCorreosPara
                        , List<EmailAddress> listaCorreosCC
                        , List<EmailAddress> listaCorreoBcc
                        , string correoAsunto
                        , string correoCuerpo
                        , Dictionary<string, byte[]> listaAdjuntos, string carpetaBuzon)
        {
            EmailMessage mensaje = new EmailMessage(service);
            mensaje.ToRecipients.AddRange(listaCorreosPara);
            mensaje.CcRecipients.AddRange(listaCorreosCC);
            mensaje.BccRecipients.AddRange(listaCorreoBcc);


            mensaje.Subject = correoAsunto;
            mensaje.Body = correoCuerpo;

            foreach (var adjunto in listaAdjuntos)
            {
                mensaje.Attachments.AddFileAttachment(adjunto.Key, adjunto.Value);
            }

            Folder rootfolder = Folder.Bind(service, WellKnownFolderName.MsgFolderRoot);
            PropertySet propsToLoad = new PropertySet(FolderSchema.DisplayName,
                                                      FolderSchema.ChildFolderCount,
                                                      FolderSchema.FolderClass,
                                                      FolderSchema.Id);
            rootfolder.Load(propsToLoad);
            var resultado = rootfolder.FindFolders(new FolderView(100)).ToList().Find(item => item.DisplayName.Equals(carpetaBuzon));

            if (resultado != null)
            {
                mensaje.Save(resultado.Id);
            }
            else
            {

                mensaje.Save(rootfolder.Id);
            }
        }
    }
}
