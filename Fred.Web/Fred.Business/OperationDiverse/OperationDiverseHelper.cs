using System;
using Fred.Entities.EcritureComptable;
using Fred.Entities.OperationDiverse;
using Fred.Entities.Referential;
using Fred.Entities.RepartitionEcart;
using Fred.Framework.DateTimeExtend;
using Fred.Web.Shared.App_LocalResources;

namespace Fred.Business.OperationDiverse
{
    /// <summary>
    /// Helper des OperationDiverseEnt
    /// </summary>
    public static class OperationDiverseHelper
    {
        /// <summary>
        /// Crée un OD d'écart a partir d'une OD
        /// </summary>
        /// <param name="operationDiverse">Opération Diverse</param>
        /// <param name="userId">User Id</param>
        /// <param name="uniteForfait">unité Forfait</param>
        /// <returns>OperationDiverseEnt</returns>
        public static OperationDiverseEnt GenerateOdEcart(OperationDiverseEnt operationDiverse, int userId, UniteEnt uniteForfait)
        {
            OperationDiverseEnt operationDiverseEcart = new OperationDiverseEnt
            {
                CiId = operationDiverse.CiId,
                TacheId = operationDiverse.TacheId,
                RessourceId = operationDiverse.RessourceId,
                AuteurCreationId = userId,
                Cloturee = true,
                DateCloture = DateTime.UtcNow,
                Commentaire = string.Empty,
                DateCreation = DateTime.UtcNow,
                DeviseId = 48,
                FamilleOperationDiverseId = operationDiverse.FamilleOperationDiverseId,
                Libelle = operationDiverse.Libelle,
                Montant = operationDiverse.Montant,
                OdEcart = true,
                UniteId = uniteForfait.UniteId,
                Quantite = 1,
                PUHT = operationDiverse.Montant,
                EcritureComptableId = operationDiverse.EcritureComptableId
            };

            operationDiverseEcart.DateComptable = SetDateComptable(operationDiverseEcart.OdEcart, operationDiverse.DateComptable);

            return operationDiverseEcart;
        }

        /// <summary>
        /// Crée un OD d'écart a partir d'une répartition
        /// </summary>
        /// <param name="repartition">Répartion</param>
        /// <param name="userId">User Id</param>
        /// <param name="uniteForfait">unité Forfait</param>
        /// <returns>OperationDiverseEnt</returns>
        public static OperationDiverseEnt GenerateOdEcart(RepartitionEcartEnt repartition, int userId, UniteEnt uniteForfait)
        {
            OperationDiverseEnt operationDiverseEcart = new OperationDiverseEnt
            {
                CiId = repartition.CiId,
                TacheId = repartition.FamilleOperationDiverse.TacheId,
                RessourceId = repartition.FamilleOperationDiverse.RessourceId,
                AuteurCreationId = userId,
                Cloturee = true,
                DateCloture = DateTime.UtcNow,
                Commentaire = string.Empty,
                DateCreation = DateTime.UtcNow,
                DeviseId = 48,
                FamilleOperationDiverseId = repartition.FamilleOperationDiverse.FamilleOperationDiverseId,
                Libelle = string.Empty,
                Montant = repartition.Ecart,
                OdEcart = true,
                UniteId = uniteForfait.UniteId,
                Quantite = 1,
                PUHT = repartition.Ecart
            };

            operationDiverseEcart.DateComptable = SetDateComptable(operationDiverseEcart.OdEcart, repartition.DateComptable);

            return operationDiverseEcart;
        }

        /// <summary>
        /// Crée un OD d'écart a partir d'une répartition et d'une écriture comptable 
        /// </summary>
        /// <param name="repartition">Répartion</param>
        /// <param name="userId">User Id</param>
        /// <param name="uniteForfait">unité Forfait</param>
        /// <param name="ecritureComptable">Ecriture Comptable</param>
        /// <returns>OperationDiverseEnt</returns>
        public static OperationDiverseEnt GenerateOdEcart(RepartitionEcartEnt repartition, int userId, UniteEnt uniteForfait, EcritureComptableEnt ecritureComptable)
        {
            OperationDiverseEnt operationDiverseEcart = new OperationDiverseEnt
            {
                CiId = ecritureComptable.CiId,
                TacheId = repartition.FamilleOperationDiverse.TacheId,
                RessourceId = repartition.FamilleOperationDiverse.RessourceId,
                AuteurCreationId = userId,
                Cloturee = true,
                DateCloture = DateTime.UtcNow,
                Commentaire = ecritureComptable.Libelle,
                DateCreation = DateTime.UtcNow,
                DeviseId = ecritureComptable.DeviseId,
                FamilleOperationDiverseId = ecritureComptable.FamilleOperationDiverseId,
                Libelle = ecritureComptable.Libelle,
                Montant = ecritureComptable.Montant,
                OdEcart = true,
                UniteId = uniteForfait.UniteId,
                Quantite = 1,
                PUHT = ecritureComptable.Montant,
                EcritureComptableId = ecritureComptable.EcritureComptableId
            };

            operationDiverseEcart.DateComptable = SetDateComptable(operationDiverseEcart.OdEcart, repartition.DateComptable);

            return operationDiverseEcart;
        }

