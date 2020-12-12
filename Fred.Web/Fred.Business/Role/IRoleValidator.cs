using FluentValidation;
using Fred.Entities.Role;

namespace Fred.Business.Role
{
  /// <summary>
  /// Interface Valideur de Rôles
  /// </summary>
  public interface IRoleValidator : IValidator<RoleEnt>
  {
  }
}