using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Business.Organisation.Tree;
using Fred.Business.RepriseDonnees.Commande.Models;
using Fred.Business.RepriseDonnees.Commande.Selector;
using Fred.Business.RepriseDonnees.Common.Selector;
using Fred.Entities.Commande;
using Fred.Entities.Depense;
using Fred.Entities.RepriseDonnees.Excel;
using Fred.Framework.Extensions;
namespace Fred.Business.RepriseDonnees.Commande.Mapper
{
    /// <summary>
    /// Mappe les données du ci excel vers le ciEnt
    /// </summary>
    public class CommandeDataMapper : ICommandeDataMapper
    {
        private const string FREDCOMMANDPREFIX = "F";

        /// <summary>
        /// Affecte les nouvelles valeurs a certains champs des cis
        /// </summary>
        /// <param name="context">context contenant les data necessaire a l'import</param>
        /// <param name="repriseExcelCommandes">les ci sous la forme excel</param>
        /// <returns>Liste de ci avec certains champs modifiés</returns>
        public CommandeTransformResult Transform(ContextForImportCommande context, List<RepriseExcelCommande> repriseExcelCommandes)
        {
            var result = new CommandeTransformResult();

            var commonFieldSelector = new CommonFieldSelector();

            var commmandesGroupedByNumeroExterne = repriseExcelCommandes.GroupBy(x => x.NumeroCommandeExterne);

            foreach (var commmandeGroupedByNumeroExterne in commmandesGroupedByNumeroExterne)
            {
                var repriseExcelCommande = commmandeGroupedByNumeroExterne.First();

                var ci = commonFieldSelector.GetCiOfDatabase(context.OrganisationTree, context.GroupeId, repriseExcelCommande.CodeSociete, repriseExcelCommande.CodeCi, context.CisUsedInExcel);
                var devise = context.DevisesUsedInExcel.First(d => string.Equals(d.IsoCode, repriseExcelCommande.CodeDevise, StringComparison.OrdinalIgnoreCase));

                var newCommande = new CommandeEnt();

                var selector = new TypeIdSelectorHelper();

                newCommande.TypeId = selector.GetTypeId(repriseExcelCommande.TypeCommande, context);
                newCommande.FournisseurId = context.FournisseurUsedInExcel.First(f => string.Equals(f.Code, repriseExcelCommande.CodeFournisseur, StringComparison.OrdinalIgnoreCase)).FournisseurId;
                newCommande.CiId = ci.CiId;
                newCommande.StatutCommandeId = context.StatutCommandeValidee.StatutCommandeId;
                newCommande.ContactId = null;
                newCommande.SuiviId = null;
                // ici je laisse en commentaire : en effet le numero ne peux etre créer qu'apres la creation de la commande
                newCommande.Numero = "FRED_001";
                newCommande.Libelle = repriseExcelCommande.LibelleEnteteCommande.Truncate(250);
                newCommande.Date = commonFieldSelector.GetDate(repriseExcelCommande.DateCommande);
                newCommande.DelaiLivraison = null;
                newCommande.DateMiseADispo = null;
                newCommande.MOConduite = false;
                newCommande.EntretienMecanique = false;
                newCommande.EntretienJournalier = false;
                newCommande.Carburant = false;
                newCommande.Lubrifiant = false;
                newCommande.FraisAmortissement = false;
                newCommande.FraisAssurance = false;
                newCommande.ConditionSociete = null;
                newCommande.ConditionPrestation = null;
                newCommande.ContactTel = null;
                newCommande.ValideurId = context.FredIeUser.UtilisateurId;
                newCommande.DateValidation = DateTime.UtcNow;
                newCommande.LivraisonEntete = null;
                newCommande.LivraisonAdresse = null;
                newCommande.LivraisonVille = null;
                newCommande.LivraisonCPostale = null;
                newCommande.FacturationAdresse = null;
                newCommande.FacturationVille = null;
                newCommande.FacturationCPostale = null;
                newCommande.DateCloture = null;
                newCommande.Justificatif = null;
                newCommande.CommentaireFournisseur = null;
                newCommande.CommentaireInterne = null;
                newCommande.CommandeManuelle = false;
                newCommande.DeviseId = devise.DeviseId;
                newCommande.AuteurCreationId = context.FredIeUser.UtilisateurId;
                newCommande.DateCreation = DateTime.UtcNow;
                newCommande.AuteurModificationId = null;
                newCommande.DateModification = null;
                newCommande.AuteurSuppressionId = null;
                newCommande.DateSuppression = null;
                newCommande.AccordCadre = false;
                newCommande.LivraisonPaysId = null;
                newCommande.FacturationPaysId = null;
                newCommande.FournisseurAdresse = null;
                newCommande.FournisseurVille = null;
                newCommande.FournisseurCPostal = null;
                newCommande.FournisseurPaysId = null;
                newCommande.HangfireJobId = null;
                newCommande.IsAbonnement = false;
                newCommande.DureeAbonnement = null;
                newCommande.FrequenceAbonnement = null;
                newCommande.DatePremiereReception = null;
                newCommande.DateProchaineReception = null;
                newCommande.NumeroContratExterne = null;
                newCommande.NumeroCommandeExterne = repriseExcelCommande.NumeroCommandeExterne;
                newCommande.SystemeExterneId = null;
                newCommande.OldFournisseurId = null;
                newCommande.PiecesJointesCommande = null;
                var createCommandeLigneReceptionResult = CreateCommandeLignesAndReceptions(newCommande, commmandeGroupedByNumeroExterne.ToList(), context);
                result.Commandes.Add(newCommande);
                result.CommandeLignes.AddRange(createCommandeLigneReceptionResult.CommandeLignes);
                result.Receptions.AddRange(createCommandeLigneReceptionResult.Receptions);
            }

            return result;
        }

