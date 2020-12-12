using System.Collections.Generic;
using Fred.Entities.Facture;
using Fred.Entities.Societe;

namespace Fred.Business.Facture
{
  /// <summary>
  ///   Gestionnaire des factures.
  /// </summary>
  public interface IFactureManager : IManager<FactureEnt>
  {
    #region Facture entête

    /// <summary>
    ///   Retourne une nouvelle instance de facture AR.
    /// </summary>
    /// <returns>Retourne un nouveau favori.</returns>
    FactureEnt GetNewFactureAR();

    /// <summary>
    ///   Retourne une facture
    /// </summary>
    /// <param name="factureId">Identifiant de la facture</param>
    /// <returns>Retourne une Facture</returns>
    FactureEnt GetFactureById(int factureId);

    /// <summary>
    ///   Retourne la liste des factures AR
    /// </summary>
    /// <returns>Liste des favoris.</returns>
    IEnumerable<FactureEnt> GetFactureARList();

    /// <summary>
    ///   Retourne la liste des factures d'une societe
    /// </summary>
    /// <param name="societeId">Id de la societe</param>
    /// <returns>Renvoie la liste des facture AR par id societe passé en parametre</returns>
    IEnumerable<FactureEnt> GetFactureARList(int societeId);

    /// <summary>
    ///   Retourne la liste des factures filtrée, triée (avec pagination)
    /// </summary>
    /// <param name="filters">Objet de recherche et de tri des factures</param>
    /// <param name="page">Numéro de page</param>
    /// <param name="pageSize">Taille de la page</param>
    /// <returns>Retourne la liste des factures filtrées, triées et paginées</returns>
    IEnumerable<FactureEnt> SearchFactureListWithFilterForRapprochement(SearchFactureEnt filters, int page, int pageSize);

    /// <summary>
    ///   Retourne une facture par son numero
    /// </summary>
    /// <param name="noFacture">Numero de la facture</param>
    /// <param name="societeId">Identifiant unique de la societe</param>
    /// <returns>Renvoie une facture par le numero passé en parametre</returns>
    FactureEnt GetFactureARByNumero(string noFacture, int societeId);

    /// <summary>
    ///   Méthode d'enregistrement d'une facture
    /// </summary>
    /// <param name="factureAr"> La facture à enregistrer </param>
    /// <returns> Identifiant de la facture enregistrer </returns>
    int Add(FactureEnt factureAr);

    /// <summary>
    ///   Mise à jour d'une facture
    /// </summary>
    /// <param name="facture">La facture à mettre à jour</param>
    /// <returns>Le facture mise à jour</returns>
    FactureEnt Update(FactureEnt facture);

    /// <summary>
    ///   Vérificartion des règles de gestion avant enregistrement
    /// </summary>
    /// <param name="factureAr">Représente la facture à enregistrer</param>
    /// <returns>True : Facture valide pour enregistrement, False : Facture non valide pour enregistrement</returns>
    bool IsValid(FactureEnt factureAr);

    /// <summary>
    ///   RG_645_001 - Vérification de l'unicité de la facture pour une societe
    /// </summary>
    /// <param name="factureAr">Facture à vérifier</param>
    /// <returns>True : la facture n'existe pas, False : la facture existe</returns>
    bool IsUnique(FactureEnt factureAr);

    /// <summary>
    ///   RG_645_003 - Vérifie que la facture fait partie d'un journal à importer pour la societe
    /// </summary>
    /// <param name="factureAR"> Facture à vérifier </param>
    /// <returns> True : la facture fait partie d'un journal, False : la facture ne fait pas partie d'un journal </returns>
    bool IsExistJournal(FactureEnt factureAR);

    /// <summary>
    ///   Récupération de la societe à partir d'un objet facture AR
    /// </summary>
    /// <param name="factureAr">Représente une facture à rapprocher</param>
    /// <returns>Retourne la société de facturation</returns>
    SocieteEnt GetSociete(FactureEnt factureAr);

    /// <summary>
    ///   Vérifie la validité et enregistre les facture importés depuis ANAËL Finances
    /// </summary>
    /// <param name="factureEnt">Liste des entités dont il faut vérifier la validité</param>
    /// <returns>Retourne vrai si la facture peut être importé</returns>
    bool ManageImportedFacture(FactureEnt factureEnt);

    /// <summary>
    ///   Initialise une nouvelle instance de la classe de recherche des factures
    /// </summary>
    /// <returns>Objet de filtrage + tri des factures initialisé</returns>
    SearchFactureEnt GetFilter();

    #endregion

    #region Lignes

    /// <summary>
    ///   Insertion en base d'une ligne de facture AR
    /// </summary>
    /// <param name="factureLigne">La ligne de facture AR à enregistrer</param>
    /// <returns>Retourne l'identifiant unique de la ligne de facture AR</returns>
    int AddLigne(FactureLigneEnt factureLigne);

    /// <summary>
    ///   Retourne une nouvelle liste de factures AR
    /// </summary>
    /// <returns>Liste des favoris.</returns>
    IEnumerable<FactureLigneEnt> GetNewLigneFactureARList();

    #endregion
  }
}