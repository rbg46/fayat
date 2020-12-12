using System;
using System.Collections.Generic;
using Fred.Entities.CI;
using Fred.Entities.Personnel.Interimaire;
using Fred.Web.Shared.Models.Personnel.Interimaire;

namespace Fred.Business.Personnel.Interimaire
{
    /// <summary>
    ///   Gestionnaire des contrats d'intérimaires
    /// </summary>
    public interface IContratInterimaireManager : IManager<ContratInterimaireEnt>
    {
        /// <summary>
        /// Permet de récupérer une liste de contrat intérimaire en fonction d'un personnel id
        /// </summary>
        /// <param name="personnelId">Identifiant unique du personnel</param>
        /// <returns>Liste des contrats intérimaire appartenant au personnel id</returns>
        List<ContratInterimaireEnt> GetContratInterimaireByPersonnelId(int personnelId);

        /// <summary>
        /// Permet de récupérer une liste des motifs de remplacement
        /// </summary>
        /// <returns>Liste des motifs de remplacement</returns>
        List<MotifRemplacementEnt> GetMotifRemplacement();

        /// <summary>
        /// Permet de récupérer un contrat intérimaire en fonction de l'identifiant unique du contrat
        /// </summary>
        /// <param name="contratInterimaireId">Identifiant unique du contrat d'interimaire</param>
        /// <returns>Le contrat d'intérimaire</returns>
        ContratInterimaireEnt GetContratInterimaireById(int contratInterimaireId);

        /// <summary>
        /// Permet de récupérer le contrat actif d'un intérimaire en fonction de son identifiant unique
        /// </summary>
        /// <param name="interimaireId">Identifiant unique de l'interimaire</param>
        /// <returns>Le contrat d'intérimaire</returns>
        ContratInterimaireEnt GetContratInterimaireActifByPersonnelId(int interimaireId);

        /// <summary>
        /// Permet de récupérer un contrat intérimaire en fonction de son numéro de contrat
        /// </summary>
        /// <param name="numeroContrat">Numéro du contrat d'interimaire</param>
        /// <param name="contratInterimaireId">Identifiant unique du contrat d'interimaire</param>
        /// <returns>Le contrat d'intérimaire</returns>
        ContratInterimaireEnt GetContratInterimaireByNumeroContrat(string numeroContrat, int contratInterimaireId);

        /// <summary>
        /// Permet de récupérer le contrat actif d'un intérimaire
        /// </summary>
        /// <param name="contratInterimaireId">Identifiant Unique du Contrat Intérimaire</param>
        /// <param name="interimaireId">Identifiant Unique de l'Intérimaire</param>
        /// <param name="dateDebut">Date de début du Contrat Intérimaire</param>
        /// <param name="dateFin">Date de fin du Contrat Intérimaire</param>
        /// <returns>Le contrat d'intérimaire</returns>
        ContratInterimaireEnt GetContratInterimaireAlreadyActif(int contratInterimaireId, int interimaireId, DateTime dateDebut, DateTime dateFin);

        /// <summary>
        /// Permet de récupérer le contrat d'un intérimaire à une date de pointage donnée
        /// </summary>
        /// <param name="interimaireId">Identifiant unique de l'interimaire</param>
        /// <param name="datePointage">date de pointage d'un rapport</param>
        /// <returns>Le contrat d'intérimaire</returns>
        ContratInterimaireEnt GetContratInterimaireByDatePointage(int? interimaireId, DateTime datePointage);

        /// <summary>
        /// Permet d'ajouter un contrat à un personnel intérimaire
        /// </summary>
        /// <param name="contratInterimaireEnt">Contrat Intérimaire</param>
        /// <returns>Le contrat intérimaire enregistré</returns>
        ContratInterimaireEnt AddContratInterimaire(ContratInterimaireEnt contratInterimaireEnt, int? userId = null);

        /// <summary>
        /// Permet de modifier un contrat d'intérimaire
        /// </summary>
        /// <param name="contratInterimaireEnt">Contrat Intérimaire</param>
        /// <returns>Le contrat intérimaire enregistré</returns>
        ContratInterimaireEnt UpdateContratInterimaire(ContratInterimaireEnt contratInterimaireEnt);

        /// <summary>
        /// Permet de supprimer un contrat d'intérimaire en fonction de son identifiant
        /// </summary>
        /// <param name="contratInterimaireId">Identifiant unique d'un contart intérimaire</param>
        void DeleteContratInterimaireById(int contratInterimaireId);


        /// <summary>
        /// Retourne la liste des CI pour un intérimaire et ue date donnée
        /// </summary>
        /// <param name="personnelId">Identifiant du personnel</param>
        /// <param name="date">Date du pointage</param>
        /// <returns>La liste des CI pour un intérimaire et ue date donnée</returns>
        IEnumerable<CIEnt> GetCIList(int personnelId, DateTime date);

