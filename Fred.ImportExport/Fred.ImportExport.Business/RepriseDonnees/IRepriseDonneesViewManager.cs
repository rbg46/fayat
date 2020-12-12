using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Fred.Business;
using Fred.Entities.Groupe;
using Fred.Entities.RepriseDonnees;
using Fred.ImportExport.Business.CI.AnaelSystem.Context.Common;

namespace Fred.ImportExport.Business.RepriseDonnees
{
    /// <summary>
    /// Manager des vues RepriseDonnees
    /// </summary>
    public interface IRepriseDonneesViewManager : IManager
    {

        /// <summary>
        /// Retourne tous les groupes
        /// </summary>
        /// <returns>Liste de groupes</returns>
        List<GroupeEnt> GetAllGroupes();


        /// <summary>
        /// Importation des Cis 
        /// </summary>
        /// <param name="groupeId">groupeId</param>
        /// <param name="stream">stream</param>
        /// <returns>le resultat de l'import</returns>
        ImportCiResult ImportCis(int groupeId, Stream stream);

        /// <summary>
        /// Creation des commandes et des recepetions
        /// </summary>
        /// <param name="groupeId">groupeId</param>
        /// <param name="stream">stream</param>
        /// <returns>le resultat de l'import</returns>
        ImportCommandeResult CreateCommandeAndReceptions(int groupeId, Stream stream);

        /// <summary>
        /// Creation des rapports et rapport lignes 
        /// </summary>
        /// <param name="groupeId">groupeId</param>
        /// <param name="stream">stream</param>
        /// <returns>le resultat de l'import</returns>
        ImportRapportResult ImportRapports(int groupeId, Stream stream);

        /// <summary>
        /// Validation des commandes
        /// </summary>
        /// <param name="groupeId">groupeId</param>
        /// <param name="stream">stream</param>
        /// <returns>le resultat de l'import</returns>
        ImportValidationCommandeResult ValidationCommandes(int groupeId, Stream stream);

        /// <summary>
        /// Creation du Plan de taches
        /// </summary>
        /// <param name="groupeId">groupeId</param>
        /// <param name="stream">stream</param>
        /// <returns>le resultat de l'import</returns>
        ImportPlanTachesResult CreatePlanTaches(int groupeId, Stream stream);

        /// <summary>
        /// Creation des Personnels
        /// </summary>
        /// <param name="groupeId">groupeId</param>
        /// <param name="stream">stream</param>
        /// <returns>Le resultat de l'import</returns>
        ImportPersonnelResult CreatePersonnel(int groupeId, Stream stream);

        /// <summary>
        /// Création des Matériels
        /// </summary>
        /// <param name="groupeId">groupeId</param>
        /// <param name="stream">stream</param>
        /// <returns>Le résultat de l'import</returns>
        ImportMaterielResult CreateMateriel(int groupeId, Stream stream);

        /// <summary>
        /// Creation des Indemnites de Deplacement (et suppression logique si déjà existantes)
        /// </summary>
        /// <param name="groupeId">groupeId</param>
        /// <param name="stream">stream</param>
        /// <returns>Le résultat de l'import</returns>
        ImportIndemniteDeplacementResult CreateIndemniteDeplacement(int groupeId, Stream stream);

        /// <summary>
        /// Import des ci depuis Anael et export vers Sap
        /// </summary>
        /// <param name="groupeId">groupeId</param>
        /// <param name="stream">stream</param>
        /// <returns>le resultat de l'import</returns>
        Task<ImportResult> ImportCiFromAnaelAndSendToSapAsync(int groupeId, Stream stream);

        /// <summary>
        /// Import des fuornisseurs depuis Anael et export vers Sap
        /// </summary>
        /// <param name="groupeId">groupeId</param>
        /// <param name="stream">stream</param>
        /// <returns>le resultat de l'import</returns>
        Task<Common.ImportResult> ImportFournisseursFromAnaelAndSendToSapAsync(int groupeId, Stream stream);
    }
}
