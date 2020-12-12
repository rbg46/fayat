using Fred.Entities;
using Fred.Entities.Depense;
using Fred.Entities.Utilisateur;
using Fred.Framework.Extensions;
using System;

namespace Fred.ImportExport.Business.Facturation
{
  public static class FacturationHelper
  {
    public static DepenseAchatEnt CreateDepenseAchatBase(DepenseAchatEnt reception, UtilisateurEnt fredie)
    {
      // RG_3656_021
      return new DepenseAchatEnt
      {
        CiId = reception?.CiId,
        DeviseId = reception?.DeviseId,
        UniteId = reception?.UniteId,
        FournisseurId = reception?.FournisseurId,
        DepenseParentId = reception?.DepenseId,
        Libelle = reception?.Libelle,
        TacheId = reception?.TacheId,
        RessourceId = reception?.RessourceId,
        DateVisaReception = null,
        AuteurVisaReceptionId = null,
        Commentaire = reception?.Commentaire,
        NumeroBL = reception?.NumeroBL,
        DateCreation = DateTime.UtcNow,
        AuteurCreationId = fredie?.UtilisateurId,
        CommandeLigneId = reception?.CommandeLigneId
      };
    }

#pragma warning disable S107 // Methods should not have too many parameters
    public static DepenseAchatEnt UpdateDepenseAchat(DepenseAchatEnt depAchat,
                                                     DepenseType depenseType,
                                                     decimal puHt,
                                                     decimal quantite,
                                                     DateTime? date,
                                                     DateTime? dateCompta,
                                                     DateTime? dateFacture,
                                                     DateTime? dateOperation,
                                                     bool afficherQte,
                                                     bool afficherPuHt,
                                                     decimal? montantHtInitial)
#pragma warning restore S107 // Methods should not have too many parameters
    {
      depAchat.DepenseTypeId = depenseType.ToIntValue();
      depAchat.PUHT = puHt;
      depAchat.Quantite = quantite;
      depAchat.Date = date;
      depAchat.DateComptable = dateCompta;
      depAchat.DateFacturation = dateFacture;
      depAchat.DateOperation = dateOperation;
      depAchat.AfficherQuantite = afficherQte;
      depAchat.AfficherPuHt = afficherPuHt;
      depAchat.MontantHtInitial = montantHtInitial;

      return depAchat;
    }
  }
}
