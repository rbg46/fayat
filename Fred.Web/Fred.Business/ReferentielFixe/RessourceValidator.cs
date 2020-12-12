using FluentValidation;
using FluentValidation.Results;
using Fred.Business.ReferentielEtendu;
using Fred.Entities.ReferentielFixe;

namespace Fred.Business.ReferentielFixe
{
  /// <summary>
  ///   Implémentation de la validation d'une ressource
  /// </summary>
  public class RessourceValidator : AbstractValidator<RessourceEnt>, IRessourceValidator
  {
    private readonly IReferentielEtenduValidator referentielEtenduValidator;

    /// <summary>
    ///   Constructeur
    /// </summary>
    /// <param name="rev">Validateur du référentiel étendu pour une validation des éléments enfants</param>
    public RessourceValidator(IReferentielEtenduValidator rev)
    {
      referentielEtenduValidator = rev;

      // Règles de gestions techniques
      AddTechnicalRules();

      // Règles de gestions métiers
      AddBusinessRules();

      // Règles de gestion en cascade
      AddChildRules();
    }

    /// <summary>
    ///   Ajout de règles de validation techniques
    /// </summary>
    private void AddTechnicalRules()
    {
      ////
    }

    /// <summary>
    ///   Ajout des règles de validation métiers
    /// </summary>
    private void AddBusinessRules()
    {
      RuleFor(e => e.Code).NotEmpty().WithMessage("Le code de la ressource doit être renseignée.");
      RuleFor(e => e.Libelle).NotEmpty().WithMessage("Le libellé de la ressource doit être renseigné.");
    }

    /// <summary>
    ///   Ajout des règles des éléments enfants pour une validation en cascade
    /// </summary>
    private void AddChildRules()
    {
      if (this.referentielEtenduValidator != null)
      {
        RuleFor(r => r.ReferentielEtendus).SetCollectionValidator(t => this.referentielEtenduValidator);
      }
    }

    /// <summary>
    ///   Valider le budget.
    /// </summary>
    /// <param name="instance">Ressource à valider.</param>
    /// <returns>Résultat de validation</returns>
    public new ValidationResult Validate(RessourceEnt instance)
    {
      // rien de spécial ici pour le moment.
      return base.Validate(instance);
    }
  }
}
