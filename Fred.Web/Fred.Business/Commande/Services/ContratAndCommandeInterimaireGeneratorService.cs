using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fred.Business.Personnel.Interimaire;
using Fred.Business.Rapport.Pointage;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Commande;
using Fred.Entities.Personnel.Interimaire;
using Fred.Entities.Rapport;
using Fred.Entities.Referential;
using Fred.Entities.Utilisateur;
using Fred.Framework.Exceptions;
using static Fred.Entities.Constantes;

namespace Fred.Business.Commande.Services
{
    public class ContratAndCommandeInterimaireGeneratorService : IContratAndCommandeInterimaireGeneratorService
    {
        private const string FREDCOMMANDPREFIX = "F";
        private readonly IUnitOfWork unitOfWork;
        private readonly IContratInterimaireManager contratInterimaireManager;
        private readonly IPointageManager pointageManager;
        private readonly ICommandeContratInterimaireManager commandeContratInterimaireManager;
        private readonly ICommandeRepository commandeRepository;
        private readonly ITacheRepository tacheRepository;
        private readonly ICIRepository ciRepository;
        private readonly IUtilisateurManager userManager;
        private readonly IStatutCommandeManager statutCommandeManager;
        private readonly ICommandeTypeManager commandeTypeManager;

        /// <summary>
        /// Initialise une nouvelle instance de la classe <see cref="ContratInterimaireController" />.
        /// </summary>  
        /// <param name="contratInterimaireManager">Manager des contrats d'interimaire</param>
        /// <param name="pointageManager">Manager des pointages</param>
        /// <param name="commandeManager">Manager des commandes</param>
        /// <param name="tacheRepository">Repo Tache</param>
        /// <param name="ciRepository">Repo CI</param>
        public ContratAndCommandeInterimaireGeneratorService(IUnitOfWork unitOfWork,
            IContratInterimaireManager contratInterimaireManager,
            IPointageManager pointageManager,
            ICommandeContratInterimaireManager commandeContratInterimaireManager,
            ICommandeRepository commandeRepository,
            ITacheRepository tacheRepository,
            ICIRepository ciRepository, IUtilisateurManager userManager,
            IStatutCommandeManager statutCommandeManager,
            ICommandeTypeManager commandeTypeManager)
        {
            this.unitOfWork = unitOfWork;
            this.contratInterimaireManager = contratInterimaireManager;
            this.pointageManager = pointageManager;
            this.commandeContratInterimaireManager = commandeContratInterimaireManager;
            this.commandeRepository = commandeRepository;
            this.tacheRepository = tacheRepository;
            this.ciRepository = ciRepository;
            this.userManager = userManager;
            this.statutCommandeManager = statutCommandeManager;
            this.commandeTypeManager = commandeTypeManager;
        }


        public async Task CreateCommandesForPointagesInterimairesAsync(ContratInterimaireEnt contratInterimaire, Func<int, Task> callback)
        {
            if (contratInterimaire == null)
                throw new ArgumentNullException(nameof(contratInterimaire));

            var pointagesInterimairesNonReceptionnees = this.pointageManager.GetPointagesInterimaireNonReceptionnees(contratInterimaire.InterimaireId, contratInterimaire.DateDebut, contratInterimaire.DateFin);

            pointagesInterimairesNonReceptionnees = pointagesInterimairesNonReceptionnees.OrderBy(p => p.DatePointage).ToList();

            var newCommandes = CreateCommandesForPointagesInterimaires(pointagesInterimairesNonReceptionnees);

            var newCallBackTasks = new List<Task>();

            foreach (var commande in newCommandes)
            {
                newCallBackTasks.Add(callback(commande.CommandeId));
            }

            await Task.WhenAll(newCallBackTasks.ToArray());
        }

