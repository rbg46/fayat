using System;
using System.Collections.Generic;
using System.Diagnostics;
using Fred.Entities.Affectation;
using Fred.Entities.IndemniteDeplacement;
using Fred.Entities.Personnel.Interimaire;
using Fred.Entities.Rapport;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielFixe;
using Fred.Entities.Societe;
using Fred.Entities.Utilisateur;
using Fred.Entities.Valorisation;

namespace Fred.Entities.Personnel
{
  /// <summary>
  ///   Représente un membre du personnel
  /// </summary>
  [DebuggerDisplay("PersonnelId = {PersonnelId} Nom = {Nom} Prenom = {Prenom} Utilisateur = {Utilisateur}")]
  public class PersonnelEnt : ICloneable
  {
    private DateTime? dateModification;
    private DateTime? dateSortie;
    private DateTime? dateSuppression;
    private DateTime? dateEntree;
    private DateTime? dateCreation;

    /// <summary>
    ///   Obtient ou définit l'identifiant unique du personnel.
    /// </summary>
    public int PersonnelId { get; set; }

    /// <summary>
    ///   Obtient l'id utilisateur associé au personnel.
    /// </summary>
    public int? UtilisateurId
    {
      get
      {
        return this.Utilisateur != null ? Utilisateur.UtilisateurId : default(int?);
      }
    }

    /// <summary>
    ///   obtient ou définit l'utilisateur associé au personnel.
    /// </summary>  
    public UtilisateurEnt Utilisateur { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant unique de la société.
    /// </summary>
    public int? SocieteId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant unique de la société.
    /// </summary>
    public SocieteEnt Societe { get; set; }

    /// <summary>
    ///   Obtient ou définit le matricule du personnel.
    /// </summary>
    public string Matricule { get; set; }

    /// <summary>
    ///   Obtient ou définit le nom du personnel.
    /// </summary>
    public string Nom { get; set; }

    /// <summary>
    ///   Obtient ou définit le prénom du personnel.
    /// </summary>
    public string Prenom { get; set; }

    /// <summary>
    ///   Obtient ou définit l'id ressource du personnel.
    /// </summary>
    public int? RessourceId { get; set; }

    /// <summary>
    ///   Obtient ou définit le personnel.
    /// </summary>
    public RessourceEnt Ressource { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant du matériel par défaut.
    /// </summary>
    public int? MaterielId { get; set; }

    /// <summary>
    ///   Obtient ou définit le matériel par défaut.
    /// </summary>
    public MaterielEnt Materiel { get; set; }

    /// <summary>
    ///   Obtient ou définit le statut du Personnel.
    /// </summary>    
    public string Statut { get; set; }

    /// <summary>
    ///   Obtient ou définit la catégorie du Personnel Interne.
    /// </summary>    
    public string CategoriePerso { get; set; }

    /// <summary>
    ///   Obtient ou définit une valeur indiquant si le personnel est interne ou externe.
    /// </summary>
    public bool IsInterne { get; set; }

    /// <summary>
    ///   Obtient ou définit la date de modification.
    /// </summary>
    public DateTime? DateModification
    {
      get
      {
        return (dateModification.HasValue) ? DateTime.SpecifyKind(dateModification.Value, DateTimeKind.Utc) : default(DateTime?);
      }
      set
      {
        dateModification = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?);
      }
    }

    /// <summary>
    ///   Obtient ou définit la date de création.
    /// </summary>
    public DateTime? DateCreation
    {
      get
      {
        return (dateCreation.HasValue) ? DateTime.SpecifyKind(dateCreation.Value, DateTimeKind.Utc) : default(DateTime?);
      }
      set
      {
        dateCreation = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?);
      }
    }

    /// <summary>
    ///   Obtient ou définit la date de suppression.
    /// </summary>
    public DateTime? DateSuppression
    {
      get
      {
        return (dateSuppression.HasValue) ? DateTime.SpecifyKind(dateSuppression.Value, DateTimeKind.Utc) : default(DateTime?);
      }
      set
      {
        dateSuppression = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?);
      }
    }

    /// <summary>
    ///   Obtient ou définit l'id utilisateur ayant fait la création du personnel.
    /// </summary>   
    public int? UtilisateurIdCreation { get; set; }

    /// <summary>   
    ///   Obtient ou définit l'id utilisateur ayant fait la modification du personnel.
    /// </summary>  
    public int? UtilisateurIdModification { get; set; }

    /// <summary>
    ///   Obtient ou définit l'id utilisateur ayant fait la suppression du personnel.
    /// </summary>   
    public int? UtilisateurIdSuppression { get; set; }

    /// <summary>
    ///   Obtient ou définit une valeur indiquant si le personnel est intérimaire ou non.
    /// </summary>
    public bool IsInterimaire { get; set; }

