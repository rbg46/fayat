using Fred.Entities;
using Fred.Entities.Depense;
using Fred.Entities.Facturation;
using Fred.Entities.Utilisateur;
using Fred.Framework.Extensions;
using Fred.ImportExport.Models.Facturation;

namespace Fred.ImportExport.Business.Facturation
{
  public static class Facturation
  {
    public static void Process(FacturationSapValidationResult result, FacturationSapModel fSap, FacturationEnt fFred, UtilisateurEnt fredie)
    {
      DepenseAchatEnt depAchatExtourneFar, depAchatFacture, depAchatFactureEcart, depAchatAjustement;

      // RG_3656_014

      // RG_3656_015 & RG_3656_016
      ProcessExtourneFarAndFacture(result, fSap, fredie, out depAchatExtourneFar, out depAchatFacture);

      // RG_3656_017
      // RG_3656_018
      ProcessFactureEcart(result, fSap, fredie, out depAchatFactureEcart);

      // RG_3656_088
      ProcessAjustement(result, fSap, fredie, out depAchatAjustement);

      fFred.FacturationTypeId = FacturationType.Facturation.ToIntValue();
      fFred.DepenseAchatFacture = depAchatFacture;
      fFred.DepenseAchatFactureEcart = depAchatFactureEcart;
      fFred.DepenseAchatFar = depAchatExtourneFar;
      fFred.DepenseAchatAjustement = depAchatAjustement;
    }

    private static void ProcessExtourneFarAndFacture(
      FacturationSapValidationResult result,
      FacturationSapModel fSap,
      UtilisateurEnt fredie,
      out DepenseAchatEnt depAchatExtourneFar,
      out DepenseAchatEnt depAchatFacture)
    {
      depAchatExtourneFar = null;
      depAchatFacture = null;

      if (fSap.EcartQuantite < fSap.Quantite)
      {
        decimal puhtExtourneFar, quantiteExtourneFar;
        decimal puhtFacture, quantiteFacture;

        if (fSap.EcartQuantite <= 0)
        {
          // RG_3656_091
          quantiteExtourneFar = -fSap.Quantite;
          puhtExtourneFar = fSap.MouvementFarHt / quantiteExtourneFar;

          // RG_3656_093
          quantiteFacture = fSap.Quantite;
          puhtFacture = fSap.MouvementFarHt / -quantiteFacture;
        }
        else
        {
          // RG_3656_092
          puhtExtourneFar = result.Reception.PUHT;
          quantiteExtourneFar = fSap.EcartQuantite - fSap.Quantite;

          // RG_3656_094
          puhtFacture = result.Reception.PUHT;
          quantiteFacture = fSap.Quantite - fSap.EcartQuantite;
        }

        // RG_3656_015
        depAchatExtourneFar = FacturationHelper.UpdateDepenseAchat(
          depAchat: FacturationHelper.CreateDepenseAchatBase(result.Reception, fredie),
          depenseType: DepenseType.ExtourneFar,
          puHt: puhtExtourneFar,
          quantite: quantiteExtourneFar,
          date: result.Reception.Date,
          dateCompta: result.Reception.DateComptable,
          dateFacture: null,
          dateOperation: fSap.DateComptable,
          afficherQte: true,
          afficherPuHt: true,
          montantHtInitial: result.Reception.PUHT * result.Reception.Quantite);

        // RG_3656_016
        depAchatFacture = FacturationHelper.UpdateDepenseAchat(
          depAchat: FacturationHelper.CreateDepenseAchatBase(result.Reception, fredie),
          depenseType: DepenseType.Facture,
          puHt: puhtFacture,
          quantite: quantiteFacture,
          date: result.Reception.Date,
          dateCompta: result.Reception.DateComptable,
          dateFacture: fSap.DateComptable,
          dateOperation: fSap.DateComptable,
          afficherQte: true,
          afficherPuHt: true,
          montantHtInitial: result.Reception.PUHT * result.Reception.Quantite);
      }
    }

    private static void ProcessFactureEcart(
      FacturationSapValidationResult result,
      FacturationSapModel fSap,
      UtilisateurEnt fredie,
      out DepenseAchatEnt depAchatFactureEcart)
    {
      depAchatFactureEcart = null;

      if (fSap.MontantHT + fSap.MouvementFarHt != 0 && fSap.EcartQuantite <= 0)
      {
        depAchatFactureEcart = FacturationHelper.UpdateDepenseAchat(
          depAchat: FacturationHelper.CreateDepenseAchatBase(result.Reception, fredie),
          depenseType: DepenseType.FactureEcart,
          puHt: fSap.EcartPu,
          quantite: (fSap.MontantHT + fSap.MouvementFarHt) / fSap.EcartPu,
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
        var montantLigneFacture = (fSap.Quantite - fSap.EcartQuantite) * result.Reception.PUHT;

        // RG_3656_019
        if (fSap.EcartQuantite > 0 && fSap.Quantite > fSap.EcartQuantite
          && fSap.MontantHT != montantLigneFacture)
        {
          depAchatFactureEcart = FacturationHelper.UpdateDepenseAchat(
            depAchat: FacturationHelper.CreateDepenseAchatBase(result.Reception, fredie),
            depenseType: DepenseType.FactureEcart,
            puHt: (fSap.MontantHT - montantLigneFacture) / fSap.EcartQuantite,
            quantite: fSap.EcartQuantite,
            date: fSap.DateFacture,
            dateCompta: fSap.DateComptable,
            dateFacture: fSap.DateComptable,
            dateOperation: fSap.DateComptable,
            afficherQte: true,
            afficherPuHt: fSap.EcartPu == 0,
            montantHtInitial: null);
        }

        // RG_3656_020
        else if (fSap.EcartQuantite >= fSap.Quantite)
        {
          depAchatFactureEcart = FacturationHelper.UpdateDepenseAchat(
            depAchat: FacturationHelper.CreateDepenseAchatBase(result.Reception, fredie),
            depenseType: DepenseType.FactureEcart,
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

      if (fSap.EcartQuantite > 0)
      {
        var montantLigneFacture = (fSap.EcartQuantite - fSap.Quantite) * result.Reception.PUHT;

        // RG_3656_089
        if (fSap.Quantite > fSap.EcartQuantite
          && fSap.MouvementFarHt != montantLigneFacture)
        {
          depAchatAjustement = FacturationHelper.UpdateDepenseAchat(
            depAchat: FacturationHelper.CreateDepenseAchatBase(result.Reception, fredie),
            depenseType: DepenseType.AjustementFar,
            puHt: (montantLigneFacture - fSap.MouvementFarHt) / fSap.EcartQuantite,
            quantite: -fSap.EcartQuantite,
            date: fSap.DateFacture,
            dateCompta: fSap.DateComptable,
            dateFacture: fSap.DateComptable,
            dateOperation: fSap.DateComptable,
            afficherQte: true,
            afficherPuHt: fSap.EcartPu == 0,
            montantHtInitial: null);
        }

        // RG_3656_090
        else if (fSap.EcartQuantite >= fSap.Quantite)
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
