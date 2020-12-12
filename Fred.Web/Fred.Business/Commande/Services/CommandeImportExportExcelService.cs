using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using AutoMapper;
using Fred.Business.CI;
using Fred.Business.Commande.Models;
using Fred.Business.Commande.Reporting;
using Fred.Business.Referential.Tache;
using Fred.Business.ReferentielFixe;
using Fred.Business.Unite;
using Fred.Framework.Exceptions;
using Fred.Framework.Extensions;
using Fred.Web.Models.Referential;
using Fred.Web.Models.ReferentielFixe;
using Fred.Web.Shared.App_LocalResources;

namespace Fred.Business.Commande.Services
{
    public class CommandeImportExportExcelService : ICommandeImportExportExcelService
    {
        private readonly IUniteManager uniteManager;
        private readonly ICIManager ciManager;
        private readonly ITacheManager tacheManager;
        private readonly IReferentielFixeManager referentielFixeManager;
        private readonly IMapper mapper;

        public CommandeImportExportExcelService(
            IUniteManager uniteManager,
            ICIManager ciManager,
            ITacheManager tacheManager,
            IReferentielFixeManager referentielFixeManager,
            IMapper mapper)
        {
            this.uniteManager = uniteManager;
            this.ciManager = ciManager;
            this.tacheManager = tacheManager;
            this.referentielFixeManager = referentielFixeManager;
            this.mapper = mapper;
        }

        /// <summary>
        /// Generate File Excel for Lignes Ordered
        /// </summary>
        /// <param name="ciId">Id CI</param>
        /// <param name="isAvenant">Type Avenant</param>
        /// <returns>retourne un object </returns>
        public ImportResultImportLignesCommande GenerateExempleExcel(int ciId, bool isAvenant)
        {
            try
            {
                var ciEnt = ciManager.GetCiById(ciId, true);
                string societeCode = ciEnt.Societe?.Code;

                List<RessourceModel> ressourceModel = mapper.Map<IEnumerable<RessourceModel>>(referentielFixeManager.SearchRessourcesRecommandees(string.Empty, ciEnt, 1, int.MaxValue, false, null)).ToList();
                ressourceModel.ToList().ForEach(m => m.SocieteCode = societeCode);

                // On ne récupère que les tâches de niveau 3, car les niveau 1 et 2 sont des niveaux d'organisation seulement
                List<TacheModel> tacheModel = mapper.Map<IEnumerable<TacheModel>>(tacheManager.GetTacheListByCiIdAndNiveau(ciId, 3)).ToList();
                tacheModel.ToList().ForEach(m => m.SocieteCode = societeCode);

                List<UniteModel> uniteModel = mapper.Map<IEnumerable<UniteModel>>(uniteManager.SearchLight(string.Empty, 1, int.MaxValue)).ToList();

                CommandeExcelLignesCommande cmdExcel = new CommandeExcelLignesCommande();
                byte[] excelBytes = cmdExcel.CreateExempleCommandeLignes(ressourceModel, tacheModel, uniteModel, isAvenant);
                return GetIdMemoryCache(excelBytes);
            }
            catch (Exception ex)
            {
                throw new FredBusinessException(FeatureExportExcel.Invalid_FileFormat, ex);
            }
        }

        private ImportResultImportLignesCommande GetIdMemoryCache(byte[] excelBytes)
        {
            var policy = new CacheItemPolicy { AbsoluteExpiration = DateTimeOffset.Now.AddSeconds(30), Priority = CacheItemPriority.NotRemovable };
            var cacheId = Guid.NewGuid().ToString();
            MemoryCache.Default.Add("excelBytes_" + cacheId, excelBytes, policy);

            return new ImportResultImportLignesCommande { Id = cacheId };
        }

