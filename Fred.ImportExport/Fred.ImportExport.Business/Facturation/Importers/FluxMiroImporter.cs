using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using AutoMapper;
using Fred.Business.Commande;
using Fred.Business.Depense;
using Fred.Business.Facturation;
using Fred.Business.FeatureFlipping;
using Fred.Business.Groupe;
using Fred.Entities;
using Fred.Entities.Commande;
using Fred.Entities.Depense;
using Fred.Entities.Facturation;
using Fred.Entities.Referential;
using Fred.Framework.Extensions;
using Fred.Framework.FeatureFlipping;
using Fred.ImportExport.Business.Sap;
using Fred.ImportExport.Framework.Exceptions;
using Fred.ImportExport.Models.Facturation;

namespace Fred.ImportExport.Business.Facturation.Validators
{
    /// <summary>
    ///   Gestion du CR Rappro (Flux MIRO)
    /// </summary>
    public class FluxMiroImporter : SapImporter<IEnumerable<FacturationSapModel>>
    {
        private const string UniteForfaitCode = "FRT";
        private const string SystemeImportCode = "STORM_CA";
        private const string CompteComptableBegin = "408";
        private const string CodeLitige = "L25";

        private readonly IFacturationManager facturationManager;
        private readonly ICommandeManager commandeManager;
        private readonly IDepenseManager depenseManager;
        private readonly IMapper mapper;
        private readonly IFeatureFlippingManager featureFlippingManager;
        private readonly IGroupeManager groupeManager;

        public FluxMiroImporter(
            IFacturationManager facturationManager,
            ICommandeManager commandeManager,
            IDepenseManager depenseManager,
            IMapper mapper,
            IFeatureFlippingManager featureFlippingManager,
            IGroupeManager groupeManager)
          : base("FACTURATION", "MIRO", "CR Rappro")
        {
            this.facturationManager = facturationManager;
            this.commandeManager = commandeManager;
            this.depenseManager = depenseManager;
            this.mapper = mapper;
            this.featureFlippingManager = featureFlippingManager;
            this.groupeManager = groupeManager;
        }

        protected override void ImportModel(IEnumerable<FacturationSapModel> model)
        {
            List<FacturationEnt> facturations = new List<FacturationEnt>();
            ////decimal totalFarSap = facturationModels.FirstOrDefault()?.TotalFarHt ?? 0;
            List<FacturationSapModel> facturationSapModels = model.ToList();

            NLog.LogManager.GetCurrentClassLogger().Info($"[{Contexte}][{Code}] {Libelle} : { facturationSapModels.Count } entrée(s) : { ToLog(facturationSapModels) }");

            if (IsChargementDechargementFar(facturationSapModels))
            {
                ProcessChargementDechargementFar(facturationSapModels, facturations);
            }
            else
            {
                ProcessAll(facturationSapModels, facturations);
            }

            facturationManager.BulkInsert(facturations);

            // RG_3656_063 
            ////foreach (var fFred in facturations)
            ////{
            ////    IEnumerable<DepenseAchatEnt> receptions = receptionMgr.Get(fFred.CommandeId.Value).ToList();
            ////    receptions.ComputeAll();
            ////    TotalFarValidation(receptions.ToList(), totalFarSap);
            ////}             
        }

        #region Private

        private void ProcessOperationFacture(FacturationSapModel fSap, FacturationEnt fFred, FacturationSapValidationResult result)
        {
            if (fSap.TypeLigneFactureCode == FacturationType.Facturation.ToStringValue())
            {
                Facturation.Process(result, fSap, fFred, this.GetFredIeUtilisateur());
            }
            else if (fSap.TypeLigneFactureCode == FacturationType.CoutAdditionnel.ToStringValue())
            {
                CoutAdditionnel.Process(result, fSap, fFred, this.GetFredIeUtilisateur());
            }
            else if (fSap.TypeLigneFactureCode == FacturationType.ArticleNonCommande.ToStringValue())
            {
                NonCommande.Process(result, fSap, fFred, this.GetFredIeUtilisateur());
            }
        }

        private void ProcessOperationAvoir(FacturationSapModel fSap, FacturationEnt fFred, FacturationSapValidationResult result)
        {
            AvoirQuantite.Process(result, fSap, fFred, this.GetFredIeUtilisateur());
        }

