using System.Collections.Generic;
using Fred.Entities.Referential;

namespace Fred.Business.Referential
{
    /// <summary>
    ///   Interface des gestionnaires des codes zones deplacements
    /// </summary>
    public interface ICodeZoneDeplacementManager : IManager<CodeZoneDeplacementEnt>
    {
        /// <summary>
        ///   Retourne la liste de tous les codes zones deplacements.
        /// </summary>
        /// <returns>List de toutes les sociétés</returns>
        IEnumerable<CodeZoneDeplacementEnt> GetCodeZoneDeplacementListAll();

        /// <summary>
        ///   La liste de tous les codes zones deplacements.
        /// </summary>
        /// <returns>Renvoie la liste de des codes zones deplacements active</returns>
        IEnumerable<CodeZoneDeplacementEnt> GetCodeZoneDeplacementList();

        /// <summary>
        ///   Sauvegarde les modifications d'un CodeZoneDeplacement
        /// </summary>
        /// <param name="codeAbs">Code zone deplacement à modifier</param>
        void UpdateCodeZoneDeplacement(CodeZoneDeplacementEnt codeAbs);

        /// <summary>
        ///   Ajout d'une code zone deplacement
        /// </summary>
        /// <param name="codeAbs">Code zone deplacement à ajouter</param>
        /// <returns>L'identifiant du code zone deplacement ajouté</returns>
        int AddCodeZoneDeplacement(CodeZoneDeplacementEnt codeAbs);

        /// <summary>
        ///   Supprime un code zone deplacement
        /// </summary>
        /// <param name="codeZoneDeplacement">Le code zone deplacement à supprimer</param>
        /// <returns>Retourne vrai si l'entité a bien été supprimé sinon faux</returns>
        bool DeleteCodeZoneDeplacementById(CodeZoneDeplacementEnt codeZoneDeplacement);

        /// <summary>
        ///   Code zone deplacement via l'id
        /// </summary>
        /// <param name="id">Id du code zone deplacement</param>
        /// <returns>Renvoie un code zone deplacement</returns>
        CodeZoneDeplacementEnt GetCodeZoneDeplacementById(int id);

        /// <summary>
        ///   Code zone deplacement via societeId
        /// </summary>
        /// <param name="societeId">idSociete de la société</param>
        /// <param name="actif">True pour les actifs, false pour les inactifs, null pour tous.</param>
        /// <returns>Renvoie un code zone deplacement</returns>
        IEnumerable<CodeZoneDeplacementEnt> GetCodeZoneDeplacementBySocieteId(int societeId, bool? actif = null);

        /// <summary>
        ///   Import des codes zones deplacements automatiques depuis la Holding
        /// </summary>
        /// <param name="holdingId"> Id du Holding</param>
        /// <returns>Renvoie int</returns>
        int ImportCodeZoneDeplacementFromHolding(int holdingId);

        /// <summary>
        ///   Permet de récupérer la liste des codes zones deplacements en fonction des critères de recherche.
        /// </summary>
        /// <param name="text">Texte recherché dans les sociétés</param>
        /// <param name="filters">Filtres de recherche</param>
        /// <returns>Retourne la liste filtré des sociétés</returns>
        IEnumerable<CodeZoneDeplacementEnt> SearchCodeZoneDeplacementWithFilters(string text, SearchCodeZoneDeplacementEnt filters);

        /// <summary>
        ///   Permet de récupérer la liste de tous les codes zones deplacements en fonction des critères de recherche.
        /// </summary>
        /// <param name="text">Texte recherché dans tous les codes zones deplacements</param>
        /// <param name="filters">Filtres de recherche</param>
        /// <returns>Retourne la liste filtré de tous les codes zones deplacements</returns>
        IEnumerable<CodeZoneDeplacementEnt> SearchCodeZoneDeplacementAllWithFilters(string text, SearchCodeZoneDeplacementEnt filters);

        /// <summary>
        ///   Permet l'initialisation d'une nouvelle instance de code zone deplacement.
        /// </summary>
        /// <param name="societeId">Identifiant de la societe</param>
        /// <returns>Nouvelle instance de code zone deplacement intialisée</returns>
        CodeZoneDeplacementEnt GetNewCodeZoneDeplacement(int societeId);

