using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Business.Images;
using Fred.Business.Personnel;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Framework.Models.Reporting;
using Fred.Framework.Reporting;
using Fred.Framework.Reporting.SyncFusion.Excel;
using Fred.Web.Shared.App_LocalResources;
using Syncfusion.XlsIO;

namespace Fred.Business.Budget.BudgetComparaison.ExcelExport
{
    /// <summary>
    /// Permet d'exporter en Excel une comparaison de budget.
    /// </summary>
    public class BudgetComparaisonExcelExporter : SimpleManagerFeature, IBudgetComparaisonExcelExporter
    {
        #region Membres

        private const string UniteMultiple = "#";
        private const string SansUnite = "-";

        private readonly IBudgetComparer budgetComparer;
        private readonly IUtilisateurManager utilisateurManager;
        private readonly IImageManager imageManager;
        private readonly IPersonnelManager personnelManager;
        private readonly IBudgetRepository budgetRepository;
        private readonly IPersonnelRepository personnelRepository;
        private readonly Dto.Comparaison.Result.UniteDto sansUnite;

        private Dto.ExcelExport.RequestDto request;
        private Dto.ExcelExport.ResultDto result;
        private Dto.Comparaison.ResultDto comparaisonResult;
        private IWorksheet worksheet;
        private int currentNodeRowIndex;
        private BestIndexHeaderExcelModel headerIndex;
        private BudgetComparaisonExcelStyles styles;
        private BudgetComparaisonExcelPositions positions;

        #endregion
        #region Constructeur

        /// <summary>
        /// Constructeur.
        /// </summary>
        /// <param name="budgetComparer">Comparateur de budget.</param>
        /// <param name="utilisateurManager">Gestionnaire des utilisateurs.</param>
        /// <param name="imageManager">Gestionnaire des images.</param>
        /// <param name="budgetRepository">Référentiel de données pour les budgets.</param>
        /// <param name="personnelRepository">Référentiel de données pour les personnels.</param>
        public BudgetComparaisonExcelExporter(
            IBudgetComparer budgetComparer,
            IUtilisateurManager utilisateurManager,
            IImageManager imageManager,
            IPersonnelManager personnelManager,
            IBudgetRepository budgetRepository,
            IPersonnelRepository personnelRepository)
        {
            this.budgetComparer = budgetComparer;
            this.utilisateurManager = utilisateurManager;
            this.imageManager = imageManager;
            this.personnelManager = personnelManager;
            this.budgetRepository = budgetRepository;
            this.personnelRepository = personnelRepository;

            sansUnite = new Dto.Comparaison.Result.UniteDto
            {
                UniteId = 0,
                Code = FeatureBudgetComparaison.BudgetComparaison_SansUnite_Code,
                Libelle = FeatureBudgetComparaison.BudgetComparaison_SansUnite_Libelle
            };
        }

        #endregion
        #region Export

        /// <summary>
        /// Exporte la comparaison de budget.
        /// </summary>
        /// <param name="request">La requête de l'export Excel.</param>
        /// <returns>Le résultat de l'export.</returns>
        public Dto.ExcelExport.ResultDto Export(Dto.ExcelExport.RequestDto request)
        {
            this.request = request;
            result = new Dto.ExcelExport.ResultDto();
            if (!Compare())
            {
                return result;
            }

            using (var excelFormat = new ExcelFormat())
            {
                var workbook = excelFormat.GetNewWorbook();
                worksheet = workbook.Worksheets[0];

                Initialize();
                AddSubHeader();
                AddHeader();
                AddNodes();
                AddTotal();
                AddBanner(excelFormat);

                result.Data = excelFormat.ConvertToByte(workbook);
            }

            return result;
        }

