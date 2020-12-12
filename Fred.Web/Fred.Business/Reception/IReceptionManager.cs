using Fred.Business.DepenseGlobale;
using Fred.Entities.Depense;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Fred.Business.Reception
{
    /// <summary>
    /// IReceptionManager
    /// </summary>
    public interface IReceptionManager : IManager<DepenseAchatEnt>
    {
        /// <summary>
        /// Récupération d'un filtre pour les réceptions
        /// </summary>
        /// <returns>Filtre</returns>
        SearchDepenseEnt GetNewFilter();

        /// <summary>
        /// Création d'une nouvelle réception en fonction d'une ligne de commande
        /// </summary>
        /// <param name="commandeLigneId">Identifiant de la ligne de commande</param>
        /// <returns>Réception créée</returns>
        DepenseAchatEnt GetNew(int commandeLigneId);

        /// <summary>
        /// Suppression d'une réception
        /// </summary>
        /// <param name="receptionId">Identifiant de la Reception à supprimer</param>    
        /// <returns>Reception supprimée</returns>
        void Delete(int receptionId);

        /// <summary>
        /// Duplication d'une réception
        /// </summary>
        /// <param name="commandeLigneId">Identifiant de la ligne de commande courante</param>
        /// <param name="receptionId">Identifiant de la Réception courante</param>
        /// <returns>Réception dupliquée</returns>
        DepenseAchatEnt Duplicate(int commandeLigneId, int receptionId);

        /// <summary>
        /// Permet l'idenfiant du job Hangfire pour une commande.
        /// </summary>
        /// <param name="receptionIds">La liste d'identifant des réceptions.</param>
        /// <param name="hangfireJobId">L'identifant du job Hanfire.</param>
        void UpdateHangfireJobId(IEnumerable<int> receptionIds, string hangfireJobId);

        /// <summary>
        ///   Récupération d'une réception en fonction de son identifiant
        /// </summary>
        /// <param name="receptionId">Identifiant de la réception</param>
        /// <returns>Réception trouvée</returns>
        DepenseAchatEnt GetReception(int receptionId);

        /// <summary>
        ///   Récupération des lignes rapports  contenant des intérimaires non récéptionnées 
        /// </summary>
        /// <param name="listeCiId">liste d'identifiant unique de ci</param>
        /// <param name="utilisateurId">uilisateur qui a lancer le job</param>
        void ReceptionInterimaire(List<int> listeCiId, int utilisateurId);

        /// <summary>
        /// Récupération des lignes rapports  contenant des matériels externe non récéptionnées 
        /// </summary>
        /// <param name="societeId">identifiant unique de la societe</param>
        void ReceptionMaterielExterne(int societeId);

        /// <summary>
        /// Retourne la liste des dépenses selon les paramètres
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="ressourceId">Identifiant de la ressource</param>    
        /// <param name="periodeDebut">Date periode début</param>
        /// <param name="periodeFin">Date periode fin</param>
        /// <param name="deviseId">Identifiant devise</param>
        /// <returns>Liste de dépense achat</returns>
        IEnumerable<DepenseAchatEnt> GetReceptions(int ciId, int? ressourceId, DateTime? periodeDebut, DateTime? periodeFin, int? deviseId);

        /// <summary>
        /// Retourne la liste des dépenses selon les paramètres
        /// </summary>
        /// <param name="filtres">Liste de <see cref="DepenseGlobaleFiltre"/></param>
        /// <returns>Liste de <see cref="DepenseAchatEnt"/></returns>
        Task<IEnumerable<DepenseAchatEnt>> GetReceptionsAsync(List<DepenseGlobaleFiltre> filtres);

        /// <summary>
        /// Retourne la liste des réceptions selon les paramètres
        /// </summary>
        /// <param name="ciIdList">liste d'identifiants de CIs</param>
        /// <param name="tacheIdList">liste d'identifiants de taches</param>    
        /// <param name="dateDebut">Date periode début</param>
        /// <param name="dateFin">Date periode fin</param>
        /// <param name="deviseId">Identifiant devise</param>
        /// <param name="includeProperties">include des navigation properties</param>
        /// <returns>Liste de dépense de type réception</returns>
        Task<IEnumerable<DepenseAchatEnt>> GetReceptionsAsync(List<int> ciIdList, List<int> tacheIdList, DateTime? dateDebut = null, DateTime? dateFin = null, int? deviseId = null, bool includeProperties = false);

        /// <summary>
        /// Récupère les identifiant unique des réceptions intérimaire qui n'ont pas été envoyé à SAP
        /// </summary>
        /// <param name="listeCiId">liste d'identifiant unique de ci</param>
        /// <returns>Réception</returns>
        IEnumerable<int> GetReceptionInterimaireToSend(List<int> listeCiId);

        /// <summary>
        /// Récupère les identifiant unique des réceptions materiel externe qui n'ont pas été envoyé à SAP
        /// </summary>
        /// <returns>Réception</returns>
        IEnumerable<int> GetReceptionMaterielExterneToSend();

        /// <summary>
        ///  Mise à jour d'une liste de réceptions sans passer par la validation 
        /// </summary>
        /// <param name="receptions">Liste de réceptions à mettre à jour</param>
        /// <param name="auteurModificationId">Identifaint de l'utilisateur effectuant la modification</param>
        void UpdateAndSaveWithoutValidation(IEnumerable<DepenseAchatEnt> receptions, int auteurModificationId);

        /// <summary>
        ///  Mise à jour d'une liste de réceptions sans passer par la validation 
        /// </summary>
        /// <param name="reception">Réception à mettre à jour</param>
        /// <param name="auteurModificationId">Identifaint de l'utilisateur effectuant la modification</param>
        void UpdateAndSaveWithoutValidation(DepenseAchatEnt reception, int auteurModificationId);

        /// <summary>
        ///  Mise à jour d'une liste de réceptions
        /// </summary>
        /// <param name="receptions">Liste de réceptions à mettre à jour</param>   
        void UpdateAndSaveWithValidation(List<DepenseAchatEnt> receptions);

        /// <summary>
        /// Retourner la requête de récupération des dépenses
        /// </summary>
        /// <param name="filters">Les filtres.</param>
        /// <param name="orderBy">Les tris</param>
        /// <param name="includeProperties">Les propriétés à inclure.</param>
        /// <param name="page">La page.</param>
        /// <param name="pageSize">Taille de la page.</param>
        /// <param name="asNoTracking">asNoTracking</param>
        /// <returns>Une requête</returns>
        List<DepenseAchatEnt> Get(List<Expression<Func<DepenseAchatEnt, bool>>> filters,
                                                Func<IQueryable<DepenseAchatEnt>, IOrderedQueryable<DepenseAchatEnt>> orderBy = null,
                                                List<Expression<Func<DepenseAchatEnt, object>>> includeProperties = null,
                                                int? page = null,
                                                int? pageSize = null,
                                                bool asNoTracking = true);

        /// <summary>
        ///  Mise à jour d'une réception
        /// </summary>
        /// <param name="reception">Liste de réceptions à mettre à jour</param>  
        void UpdateAndSaveWithValidation(DepenseAchatEnt reception);

        /// <summary>
        /// Permet de viser une liste de réceptions. Cad de signée la reception avec l'utilisateur courrant
        /// </summary>
        /// <param name="receptionIds">les ids des receptions a visées </param>
        /// <param name="callFredIeAndSetHangfireJobIdAction">callFredIeAndSetHangfireJobIdAction</param>      
        Task ViserReceptionsAsync(List<int> receptionIds, Func<List<DepenseAchatEnt>, Task> callFredIeAndSetHangfireJobIdAction);

        /// <summary>
        /// Détermine s'il y a au moins une réception dont la date correspond est comprise dans une période bloquée en réception
        /// </summary>
        /// <param name="receptionIds">Liste des identifiants des réceptions</param>
        /// <returns>Vrai s'il existe une réception bloquée, sinon faux</returns>
        bool CheckAnyReceptionsIsBlocked(List<int> receptionIds);


        /// <summary>
        /// Retourne le total du montant receptionné pour une liste de ligne de commandes
        /// </summary>
        /// <param name="commandeLigneIds">Identifiant des lignes de commande</param>
        /// <returns>PUHT * Qte des receptions des lignes de commande </returns>
        decimal GetMontant(List<int> commandeLigneIds);
        DepenseAchatEnt AddAndValidate(DepenseAchatEnt reception);
    }
}
