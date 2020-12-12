using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Fred.Entities.Referential;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    ///   Représente un référentiel de données pour les codes zone déplacement
    /// </summary>
    public interface ICodeZoneDeplacementRepository : IRepository<CodeZoneDeplacementEnt>
    {
        /// <summary>
        ///   Retourne la liste de tous les codes zone déplacement.
        /// </summary>
        /// <returns>List de toutes les sociétés</returns>
        IEnumerable<CodeZoneDeplacementEnt> GetCodeZoneDeplacementListAll();

        /// <summary>
        ///   La liste de tous les codes zone déplacement.
        /// </summary>
        /// <returns>Renvoie la liste de des codes zone déplacement active</returns>
        IEnumerable<CodeZoneDeplacementEnt> GetCodeZoneDeplacementList();

        /// <summary>
        ///   Permet la récupération d'une zone de déplacement en fonction de l'identifiant de sa société de paramétrage et de son
        ///   code.
        /// </summary>
        /// <param name="societeId">Identifiant de la société de paramétrage de la zone de déplacement</param>
        /// <param name="codeZone">Code de la zone de déplacement</param>
        /// <returns>Retourne la zone de déplacement en fonction du code passé en paramètre, null si inexistant.</returns>
        CodeZoneDeplacementEnt GetZoneBySocieteIdAndCode(int societeId, string codeZone);

        /// <summary>
        ///   Sauvegarde les modifications d'un CodeZoneDeplacement
        /// </summary>
        /// <param name="codeZoneDeplacement">Code zone deplacement à modifier</param>
        void UpdateCodeZoneDeplacement(CodeZoneDeplacementEnt codeZoneDeplacement);

        /// <summary>
        ///   Ajout d'une code zone déplacement
        /// </summary>
        /// <param name="codeZoneDeplacement">Code zone déplacement à ajouter</param>
        /// <returns>L'identifiant du code zone déplacement ajouté</returns>
        int AddCodeZoneDeplacement(CodeZoneDeplacementEnt codeZoneDeplacement);

        /// <summary>
        ///   Supprime un code zone deplacement
        /// </summary>
        /// <param name="id">L'identifiant du code zone deplacement à supprimer</param>
        /// <param name="idUtilisateur">Identifiant de l'utilisateur ayant fait l'action</param>
        void DeleteCodeZoneDeplacementById(int id, int idUtilisateur);

        /// <summary>
        ///   Vérifie qu'une societe peut être supprimée
        /// </summary>
        /// <param name="codeZoneDeplacementEnt">Le code zone déplacement à vérifier</param>
        /// <returns>True = suppression ok</returns>
        bool IsDeletable(CodeZoneDeplacementEnt codeZoneDeplacementEnt);

        /// <summary>
        ///   Code zone déplacement via l'id
        /// </summary>
        /// <param name="id">Id du code zone déplacement</param>
        /// <returns>Renvoie un code zone déplacement</returns>
        CodeZoneDeplacementEnt GetCodeZoneDeplacementById(int id);

        /// <summary>
        ///   Code zone déplacement via societeId
        /// </summary>
        /// <param name="societeId">idSociete de la société</param>
        /// <param name="actif">True pour les actifs, false pour les inactifs, null pour tous.</param>
        /// <returns>Renvoie un code zone déplacement</returns>
        IEnumerable<CodeZoneDeplacementEnt> GetCodeZoneDeplacementBySocieteId(int societeId, bool? actif = null);

        /// <summary>
        ///   Import des codes zones deplacements automatiques depuis la Holding
        /// </summary>
        /// <param name="holdingId"> Id du Holding</param>
        /// <returns>Renvoie un int</returns>
        int ImportCodeZoneDeplacementFromHolding(int holdingId);

        /// <summary>
        ///   Permet de récupérer la liste des codes zones deplacements en fonction des critères de recherche.
        /// </summary>
        /// <param name="predicate">Filtres de recherche sur les codes zones deplacements</param>
        /// <returns>Retourne la liste filtrée des codes zones deplacements</returns>
        IEnumerable<CodeZoneDeplacementEnt> SearchCodeZoneDeplacementWithFilters(Expression<Func<CodeZoneDeplacementEnt, bool>> predicate);

        /// <summary>
        ///   Permet de récupérer la liste de tous les codes zones deplacements en fonction des critères de recherche.
        /// </summary>
        /// <param name="predicate">Filtres de recherche sur tous les codes zones deplacements</param>
        /// <returns>Retourne la liste filtrée de tous les codes zones deplacements</returns>
        IEnumerable<CodeZoneDeplacementEnt> SearchCodeZoneDeplacementAllWithFilters(Expression<Func<CodeZoneDeplacementEnt, bool>> predicate);

        /// <summary>
        ///   Permet de récupérer la liste de tous les codes zones deplacements en fonction des critères de recherche par société.
        /// </summary>
        /// <param name="societeId">Id de la societe</param>
        /// <param name="predicate">Filtres de recherche sur tous les codes zones deplacements</param>
        /// <returns>Retourne la liste filtrée de tous les codes zones deplacements</returns>
        IEnumerable<CodeZoneDeplacementEnt> SearchCodeZoneDeplacementAllBySocieteIdWithFilters(int societeId, Expression<Func<CodeZoneDeplacementEnt, bool>> predicate);

        /// <summary>
        ///   Permet de connaître l'existence d'une société depuis son code.
        /// </summary>
        /// <param name="idCourant">id courant</param>
        /// <param name="codeZoneDeplacement">code CodeZoneDeplacement</param>
        /// <param name="societeId">Id societe</param>
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
        /// <param name="text">Texte à rechercher dans le champs code</param>
        /// <returns>Retourne une liste de code zone deplacement</returns>
        IEnumerable<CodeZoneDeplacementEnt> SearchLight(string text);

        /// <summary>
        ///  Vérifie s'il l'entité est déja utilisée
        /// </summary>
        /// <param name="id">Id de l'entité à vérifié</param>
        /// <returns>True = déjà Utilisée</returns>
        bool IsAlreadyUsed(int id);

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
