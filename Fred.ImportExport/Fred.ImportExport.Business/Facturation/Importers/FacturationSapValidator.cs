using Fred.Business.CI;
using Fred.Business.Commande;
using Fred.Business.Depense;
using Fred.Business.Import;
using Fred.Business.Referential;
using Fred.Business.Referential.Nature;
using Fred.Business.Referential.Tache;
using Fred.Business.ReferentielFixe;
using Fred.Business.Societe;
using Fred.Entities;
using Fred.Entities.CI;
using Fred.Entities.Commande;
using Fred.Entities.Depense;
using Fred.Entities.Import;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielFixe;
using Fred.Entities.Societe;
using Fred.Framework.Extensions;
using Fred.ImportExport.Framework.Exceptions;
using Fred.ImportExport.Models.Facturation;
using System;
using System.Linq;

namespace Fred.ImportExport.Business.Facturation
{
  public class FacturationSapValidator
  {
    private readonly IDepenseManager depenseMgr;
    private readonly ICommandeManager commandeMgr;
    private readonly IDeviseManager deviseMgr;
    private readonly ICIManager ciMgr;
    private readonly INatureManager natureMgr;
    private readonly IFournisseurManager fournisseurManager;
    private readonly ISocieteManager societeMgr;
    private readonly ITranscoImportManager transcoImportMgr;
    private readonly ISystemeImportManager systemeImportMgr;
    private readonly IReferentielFixeManager referentielFixeMgr;
    private readonly ITacheManager tacheManager;
    private readonly IRemplacementTacheManager remplacementTacheManager;

    private readonly string systemeImportCode = "STORM_CA";
    private readonly Tuple<string, string> miro = Tuple.Create("FLUX_MIRO", "CR Rappro");
    private readonly Tuple<string, string> mr11 = Tuple.Create("FLUX_MR11", "Annulation FAR");
    private readonly Tuple<string, string> fb60 = Tuple.Create("FLUX_FB60", "Avoir sans commande");
    private const string CompteComptableBegin = "408";
    private const string CodeLitige = "L25";
    private readonly string baseErrorMessage = "[FACTURATION][{0}] Erreur intégration {1} : {2}";
    private readonly string unknowDataError = "[FACTURATION][{0}] Erreur intégration {1} : '{2}' = {3} non reconnu dans FRED.";
    private readonly string invalidDataError = "[FACTURATION][{0}] Erreur intégration {1} : '{2}' = {3} invalide.";

#pragma warning disable S107 // Methods should not have too many parameters
    /// <summary>
    /// Initialise une nouvelle instance de la classe <see cref="FacturationSapValidator"/>.
    /// </summary>        
    /// <param name="depenseMgr">Le gestionnaire des dépenses.</param>
    /// <param name="commandeMgr">Le gestionnaire des commandes.</param>
    /// <param name="deviseMgr">Le gestionnaire des dévises.</param>
    /// <param name="ciMgr">Le gestionnaire des CIs.</param>                
    /// <param name="natureMgr">Gestionnaire des nature.</param>        
    /// <param name="fournisseurManager">Gestionnaire des fournisseurs</param>
    /// <param name="societeMgr">Gestionnaire des Sociétés</param>
    /// <param name="transcoImportMgr">Gestionnaire transco import</param>
    /// <param name="systemeImportMgr">Gestionnaire system import</param>                                    
    /// <param name="referentielFixeMgr">Gestionnaire du référentiel fixe</param>
    /// <param name="tacheManager">Gestionnaire de taches</param><
    /// <param name="remplacementTacheManager">Gestionnaire de remplacement des tâches</param>
    public FacturationSapValidator(IDepenseManager depenseMgr,
                                ICommandeManager commandeMgr,
                                IDeviseManager deviseMgr,
                                ICIManager ciMgr,
                                INatureManager natureMgr,
                                IFournisseurManager fournisseurManager,
                                ISocieteManager societeMgr,
                                ITranscoImportManager transcoImportMgr,
                                ISystemeImportManager systemeImportMgr,
                                IReferentielFixeManager referentielFixeMgr,
                                ITacheManager tacheManager,
                                IRemplacementTacheManager remplacementTacheManager)
#pragma warning restore S107 // Methods should not have too many parameters
    {
      this.depenseMgr = depenseMgr;
      this.commandeMgr = commandeMgr;
      this.deviseMgr = deviseMgr;
      this.ciMgr = ciMgr;
      this.natureMgr = natureMgr;
      this.fournisseurManager = fournisseurManager;
      this.societeMgr = societeMgr;
      this.transcoImportMgr = transcoImportMgr;
      this.systemeImportMgr = systemeImportMgr;
      this.referentielFixeMgr = referentielFixeMgr;
      this.tacheManager = tacheManager;
      this.remplacementTacheManager = remplacementTacheManager;
    }

