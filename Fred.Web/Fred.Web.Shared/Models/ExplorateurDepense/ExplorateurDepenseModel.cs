using Fred.Web.Models.Referential;
using Fred.Web.Models.ReferentielFixe;
using System;

namespace Fred.Web.Models
{
  /// <summary>
  ///   Model Explorateur depense
  /// </summary>
  public class ExplorateurDepenseModel
  {
    /// <summary>
    ///   Obtient ou définit l'identifiant unique de la dépense
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant de la ressource
    /// </summary>
    public int RessourceId { get; set; }

    /// <summary>
    ///   Obtient ou définit la ressource
    /// </summary>
    public RessourceModel Ressource { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant de la tâche
    /// </summary>
    public int TacheId { get; set; }

    /// <summary>
    ///   Obtient ou définit la tache
    /// </summary>
    public TacheModel Tache { get; set; }

    /// <summary>
    ///   Obtient ou définit le libelle1
    /// </summary>
    public string Libelle1 { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant de l'unité
    /// </summary>
    public int UniteId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'unité
    /// </summary>
    public UniteModel Unite { get; set; }

    /// <summary>
    ///   Obtient ou définit la quantité
    /// </summary>
    public decimal? Quantite { get; set; }

    /// <summary>
    ///   Obtient ou définit le prix unitaire hors taxe
    /// </summary>
    public decimal? PUHT { get; set; }

    /// <summary>
    ///   Obtient ou définit le montant hors taxe
    /// </summary>
    public decimal MontantHT { get; set; }

    /// <summary>
    ///   Obtient ou définit le code
    /// </summary>
    public string Code { get; set; }

    /// <summary>
    ///   Obtient ou définit le libellé 2
    /// </summary>
    public string Libelle2 { get; set; }

    /// <summary>
    ///   Obtient ou définit le commentaire
    /// </summary>
    public string Commentaire { get; set; }

    /// <summary>
    ///   Obtient ou définit la date de la dépense
    /// </summary>
    public DateTime DateDepense { get; set; }

    /// <summary>
    ///   Obtient ou définit la date comptable de remplacement de la dépense
    /// </summary>
    public DateTime DateComptableRemplacement { get; set; }

    /// <summary>
    ///   Obtient ou définit la période
    /// </summary>
    public DateTime Periode { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant de la nature
    /// </summary>
    public int NatureId { get; set; }

    /// <summary>
    ///   Obtient ou définit la nature
    /// </summary>
    public NatureModel Nature { get; set; }

    /// <summary>
    ///   Obtient ou définit le type de dépense ["OD", "Valorisation", "Reception", "Facture"]
    /// </summary>
    public string TypeDepense { get; set; }

    /// <summary>
    ///   Obtient ou définit le type de sous dépense
    /// </summary>
    public string SousTypeDepense { get; set; }

    /// <summary>
    ///   Obtient ou définit la Date de Rapprochement
    /// </summary>
    public DateTime? DateRapprochement { get; set; }

    /// <summary>
    ///   Obtient ou définit la date facture (ou date opération pour une Réception)
    /// </summary>
    public DateTime? DateFacture { get; set; }

    /// <summary>
    ///   Obtient ou définit le numéro de facture
    /// </summary>
    public string NumeroFacture { get; set; }

    /// <summary>
    ///   Obtient ou définit le montant de la facture
    /// </summary>
    public decimal? MontantFacture { get; set; }

    /// <summary>
    ///   Obtient ou définit l'identifiant de la commande
    /// </summary>
    public int? CommandeId { get; set; }

    /// <summary>
    ///   Obtient ou définit si la tâche est remplaçable
    /// </summary>
    public bool TacheRemplacable { get; set; }

    /// <summary>
    ///   Obtient ou définit l'Id de la dépense (Dép / Valo / OD)
    /// </summary>
    public int DepenseId { get; set; }

    /// <summary>
    ///   Obtient ou définit l'Id du groupe de remplacement des tâches
    /// </summary>
    public int GroupeRemplacementTacheId { get; set; }

    /// <summary>
    /// Obtient ou définit le code et libellé de la Tâche d'origine si elle a été remplacée dans la dépense
    /// </summary>
    public string TacheOrigineCodeLibelle { get; set; }
  }
}