        /// <summary>
        ///   Permet de récupérer les champs de recherche lié à un code zone deplacement.
        /// </summary>
        /// <returns>Retourne la liste des champs de recherche par défaut d'uncode zone deplacement</returns>
        SearchCodeZoneDeplacementEnt GetDefaultFilter();

        /// <summary>
        ///   Permet de récupérer la liste de tous les codes zones deplacements en fonction des critères de recherche par société.
        /// </summary>
        /// <param name="filters">Filtres de recherche sur tous les codes zones deplacements</param>
        /// <param name="societeId">Id de la societe</param>
        /// <param name="text">text de recherche</param>
        /// <returns>Retourne la liste filtrée de tous les codes zones deplacements</returns>
        IEnumerable<CodeZoneDeplacementEnt> SearchCodeZoneDeplacementAllBySocieteIdWithFilters(SearchCodeZoneDeplacementEnt filters, int societeId, string text);

        /// <summary>
        ///   Permet de connaître l'existence d'une société depuis son code.
        /// </summary>
        /// <param name="idCourant">L'Id courant</param>
        /// <param name="codeZoneDeplacement">Le code de codeZoneDeplacement</param>
        /// <param name="societeId">Le code de la société</param>
        /// <returns>Retourne vrai si le code passé en paramètre existe déjà, faux sinon</returns>
        bool IsCodeZoneDeplacementExistsByCode(int idCourant, string codeZoneDeplacement, int societeId);

        /// <summary>
        ///   Permet de savoir si le code zone deplacement existe déjà pour une societe
        /// </summary>
        /// <param name="codeZoneDeplacement">code CodeZoneDeplacement</param>
        /// <param name="societeId">Id societe</param>
        /// <returns>Retourne vrai si le code passé en paramètre existe déjà, faux sinon</returns>
        bool IsCodeZoneDeplacementExistsBySoc(string codeZoneDeplacement, int societeId);

        /// <summary>
        ///   Recherche code zone deplacement sur le champ code uniquement
        /// </summary>
        /// <param name="text">Texte à rechercher dans les champs code-libelle</param>
        /// <param name="page">Page actuelle</param>
        /// <param name="pageSize">Taille d'une page</param>
        /// <param name="ciId">Identifiant du CI</param>
        /// <returns>Retourne une liste de code zone deplacement</returns>
        IEnumerable<CodeZoneDeplacementEnt> SearchLight(string text, int page, int pageSize, int? ciId, bool? isOuvrier = null, bool? isEtam = null, bool? isCadre = null);

        /// <summary>
        ///  Vérifie s'il l'entité est déja utilisée
        /// </summary>
        /// <param name="id">Id de l'entité à vérifié</param>
        /// <returns>True = déjà Utilisée</returns>
        bool IsAlreadyUsed(int id);

        /// <summary>
        ///   Permet la récupération d'une zone de déplacement en fonction de l'identifiant de sa société de paramétrage et de son
        ///   code.
        /// </summary>
        /// <param name="societeId">Identifiant de la société de paramétrage de la zone de déplacement</param>
        /// <param name="codeZone">Code de la zone de déplacement</param>
        /// <returns>Retourne la zone de déplacement en fonction du code passé en paramètre, null si inexistant.</returns>
        CodeZoneDeplacementEnt GetZoneBySocieteIdAndCode(int societeId, string codeZone);

        /// <summary>
        ///  Récupère le code zone le plus avantageux 
        /// </summary>
        /// <param name="codeZone1">Premier code zone à comparer</param>
        /// <param name="codeZone2">Deuxième code zone à comparer</param>
        /// <param name="societeId">Identifiant de la société</param>
        /// <returns>Le code le plus avantageux</returns>
        string GetCompareBiggestCodeZondeDeplacement(string codeZone1, string codeZone2, int societeId);

        /// <summary>
        /// Permet de récupérer le code zone deplacement en fonction du kilométrage
        /// </summary>
        /// <param name="societeId">société concerné</param>
        /// <param name="km">kilomètre</param>
        /// <returns>code zone déplacement</returns>
        CodeZoneDeplacementEnt GetCodeZoneDeplacementByKm(int societeId, double km);
    }
}