        private CreateCommandeLigneReceptionResult CreateCommandeLignesAndReceptions(CommandeEnt commande, List<RepriseExcelCommande> repriseExcelCommandes, ContextForImportCommande context)
        {
            var result = new CreateCommandeLigneReceptionResult();

            var commonFieldSelector = new CommonFieldSelector();

            var quantiteSelector = new QuantiteSelector();

            var indexNumeroDeLigne = 1;

            foreach (var repriseExcelCommande in repriseExcelCommandes)
            {
                var ci = context.OrganisationTree.GetCi(context.GroupeId, repriseExcelCommande.CodeSociete, repriseExcelCommande.CodeCi);

                // pas de verif si ci est null, car la validation dois le faire
                var tache = commonFieldSelector.GetTache(ci.Id, repriseExcelCommande.CodeTache, context.TachesUsedInExcel);

                var ressource = context.RessourcesUsedInExcel.FirstOrDefault(x => x.Code == repriseExcelCommande.CodeRessource);

                var quantite = quantiteSelector.GetQuantiteCommandeLigne(repriseExcelCommande);

                var commandeLigne = new CommandeLigneEnt();

                commandeLigne.Commande = commande;
                // pas de verif si tache est null, car la validation dois le faire
                commandeLigne.TacheId = tache.TacheId;
                // pas de verif si ressource est null, car la validation dois le faire
                commandeLigne.RessourceId = ressource.RessourceId;

                commandeLigne.Libelle = repriseExcelCommande.DesignationLigneCommande.Truncate(500);
                // pas de verif si ressource est null, car la validation dois le faire
                commandeLigne.Quantite = quantite.Value;

                commandeLigne.PUHT = commonFieldSelector.GetDecimal(repriseExcelCommande.PuHt);

                commandeLigne.UniteId = context.UnitesUsedInExcel.FirstOrDefault(x => string.Equals(x.Code, repriseExcelCommande.Unite, StringComparison.OrdinalIgnoreCase)).UniteId;

                commandeLigne.AuteurCreationId = context.FredIeUser.UtilisateurId;

                commandeLigne.DateCreation = DateTime.UtcNow;

                commandeLigne.AuteurModificationId = null;

                commandeLigne.DateModification = null;

                commandeLigne.AvenantLigneId = null;

                commandeLigne.NumeroLigne = indexNumeroDeLigne;

                indexNumeroDeLigne += 1;

                result.CommandeLignes.Add(commandeLigne);

                if (CanCreateReception(repriseExcelCommande))
                {
                    result.Receptions.Add(CreateReception(commande, commandeLigne, repriseExcelCommande, context));
                }


            }
            return result;
        }

        private DepenseAchatEnt CreateReception(CommandeEnt commande, CommandeLigneEnt commandeLigne, RepriseExcelCommande repriseExcelCommande, ContextForImportCommande context)
        {
            var reception = new DepenseAchatEnt();
            var commonFieldSelector = new CommonFieldSelector();
            var quantiteSelector = new QuantiteSelector();
            reception.CommandeLigne = commandeLigne;
            reception.CiId = commande.CiId;
            reception.FournisseurId = commande.FournisseurId;
            reception.Libelle = commandeLigne.Libelle.Truncate(500);
            reception.TacheId = commandeLigne.TacheId;
            reception.RessourceId = commandeLigne.RessourceId;
            reception.Quantite = quantiteSelector.GetQuantiteReception(repriseExcelCommande);
            reception.PUHT = commandeLigne.PUHT;
            reception.Date = commonFieldSelector.GetDate(repriseExcelCommande.DateReception);
            reception.AuteurCreationId = context.FredIeUser.UtilisateurId;
            reception.DateCreation = DateTime.UtcNow;
            reception.AuteurModificationId = null;
            reception.DateModification = null;
            reception.AuteurSuppressionId = null;
            reception.DateSuppression = null;
            reception.Commentaire = null;
            reception.DeviseId = commande.DeviseId;
            reception.NumeroBL = "Reprise";
            reception.DepenseParentId = null;
            reception.UniteId = commandeLigne.UniteId;
            reception.DepenseTypeId = context.DepenseTypeReception.DepenseTypeId;
            reception.DateComptable = commonFieldSelector.GetDate(repriseExcelCommande.DateReception);
            reception.DateVisaReception = null;
            reception.DateFacturation = null;
            reception.AuteurVisaReceptionId = null;
            reception.QuantiteDepense = 0;
            reception.FarAnnulee = false;
            reception.HangfireJobId = null;
            reception.AfficherPuHt = true;
            reception.AfficherQuantite = true;
            reception.CompteComptable = null;
            reception.ErreurControleFar = null;
            reception.DateControleFar = null;
            reception.StatutVisaId = null;
            reception.DateOperation = null;
            reception.MontantHtInitial = null;
            reception.GroupeRemplacementTacheId = null;
            reception.PiecesJointesReception = null;

            return reception;
        }

        private bool CanCreateReception(RepriseExcelCommande repriseExcelCommande)
        {
            var quantiteSelector = new QuantiteSelector();
            return quantiteSelector.CanCreateReception(repriseExcelCommande);
        }


        /// <summary>
        /// Mets a jours les numeros de commande
        /// </summary>
        /// <param name="createdCommandes">createdCommandes</param>
        /// <returns>Les commandes mise a jour</returns>
        public List<CommandeEnt> UpdateNumeroDeCommandes(List<CommandeEnt> createdCommandes)
        {
            foreach (var commande in createdCommandes)
            {
                commande.Numero = FREDCOMMANDPREFIX + commande.CommandeId.ToString().PadLeft(9, '0');
            }
            return createdCommandes;
        }
    }
}
