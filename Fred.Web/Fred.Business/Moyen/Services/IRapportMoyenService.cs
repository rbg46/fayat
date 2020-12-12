using System;
using System.Collections.Generic;
using Fred.Business.Moyen.Common;
using Fred.Entities.Moyen;
using Fred.Entities.Rapport;
using Fred.Framework.DateTimeExtend;
using Fred.Web.Shared.Models.Moyen;

namespace Fred.Business.Moyen
{
    /// <summary>
    /// Fonctionnalité pour assurer l'interaction avec rapport de pointage : Rapport et RapportLigne
    /// </summary>
    public interface IRapportMoyenService : IService
    {

        /// <summary>
        /// Retourne la liste des pointage pour les personnels et les Cis envoyés en paramétres dans la plage des dates définies
        /// </summary>
        /// <param name="ciIdList">La liste des id des Cis</param>
        /// <param name="personnelIdList">La liste des id des personnels</param>
        /// <param name="startDate">Date de début limite</param>
        /// <param name="endDate">Date de fin limite</param>
        /// <returns>Retourne la liste des pointage</returns>
        IEnumerable<RapportLigneEnt> GetPointageByCisPersonnelsAndDates(
            IEnumerable<int> ciIdList,
            IEnumerable<int> personnelIdList,
            DateTime startDate, DateTime endDate);

        /// <summary>
        /// Process de l'affectation à d'un moyen 
        /// </summary>
        /// <param name="request">Update moyen pointage input</param>
        /// <param name="affectationMoyen">L'affectation du moyen</param>
        /// <param name="dateTimeExtendManager">Date time extend manager</param>
        /// <param name="helper">Moyen pointage helper</param>
        /// <param name="affectationMoyenIdList">Liste Des Affections dont le rapport ligne ete cree </param>
        /// <returns>Réponse suite au traitement d'une affectation</returns>
        ProcessAffectationResponse Process(
            UpdateMoyenPointageInput request,
            AffectationMoyenEnt affectationMoyen,
            IDateTimeExtendManager dateTimeExtendManager,
            MoyenPointageHelper helper,
            List<AffectationMoyenRapportModel> affectationMoyenIdList);

        /// <summary>
        /// Renvoie les heures travaillées par un personnel donné
        /// </summary>
        /// <param name="rapportLignes">La liste des lignes de rapport</param>
        /// <param name="helper">Moyen pointage helper</param>
        /// <returns>Liste des PersonnelPointage</returns>
        IEnumerable<PersonnelPointage> GetPersonnelPointageSummary(IEnumerable<RapportLigneEnt> rapportLignes, MoyenPointageHelper helper);

        /// <summary>
        /// Enregistrement des modifications au niveau du pointage
        /// </summary>
        /// <param name="linesToCreate">La liste des lignes de rapport à créer</param>
        /// <param name="linesToUpdate">La liste des lignes de rapport à modifier</param>
        void UpdatePointageMoyen(IEnumerable<RapportLigneEnt> linesToCreate, IEnumerable<RapportLigneEnt> linesToUpdate);
    }
}