    /// <summary>
    ///     Validation du flux CR Rappro / MIRO
    /// </summary>
    /// <param name="f">Model facturation SAP</param>
    /// <param name="facturationSapModelMax">Facturation max en montant /!\ uniquement utilisé pour les coûts additionnels</param>
    /// <returns>Classe contenant les objets récupérés à partir des données issus de SAP</returns>
    public FacturationSapValidationResult FluxMIROValidation(FacturationSapModel f, FacturationSapModel facturationSapModelMax = null)
    {
      FacturationSapValidationResult result = new FacturationSapValidationResult();

      // RG_3530_035, RG_3530_036
      if (f.TypeLigneFactureCode != FacturationType.ArticleNonCommande.ToStringValue())
      {
        result.Commande = CommandeValidation(miro, f.CommandeNumero);

        // Si c'est un coût additionnel, on récupère la réception dont le montantHT est le plus élevé 
        if (f.TypeLigneFactureCode == FacturationType.CoutAdditionnel.ToStringValue()
            && f.Operation == OperationType.Facture.ToStringValue())
        {
          result.Reception = ReceptionValidation(miro, facturationSapModelMax.ReceptionId.Value, f.DateComptable);
        }
        else
        {
          result.Reception = ReceptionValidation(miro, f.ReceptionId.Value, f.DateComptable);
        }
      }

      // RG_3530_037
      result.Devise = DeviseValidation(miro, f.DeviseIsoCode);

      ValidationOperationFacture(result, f);

      ValidationOperationAvoir(f);

      ValidationOperationChargementDechargement(f);

      return result;
    }

    /// <summary>
    ///     Validation du flux Annulation FAR / MR11
    /// </summary>
    /// <param name="f">Objet facturation reçu de SAP</param>
    /// <returns>Resultat de validation</returns>
    public FacturationSapValidationResult FluxMR11Validation(FacturationSapModel f)
    {
      return new FacturationSapValidationResult
      {
        // RG_3530_043
        Commande = CommandeValidation(mr11, f.CommandeNumero),
        // RG_3530_044
        Reception = ReceptionValidation(mr11, f.ReceptionId.Value, f.DateComptable),
        // RG_3530_045
        Devise = DeviseValidation(mr11, f.DeviseIsoCode)
      };
    }

    /// <summary>
    ///     Validation du flux Avoir Sans Commande / FB60
    /// </summary>
    /// <param name="f">Objet facturation reçu de SAP</param>
    /// <returns>Resultat de validation</returns>
    public FacturationSapValidationResult FluxFB60Validation(FacturationSapModel f)
    {
      FacturationSapValidationResult result = new FacturationSapValidationResult
      {
        // RG_3530_046
        Devise = DeviseValidation(fb60, f.DeviseIsoCode),
        // RG_3656_077
        CI = CIValidation(fb60, f.CiCode, f.SocieteCode),
        // RG_3656_074
        Societe = SocieteValidation(fb60, f.SocieteCode)
      };
      // RG_3656_076
      result.Fournisseur = FournisseurValidation(fb60, f.FournisseurCode, result.Societe.GroupeId);
      // RG_3656_075
      result.Nature = NatureValidation(fb60, f.NatureCode, result.Societe.CodeSocieteComptable);
      // RG_3656_081
      result.Ressource = RessourceValidation(fb60, result.Societe.SocieteId, f.NatureCode, result.Societe.CodeSocieteComptable);
      // RG_3656_082
      result.Tache = TacheValidation(fb60, result.CI.CiId, f.CiCode, f.SocieteCode);

      return result;
    }

    #region Private 

