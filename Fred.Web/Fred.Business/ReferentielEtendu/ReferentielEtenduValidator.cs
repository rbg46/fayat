using FluentValidation;
using FluentValidation.Results;
using Fred.Entities.ReferentielEtendu;

namespace Fred.Business.ReferentielEtendu
{
  /// <summary>
  ///   Implémentation de la validation d'un ReferentielEtendu
  /// </summary>
  public class ReferentielEtenduValidator : AbstractValidator<ReferentielEtenduEnt>, IReferentielEtenduValidator
  {
    private readonly IParametrageReferentielEtenduValidator paramRefEtenduValidator;

    /// <summary>
    ///   Constructeur
    /// </summary>
    /// <param name="prev"> validateur du référentiel étendu pour valider les éléments enfants</param>
    public ReferentielEtenduValidator(IParametrageReferentielEtenduValidator prev)
    {
      paramRefEtenduValidator = prev;

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
      ////
    }

    /// <summary>
    ///   Ajout des règles des éléments enfants pour une validation en cascade
    /// </summary>
    private void AddChildRules()
    {
      if (paramRefEtenduValidator != null)
      {
        RuleFor(r => r.ParametrageReferentielEtendus).SetCollectionValidator(t => this.paramRefEtenduValidator);
      }
    }

    /// <summary>
    ///   Valider le budget.
    /// </summary>
    /// <param name="instance">ReferentielEtendu à valider.</param>
    /// <returns>Résultat de validation</returns>
    public new ValidationResult Validate(ReferentielEtenduEnt instance)
    {
      // rien de spécial ici pour le moment.
      return base.Validate(instance);
    }
  }
}