        /// <summary>
        /// Generate File Excel for Lignes Ordered
        /// </summary>
        /// <param name="checkinValue">valeur à checker</param>
        /// <param name="ciId">id Ci</param>
        /// <param name="stream">File format Stream</param>
        /// <param name="isAvenant">Is Avenant</param>
        /// <returns>retourne un object </returns>
        public ImportResultImportLignesCommande ImportCommandeLignes(string checkinValue, int ciId, Stream stream, bool isAvenant)
        {
            ImportResultImportLignesCommande result = new ImportResultImportLignesCommande();

            CommandeExcelLignesCommande cmdExcel = new CommandeExcelLignesCommande();

            try
            {
                var ciEnt = ciManager.GetCiById(ciId, true);

                // Recuperation des données de la feuille excel.
                List<ExcelLigneCommandeModel> parsageResult = cmdExcel.ParseExcelFile(stream, isAvenant);
                List<RessourceModel> ressourceModel = mapper.Map<IEnumerable<RessourceModel>>(referentielFixeManager.SearchRessourcesRecommandees(string.Empty, ciEnt, 1, int.MaxValue, false, null)).ToList();
                List<TacheModel> tacheModel = mapper.Map<IEnumerable<TacheModel>>(tacheManager.GetTacheListByCiId(ciId, false)).ToList();
                List<UniteModel> uniteModel = mapper.Map<IEnumerable<UniteModel>>(uniteManager.SearchLight(string.Empty, 1, int.MaxValue)).ToList();

                // verification des règles(RG) de l'import de commande lignes.
                List<ExcelLigneCommandeModel> importRapportsRulesResult = VerifyImportRules(parsageResult, checkinValue, ressourceModel, tacheModel, uniteModel, isAvenant);

                if (!importRapportsRulesResult.Any(x => x.Erreurs.Count != 0))
                {
                    // Ici je mets les valeurs recu du fichier excel    
                    importRapportsRulesResult.ForEach(
                        x => result.CommandeLignes.Add(new LigneCommandeImportModel()
                        {
                            Lignecommande = int.Parse(x.NumeroDeLigne),
                            Libelle = x.DesignationLigneCommande,
                            Ressource = ressourceModel.Find(r => r.Code == x.CodeRessource),
                            Tache = tacheModel.Find(t => t.Code == x.CodeTache),
                            PuHT = decimal.Parse(!x.PuHt.IsNullOrEmpty() ? x.PuHt : "0"),
                            Unite = uniteModel.Find(u => u.Code == x.Unite),
                            Quantite = decimal.Parse(!x.QuantiteCommande.IsNullOrEmpty() ? x.QuantiteCommande : "0"),
                            IsDiminution = !x.IsDiminution.IsNullOrEmpty() && string.Equals(x.IsDiminution, FredResource.Global_Oui, StringComparison.OrdinalIgnoreCase)
                        })
                        );
                }
                else
                {
                    result.ErrorMessages = importRapportsRulesResult.Where(x => x.Erreurs.Count > 0).Select(x => x.Erreurs).ToList();
                }
                return result;
            }
            catch (Exception ex)
            {
                throw new FredBusinessException(FeatureExportExcel.Invalid_FileFormat, ex);
            }
        }

        private List<ExcelLigneCommandeModel> VerifyImportRules(List<ExcelLigneCommandeModel> parsageResult, string checkinValue,
            List<RessourceModel> ressources,
            List<TacheModel> taches,
            List<UniteModel> unites,
            bool isAvenant)
        {
            foreach (ExcelLigneCommandeModel ligneCmd in parsageResult)
            {
                if (isAvenant)
                {
                    ValidateNumeroCommandeLigne(checkinValue, ligneCmd);
                }

                ValidateLibelle(ligneCmd);
                ValidateRessource(ressources, ligneCmd);
                ValidateTache(taches, ligneCmd);
                ValidatePuHT(ligneCmd);
                ValidateUnite(unites, ligneCmd);
                ValidateQuantite(ligneCmd);
                ValidateIsDiminution(ligneCmd);
            }
            return parsageResult;
        }

        private void ValidateNumeroCommandeLigne(string checkinValue, ExcelLigneCommandeModel ligneCmd)
        {
            //Validate designation
            if (!ligneCmd.NumeroComande.IsNullOrEmpty())
            {
                if (checkinValue != ligneCmd.NumeroComande)
                {
                    AddErreurLigne(ligneCmd, FeatureCommande.Commande_Popin_ImportLignes_Erreur_NumCommande);
                }
            }
            else
            {
                AddErreurLigne(ligneCmd, string.Format(FeatureCommande.Commande_Popin_ImportLignes_Erreur_ChampObligatoire, FeatureCommande.Commande_Index_Table_NumeroCommande));
            }
        }

        private void ValidateLibelle(ExcelLigneCommandeModel ligneCmd)
        {
            //Validate designation
            if (ligneCmd.DesignationLigneCommande.IsNullOrEmpty())
            {
                AddErreurLigne(ligneCmd, string.Format(FeatureCommande.Commande_Popin_ImportLignes_Erreur_ChampObligatoire, FeatureCommande.Commande_Detail_Ligne_Entete_Libelle));
            }
        }