    /// <summary>
    ///     Validation des Rappro facture Réception (ou Facturation) / Coût additionnel / Litiges (ou Article non commandé)
    /// </summary>
    /// <param name="result">Résultat de validation</param>
    /// <param name="f">Facturation SAP</param>
    private void ValidationOperationFacture(FacturationSapValidationResult result, FacturationSapModel f)
    {
      string msg;
      if (f.Operation == OperationType.Facture.ToStringValue())
      {
        // RG_3530_038
        if (!string.IsNullOrEmpty(f.CodeLitige) && f.CodeLitige != CodeLitige)
        {
          msg = string.Format(unknowDataError, miro.Item1, miro.Item2, "Litige Code", f.CodeLitige);
          throw new FredIeBusinessException(msg);
        }

        if (f.TypeLigneFactureCode == FacturationType.CoutAdditionnel.ToStringValue() || f.TypeLigneFactureCode == FacturationType.ArticleNonCommande.ToStringValue())
        {
          // RG_3656_073
          result.CI = CIValidation(miro, f.CiCode, f.SocieteCode);
          // RG_3656_065
          result.Societe = SocieteValidation(miro, f.SocieteCode);
          // RG_3656_070
          result.Nature = NatureValidation(miro, f.NatureCode, result.Societe?.CodeSocieteComptable);
          // RG_3656_071
          result.Ressource = RessourceValidation(miro, result.Societe.SocieteId, f.NatureCode, f.SocieteCode);
        }

        if (f.TypeLigneFactureCode == FacturationType.ArticleNonCommande.ToStringValue())
        {
          // RG_3656_033, RG_3656_080
          result.Tache = TacheValidation(miro, result.CI.CiId, f.CiCode, f.SocieteCode);
          // RG_3656_072
          result.Fournisseur = FournisseurValidation(miro, f.FournisseurCode, result.Societe.GroupeId);
        }

        ValidationRapprochement(f);
      }
    }

    private void ValidationRapprochement(FacturationSapModel f)
    {
      string msg;
      if (f.TypeLigneFactureCode == FacturationType.Facturation.ToStringValue())
      {
        // RG_3656_005
        ////if ((f.MontantHT + f.MouvementFarHt) < 0)
        ////{
        ////    msg = $"Erreur intégration {Miro} : lors d'une opération \"Rapprochement\", le 'Mouvement FAR' ne doit pas être plus élevé que le 'Montant HT' facturé.";
        ////    throw new FredIeBusinessException(msg);
        ////}
        // RG_3656_006
#pragma warning disable S1066 // Collapsible "if" statements should be merged
        if (f.MouvementFarHt > 0)
#pragma warning restore S1066 // Collapsible "if" statements should be merged
        {
          msg = string.Format(baseErrorMessage, miro.Item1, miro.Item2, "lors d'une opération \"Rapprochement\", le 'Mouvement FAR' ne doit pas être positif.");
          throw new FredIeBusinessException(msg);
        }
        // RG_3656_007
        ////if ((f.MontantHT + f.MouvementFarHt) > 0 && f.EcartPu == 0 && f.EcartQuantite <= 0)
        ////{
        ////    msg = $"Erreur intégration {Miro} : lors d'une opération \"Rapprochement\", en cas d'écart entre 'Mouvement FAR' et 'Montant HT' facturé, on doit avoir soit 'Ecart PU <> 0' soit 'Ecart Qté' > 0.";
        ////    throw new FredIeBusinessException(msg);
        ////}
      }
    }

    private void ValidationOperationChargementDechargement(FacturationSapModel f)
    {
      string msg;
      if (f.Operation == OperationType.Chargement.ToStringValue() || f.Operation == OperationType.Dechargement.ToStringValue())
      {
        if (string.IsNullOrEmpty(f.CompteComptable))
        {
          msg = string.Format(invalidDataError, miro.Item1, miro.Item2, "Compte Comptable", f.CompteComptable);
          throw new FredIeBusinessException(msg);
        }
        else if (!f.CompteComptable.StartsWith(CompteComptableBegin))
        {
          // RG_3530_040
          if (string.IsNullOrEmpty(f.NumeroFactureFMFI))
          {
            msg = string.Format(baseErrorMessage, miro.Item1, miro.Item2, $"'Numéro Facture FMFI' manquant pour l' 'Opération' = {f.Operation} sur le 'Compte Comptable' = {f.CompteComptable}.");
            throw new FredIeBusinessException(msg);
          }
        }
        else
        {
          // RG_3530_041
          if (f.MouvementFarHt == 0)
          {
            msg = string.Format(baseErrorMessage, miro.Item1, miro.Item2, $"'Mouvement FAR' manquant pour l' 'Opération' = {f.Operation} sur le 'Compte Comptable' = {f.CompteComptable}.");
            throw new FredIeBusinessException(msg);
          }
          // RG_3530_042
          else if (!string.IsNullOrEmpty(f.NumeroFactureFMFI))
          {
            msg = string.Format(baseErrorMessage, miro.Item1, miro.Item2, $"'Numéro Facture FMFI' = {f.NumeroFactureFMFI} invalide pour l' 'Opération' = {f.Operation} sur le 'Compte Comptable' = {f.CompteComptable}.");
            throw new FredIeBusinessException(msg);
          }
        }
      }
    }

