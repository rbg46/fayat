using Fred.Web.Models.CI;
using Fred.Web.Models.Personnel;
using Fred.Web.Models.Rapport;
using Fred.Web.Models.Referential;
using Fred.Web.Models.ReferentielEtendu;
using Fred.Web.Models.ReferentielFixe;
using Fred.Web.Shared.Models.Bareme;
using System;

namespace Fred.Web.Shared.Models.Valorisation
{
  public class ValorisationModel
  {
    private DateTime periode;

    /// <summary>
    ///   Obtient ou définit l'identifiant unique d'une valorisation.
    /// </summary>
    public int ValorisationId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant du CI
    /// </summary>
    public int CiId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'entité CI
    /// </summary>
    public CIModel CI { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant du CI
    /// </summary>
    public int RapportId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'entité rapport
    /// </summary>
    public RapportModel Rapport { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant d'une ligne de rapport
    /// </summary>
    public int LigneRapportId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'entité ligne de rapport
    /// </summary>
    public RapportLigneModel LigneRapport { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant de la tache
    /// </summary>
    public int TacheId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'entité tache
    /// </summary>
    public TacheModel Tache { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant du chapitre
    /// </summary>

    public int ChapitreId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'entité chapitre
    /// </summary>
    public ChapitreModel Chapitre { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant d'une ressource / nature analytique
    /// </summary>
    public int SousChapitreId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'entité établissement comptable de l'affaire.
    /// </summary>
    public SousChapitreModel SousChapitre { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant d'une ressource / nature analytique
    /// </summary>
    public int ReferentielEtenduId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'entité établissement comptable de l'affaire.
    /// </summary>
    public ReferentielEtenduModel ReferentielEtendu { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant du barème CI
    /// </summary>
    public int BaremeId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'entité barème CI
    /// </summary>
    public BaremeExploitationCIModel Bareme { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant de la devise
    /// </summary>
    public int DeviseId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'entité devise
    /// </summary>
    public DeviseModel Devise { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant du personnel
    /// </summary>
    public int PersonnelId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'entité personnel
    /// </summary>
    public PersonnelModel Personnel { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant du matériel
    /// </summary>
    public int MaterielId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'entité matériel
    /// </summary>
    public MaterielModel Materiel { get; set; }

    /// <summary>
    ///   Obtient ou définit la période
    /// </summary>
    public DateTime Date
    {
      get { return DateTime.SpecifyKind(periode, DateTimeKind.Utc); }
      set { periode = DateTime.SpecifyKind(value, DateTimeKind.Utc); }
    }

    /// <summary>
    ///   Obtient ou définit si la période est verrouillé
    /// </summary>
    public bool VerrouPeriode { get; set; }

    /// <summary>
    ///   Obtient ou définit la date de création
    /// </summary>

    public DateTime DateCreation { get; set; }

    /// <summary>
    ///   Obtient ou définit la date de modififcation
    /// </summary>
    public DateTime DateModification { get; set; }

    /// <summary>
    ///   Obtient ou définit la source de recalcul de la valorisation
    /// </summary>
    public string Source { get; set; }

    /// <summary>
    ///   Obtient ou définit le prix unitaire valorisé
    /// </summary>
    public decimal PUHT { get; set; }

    /// <summary>
    ///   Obtient ou définit la quantité
    /// </summary>
    public decimal Quantite { get; set; }

    /// <summary>
    ///   Obtient ou définit le montant
    /// </summary>
    public decimal Montant { get; set; }

    /// <summary>
    ///   Obtient ou définit le THC
    /// </summary>
    public decimal? TauxHoraireConverti { get; set; }
  }
}

