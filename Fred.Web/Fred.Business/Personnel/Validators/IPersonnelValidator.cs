using FluentValidation;
using Fred.Entities.Personnel;

namespace Fred.Business.Personnel
{
  /// <summary>
  ///   Interface du valideur des Personnels
  /// </summary>
  public interface IPersonnelValidator : IValidator<PersonnelEnt>
  {
  }
}