        private void ProcessOperationChargement(FacturationSapModel fSap, FacturationEnt fFred, FacturationSapValidationResult result)
        {
            if (!fSap.CompteComptable.StartsWith(CompteComptableBegin))
            {
                FacturationMontant.Process(result, fSap, fFred, this.GetFredIeUtilisateur());
            }
        }

        private void ProcessOperationDechargement(FacturationSapModel fSap, FacturationEnt fFred, FacturationSapValidationResult result)
        {
            if (!fSap.CompteComptable.StartsWith(CompteComptableBegin))
            {
                AvoirMontant.Process(result, fSap, fFred, this.GetFredIeUtilisateur());
            }
        }

        private bool IsChargementDechargementFar(List<FacturationSapModel> facturationModels)
        {
            return facturationModels.Count == 2
                   && facturationModels[0].TypeLigneFactureCode == FacturationType.Facturation.ToStringValue() && (facturationModels[0].Operation == OperationType.Chargement.ToStringValue() || facturationModels[0].Operation == OperationType.Dechargement.ToStringValue())
                   && facturationModels[1].TypeLigneFactureCode == FacturationType.CoutAdditionnel.ToStringValue() && (facturationModels[1].Operation == OperationType.Chargement.ToStringValue() || facturationModels[1].Operation == OperationType.Dechargement.ToStringValue());
        }

        /// <summary>
        /// Gestion très particulière d'un chargement ou déchargement de provision FAR (Ceci est dû à une limitation de SAP)
        /// Conditions à respecter :
        ///     - La liste de facturationSapModel doit contenir exactement 2 éléments
        ///         • Pour un Chargement Provision FAR :
        ///             o Le 1er élément doit avoir [TypeLigneFactureCode = 1, Operation = 3]
        ///             o Le 2eme élément doit avoir [TypeLigneFactureCode = 2, Operation = 3]
        ///         • Pour un Déchargement Provision FAR :
        ///             o Le 1er élément doit avoir [TypeLigneFactureCode = 1, Operation = 4]
        ///             o Le 2eme élément doit avoir [TypeLigneFactureCode = 2, Operation = 4]
        /// cf. : la fonction IsChargementDechargementFar                
        /// </summary>
        /// <param name="facturationModels">Liste de facturations issues de SAP</param>
        /// <param name="facturations">Liste de Facturations FRED</param>
        private void ProcessChargementDechargementFar(List<FacturationSapModel> facturationModels, List<FacturationEnt> facturations)
        {
            FacturationSapModel fSap1 = facturationModels[0];
            // fSap2 est la facturation à enregistrer dans FRED
            FacturationSapModel fSap2 = facturationModels[1];

            // DebitCredit = [H: crédit, S: débit]
            fSap2.MouvementFarHt *= fSap2.DebitCredit == Constantes.MouvementComptable.Debit ? -1 : 1;

            // Les champs à récupérer de fSap1 sont l'identifiant de la réception et la quantité
            fSap2.ReceptionId = fSap1.ReceptionId;
            fSap2.Quantite = fSap1.Quantite;

            FacturationSapValidationResult result = FluxMIROValidation(fSap2);

            FacturationEnt fFred = mapper.Map<FacturationEnt>(fSap2);
            fFred.CommandeId = result.Commande?.CommandeId;
            fFred.DeviseId = result.Devise?.DeviseId;
            fFred.CiId = result.CI?.CiId;

            ChargementDechargement.Process(result, fSap2, fFred, this.GetFredIeUtilisateur());

            facturations.Add(fFred);
        }

