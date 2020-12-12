using System;
using System.Collections.Generic;
using Fred.Entities.Rapport;
using Fred.Entities.Rapport.Search;
using Fred.Entities.Referential;
using Fred.Web.Shared.Models.PointagePersonnel;

namespace Fred.Business.Rapport
{
    /// <summary>
    /// Fonctionnalité Create Read Update Delete des indemnités de déplacement
    /// </summary>
    public interface ICrudFeature
    {
        /// <summary>
        /// Permet de mettre à jour ou d'ajouter un rapport
        /// </summary>
        /// <param name="rapport">Un rapport</param>
        void AddOrUpdateRapport(RapportEnt rapport);

        /// <summary>
        ///   Méthode d'ajout d'un rapport
        /// </summary>
        /// <param name="rapport">Rapport à ajouter</param>
        /// <returns>Retourne le rapport nouvellement créée</returns>
        RapportEnt AddRapport(RapportEnt rapport);

        /// <summary>
        /// Méthode d'ajout d'un rapport depuis la gestion de moyens avec TypeStatutRapport = 99
        /// </summary>
        /// <param name="rapport"></param>
        /// <returns></returns>
        RapportEnt AddRapportMaterialType(RapportEnt rapport);

        /// <summary>
        ///   Ajoute une ligne dans le rapport
        /// </summary>
        /// <param name="rapport">Le rapport</param>
        /// <returns>Un rapport</returns>
        RapportEnt AddNewPointageReelToRapport(RapportEnt rapport);

        /// <summary>
        ///   Ajoute une prime au paramétrage du rapport et à toutes les lignes de rapport
        /// </summary>
        /// <param name="rapport">Le rapport</param>
        /// <param name="prime">La prime</param>
        /// <returns>Un rapport</returns>
        RapportEnt AddPrimeToRapport(RapportEnt rapport, PrimeEnt prime);

        /// <summary>
        ///   Ajoute une tache au paramétrage du rapport et à toutes les lignes de rapport
        /// </summary>
        /// <param name="rapport">Le rapport</param>
        /// <param name="tache">La tache</param>
        /// <returns>Un rapport</returns>
        RapportEnt AddTacheToRapport(RapportEnt rapport, TacheEnt tache);

        /// <summary>
        /// Vérifie les écarts éventuel de status entre l'état du rapport avant et apres update
        /// </summary>
        /// <param name="rapport">Le rapport envoyé pour mettre a jour la base</param>
        /// <param name="rapportBeforeUpdate">Le rapport avant la mise à jour (en base de données)</param>
        /// <exception>ValidationException si les statuts sont différents</exception>
        void CheckRapportStatutChangedInDb(RapportEnt rapport, RapportEnt rapportBeforeUpdate);

        /// <summary>
        /// Vérifie les mises à jour de Materiel en comparant les lignes de rapport avant et apres update
        /// </summary>
        /// <param name="rapport">Le rapport mis à jour</param>
        /// <param name="rapportBeforeUpdate">Le rapport avant la mise à jour (en base de données)</param>
        /// <returns>un booleen indiquant si des modifications existent</returns>
        bool CheckRapportLignesMaterielChanged(RapportEnt rapport, RapportEnt rapportBeforeUpdate);

        /// <summary>
        /// Vérifie les mises à jour de Personnel en comparant les lignes de rapport avant et apres update
        /// </summary>
        /// <param name="rapport">Le rapport mis à jour</param>
        /// <param name="rapportBeforeUpdate">Le rapport avant la mise à jour (en base de données)</param>
        /// <returns>un booleen indiquant si des modifications existent</returns>
        bool CheckRapportLignesPersonnelChanged(RapportEnt rapport, RapportEnt rapportBeforeUpdate);

        /// <summary>
        ///   Méthode de mise à jour d'un rapport
        /// </summary>
        /// <param name="rapport">Rapport à mettre à jour</param>
        /// <returns>Retourne le rapport nouvellement créée</returns>
        RapportEnt UpdateRapport(RapportEnt rapport);

        /// <summary>
        ///   Supprimer un rapport
        /// </summary>
        /// <param name="rapport">Le rapport à supprimer</param>
        /// <param name="suppresseurId">Identifiant de l'utilisateur ayant supprimer le rapport</param>
        /// <param name="fromListeRapport">Indique si on supprime depuis la liste des rapports</param>
        void DeleteRapport(RapportEnt rapport, int suppresseurId, bool fromListeRapport = false);

        /// <summary>
        ///   Retourne la liste des Rapports.
        /// </summary>
        /// <returns>Liste des Rapports.</returns>
        IEnumerable<RapportEnt> GetRapportList();

