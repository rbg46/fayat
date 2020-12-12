using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Entities.Rapport;
using Fred.Web.Shared.Models.Rapport;
using Fred.Web.Shared.Models.Rapport.RapportHebdo;

namespace Fred.Business.Rapport.RapportHebdo
{
    /// <summary>
    /// Interface des gestionnaires des rapports hebdomadaires
    /// </summary>
    public interface IRapportHebdoManager
    {
        /// <summary>
        /// Récupérer le rapport hebdomadaire par CI
        /// </summary>
        /// <param name="ciPersonnelListPairs">Dictionnaire contenant les CI et la liste des personnels correspandates</param>
        /// <param name="mondayDate">Date de lundi de la semaine choisie</param>
        /// <returns>Le rapport hebdomadaire</returns>
        List<RapportHebdoNode<PointageCell>> GetRapportHebdoByCi(Dictionary<int, List<int>> ciPersonnelListPairs, DateTime mondayDate);

        /// <summary>
        /// Récupérer le rapport hebdomadaire par ouvrier
        /// </summary>
        /// <param name="personnelId">L'identifiant du personnel</param>
        /// <param name="mondayDate">Date de lundi de la semaine choisie</param>
        /// <param name="allCi">Booléan indique s'il faut récupérer tous les CI pour le pointage</param>
        /// <returns>Le rapport hebdomadaire</returns>
        RapportHebdoNode<PointageCell> GetRapportHebdoByEmployee(int personnelId, DateTime mondayDate, bool allCi = false);

        /// <summary>
        /// Enregistrer le rapport hebdomadaire
        /// </summary>
        /// <param name="rapportHebdoSaveViewModel">Model pour enregistrer le rapport hebdomadaire</param>
        /// <returns>Le résultat de l'enregistrement.</returns>
        RapportHebdoSaveResultModel SaveRapportHebdo(RapportHebdoSaveViewModel rapportHebdoSaveViewModel);

        /// <summary>
        /// Vérifier et valider le rapport hebdomadaire
        /// </summary>
        /// <param name="rapportHebdoSaveViewModel">Model de Rapport hebdomadaire pour enregistrer</param>
        /// <param name="isEtamIac">Boolean indique si la validation concerne ETAM \ IAC ou Ouvrier</param>
        /// <returns>Résultat de validation d'un rapport hebdomadaire</returns>
        RapportHebdoValidationReponseViewModel CheckAndValidateRapportHebdo(RapportHebdoSaveViewModel rapportHebdoSaveViewModel, bool isEtamIac);

        /// <summary>
        /// Get synthese mensuelle
        /// </summary>
        /// <param name="utilisateurId">Identifiant de l'utilisateur</param>
        /// <param name="dateMonth">Mois du pointage</param>
        /// <returns>Rapport hebdo synthese mensuelle model</returns>
        IEnumerable<RapportHebdoSyntheseMensuelleEnt> GetSyntheseMensuelleRapportHebdo(int utilisateurId, DateTime dateMonth);

        /// <summary>
        /// Synthese mensuelle validation Model pour les Etam et Iac
        /// </summary>
        /// <param name="validateSyntheseMensuelleModel">Validation synthese mensuelle model</param>
        void ValiderSyntheseMensuelleEtamIac(ValidateSyntheseMensuelleModel validateSyntheseMensuelleModel);

        /// <summary>
        /// verifie si l'astreinte est prise entre un intervale de jour
        /// </summary>
        /// <param name="rapportLigne">Représente ou défini une sortie asteinte associées à une ligne de rapport</param>
        void CheckinAstreinteIntervalDay(RapportLigneEnt rapportLigne);

        /// <summary>
        /// Recupere l'id d'un statut de rapport verrouille
        /// </summary>
        /// <returns>Id du statut verrouille</returns>
        int GetRapportStatutVerrouille();

        /// <summary>
        /// Get les statuts des rapport lignes pour vérification Rapport hebdo
        /// </summary>
        /// <param name="personnelId">Personnek identifier</param>
        /// <param name="ciId">Ci identifier</param>
        /// <param name="mondayDate">Date du lundi</param>
        /// <returns>List des rapport hebdo new pointage statut</returns>
        List<RapportHebdoNewPointageStatutEnt> GetRapportLigneStatutForNewPointage(int personnelId, int ciId, DateTime mondayDate);

        /// <summary>
        /// Récupère une Node de sortie d'astreintes
        /// </summary>
        /// <param name="personnelId">Identifiant du personnel</param>
        /// <param name="ciId">Identifiant du Ci</param>
        /// <param name="mondayDate">Date du lundi</param>
        /// <returns>une Node de sortie d'astreintes</returns>
        RapportHebdoSubNode<PointageCell> GetSortie(int personnelId, int ciId, DateTime mondayDate);

        /// <summary>
        /// Get Validation Affaires 
        /// </summary>
        /// <param name="dateDebut">Date du lundi d'une semaine</param>
        /// <returns>List du synthese validation affaires model</returns>
        Task<IEnumerable<SyntheseValidationAffairesModel>> GetValidationAffairesByResponsableAsync(DateTime dateDebut);

        /// <summary>
        ///  Validation Pointage (Staut V2) par CiList and Affected ouvrier id List
        /// </summary>
        /// <param name="validationAffaireModel">Validation affaire mopdel</param>
        /// <returns>List du synthese validation affaires model</returns>
        Task ValidateAffairesByResponsableAsync(ValidationAffaireModel validationAffaireModel);
    }
}
