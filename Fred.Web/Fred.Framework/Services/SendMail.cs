using Fred.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Web;

namespace Fred.Framework.Services
{
    /// <summary>
    ///   <para/> Permet d'envoyer une mail en utilisant le protocol SMPT.
    ///   <para/> Les paramètres du serveur de mail sont récupérés depuis la config.
    ///   <para/> VOUS DEVEZ APPELER IDisposable.Dispose ou utiliser Using().
    ///   <para/> Pour utiliser un serveur mock : https://github.com/rnwood/smtp4dev
    /// <example>
    ///   <para/> Exemple d'utilisation :
    /// <code>
    ///   <para/> using (SendMail sender = new SendMail())
    ///   <para/> {
    ///   <para/> sender.From("test@test.com", "Prenom Nom");
    ///   <para/> sender.To("test@test.com");
    ///   <para/> sender.Subject = "test";
    ///   <para/> sender.Body = "This is a test e-mail.";
    ///   <para/> sender.AddAttachement(@"D:\file.pdf");
    ///   <para/> sender.ServerAddress = "smtp.test.com";
    ///   <para/> sender.ServerPort = 25;
    ///   <para/> sender.Send();
    ///   <para/> }
    /// </code>
    /// </example>
    /// </summary>
    public class SendMail : IDisposable
    {
#if DEBUG
        private readonly string login;
        private readonly string mp;
        private readonly string mailTest = "nmathlouthi@sqli.com";//votre
#endif
        /// <summary>
        ///   Initialise une nouvelle instance de la classe<see cref="SendMail"/>.
        ///   Récupère les paramètres du serveur dans la config
        /// </summary>
        public SendMail()
        {
#if DEBUG
            ServerAddress = "smtp.office365.com"; //votre serveur Smpt exemple
            ServerPort = 587; // port serveur Smtp
            this.login = "nmathlouthi@sqli.com"; //votre login OutLook
            this.mp = ":p"; //votre MP OutLook
#else
            Fred.Framework.Tool.ToolManager config = new Fred.Framework.Tool.ToolManager();
            ServerAddress = config.GetConfig("eMail:Server:Adress");
            ServerPort = int.Parse(config.GetConfig("eMail:Server:Port"));
#endif
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="SendMail"/> class.
        /// Because disposable types implement a finalizer.
        /// </summary>
        ~SendMail()
        {
            Dispose(false);
        }

        /// <summary>
        ///   Objet décrivant le message à envoyer
        ///   Permet d'utiliser des options avancées
        ///   Si vous utilisez cet objetet, ajouter votre code dans cette classe pour en faire bénéficier tout le monde si c'est justifié...
        /// </summary>
        public MailMessage EMail { get; } = new MailMessage();

        /// <summary>
        ///   Liste des destinataires de l'email
        /// </summary>
        public MailAddressCollection Tos => EMail.To;

        /// <summary>
        ///   Liste des destinataires en copie de l'email
        /// </summary>
        public MailAddressCollection ToCc => EMail.CC;

        /// <summary>
        ///   Liste des destinataires en copie carbonne de l'email
        /// </summary>
        public MailAddressCollection ToBcc => EMail.Bcc;

        /// <summary>
        ///   Sujet du mail
        /// </summary>
        public string Subject
        {
            get { return EMail.Subject; }
            set { EMail.Subject = value; }
        }

        /// <summary>
        ///   Corps du mail.
        ///   Si vous voulez des options avancées comme un mail html, utilisez EMail
        /// </summary>
        public string Body
        {
            get { return EMail.Body; }
            set { EMail.Body = value; }
        }

        /// <summary>
        ///   Adresse IP ou Hostname du serveur
        /// </summary>
        public string ServerAddress { get; set; }

        /// <summary>
        ///   Port du serveur, par défaut 25
        /// </summary>
        public int ServerPort { get; set; }

        /// <summary>
        ///   Retourne la liste des pièces jointes
        /// </summary>
        public AttachmentCollection Attachments => EMail.Attachments;

        /// <summary>
        ///   Ajoute une pièce jointe au mail
        /// </summary>
        /// <param name="fullPath">
        ///   Un String qui contient un chemin d’accès de fichier à utiliser pour créer cette pièce jointe.
        /// </param>
        /// <param name="fileName">Nom de la piece jointe voulue</param>
        /// <param name="contentType">
        ///   Un String qui contient les informations d’en-tête de contenu MIME pour cette pièce jointe.
        ///   Utilisez par exemple MediaTypeNames.Text.Plain pour du texte ou MediaTypeNames.Application.Octet pour du binaire
        ///   (valeur par défaut)
        /// </param>
        /// <exception cref="T:Fred.Framework.Exceptions.FredTechnicalException">
        ///   <paramref name="fullPath" /> is null.
        /// </exception>
        /// 
        public void AddAttachement(string fullPath, string fileName, string contentType = MediaTypeNames.Application.Octet)
        {
            try
            {
                // Create  the file attachment for this e-mail message.
                Attachment attachment = new Attachment(fullPath, contentType);

                // Affecter le nom du fichier
                attachment.Name = fileName;

                // Add the file attachment to this e-mail message.
                EMail.Attachments.Add(attachment);
            }
            catch (ArgumentNullException e)
            {
                throw new FredTechnicalException("Une erreur est survenue lors de l'ajout de la pièce jointe.", e);
            }
            catch (ArgumentException e)
            {
                throw new FredTechnicalException("Une erreur est survenue lors de l'ajout de la pièce jointe.", e);
            }
        }

        /// <summary>
        ///   Ajoute une pièce jointe au mail
        /// </summary>
        /// <param name="contentStream">
        ///   Lisible Stream qui contient le contenu de cette pièce jointe.
        /// </param>
        /// <param name="name">Nom du fichier de la pièce jointe</param>
        /// <param name="contentType">
        ///   Un String qui contient les informations d’en-tête de contenu MIME pour cette pièce jointe.
        ///   Utilisez par exemple MediaTypeNames.Text.Plain pour du texte ou MediaTypeNames.Application.Octet pour du binaire
        ///   (valeur par défaut)
        /// </param>
        /// <exception cref="T:Fred.Framework.Exceptions.FredTechnicalException">
        ///   <paramref name="contentStream" /> is null.
        /// </exception>
        public void AddAttachement(Stream contentStream, string name, string contentType = MediaTypeNames.Application.Octet)
        {
            try
            {
                // déplacer le curseur au début du fichier.
                contentStream.Seek(0, SeekOrigin.Begin);

                // Create  the file attachment for this e-mail message.
                Attachment data = new Attachment(contentStream, contentType);
                data.Name = name;

                // Add the file attachment to this e-mail message.
                EMail.Attachments.Add(data);
            }
            catch (ArgumentNullException e)
            {
                throw new FredTechnicalException("Une erreur est survenue lors de l'ajout de la pièce jointe.", e);
            }
        }

        /// <summary>
        ///   Ajoute un destinataire au mail
        /// </summary>
        /// <param name="addressEmail">A <see cref="T:System.String" /> that contains an e-mail address.</param>
        /// <param name="displayName">
        ///   A <see cref="T:System.String" /> that contains the display name associated with
        ///   <paramref name="addressEmail" />. This parameter can be null.
        /// </param>
        public void To(string addressEmail, string displayName)
        {
            EMail.To.Add(new MailAddress(addressEmail, displayName, Encoding.UTF8));
        }

        /// <summary>
        ///   Ajoute un destinataire au mail
        /// </summary>
        /// <param name="addressEmail">A <see cref="T:System.String" /> that contains an e-mail address.</param>
        public void To(string addressEmail)
        {
            To(addressEmail, string.Empty);
        }

        /// <summary>
        ///   Personne qui envoie le mail
        /// </summary>
        /// <param name="addressEmail">A <see cref="T:System.String" /> that contains an e-mail address.</param>
        /// <param name="displayName">
        ///   A <see cref="T:System.String" /> that contains the display name associated with
        ///   <paramref name="addressEmail" />. This parameter can be null.
        /// </param>
        public void From(string addressEmail, string displayName)
        {
            EMail.From = new MailAddress(addressEmail, displayName, Encoding.UTF8);
        }

        /// <summary>
        ///   Personne qui envoie le mail
        /// </summary>
        /// <param name="addressEmail">A <see cref="T:System.String" /> that contains an e-mail address.</param>
        public void From(string addressEmail)
        {
            From(addressEmail, string.Empty);
        }

        /// <summary>
        ///   Envoie l'email.
        ///   Les différents éléments doivent avoir été renseignés. (from, to, subjet, body ...)
        ///   #########################################################################
        ///   Configuration avec smtp4dev :
        ///   sender.Host = "localhost";
        ///   sender.Port = 25;
        ///   #########################################################################
        /// </summary>
        /// <exception cref="FredTechnicalException">En cas d'erreur d'envoie"</exception>
        public void Send()
        {
            try
            {
                using (SmtpClient sender = new SmtpClient())
                {
                    sender.Host = ServerAddress;
                    sender.Port = ServerPort;
                    sender.Timeout = 60000; // 60 secondes
                    sender.Send(EMail);
                }
            }
            catch (SmtpFailedRecipientsException e)
            {
                throw new FredTechnicalException("Les destinataires de l'email ne sont pas valides.", e);
            }
            catch (SmtpFailedRecipientException e)
            {
                throw new FredTechnicalException("Le destinataire de l'email n'est pas valide.", e);
            }
            catch (SmtpException e)
            {
                throw new FredTechnicalException("Une erreur SMTP est survenue lors de l'envoi de l'email.", e);
            }
            catch (Exception e)
            {
                throw new FredTechnicalException("Une erreur inconnue est survenue lors de l'envoi de l'email.", e);
            }
        }

        /// <summary>
        /// Generer le Body bMail
        /// </summary>
        /// <param name="myDictionary">Dictionary value appent</param>
        /// <param name="templateFileName">Path for Teamplate HTML</param>
        /// <returns>retourn un format de l'Email</returns>
        public string PopulateBody(Dictionary<string, string> myDictionary, string templateFileName)
        {
            string body = string.Empty;

            using (StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("/Templates/BodyHtml/" + templateFileName)))
            {
                body = reader.ReadToEnd();
            }
            foreach (var item in myDictionary)
            {
                body = body.Replace(item.Key, item.Value);
            }
            return body;
        }