        private void ValidateRessource(List<RessourceModel> ressources, ExcelLigneCommandeModel ligneCmd)
        {
            //Validate ressource
            if (!ligneCmd.CodeRessource.IsNullOrEmpty() && !ressources.Any(x => x.Code == ligneCmd.CodeRessource))
            {
                AddErreurLigne(ligneCmd, string.Format(FeatureCommande.Commande_Popin_ImportLignes_Erreur_CheckCI, FeatureCommande.Commande_Detail_Ligne_Entete_Ressource));
            }
        }

        private void ValidateTache(List<TacheModel> taches, ExcelLigneCommandeModel ligneCmd)
        {
            //Validate Tache
            if (!ligneCmd.CodeTache.IsNullOrEmpty() && !taches.Any(x => x.Code == ligneCmd.CodeTache))
            {
                AddErreurLigne(ligneCmd, string.Format(FeatureCommande.Commande_Popin_ImportLignes_Erreur_CheckCI, FeatureCommande.Commande_Detail_Ligne_Entete_Tache));
            }
        }

        private void ValidatePuHT(ExcelLigneCommandeModel ligneCmd)
        {
            decimal isnumeric;
            //Validate Prix Unité
            if (!ligneCmd.PuHt.IsNullOrEmpty())
            {
                if (decimal.TryParse(ligneCmd.PuHt, out isnumeric))
                {
                    if (isnumeric <= 0)
                    {
                        AddErreurLigne(ligneCmd, string.Format(FeatureCommande.Commande_Popin_ImportLignes_Erreur_SupA0, FeatureCommande.Commande_Detail_Ligne_Entete_PUHT));
                    }
                }
                else
                {
                    AddErreurLigne(ligneCmd, string.Format(FeatureCommande.Commande_Popin_ImportLignes_Erreur_Format, FeatureCommande.Commande_Detail_Ligne_Entete_PUHT));
                }
            }
        }

        private void ValidateUnite(List<UniteModel> unites, ExcelLigneCommandeModel ligneCmd)
        {
            //Validate Unité
            if (!ligneCmd.Unite.IsNullOrEmpty() && !unites.Any(x => x.Code == ligneCmd.Unite))
            {
                AddErreurLigne(ligneCmd, string.Format(FeatureCommande.Commande_Popin_ImportLignes_Erreur_Format, FeatureCommande.Commande_Detail_Ligne_Entete_Unite));
            }
        }

        private void ValidateQuantite(ExcelLigneCommandeModel ligneCmd)
        {
            decimal isnumeric;
            //Validate Quantité

            if (!ligneCmd.QuantiteCommande.IsNullOrEmpty())
            {
                if (decimal.TryParse(ligneCmd.QuantiteCommande, out isnumeric))
                {
                    if (isnumeric <= 0)
                    {
                        AddErreurLigne(ligneCmd, string.Format(FeatureCommande.Commande_Popin_ImportLignes_Erreur_SupA0, FeatureCommande.Commande_Detail_Ligne_Entete_Quantite));
                    }
                }
                else
                {
                    AddErreurLigne(ligneCmd, string.Format(FeatureCommande.Commande_Popin_ImportLignes_Erreur_Format, FeatureCommande.Commande_Detail_Ligne_Entete_Quantite));
                }
            }
        }

        private void ValidateIsDiminution(ExcelLigneCommandeModel ligneCmd)
        {
            //Validate isDiminution
            if (!ligneCmd.IsDiminution.IsNullOrEmpty() && !string.Equals(ligneCmd.IsDiminution, FredResource.Global_Oui, StringComparison.OrdinalIgnoreCase) && !string.Equals(ligneCmd.IsDiminution, FredResource.Global_Non, StringComparison.OrdinalIgnoreCase))
            {
                AddErreurLigne(ligneCmd, string.Format(FeatureCommande.Commande_Popin_ImportLignes_Erreur_Format, FeatureCommande.Commande_Detail_Ligne_Entete_Diminution));
            }
        }

        private void AddErreurLigne(ExcelLigneCommandeModel ligneCmd, string message)
        {
            ligneCmd.Erreurs.Add(string.Format(FeatureCommande.Commande_Erreurs_Excel_LigneCommande, ligneCmd.NumeroDeLigne, message));
        }
    }
}
