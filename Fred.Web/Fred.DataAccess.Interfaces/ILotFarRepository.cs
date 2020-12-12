
using System.Collections.Generic;
using Fred.Entities.Depense;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    ///   Représente un référentiel de données pour les lot de FARs/Depenses
    /// </summary>
    public interface ILotFarRepository : IRepository<LotFarEnt>
    {
        /// <summary>
        ///   Récupération de tous les lots de FARs
        /// </summary>
        /// <returns>Liste des Lots de FARs</returns>
        IEnumerable<LotFarEnt> GetAll();

        /// <summary>
        ///   Récupération d'un Lot de FARs par son identifiant
        /// </summary>
        /// <param name="lotFarId">Identifiant du Lot de FARs</param>
        /// <returns>Lot de FARs</returns>
        LotFarEnt GetById(int lotFarId);

        /// <summary>
        ///   Récupération d'un Lot de FARs par son numéro de lot
        /// </summary>
        /// <param name="numLot">Numéro du Lot de FARs</param>
        /// <returns>Lot de FARs</returns>
        LotFarEnt GetByNumeroLot(int numLot);

        /// <summary>
        ///   Ajout d'un nouveau lot de FARs
        /// </summary>
        /// <param name="lotFar">Lot de FARs à ajouter</param>
        /// <returns>Lot de FARs ajouté</returns>
        LotFarEnt AddLotFar(LotFarEnt lotFar);
    }
}