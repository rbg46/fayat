using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Business.Depense;
using Fred.Business.ExplorateurDepense;
using Fred.Business.ExplorateurDepense.Models;
using Fred.Entities.Depense;
using Fred.Entities.Personnel;
using Fred.Entities.Rapport;
using Fred.Entities.ReferentielEtendu;
using Fred.Entities.Societe;
using Fred.Entities.Valorisation;
using Fred.Web.Shared.Models.Valorisation;

namespace Fred.Business.Valorisation
{
    /// <summary>
    /// Interface de Valorisation
    /// </summary>
    public interface IValorisationManager : IManager<ValorisationEnt>
    {

        /// <summary>
        /// Déverrouille l'ensemble des lignes de valorisation correspondantes au Ci et à la période en paramètre
        /// </summary>
        /// <param name="ciId">Identifiant du Ci</param>
        /// <param name="annee">Année</param>
        /// <param name="mois">Mois</param>
        /// <param name="verrou">Indique si on verrouille ou déverrouille</param>
        void UpdateVerrouPeriodeValorisation(int ciId, int annee, int mois, bool verrou);

        void UpdateVerrouPeriodeValorisation(List<int> ciIds, int annee, int mois, bool verrou);

        /// <summary>
        /// Retourne la liste des valorisations associées à un rapport
        /// </summary>
        /// <param name="rapportId">identifiant du rapport</param>
        /// <returns>valorisations</returns>
        IList<ValorisationEnt> GetValorisationByRapport(int rapportId);

        /// <summary>
        /// Crée des lignes de valorisations à partir de lignes de pointages
        /// </summary>
        /// <param name="listPointages">Une liste de pointages</param>
        /// <param name="source">Source d'execution de la valorisation</param>
        /// <param name="periode">période où le barème est mis à jour sinon null</param>
        void CreateValorisation(ICollection<RapportLigneEnt> listPointages, string source, DateTime? periode = null);

        /// <summary>
        /// Insère des lignes de valorisation en base
        /// </summary>
        /// <param name="pointage">Pointage de référence</param>
        /// <param name="source">Source d'execution de la valorisation</param>
        /// <param name="periode">période où le barème est mis à jour sinon null</param>
        void InsertValorisationFromPointage(RapportLigneEnt pointage, string source, DateTime? periode = null);

        /// <summary>
        /// Met à jour des lignes de valorisation en base
        /// </summary>
        /// <param name="pointage">Pointage de référence</param>
        void UpdateValorisationFromPointage(RapportLigneEnt pointage);

        /// <summary>
        /// Retourne des lignes de valorisations
        /// </summary>
        /// <param name="ciId">Identifiant de ci</param>
        /// <param name="periode">Période</param>
        /// <returns>Liste de valorisations</returns>
        IEnumerable<ValorisationEnt> GetByCiAndPeriod(int ciId, DateTime periode);

        /// <summary>
        /// Met à jour des lignes de valo depuis une liste barèmes
        /// </summary>
        /// <param name="personnelId">Identifiant du personnel</param>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        /// <param name="periode">Période toujours null dans cette méthode</param>
        void UpdateValorisationFromPersonnel(int personnelId, int userId, DateTime? periode = null);

        /// <summary>
        /// Met à jour des lignes de valo depuis une liste barèmes
        /// </summary>
        /// <param name="materielId">Identifiant du materiel</param>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        /// <param name="periode">Période toujours null dans cette méthode</param>
        void UpdateValorisationFromMateriel(int materielId, int userId, DateTime? periode = null);

        /// <summary>
        /// Supprime des lignes de valorisation en base
        /// </summary>
        /// <param name="pointage">Pointage de référence</param>
        void DeleteValorisationFromPointage(RapportLigneEnt pointage);

        /// <summary>
        /// Met à jour des lignes de valo depuis une liste barèmes
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        /// <param name="periode">Période comptable</param>
        void UpdateValorisationFromListBareme(int ciId, int userId, DateTime? periode = null);

