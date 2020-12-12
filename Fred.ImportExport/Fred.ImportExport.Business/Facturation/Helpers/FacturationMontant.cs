using Fred.Entities;
using Fred.Entities.Depense;
using Fred.Entities.Facturation;
using Fred.Entities.Utilisateur;
using Fred.Framework.Extensions;
using Fred.ImportExport.Models.Facturation;

namespace Fred.ImportExport.Business.Facturation
{
    public static class FacturationMontant
    {
        public static void Process(FacturationSapValidationResult result, FacturationSapModel fSap, FacturationEnt fFred, UtilisateurEnt fredie)
        {
            // RG_3656_039
            DepenseAchatEnt depAchatFacturationEcart = FacturationHelper.UpdateDepenseAchat(
              depAchat: FacturationHelper.CreateDepenseAchatBase(result.Reception, fredie),
              depenseType: DepenseType.FactureEcart,
              puHt: fSap.Quantite != 0 ? fSap.MontantHT / fSap.Quantite : fSap.MontantHT,
              quantite: fSap.Quantite != 0 ? fSap.Quantite : 1,
              date: fSap.DateFacture,
              dateCompta: fSap.DateComptable,
              dateFacture: fSap.DateComptable,
              dateOperation: fSap.DateComptable,
              afficherQte: false,
              afficherPuHt: true,
              montantHtInitial: null);
            depAchatFacturationEcart.DeviseId = result.Devise.DeviseId;
            depAchatFacturationEcart.DepenseParentId = null;
            depAchatFacturationEcart.CompteComptable = fSap.CompteComptable;

            // RG_3656_105
            DepenseAchatEnt depAchatAjustement = null;
            if (fSap.MouvementFarHt != 0)
            {
                depAchatAjustement = FacturationHelper.UpdateDepenseAchat(
                  depAchat: FacturationHelper.CreateDepenseAchatBase(result.Reception, fredie),
                  depenseType: DepenseType.AjustementFar,
                  puHt: fSap.EcartQuantite != 0 ? fSap.MouvementFarHt / -fSap.EcartQuantite : fSap.MouvementFarHt,
                  quantite: fSap.EcartQuantite != 0 ? -fSap.EcartQuantite : 1,
                  date: fSap.DateFacture,
                  dateCompta: fSap.DateComptable,
                  dateFacture: fSap.DateComptable,
                  dateOperation: fSap.DateComptable,
                  afficherQte: false,
                  afficherPuHt: true,
                  montantHtInitial: null);
                depAchatAjustement.CompteComptable = fSap.CompteComptable;
                depAchatAjustement.DepenseId = result.Devise.DeviseId;                    // Provient de fSap.DeviseIsoCode
                depAchatAjustement.DepenseParentId = null;
                depAchatAjustement.CommandeLigne = null;
            }

            fFred.FacturationTypeId = FacturationType.FacturationMontant.ToIntValue();
            fFred.DepenseAchatFactureEcart = depAchatFacturationEcart;
            fFred.DepenseAchatAjustement = depAchatAjustement;
        }
    }
}
