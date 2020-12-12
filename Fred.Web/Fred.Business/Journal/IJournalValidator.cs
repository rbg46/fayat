using FluentValidation;
using Fred.Entities.Journal;

namespace Fred.Business.Journal
{
  /// <summary>
  /// Interface pour valider les journaux comptables
  /// </summary>
  public interface IJournalValidator : IValidator<JournalEnt>
  {
  }
}
