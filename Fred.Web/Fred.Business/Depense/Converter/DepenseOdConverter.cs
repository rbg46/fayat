using System.Collections.Generic;
using Fred.Entities;
using Fred.Entities.DatesClotureComptable;
using Fred.Entities.OperationDiverse;
using Fred.Web.Shared.Models.Depense;

namespace Fred.Business.Depense.Converter
{
    /// <summary>
    /// Permet de convertir un OperationDiverses en DepenseExhibition
    /// </summary>
    public class DepenseOdConverter
    {
        /// <summary>
        /// Converti une liste d'OperationDiverseEnt en DepenseExhibition
        /// </summary>
        /// <param name="operationDiverses">List de DepenseAchatEnt</param>
        /// <param name="datesClotureComptables">Liste de DateClotureEnt</param>
        /// <returns>Liste de DepenseExhibition</returns>
        public IEnumerable<DepenseExhibition> Convert(List<OperationDiverseEnt> operationDiverses, List<DatesClotureComptableEnt> datesClotureComptables = null)
        {
            List<DepenseExhibition> result = new List<DepenseExhibition>();

            foreach (OperationDiverseEnt od in operationDiverses)
            {
                DepenseExhibition depenseExhibition = Convert(od, datesClotureComptables);
                if (depenseExhibition != null)
                {
                    result.Add(depenseExhibition);
                }
            }
            return result;
        }

        /// <summary>
        /// Converti une liste d'OperationDiverseEnt en DepenseExhibition
        /// </summary>
        /// <param name="od"><see cref="OperationDiverseEnt"/></param>
        /// <param name="datesClotureComptables">Liste de <see cref="DatesClotureComptableEnt"/></param>
        /// <returns><see cref="DepenseExhibition"/></returns>
        private DepenseExhibition Convert(OperationDiverseEnt od, List<DatesClotureComptableEnt> datesClotureComptables)
        {
            DepenseExhibition depenseExhibition = new DepenseExhibition();
            depenseExhibition.Ci = od.CI;
            depenseExhibition.Commentaire = od.Commentaire;
            depenseExhibition.TacheId = od.TacheId;
            depenseExhibition.Tache = od.Tache;
            depenseExhibition.UniteId = od.UniteId;
            depenseExhibition.Unite = od.Unite;
            depenseExhibition.DeviseId = od.DeviseId;
            depenseExhibition.Devise = od.Devise;
            depenseExhibition.TypeDepense = Constantes.DepenseType.OD;
            depenseExhibition.PUHT = od.PUHT;
            depenseExhibition.Quantite = od.Quantite;
            depenseExhibition.MontantHT = od.Montant;
            depenseExhibition.Libelle1 = od.Libelle;
            depenseExhibition.DateDepense = od.DateComptable.Value;
            depenseExhibition.Periode = od.DateComptable.Value;
            depenseExhibition.Id = string.Concat(depenseExhibition.TypeDepense, od.OperationDiverseId.ToString());
            depenseExhibition.DepenseId = od.OperationDiverseId;
            depenseExhibition.RessourceId = od.RessourceId;
            depenseExhibition.Ressource = od.Ressource;
            depenseExhibition.GroupeRemplacementTacheId = od.GroupeRemplacementTacheId ?? 0;

            DatesClotureComptableEnt period = datesClotureComptables?.Find(x => depenseExhibition.Periode.Month == x.Mois && depenseExhibition.Periode.Year == x.Annee && x.DateCloture.HasValue);
            if (period != null)
            {
                depenseExhibition.TacheRemplacable = true;
            }

            return depenseExhibition;
        }

        /// <summary>
        /// Converti une liste d'OperationDiverseEnt en DepenseExhibition pour l'export
        /// </summary>
        /// <param name="operationDiverses">List de DepenseAchatEnt</param>
        /// <param name="datesClotureComptables">Liste de DateClotureEnt</param>
        /// <returns>Liste de DepenseExhibition</returns>
        public IEnumerable<DepenseExhibition> ConvertForExport(List<OperationDiverseEnt> operationDiverses, List<DatesClotureComptableEnt> datesClotureComptables = null)
        {
            List<DepenseExhibition> result = new List<DepenseExhibition>();

            foreach (OperationDiverseEnt od in operationDiverses)
            {
                DepenseExhibition expDep = ConvertForExport(od, datesClotureComptables);
                if (expDep != null)
                {
                    result.Add(expDep);
                }
            }
            return result;
        }

        /// <summary>
        /// Converti une liste d'OperationDiverseEnt en DepenseExhibition pour l'export
        /// </summary>
        /// <param name="od"><see cref="OperationDiverseEnt"/></param>
        /// <param name="datesClotureComptables">Liste de <see cref="DatesClotureComptableEnt"/></param>
        /// <returns><see cref="DepenseExhibition"/></returns>
        private DepenseExhibition ConvertForExport(OperationDiverseEnt od, List<DatesClotureComptableEnt> datesClotureComptables)
        {
            DepenseExhibition depenseExhibition = Convert(od, datesClotureComptables);
            depenseExhibition.RemplacementTaches = od.RemplacementTaches;
            return depenseExhibition;
        }
    }
}
