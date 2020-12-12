using FluentValidation;
using FluentValidation.Results;
using Fred.Business.ReferentielFixe;
using Fred.Entities.Budget;
using System;
using System.Data;

namespace Fred.Business.Budget
{
  /// <summary>
  ///   Implémentation de la validation du budget
  /// </summary>
  public class RessourceTacheValidatorOld : AbstractValidator<RessourceTacheEnt>, IRessourceTacheValidatorOld
  {
    private readonly IRessourceValidator ressourceValidator;
    private readonly IRessourceTacheDeviseValidatorOld ressourceTacheDeviseValidator;

    /// <summary>
    ///   Constructeur
    /// </summary>
    /// <param name="rValidator">Validateur d'une ressource</param>
    /// <param name="rtdValidator">Validateur d'une RessouceTacheDevise</param>
    public RessourceTacheValidatorOld(IRessourceValidator rValidator, IRessourceTacheDeviseValidatorOld rtdValidator)
    {
      this.CascadeMode = CascadeMode.StopOnFirstFailure;
      this.ressourceValidator = rValidator;
      this.ressourceTacheDeviseValidator = rtdValidator;

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
      // RG_1132_003: Les champs de quantité ne peuvent excéder 999 999 999 999,99 avec une précision maximale de 2 décimales (+ERO : pour prendre en compte les devises avec un cours très faible(ex 1 EUR = 1 384, 56 MMK)
      const double max = 999999999999.99;
      ////RuleFor(e => e.Quantite).NotNull().WithMessage("La quantité doit être renseignée.");
      RuleFor(e => e.Quantite).InclusiveBetween(0, max).WithMessage((r) => $"[{r.RessourceTacheId}] : La quantité doit être comprise entre 0 et " + max + ".");
      ////RuleFor(e => e.QuantiteBase).NotNull().WithMessage("La quantité de base doit être renseignée.");
      RuleFor(e => e.QuantiteBase).InclusiveBetween(0, max).WithMessage((r) => $"[{r.RessourceTacheId}] : La quantité de base doit être comprise entre 0 et " + max + ".");

      // RG_1132_004 : Les champs de montant/P.U. ne peuvent excéder 999 999 999 999,99 avec une précision maximale de 2 décimales
      ////RuleFor(e => e.PrixUnitaire).NotNull().WithMessage("Le prix unitaire doit être renseignée.");
      RuleFor(e => e.PrixUnitaire).InclusiveBetween(0, max).WithMessage((r) => $"[{r.RessourceTacheId}] : Le prix unitaire doit être compris entre 0 et " + max + ".");

      // RG_1132_009 : validation de la formule
      RuleFor(e => e.Formule).Cascade(CascadeMode.StopOnFirstFailure).Must(Validate_Formule).WithMessage((r) => $"[{r.RessourceTacheId}] : La formule n'est pas valide ou le résulat du calcul n'est pas valide. Vous pouvez utiliser + - / * et le caractère q pour désigner la quantité de base.");
    }



    /// <summary>
    /// Calcule la formule et vérifie quelle est égale à la quantité
    /// </summary>
    /// <param name="ressourceTacheEnt">la ressource tâche à valider</param>
    /// <param name="formule">formule de la ressource tâche</param>
    /// <returns>vrai si ok, sinon faux</returns>
    private bool Validate_Formule(RessourceTacheEnt ressourceTacheEnt, string formule)
    {
      try
      {
        // S'il n'y pas de formule
        if (string.IsNullOrEmpty(formule))
        {
          return true;
        }

        if (!ressourceTacheEnt.QuantiteBase.HasValue)
        {
          return false;
        }

        // On force les parenthèses sur quantité de base, cela permet d'obtenir
        // une exception quand utilisateur écrit une formule comme : qq => (q)(q)
        formule = formule.Replace("q", "(" + ressourceTacheEnt.QuantiteBase.ToString() + ")");
        formule = formule.Replace(',', '.');
        Convert.ToDouble(new DataTable().Compute(formule, null));

        return true;
      }
      catch (Exception)
      {
        return false;
      }
    }


    /// <summary>
    ///   Ajout des règles des éléments enfants pour une validation en cascade
    /// </summary>
    private void AddChildRules()
    {
      if (this.ressourceValidator != null)
      {
        RuleFor(r => r.Ressource)
          .Cascade(CascadeMode.StopOnFirstFailure)
          .SetValidator(t => this.ressourceValidator);
      }

      if (this.ressourceTacheDeviseValidator != null)
      {
        RuleFor(r => r.RessourceTacheDevises)
          .Cascade(CascadeMode.StopOnFirstFailure)
          .SetCollectionValidator(t => this.ressourceTacheDeviseValidator);
      }
    }


    /// <summary>
    ///   Valider le budget.
    /// </summary>
    /// <param name="instance">RessourceTache à valider.</param>
    /// <returns>Résultat de validation</returns>
    public new ValidationResult Validate(RessourceTacheEnt instance)
    {
      // rien de spécial ici pour le moment.
      return base.Validate(instance);
    }
  }
}