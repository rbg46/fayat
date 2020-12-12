using Microsoft.AspNet.Identity;
using System.Threading.Tasks;

namespace Fred.ImportExport.Framework.Services
{
  /// <summary>
  /// Gestionnaire de l'envoi des SMS
  /// </summary>
  public class SmsService : IIdentityMessageService
  {
    /// <summary>
    /// Permet d'envoyer un SMS
    /// </summary>
    /// <param name="message">Le message à envoyer</param>
    /// <returns>Une opération asynchrone</returns>
    public Task SendAsync(IdentityMessage message)
    {
      // Plug in your SMS service here to send a text message.
      return Task.FromResult(0);
    }
  }
}
