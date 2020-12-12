using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Business.EcritureComptable;
using Fred.Business.OperationDiverse;
using Fred.Entities.EcritureComptable;
using Fred.Entities.OperationDiverse;
using Fred.Entities.RepartitionEcart;

namespace Fred.Business.RepartitionEcart
{
    /// <summary>
    /// Helper des  RepartitionEcartManager
    /// </summary>
    public class RepartitionEcartCreatorHelper
    {
        /// <summary>
        /// Créer une répartition d'écart
        /// </summary>
        /// <param name="ciID">Identifiant du CI</param>
        /// <param name="dateComptable">Date Comptable</param>
        /// <param name="ods">Liste d'opréation Diverse</param>
        /// <param name="ecritures">Liste d'écriture comptable</param>
        /// <param name="familleOperationDiverse">Famille d'OD</param>
        /// /// <param name="valorisation">Montant de la valorisation</param>
        /// <returns><see cref="RepartitionEcartEnt"/></returns>
        public RepartitionEcartEnt CreateRepartion(int ciID, DateTime dateComptable, IEnumerable<OperationDiverseEnt> ods, IEnumerable<EcritureComptableEnt> ecritures, FamilleOperationDiverseEnt familleOperationDiverse, decimal valorisation)
        {
            RepartitionEcartEnt result = new RepartitionEcartEnt();
            result.DateComptable = dateComptable;
            result.CiId = ciID;
            result.Libelle = familleOperationDiverse.Libelle;
            result.OperationDiverses = ods.Where(q => q.CiId == ciID);
            if (familleOperationDiverse.IsValued)
            {
                result.ValorisationInitiale = valorisation;
            }
            result.ValorisationRectifiee = valorisation + ods.Where(q => q.CiId == ciID).GetMontantTotal();
            result.MontantCapitalise = ecritures.Where(q => q.CiId == ciID).GetMontantTotal();
            result.Ecart = ecritures.Where(q => q.CiId == ciID).GetMontantTotal() - (valorisation + ods.Where(q => q.CiId == ciID).GetMontantTotal());
            result.FamilleOperationDiverse = familleOperationDiverse;
            return result;
        }

        /// <summary>
        /// Permet de créer une liste de répartition d'écart
        /// </summary>
        /// <param name="valorisationByCi">Dictionnaire contenant l'identifiant des CI avec le montant de leur valorisation</param>
        /// <param name="dateComptable">Date comptable de la valorisation</param>
        /// <param name="operationsDiverse">Liste des opérations diverses</param>
        /// <param name="ecritureComptable">Liste des écritures comptable</param>
        /// <param name="famille">Famille de l'opération diverse sur laquelle la répartition doit être créée</param>
        /// <param name="valorisation">Montant total de la valorisation</param>
        /// <returns>Liste de <see cref="RepartitionEcartEnt"/></returns>
        public List<RepartitionEcartEnt> CreateRepartion(Dictionary<int, decimal> valorisationByCi, DateTime dateComptable, IEnumerable<OperationDiverseEnt> operationsDiverse, IEnumerable<EcritureComptableEnt> ecritureComptable, FamilleOperationDiverseEnt famille, Dictionary<int, decimal> valorisation)
        {
            List<RepartitionEcartEnt> result = new List<RepartitionEcartEnt>();

            foreach (var item in valorisationByCi)
            {
                if (valorisation.Count != 0 && valorisation[item.Key] != 0)
                {
                    result.Add(CreateRepartion(item.Key, dateComptable, operationsDiverse, ecritureComptable, famille, valorisation[item.Key]));
                }
                else
                {
                    result.Add(CreateRepartion(item.Key, dateComptable, operationsDiverse, ecritureComptable, famille, item.Value));
                }
            }
            return result;
        }
    }
}