        /// <summary>
        /// Met à jour une ligne de valorisation avec le groupe
        /// </summary>
        /// <param name="valorisation">Identifiant du CI</param>
        void UpdateValorisationGroupeRemplacement(ValorisationEnt valorisation);

        /// <summary>
        /// Met à jour des lignes de valo depuis une liste barèmes Strom
        /// </summary>
        /// <param name="orgaId">Identifiant de l'organisation</param>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        /// <param name="periode">Période comptable</param>
        void UpdateValorisationFromListBaremeStrom(int orgaId, int userId, DateTime? periode = null);

        /// <summary>
        /// Execute une procédure avec les paramètres
        /// </summary>
        /// <param name="objectId">Identifiant de l'objet appelant</param>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        /// <param name="procedureToExecute">Procédure à executé</param>
        /// <param name="periode">Période optionnelle</param>
        void NewValorisationJob(int objectId, int userId, Action<int, int, DateTime?> procedureToExecute, DateTime? periode = null);

        /// <summary>
        /// Renvoi le calcul d'un total de valorisation pour un CI et une période
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="dateDebut">Date de début de sélection</param>
        /// <param name="dateFin">Date de fin de sélection</param>
        /// <returns>Retourne le calcul d'un total de valorisation</returns>
        decimal Total(int ciId, DateTime dateDebut, DateTime dateFin);

        /// <summary>
        /// Renvoi le calcul d'un total de valorisation pour un CI et une période
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="dateDebut">Date de début de sélection</param>
        /// <param name="dateFin">Date de fin de sélection</param>
        /// <param name="ressourceId">Identifiant d'une ressource de sélection</param>
        /// <param name="tacheId">Identifiant d'une tache de sélection</param>
        /// <returns>Retourne le calcul d'un total de valorisation</returns>
        decimal Total(int ciId, DateTime dateDebut, DateTime dateFin, int? ressourceId, int? tacheId);

        /// <summary>
        /// Renvoi le total de valorisation pour un chapitre
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="dateDebut">Date de début de sélection</param>
        /// <param name="dateFin">Date de fin de sélection</param>
        /// <param name="chapitreId">Identifiant d'un chapitre de sélection</param>
        /// <returns>Retourne le calcul d'un total de valorisation</returns>
        decimal TotalByChapitreId(int ciId, DateTime dateDebut, DateTime dateFin, int chapitreId);

        /// <summary>
        /// Renvoi le total de valorisation pour une liste de chapitre codes
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="dateDebut">Date de début de sélection</param>
        /// <param name="dateFin">Date de fin de sélection</param>
        /// <param name="chapitreCodes">liste de chapitre codes</param>
        /// <returns>Retourne le calcul d'un total de valorisation</returns>
        decimal TotalByChapitreCodes(int ciId, DateTime dateDebut, DateTime dateFin, List<string> chapitreCodes);

        /// <summary>
        /// Récupération des valorisations (explorateur des dépenses)
        /// </summary>
        /// <param name="filtre">filtre de l'explorateur de dépenses</param>
        /// <returns>Liste de valorisations</returns>
        Task<IEnumerable<ValorisationEnt>> GetValorisationListAsync(SearchExplorateurDepense filtre);

        /// <summary>
        /// Récupération des valorisations
        /// </summary>
        /// <param name="filtre">filtre de dépenses</param>
        /// <returns>Liste de valorisations</returns>
        Task<IEnumerable<ValorisationEnt>> GetValorisationListAsync(SearchDepense filtre);

        Task<List<ValorisationEnt>> GetValorisationListAsync(int ciId, MainOeuvreAndMaterielsFilter mainOeuvreAndMaterielsFilter);

        /// <summary>
        /// Récupération de la valorisation (explorateur des dépenses)
        /// </summary>
        /// <param name="groupRemplacementId">Identifiant du groupe de remplacement</param>
        /// <returns>Liste de valorisations</returns>
        ValorisationEnt GetByGroupRemplacementId(int groupRemplacementId);

