using Fred.Entities.Facturation;
using Fred.Web.Shared.Models.Facture;
using System;
using System.Collections.Generic;

namespace Fred.Business.Facturation
{
    /// <summary>
    /// Définit du gestionnaire des facturation
    /// </summary>
    public interface IFacturationManager : IManager<FacturationEnt>
    {
        /// <summary>
        /// Permet d'ajouter une liste de facturation.
        /// </summary>
        /// <param name="facturations">Une liste de facturations.</param>
        void BulkInsert(IEnumerable<FacturationEnt> facturations);

        /// <summary>
        /// Récupération d'une liste de facturations par numéro de facture SAP
        /// </summary>
        /// <param name="numFactureSap">Numéro de facture SAP</param>
        /// <returns>Liste de Facturations</returns>
        IEnumerable<FacturationEnt> GetList(string numFactureSap);

        IReadOnlyList<FactureEcritureComptableModel> GetExistingNumeroFactureSap(List<string> numFacturesSap);

        /// <summary> 
        /// Récupération d'une liste de facturations par receptionId, montant et date de saisie
        /// </summary>
        /// <param name="receptionId">receptionid</param>
        /// <param name="montantHt">montant de la facturation</param>
        /// <param name="dateSaisie">date de saisie</param>
        /// <returns>Liste de Facturations</returns>
        IEnumerable<FacturationEnt> GetListByReceptionID(int receptionId, decimal montantHt, DateTime dateSaisie);

        /// <summary>
        /// Return list id of Depense with invoice
        /// </summary>
        /// <returns>List Of Id</returns>
        List<int?> GetIdDepenseAchatWithFacture();
    }
}