        /// <summary>
        /// Creer des commandes pour des pointages interimaires
        /// </summary>
        /// <param name="listRapport"></param>
        /// <param name="callback">methode appelée apres la creation des commandes(parametre la commande Id)</param>
        public async Task CreateCommandesForPointagesInterimairesAsync(List<RapportEnt> listRapport, Func<int, Task> callback)
        {
            if (listRapport == null)
                return;

            var newCommandes = new List<CommandeEnt>();

            foreach (RapportEnt rapportEnt in listRapport)
            {
                var newCommandesForRapport = CreateCommandesForPointagesInterimaires(rapportEnt);

                newCommandes.AddRange(newCommandesForRapport);
            }

            var newCallBackTasks = new List<Task>();

            foreach (var commande in newCommandes)
            {
                newCallBackTasks.Add(callback(commande.CommandeId));
            }

            await Task.WhenAll(newCallBackTasks.ToArray()).ConfigureAwait(false);
        }

        /// <summary>
        /// Creer des commandes pour des pointages interimaires
        /// </summary>
        /// <param name="rapportsLignes">liste de rapports</param>      
        private List<CommandeEnt> CreateCommandesForPointagesInterimaires(List<RapportLigneEnt> rapportsLignes)
        {

            if (rapportsLignes == null)
                throw new ArgumentNullException(nameof(rapportsLignes));

            try
            {
                List<CommandeEnt> commandeEntList = new List<CommandeEnt>();

                foreach (RapportLigneEnt rapportLignesInterimaire in rapportsLignes)
                {
                    ContratInterimaireEnt contratInterimaireEnt = contratInterimaireManager.GetContratInterimaireByDatePointage(rapportLignesInterimaire.PersonnelId, rapportLignesInterimaire.DatePointage);

                    if (contratInterimaireEnt == null)
                    {
                        contratInterimaireEnt = contratInterimaireManager.GetContratInterimaireByDatePointageAndSouplesse(rapportLignesInterimaire.PersonnelId, rapportLignesInterimaire.DatePointage);
                    }

                    if (contratInterimaireEnt == null)
                    {
                        continue;
                    }

                    CommandeContratInterimaireEnt commandeContratInterimaire =
                        commandeContratInterimaireManager.GetCommandeContratInterimaireExist(contratInterimaireEnt.ContratInterimaireId);

                    if (commandeContratInterimaire != null)
                    {
                        continue;
                    }

                    // RG_6472_012
                    if (!(contratInterimaireEnt.Ci.Societe.TypeSociete?.Code == TypeSociete.Sep &&
                           contratInterimaireEnt.Societe.TypeSociete?.Code == TypeSociete.Partenaire &&
                           contratInterimaireEnt.Energie))
                    {
                        CommandeEnt commandeEnt = AddCommandeInterimaire(rapportLignesInterimaire, contratInterimaireEnt);
                        commandeEntList.Add(commandeEnt);
                    }
                }

                return commandeEntList;
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        public List<CommandeEnt> CreateCommandesForPointagesInterimaires(RapportEnt rapportComplet)
        {
            if (rapportComplet == null)
                throw new ArgumentNullException(nameof(rapportComplet));

            try
            {
                List<RapportLigneEnt> rapportLignesInterimaires = rapportComplet.ListLignes.Where(rl => rl.Personnel != null && rl.Personnel.IsInterimaire).ToList();

                return CreateCommandesForPointagesInterimaires(rapportLignesInterimaires);
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }




        /// <summary>
        /// Permet d'ajouter une commande intérimaire
        /// </summary>
        /// <param name="rapportLigneEnt">rapport ligne</param>
        /// <param name="contratInterimaireEnt">contrat intérimaire</param>
        /// <returns>La commande contrat intérimaire enregistrée</returns>
        private CommandeEnt AddCommandeInterimaire(RapportLigneEnt rapportLigneEnt, ContratInterimaireEnt contratInterimaireEnt)
        {
            if (contratInterimaireEnt == null)
            {
                throw new ArgumentNullException(nameof(contratInterimaireEnt));
            }

            try
            {
                int ciId = contratInterimaireEnt.CiId.Value;
                DeviseEnt deviseCIRef = null;

                // RG_6472_011
                if (contratInterimaireEnt.Ci.Societe?.TypeSociete.Code == TypeSociete.Sep &&
                    contratInterimaireEnt.Societe.TypeSociete?.Code == TypeSociete.Interne &&
                    contratInterimaireEnt.Energie)
                {
                    ciId = contratInterimaireEnt.Ci.CompteInterneSepId.Value;
                    deviseCIRef = ciRepository.GetDeviseRef(ciId);
                }
                UtilisateurEnt currentUser = userManager.GetContextUtilisateur();

                CommandeEnt commandeEnt = new CommandeEnt()
                {
                    AuteurCreationId = currentUser.UtilisateurId,
                    Numero = "Temp",
                    IsAbonnement = false,
                    CiId = ciId,
                    FournisseurId = contratInterimaireEnt.FournisseurId,
                    TypeId = commandeTypeManager.GetByCode("I").CommandeTypeId,
                    CommandeManuelle = false,
                    SuiviId = rapportLigneEnt.AuteurCreationId,
                    ContactId = rapportLigneEnt.AuteurCreationId,
                    Date = contratInterimaireEnt.DateDebut,
                    DateCreation = DateTime.Now,
                    StatutCommandeId = statutCommandeManager.GetByCode(StatutCommandeEnt.CommandeStatutVA).StatutCommandeId,
                    NumeroContratExterne = contratInterimaireEnt.NumContrat,
                    Libelle = "Commande Automatique - " + contratInterimaireEnt.NumContrat + " - " + contratInterimaireEnt.Ci.Code,
                    LivraisonAdresse = contratInterimaireEnt.Ci.AdresseLivraison,
                    LivraisonVille = contratInterimaireEnt.Ci.VilleLivraison,
                    LivraisonCPostale = contratInterimaireEnt.Ci.CodePostalLivraison,
                    FacturationAdresse = contratInterimaireEnt.Ci.AdresseFacturation,
                    FacturationVille = contratInterimaireEnt.Ci.VilleFacturation,
                    FacturationCPostale = contratInterimaireEnt.Ci.CodePostalFacturation,
                    Lignes = new List<CommandeLigneEnt>()
                };

                if (deviseCIRef != null)
                {
                    commandeEnt.DeviseId = deviseCIRef.DeviseId;
                }
                else
                {
                    commandeEnt.DeviseId = SetDeviseCi(contratInterimaireEnt);
                }

                CommandeLigneEnt commandeLigneEnt = new CommandeLigneEnt()
                {
                    AuteurCreationId = rapportLigneEnt.AuteurCreationId,
                    DateCreation = DateTime.UtcNow,
                    Libelle = "Intérim " + contratInterimaireEnt.Interimaire.Matricule.Replace("I_", string.Empty) + " " + contratInterimaireEnt.Interimaire.NomPrenom,
                    RessourceId = contratInterimaireEnt.RessourceId,
                    PUHT = contratInterimaireEnt.Valorisation,
                    Quantite = 1,
                    UniteId = contratInterimaireEnt.UniteId,
                    TacheId = tacheRepository.GetTacheIdInterimByCiId(ciId)
                };

                commandeEnt.Lignes.Add(commandeLigneEnt);

                commandeRepository.Insert(commandeEnt);
                unitOfWork.Save();

                commandeEnt.Numero = FREDCOMMANDPREFIX + commandeEnt.CommandeId.ToString().PadLeft(9, '0');
                unitOfWork.Save();

                commandeContratInterimaireManager.AddCommandeContratInterimaire(rapportLigneEnt, commandeEnt, contratInterimaireEnt);

                return commandeEnt;
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        private static int? SetDeviseCi(ContratInterimaireEnt contratInterimaireEnt)
        {
            if (contratInterimaireEnt.Ci.CIDevises != null)
            {
                foreach (var devise in contratInterimaireEnt.Ci.CIDevises)
                {
                    if (devise.Reference)
                    {
                        return devise.DeviseId;
                    }
                }
            }
            return null;
        }
    }
}
