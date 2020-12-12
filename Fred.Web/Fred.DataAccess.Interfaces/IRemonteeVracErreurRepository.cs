
using System.Collections.Generic;
using Fred.Entities.ValidationPointage;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    ///   Interface IRemonteeVracErreurRepository
    ///   Erreurs liés à la Remontée Vrac dans AS400
    /// </summary>
    public interface IRemonteeVracErreurRepository : IRepository<RemonteeVracErreurEnt>
    {
        /// <summary>
        ///   Récupère la liste des erreurs de remontée vrac en fonction de l'identifiant de la remontée vrac et d'un texte
        /// </summary>
        /// <param name="remonteeVracId">Identifiant de la Remontée Vrac</param>
        /// <param name="searchText">Texte recherché</param>
        /// <returns>Liste des Erreurs de Remontée Vrac</returns>
        IEnumerable<RemonteeVracErreurEnt> Get(int remonteeVracId, string searchText);

        /// <summary>
        ///   Ajoute une Nouvelle Erreur Remontée Vrac
        /// </summary>
        /// <param name="remonteeVracErreur">Erreur Remontée Vrac à ajouter</param>    
        /// <returns>Erreur Remontée Vrac ajoutée</returns>
        RemonteeVracErreurEnt AddRemonteeVracErreur(RemonteeVracErreurEnt remonteeVracErreur);

    }
}
