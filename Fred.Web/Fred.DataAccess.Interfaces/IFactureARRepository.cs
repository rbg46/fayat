
using System.Collections.Generic;
using Fred.Entities.Facture;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    ///   Représente un référentiel de données pour les factures AR.
    /// </summary>
    public interface IFactureArRepository : IRepository<FactureEnt>
    {
        #region Facture entête

        /// <summary>
        ///   Retourne la liste de toute les factures a rapprocher
        /// </summary>
        /// <returns>Une liste de facture a rapprocher triée par date de facture</returns>
        IEnumerable<FactureEnt> GetAllFactureAr();

        /// <summary>
        ///   Retourne la liste des factures filtrée selon les critères de recherche.
        /// </summary>
        /// <param name="userCiIdList">Liste d'identifiant de CI autorisé pour l'utilisateur courant</param>
        /// <param name="filter">Filtre de recherche</param>
        /// <param name="page">Page courante</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <returns>La liste des factures filtrées selon les critères de recherche et ordonnées selon les critères de tri</returns>
        IEnumerable<FactureEnt> SearchFactureListWithFilter(List<int> userCiIdList, SearchFactureEnt filter, int page, int pageSize = 20);

        /// <summary>
        ///   Retourne la liste des factures AR pour un id de societe passé en parametre
        /// </summary>
        /// <param name="societeId">Identifiant unique de la societe</param>
        /// <returns>Une liste de facture AR triée par date de cloture</returns>
        IEnumerable<FactureEnt> GetFactureArBySocieteId(int societeId);

        /// <summary>
        ///   Retourne une facture par son numero
        /// </summary>
        /// <param name="noFacture">Numero de la facture</param>
        /// <param name="societeId">Identifiant unique de la societe</param>
        /// <returns>Une facture AR</returns>
        FactureEnt GetFactureArByNumero(string noFacture, int societeId);

        /// <summary>
        ///   Retourne la liste des facture pour un code de societe passé en parametre
        /// </summary>
        /// <param name="societeId">Identifiant unique de la societe</param>
        /// <returns>Une liste de facture AR triée par date de cloture</returns>
        IEnumerable<FactureEnt> GetFactureArBySocieteCode(string societeId);

        /// <summary>
        ///   Insertion en base d'une facture à rapprocher
        /// </summary>
        /// <param name="facture">La facture AR à enregistrer</param>
        /// <returns>Retourne l'identifiant unique de la facture AR</returns>
        int Add(FactureEnt facture);

        /// <summary>
        ///   Mise à jour d'une facture
        /// </summary>
        /// <param name="facture">La facture à mettre à jour</param>
        /// <returns>Facture mise à jour</returns>
        FactureEnt UpdateFacture(FactureEnt facture);

        #endregion

        #region Lignes

        /// <summary>
        ///   Retourne la liste de tout les journaux
        /// </summary>
        /// <returns>Une liste de journal triée par date de cloture</returns>
        IEnumerable<FactureLigneEnt> GetAllFactureLigneAr();

        /// <summary>
        ///   Retourne la liste des lignes de facture AR pour un id de facture passé en parametre
        /// </summary>
        /// <param name="factureId">Identifiant unique de la facture AR</param>
        /// <returns>Une liste de ligne facture AR triée par id</returns>
        IEnumerable<FactureLigneEnt> GetFactureLigneByFactureId(int factureId);

        /// <summary>
        ///   Insertion en base d'une ligne de facture AR
        /// </summary>
        /// <param name="factureLigne">La ligne de facture AR à enregistrer</param>
        /// <returns>Retourne l'identifiant unique de la ligne de facture AR</returns>
        int AddLigne(FactureLigneEnt factureLigne);

        #endregion
    }
}