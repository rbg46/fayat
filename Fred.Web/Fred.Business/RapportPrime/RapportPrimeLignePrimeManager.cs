using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities.RapportPrime;

namespace Fred.Business.RapportPrime
{
    public class RapportPrimeLignePrimeManager : Manager<RapportPrimeLignePrimeEnt, IRapportPrimeLignePrimeRepository>, IRapportPrimeLignePrimeManager
    {
        private readonly IRapportPrimeLigneManager rapportPrimeLigneManager;
        private readonly IUtilisateurManager utilisateurManager;

        public RapportPrimeLignePrimeManager(
            IUnitOfWork uow,
            IRapportPrimeLignePrimeRepository rapportPrimeLignePrimeRepository,
            IRapportPrimeLigneManager rapportPrimeLigneManager,
            IUtilisateurManager utilisateurManager)
            : base(uow, rapportPrimeLignePrimeRepository)
        {
            this.rapportPrimeLigneManager = rapportPrimeLigneManager;
            this.utilisateurManager = utilisateurManager;
        }

        /// <summary>
        /// Indique si une ligne de prime peux être envoyé vers Anael
        /// </summary>
        /// <param name="rapportPrimeLigne">Ligne de prime du rapport de prime</param>
        /// <returns>True si la ligne peux être envoyée</returns>
        public bool CanBeSendToAnael(RapportPrimeLigneEnt rapportPrimeLigne)
        {
            return rapportPrimeLigneManager.IsLineValidated(rapportPrimeLigne);
        }

        /// <summary>
        /// Renvoie la liste des lignes de rapports de ligne de primes donné sur une periode
        /// </summary>
        /// <param name="periode">Periode choisie</param>
        /// <param name="isSendToAnael">Indique si l'on doit retourner les lignes déjà envoyé a ANAEL</param>
        /// <returns>Liste de RapportPrimeLignePrimeEnt</returns>
        public List<RapportPrimeLignePrimeEnt> GetRapportPrimeLignePrime(DateTime periode, bool isSendToAnael)
        {
            return Repository.GetRapportPrimeLignePrime(periode).Where(q => q.IsSendToAnael == isSendToAnael && q.Montant > 0).ToList();
        }

        /// <summary>
        /// Permet la mise à jour de certains champs de l'entité <see cref="RapportPrimeLignePrimeEnt"/>
        /// </summary>
        /// <param name="rapportPrimeLignePrimeEnt">Entité  <see cref="RapportPrimeLignePrimeEnt"/></param>
        /// <param name="fieldsToUpdate">List des champs à mettre à jour</param>
        public void QuickUpdate(RapportPrimeLignePrimeEnt rapportPrimeLignePrimeEnt, List<Expression<Func<RapportPrimeLignePrimeEnt, object>>> fieldsToUpdate)
        {
            rapportPrimeLignePrimeEnt.UpdateUtilisateurId = utilisateurManager.GetContextUtilisateurId();
            rapportPrimeLignePrimeEnt.UpdateDate = DateTime.UtcNow;

            fieldsToUpdate.Add(x => x.UpdateUtilisateurId);
            fieldsToUpdate.Add(x => x.UpdateDate);

            Repository.Update(rapportPrimeLignePrimeEnt, fieldsToUpdate);
        }

    }
}
