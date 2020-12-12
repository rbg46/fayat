using System.Collections.Generic;
using System.Linq;
using Fred.Business.Avis;
using Fred.Business.Utilisateur;
using Fred.Entities.Avis;
using Fred.Entities.Commande;
using Fred.Entities.Utilisateur;
using Fred.Web.Shared.Enum;
using Fred.Web.Shared.Models.Commande;

namespace Fred.Business.Commande.Services
{
    public class CommandeHistoriqueService : ICommandeHistoriqueService
    {
        private readonly IAvisManager avisManager;
        private readonly IUtilisateurManager utilisateurManager;
        private readonly ICommandeManager commandeManager;

        public CommandeHistoriqueService(
            IAvisManager avisManager,
            IUtilisateurManager utilisateurManager,
            ICommandeManager commandeManager)
        {
            this.avisManager = avisManager;
            this.utilisateurManager = utilisateurManager;
            this.commandeManager = commandeManager;
        }

        public List<CommandeEventModel> GetHistorique(int commandeId)
        {
            CommandeEnt commande = commandeManager.GetCommandeById(commandeId);

            List<CommandeEventModel> events = new List<CommandeEventModel>();

            // Ajouter les events sur la commande
            events.AddRange(GetEventsForCommande(commande));

            // Ajouter les events sur les avenants
            events.AddRange(GetEventsForAvenants(commande));

            // Trier par date de création
            events = events.OrderByDescending(x => x.Creation).ToList();

            return events;
        }

        /// <summary>
        /// Récupérer les events sur une commande
        /// </summary>
        /// <param name="commande">Commande concernée</param>
        /// <returns>Liste des events</returns>
        private List<CommandeEventModel> GetEventsForCommande(CommandeEnt commande)
        {
            List<CommandeEventModel> events = new List<CommandeEventModel>();

            // Event : Creation 
            events.Add(new CommandeEventModel()
            {
                Title = "Création commande",
                TypeCommandeEvent = TypeCommandeEvent.Creation,
                Auteur = commande.AuteurCreation != null ? commande.AuteurCreation.PrenomNom : " - ",
                Creation = commande.DateCreation
            });

            // Event : Validation 
            if ((commande.StatutCommande.Code == StatutCommandeEnt.CommandeStatutVA) || (commande.StatutCommande.Code == StatutCommandeEnt.CommandeStatutMVA))
            {
                events.Add(new CommandeEventModel()
                {
                    Title = "Validation commande",
                    TypeCommandeEvent = TypeCommandeEvent.ValidationCommande,
                    Auteur = commande.Valideur != null ? commande.Valideur.PrenomNom : " - ",
                    Creation = commande.DateValidation
                });
            }

            // Event : Première impression
            if (commande.DatePremiereImpressionBrouillon != null && commande.AuteurPremiereImpressionBrouillonId.HasValue)
            {
                UtilisateurEnt auteurPremiereImpression = utilisateurManager.GetById(commande.AuteurPremiereImpressionBrouillonId.Value);
                events.Add(new CommandeEventModel()
                {
                    Title = "1ère impression de brouillon",
                    TypeCommandeEvent = TypeCommandeEvent.Impression,
                    Auteur = auteurPremiereImpression.PrenomNom,
                    Creation = commande.DatePremiereImpressionBrouillon
                });
            }

            // Event : Avis sur la validation de la commande
            List<AvisEnt> avis = avisManager.GetHistoriqueAvisForCommande(commande.CommandeId);

            foreach (var avisItem in avis)
            {
                events.Add(new CommandeEventModel()
                {
                    Title = "Relecture/envoi commande",
                    TypeCommandeEvent = TypeCommandeEvent.Avis,
                    Auteur = avisItem.Expediteur != null ? avisItem.Expediteur.PrenomNom : " - ",
                    Creation = avisItem.DateCreation,
                    TypeAvis = avisItem.TypeAvis,
                    Commentaire = avisItem.Commentaire,
                    Destinataire = avisItem.Destinataire != null ? avisItem.Destinataire.PrenomNom : " - "
                });
            }

            return events;
        }