    /// <summary>
    ///   Obtient ou définit le code établissement de paye du personnel.
    /// </summary>
    public int? EtablissementPaieId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'établissement de paye du personnel
    /// </summary>
    public EtablissementPaieEnt EtablissementPaie { get; set; }

    /// <summary>
    ///   Obtient ou définit l'Adresse 1 du personnel.
    /// </summary>
    public string Adresse1 { get; set; }

    /// <summary>
    ///   Obtient ou définit l'Adresse 2 du personnel.
    /// </summary>
    public string Adresse2 { get; set; }

    /// <summary>
    ///   Obtient ou définit l'Adresse 3 du personnel.
    /// </summary>
    public string Adresse3 { get; set; }

    /// <summary>
    ///   Obtient ou définit le code postal du personnel.
    /// </summary>
    public string CodePostal { get; set; }

    /// <summary>
    ///   Obtient ou définit la ville du personnel.
    /// </summary>
    public string Ville { get; set; }

    /// <summary>
    ///   Obtient ou définit le label du pays
    /// </summary>
    public string PaysLabel { get; set; }

    /// <summary>
    ///   Obtient ou définit l'id pays du personnel.
    /// </summary>
    public int? PaysId { get; set; }

    /// <summary>
    ///   Obtient ou définit le pays du personnel.
    /// </summary>
    public PaysEnt Pays { get; set; } = null;

    // REMARQUE LORS DE LE MIGRATION CODE FIRST 
    // DENORMALISATION : LE NOM DE LA COLONNE EST DIFFERENT DU NOM DE LA PROPRIÉTÉ.
    /// <summary>
    ///   Obtient ou définit le numéro de téléphone 1 du personnel.
    /// </summary>
    public string Telephone1 { get; set; }

    // REMARQUE LORS DE LE MIGRATION CODE FIRST 
    // DENORMALISATION : LE NOM DE LA COLONNE EST DIFFERENT DU NOM DE LA PROPRIÉTÉ.
    /// <summary>
    ///   Obtient ou définit le numéro de téléphone 2 du personnel.
    /// </summary>
    public string Telephone2 { get; set; }

    /// <summary>
    ///   Obtient ou définit l'email du personnel.
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    ///   Obtient ou définit la longitude GPS du personnel.
    /// </summary>
    public double? LongitudeDomicile { get; set; }

    /// <summary>
    ///   Obtient ou définit la latitude du personnel.
    /// </summary>
    public double? LatitudeDomicile { get; set; }

    /// <summary>
    ///   Obtient ou définit le point de rattachement du personnel.
    /// </summary>
    public int? EtablissementRattachementId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'établissement de rattachement lié
    /// </summary>
    public EtablissementPaieEnt EtablissementRattachement { get; set; }

    /// <summary>
    ///   Obtient ou définit le point de rattachement du personnel.
    /// </summary>
    public string TypeRattachement { get; set; }

