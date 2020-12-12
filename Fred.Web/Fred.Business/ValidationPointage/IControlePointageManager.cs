using Fred.Entities.ValidationPointage;
using System.Collections.Generic;

namespace Fred.Business.ValidationPointage
{
    /// <summary>
    ///   Interface Gestionnaire des Contrôles de Pointage
    /// </summary>
    public interface IControlePointageManager : IManager<ControlePointageEnt>
    {
        /// <summary>
        ///   Récupère une contrôle de pointage en fonction de son Identifiant
        /// </summary>
        /// <param name="controlePointageId">Identifiant de la contrôle de pointage</param>
        /// <returns>Contrôle de pointage</returns>
        ControlePointageEnt Get(int controlePointageId);

        /// <summary>
        ///   Récupère la contrôle de pointage la plus récente en fonction de l'identifiant du lot de pointage et du type de contrôle
        /// </summary>
        /// <param name="lotPointageId">Identifiant du lot de pointage</param>
        /// <param name="typeControle">Type de la contrôle de pointage (Controle Chantier, Controle Vrac)</param>
        /// <returns>Contrôle de pointage</returns>
        ControlePointageEnt GetLatest(int lotPointageId, int typeControle);

        /// <summary>
        ///   Récupère toutes les contrôles de pointage confondus
        /// </summary>
        /// <returns>Liste des contrôles de pointage</returns>
        IEnumerable<ControlePointageEnt> GetAll();

        /// <summary>
        ///   Récupère la liste des contrôles de pointages par identifiant du lot de pointage
        /// </summary>
        /// <param name="lotPointageId">Identifiant du lot de pointage</param>
        /// <returns>Liste des contrôles de pointage filtré</returns>
        IEnumerable<ControlePointageEnt> GetList(int lotPointageId);

        /// <summary>
        ///   Récupère la liste des dernières contrôles de pointages par identifiant du lot de pointage (Un ControlePointageEnt par type de contrôle)
        ///   Rappel : TypeControle = [Contrôle Chantier, Contrôle Vrac]
        /// </summary>
        /// <param name="listLotsPointagesIds">Liste des identifiants du lot de pointage</param>
        /// <returns>Liste des contrôles de pointage filtré</returns>
        IEnumerable<ControlePointageEnt> GetLatestList(List<int> listLotsPointagesIds);

        /// <summary>
        ///   Récupère la liste des contrôles de pointages par identifiant du Lot de pointage et par type de contrôle
        /// </summary>
        /// <param name="lotPointageId">Identifiant du lot de pointage</param>
        /// <param name="typeControle">Type de la contrôle de pointage (Controle Chantier, Controle Vrac)</param>
        /// <returns>Liste des contrôles de pointage filtré</returns>
        IEnumerable<ControlePointageEnt> GetList(int lotPointageId, int typeControle);

        /// <summary>
        ///   Ajoute une nouvelle contrôle de pointage
        /// </summary>
        /// <param name="controlePointage">Contrôle de pointage a ajouter</param>
        /// <returns>Contrôle de pointage ajouté</returns>
        ControlePointageEnt AddControlePointage(ControlePointageEnt controlePointage);

        /// <summary>
        ///   Mise à jour Contrôle de pointage
        /// </summary>
        /// <param name="controlePointage">Contrôle de pointage à mettre à jour</param>
        /// <returns>Contrôle de pointage mis à jour</returns>
        ControlePointageEnt UpdateControlePointage(ControlePointageEnt controlePointage);

        /// <summary>
        ///   Mise à jour du statut du Contrôle de pointage
        /// </summary>
        /// <param name="controlePointage">Contrôle de pointage à mettre à jour</param>
        /// <param name="status">Statut</param>
        /// <returns>Contrôle de pointage mis à jour</returns>
        ControlePointageEnt UpdateControlePointage(ControlePointageEnt controlePointage, int status);

        /// <summary>
        ///   Supprime un contrôle de pointage par son identifiant
        /// </summary>
        /// <param name="controlePointageId">Identifiant de la Contrôle de pointage</param>
        void DeleteControlePointage(int controlePointageId);

        #region Gestion Controle Pointage Erreur 

        /// <summary>
        ///   Ajoute un ControlePointageErreurEnt en base
        /// </summary>
        /// <param name="cpErreur">ControlePointageErreur à ajouter</param>
        /// <returns>ControlePointageErreur ajouté</returns>
        ControlePointageErreurEnt AddControlePointageErreur(ControlePointageErreurEnt cpErreur);

        /// <summary>
        ///   Récupère la liste du personnel avec ses erreurs liées au contrôle pointage
        /// </summary>
        /// <param name="controlePointageId">Identifiant du contrôle pointage</param>
        /// <param name="searchText">Texte à rechercher</param>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <returns>Liste des personnels avec erreurs</returns>
        IEnumerable<PersonnelErreur<ControlePointageErreurEnt>> GetPersonnelErreurList(int controlePointageId, string searchText, int page, int pageSize);

        /// <summary>
        ///   Récupère la liste du personnel avec ses erreurs liées au contrôle pointage
        /// </summary>
        /// <param name="controlePointageId">Identifiant du contrôle pointage</param>
        /// <returns>Liste des personnels avec erreurs</returns>
        IEnumerable<PersonnelErreur<ControlePointageErreurEnt>> GetPersonnelErreurList(int controlePointageId);

        #endregion
    }
}