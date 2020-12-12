using Fred.Entities;
using Fred.Entities.Depense;
using Fred.Entities.Facturation;
using Fred.Entities.Utilisateur;
using Fred.Framework.Extensions;
using Fred.ImportExport.Models.Facturation;

namespace Fred.ImportExport.Business.Facturation
{
    public static class NonCommande
    {
        public static void Process(FacturationSapValidationResult result, FacturationSapModel fSap, FacturationEnt fFred, UtilisateurEnt fredie)
        {
            DepenseAchatEnt depAchatFactureNonCmdee = FacturationHelper.UpdateDepenseAchat(depAchat: FacturationHelper.CreateDepenseAchatBase(result.Reception, fredie),
                                                                                           depenseType: DepenseType.FactureNonCommande,
                                                                                           puHt: fSap.MontantHT,
                                                                                           quantite: 1,
                                                                                           date: fSap.DateFacture,
                                                                                           dateCompta: fSap.DateComptable,
                                                                                           dateFacture: fSap.DateComptable,
                                                                                           dateOperation: fSap.DateComptable,
                                                                                           afficherQte: false,
                                                                                           afficherPuHt: false,
                                                                                           montantHtInitial: null);
            depAchatFactureNonCmdee.DepenseParentId = null;
            depAchatFactureNonCmdee.NumeroBL = null;
            depAchatFactureNonCmdee.Commentaire = null;
            depAchatFactureNonCmdee.Libelle = fSap?.Commentaire + " " + result.Ressource?.Libelle;
            fFred.CommandeId = null;
            fFred.FacturationTypeId = FacturationType.ArticleNonCommande.ToIntValue();
            fFred.DepenseAchatFactureEcart = depAchatFactureNonCmdee;
        }
    }
}
