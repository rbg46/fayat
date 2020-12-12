using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Fred.Entities.Journal;

namespace Fred.Business.Journal
{
  /// <summary>
  ///   Valideur des journaux comptables
  /// </summary>
  public class JournalValidator : AbstractValidator<JournalEnt>, IJournalValidator
  {
    /// <summary>
    ///   Initialise une nouvelle instance de la classe <see cref="JournalValidator" />.
    /// </summary>   
    public JournalValidator()
    {
      RuleFor(j => j).NotNull().WithMessage(JournalResource.JournalNonSpecifie);

      RuleFor(j => j.Code).NotNull().Length(1, 3).WithMessage(JournalResource.LongueurCode);
      RuleFor(j => j.Libelle).NotNull().NotEmpty().Length(1, 250).WithMessage(JournalResource.LongueurLibelle);
    }
  }
}