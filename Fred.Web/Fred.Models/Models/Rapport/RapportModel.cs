using Fred.Web.Models.CI;
using Fred.Web.Models.Referential;
using Fred.Web.Models.Utilisateur;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Fred.Entities.Rapport;

namespace Fred.Web.Models.Rapport
{
  public class RapportModel
  {
    /// <summary>
    /// Obtient ou définit l'identifiant du rapport
    /// </summary>
    public int RapportId { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant du statut du rapport
    /// </summary>
    public int RapportStatutId { get; set; }

    /// <summary>
    ///  Obtient ou définit l'entité CI
    /// </summary>
    public RapportStatutModel RapportStatut { get; set; }

    /// <summary>
    /// Obtient ou définit la date du chantier
    /// </summary>
    public DateTime DateChantier { get; set; }


    /// <summary>
    /// Obtient ou définit la date du rapport
    /// </summary>
    public DateTime DateRapport { get; set; }

    /// <summary>
    /// Obtient ou définit l'horaire de debut le matin
    /// </summary>
    public DateTime HoraireDebutM { get; set; }

    /// <summary>
    /// Obtient ou définit l'horaire de fin le matin
    /// </summary>
    public DateTime HoraireFinM { get; set; }

    /// <summary>
    /// Obtient ou définit l'horaire de debut le soir
    /// </summary>
    public DateTime? HoraireDebutS { get; set; }

    /// <summary>
    /// Obtient ou définit l'horaire de fin le soir
    /// </summary>
    public DateTime? HoraireFinS { get; set; }

    /// <summary>
    /// Obtient ou définit la météo
    /// </summary>
    public string Meteo { get; set; }

    /// <summary>
    /// Obtient ou définit les évènements du jour 
    /// </summary>
    public string Evenements { get; set; }

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
    /// Obtient ou définit l'identifiant de l'auteur du verrouillage
    /// </summary>
    public int? AuteurVerrouId { get; set; }

    /// <summary>
    /// Obtient ou définit l'auteur du verrouillage
    /// </summary>
    public UtilisateurModel AuteurVerrou { get; set; }

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
    /// Obtient ou définit la date de verrouillage
    /// </summary>
    public DateTime? DateVerrou { get; set; }

    /// <summary>
    /// Obtient ou définit l'identifiant du CI
    /// </summary>
    public int CiId { get; set; }

    /// <summary>
    /// Obtient ou définit le CI
    /// </summary>
    public CIModel CI { get; set; }

    /// <summary>
    /// Obtient ou définit la liste des lignes du rapport
    /// </summary>
    public RapportLigneModel[] ListLignes { get; set; }

    /// <summary>
    /// Obtient ou définit le nombre maximum de primes à saisir dans le rapport journalier
    /// </summary>
    public int NbMaxPrimes { get; set; }

    /// <summary>
    /// Obtient ou définit la liste des primes à paramétrer dans le rapport
    /// </summary>
    public PrimeModel[] ListPrimes { get; set; }

    /// <summary>
    /// Obtient ou définit le nombre maximum de taches à saisir dans le rapport journalier
    /// </summary>
    public int NbMaxTaches { get; set; }

    /// <summary>
    /// Obtient ou définit la liste des taches à paramétrer dans le rapport
    /// </summary>
    public TacheModel[] ListTaches { get; set; }


    /// <summary>
    /// Indique si le rapport est au statut Brouillon
    /// </summary>
    public bool IsStatutEnCours { get; set; }

    /// <summary>
    /// Indique si le rapport est au statut validé Rédacteur
    /// </summary>
    public bool IsStatutValideRedacteur { get; set; }

    /// <summary>
    /// Indique si le rapport est au statut validé Conducteur
    /// </summary>
    public bool IsStatutValideConducteur { get; set; }

    /// <summary>
    /// Indique si le rapport est au statut validé Direction
    /// </summary>
    public bool IsStatutValideDirection { get; set; }

    /// <summary>
    /// Indique si le rapport est au statut Vérouillé
    /// </summary>
    public bool IsStatutVerrouille { get; set; }

    /// <summary>
    /// Indique si le rapport possède un des trois statuts de validation
    /// </summary>
    public bool IsStatutValide { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si ReadOnly.
    /// </summary>
    public bool ReadOnly { get; set; }

    /// <summary>
    ///  Obtient ou définit l'identifiant de l'utilisateur valideur CDC - Chef de Chantier
    /// </summary>
    public int? ValideurCDCId { get; set; }

    /// <summary>
    ///  Obtient ou définit l'entité de l'utilisateur valideur CDC - Chef de Chantier
    /// </summary>
    public UtilisateurModel ValideurCDC { get; set; }

    /// <summary>
    ///  Obtient ou définit l'identifiant de l'utilisateur valideur CDT - Conducteur de Travaux
    /// </summary>
    public int? ValideurCDTId { get; set; }

    /// <summary>
    ///  Obtient ou définit l'entité de l'utilisateur valideur CDT - Conducteur de Travaux
    /// </summary>
    public UtilisateurModel ValideurCDT { get; set; }

    /// <summary>
    ///  Obtient ou définit l'identifiant de l'utilisateur valideur DRC - Directeur de Chantier
    /// </summary>
    public int? ValideurDRCId { get; set; }

    /// <summary>
    ///  Obtient ou définit l'entité de l'utilisateur valideur DRC - Directeur de Chantier
    /// </summary>
    public UtilisateurModel ValideurDRC { get; set; }

    /// <summary>
    ///  Obtient ou définit l'identifiant de l'utilisateur valideur GSP - Correspondant Paie
    /// </summary>
    public int? ValideurGSPId { get; set; }

    /// <summary>
    ///  Obtient ou définit l'entité de l'utilisateur valideur GSP - Correspondant Paie
    /// </summary>
    public UtilisateurModel ValideurGSP { get; set; }

    /// <summary>
    ///  Obtient ou définit les date et heure de la validation CDC - Chef de Chantier
    /// </summary>
    public DateTime? DateValidationCDC { get; set; }

    /// <summary>
    ///  Obtient ou définit les date et heure de la validation CDT - Conducteur de Travaux
    /// </summary>
    public DateTime? DateValidationCDT { get; set; }

    /// <summary>
    ///  Obtient ou définit les date et heure de la validation DRC - Directeur de Chantier
    /// </summary>
    public DateTime? DateValidationDRC { get; set; }

    /// <summary>
    ///  Obtient ou définit les date et heure de la validation GSP - Correspondant Paie
    /// </summary>
    public DateTime? DateValidationGSP { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si si le rapport peut être supprimé
    /// </summary>
    public bool CanBeDeleted { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si si le rapport peut être validé
    /// </summary>
    public bool CanBeValidated { get; set; }

    /// <summary>
    ///   Obtient ou définit une valeur indiquant si le rapport est clôturé
    /// </summary>
    public bool Cloture { get; set; }

    /// <summary>
    ///   Obtient ou définit une valeur indiquant si le rapport est verrouillé
    /// </summary>
    public bool Verrouille { get; set; }

    /// <summary>
    /// Obtient ou déinit une valeur indiquant si le rapport a étét supprimé
    /// </summary>
    public bool IsDeleted { get; set; }

    /// <summary>
    /// Obtient ou définit le dernier valideur
    /// </summary>
    public UtilisateurModel LastValideur { get; set; }

    /// <summary>
    /// Obtient ou définit la liste des erreurs du rapport
    /// </summary>
    public string[] ListErreurs { get; set; }

    /// <summary>
    /// Obtient ou définit le type de rapport.
    /// </summary>
    public TypeRapportEnum TypeRapport { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si le pointage a été généré.
    /// </summary>
    public bool IsGenerated { get; set; }

    /// <summary>
    /// Obtient ou définit une valeur indiquant si le rapport est sélectionné dans la liste des rapports
    /// </summary>
    public bool IsSelected { get; set; }
  }
}