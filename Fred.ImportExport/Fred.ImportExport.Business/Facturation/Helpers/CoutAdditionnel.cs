using Fred.Entities;
using Fred.Entities.Depense;
using Fred.Entities.Facturation;
using Fred.Entities.Utilisateur;
using Fred.Framework.Extensions;
using Fred.ImportExport.Models.Facturation;

namespace Fred.ImportExport.Business.Facturation
{
    public static class CoutAdditionnel
    {
        public static void Process(FacturationSapValidationResult result, FacturationSapModel fSap, FacturationEnt fFred, UtilisateurEnt fredie)
        {
            // RG_3656_024            
            DepenseAchatEnt depAchatFactureEcart = FacturationHelper.UpdateDepenseAchat(
              depAchat: FacturationHelper.CreateDepenseAchatBase(result.Reception, fredie),
              depenseType: DepenseType.FactureEcart,
              puHt: fSap.MontantHT,
              quantite: 1,
              date: fSap.DateFacture,
              dateCompta: fSap.DateComptable,
              dateFacture: fSap.DateComptable,
              dateOperation: fSap.DateComptable,
              afficherQte: true,
              afficherPuHt: true,
              montantHtInitial: null);

            depAchatFactureEcart.Libelle = fSap.Commentaire + " " + result.Ressource?.Libelle;
            depAchatFactureEcart.RessourceId = result.Ressource.RessourceId;
            fFred.DepenseAchatReceptionId = result.Reception.DepenseId;
            fFred.FacturationTypeId = FacturationType.CoutAdditionnel.ToIntValue();
            fFred.DepenseAchatFactureEcart = depAchatFactureEcart;
        }
    }
}
