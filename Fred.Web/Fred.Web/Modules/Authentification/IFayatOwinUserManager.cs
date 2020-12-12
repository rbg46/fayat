using System.Threading.Tasks;

namespace Fred.Web.Modules.Authentification
{
  public interface IFayatOwinUserManager
  {
    Task<User> FindAsync(string userName, string password);
    Task SignInAsync(User user, bool isPersistent);
    void SignOut();
  }
}