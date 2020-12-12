using Fred.Entities.Facturation;
using System;
using System.Collections.Generic;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    /// Définit un référentiel de données des facturation.
    /// </summary>
    public interface IFacturationRepository : IRepository<FacturationEnt>
    {
        /// <summary>
        /// Permet d'ajouter des facturations en masse.
        /// </summary>
        /// <param name="facturations">Une liste de facturations.</param>
        void InsertInMass(IEnumerable<FacturationEnt> facturations);

        /// <summary>
        /// Return list id of Depense with invoice
        /// </summary>
        /// <returns>List of Id</returns>
        List<int?> GetIdDepenseAchatWithFacture();

        IReadOnlyList<FacturationEnt> GetExistingNumeroFactureSap(List<string> numFacturesSap);

        IEnumerable<FacturationEnt> GetExistingNumeroFactureSap(int receptionId, decimal montantHt, DateTime dateSaisie);
    }
}
