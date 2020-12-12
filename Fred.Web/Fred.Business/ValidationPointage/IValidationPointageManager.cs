using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Entities.CI;
using Fred.Entities.ValidationPointage;

namespace Fred.Business.ValidationPointage
{
    /// <summary>
    ///   Interface IValidationPointageManager
    /// </summary>
    public interface IValidationPointageManager
    {
        /// <summary>
        ///   Lance le contrôle chantier d'un lot de pointage
        /// </summary>    
        /// <param name="lotPointageId">Identifiant du lot de pointage</param>    
        /// <param name="userId">Identifiant de l'utilisateur</param>
        /// <returns>Lot de pointage avec son statut</returns>
        Task<ControlePointageEnt> ExecuteControleChantierAsync(int lotPointageId, int userId);

        /// <summary>
        ///   Récupération de liste des lots de pointage comprenant :
        ///     - Le lot de pointage de l'utilisateur
        ///     - Les lots de pointages des autres utilisateurs ayant verrouillés des Rapports sur le même périmètre et pour la même période (Droits sur CI qui se chevauchent)
        /// </summary>
        /// <param name="utilisateurId">Identifiant de l'utilisateur</param>
        /// <param name="periode">Période choisie</param>
        /// <returns>Liste des lots de pointages</returns>
        IEnumerable<LotPointageEnt> GetAllLotPointage(int utilisateurId, DateTime periode);

        /// <summary>
        ///   Récupération de liste des lots de pointage comprenant :
        ///     - Le lot de pointage de l'utilisateur connecté
        ///     - Les lots de pointages des autres utilisateurs ayant verrouillés des Rapports sur le même périmètre et pour la même période (Droits sur CI qui se chevauchent)
        /// </summary>    
        /// <param name="periode">Période choisie</param>
        /// <returns>Liste des lots de pointages</returns>
        /// 
        IEnumerable<LotPointageEnt> GetAllLotPointage(DateTime periode);

        /// <summary>
        ///   Récupère le nombre de pointages non verrouillés, c-a-d, Rapport non verrouillé pour un utilisateur donné et une période donnée
        /// </summary>    
        /// <param name="periode">Période choisie</param>
        /// <returns>Nombre de pointages non verrouillés</returns>
        int CountPointageNonVerrouille(DateTime periode);

        /// <summary>
        ///   Récupère un filtre en fonction du type de contrôle
        /// </summary>
        /// <param name="typeControle">Type de contrôle</param>
        /// <returns>Filtre lot de pointage</returns>
        PointageFiltre GetFilter(int typeControle);

        /// <summary>
        ///   Récupère la liste du personnel du lot de pointage (Ceux avec et sans erreurs)
        /// </summary>
        /// <param name="controlePointageId">Identifiant du contrôle pointage</param>
        /// <param name="searchText">Texte à rechercher</param>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <returns>Liste des personnels avec erreurs avec nombre total d'erreurs</returns>
        SearchValidationResult<ControlePointageErreurEnt> GetAllPersonnelList(int controlePointageId, string searchText, int page, int pageSize);

        /// <summary>
        ///   Récupération du personnel avec ses erreurs de remontées vrac
        /// </summary>
        /// <param name="remonteeVracId">Identifiant de la remontée vrac</param>
        /// <param name="searchText">Texte recherché</param>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <returns>Liste du personnel avec erreur Remontée Vrac et nombre total d'erreurs</returns>
        SearchValidationResult<RemonteeVracErreurEnt> GetRemonteeVracErreurList(int remonteeVracId, string searchText, int page, int pageSize);

        /// <summary>
        ///   Génère et renvoie le document pdf (au format byte[]) contenant la liste des erreurs de validation
        /// </summary>
        /// <param name="controlePointageId">Identifiant du contrôle pointage</param>
        /// <returns>Tableau de byte</returns>
        byte[] GetControlePointageErreurPdf(int controlePointageId);

        /// <summary>
        ///   Retourne le nom du fichier d'export des erreurs de valdiation
        /// </summary>
        /// <param name="controlePointageId">Identifiant du controle pointage</param>
        /// <returns>Nom du fichier</returns>
        string GetControlePointageErreurFilename(int controlePointageId);

        /// <summary>
        ///   Génère et renvoie le document pdf (au format byte[]) contenant la liste des erreurs de remontée vrac
        /// </summary>
        /// <param name="remonteeVracId">Identifiant de la remontée vrac</param>
        /// <returns>Tableau de byte</returns>
        byte[] GetRemonteeVracErreurPdf(int remonteeVracId);

        /// <summary>
        ///   Retourne le nom du fichier d'export des erreurs de remontée vrac
        /// </summary>
        /// <param name="remonteeVracId">Identifiant de la remontée vrac</param>
        /// <returns>Nom du fichier</returns>
        string GetRemonteeVracErreurFilename(int remonteeVracId);

        /// <summary>
        ///   POST Vérification du passage Ci Sep au CI interne avant l'execution du controle vrac et remontee vrac
        /// </summary>
        /// <param name="lotPointageId">Identifiant du lot de pointage</param>    
        /// <param name="periode">Période choisie</param>
        /// <param name="filtre">Filtre pour lot de pointage</param>
        /// <returns>Liste Des Ci Sep non configurés</returns>
        List<CIEnt> VerificationCiSep(int? lotPointageId, DateTime? periode, PointageFiltre filtre);
    }
}
