using System;
using System.Collections.Generic;

using Fred.Entities.CI;
using Fred.Entities.Personnel.Interimaire;
using Fred.Web.Shared.Models.Personnel.Interimaire;

namespace Fred.DataAccess.Interfaces
{
    /// <summary>
    ///   Représente un référentiel de données des contrat d'intérimaire
    /// </summary>
    public interface IContratInterimaireRepository : IRepository<ContratInterimaireEnt>
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
        /// Permet de récupérer une liste de contrat intérimaire en fonction d'un personnel id
        /// </summary>
        /// <param name="contratInterimaireId">Identifiant unique du contrat d'interimaire</param>
        /// <returns>Le contrat d'intérimaire</returns>
        ContratInterimaireEnt GetContratInterimaireById(int contratInterimaireId);

        /// <summary>
        /// Permet de récupérer un contrat intérimaire en fonction de son numéro de contrat
        /// </summary>
        /// <param name="numeroContrat">Numéro du contrat d'interimaire</param>
        /// <param name="contratInterimaireId">Identifiant unique du contrat d'interimaire</param>
        /// <returns>Le contrat d'intérimaire</returns>
        ContratInterimaireEnt GetContratInterimaireByNumeroContrat(string numeroContrat, int contratInterimaireId);

        /// <summary>
        /// Permet de récupérer le contrat d'un intérimaire à une date de pointage donnée
        /// </summary>
        /// <param name="interimaireId">Identifiant unique de l'interimaire</param>
        /// <param name="datePointage">date de pointage d'un rapport</param>
        /// <returns>Le contrat d'intérimaire</returns>
        ContratInterimaireEnt GetContratInterimaireByDatePointage(int? interimaireId, DateTime datePointage);

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
        /// Permet d'ajouter un contrat à un personnel intérimaire
        /// </summary>
        /// <param name="contratInterimaireEnt">Contrat Intérimaire</param>
        /// <returns>Le contrat intérimaire enregistré</returns>
        ContratInterimaireEnt AddContratInterimaire(ContratInterimaireEnt contratInterimaireEnt);

        /// <summary>
        /// Permet de modifier un contrat d'intérimaire
        /// </summary>
        /// <param name="contratInterimaireEnt">Contrat Intérimaire</param>
        /// <returns>Le contrat intérimaire enregistré</returns>
        ContratInterimaireEnt UpdateContratInterimaire(ContratInterimaireEnt contratInterimaireEnt);

        /// <summary>
        /// Permet de supprimer un contrat d'intérimaire en fonction de son identifiant
        /// </summary>
        /// <param name="id">Identifiant unique d'une délégation</param>
        void DeleteContratInterimaireById(int id);

        /// <summary>
        /// Retourne la liste des CI pour un intérimaire et une date donnée
        /// </summary>
        /// <param name="personnelId">Identifiant du personnel</param>
        /// <param name="date">Date du pointage</param>
        /// <returns>La liste des CI pour un intérimaire et ue date donnée</returns>
        IEnumerable<CIEnt> GetCIList(int personnelId, DateTime date);

        /// <summary>
        /// Retourne la date de début du prochain contrat de l'intérimaire
        /// </summary>
        /// <param name="personnelId">Identifiant de l'intérimaire</param>
        /// <param name="date">Date</param>
        /// <returns>une date</returns>
        DateTime? GetDateDebutNextContrat(int personnelId, DateTime date);

        /// <summary>
        /// Retourne une liste de libelle de ci sur lesquels l'intérimaire a été pointé lors de sa période de contrat souplesse incluse
        /// </summary>
        /// <param name="interimaireId">Identifiant Unique d'un intérimaire</param>
        /// <param name="periode">période du contrat</param>
        /// <param name="ciId">Identifiant unique d'un ci</param>
        /// <returns>liste des libelle des ci</returns>
        List<string> GetCiInRapportLigneByDateContratInterimaire(int interimaireId, DatesMaxInterimaireModel periode, int ciId);

