using Fred.Entities.CI;
using Fred.Entities.Personnel;
using Fred.Entities.Referential;
using Fred.Entities.Utilisateur;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fred.Entities.Rapport
{
  /// <summary>
  ///   Classe de base représentant les pointage
  /// </summary>
  public abstract class PointageBase
  {

    /// <summary>
    ///   Obtient ou définit l'id de la ligne de rapport
    /// </summary>
    public abstract int PointageId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'id du rapport auquel est rattachée la ligne de rapport
    /// </summary>
    public abstract int CiId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'entité Rapport
    /// </summary>
    public abstract CIEnt Ci { get; set; }

    /// <summary>
    ///   Obtient ou définit le prénom nom temporaire
    /// </summary>
    public abstract string PrenomNomTemporaire { get; set; }

    /// <summary>
    ///   Obtient ou définit l'id de l'entité personnel
    /// </summary>
    public abstract int? PersonnelId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'entité Personnel
    /// </summary>
    public abstract PersonnelEnt Personnel { get; set; }

    /// <summary>
    ///   Obtient le nombre maximum d'heures de travail sur une journée
    /// </summary>
    public abstract int MaxHeuresTravailleesJour { get; }

    /// <summary>
    ///   Obtient ou définit l'heure normale
    /// </summary>
    public abstract double HeureNormale { get; set; }

    /// <summary>
    /// Obtient ou définit le nombre d'heures de marche du matériel
    /// </summary>
    public abstract double MaterielMarche { get; set; }

    /// <summary>
    ///   Obtient ou définit l'id de l'entité CodeMajoration
    /// </summary>
    public abstract int? CodeMajorationId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'entité CodeMajoration
    /// </summary>
    public abstract CodeMajorationEnt CodeMajoration { get; set; }

    /// <summary>
    ///   Obtient ou définit le l'heure majorée
    /// </summary>
    public abstract double HeureMajoration { get; set; }

    /// <summary>
    ///   Obtient ou définit l'id de l'entité code absence
    /// </summary>
    public abstract int? CodeAbsenceId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'entité CodeAbsence
    /// </summary>
    public abstract CodeAbsenceEnt CodeAbsence { get; set; }

    /// <summary>
    ///   Obtient ou définit le l'heure de l'absence
    /// </summary>
    public abstract double HeureAbsence { get; set; }

    /// <summary>
    ///   Obtient ou définit la semaine de l'intemperie
    /// </summary>
    public abstract int? NumSemaineIntemperieAbsence { get; set; }

    /// <summary>
    ///   Obtient ou définit l'id de l'entité code déplacement
    /// </summary>
    public abstract int? CodeDeplacementId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'entité CodeDeplacement de la ligne de rapport
    /// </summary>
    public abstract CodeDeplacementEnt CodeDeplacement { get; set; }

    /// <summary>
    ///   Obtient ou définit l'id de l'entité CodeZoneDeplacement
    /// </summary>
    public abstract int? CodeZoneDeplacementId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'entité CodeZoneDeplacement
    /// </summary>
    public abstract CodeZoneDeplacementEnt CodeZoneDeplacement { get; set; }

    /// <summary>
    ///   Obtient ou définit une valeur indiquant si la colonne IVD est cochée
    /// </summary>
    public abstract bool DeplacementIV { get; set; }

    /// <summary>
    ///   Obtient ou définit la date de pointage
    /// </summary>
    public abstract DateTime DatePointage { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant de l'auteur de la création
    /// </summary>
    public abstract int? AuteurCreationId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'auteur de la création
    /// </summary>
    public abstract UtilisateurEnt AuteurCreation { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant de l'auteur de la modification
    /// </summary>
    public abstract int? AuteurModificationId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'auteur de la modification
    /// </summary>
    public abstract UtilisateurEnt AuteurModification { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant de l'auteur de la suppression
    /// </summary>
    public abstract int? AuteurSuppressionId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'auteur de la suppression
    /// </summary>
    public abstract UtilisateurEnt AuteurSuppression { get; set; }

    /// <summary>
    ///   Obtient ou définit la date de création de la ligne
    /// </summary>
    public abstract DateTime? DateCreation { get; set; }

    /// <summary>
    ///   Obtient ou définit la date de modification de la ligne
    /// </summary>
    public abstract DateTime? DateModification { get; set; }

    /// <summary>
    ///   Obtient ou définit la date de suppression de la ligne
    /// </summary>
    public abstract DateTime? DateSuppression { get; set; }

    /// <summary>
    ///   Obtient ou définit le nombre maximum de primes à saisir dans le pointage
    /// </summary>
    [NotMapped]
    public abstract int NbMaxPrimes { get; set; }

    /// <summary>
    ///   Obtient ou définit une valeur indiquant si la ligne est en création
    /// </summary>
    [NotMapped]
    public abstract bool IsCreated { get; }

    /// <summary>
    ///   Obtient ou définit une valeur indiquant si la ligne est à supprimer
    /// </summary>
    [NotMapped]
    public abstract bool IsDeleted { get; set; }

    /// <summary>
    ///   Obtient ou définit la liste des erreurs de saisie sur la ligne de rapport
    /// </summary>
    [NotMapped]
    public abstract ICollection<string> ListErreurs { get; set; }

    /// <summary>
    ///   Obtient ou définit la liste des prime d'un pointages
    /// </summary>
    [NotMapped]
    public virtual ICollection<PointagePrimeBase> ListePrimes { get; set; }

    /// <summary>
    ///   Obtient ou définit la liste des majorations d'un pointages
    /// </summary>
    [NotMapped]
    public virtual ICollection<PointageMajorationBase> ListeMajorations { get; set; }

    /// <summary>
    ///   Obtient ou définit le type du pointage si il est anticipé
    /// </summary>
    [NotMapped]
    public abstract bool IsAnticipe { get; }

    /// <summary>
    ///   Obtient ou définit l'heure normale
    /// </summary>
    public abstract double HeureTotalTravail { get; }

    /// <summary>
    ///   Obtient ou définit une valeur indiquant si ReadOnly.
    /// </summary>
    [NotMapped]
    public bool ReadOnly { get; set; }

    /// <summary>
    ///   Obtient ou définit une valeur indiquant si ReadOnly.
    /// </summary>
    [NotMapped]
    public bool HeureAbsenceReadOnly { get; set; }

    /// <summary>
    ///   Obtient ou définit une valeur indiquant si ReadOnly.
    /// </summary>
    [NotMapped]
    public bool CodeDeplacementReadOnly { get; set; }

    /// <summary>
    ///   Obtient ou définit une valeur indiquant si ReadOnly.
    /// </summary>
    [NotMapped]
    public bool CodeZoneDeplacementReadOnly { get; set; }

    /// <summary>
    ///   Obtient ou définit une valeur indiquant si le pointage a été généré.
    /// </summary>
    public abstract bool IsGenerated { get; set; }

    /// <summary>
    ///   Nettoyage des propriétés
    /// </summary>
    public void CleanProperties()
    {
      Ci = null;
      CiId = 0;
      CodeAbsence = null;
      CodeAbsenceId = null;
      CodeDeplacement = null;
      CodeDeplacementId = null;
      CodeMajoration = null;
      CodeMajorationId = null;
      CodeZoneDeplacement = null;
      CodeZoneDeplacementId = null;
      DeplacementIV = false;
      HeureAbsence = 0;
      HeureMajoration = 0;
      HeureNormale = 0;
      NumSemaineIntemperieAbsence = null;
      Personnel = null;
      PersonnelId = null;
      PrenomNomTemporaire = null;
    }
  }
}