        /// <summary>
        /// Compare les budgets.
        /// </summary>
        /// <returns>True si la comparaison a réussi, false sinon.</returns>
        private bool Compare()
        {
            comparaisonResult = budgetComparer.Compare(new Dto.Comparaison.RequestDto
            {
                BudgetId1 = request.BudgetId1,
                BudgetId2 = request.BudgetId2,
                Axes = request.Axes
            });
            if (!string.IsNullOrEmpty(comparaisonResult.Erreur))
            {
                result.Erreur = comparaisonResult.Erreur;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Initialise l'export.
        /// </summary>
        private void Initialize()
        {
            styles = new BudgetComparaisonExcelStyles(worksheet);

            var index = 1;
            positions = new BudgetComparaisonExcelPositions();
            positions.AxeColumn = new ExcelColumn(worksheet, index++);
            styles.Separator.Apply(new ExcelColumn(worksheet, index++), 1, null);
            positions.Budget1Columns = GetGroupColumns(request.Colonnes.Budget1, ref index);
            styles.Separator.Apply(new ExcelColumn(worksheet, index++), 1, null);
            positions.Budget2Columns = GetGroupColumns(request.Colonnes.Budget2, ref index);
            styles.Separator.Apply(new ExcelColumn(worksheet, index++), 1, null);
            positions.EcartColumns = GetGroupColumns(request.Colonnes.Ecart, ref index);

            headerIndex = new BestIndexHeaderExcelModel(21, 48, 34, 17, positions.EcartColumns.Montant.Index);
        }

        /// <summary>
        /// Retourne les positions des colonnes d'un groupe.
        /// </summary>
        /// <param name="colonnes">Les colonnes du groupe.</param>
        /// <param name="index">L'index de la colonne de départ du groupe.</param>
        /// <returns>Les positions des colonnes du groupe.</returns>
        private BudgetComparaisonExcelGroupColumns GetGroupColumns(Dto.ExcelExport.Request.GroupColonnesDto colonnes, ref int index)
        {
            int quantite = 0, unite = 0, prixUnitaire = 0;
            int start = index;
            if (colonnes.HasQuantite)
            {
                quantite = index++;
            }
            if (colonnes.HasUnite)
            {
                unite = index++;
            }
            if (colonnes.HasPrixUnitaire)
            {
                prixUnitaire = index++;
            }
            return new BudgetComparaisonExcelGroupColumns(worksheet, start, index, quantite, unite, prixUnitaire, index++);
        }

        #endregion
        #region Header

        /// <summary>
        /// Ajoute le sous-entête.
        /// </summary>
        private void AddSubHeader()
        {
            styles.Budget1.SubHeader.Apply(positions.Budget1Columns, BudgetComparaisonExcelPositions.SubHeaderRowIndex, FeatureBudgetComparaison.BudgetComparaison_TableHeader_Budget1);
            styles.Budget2.SubHeader.Apply(positions.Budget2Columns, BudgetComparaisonExcelPositions.SubHeaderRowIndex, FeatureBudgetComparaison.BudgetComparaison_TableHeader_Budget2);
            styles.Ecart.SubHeader.Apply(positions.EcartColumns, BudgetComparaisonExcelPositions.SubHeaderRowIndex, FeatureBudgetComparaison.BudgetComparaison_TableHeader_Ecart);
        }

        /// <summary>
        /// Ajoute l'entête.
        /// </summary>
        private void AddHeader()
        {
            string text;
            switch (request.AxePrincipal)
            {
                case Dto.AxePrincipalType.TacheRessource:
                    text = FeatureBudgetComparaison.BudgetComparaison_FilterPanel_AxeTacheRessource;
                    break;
                case Dto.AxePrincipalType.RessourceTache:
                    text = FeatureBudgetComparaison.BudgetComparaison_FilterPanel_AxeRessourceTache;
                    break;
                default:
                    throw new NotImplementedException();
            }
            styles.Axe.Header.Apply(positions.AxeColumn, BudgetComparaisonExcelPositions.HeaderRowIndex, text);

            AddHeader(positions.Budget1Columns, styles.Budget1.Header);
            AddHeader(positions.Budget2Columns, styles.Budget2.Header);
            AddHeader(positions.EcartColumns, styles.Ecart.Header);
        }

        /// <summary>
        /// Ajoute un entête pour un groupe.
        /// </summary>
        /// <param name="columns">Les positions des colonnes du groupe.</param>
        /// <param name="style">Le style du groupe.</param>
        private void AddHeader(BudgetComparaisonExcelGroupColumns columns, ExcelBasicStyle style)
        {
            if (columns.Quantite.Index > 0)
            {
                style.Apply(columns.Quantite, BudgetComparaisonExcelPositions.HeaderRowIndex, FeatureBudgetComparaison.BudgetComparaison_TableHeader_Quantite);
            }
            if (columns.Unite.Index > 0)
            {
                style.Apply(columns.Unite, BudgetComparaisonExcelPositions.HeaderRowIndex, FeatureBudgetComparaison.BudgetComparaison_TableHeader_Unite);
            }
            if (columns.PrixUnitaire.Index > 0)
            {
                style.Apply(columns.PrixUnitaire, BudgetComparaisonExcelPositions.HeaderRowIndex, FeatureBudgetComparaison.BudgetComparaison_TableHeader_PrixUnitaire);
            }
            style.Apply(columns.Montant, BudgetComparaisonExcelPositions.HeaderRowIndex, FeatureBudgetComparaison.BudgetComparaison_TableHeader_Montant);
        }

        #endregion
        #region Noeuds

        /// <summary>
        /// Ajoute les noeuds.
        /// </summary>
        private void AddNodes()
        {
            currentNodeRowIndex = BudgetComparaisonExcelPositions.FirstNodeRowIndex - 1;
            foreach (var node in comparaisonResult.Tree.Nodes)
            {
                AddNode(node, 0);
            }

            // Applique le style sur le dernier noeud
            styles.LastNode.Apply(positions.AxeColumn, currentNodeRowIndex, null);
            styles.LastNode.Apply(positions.Budget1Columns, currentNodeRowIndex, null);
            styles.LastNode.Apply(positions.Budget2Columns, currentNodeRowIndex, null);
            styles.LastNode.Apply(positions.EcartColumns, currentNodeRowIndex, null);
        }

        /// <summary>
        /// Ajoute un noeud et ses enfants.
        /// </summary>
        /// <param name="node">Le noeud concerné.</param>
        /// <param name="nodeIndex">L'index hiérarchique du noeud.</param>
        private void AddNode(Dto.Comparaison.Result.NodeDto node, int nodeIndex)
        {
            currentNodeRowIndex++;

            styles.Axe.Apply(positions.AxeColumn, currentNodeRowIndex, request.Axes[nodeIndex], nodeIndex, $"{node.Code} - {node.Libelle}");
            AddGroup(node.Budget1, node.Ecart, positions.Budget1Columns, styles.Budget1.Items);
            AddGroup(node.Budget2, node.Ecart, positions.Budget2Columns, styles.Budget2.Items);
            AddGroup(node.Ecart, null, positions.EcartColumns, styles.Ecart.Items);

            foreach (var subNode in node.Nodes)
            {
                AddNode(subNode, nodeIndex + 1);
            }
        }

        /// <summary>
        /// Ajoute un groupe.
        /// </summary>
        /// <param name="group">Le groupe à ajouter.</param>
        /// <param name="ecartGroup">Le groupe des écarts.</param>
        /// <param name="columns">Les positions des colonnes du groupe.</param>
        /// <param name="style">Le style des items du groupe.</param>
        private void AddGroup(
            Dto.Comparaison.Result.GroupDto group,
            Dto.Comparaison.Result.GroupDto ecartGroup,
            BudgetComparaisonExcelGroupColumns columns,
            BudgetComparaisonExcelGroupItemsStyle style)
        {
            if (columns.Quantite.Index > 0)
            {
                style.Quantite.Apply(columns.Quantite, currentNodeRowIndex, ecartGroup?.Quantite != null, group.Quantite?.ToString());
            }
            if (columns.Unite.Index > 0)
            {
                var unites = GetGroupUnites(group);
                style.Unite.Apply(columns.Unite, currentNodeRowIndex, ecartGroup != null && ecartGroup.UniteIds.Count > 0, GetUnite(unites));
            }
            if (columns.PrixUnitaire.Index > 0)
            {
                style.PrixUnitaire.Apply(columns.PrixUnitaire, currentNodeRowIndex, ecartGroup?.PrixUnitaire != null, group.PrixUnitaire?.ToString());
            }
            style.Montant.Apply(columns.Montant, currentNodeRowIndex, ecartGroup?.Montant != null, group.Montant?.ToString());
        }

        /// <summary>
        /// Retourne les unités triées d'un groupe.
        /// </summary>
        /// <param name="group">Le groupe concerné.</param>
        /// <returns>Les unités triées du groupe.</returns>
        private List<Dto.Comparaison.Result.UniteDto> GetGroupUnites(Dto.Comparaison.Result.GroupDto group)
        {
            var hasSansUnite = false;
            var ret = new List<Dto.Comparaison.Result.UniteDto>(group.UniteIds.Count);
            foreach (var uniteId in group.UniteIds)
            {
                if (uniteId == null)
                {
                    hasSansUnite = true;
                }
                else
                {
                    ret.Add(comparaisonResult.Unites.FirstOrDefault(u => u.UniteId == uniteId));
                }
            }
            ret = ret.OrderBy(u => u.Code).ToList();
            if (hasSansUnite)
            {
                ret.Add(sansUnite);
            }
            return ret;
        }

        /// <summary>
        /// Récupère l'unité à afficher.
        /// </summary>
        /// <param name="unites">Les unités concernées.</param>
        /// <returns>L'unité à afficher.</returns>
        private string GetUnite(List<Dto.Comparaison.Result.UniteDto> unites)
        {
            if (unites.Count == 0)
            {
                return string.Empty;
            }
            if (unites.Count == 1)
            {
                return unites[0]?.Code ?? SansUnite;
            }
            return UniteMultiple;
        }

        #endregion
        #region Total

        /// <summary>
        /// Ajoute le total.
        /// </summary>
        private void AddTotal()
        {
            currentNodeRowIndex++;
            styles.Total.Libelle.Apply(positions.Budget2Columns.Montant, currentNodeRowIndex, FeatureBudgetComparaison.BudgetComparaison_TotalEcart);
            AddGroup(comparaisonResult.EcartTotal, null, positions.EcartColumns, styles.Total.EcartItems);
        }

        #endregion
        #region Bannière

        /// <summary>
        /// Ajoute la bannière.
        /// </summary>
        /// <param name="excelFormat">Générateur de document excel.</param>
        private void AddBanner(ExcelFormat excelFormat)
        {
            var now = DateTime.Now;
            var affaire = budgetRepository.GetBudget(request.BudgetId1, b => new { CodeLibelle = b.Ci.Code + " - " + b.Ci.Libelle });
            var utilisateurId = utilisateurManager.GetContextUtilisateurId();
            var editeur = personnelRepository.GetPersonnelPourExportExcelHeader(utilisateurId);
            var revisions = budgetRepository.GetBudgetRevisionsPourBudgetComparaisonExportExcel(request.BudgetId1, request.BudgetId2);
            var revisionBudget1 = string.Format(FeatureBudgetComparaison.BudgetComparaison_ExportExcel_Revision, 1, revisions.Budget1.Revision, revisions.Budget1.Etat);
            var revisionBudget2 = string.Format(FeatureBudgetComparaison.BudgetComparaison_ExportExcel_Revision, 2, revisions.Budget2.Revision, revisions.Budget2.Etat);
            var buildHeaderModel = new BuildHeaderExcelModel(
                FeatureBudgetComparaison.BudgetComparaison_Titre,
                affaire.CodeLibelle,
                BusinessResources.Export_Header_DateEdition + now.ToShortDateString() + " " + now.ToShortTimeString(),
                BusinessResources.Export_Header_EditePar + personnelManager.GetPersonnelMatriculeNomPrenom(editeur),
                revisionBudget1 + Environment.NewLine + revisionBudget2,
                editeur.SocieteId != null ? AppDomain.CurrentDomain.BaseDirectory + imageManager.GetLogoImage(editeur.SocieteId.Value).Path : null,
                headerIndex);
            excelFormat.BuildHeader(worksheet, buildHeaderModel, false);
        }

        #endregion
    }
}