        /// <summary>
        /// Récupère une valo
        /// </summary>
        /// <param name="valoId">Id valorisation</param>
        /// <returns>Valorisation</returns>
        ValorisationEnt GetValorisationById(int valoId);

        /// <summary>
        /// Retourne la liste des valorisations selon les paramètres
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="ressourceId">Identifiant de la ressource</param>    
        /// <param name="periodeDebut">Date periode début</param>
        /// <param name="periodeFin">Date periode fin</param>
        /// <param name="deviseId">Identifiant devise</param>
        /// <returns>Liste de dépense achat</returns>
        Task<IEnumerable<ValorisationEnt>> GetValorisationListAsync(int ciId, int? ressourceId, DateTime? periodeDebut, DateTime? periodeFin, int? deviseId);

        /// <summary>
        /// Retourne la liste des valorisations sans reception intérimaire
        /// </summary>
        /// <param name="ciIdList">Liste d'Iidentifiants de CI</param>
        /// <param name="datedebut">Date periode début</param>
        /// <param name="dateFin">Date periode fin</param>
        /// <returns>Liste de <see cref="ValorisationEnt"/></returns>
        Task<IEnumerable<ValorisationEnt>> GetValorisationsWithoutReceptionInterimaireAsync(List<int> ciIdList, DateTime datedebut, DateTime dateFin);

        /// <summary>
        /// Retourne la liste des valorisations pour une liste d'identifiant de rapport de ligne
        /// </summary>
        /// <param name="rapportLigneIds">Liste de rapport de ligne</param>
        /// <returns><see cref="ValorisationEcritureComptableODModel"/></returns>
        IReadOnlyList<ValorisationEcritureComptableODModel> GetValorisationByRapportLigneIds(List<int> rapportLigneIds);

        /// <summary>
        /// Ajoute une valorisation négative suite à la réception intérimaire
        /// </summary>
        /// <param name="firstPointage">premier pointage concenant la reception intérimaire</param>
        /// <param name="receptionInterimaire">réception intérimaire</param>
        void InsertValorisationNegativeForInterimaire(RapportLigneEnt firstPointage, DepenseAchatEnt receptionInterimaire);

        /// <summary>
        /// Ajoute une valorisation négative suite à la réception materiel externe
        /// </summary>
        /// <param name="firstPointage">premier pointage concenant la reception materiel externe</param>
        /// <param name="receptionMaterielExterne">réception materiel externe</param>
        void InsertValorisationNegativeForMaterielExterne(RapportLigneEnt firstPointage, DepenseAchatEnt receptionMaterielExterne);

        /// <summary>
        /// mise à jour du prix et du montant
        /// </summary>
        /// <param name="idRapportLignes">id repport ligne </param>
        /// <param name="valo"> la nouvelle valorisation</param>
        void UpdateValorisationMontant(List<int> idRapportLignes, decimal valo);

        bool GetVerrouPeriodeTrueValorisation(int rapportId, int rapportLigneId);

        ValorisationMaterielModel GetValorisationMaterielByMaterielId(int materielId);

        int GetSocieteId(SocieteEnt societe, ValorisationMaterielModel materiel, PersonnelEnt personnel);

        int GetSocieteIdByPointage(RapportLigneEnt pointage, ValorisationMaterielModel materiel, PersonnelEnt personnel);

        void InsertValorisationPersonnel(RapportLigneEnt pointage, RapportLigneTacheEnt pointageTache, string source, DateTime? periode = null);

        ValorisationEnt NewValorisationMateriel(RapportLigneEnt pointage, RapportLigneTacheEnt pointageTache, ReferentielEtenduEnt referentielEtendu, ValorisationMaterielModel materiel, string source, DateTime? periode = null);
        void UpdateValorisationFromListBaremeStorm(int orgaId, DateTime periode, List<int> ressourcesIds);

        Task<IEnumerable<ValorisationEnt>> GetValorisationsAsync(IEnumerable<int> ciIdList, DateTime datedebut, DateTime dateFin);
    }
}