        /// <summary>
        ///     Gestion de tous les autres types de dépenses
        /// </summary>
        /// <param name="facturationModels">Liste des facturations SAP</param>
        /// <param name="facturations">Liste des facturations FRED</param>
        private void ProcessAll(List<FacturationSapModel> facturationModels, List<FacturationEnt> facturations)
        {
            // On récupére la ligne de facturation de type 1 ayant le « Montant Facturé HT » le plus élevé
            // /!\ Uniquement utilisé pour les coûts additionnels
            FacturationSapModel facturationSapModelMax = facturationModels.Where(x => x.TypeLigneFactureCode == FacturationType.Facturation.ToStringValue())
                                                                          .OrderByDescending(x => x.MontantHT)
                                                                          .FirstOrDefault();

            foreach (var fSap in facturationModels)
            {
                // DebitCredit = [H: crédit, S: débit]
                fSap.MouvementFarHt *= fSap.DebitCredit == Constantes.MouvementComptable.Debit ? -1 : 1;

                FacturationSapValidationResult result = FluxMIROValidation(fSap, facturationSapModelMax);

                FacturationEnt fFred = mapper.Map<FacturationEnt>(fSap);
                fFred.CommandeId = result.Commande?.CommandeId;
                fFred.DeviseId = result.Devise?.DeviseId;
                fFred.CiId = result.CI?.CiId;

                // Si Article Non Commandé / Litige (Seul cas où la Réception doit être nulle)
                if (result.Reception == null)
                {
                    UniteEnt uniteForfait = GetUnite(UniteForfaitCode);
                    result.Reception = new DepenseAchatEnt
                    {
                        CiId = result.CI?.CiId, // RG_3656_031                                          
                        TacheId = result.Tache?.TacheId, // RG_3656_033                                        
                        FournisseurId = result.Fournisseur?.FournisseurId, // RG_3656_032                                        
                        RessourceId = result.Ressource?.RessourceId, // RG_3656_026                                        
                        UniteId = uniteForfait?.UniteId, // RG_3656_030
                        DeviseId = result.Devise?.DeviseId
                    };
                }

                if (fSap.Operation == OperationType.Facture.ToStringValue())
                {
                    ProcessOperationFacture(fSap, fFred, result);
                }
                else if (fSap.Operation == OperationType.Avoir.ToStringValue())
                {
                    ProcessOperationAvoir(fSap, fFred, result);
                }
                else if (fSap.Operation == OperationType.Chargement.ToStringValue())
                {
                    ProcessOperationChargement(fSap, fFred, result);
                }
                else if (fSap.Operation == OperationType.Dechargement.ToStringValue())
                {
                    ProcessOperationDechargement(fSap, fFred, result);
                }

                // On ajoute une facturation.  
                facturations.Add(fFred);
            }
        }

        /// <summary>
        ///     Validation du flux CR Rappro / MIRO
        /// </summary>
        /// <param name="f">Model facturation SAP</param>
        /// <param name="facturationSapModelMax">Facturation max en montant /!\ uniquement utilisé pour les coûts additionnels</param>
        /// <returns>Classe contenant les objets récupérés à partir des données issus de SAP</returns>
        private FacturationSapValidationResult FluxMIROValidation(FacturationSapModel f, FacturationSapModel facturationSapModelMax = null)
        {
            FacturationSapValidationResult result = new FacturationSapValidationResult();

            // RG_3656_108
            GetFacturations(f.NumeroFactureSAP);

            // RG_3530_035, RG_3530_036
            if (f.TypeLigneFactureCode != FacturationType.ArticleNonCommande.ToStringValue())
            {
                result.Commande = GetCommandeByNumberOrExternalNumber(f.CommandeNumero);

                // US 5163 - Commande - Remontée dans FRED du fournisseur corrigé dans SAP (Sous Feature Flipping)
                UpdateFournisseurBySap(result, f);

                // Si c'est un coût additionnel, on récupère la réception dont le montantHT est le plus élevé 
                if (f.TypeLigneFactureCode == FacturationType.CoutAdditionnel.ToStringValue() && f.Operation == OperationType.Facture.ToStringValue())
                {
                    result.Reception = GetReception(facturationSapModelMax.ReceptionId.Value, f.DateComptable);
                }
                else if (f.TypeLigneFactureCode == FacturationType.CoutAdditionnel.ToStringValue() && f.Operation == OperationType.Avoir.ToStringValue() && HasFeatureAvoirAvecCommandeAvecCoutAdditionnelAsFactureAvecCoutAdditionnel(f.SocieteCode))
                {
                    result.Reception = GetReception(facturationSapModelMax.ReceptionId.Value, f.DateComptable);
                }
                else
                {
                    result.Reception = GetReception(f.ReceptionId.Value, f.DateComptable);
                }
            }

            // RG_3530_037
            result.Devise = GetDevise(f.DeviseIsoCode);

            ValidationOperationFacture(result, f);

            ValidationOperationAvoir(f);

            ValidationOperationChargementDechargement(f);

            return result;
        }

