using FluentValidation;
using Fred.Entities.Module;

namespace Fred.Business.Module
{
  /// <summary>
  ///   Interface du valideur de modules
  /// </summary>
  public interface IModuleValidator : IValidator<ModuleEnt>
  {
  }
}