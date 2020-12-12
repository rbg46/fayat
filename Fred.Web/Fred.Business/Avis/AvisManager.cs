using System;
using System.Collections.Generic;
using Fred.DataAccess.Avis;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Avis;

namespace Fred.Business.Avis
{
    public class AvisManager : Manager<AvisEnt, IAvisRepository>, IAvisManager
    {
        private readonly IAvisCommandeRepository avisCommandeRepository;
        private readonly IAvisCommandeAvenantRepository avisCommandeAvenantRepository;

        public AvisManager(
            IUnitOfWork uow,
            IAvisRepository avisRepository,
            IAvisCommandeRepository avisCommandeRepository,
            IAvisCommandeAvenantRepository avisCommandeAvenantRepository)
            : base(uow, avisRepository)
        {
            this.avisCommandeRepository = avisCommandeRepository;
            this.avisCommandeAvenantRepository = avisCommandeAvenantRepository;
        }

        /// <summary>
        /// Ajouter un avis à une commande
        /// </summary>
        /// <param name="commandeId">Identifiant d'une commande</param>
        /// <param name="avis">Avis à ajouter</param>
        /// <returns>Avis ajouté (attached)</returns>
        public AvisEnt AddAvisForCommande(int commandeId, AvisEnt avis)
        {
            // Ajouter avis
            AvisEnt avisAttached = Repository.Add(avis);

            AvisCommandeEnt avisCommande = new AvisCommandeEnt()
            {
                Avis = avisAttached,
                CommandeId = commandeId
            };

            // Ajouter relation
            avisCommandeRepository.Add(avisCommande);

            return avisAttached;
        }

        /// <summary>
        /// Ajouter un avis à un avenant d'une commande
        /// </summary>
        /// <param name="commandeAvenantId">Identifiant de l'avenant</param>
        /// <param name="avis">Avis à ajouter</param>
        /// <returns>Avis ajouté</returns>
        public AvisEnt AddAvisForCommandeAvenant(int? commandeAvenantId, AvisEnt avis)
        {
            // Affectation des champs
            avis.AuteurCreationId = avis.ExpediteurId;
            avis.AuteurCreation = avis.Expediteur;
            avis.AuteurModification = avis.Expediteur;
            avis.AuteurModificationId = avis.ExpediteurId;
            avis.DateCreation = DateTime.UtcNow;
            avis.DateModification = avis.DateCreation;

            // Ajouter avis
            AvisEnt avisAttached = Repository.Add(avis);

            AvisCommandeAvenantEnt avisCommandeAvenant = new AvisCommandeAvenantEnt()
            {
                Avis = avisAttached,
                CommandeAvenantId = commandeAvenantId
            };

            // Ajouter relation
            avisCommandeAvenantRepository.Insert(avisCommandeAvenant);

            return avis;
        }

        /// <summary>
        /// Récupérer l'historique des avis sur une validation de commande
        /// </summary>
        /// <param name="commandeId">Identifiant de la commande</param>
        /// <returns>Historique des avis</returns>
        public List<AvisEnt> GetHistoriqueAvisForCommande(int commandeId)
        {
            return avisCommandeRepository.GetAvisByCommandeId(commandeId);
        }

        /// <summary>
        /// Récupérer l'historique des avis sur une validation d'un avenat sur une commande
        /// </summary>
        /// <param name="commandeAvenantId">Identifiant de l'avenant sur une commande</param>
        /// <returns>Historique des avis</returns>
        public List<AvisEnt> GetHistoriqueAvisForCommandeAvenant(int commandeAvenantId)
        {
            return avisCommandeAvenantRepository.GetAvisByCommandeAvenantId(commandeAvenantId);
        }
    }
}
