using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fred.Business.OperationDiverse.ImportODfromExcel.Selectors;
using Fred.Business.OperationDiverse.ImportODfromExcel.ContextProvider;
using Fred.Business.RepriseDonnees.Common.Selector;
using Fred.Entities.OperationDiverse;
using Fred.Entities.OperationDiverse.Excel;

namespace Fred.Business.OperationDiverse.ImportODfromExcel.Mapper
{
    /// <summary>
    /// permet de transformer un model excel vers une operation diverse entite
    /// </summary>
    public class OperationDiverseDataMapper : IOperationDiverseDataMapper
    {
        /// <summary>
        /// returne la liste des rapports à créer 
        /// </summary>
        /// <param name="context">context contenant les data necessaire a l'import</param>
        /// <param name="excelOperationDiverses">les operations diverses sous la forme excel</param>
        /// <returns>Liste des operations diverses à créer</returns>
        public List<OperationDiverseEnt> Transform(ContextForImportOD context, List<ExcelOdModel> excelOperationDiverses)
        {
            var result = new List<OperationDiverseEnt>();
            var commonFieldSelector = new CommonFieldSelector();
            var operationDiverseSelectors = new OperationDiverseSelectors();
            foreach (var operationDiverseExcel in excelOperationDiverses)
            {
                var ci = commonFieldSelector.GetCiOfDatabase(context.OrganisationTree, context.GroupeId, operationDiverseExcel.CodeSociete, operationDiverseExcel.CodeCi, context.CisUsedInExcel);
                var dateComtable = operationDiverseExcel.DateComptable;
                var newOD = new OperationDiverseEnt
                {
                    Libelle = operationDiverseExcel.Libelle,
                    Commentaire = operationDiverseExcel.Commentaire,
                    CiId = ci.CiId,
                    TacheId = commonFieldSelector.GetTache(ci.CiId, operationDiverseExcel.CodeTache, context.TachesUsedInExcel).TacheId,
                    PUHT = commonFieldSelector.GetDecimal(operationDiverseExcel.PuHT),
                    Quantite = commonFieldSelector.GetDecimal(operationDiverseExcel.Quantite),
                    UniteId = operationDiverseSelectors.GetUnite(operationDiverseExcel.CodeUnite,context).UniteId,
                    DeviseId = operationDiverseSelectors.GetDevise(operationDiverseExcel.CodeDevise,context).DeviseId,
                    DateComptable = !string.IsNullOrEmpty(dateComtable) ? commonFieldSelector.GetDate(dateComtable):context.DateComtableFromUI,
                    AuteurCreationId = context.User.UtilisateurId,
                    DateCreation = DateTime.UtcNow,
                    FamilleOperationDiverseId = operationDiverseSelectors.GetFamilleOD(operationDiverseExcel.CodeSociete,operationDiverseExcel.CodeFamille,context).FamilleOperationDiverseId,
                    RessourceId = operationDiverseSelectors.GetRessource(operationDiverseExcel.CodeRessource,context).RessourceId,
                    Cloturee = false,
                    DateCloture = null,
                    OdEcart = false,
                    GroupeRemplacementTacheId = null,
                    EcritureComptableId = null,
                };
                newOD.Montant = newOD.PUHT * newOD.Quantite;
                result.Add(newOD);
            }
            return result;
        }

    }
}
