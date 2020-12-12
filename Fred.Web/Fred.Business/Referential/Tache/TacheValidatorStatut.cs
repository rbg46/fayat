using FluentValidation.Results;
using Fred.Entities;
using Fred.Entities.Referential;
using Fred.Web.Shared.App_LocalResources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fred.Business.Referential
{
  /// <summary>
  ///   totaly custom validator, n'utilise pas FluentValidation
  ///   Utilsé pour factoriser la RG_1132_012, notament car les tâches sont récupérées de la base et non envoyée par le front
  ///   (Le front n'envoie que les tâches modifiées par l'utilisateur pour des raisons de performance)
  ///   Il est nécessaire de récupérer toutes les tâches de la base pour les vérifier avant le changement de statut de
  ///   Brouillon à Valider
  ///   RG_1132_012 = Validation du budget - Une affectation de ressource à chaque tâche de niveau 4 est indispensable avant
  ///   le changement de statut "à valider" ou "validé"
  /// </summary>
  public class TacheValidatorStatut
  {
    /// <summary>
    ///   Constructeur
    /// </summary>
    /// <param name="statut">Statut de la révision des tâches</param>
    /// <param name="taches">Liste des tâches à valider</param>
    public TacheValidatorStatut(StatutBudget statut, ICollection<TacheEnt> taches)
    {
      Statut = statut;
      Taches = taches;
    }

    /// <summary>
    ///   Révision des tâches à valider
    /// </summary>
    private StatutBudget Statut { get; }

    /// <summary>
    ///   Liste des tâches à valider
    /// </summary>
    private ICollection<TacheEnt> Taches { get; }


    /// <summary>
    ///   Vérifie la règle RG_1132_012: Validation du budget - Une affectation de ressource à chaque tâche de niveau 4 est
    ///   indispensable avant le changement de statut "à valider" ou "validé"
    /// </summary>
    /// <param name="tache">La tâche à tester</param>
    /// <param name="statutRevision">Statut de la révision du budget</param>
    /// <returns>vrai si la règle est vérifiée, sinon faux </returns>
    public static bool ValidateUs1132Rg012(TacheEnt tache, StatutBudget statutRevision)
    {
      if (tache == null)
      {
        return false;
      }

      // Seuls les statuts AValider et Valider sont concernés.
      switch (statutRevision)
      {
        case StatutBudget.AValider:
        case StatutBudget.Valider:
          break;
        default:
          return true;
      }


      // Seules les taches de niveau 4 sont concernées.
      if (tache.Niveau != 4)
      {
        return true;
      }


      if (tache.RessourceTaches == null)
      {
        return false; // il doit y a voir une ou plusieurs ressourceTache
      }

      if (!tache.RessourceTaches.Any())
      {
        return false; // il doit y a voir une ou plusieurs ressourceTache
      }

      // Il y a bien des ressources tâches associées à cette tâche de niveau 4
      return true;
    }


    /// <summary>
    ///   Valide les taches pour un changement de statut de la révision du budgets
    /// </summary>
    /// <returns>Résultat de validation</returns>
    public ValidationResult Validate()
    {
      try
      {
        if (Taches == null)
        {
          return BuildValidationResultFail(string.Format(FeatureTache.Tache_Validator_UnknowError, string.Empty));
        }

        foreach (TacheEnt tache in Taches)
        {
          bool ok = ValidateUs1132Rg012(tache, Statut);
          if (!ok)
          {
            return BuildValidationResultFail(string.Format(FeatureTache.Tache_Validator_Level4Error, tache.Code));
          }
        }
      }
      catch (Exception e)
      {
        return BuildValidationResultFail(string.Format(FeatureTache.Tache_Validator_UnknowError, e.Message));
      }

      return BuildValidationResultSuccess();
    }


    /// <summary>
    ///   Construit un ValidationResult de FluentAssertion qui est passant
    /// </summary>
    /// <returns>ValidationResult</returns>
    private ValidationResult BuildValidationResultSuccess()
    {
      return new ValidationResult();
    }


    /// <summary>
    ///   Construit un ValidationResult de FluentAssertion non passant contenant une erreur
    /// </summary>
    /// <param name="message">message d'erreur</param>
    /// <returns>ValidationResult</returns>
    private ValidationResult BuildValidationResultFail(string message)
    {
      ValidationFailure error = new ValidationFailure(string.Empty, message);
      var errors = new List<ValidationFailure>();
      errors.Add(error);
      return new ValidationResult(errors);
    }
  }
}