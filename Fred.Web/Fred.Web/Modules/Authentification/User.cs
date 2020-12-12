using Fred.Entites;
using Microsoft.AspNet.Identity;

namespace Fred.Web.Modules.Authentification
{
  public class User : IUser<int>
  {
    public int Id { get; set; }
    public string UserName { get; set; }
    public string FirstName { get; set; }

    public AuthenticationStatus Status { get; set; }
  }
}