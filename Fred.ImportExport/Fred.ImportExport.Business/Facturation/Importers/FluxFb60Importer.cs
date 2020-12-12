using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Fred.Business.Facturation;
using Fred.Entities;
using Fred.Entities.Depense;
using Fred.Entities.Facturation;
using Fred.ImportExport.Business.Sap;
using Fred.ImportExport.Models.Facturation;

namespace Fred.ImportExport.Business.Facturation.Validators
{
    /// <summary>
    ///   Gestion des avoirs sans commande (Flux FB60)
    /// </summary>
    public class FluxFb60Importer : SapImporter<IEnumerable<FacturationSapModel>>
    {
        private const string UniteForfaitCode = "FRT";
        private const string SystemeImportCode = "STORM_CA";

        private readonly IFacturationManager facturationManager;
        private readonly IMapper mapper;
        private readonly IFluxFB60ImporterValidator fluxFB60ImporterValidator;

        public FluxFb60Importer(IFacturationManager facturationManager,
            IMapper mapper,
            IFluxFB60ImporterValidator fluxFB60ImporterValidator)
          : base("FACTURATION", "FB60", "Facturation sans commande")
        {
            this.facturationManager = facturationManager;
            this.mapper = mapper;
            this.fluxFB60ImporterValidator = fluxFB60ImporterValidator;
        }

        protected override void ImportModel(IEnumerable<FacturationSapModel> model)
        {
            List<FacturationEnt> facturations = new List<FacturationEnt>();
            List<FacturationSapModel> facturationSapModels = model.ToList();

            NLog.LogManager.GetCurrentClassLogger().Info($"[{Contexte}][{Code}] {Libelle} : { facturationSapModels.Count } entrée(s) : { ToLog(facturationSapModels) }");

            foreach (var fSap in facturationSapModels)
            {
                // DebitCredit = [H: crédit, S: débit]
                fSap.MouvementFarHt *= fSap.DebitCredit == Constantes.MouvementComptable.Debit ? -1 : 1;

                fluxFB60ImporterValidator.Validate(fSap);

                FacturationSapValidationResult result = FluxFB60Validation(fSap);
                FacturationEnt fFred = mapper.Map<FacturationEnt>(fSap);

                fFred.DeviseId = result.Devise?.DeviseId;
                fFred.CiId = result.CI?.CiId;

                result.Reception = new DepenseAchatEnt
                {
                    CiId = result.CI?.CiId,
                    TacheId = result.Tache?.TacheId,
                    FournisseurId = result.Fournisseur?.FournisseurId,
                    RessourceId = result.Ressource?.RessourceId,
                    UniteId = GetUnite(UniteForfaitCode)?.UniteId,
                    DeviseId = result.Devise?.DeviseId
                };

                if (fSap.DebitCredit == Constantes.MouvementComptable.Debit)
                {
                    //FactureSansCommande
                    FactureSansCommande.Process(result, fSap, fFred, GetFredIeUtilisateur());
                }
                else if (fSap.DebitCredit == Constantes.MouvementComptable.Credit)
                {
                    AvoirSansCommande.Process(result, fSap, fFred, GetFredIeUtilisateur());
                }

                facturations.Add(fFred);
            }

            facturationManager.BulkInsert(facturations);
        }

        /// <summary>
        ///     Validation du flux Avoir Sans Commande / FB60
        /// </summary>
        /// <param name="f">Objet facturation reçu de SAP</param>
        /// <returns>Resultat de validation</returns>
        private FacturationSapValidationResult FluxFB60Validation(FacturationSapModel f)
        {
            FacturationSapValidationResult result = new FacturationSapValidationResult
            {
                // RG_3530_046
                Devise = GetDevise(f.DeviseIsoCode),
                // RG_3656_077
                CI = GetCI(f.CiCode, f.SocieteCode),
                // RG_3656_074
                Societe = GetSociete(f.SocieteCode)
            };
            // RG_3656_076
            result.Fournisseur = GetFournisseur(f.FournisseurCode, result.Societe);
            // RG_3656_075
            result.Nature = GetNature(f.NatureCode, result.Societe);
            // RG_3656_081
            var systemeImport = GetSystemImport(SystemeImportCode);
            var transcoImport = GetTranscoImport(f.NatureCode, result.Societe, systemeImport);
            result.Ressource = GetRessource(transcoImport, result.Societe);
            // RG_3656_082
            result.Tache = GetTacheLitigeParDefaut(result.CI, result.Societe);

            return result;
        }

        private string ToLog(List<FacturationSapModel> facturationSapModels)
        {
            StringBuilder str = new StringBuilder();
            List<string> result = new List<string>();

            foreach (var f in facturationSapModels)
            {
                str.Append("{ ");
                str.Append("NumeroFactureSAP:").Append(f.NumeroFactureSAP).Append(", ");
                str.Append("NatureCode:").Append(f.NatureCode).Append(", ");
                str.Append("CiCode:").Append(f.CiCode).Append(", ");
                str.Append("SocieteCode:").Append(f.SocieteCode).Append(", ");
                str.Append(" }");
                result.Add(str.ToString());
            }

            return string.Join(",", result);
        }
    }
}
