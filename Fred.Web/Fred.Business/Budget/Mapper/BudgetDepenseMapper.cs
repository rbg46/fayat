using System.Collections.Generic;
using System.Linq;
using Fred.Web.Shared.Models.Budget.Depense;
using Fred.Web.Shared.Models.Depense;

namespace Fred.Business.Budget.Mapper
{
    /// <summary>
    /// Classe de mapping des BudgetDepenseModel
    /// </summary>
    public static class BudgetDepenseMapper
    {
        /// <summary>
        /// Mapping des DepenseExhibition vers les BudgetDepenseModel
        /// </summary>
        /// <param name="depenses">Liste de DepenseExhibitions</param>
        /// <returns>Liste de BudgetDepenseModel</returns>
        public static IEnumerable<BudgetDepenseModel> Map(IEnumerable<DepenseExhibition> depenses)
        {
            if (depenses == null)
            {
                return Enumerable.Empty<BudgetDepenseModel>();
            }

            return depenses.Select(x =>
                new BudgetDepenseModel
                {
                    Ci = x.Ci,
                    Id = x.Id,
                    RessourceId = x.RessourceId,
                    Ressource = x.Ressource,
                    TacheId = x.TacheId,
                    Tache = x.Tache,
                    Libelle1 = x.Libelle1,
                    UniteId = x.UniteId,
                    Unite = x.Unite,
                    Quantite = x.Quantite,
                    PUHT = x.PUHT,
                    MontantHT = x.MontantHT,
                    DeviseId = x.DeviseId,
                    Devise = x.Devise,
                    Code = x.Code,
                    Libelle2 = x.Libelle2,
                    Commentaire = x.Commentaire,
                    DateComptableRemplacement = x.DateComptableRemplacement,
                    DateDepense = x.DateDepense,
                    Periode = x.Periode,
                    NatureId = x.NatureId,
                    Nature = x.Nature,
                    TypeDepense = x.TypeDepense,
                    SousTypeDepense = x.SousTypeDepense,
                    DateRapprochement = x.DateRapprochement,
                    DateFacture = x.DateFacture,
                    NumeroFacture = x.NumeroFacture,
                    MontantFacture = x.MontantFacture,
                    MontantHtInitial = x.MontantHtInitial,
                    CommandeId = x.CommandeId,
                    IsEnergie = x.IsEnergie,
                    TacheRemplacable = x.TacheRemplacable,
                    DepenseVisee = x.DepenseVisee,
                    DepenseId = x.DepenseId,
                    GroupeRemplacementTacheId = x.GroupeRemplacementTacheId,
                    TacheOrigineCodeLibelle = x.TacheOrigineCodeLibelle,
                    TacheOrigineId = x.TacheOrigineId,
                    TacheOrigine = x.TacheOrigine,
                    Personnel = x.Personnel,
                    FournisseurId = x.FournisseurId,
                    AgenceId = x.AgenceId,
                    RemplacementTaches = x.RemplacementTaches,
                    SoldeFar = x.SoldeFar
                });
        }
    }
}
