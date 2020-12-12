
using System.Collections.Generic;
using Fred.Entities.ValidationPointage;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    ///   Interface IControlePointageRepository
    /// </summary>
    public interface IControlePointageRepository : IRepository<ControlePointageEnt>
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
        /// <param name="typeControle">Type de la contrôle de pointage (Controle Chantier, Controle Vrac, Remontee Vrac)</param>
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
        ///   Récupère la liste des derniers contrôles de pointages par identifiant du lot de pointage (Une ControlePointageEnt par type de contrôle)
        ///   Rappel : typeControle = [Contrôle Chantier, Contrôle Vrac, Remontée Vrac]
        /// </summary>
        /// <param name="listLotsPointagesIds">Liste des identifiant du lot de pointage</param>
        /// <returns>Liste des contrôles de pointage filtré</returns>
        IEnumerable<ControlePointageEnt> GetLatestList(List<int> listLotsPointagesIds);

        /// <summary>
        ///   Récupère la liste des contrôles de pointages par identifiant du Lot de pointage et par type de contrôle
        /// </summary>
        /// <param name="lotPointageId">Identifiant du lot de pointage</param>
        /// <param name="typeControle">Type de la contrôle de pointage (Controle Chantier, Controle Vrac, Remontee Vrac)</param>
        /// <returns>Liste des contrôles de pointage filtré</returns>
        IEnumerable<ControlePointageEnt> GetList(int lotPointageId, int typeControle);

        /// <summary>
        ///   Ajoute une nouvelle contrôle de pointage
        /// </summary>
        /// <param name="controlePointage">Contrôle de pointage a ajouter</param>
        /// <returns>Contrôle de pointage ajouté</returns>
        ControlePointageEnt AddControlePointage(ControlePointageEnt controlePointage);

        /// <summary>
        ///   Mise à jout de la Contrôle de pointage
        /// </summary>
        /// <param name="controlePointage">Contrôle de pointage à mettre à jour</param>
        /// <returns>Contrôle de pointage mis à jour</returns>
        ControlePointageEnt UpdateControlePointage(ControlePointageEnt controlePointage);

        /// <summary>
        ///   Supprime une contrôle de pointage par son identifiant
        /// </summary>
        /// <param name="controlePointageId">Identifiant de la Contrôle de pointage</param>
        void DeleteControlePointage(int controlePointageId);
    }
}
