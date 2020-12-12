using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace Fred.ImportExport.Framework.Services
{
  /// <summary>
  /// Gestionnaire de l'envoi des emails
  /// </summary>
  public class EmailService : IIdentityMessageService
  {
    /// <summary>
    /// Permet d'envoyer un email
    /// </summary>
    /// <param name="message">Le message à envoyer</param>
    /// <returns>Une opération asynchrone</returns>
    public Task SendAsync(IdentityMessage message)
    {
      // Plug in your email service here to send an email.
      return Task.FromResult(0);
    }
  }
}