        /// <summary>
        /// Send Mail
        /// </summary>
        /// <param name="fromEmail">Adresse Email From Send</param>
        /// <param name="toEmail">Adresse Email To Send</param>
        /// <param name="subject">subject  Email</param>
        /// <param name="body">corps de L'email</param>
        /// <param name="isBodyHtml">Format Html ou non</param>
        public void SendFormattedEmail(string fromEmail, string toEmail, string subject, string body, bool isBodyHtml = false)
        {
            try
            {
                EMail.Subject = subject;
                EMail.Body = body;
                EMail.IsBodyHtml = isBodyHtml;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = ServerAddress;
                smtp.Port = ServerPort;
                smtp.Timeout = 60000; // 60 secondes

#if DEBUG
                smtp.EnableSsl = true;
                EMail.From = new MailAddress(mailTest);
                EMail.To.Add(new MailAddress(mailTest));
                System.Net.NetworkCredential networkCredential = new System.Net.NetworkCredential();
                networkCredential.UserName = login;
                networkCredential.Password = mp;
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = networkCredential;
#else
            EMail.From = new MailAddress(fromEmail);
            EMail.To.Add(new MailAddress(toEmail));
#endif
                smtp.Send(EMail);
            }
            catch (SmtpFailedRecipientsException e)
            {
                throw new FredTechnicalException("Les destinataires de l'email ne sont pas valides.", e);
            }
            catch (SmtpFailedRecipientException e)
            {
                throw new FredTechnicalException("Le destinataire de l'email n'est pas valide.", e);
            }
            catch (SmtpException e)
            {
                throw new FredTechnicalException("Une erreur SMTP est survenue lors de l'envoi de l'email.", e);
            }
            catch (Exception e)
            {
                throw new FredTechnicalException("Une erreur inconnue est survenue lors de l'envoi de l'email.", e);
            }
        }

        /// <summary>
        ///   Releases all resources used by the System.Net.Mail.MailMessage.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///   Releases the unmanaged resources used by the System.Net.Mail.MailMessage and optionally releases the managed
        ///   resources.
        /// </summary>
        /// <param name="disposing">
        ///   true to release both managed and unmanaged resources; false to release only unmanaged
        ///   resources.
        /// </param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2213:DisposableFieldsShouldBeDisposed", MessageId = "<EMail>k__BackingField", Justification = "code analysis bug.")]
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                EMail?.Dispose(); // détruit aussi les pièces jointes
            }
        }
    }
}