        /// <summary>
        /// Permet de récupérer le contrat d'un intérimaire à une date de pointage donnée en comprenant la souplesse
        /// </summary>
        /// <param name="interimaireId">Identifiant unique de l'interimaire</param>
        /// <param name="date">date de pointage d'un rapport</param>
        /// <returns>Le contrat d'intérimaire</returns>
        List<ContratInterimaireEnt> GetContratInterimaireByDatePointageAndSouplesse(int? interimaireId, DateTime date);

        /// <summary>
        /// Permet de récupérer le contrat d'un intérimaire à une date de pointage donnée pour les réception intérimaire
        /// </summary>
        /// <param name="interimaireId">Identifiant unique de l'interimaire</param>
        /// <param name="datePointage">date de pointage d'un rapport</param>
        /// <returns>Le contrat d'intérimaire</returns>
        ContratInterimaireEnt GetContratInterimaireByDatePointageForReceptionInterimaire(int? interimaireId, DateTime datePointage);

        /// <summary>
        /// Permet de récupérer le contrat d'un intérimaire à une date de pointage donnée en comprenant la souplesse pour les réception intérimaires
        /// </summary>
        /// <param name="interimaireId">Identifiant unique de l'interimaire</param>
        /// <param name="date">date de pointage d'un rapport</param>
        /// <returns>Le contrat d'intérimaire</returns>
        ContratInterimaireEnt GetContratInterimaireByDatePointageAndSouplesseForReceptionInterimaire(int? interimaireId, DateTime date);

        /// <summary>
        /// Retourne la liste des contrats d'un intérimaire ayant des jours ouverts sur la période donnée
        /// </summary>
        /// <param name="interimaireId">Identifiant de l'intérimaire</param>
        /// <param name="period">Période</param>
        /// <returns>La liste des contrats d'un intérimaire ayant des jours ouverts sur la période donnée</returns>
        List<ContratInterimaireEnt> GetListContratInterimaireOpenOnPeriod(int interimaireId, DateTime period);

        /// <summary>
        /// Retourne la liste des contrats d'un intérimaire ayant des jours ouverts sur la période donnée, concernant les personnels et sur un ci donné
        /// </summary>
        /// <param name="personnelIds">Les id des personnels ou l'on recherche les contrats</param>       
        /// <param name="startDate">Date de debut de la periode</param>
        /// <param name="endDate">date de fin de la periode</param>   
        /// <returns>La liste des contrats d'un intérimaire ayant des jours ouverts sur la période donnée</returns>
        List<ContratInterimaireEnt> GetListContratInterimaireOpenOnPeriod(List<int> personnelIds, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Permet de récupérer le contrat intérimaire actif en fonction d'un personnel id
        /// </summary>
        /// <param name="personnelId">Identifiant unique du personnel</param>
        /// <returns>Le contrat intérimaire appartenant au personnel id</returns>
        ContratInterimaireEnt GetContratInterimaireActifByPersonnelId(int personnelId);

        /// <summary>
        /// Permet de récupérer les contrats intérimaires par date début et fin, par CiId et par Etablissement comptable Id
        /// </summary>
        /// <param name="dateDebutChantier">Date debut chantier</param>
        /// <param name="dateFinChantier">Date fin chantier</param>
        /// <returns>List des contrats intérimaires</returns>
        List<ContratInterimaireEnt> GetListContratsInterimaires(DateTime? dateDebutChantier, DateTime? dateFinChantier);

        /// <summary>
        /// Récupére le contrat intérimaire par numéro de contrat et groupe code
        /// </summary>
        /// <param name="numeroContrat">Numero de contrat</param>
        /// <param name="groupeCode">Code du groupe</param>
        /// <returns>Un contrat interimaire</returns>
        ContratInterimaireEnt GetContratInterimaireByNumeroContratAndGroupeCode(string numeroContrat, string groupeCode);
    }
}
