using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Fred.Business.Avis;
using Fred.Business.Referential;
using Fred.Business.Template.Models.Avis;
using Fred.Business.Template.Models.CommandeValidation;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Avis;
using Fred.Entities.Commande;
using Fred.Entities.Referential;
using Fred.Entities.Utilisateur;
using Fred.Framework.Services;
using Fred.Framework.Templating;

namespace Fred.Business.Commande.Services
{
    public class CommandeValidationRequestService : ICommandeValidationRequestService
    {
        private readonly IAvisManager avisManager;
        private readonly IUtilisateurManager utilisateurManager;
        private readonly IFournisseurManager fournisseurManager;
        private readonly ISearchCommandeService searchCommandeService;
        private readonly IUnitOfWork uow;

        public CommandeValidationRequestService(
            IAvisManager avisManager,
            IUtilisateurManager utilisateurManager,
            IFournisseurManager fournisseurManager,
            ISearchCommandeService searchCommandeService,
            IUnitOfWork uow)
        {
            this.avisManager = avisManager;
            this.utilisateurManager = utilisateurManager;
            this.fournisseurManager = fournisseurManager;
            this.searchCommandeService = searchCommandeService;
            this.uow = uow;
        }

        /// <summary>
        /// Demander une validation sur une commande / commandeAvenant
        /// </summary>
        /// <param name="commandeId">Identifiant de la commande</param>
        /// <param name="commandeAvenantId">Identifiant de l'avenant</param>
        /// <param name="avis">Avis à sauvegarder</param>
        /// <param name="baseUrl">Base url</param>
        public void RequestValidation(int commandeId, int? commandeAvenantId, AvisEnt avis, string baseUrl)
        {
            List<AvisEnt> historiqueAvis;
            CommandeAvenantEnt commandeAvenant = null;
            string subject;

            List<Expression<Func<CommandeEnt, bool>>> filter = new List<Expression<Func<CommandeEnt, bool>>>
            {
                x => x.CommandeId == commandeId
            };

            List<Expression<Func<CommandeEnt, object>>> includeProperties = new List<Expression<Func<CommandeEnt, object>>>
            {
                x => x.Lignes.Select(l => l.AvenantLigne.Avenant)
            };

            // Récupérer une commande / commandeAvenant
            CommandeEnt commande = searchCommandeService.Search(filter, null, includeProperties: includeProperties, asNoTracking: true).FirstOrDefault();

            // Récupérer un fournisseur
            FournisseurEnt fournisseur = fournisseurManager.FindById((int)commande.FournisseurId);

            // Affecter l'utilisateur
            avis.Expediteur = utilisateurManager.GetById((int)avis.ExpediteurId, true);

            // Numéro de commande
            string numero = !string.IsNullOrEmpty(commande.NumeroCommandeExterne) ? commande.NumeroCommandeExterne : commande.Numero;

            // Process spécifique
            if (commandeAvenantId == null)
            {
                // Si commande
                avisManager.AddAvisForCommande(commandeId, avis);

                // Récupérer les avis attachés à un avenant d'une commande
                historiqueAvis = avisManager.GetHistoriqueAvisForCommande(commandeId);

                subject = string.Format(CommandeResources.MailSubjectEmailValidationCommandeAvenant, numero, fournisseur.Libelle);
            }
            else
            {
                // Si avenant sur une commande
                avisManager.AddAvisForCommandeAvenant(commandeAvenantId, avis);

                // Récupérer les avis attachés à une commande
                historiqueAvis = avisManager.GetHistoriqueAvisForCommandeAvenant((int)commandeAvenantId);

                // Récupérer la commande avenant correspondante à l'ID
                commandeAvenant = commande.Lignes
                    .Where(y => (y.AvenantLigne?.Avenant != null))
                    .Select(x => x.AvenantLigne.Avenant)
                    .FirstOrDefault(y => y.CommandeAvenantId == commandeAvenantId);

                subject = string.Format(CommandeResources.MailSubjectEmailValidationCommande, commandeAvenant.NumeroAvenant, numero, fournisseur.Libelle);
            }

            // Ajouter l'avis
            historiqueAvis.Add(avis);

            // Générer l'email du model
            CommandeValidationTemplateModel emailModel = GenerateEmailModel(commande, commandeAvenant, historiqueAvis, baseUrl);

            // Générer le html correspondant au model
            TemplatingService tempService = new TemplatingService();
            var htmlContent = tempService.GenererHtmlFromTemplate(TemplateNames.EmailCommandeValidation, emailModel);

            // Récupérer le destinataire
            UtilisateurEnt destinataire = utilisateurManager.GetById((int)avis.DestinataireId, true);

            // Envoyer le mail si l'utilisateur possède un email
            if (!string.IsNullOrEmpty(destinataire.Email))
            {
                using (SendMail sender = new SendMail())
                {
                    sender.SendFormattedEmail("support.FRED@fayatit.fayat.com", destinataire.Email, subject, htmlContent, true);
                }
            }

            // Sauvegarder les données
            uow.Save();
        }

        private CommandeValidationTemplateModel GenerateEmailModel(CommandeEnt commande, CommandeAvenantEnt commandeAvenant, List<AvisEnt> historiqueAvis, string baseUrl)
        {
            // Modèle à retourner
            CommandeValidationTemplateModel emailModel = new CommandeValidationTemplateModel();
            emailModel.CommandeId = commande.CommandeId;
            emailModel.CommandeAvenantId = commandeAvenant?.CommandeAvenantId;
            emailModel.Url = baseUrl + "/Commande/Commande/Detail/" + commande.CommandeId;

            // Calculer le montant HT
            commande.ComputeMontantHT();

            if (commandeAvenant == null)
            {
                // Si commande
                emailModel.MontantTotalHt = commande.MontantHT;
            }
            else
            {
                // Si commande avenant : Calculer le montant de l'avenant
                emailModel.MontantTotalHt = commande.Lignes
                    .Where(y =>
                        (y.AvenantLigne?.Avenant != null)
                        && (y.AvenantLigne.Avenant.CommandeAvenantId == commandeAvenant.CommandeAvenantId)
                    )
                    .Sum(ligne =>
                {
                    if (ligne.AvenantLigne.IsDiminution)
                    {
                        return -ligne.Quantite * ligne.PUHT;
                    }
                    else
                    {
                        return ligne.Quantite * ligne.PUHT;
                    }
                });
            }

            emailModel.Avis = new List<AvisTemplateModel>();

            // Ordonner l'historique
            historiqueAvis = historiqueAvis.OrderByDescending(x => x.DateCreation ?? DateTime.MaxValue).ToList();

            // Ajouter tous les anciens avis
            foreach (var avisItem in historiqueAvis)
            {
                emailModel.Avis.Add(new AvisTemplateModel()
                {
                    Commentaire = avisItem.Commentaire,
                    DateCreation = avisItem.DateCreation != null ? ((DateTime)avisItem.DateCreation).ToString("dd/MM/yyyy") : " - ",
                    Expediteur = (avisItem.Expediteur != null ? avisItem.Expediteur.PrenomNom : " - "),
                    Destinataire = (avisItem.Destinataire != null ? avisItem.Destinataire.PrenomNom : " - "),
                    TypeAvis = avisItem.TypeAvis
                });
            }

            // Email model
            return emailModel;
        }
    }
}