    private void ValidationOperationAvoir(FacturationSapModel f)
    {
      string msg;
      if (f.Operation == OperationType.Avoir.ToStringValue())
      {
        // RG_3656_008
#pragma warning disable S1066 // Collapsible "if" statements should be merged
        if (f.MouvementFarHt < 0)
#pragma warning restore S1066 // Collapsible "if" statements should be merged
        {
          msg = string.Format(baseErrorMessage, miro.Item1, miro.Item2, "lors d'une opération \"Avoir\", le 'Mouvement FAR' ne doit pas être négatif.");
          throw new FredIeBusinessException(msg);
        }
        ////// RG_3656_009
        ////if (f.MontantHT < f.MouvementFarHt)
        ////{
        ////    msg = $"Erreur intégration {Miro} : lors d'une opération \"Avoir\", le 'Mouvement FAR' ne doit pas être plus élevé que le 'Montant HT' rapproché.";
        ////    throw new FredIeBusinessException(msg);
        ////}
        ////// RG_3656_064
        ////if (f.MontantHT > f.MouvementFarHt && f.EcartPu == 0 && (f.EcartQuantite + f.Quantite <= 0))
        ////{
        ////    msg = $"Erreur intégration {Miro} : lors d'une opération \"Avoir\", en cas d'écart entre 'Mouvement FAR' et 'Montant HT' facturé, il est nécessaire que 'Ecart PU <> 0' et/ou 'Ecart Qté' <= Quantité.";
        ////    throw new FredIeBusinessException(msg);
        ////}
      }
    }

    private CommandeEnt CommandeValidation(Tuple<string, string> flux, string commandeNumero)
    {
      CommandeEnt commande = commandeMgr.GetCommande(commandeNumero);
      if (commande == null)
      {
        string msg = string.Format(unknowDataError, flux.Item1, flux.Item2, "Commande Numéro", commandeNumero);
        throw new FredIeBusinessException(msg);
      }
      return commande;
    }

    private DeviseEnt DeviseValidation(Tuple<string, string> flux, string deviseIsoCode)
    {
      DeviseEnt devise = deviseMgr.GetDevise(deviseIsoCode);
      if (devise == null)
      {
        string msg = string.Format(unknowDataError, flux.Item1, flux.Item2, "Devise Iso Code", deviseIsoCode);
        throw new FredIeBusinessException(msg);
      }

      return devise;
    }

    private FournisseurEnt FournisseurValidation(Tuple<string, string> flux, string fournisseurCode, int groupeId)
    {
      FournisseurEnt fournisseur = fournisseurManager.GetFournisseurList().FirstOrDefault(x => x.Code == fournisseurCode && x.GroupeId == groupeId);
      if (fournisseur == null)
      {
        string msg = string.Format(baseErrorMessage, flux.Item1, flux.Item2, $"aucun Fournisseur trouvé pour le 'Fournisseur Code' = {fournisseurCode} et le 'GroupeId' = {groupeId} fournis.");
        throw new FredIeBusinessException(msg);
      }

      return fournisseur;
    }

    private CIEnt CIValidation(Tuple<string, string> flux, string codeCi, string codeSocieteCompta)
    {
      CIEnt ci = ciMgr.GetCI(codeCi, codeSocieteCompta);
      if (ci == null)
      {
        string msg = string.Format(baseErrorMessage, flux.Item1, flux.Item2, $"aucun CI trouvée pour le 'CI Code' = {codeCi} et le 'Societe Code' = {codeSocieteCompta} fournis.");
        throw new FredIeBusinessException(msg);
      }

      return ci;
    }

    private DepenseAchatEnt ReceptionValidation(Tuple<string, string> flux, int receptionId, DateTime dateComptable)
    {
      DepenseAchatEnt reception = depenseMgr.GetDepenseById(receptionId);
      if (reception == null)
      {
        string msg = string.Format(unknowDataError, flux.Item1, flux.Item2, "Réception ID", receptionId);
        throw new FredIeBusinessException(msg);
      }
      else if (reception.GroupeRemplacementTacheId > 0)
      {
        RemplacementTacheEnt rt = remplacementTacheManager.GetLast(reception.GroupeRemplacementTacheId.Value);

        if (rt != null)
        {
          if (rt.DateComptableRemplacement.Value.Month > dateComptable.Month && rt.DateComptableRemplacement.Value.Year > dateComptable.Year)
          {
            string msg = string.Format(baseErrorMessage, flux.Item1, flux.Item2, "Opération antérieure à un remplacement de tâche de la Réception dans FRED.");
            throw new FredIeBusinessException(msg);
          }
          else
          {
            reception.TacheId = rt.TacheId;
            reception.Tache = rt.Tache;
            rt.Annulable = false;
            remplacementTacheManager.Update(rt);
          }
        }
      }

      return reception;
    }

