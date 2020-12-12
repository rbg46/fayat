using Fred.Entities.Referential;
using Fred.Web.Models.CI;
using Fred.Web.Models.CodeAbsence;
using Fred.Web.Models.Personnel;
using Fred.Web.Models.Referential;
using Fred.Web.Models.Utilisateur;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fred.Web.Models.Rapport
{
  public class RapportLigneModel
  {
    /// <summary>
    /// Obtient ou définit l'identifiant unique de la ligne du rapport
    /// </summary>
    public int PointageId { get; set; }

    /// <summary>
    /// Obtient ou définit le fait que la ligne d'un rapport soit sélectionné de l'UI
    /// </summary>
    public bool IsChecked { get; set; }

    /// <summary>
    /// Obtient ou définit l'id du rapport auquel est rattachée la ligne de rapport
    /// </summary>
    public int RapportId { get; set; }

    /// <summary>
    /// Obtient ou définit l'entité Rapport
    /// </summary>
    public RapportModel Rapport { get; set; }

    /// <summary>
    /// Obtient ou définit l'id du CI associé au pointage
    /// </summary>
    public int? CiId { get; set; }

    /// <summary>
    /// Obtient ou définit l'entié CI associée au pointage
    /// </summary>
    public CIModel Ci { get; set; }

    /// <summary>
    /// Obtient ou définit le prénom nom temporaire
    /// </summary>
    public string PrenomNomTemporaire { get; set; }

    /// <summary>
    /// Obtient ou définit l'id de l'entité personnel
    /// </summary>
    public int? PersonnelId { get; set; }

    /// <summary>
    /// Obtient ou définit l'entité Personnel
    /// </summary>
    public PersonnelModel Personnel { get; set; } = null;

    /// <summary>
    /// Obtient ou définit l'heure normale
    /// </summary>
    public double HeureTotalTravail {
      get
      {
        return this.HeureNormale + this.HeureMajoration;
      }
    }

    /// <summary>
    /// Obtient le nombre maximum d'heures de travail sur une journée
    /// </summary>
    public int MaxHeuresTravailleesJour { get; set; }

    /// <summary>
    /// Obtient ou définit l'heure Totale
    /// </summary>
    public double HeureNormale { get; set; }

    /// <summary>
    /// Obtient le code majoration
    /// </summary>
    public string CodeMajorationField
    {
      get
      {
        if (this.CodeMajoration != null)
        {
          return this.CodeMajoration.Code;
        }
        else
        {
          return string.Empty;
        }

      }
    }

    /// <summary>
    /// Obtient ou définit l'id de l'entité CodeMajoration
    /// </summary>
    public int? CodeMajorationId { get; set; }

    /// <summary>
    /// Obtient ou définit l'entité CodeMajoration
    /// </summary>
    public CodeMajorationModel CodeMajoration { get; set; } = null;

    /// <summary>
    /// Obtient ou définit le l'heure majorée
    /// </summary>
    public double HeureMajoration { get; set; }

    /// <summary>
    /// Obtient le code absences
    /// </summary>
    public string CodeAbsenceField
    {
      get
      {
        if (this.CodeAbsence != null)
        {
          return this.CodeAbsence.Code;
        }
        else
        {
          return string.Empty;
        }
      }
    }

    /// <summary>
    /// Obtient ou definit le nom temporaire du matériel
    /// </summary>
    public string MaterielNomTemporaire { get; set; }

    /// <summary>
    /// Obtient ou définit l'id de l'entité code absence
    /// </summary>
    public int? CodeAbsenceId { get; set; }

    /// <summary>
    /// Obtient ou définit l'entité CodeAbsence
    /// </summary>
    public CodeAbsenceModel CodeAbsence { get; set; }

    /// <summary>
    /// Obtient ou définit l'id de l'entité matériel
    /// </summary>
    public int? MaterielId { get; set; }

    /// <summary>
    /// Obtient ou définit l'entité Materiel
    /// </summary>
    public MaterielModel Materiel { get; set; }

    /// <summary>
    /// Obtient ou définit le l'heure de l'absence
    /// </summary>
    public double HeureAbsence { get; set; }

    /// <summary>
    /// Obtient ou définit la semaine de l'intemperie
    /// </summary>
    public int? NumSemaineIntemperieAbsence { get; set; }

    /// <summary>
    /// Obtient ou définit l'id de l'entité code déplacement
    /// </summary>
    public int? CodeDeplacementId { get; set; }

    /// <summary>
    /// Obtient ou définit l'entité CodeDeplacement de la ligne de rapport
    /// </summary>
    public CodeDeplacementModel CodeDeplacement { get; set; }

    /// <summary>
    /// Obtient ou définit l'id de l'entité CodeZoneDeplacement
    /// </summary>
    public int? CodeZoneDeplacementId { get; set; }

    /// <summary>
    /// Obtient ou définit l'entité CodeZoneDeplacement
    /// </summary>
    public CodeZoneDeplacementEnt CodeZoneDeplacement { get; set; }

    /// <summary>
    /// Obtient ou définit l'entité IVDeplacement de la ligne de rapport
    /// </summary>
    public bool DeplacementIV { get; set; }

    /// <summary>
    /// Obtient ou définit le temps de Marche matériel
    /// </summary>
    public double MaterielMarche { get; set; }

    /// <summary>
    /// Obtient ou définit le temps d'Arret matériel
    /// </summary>
    public double MaterielArret { get; set; }

    /// <summary>
    /// Obtient ou définit le temps de Panne matériel
    /// </summary>
    public double MaterielPanne { get; set; }
    
    /// <summary>
    /// Obtient ou définit le numéro de la semaine en intempérie
    /// </summary>
    public double MaterielIntemperie { get; set; }

    /// <summary>
    /// Obtient ou définit la date du pointage
    /// </summary>
    public DateTime? DatePointage { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant de l'auteur de la création
    /// </summary>
    public int? AuteurCreationId { get; set; }

    /// <summary>
    /// Obtient ou définit l'auteur de la création
    /// </summary>
    public UtilisateurModel AuteurCreation { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant de l'auteur de la modification
    /// </summary>
    public int? AuteurModificationId { get; set; }

    /// <summary>
    /// Obtient ou définit l'auteur de la modification
    /// </summary>
    public UtilisateurModel AuteurModification { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant de l'auteur de la suppression
    /// </summary>
    public int? AuteurSuppressionId { get; set; }

    /// <summary>
    /// Obtient ou définit l'auteur de la suppression
    /// </summary>
    public UtilisateurModel AuteurSuppression { get; set; }

    /// <summary>
    /// Obtient ou définit la date de création
    /// </summary>
    public DateTime? DateCreation { get; set; }

    /// <summary>
    /// Obtient ou définit la date de modification
    /// </summary>
    public DateTime? DateModification { get; set; }

    /// <summary>
    /// Obtient ou définit la date de suppression
    /// </summary>
    public DateTime? DateSuppression { get; set; }

    /// <summary>
    /// Obtient ou définit le nombre maximum de primes à saisir dans un pointage
    /// </summary>
    public int NbMaxPrimes { get; set; }

    /// <summary>
    /// Obtient ou définit la liste des primes
    /// </summary>
    public RapportLignePrimeModel[] ListPrimes { get; set; }

    /// <summary>
    /// Obtient ou définit la liste des taches
    /// </summary>
    public RapportLigneTacheModel[] ListTaches { get; set; }

    /// <summary>
    /// Obtient ou définit le fait que la ligne soit en création
    /// </summary
    public bool IsCreated { get; set; } = false;

    /// <summary>
    /// Obtient ou définit le fait que la ligne soit en modification
    /// </summary>
    public bool IsUpdated { get; set; } = false;

    /// <summary>
    /// Obtient ou définit le fait que la ligne soit à supprimer
    /// </summary
    public bool IsDeleted { get; set; } = false;

    /// <summary>
    /// Obtient ou définit si la ligne est prévisionnelle ou non
    /// </summary>
    public bool IsPrevisionnelle { get; set; }

    /// <summary>
    /// Obtient ou définit le type de rapport : 
    /// false => rapport personnel
    /// true => rapport matériel
    /// null => rapport personnel et materiel
    /// </summary>
    public bool? RapportLigneType { get; set; }

    /// <summary>
    /// Obtient ou définit la liste des erreurs de saisie sur la ligne de rapport
    /// </summary>
    public string[] ListErreurs { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si ReadOnly.
    /// </summary>
    public bool ReadOnly { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si ReadOnly.
    /// </summary>
    public bool HeureAbsenceReadOnly { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si ReadOnly.
    /// </summary>
    public bool CodeDeplacementReadOnly { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si ReadOnly.
    /// </summary>
    public bool CodeZoneDeplacementReadOnly {
      get
      {
        if (this.CodeDeplacement != null)
        {
          return (this.CodeDeplacement.IGD);
        }
        else
        {
          return true;
        }
      }
    }

    /// <summary>
    /// Obtient ou défini le type du pointage (Anticipe lu reel)
    /// </summary>
    public bool IsAnticipe { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si le pointage a été généré.
    /// </summary>
    public bool IsGenerated { get; set; }
  }
}