using Fred.Entities;
using Fred.Entities.Depense;
using Fred.Entities.Facturation;
using Fred.Entities.Utilisateur;
using Fred.Framework.Extensions;
using Fred.ImportExport.Models.Facturation;

namespace Fred.ImportExport.Business.Facturation
{
  public static class ChargementDechargement
  {
    public static void Process(FacturationSapValidationResult result, FacturationSapModel fSap, FacturationEnt fFred, UtilisateurEnt fredie)
    {
      // RG_3656_043
      DepenseAchatEnt depAchatFacturationMontant = FacturationHelper.UpdateDepenseAchat(
        depAchat: FacturationHelper.CreateDepenseAchatBase(result.Reception, fredie),
        depenseType: DepenseType.AjustementFar,
        puHt: fSap.Quantite != 0 ? fSap.MouvementFarHt / fSap.Quantite : fSap.MouvementFarHt,
        quantite: fSap.Quantite != 0 ? fSap.Quantite : 1,
        date: fSap.DateComptable,
        dateCompta: fSap.DateComptable,
        dateFacture: fSap.DateComptable,
        dateOperation: fSap.DateComptable,
        afficherQte: false,
        afficherPuHt: true,
        montantHtInitial: null);

      depAchatFacturationMontant.CompteComptable = fSap.CompteComptable;
      depAchatFacturationMontant.CommandeLigne = null;
      depAchatFacturationMontant.Devise = result.Devise;                    // Provient de fSap.DeviseIsoCode
      depAchatFacturationMontant.Commentaire = string.Empty;
      depAchatFacturationMontant.Commentaire = $"{result.Reception.Commentaire ?? string.Empty} {fSap.NumeroFactureFournisseur ?? string.Empty} {fSap.Commentaire ?? string.Empty}";

      fFred.FacturationTypeId = fSap.Operation == OperationType.Chargement.ToStringValue() ? FacturationType.ChargementProvisionFar.ToIntValue() : FacturationType.DechargementProvisionFar.ToIntValue();
      fFred.DepenseAchatFar = depAchatFacturationMontant;
    }
  }
}
