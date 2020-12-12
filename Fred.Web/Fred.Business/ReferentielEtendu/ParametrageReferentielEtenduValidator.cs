using FluentValidation;
using FluentValidation.Results;
using Fred.Entities.ReferentielEtendu;

namespace Fred.Business.ReferentielEtendu
{
  /// <summary>
  ///   Implémentation de la validation d'un ParametrageReferentielEtendu
  /// </summary>
  public class ParametrageReferentielEtenduValidator : AbstractValidator<ParametrageReferentielEtenduEnt>, IParametrageReferentielEtenduValidator
  {
    /// <summary>
    ///   Constructeur
    /// </summary>
    public ParametrageReferentielEtenduValidator()
    {
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
      ////
    }

    /// <summary>
    ///   Valider le budget.
    /// </summary>
    /// <param name="instance">ParametrageReferentielEtendu à valider.</param>
    /// <returns>Résultat de validation</returns>
    public new ValidationResult Validate(ParametrageReferentielEtenduEnt instance)
    {
      // rien de spécial ici pour le moment.
      return base.Validate(instance);
    }
  }
}