    private NatureEnt NatureValidation(Tuple<string, string> flux, string natureCode, string codeSocieteCompta)
    {
      NatureEnt nature = natureMgr.GetNature(natureCode, codeSocieteCompta);
      if (nature == null)
      {
        string msg = string.Format(baseErrorMessage, flux.Item1, flux.Item2, $"aucune Nature trouvée pour le 'Nature Code' = {natureCode} et le 'Societe Code' = {codeSocieteCompta} fournis.");
        throw new FredIeBusinessException(msg);
      }
      return nature;
    }

    private SocieteEnt SocieteValidation(Tuple<string, string> flux, string societeCode)
    {
      SocieteEnt societe = societeMgr.GetSocieteByCodeSocieteComptable(societeCode);
      if (societe == null)
      {
        string msg = string.Format(unknowDataError, flux.Item1, flux.Item2, "Societe Code", societeCode);
        throw new FredIeBusinessException(msg);
      }
      return societe;
    }

    private TacheEnt TacheValidation(Tuple<string, string> flux, int ciId, string codeCi, string societeCode)
    {
      TacheEnt tache = tacheManager.GetTacheListByCiId(ciId)?.FirstOrDefault(x => x.Code == Constantes.TacheSysteme.CodeTacheLitige) ?? tacheManager.GetTacheParDefaut(ciId);

      if (tache == null)
      {
        string msg = string.Format(baseErrorMessage, flux.Item1, flux.Item2, $"aucune Tâche FRED trouvée pour le 'CI Code' = {codeCi} et le 'Societe Code' = {societeCode} fournis.");
        throw new FredIeBusinessException(msg);
      }
      return tache;
    }

    // RG_3656_026 
    private RessourceEnt RessourceValidation(Tuple<string, string> flux, int societeId, string natureCode, string codeSocieteCompta)
    {
      RessourceEnt ressource = null;
      string msg;

      // RG_3656_068 : Identifier le « Code Ressource FRED » via la table « Transco Import » 
      SystemeImportEnt systemeImport = systemeImportMgr.GetSystemeImport(systemeImportCode);
      if (systemeImport != null)
      {
        TranscoImportEnt transcoImport = transcoImportMgr.GetTranscoImport(natureCode, societeId, systemeImport.SystemeImportId);
        if (transcoImport != null)
        {
          ressource = referentielFixeMgr.GetRessource(transcoImport.CodeInterne);
          //A ajouter plus tard quand la modification de l'entité DepenseEnt sera effectué :
          //// referentielEtenduMgr.GetReferentielEtendu(societe.SocieteId, ressource.RessourceId, nature.NatureId);
        }
        else
        {
          msg = string.Format(baseErrorMessage, flux.Item1, flux.Item2, $"TranscoImportEnt non trouvé pour la 'Nature Code Externe' = {natureCode}, la 'SocieteId' = {societeId} et le SystemeImportId = {systemeImport.SystemeImportId}.");
          NLog.LogManager.GetCurrentClassLogger().Error(msg);
        }
      }
      else
      {
        msg = string.Format(baseErrorMessage, flux.Item1, flux.Item2, $"SystemImportEnt non trouvé pour {systemeImportCode}");
        NLog.LogManager.GetCurrentClassLogger().Error(msg);
      }

      if (ressource == null)
      {
        //RG_2853_263 : Règle par défaut d’attribution de l’ « ID Referentiel Etendu » en cas d’échec de la RG_2853_262
        ////NatureEnt nature = natureMgr.GetNatureActive(natureCode, societeId);
        ////ReferentielEtenduEnt referentielEtendu= referentielEtenduMgr.GetReferentielEtendu(societeId, nature.NatureId);
        ////if (referentielEtendu == null)
        ////{
        ////    referentielEtendu = referentielEtenduMgr.GetReferentielEtenduByRessourceAndSociete(receptionRessourceId, societeId, true);
        ////}
        ////ressource = referentielEtendu?.Ressource;

        msg = string.Format(baseErrorMessage, flux.Item1, flux.Item2, $"aucune Ressource FRED trouvée pour le 'Nature Code' = {natureCode} et le 'Societe Code' = {codeSocieteCompta} fournis.");
        throw new FredIeBusinessException(msg);
      }

      return ressource;
    }

    #endregion
  }
}