        /// <summary>
        /// Retourne un modèle de dates limites pour le contrat intérimaire
        /// </summary>
        /// <param name="personnelId">Identifiant du personnel</param>
        /// <param name="date">Date pour le lequel on veut connaître les limites du contrat actif</param>
        /// <returns>Un modèle de dates limites pour le contrat intérimaire</returns>
        DatesMaxInterimaireModel GetDatesMax(int personnelId, DateTime date);

        /// <summary>
        /// Retourne une liste de libelle de ci sur lesquels l'intérimaire a été pointé lors de sa période de contrat souplesse incluse
        /// </summary>
        /// <param name="contratInterimaireEnt">contrat intérimaire</param>
        /// <returns>liste des libelle des ci</returns>
        List<string> GetCiInRapportLigneByDateContratInterimaire(ContratInterimaireEnt contratInterimaireEnt);

        /// <summary>
        /// Permet de récupérer le contrat d'un intérimaire à une date de pointage donnée en comprenant la souplesse
        /// </summary>
        /// <param name="interimaireId">Identifiant unique de l'interimaire</param>
        /// <param name="date">date de pointage d'un rapport</param>
        /// <returns>Le contrat d'intérimaire</returns>
        ContratInterimaireEnt GetContratInterimaireByDatePointageAndSouplesse(int? interimaireId, DateTime date);

        /// <summary>
        /// Permet de récupérer le contrat d'un intérimaire à une date de pointage donnée pour les réceptions intérimaires
        /// </summary>
        /// <param name="interimaireId">Identifiant unique de l'interimaire</param>
        /// <param name="datePointage">date de pointage d'un rapport</param>
        /// <returns>Le contrat d'intérimaire</returns>
        ContratInterimaireEnt GetContratInterimaireByDatePointageForReceptionInterimaire(int? interimaireId, DateTime datePointage);

        /// <summary>
        /// Permet de récupérer le contrat d'un intérimaire à une date de pointage donnée en comprenant la souplesse pour les réceptions intérimaires
        /// </summary>
        /// <param name="interimaireId">Identifiant unique de l'interimaire</param>
        /// <param name="date">date de pointage d'un rapport</param>
        /// <returns>Le contrat d'intérimaire</returns>
        ContratInterimaireEnt GetContratInterimaireByDatePointageAndSouplesseForReceptionInterimaire(int? interimaireId, DateTime date);

        /// <summary>
        /// Retourne la liste des jours ouverts dans les contrats de l'intérimaire
        /// </summary>
        /// <param name="interimaireId">Identifiant de l'intérimaire</param>
        /// <param name="period">Période</param>
        /// <returns>La liste des jours ouverts dans les contrats de l'intérimaire</returns>
        List<int> GetListDaysAvailableInPeriod(int interimaireId, DateTime period);


        /// <summary>
        /// Retourne la liste des contrats d'un intérimaire ayant des jours ouverts sur la période donnée
        /// </summary>
        /// <param name="personnelIds">Identifiant des personnel</param>      
        /// <param name="startDate">Date de debut de la periode</param>
        /// <param name="endDate">date de fin de la periode</param>       
        /// <returns>La liste des contrats d'un intérimaire ayant des jours ouverts sur la période donnée</returns>
        List<ContratInterimaireEnt> GetListContratInterimaireOpenOnPeriod(List<int> personnelIds, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Retourne la liste des contrats d'un intérimaire ayant des jours ouverts sur la période donnée
        /// </summary>
        /// <param name="interimaireId">Identifiant de l'intérimaire</param>
        /// <param name="period">Période</param>
        /// <returns>La liste des contrats d'un intérimaire ayant des jours ouverts sur la période donnée</returns>
        List<ContratInterimaireEnt> GetListContratInterimaireOpenOnPeriod(int interimaireId, DateTime period);

        /// <summary>
        /// Pourun interiaire, vérifie si le compte est expiré 
        /// et met à jour la date d'expritation avec la date de fin de contrat interimaire
        /// </summary>
        /// <param name="userId">Udentifiant utilisateur</param>
        void CheckContratInterimaireAndExpirationDate(int userId);

        /// <summary>
        /// Récupére le contrat intérimaire par numéro de contrat et groupe code
        /// </summary>
        /// <param name="numeroContrat">Numero de contrat</param>
        /// <param name="groupeCode">Code du groupe</param>
        /// <returns>Un contrat interimaire</returns>
        ContratInterimaireEnt GetContratInterimaireByNumeroContratAndGroupeCode(string numeroContrat, string groupeCode);
    }
}
