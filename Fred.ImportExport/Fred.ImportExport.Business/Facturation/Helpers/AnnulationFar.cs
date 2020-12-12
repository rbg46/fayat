using Fred.Entities;
using Fred.Entities.Depense;
using Fred.Entities.Facturation;
using Fred.Entities.Utilisateur;
using Fred.Framework.Extensions;
using Fred.ImportExport.Models.Facturation;

namespace Fred.ImportExport.Business.Facturation
{
  public static class AnnulationFar
  {
    //RG_3656_059
    public static void Process(FacturationSapValidationResult result, FacturationSapModel fSap, FacturationEnt fFred, UtilisateurEnt fredie)
    {
      DepenseAchatEnt depAnnulationFar = FacturationHelper.UpdateDepenseAchat(depAchat: FacturationHelper.CreateDepenseAchatBase(result.Reception, fredie),
                                                                              depenseType: DepenseType.AjustementFar,
                                                                              puHt: fSap.MouvementFarHt / -fSap.Quantite,
                                                                              quantite: -fSap.Quantite,
                                                                              date: fSap.DateComptable,
                                                                              dateCompta: fSap.DateComptable,
                                                                              dateFacture: fSap.DateComptable,
                                                                              dateOperation: fSap.DateComptable,
                                                                              afficherQte: true,
                                                                              afficherPuHt: true,
                                                                              montantHtInitial: null);

      depAnnulationFar.CompteComptable = fSap.CompteComptable;

      fFred.FacturationTypeId = FacturationType.AnnulationFar.ToIntValue();
      fFred.DepenseAchatFar = depAnnulationFar;
    }
  }
}
