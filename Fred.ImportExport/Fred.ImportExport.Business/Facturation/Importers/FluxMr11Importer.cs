using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Fred.Business.Facturation;
using Fred.Entities;
using Fred.Entities.Facturation;
using Fred.ImportExport.Business.Sap;
using Fred.ImportExport.Models.Facturation;

namespace Fred.ImportExport.Business.Facturation.Validators
{
    /// <summary>
    ///   Gestion de l'annulation FAR (Flux MR11)
    /// </summary>
    public class FluxMr11Importer : SapImporter<IEnumerable<FacturationSapModel>>
    {
        private readonly IFacturationManager facturationManager;
        private readonly IMapper mapper;

        public FluxMr11Importer(IFacturationManager facturationManager, IMapper mapper)
          : base("FACTURATION", "MR11", "Annulation FAR")
        {
            this.facturationManager = facturationManager;
            this.mapper = mapper;
        }

        protected override void ImportModel(IEnumerable<FacturationSapModel> model)
        {
            List<FacturationEnt> facturations = new List<FacturationEnt>();
            List<FacturationSapModel> facturationSapModels = model.ToList();

            NLog.LogManager.GetCurrentClassLogger().Info($"[{Contexte}][{Code}] {Libelle} : { facturationSapModels.Count } entrée(s) : { ToLog(facturationSapModels) }");

            foreach (var fSap in facturationSapModels)
            {
                string messageErreur = FredImportExportBusinessResources.ImportAnnulationFarHTErreur_Entete;

                // DebitCredit = [H: crédit, S: débit]
                if (!ValidateFlux(fSap, ref messageErreur))
                {
                    NLog.LogManager.GetCurrentClassLogger().Info(messageErreur);
                    continue;
                }

                fSap.MouvementFarHt *= fSap.DebitCredit == Constantes.MouvementComptable.Debit ? -1 : 1;

                FacturationSapValidationResult result = FluxMR11Validation(fSap);
                FacturationEnt fFred = mapper.Map<FacturationEnt>(fSap);

                fFred.CommandeId = result.Commande?.CommandeId;
                fFred.DeviseId = result.Devise?.DeviseId;
                fFred.CiId = result.Reception?.CiId;
                result.Reception.DeviseId = result.Devise?.DeviseId;

                AnnulationFar.Process(result, fSap, fFred, GetFredIeUtilisateur());

                facturations.Add(fFred);
            }

            facturationManager.BulkInsert(facturations);
        }

        private bool ValidateFlux(FacturationSapModel fSap, ref string messageErreur)
        {
            bool valide = true;
            if (fSap.MouvementFarHt < 0)
            {
                valide = false;
                messageErreur += string.Format(FredImportExportBusinessResources.ImportAnnulationFarHTErreur_Detail, fSap.CommandePosteSapNumero);
            }
            return valide;
        }

        /// <summary>
        ///     Validation du flux Annulation FAR / MR11
        /// </summary>
        /// <param name="f">Objet facturation reçu de SAP</param>
        /// <returns>Resultat de validation</returns>
        private FacturationSapValidationResult FluxMR11Validation(FacturationSapModel f)
        {
            // RG_3656_111
            GetFacturationsByReceptionID(f.ReceptionId.Value, (int)f.MontantHT, f.DateSaisie);

            return new FacturationSapValidationResult
            {
                // RG_3530_043
                Commande = GetCommande(f.CommandeNumero),
                // RG_3530_044
                Reception = GetReception(f.ReceptionId.Value, f.DateComptable),
                // RG_3530_045
                Devise = GetDevise(f.DeviseIsoCode)
            };
        }

        private string ToLog(List<FacturationSapModel> facturationSapModels)
        {
            StringBuilder str = new StringBuilder();
            List<string> result = new List<string>();

            foreach (var f in facturationSapModels)
            {
                str.Append("{ ");
                str.Append("TypeLigneFactureCode:").Append(f.TypeLigneFactureCode).Append(", ");
                str.Append("CommandeNumero:").Append(f.CommandeNumero).Append(", ");
                str.Append("ReceptionId:").Append(f.ReceptionId).Append(", ");
                str.Append("CiCode:").Append(f.CiCode).Append(", ");
                str.Append(" }");
                result.Add(str.ToString());
            }

            return string.Join(",", result);
        }
    }
}
