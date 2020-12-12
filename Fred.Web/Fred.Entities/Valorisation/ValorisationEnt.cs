using Fred.Entities.Bareme;
using Fred.Entities.CI;
using Fred.Entities.Depense;
using Fred.Entities.Personnel;
using Fred.Entities.Rapport;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielEtendu;
using Fred.Entities.ReferentielFixe;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fred.Entities.Valorisation
{
  /// <summary>
  ///   Obtient ou définit l'entité de Valorisation
  /// </summary>
  public class ValorisationEnt
  {
    private DateTime date;

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
    public CIEnt CI { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant du CI
    /// </summary>
    public int RapportId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'entité rapport
    /// </summary>
    public RapportEnt Rapport { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant d'une ligne de rapport
    /// </summary>
    public int RapportLigneId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'entité ligne de rapport
    /// </summary>
    public RapportLigneEnt RapportLigne { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant de la tache
    /// </summary>
    public int TacheId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'entité tache
    /// </summary>
    public TacheEnt Tache { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant du chapitre
    /// </summary>
    public int ChapitreId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'entité chapitre
    /// </summary>
    public ChapitreEnt Chapitre { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant d'une ressource / nature analytique
    /// </summary>
    public int SousChapitreId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'entité établissement comptable de l'affaire.
    /// </summary>
    public SousChapitreEnt SousChapitre { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant d'une ressource / nature analytique
    /// </summary>
    public int ReferentielEtenduId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'entité établissement comptable de l'affaire.
    /// </summary>
    public ReferentielEtenduEnt ReferentielEtendu { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant du barème CI
    /// </summary>
    public int? BaremeId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'entité barème CI
    /// </summary>
    public BaremeExploitationCIEnt Bareme { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant du barème Organisation Storm
    /// </summary>
    public int? BaremeStormId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'entité barème Organisation Storm
    /// </summary>
    public BaremeExploitationOrganisationEnt BaremeStorm { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant de la devise
    /// </summary>
    public int UniteId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'entité unité
    /// </summary>
    public UniteEnt Unite { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant de la unité
    /// </summary>
    public int DeviseId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'entité devise
    /// </summary>
    public DeviseEnt Devise { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant du personnel
    /// </summary>
    public int? PersonnelId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'entité personnel
    /// </summary>
    public PersonnelEnt Personnel { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant du matériel
    /// </summary>
    public int? MaterielId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'entité matériel
    /// </summary>
    public MaterielEnt Materiel { get; set; }

    /// <summary>
    ///   Obtient ou définit la date
    /// </summary>
    public DateTime Date
    {
      get { return DateTime.SpecifyKind(date, DateTimeKind.Utc); }
      set { date = DateTime.SpecifyKind(value, DateTimeKind.Utc); }
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
    public DateTime? DateModification { get; set; }

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


    /// <summary>
    ///   Obtient ou définit le groupe de tâches de remplacement.
    /// </summary>
    public GroupeRemplacementTacheEnt GroupeRemplacementTache { get; set; }

    /// <summary>
    ///   Obtient ou définit l'id du groupe de tâches de remplacement.
    /// </summary>
    public int? GroupeRemplacementTacheId { get; set; }

    /// <summary>
    ///   Obtient ou définit la liste des tâches remplacées    
    /// </summary>
    public IEnumerable<RemplacementTacheEnt> RemplacementTaches { get; set; }
  }
}