        /// <summary>
        ///   liste rapport pour mobile
        /// </summary>
        /// <param name="sinceDate">The since date.</param>
        /// <param name="userId">Id utilisateur connecté.</param>
        /// <returns>Liste des rapport pour le mobile.</returns>
        IEnumerable<RapportEnt> GetRapportsMobile(DateTime? sinceDate = null, int? userId = null);

        /// <summary>
        ///   Retourne la liste des Rapports en fonction d'un utilisateur et d'une période
        /// </summary>
        /// <param name="utilisateurId">Identifiant de l'utilisateur</param> 
        /// <param name="periode">Période du rapport</param>
        /// <returns>Liste des Rapports</returns>
        IEnumerable<RapportEnt> GetRapportLightList(int utilisateurId, DateTime? periode);

        /// <summary>
        ///   Retourne le rapport portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="rapportId">Identifiant du rapport à retrouver.</param>
        /// <param name="forWebUse">Provenance du Web</param>
        /// <returns>le rapport retrouvé, sinon nulle.</returns>
        RapportEnt GetRapportById(int rapportId, bool forWebUse);

        /// <summary>
        ///   Retourne le rapport portant l'identifiant unique indiqué sans validation.
        /// </summary>
        /// <param name="rapportId">Identifiant du rapport à retrouver.</param>
        /// <returns>le rapport retrouvé, sinon nulle.</returns>
        RapportEnt GetRapportByIdWithoutValidation(int rapportId);

        /// <summary>
        ///   Retourne un rapport avec des données vides
        /// </summary>
        /// <param name="ciId">Identifiant d'un CI</param>
        /// <returns>Un rapport avec des données vides, ou avec le CI chargé</returns>
        RapportEnt GetNewRapport(int? ciId = null);

        /// <summary>
        ///   Applique les règles de gestion au pointage
        /// </summary>
        /// <param name="rapport">Un rapport</param>
        /// <param name="domaine">Le domaine à vérifier</param>
        /// <returns>Un pointage</returns>
        RapportEnt ApplyValuesRgRapport(RapportEnt rapport, string domaine);

        /// <summary>
        /// Verrouille le rapport
        /// </summary>
        /// <param name="rapport">rapport à verrouiller</param>
        /// <param name="valideurId">Identifiant de l'utilisateur</param>
        /// <returns>Le rapport s'il a été verrouillé, sinon Null</returns>
        RapportEnt VerrouillerRapport(RapportEnt rapport, int valideurId);

        /// <summary>
        /// Déverrouille le rapport
        /// </summary>
        /// <param name="rapport">rapport à déverrouiller</param>
        /// <param name="valideurId">Identifiant de l'utilisateur</param>
        void DeverrouillerRapport(RapportEnt rapport, int valideurId);

        /// <summary>
        ///   Verrouille une liste de rapport
        ///   Crée le lot de pointage de l'utilisateur 'valideurId' s'il n'existe pas (affecte un lotPointageId a chaque ligne du rapport)
        /// </summary>
        /// <param name="rapportIds">Liste de rapports</param>
        /// <param name="valideurId">Identifiant utilisateur du valideur</param>
        /// <param name="reportNotToLock">Rapports à ne pas vérouiller</param>
        /// <returns>La liste des rapports effectivement verrouillés</returns>
        LockRapportResponse VerrouillerListeRapport(IEnumerable<int> rapportIds, int valideurId, IEnumerable<int> reportNotToLock, SearchRapportEnt filter, string groupe);

        /// <summary>
        ///   Déverouille la liste des rapports
        /// </summary>
        /// <param name="rapportIds">Liste de rapport</param>
        /// <param name="valideurId">Identifiant utilisateur du valideur</param>
        void DeverrouillerListeRapport(IEnumerable<int> rapportIds, int valideurId);

        /// <summary>
        ///   Duplique un rapport sur une periode
        /// </summary>
        /// <param name="rapportId">L'id du rapport à dupliquer</param>
        /// <param name="startDate">date de depart de la duplication</param>
        /// <param name="endDate">date de fin de la duplication</param>
        /// <returns>DuplicateRapportResult</returns>
        DuplicateRapportResult DuplicateRapport(int rapportId, DateTime startDate, DateTime endDate);

        /// <summary>
        ///   Duplique un rapport
        /// </summary>
        /// <param name="rapport">Le rapport à dupliquer</param>
        /// <returns>Un rapport</returns>
        RapportEnt DuplicateRapport(RapportEnt rapport);

