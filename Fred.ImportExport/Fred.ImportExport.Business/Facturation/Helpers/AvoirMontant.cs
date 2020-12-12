using Fred.Entities;
using Fred.Entities.Depense;
using Fred.Entities.Facturation;
using Fred.Entities.Utilisateur;
using Fred.Framework.Extensions;
using Fred.ImportExport.Models.Facturation;
using System;

namespace Fred.ImportExport.Business.Facturation
{
  public static class AvoirMontant
  {
    public static void Process(FacturationSapValidationResult result, FacturationSapModel fSap, FacturationEnt fFred, UtilisateurEnt fredie)
    {
      // RG_3656_056
      DepenseAchatEnt depAchatFactureEcart = FacturationHelper.UpdateDepenseAchat(
        depAchat: FacturationHelper.CreateDepenseAchatBase(result.Reception, fredie),
        depenseType: DepenseType.AvoirEcart,
        puHt: fSap.Quantite != 0 ? fSap.MontantHT / fSap.Quantite : fSap.MontantHT,
        quantite: fSap.Quantite != 0 ? fSap.Quantite : 1,
        date: fSap.DateFacture,
        dateCompta: fSap.DateComptable,
        dateFacture: fSap.DateComptable,
        dateOperation: fSap.DateComptable,
        afficherQte: false,
        afficherPuHt: true,
        montantHtInitial: null);
      depAchatFactureEcart.CompteComptable = fSap.CompteComptable;

      // RG_3656_106
      DepenseAchatEnt depAchatAjustement = null;
      if (fSap.MouvementFarHt != 0)
      {
        depAchatAjustement = FacturationHelper.UpdateDepenseAchat(
          depAchat: FacturationHelper.CreateDepenseAchatBase(result.Reception, fredie),
          depenseType: DepenseType.AjustementFar,
          puHt: fSap.EcartQuantite != 0 ? fSap.MouvementFarHt / fSap.EcartQuantite : fSap.MouvementFarHt,
          quantite: fSap.EcartQuantite != 0 ? fSap.EcartQuantite : 1,
          date: fSap.DateFacture,
          dateCompta: fSap.DateComptable,
          dateFacture: fSap.DateComptable,
          dateOperation: fSap.DateComptable,
          afficherQte: false,
          afficherPuHt: true,
          montantHtInitial: null);
        depAchatAjustement.CompteComptable = fSap.CompteComptable;
        depAchatAjustement.Devise = result.Devise;                    // Provient de fSap.DeviseIsoCode
        depAchatAjustement.CommandeLigne = null;
      }

      fFred.FacturationTypeId = FacturationType.AvoirMontant.ToIntValue();
      fFred.DepenseAchatFactureEcart = depAchatFactureEcart;
      fFred.DepenseAchatAjustement = depAchatAjustement;
      fFred.MontantTotalHT = -Math.Abs(fFred.MontantTotalHT);
    }
  }
}
