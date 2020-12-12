using Fred.Entities;
using Fred.Entities.Depense;
using Fred.Entities.Facturation;
using Fred.Entities.Utilisateur;
using Fred.Framework.Extensions;
using Fred.ImportExport.Models.Facturation;
using System;

namespace Fred.ImportExport.Business.Facturation
{
  public static class AvoirQuantite
  {
    public static void Process(FacturationSapValidationResult result, FacturationSapModel fSap, FacturationEnt fFred, UtilisateurEnt fredie)
    {
      DepenseAchatEnt depAchatExtourneFar, depAchatAvoir, depAchatAvoirEcart, depAchatAjustement;

      // RG_3656_095 & RG_3656_096 & RG_3656_097 & RG_3656_098
      ProcessExtourneFarAndAvoir(result, fSap, fredie, out depAchatExtourneFar, out depAchatAvoir);

      // RG_3656_099 & RG_3656_100 & RG_3656_101
      ProcessAvoirEcart(result, fSap, fredie, out depAchatAvoirEcart);

      // RG_3656_102
      ProcessAjustement(result, fSap, fredie, out depAchatAjustement);

      // RG_3656_054 
      fFred.DepenseAchatFar = depAchatExtourneFar;
      fFred.DepenseAchatFacture = depAchatAvoir;
      fFred.DepenseAchatFactureEcart = depAchatAvoirEcart;
      fFred.DepenseAchatAjustement = depAchatAjustement;
      fFred.FacturationTypeId = FacturationType.AvoirQuantite.ToIntValue();
      fFred.MontantTotalHT = -Math.Abs(fFred.MontantTotalHT);
    }

    private static void ProcessExtourneFarAndAvoir(
      FacturationSapValidationResult result,
      FacturationSapModel fSap,
      UtilisateurEnt fredie,
      out DepenseAchatEnt depAchatExtourneFar,
      out DepenseAchatEnt depAchatAvoir)
    {
      depAchatExtourneFar = null;
      depAchatAvoir = null;

      // RG_3656_047 & RG_3656_048
      if (fSap.EcartQuantite < 0)
      {
        // RG_3656_095 & RG_3656_097
        if (fSap.EcartQuantite <= fSap.Quantite)
        {
          // RG_3656_095
          depAchatExtourneFar = FacturationHelper.UpdateDepenseAchat(
            depAchat: FacturationHelper.CreateDepenseAchatBase(result.Reception, fredie),
            depenseType: DepenseType.ExtourneFar,
            puHt: fSap.MouvementFarHt / -fSap.Quantite,
            quantite: -fSap.Quantite,
            date: result.Reception.Date,
            dateCompta: result.Reception.DateComptable,
            dateFacture: null,
            dateOperation: fSap.DateComptable,
            afficherQte: true,
            afficherPuHt: true,
            montantHtInitial: result.Reception.PUHT * result.Reception.Quantite);

          // RG_3656_097
          depAchatAvoir = FacturationHelper.UpdateDepenseAchat(
            depAchat: FacturationHelper.CreateDepenseAchatBase(result.Reception, fredie),
            depenseType: DepenseType.Avoir,
            puHt: fSap.MouvementFarHt / -fSap.Quantite,
            quantite: fSap.Quantite,
            date: result.Reception.Date,
            dateCompta: result.Reception.DateComptable,
            dateFacture: fSap.DateComptable,
            dateOperation: fSap.DateComptable,
            afficherQte: true,
            afficherPuHt: true,
            montantHtInitial: result.Reception.PUHT * result.Reception.Quantite);
        }

        // RG_3656_096 & RG_3656_098
        else
        {
          // RG_3656_096
          depAchatExtourneFar = FacturationHelper.UpdateDepenseAchat(
            depAchat: FacturationHelper.CreateDepenseAchatBase(result.Reception, fredie),
            depenseType: DepenseType.ExtourneFar,
            puHt: result.Reception.PUHT,
            quantite: -fSap.EcartQuantite,
            date: result.Reception.Date,
            dateCompta: result.Reception.DateComptable,
            dateFacture: null,
            dateOperation: fSap.DateComptable,
            afficherQte: true,
            afficherPuHt: true,
            montantHtInitial: result.Reception.PUHT * result.Reception.Quantite);

          // RG_3656_098
          depAchatAvoir = FacturationHelper.UpdateDepenseAchat(
            depAchat: FacturationHelper.CreateDepenseAchatBase(result.Reception, fredie),
            depenseType: DepenseType.Avoir,
            puHt: result.Reception.PUHT,
            quantite: fSap.EcartQuantite,
            date: result.Reception.Date,
            dateCompta: result.Reception.DateComptable,
            dateFacture: fSap.DateComptable,
            dateOperation: fSap.DateComptable,
            afficherQte: true,
            afficherPuHt: true,
            montantHtInitial: result.Reception.PUHT * result.Reception.Quantite);
        }
      }
    }