        /// <summary>
        ///   Duplique un rapport pour un autre ci
        /// </summary>
        /// <param name="rapport">Le rapport à dupliquer</param>
        /// <returns>Un rapport</returns>
        RapportEnt DuplicateRapportForNewCi(RapportEnt rapport);

        /// <summary>
        ///   Permet de valider un rapport
        /// </summary>
        /// <param name="rapport">Un rapport</param>
        /// <param name="valideurId">Identifiant du valideur de rapport</param>
        /// <returns>true en cas de succés, sinon false</returns>
        bool ValidationRapport(RapportEnt rapport, int valideurId);

        /// <summary>
        /// Ajoute, met à jour ou supprime les pointages de la liste
        /// </summary>
        /// <param name="listPointages">Liste des pointages à traîter</param>
        /// <param name="rapportsAdded">Liste des rapports ajoutés</param>
        /// <param name="rapportsUpdated">Liste des rapports à mettre à jour</param>
        /// <returns>Le résultat de l'enregistrement</returns>
        PointagePersonnelSaveResultModel SaveListPointagesPersonnel(IEnumerable<RapportLigneEnt> listPointages, out List<RapportEnt> rapportsAdded, out List<RapportEnt> rapportsUpdated);

        /// <summary>
        /// Ajoute, met à jour ou supprime les pointages de la liste lors d'une duplication
        /// </summary>
        /// <param name="listPointages">Liste des pointages à traîter</param>
        /// <param name="duplicatedPointageId">Id du pointage dupliqué</param>
        /// <param name="rapportsAdded">Liste des rapports ajoutés</param>
        /// <param name="rapportsUpdated">Liste des rapports à mettre à jour</param>
        /// <returns>Le résultat de l'enregistrement</returns>
        PointagePersonnelSaveResultModel SaveListDuplicatedPointagesPersonnel(IEnumerable<RapportLigneEnt> listPointages, int duplicatedPointageId, out List<RapportEnt> rapportsAdded, out List<RapportEnt> rapportsUpdated);

        /// <summary>
        /// Remplir les informations des sorties astreintes
        /// </summary>
        /// <param name="rapport">Le rapport</param>
        /// <returns>Le rapport remplit</returns>
        RapportEnt FulfillAstreintesInformations(RapportEnt rapport);

        /// <summary>
        /// Récupération de liste des rapports en fonction d'une liste d'identifiants de rapport
        /// </summary>
        /// <param name="rapportIds">Liste d'identifiants de rapport</param>
        /// <returns>Liste de rapport</returns>
        IEnumerable<RapportEnt> GetRapportList(IEnumerable<int> rapportIds);

        /// <summary>
        /// Retourne la liste des rapports avec leurs lignes 
        /// </summary>
        /// <param name="rapportIds">Id des rapports à récupérer</param>
        /// <returns>Une liste de rapport potentiellement vide, jamais null</returns>
        IEnumerable<RapportEnt> GetRapportListWithRapportLignesNoTracking(IEnumerable<int> rapportIds);

        /// <summary>
        /// Ajout ou mise à jour en masse
        /// </summary>
        /// <param name="rapports">Liste de rapports</param>
        void AddOrUpdateRapportList(IEnumerable<RapportEnt> rapports);

        /// <summary>
        /// Renvoie la liste des rapport entre 2 dates . Les rapports retournés conçernent les ci envoyés
        /// </summary>
        /// <param name="ciList">Liste des Cis</param>
        /// <param name="startDate">Date de début</param>
        /// <param name="endDate">Date de fin</param>
        /// <returns>List de rapports en les 2 dates</returns>
        IEnumerable<RapportEnt> GetRapportListBetweenDatesByCiList(IEnumerable<int> ciList, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Ajout en masse des rapport
        /// </summary>
        /// <param name="rapportList">Liste des rapports à ajouter en masse</param>
        void AddRangeRapportList(IEnumerable<RapportEnt> rapportList);

        /// <summary>
        /// Permet d'inserer un nouveau rapport creer a partir de figgo
        /// </summary>
        /// <param name="rapport">rapport a inserer</param>
        /// <returns>rapport inserer</returns>
        RapportEnt AddRapportFiggo(RapportEnt rapport);

        /// <summary>
        ///   Ajoute une tache au paramétrage du rapport et à toutes les lignes de rapport
        /// </summary>
        /// <param name="rapport">Le rapport</param>
        /// <param name="tache">La tache</param>
        /// <param name="heureTache">heure tache</param>
        /// <returns>Un rapport</returns>
        RapportEnt AddTacheToRapportFiggo(RapportEnt rapport, TacheEnt tache, double heureTache);
    }
}
