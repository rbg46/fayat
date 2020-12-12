using Fred.Web.Models.Referential;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fred.Web.Models.ReferentielFixe
{
  /// <summary>
  /// Représente un sous-chapitre
  /// </summary>
  public class SousChapitreModel : IReferentialModel
  {

    private int count;

    /// <summary>
    /// Obtient ou définit l'identifiant unique d'un sous-chapitre.
    /// </summary>
    public int SousChapitreId { get; set; }

    /// <summary>
    /// Obtient ou définit le code d'un sous-chapitre
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    /// Obtient ou définit le libellé d'un sous-chapitre
    /// </summary>
    public string Libelle { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant unique d'un chapitre.
    /// </summary>
    public int ChapitreId { get; set; }

    /// <summary>
    /// Obtient ou définit le chapitre du sous-chapitre
    /// </summary>
    public ChapitreModel Chapitre { get; set; }

    /// <summary>
    /// Obtient ou définit la liste des ressources associées au sous-chapitre
    /// </summary>
    public ICollection<RessourceModel> Ressources { get; set; }

    /// <summary>
    /// Obtient ou définit la concaténation du code et du libelle
    /// </summary>
    public string CodeLibelle
    {
      get
      {
        return this.Code + " - " + this.Libelle;
      }
    }
    public bool IsChecked { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant du référentiel
    /// </summary>
    public string IdRef => this.SousChapitreId.ToString();

    /// <summary>
    /// Obtient ou définit le libelle du référentiel
    /// </summary>
    public string LibelleRef => this.Libelle;

    /// <summary>
    /// Obtient ou définit le code du référentiel
    /// </summary>
    public string CodeRef => this.Code;


    public int CountRessourcesToBeTreated
    {
      get
      {
        try
        {
          count = 0;

          if (this.Ressources != null)
          {
            foreach (var ressource in Ressources)
            {
              if (ressource.ReferentielEtendus == null || !ressource.ReferentielEtendus.Any() || (ressource.ReferentielEtendus != null && ressource.ReferentielEtendus.Any()
                && !ressource.ReferentielEtendus.ToList()[0].NatureId.HasValue && ressource.Active && !ressource.DateSuppression.HasValue))
              {
                count++;
              }
            }
          }
        }
        catch (Exception e)
        {

        }
        return count;
      }
      set
      {
        count = value;
      }
    }

    /// <summary>
    /// Gets nombre des ressouces sans parametrage
    /// </summary>
    public int CountParamRefEtenduToBeTreated
    {
      get
      {
        try
        {
          count = 0;

          //On récupère la liste des référentiels étendus
          var refEtendus = this.Ressources
                           .SelectMany(r => r.ReferentielEtendus)
                           .Where(c => c.Ressource.Active && !c.Ressource.DateSuppression.HasValue)
                           .ToList();

          foreach (var refEtendu in refEtendus)
          {
            if (refEtendu.ParametrageReferentielEtendus.Count(p => p.Montant.HasValue) == 0)
            {
              count++;
            }
          }
        }
        catch (Exception e)
        {

        }

        return count;
      }
      set
      {
        count = value;
      }
    }
  }
}