    private static void ProcessAvoirEcart(
      FacturationSapValidationResult result,
      FacturationSapModel fSap,
      UtilisateurEnt fredie,
      out DepenseAchatEnt depAchatAvoirEcart)
    {
      depAchatAvoirEcart = null;

      // RG_3656_099
      if (fSap.MontantHT + fSap.MouvementFarHt != 0 && fSap.EcartQuantite <= fSap.Quantite)
      {
        depAchatAvoirEcart = FacturationHelper.UpdateDepenseAchat(
          depAchat: FacturationHelper.CreateDepenseAchatBase(result.Reception, fredie),
          depenseType: DepenseType.AvoirEcart,
          puHt: fSap.EcartPu,
          quantite: (fSap.MouvementFarHt + fSap.MontantHT) / fSap.EcartPu,
          date: fSap.DateFacture,
          dateCompta: fSap.DateComptable,
          dateFacture: fSap.DateComptable,
          dateOperation: fSap.DateComptable,
          afficherQte: false,
          afficherPuHt: true,
          montantHtInitial: null);
      }
      else
      {
        var montantTotalEcartQuantite = fSap.EcartQuantite * result.Reception.PUHT;

        // RG_3656_100
        if (fSap.EcartQuantite < 0 && fSap.Quantite < fSap.EcartQuantite
          && fSap.MontantHT != montantTotalEcartQuantite)
        {
          depAchatAvoirEcart = FacturationHelper.UpdateDepenseAchat(
            depAchat: FacturationHelper.CreateDepenseAchatBase(result.Reception, fredie),
            depenseType: DepenseType.AvoirEcart,
            puHt: (fSap.MontantHT - montantTotalEcartQuantite) / (fSap.Quantite - fSap.EcartQuantite),
            quantite: fSap.Quantite - fSap.EcartQuantite,
            date: fSap.DateFacture,
            dateCompta: fSap.DateComptable,
            dateFacture: fSap.DateComptable,
            dateOperation: fSap.DateComptable,
            afficherQte: true,
            afficherPuHt: fSap.EcartPu == 0,
            montantHtInitial: null);
        }

        // RG_3656_101
        else if (fSap.EcartQuantite >= 0)
        {
          depAchatAvoirEcart = FacturationHelper.UpdateDepenseAchat(
            depAchat: FacturationHelper.CreateDepenseAchatBase(result.Reception, fredie),
            depenseType: DepenseType.AvoirEcart,
            puHt: fSap.MontantHT / fSap.Quantite,
            quantite: fSap.Quantite,
            date: fSap.DateFacture,
            dateCompta: fSap.DateComptable,
            dateFacture: fSap.DateComptable,
            dateOperation: fSap.DateComptable,
            afficherQte: true,
            afficherPuHt: true,
            montantHtInitial: null);
        }
      }
    }

    private static void ProcessAjustement(
      FacturationSapValidationResult result,
      FacturationSapModel fSap,
      UtilisateurEnt fredie,
      out DepenseAchatEnt depAchatAjustement)
    {
      depAchatAjustement = null;

      // RG_3656_102
      if (fSap.Quantite < fSap.EcartQuantite)
      {
        var montantTotalEcartQuantite = fSap.EcartQuantite * result.Reception.PUHT;

        // RG_3656_103
        if (fSap.EcartQuantite < 0 && fSap.MouvementFarHt != -montantTotalEcartQuantite)
        {
          depAchatAjustement = FacturationHelper.UpdateDepenseAchat(
            depAchat: FacturationHelper.CreateDepenseAchatBase(result.Reception, fredie),
            depenseType: DepenseType.AjustementFar,
            puHt: (fSap.MouvementFarHt + montantTotalEcartQuantite) / (fSap.EcartQuantite - fSap.Quantite),
            quantite: fSap.EcartQuantite - fSap.Quantite,
            date: fSap.DateFacture,
            dateCompta: fSap.DateComptable,
            dateFacture: fSap.DateComptable,
            dateOperation: fSap.DateComptable,
            afficherQte: true,
            afficherPuHt: fSap.EcartPu == 0,
            montantHtInitial: null);
        }

        // RG_3656_104
        else if (fSap.EcartQuantite >= 0)
        {
          depAchatAjustement = FacturationHelper.UpdateDepenseAchat(
            depAchat: FacturationHelper.CreateDepenseAchatBase(result.Reception, fredie),
            depenseType: DepenseType.AjustementFar,
            puHt: fSap.MouvementFarHt / -fSap.Quantite,
            quantite: -fSap.Quantite,
            date: fSap.DateFacture,
            dateCompta: fSap.DateComptable,
            dateFacture: fSap.DateComptable,
            dateOperation: fSap.DateComptable,
            afficherQte: true,
            afficherPuHt: fSap.EcartPu == 0,
            montantHtInitial: null);
        }
      }
    }
  }
}