        /// <summary>
        /// Créé une OD inverse d'une OD existante 
        /// </summary>
        /// <param name="repartition">Répartion</param>
        /// <param name="userId">User Id</param>
        /// <param name="uniteForfait">unité Forfait</param>
        /// <param name="operationDiverse">OD</param>
        /// <returns>OD avec un montant et un quantité négatif par rapport à l'OD initiale</returns>
        public static OperationDiverseEnt GenerateRevertedOD(RepartitionEcartEnt repartition, int userId, UniteEnt uniteForfait, OperationDiverseEnt operationDiverse)
        {
            OperationDiverseEnt operationDiverseReverted = new OperationDiverseEnt
            {
                AuteurCreationId = userId,
                CiId = operationDiverse.CiId,
                Cloturee = true,
                DateCloture = DateTime.UtcNow,
                DateCreation = DateTime.UtcNow,
                DeviseId = operationDiverse.DeviseId,
                FamilleOperationDiverseId = operationDiverse.FamilleOperationDiverseId,
                Montant = -operationDiverse.Montant,
                Quantite = -operationDiverse.Quantite,
                TacheId = repartition.FamilleOperationDiverse.TacheId,
                RessourceId = repartition.FamilleOperationDiverse.RessourceId,
                OdEcart = true,
                UniteId = uniteForfait.UniteId,
                Commentaire = operationDiverse.Libelle,
                PUHT = operationDiverse.PUHT,
                Libelle = operationDiverse.Libelle
            };

            operationDiverseReverted.DateComptable = SetDateComptable(operationDiverseReverted.OdEcart, operationDiverse.DateComptable);

            return operationDiverseReverted;
        }

        /// <summary>
        /// Créé une OD inverse d'une OD existante 
        /// </summary>
        /// <param name="operationDiverse">OD</param>
        /// <returns>OD avec un montant et un quantité négatif par rapport à l'OD initiale</returns>
        public static OperationDiverseEnt GenerateRevertedOD(OperationDiverseEnt operationDiverse)
        {
            OperationDiverseEnt operationDiverseReverted = new OperationDiverseEnt
            {
                AuteurCreationId = operationDiverse.AuteurCreationId,
                CiId = operationDiverse.CiId,
                Cloturee = true,
                DateCloture = DateTime.UtcNow,
                DateCreation = DateTime.UtcNow,
                DeviseId = operationDiverse.DeviseId,
                FamilleOperationDiverseId = operationDiverse.FamilleOperationDiverseId,
                Montant = -operationDiverse.Montant,
                Quantite = -operationDiverse.Quantite,
                TacheId = operationDiverse.TacheId,
                RessourceId = operationDiverse.RessourceId,
                OdEcart = true,
                UniteId = operationDiverse.UniteId,
                Commentaire = FeatureOperationDiverse.OperationDiverse_OD_Annulation_GenererAutomatique,
                PUHT = operationDiverse.PUHT,
                Libelle = operationDiverse.Libelle,
                EcritureComptableId = operationDiverse.EcritureComptableId
            };

            operationDiverseReverted.DateComptable = SetDateComptable(operationDiverseReverted.OdEcart, operationDiverse.DateComptable);

            return operationDiverseReverted;
        }

        public static DateTime SetDateComptable(bool odEcart, DateTime? dateComptable)
        {
            return odEcart
                ? dateComptable.Value.GetLimitsOfMonth().EndDate.Date
                : new DateTime(dateComptable.Value.Year, dateComptable.Value.Month, 15);
        }
    }
}
