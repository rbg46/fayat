using FluentValidation;
using Fred.Entities;

namespace Fred.Business.Personnel
{
  /// <summary>
  ///   Interface du valideur des images du personnel
  /// </summary>
  public interface IPersonnelImageValidator : IValidator<PersonnelImageEnt>
  {
  }
}