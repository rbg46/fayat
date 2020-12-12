using System;
using System.Collections.Generic;
using Fred.Entities.Rapport;
using Fred.Web.Shared.Models.Rapport.RapportHebdo;

namespace Fred.Business.Rapport.RapportHebdo
{
    public interface IRapportHebdoService
    {
        void AddPersonnelPointageToAllRapports(List<RapportEnt> rapportList, int personnelId);
        void CreateOrUpdatePrimeAstreinte(RapportEnt rapport);
        void AddOrUpdateAstreintePrime(RapportLigneEnt rapportLigne);

        /// <summary>
        /// Récupérer la liste des rapports d'un CI dans une semaine donnée
        /// </summary>
        /// <param name="ciId">Ci identifier</param>
        /// <param name="dateDebut">Date debut de samaine</param>
        /// <param name="dateFin">Date fin</param>
        /// <param name="personnelsIds">List des personnels Id</param>
        /// <returns>List des rapports</returns>
        List<RapportEnt> GetCiRapportsByWeek(int ciId, DateTime dateDebut, DateTime dateFin, IEnumerable<int> personnelsIds = null);

        /// <summary>
        /// Check Statut personnel
        /// </summary>
        /// <param name="rapportHebdoNodes"> Pointage view model</param>
        /// <returns>statut</returns>
        string GetStatutPersonnelRapportHebdo(IEnumerable<RapportHebdoNode<PointageCell>> rapportHebdoNodes);

        /// <summary>
        /// Récupérer la liste des rapports d'un ensemble de CI dans une semaine donnée
        /// </summary>
        /// <param name="ciIds">CI concernés</param>
        /// <param name="dateDebut">Date debut de samaine</param>
        /// <returns>List des rapports</returns>
        Dictionary<int, List<RapportEnt>> GetCiRapportsByWeek(IEnumerable<int> ciIds, DateTime dateDebut, string statut = null);

        /// <summary>
        /// Handle Hebdo Rapports for employee
        /// </summary>
        /// <param name="groupes">Dictionnaire des Cis - Rapports</param>
        /// <param name="ciIds">List des CIs</param>
        /// <param name="dateDebut">Date debut</param>
        /// <param name="dateFin">Date fin</param>
        /// <returns>Dictionnaire</returns>
        Dictionary<int, List<RapportEnt>> HandleHebdoRapportsForEmployee(Dictionary<int, List<RapportEnt>> groupes, IEnumerable<int> ciIds, DateTime dateDebut, DateTime dateFin, string statut = null);
    }
}