    /// <summary>
    ///   Obtient ou définit la date d'entrée du personnel.
    /// </summary>
    public DateTime? DateEntree
    {
      get
      {
        return (dateEntree.HasValue) ? DateTime.SpecifyKind(dateEntree.Value, DateTimeKind.Utc) : default(DateTime?);
      }
      set
      {
        dateEntree = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?);
      }
    }

    /// <summary>
    ///   Obtient ou définit la date de sortie du personnel.
    /// </summary>
    public DateTime? DateSortie
    {
      get
      {
        return (dateSortie.HasValue) ? DateTime.SpecifyKind(dateSortie.Value, DateTimeKind.Utc) : default(DateTime?);
      }
      set
      {
        dateSortie = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?);
      }
    }

    /// <summary>
    ///   Obtient une concaténation du nom et du prénom du membre du personnel
    /// </summary>
    public string NomPrenom => Nom + " " + Prenom;

    /// <summary>
    ///   Obtient une concaténation du prénom et du nom du membre du personnel
    /// </summary>
    public string PrenomNom => Prenom + " " + Nom;

    /// <summary>
    ///   Obtient une concaténation du code, du prénom et du nom du membre du personnel
    /// </summary>
    public string CodeNomPrenom
    {
      get
      {
        string code = Matricule ?? string.Empty;
        string nom = Nom ?? string.Empty;
        string prenom = Prenom ?? string.Empty;

        return code + " - " + nom + " " + prenom;
      }
    }

    /// <summary>
    ///   Obtient le libellé référentiel d'un personnel.
    /// </summary>
    public string LibelleRef => Prenom + " " + Nom;

    /// <summary>
    ///   Obtient l'Adresse formaté du personnel
    /// </summary>
    public string Adresse => string.Format("{0} {1} {2} , {3} , {4} , {5}", Adresse1, Adresse2, Adresse3, CodePostal, Ville, Pays != null ? Pays.Libelle : string.Empty);

    /// <summary>
    ///   Obtient ou définit une valeur indiquant si les admin RH ont modifié manuellement les coordonnées GPS.
    /// </summary>
    public bool? IsSaisieManuelle { get; set; }

    /// <summary>
    /// Obtient ou definit l'identifiant unique de l'equipe
    /// </summary>
    public int? EquipeFavoriteId { get; set; }

    /// <summary>
    /// Obtient ou definit l'equipe entité
    /// </summary>
    public EquipeEnt Equipe { get; set; }

    /// <summary>
    /// Obtient ou definit l'identifiant unique de l'equipe
    /// </summary>
    public int? PersonnelImageId { get; set; }

    /// <summary>
    /// Obtient ou definit l'equipe entité
    /// </summary>
    public PersonnelImageEnt PersonnelImage { get; set; }

    /// <summary>
    ///   Obtient ou définit le contrat actif si intérimaire
    /// </summary>
    public ContratInterimaireEnt ContratActif { get; set; }

    /// <summary>
    /// Obtient ou definit l'identifiant unique du manager
    /// </summary>
    public int? ManagerId { get; set; }

    /// <summary>
    /// Obtient ou definit le manager entité
    /// </summary>
    public PersonnelEnt Manager { get; set; }

    /// <summary>
    /// Obtient ou definit le code d'emploi
    /// </summary>
    public string CodeEmploi { get; set; }

    /// <summary>
    /// Obtient ou definit le personnel à des heures d'insertion ou non 
    /// </summary>
    public bool HeuresInsertion { get; set; }

    /// <summary>
    /// Obtient ou definit la date de début d'insertion
    /// </summary>
    public DateTime? DateDebutInsertion { get; set; }

    /// <summary>
    /// Obtient ou definit la date de fin d'insertion
    /// </summary>
    public DateTime? DateFinInsertion { get; set; }

    /// <summary>
    /// Obtient ou definit si le personnel est pointable
    /// </summary>
    public bool IsPersonnelNonPointable { get; set; }

    /// <summary>
    /// Obtient ou definit si le import timestamp
    /// </summary>
    public ulong? TimestampImport { get; set; }

    ///////////////////////////////////////////////////////////////////////////
    // AJOUT LORS DE LE MIGRATION CODE FIRST 
    ///////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// Child AffectationInterimaires where [FRED_PERSONNEL_FOURNISSEUR_SOCIETE].[PersonnelId] point to this entity (FK_FRED_PERSONNEL_FOURNISSEUR_SOCIETE_PERSONNEL)
    /// </summary>
    public virtual ICollection<ContratInterimaireEnt> ContratInterimaires { get; set; }

    /// <summary>
    /// Child IndemniteDeplacements where [FRED_INDEMNITE_DEPLACEMENT].[PersonnelId] point to this entity (fk_indemnitePersonnel)
    /// </summary>
    public virtual ICollection<IndemniteDeplacementEnt> IndemniteDeplacements { get; set; }

    /// <summary>
    /// Child PointageAnticipes where [FRED_POINTAGE_ANTICIPE].[PersonnelId] point to this entity (FK_POINTAGE_ANTICIPE_PERSONNEL)
    /// </summary>
    public virtual ICollection<PointageAnticipeEnt> PointageAnticipes { get; set; }

    /// <summary>
    /// Child RapportLignes where [FRED_RAPPORT_LIGNE].[PersonnelId] point to this entity (FK_RAPPORT_LIGNE_PERSONNEL)
    /// </summary>
    public virtual ICollection<RapportLigneEnt> RapportLignes { get; set; }

    /// <summary>
    /// Child Affectatiions where [FRED_AFFECTATION].[PersonnelId] point to this enttity (FK_AFFECTATION_PERSONNEL)
    /// </summary>
    public virtual ICollection<AffectationEnt> Affectations { get; set; }

    /// <summary>
    /// Child EquipePersonnel where [FRED_EQUIPE_PERSONNEL].PersonnelId point to this entity (Fk_PersonnelId)
    /// </summary>
    public virtual ICollection<EquipePersonnelEnt> EquipePersonnels { get; set; }

    /// <summary>
    /// Child Petsonnel for manager where [Fred_Personnel].ManagerId point to this
    /// </summary>
    public virtual ICollection<PersonnelEnt> ManagerPersonnels { get; set; }

    /// <summary>
    /// Liste des valorisations du personnel
    /// </summary>
    public virtual ICollection<ValorisationEnt> Valorisations { get; set; }

    /// <summary>
    ///   Obtient ou définit les matricules externes
    /// </summary>
    public virtual ICollection<MatriculeExterneEnt> MatriculeExterne { get; set; }

    /// <summary>
    /// Retourne le clone de l'entité personnel
    /// </summary>
    /// <returns>Clone de l'entité personnel</returns>
    public object Clone()
    {
      return (PersonnelEnt)this.MemberwiseClone();
    }
  }
}