        /// <summary>
        /// Récuperer les events liés aux avenants
        /// </summary>
        /// <param name="commande">Commande concernée</param>
        /// <returns>Liste des events</returns>
        private List<CommandeEventModel> GetEventsForAvenants(CommandeEnt commande)
        {
            List<CommandeEventModel> avenantsEvents = new List<CommandeEventModel>();

            // Event : Avenant
            List<int> avenantsIds = new List<int>();
            foreach (var ligne in commande.Lignes)
            {
                if ((ligne.AvenantLigne != null) && (ligne.AvenantLigne.Avenant != null) && !avenantsIds.Contains(ligne.AvenantLigne.Avenant.CommandeAvenantId))
                {
                    // Ajouter l'id
                    avenantsIds.Add(ligne.AvenantLigne.Avenant.CommandeAvenantId);

                    // Création avenant
                    UtilisateurEnt auteurCreation = GetAvenantLigneCreationAuteur(ligne);

                    avenantsEvents.Add(new CommandeEventModel()
                    {
                        Title = "Création avenant #" + ligne.AvenantLigne.Avenant.NumeroAvenant,
                        TypeCommandeEvent = TypeCommandeEvent.Creation,
                        Auteur = auteurCreation != null ? auteurCreation.PrenomNom : " - ",
                        Creation = ligne.AvenantLigne.Avenant.DateCreation
                    });

                    // Validation avenant
                    if (ligne.AvenantLigne.Avenant.DateValidation != null)
                    {
                        UtilisateurEnt auteurValidation = GetAvenantLigneValidationAuteur(ligne);

                        avenantsEvents.Add(new CommandeEventModel()
                        {
                            Title = "Validation avenant #" + ligne.AvenantLigne.Avenant.NumeroAvenant,
                            TypeCommandeEvent = TypeCommandeEvent.ValidationAvenant,
                            Auteur = auteurValidation != null ? auteurValidation.PrenomNom : " - ",
                            Creation = ligne.AvenantLigne.Avenant.DateValidation
                        });
                    }

                    // Ajouter les events sur les avis
                    avenantsEvents.AddRange(GetEventsForAvisOnAvenants(ligne));
                }
            }

            return avenantsEvents;
        }

        /// <summary>
        /// Recupère l'auteur de la validation d'une ligne d'avenant de commande 
        /// </summary>
        /// <param name="ligne">ligne d'avenant</param>
        /// <returns>L'auteur de la validation </returns>
        private UtilisateurEnt GetAvenantLigneValidationAuteur(CommandeLigneEnt ligne)
        {
            UtilisateurEnt auteurValidation = null;
            if (ligne.AvenantLigne.Avenant.AuteurValidationId.HasValue)
            {
                auteurValidation = utilisateurManager.GetById(ligne.AvenantLigne.Avenant.AuteurValidationId.Value);
            }

            return auteurValidation;
        }

        /// <summary>
        /// Recupère l'auteur de la création d'une ligne d'avenant de commande 
        /// </summary>
        /// <param name="ligne">ligne d'avenant</param>
        /// <returns>L'auteur de la création</returns>
        private UtilisateurEnt GetAvenantLigneCreationAuteur(CommandeLigneEnt ligne)
        {
            UtilisateurEnt auteurCreation = null;
            if (ligne.AvenantLigne.Avenant.AuteurCreationId.HasValue)
            {
                auteurCreation = utilisateurManager.GetById(ligne.AvenantLigne.Avenant.AuteurCreationId.Value);
            }

            return auteurCreation;
        }

        /// <summary>
        /// Récupérer les events sur les avis donnés sur les avenants
        /// </summary>
        /// <param name="ligne">Ligne d'une commande</param>
        /// <returns>Liste des events</returns>
        private List<CommandeEventModel> GetEventsForAvisOnAvenants(CommandeLigneEnt ligne)
        {
            List<CommandeEventModel> avisEvents = new List<CommandeEventModel>();

            // Event : Avis sur les avenants
            List<AvisEnt> avisAvenants = avisManager.GetHistoriqueAvisForCommandeAvenant(ligne.AvenantLigne.Avenant.CommandeAvenantId);

            // Avis sur les avenants
            foreach (var avisAvenant in avisAvenants)
            {
                avisEvents.Add(new CommandeEventModel()
                {
                    Title = "Relecture/envoi avenant #" + ligne.AvenantLigne.Avenant.NumeroAvenant,
                    TypeCommandeEvent = TypeCommandeEvent.Avis,
                    Auteur = avisAvenant.Expediteur != null ? avisAvenant.Expediteur.PrenomNom : " - ",
                    Creation = avisAvenant.DateCreation,
                    TypeAvis = avisAvenant.TypeAvis,
                    Commentaire = avisAvenant.Commentaire,
                    Destinataire = avisAvenant.Destinataire != null ? avisAvenant.Destinataire.PrenomNom : " - "
                });
            }

            return avisEvents;
        }
    }
}