        /// <summary>
        /// Permet de savoir si on gere les avoirs (avec commande) avec coûts additionnels de la meme maniere 
        /// que les factures (avec commande) avec coûts additionnels.
        /// Pour le groupe FTP, c'est oui, pour les autres non.
        /// BUG 5436 :
        /// Dans le cas d'un avoir (avec commande) avec coûts additionnels, FRED renvoie une erreur 500 ID RECEPTION MANQUANT.
        /// Il faudrait gérer ce cas de la même manière qu'une facture avec commande avec coûts additionnels.
        /// Evolution demandée par SOMOPA, ce cas n'est pas prévu dans le périmètre de RZB.  (Règle à confirmer si à conditionner sur société)
        /// </summary>
        /// <param name="codeSocieteComptable">codeSocieteComptable</param>
        /// <returns>Pour le groupe FTP, c'est oui</returns>
        private bool HasFeatureAvoirAvecCommandeAvecCoutAdditionnelAsFactureAvecCoutAdditionnel(string codeSocieteComptable)
        {
            var groupe = this.groupeManager.GetGroupeByCodeSocieteComptableOfSociete(codeSocieteComptable);
            if (groupe.Code == Constantes.CodeGroupeFTP)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        ///     Validation des Rappro facture Réception (ou Facturation) / Coût additionnel / Litiges (ou Article non commandé)
        /// </summary>
        /// <param name="result">Résultat de validation</param>
        /// <param name="f">Facturation SAP</param>
        private void ValidationOperationFacture(FacturationSapValidationResult result, FacturationSapModel f)
        {
            if (f.Operation == OperationType.Facture.ToStringValue())
            {
                // RG_3530_038
                if (!string.IsNullOrEmpty(f.CodeLitige) && f.CodeLitige != CodeLitige)
                {
                    throw new FredIeBusinessException(BaseMessageErreur + $"'Litige Code' \"{f.CodeLitige}\" non reconnu dans FRED");
                }

                if (f.TypeLigneFactureCode == FacturationType.CoutAdditionnel.ToStringValue() || f.TypeLigneFactureCode == FacturationType.ArticleNonCommande.ToStringValue())
                {
                    // RG_3656_073          
                    result.CI = GetCI(f.CiCode, f.SocieteCode);
                    // RG_3656_065          
                    result.Societe = GetSociete(f.SocieteCode);
                    // RG_3656_070          
                    result.Nature = GetNature(f.NatureCode, result.Societe);
                    // RG_3656_071        
                    var systemeImport = GetSystemImport(SystemeImportCode);
                    var transcoImport = GetTranscoImport(f.NatureCode, result.Societe, systemeImport);
                    result.Ressource = GetRessource(transcoImport, result.Societe);
                }

                if (f.TypeLigneFactureCode == FacturationType.ArticleNonCommande.ToStringValue())
                {
                    // RG_3656_033, RG_3656_080          
                    result.Tache = GetTacheLitigeParDefaut(result.CI, result.Societe);

                    // RG_3656_072          
                    result.Fournisseur = GetFournisseur(f.FournisseurCode, result.Societe);
                }

                ValidationRapprochement(f);
            }
        }

        private void ValidationRapprochement(FacturationSapModel f)
        {
            if (f.TypeLigneFactureCode == FacturationType.Facturation.ToStringValue())
            {
                // RG_3656_005
                if (f.Quantite <= 0)
                {
                    throw new FredIeBusinessException(BaseMessageErreur + "lors d'une opération \"Facturation\", la 'Quantité' ne doit pas être inférieure ou égale à 0");
                }
                // RG_3656_007
                if ((f.MontantHT + f.MouvementFarHt) != 0 && f.EcartPu == 0 && f.EcartQuantite <= 0)
                {
                    throw new FredIeBusinessException(BaseMessageErreur + "lors d'une opération \"Facturation\" ou \"Avoir\", en cas d'écart entre 'Mouvement FAR' et 'Montant HT' facturé, on doit avoir 'Ecart PU'<>0 et/ou 'Ecart Qté'>0'");
                }
            }
        }

        private void ValidationOperationChargementDechargement(FacturationSapModel f)
        {
            if (f.Operation == OperationType.Chargement.ToStringValue() || f.Operation == OperationType.Dechargement.ToStringValue())
            {
                if (string.IsNullOrEmpty(f.CompteComptable))
                {
                    throw new FredIeBusinessException(BaseMessageErreur + $"'Compte Comptable' \"{f.CompteComptable}\" invalide");
                }
                else if (!f.CompteComptable.StartsWith(CompteComptableBegin))
                {
                    // RG_3530_040
                    if (string.IsNullOrEmpty(f.NumeroFactureFMFI))
                    {
                        throw new FredIeBusinessException(BaseMessageErreur + $"'Numéro Facture FMFI' manquant pour l' 'Opération' \"{f.Operation}\" sur le 'Compte Comptable' \"{f.CompteComptable}\"");
                    }
                }
                else
                {
                    // RG_3530_041
                    if (f.MouvementFarHt == 0)
                    {
                        throw new FredIeBusinessException(BaseMessageErreur + $"'Mouvement FAR' manquant pour l' 'Opération' \"{f.Operation}\" sur le 'Compte Comptable' \"{f.CompteComptable}\"");
                    }
                    // RG_3530_042
                    else if (!string.IsNullOrEmpty(f.NumeroFactureFMFI))
                    {
                        throw new FredIeBusinessException(BaseMessageErreur + $"'Numéro Facture FMFI' \"{f.NumeroFactureFMFI}\" invalide pour l' 'Opération' \"{f.Operation}\" sur le 'Compte Comptable' \"{f.CompteComptable}\"");
                    }
                }
            }
        }

        private void ValidationOperationAvoir(FacturationSapModel f)
        {
            if (f.Operation == OperationType.Avoir.ToStringValue())
            {
                // RG_3656_008
                if (f.Quantite >= 0)
                {
                    throw new FredIeBusinessException(BaseMessageErreur + "lors d'une opération \"Avoir avec commande en quantité\", la 'Quantité' ne doit pas être supérieure ou égale à 0");
                }
                // RG_3656_007
                if ((f.MontantHT + f.MouvementFarHt) != 0 && f.EcartPu == 0 && f.EcartQuantite <= 0)
                {
                    throw new FredIeBusinessException(BaseMessageErreur + "lors d'une opération \"Facturation\" ou \"Avoir\", en cas d'écart entre 'Mouvement FAR' et 'Montant HT' facturé, on doit avoir 'Ecart PU'<>0 et/ou 'Ecart Qté'>0'");
                }
            }
        }

        private string ToLog(List<FacturationSapModel> facturationSapModels)
        {
            StringBuilder str = new StringBuilder();
            List<string> result = new List<string>();

            foreach (var f in facturationSapModels)
            {
                str.Append("{ ");
                str.Append("TypeLigneFactureCode:").Append(f.TypeLigneFactureCode).Append(", ");
                str.Append("Operation:").Append(f.Operation).Append(", ");
                str.Append("CommandeNumero:").Append(f.CommandeNumero).Append(", ");
                str.Append("ReceptionId:").Append(f.ReceptionId).Append(", ");
                str.Append("NumeroFactureSAP:").Append(f.NumeroFactureSAP).Append(", ");
                str.Append("SocieteCode:").Append(f.SocieteCode);
                str.Append(" }");
                result.Add(str.ToString());
            }

            return string.Join(",", result);
        }

        /// <summary>
        ///     Mise à jour du Fournisseur si Fournisseur reçu du flux MIRO est différent de celui de la commande existante dans FRED
        /// </summary>
        /// <param name="result">Objet portant tous les éléments nécessaires (Société, FOurnisseur, Commande, etc)</param>
        /// <param name="f">Facturation SAP</param>
        private void UpdateFournisseurBySap(FacturationSapValidationResult result, FacturationSapModel f)
        {
            try
            {
                // US 5163 - Commande - Remontée dans FRED du fournisseur corrigé dans SAP
                if (this.featureFlippingManager.IsActivated(EnumFeatureFlipping.CorrectionFournisseurSAP) && !string.IsNullOrEmpty(f.SocieteCode))
                {
                    var societe = GetSociete(f.SocieteCode);

                    if (!string.IsNullOrEmpty(f.FournisseurCode))
                    {
                        FournisseurEnt fournisseur = GetFournisseur(f.FournisseurCode, societe);

                        if (result.Commande.FournisseurId != fournisseur.FournisseurId)
                        {
                            var fredIeUserId = GetFredIeUtilisateur().UtilisateurId;

                            if (!result.Commande.OldFournisseurId.HasValue)
                            {
                                result.Commande.OldFournisseurId = result.Commande.FournisseurId;
                            }

                            result.Commande.FournisseurId = fournisseur.FournisseurId;

                            List<Expression<Func<CommandeEnt, object>>> fieldsToUpdate = new List<Expression<Func<CommandeEnt, object>>>
                            {
                                x=>x.FournisseurId, x =>x.OldFournisseurId
                            };

                            // RG_5163_004 : Remplacement du fournisseur dans la commande FRED)    
                            commandeManager.QuickUpdate(result.Commande, fredIeUserId, fieldsToUpdate);

                            UpdateAllDepenseAchatFournisseurId(result.Commande.CommandeId, fournisseur.FournisseurId, fredIeUserId);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                // Le flux MIRO n'est pas bloqué par les erreurs de mise à jour du fournisseur
                NLog.LogManager.GetCurrentClassLogger().Error(e, $"[{Contexte}][{Code}] {Libelle} : Mise à jour du fournisseur différent dans SAP");
            }
        }

        /// <summary>
        /// RG_5163_004 : Remplacement du fournisseur dans la commande FRED :
        /// 3/ Idem, écraser le « Fournisseur ID » de toutes les réceptions de la commande avec l’ID Fournisseur de l’opération SAP : 
        ///     à récupérer via Commande -> Lignes de commande -> Dépense Achat de type 1=Réception.
        /// 4/ Idem, écraser le « Fournisseur ID » de toutes les autres Dépenses Achats liées à la commande avec l’ID Fournisseur de l’opération SAP :
        ///     via les Réceptions identifiées en 3/, récupérer toutes les Dépenses Achats liées à chacune de ces Réceptions via le champ « Dépense Parent ID ».
        /// </summary>
        /// <param name="commandeId">Identifiant de la commande</param>
        /// <param name="fournisseurId">Identifiant du nouveau fournisseur</param>
        /// <param name="fredIeUserId">Identifiant de l'utilisateur FRED IE</param>
        private void UpdateAllDepenseAchatFournisseurId(int commandeId, int fournisseurId, int fredIeUserId)
        {
            // La liste des dépenses achats liées à une ligne de commande est forcément une dépense achat de type réception
            List<Expression<Func<DepenseAchatEnt, bool>>> filters = new List<Expression<Func<DepenseAchatEnt, bool>>> { x => x.CommandeLigne.CommandeId == commandeId };
            List<Expression<Func<DepenseAchatEnt, object>>> includePorperties = new List<Expression<Func<DepenseAchatEnt, object>>> { x => x.Depenses };

            var depenses = depenseManager.Search(filters, null, includePorperties).ToList();

            depenses.ForEach(x =>
            {
                x.FournisseurId = fournisseurId;
                x.Depenses.ForEach(y => y.FournisseurId = fournisseurId);
                // Mise à jour des dépenses achats enfants
                depenseManager.UpdateDepense(x.Depenses.ToList(), fredIeUserId);
            });

            // Mise à jour des dépenses achat de type réception
            depenseManager.UpdateDepense(depenses, fredIeUserId);
        }

        /////// <summary>
        ///////     Validation du total des FAR
        /////// </summary>
        /////// <param name="receptions">Liste des réceptions</param>
        /////// <param name="totaFarSap">Total Far SAP</param>
        ////private void TotalFarValidation(List<DepenseAchatEnt> receptions, decimal totaFarSap)
        ////{
        ////    bool isTotalFarValid = totaFarSap != GetSoldeFar(receptions);

        ////    receptions.ForEach(x =>
        ////    {
        ////        x.ErreurControleFar = isTotalFarValid;
        ////        x.DateControleFar = DateTime.UtcNow;
        ////        // Dernière Opération Contrôle FAR = Facturation ID(ID de la ligne correspondant à l’opération SAP en cours dans la table Facturation)
        ////    });
        ////}

        ////// RG_3656_062 : Fonction de calcul du Solde FAR d’une réception à une date J
        ////private decimal GetSoldeFar(DepenseAchatEnt reception)
        ////{
        ////    decimal ajustementFarSum = reception.Depenses
        ////                                        .Where(x => x.DepenseTypeId == DepenseType.AjustementFar.ToIntValue()
        ////                                                    && x.DateOperation.HasValue
        ////                                                    && x.DateOperation.Value.Date <= DateTime.UtcNow.Date)
        ////                                        .Sum(x => x.Quantite * x.PUHT);

        ////    return (reception.Quantite * reception.PUHT) + ajustementFarSum;
        ////}

        ////// RG_3656_062 : Fonction de calcul du Solde FAR d’une réception à une date J
        ////private decimal GetSoldeFar(List<DepenseAchatEnt> receptions)
        ////{
        ////    decimal result = 0;
        ////    receptions.ForEach(x => result += GetSoldeFar(x));
        ////    return result;
        ////}
        #endregion
    }
}
