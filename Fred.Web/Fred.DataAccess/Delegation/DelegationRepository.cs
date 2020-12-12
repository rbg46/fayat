using System;
using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Delegation;
using Fred.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.Delegation
{
    /// <summary>
    ///   Référentiel de données pour les Délégations.
    /// </summary>
    public class DelegationRepository : FredRepository<DelegationEnt>, IDelegationRepository
    {
        private readonly IAffectationSeuilUtilisateurRepository affectationSeuilUtilisateurRepository;

        public DelegationRepository(FredDbContext context, IAffectationSeuilUtilisateurRepository affectationSeuilUtilisateurRepository)
          : base(context)
        {
            this.affectationSeuilUtilisateurRepository = affectationSeuilUtilisateurRepository;
        }

        /// <summary>
        /// Permet de récupérer une liste de délégations lié à un personnel de référence
        /// </summary>
        /// <param name="id">Personnel de référence.</param>
        /// <returns>Les délégations pour un personnel de référence.</returns>
        public List<DelegationEnt> GetDelegationByPersonnelId(int id)
        {
            return Query()
              .Include(d => d.PersonnelAuteur)
              .Include(d => d.PersonnelDelegant)
              .Include(d => d.PersonnelDelegue)
              .Filter(d => d.DateSuppression == null)
              .Get()
              .AsNoTracking()
              .Where(d => d.PersonnelDelegantId == id || d.PersonnelDelegueId == id)
              .ToList();
        }

        /// <summary>
        /// Permet de récupérer une délégations en fonction de son identifiant
        /// </summary>
        /// <param name="id">identifiant unique de la délégation</param>
        /// <returns>La délégation</returns>
        public DelegationEnt GetDelegationById(int id)
        {
            return Query()
              .Include(d => d.PersonnelAuteur)
              .Include(d => d.PersonnelDelegant)
              .Include(d => d.PersonnelDelegue)
              .Get()
              .AsNoTracking()
              .Where(d => d.DelegationId == id)
              .FirstOrDefault();
        }

        /// <summary>
        /// Permet de récupérer les délégations qui s'active le jour même
        /// </summary>
        /// <returns>La délégation</returns>
        public List<DelegationEnt> GetDelegationStartNow()
        {
            return Query()
              .Get()
              .Where(d => d.DateDeDebut <= DateTime.Today && d.Activated == false && d.DateDesactivation == null && d.DateSuppression == null)
              .ToList();
        }


        /// <summary>
        /// Permet de récupérer les délégations qui se désactive le jour même
        /// </summary>
        /// <returns>La délégation</returns>
        public List<DelegationEnt> GetDelegationStopNow()
        {
            return Query()
              .Get()
              .Where(d => d.DateDeFin < DateTime.Today && d.Activated == true && d.DateDesactivation == null)
              .ToList();
        }

        /// <summary>
        /// Permet de récupérer si une délégation est active ou non dans une période donnée à un personnel délégué
        /// </summary>
        /// <param name="delegationEnt">identifiant unique de la délégation</param>
        /// <returns>0 si aucune délégation active dans la période sinon 1</returns>
        public int GetDelegationAlreadyActive(DelegationEnt delegationEnt)
        {
            delegationEnt.DateDeDebut = delegationEnt.DateDeDebut.Date;
            delegationEnt.DateDeFin = delegationEnt.DateDeFin.Date;

            return Query()
              .Filter(d => d.DateSuppression == null)
              .Filter(d => d.DateDesactivation == null)
              .Filter(d => d.DelegationId != delegationEnt.DelegationId)
              .Filter(d => d.PersonnelDelegueId == delegationEnt.PersonnelDelegueId || d.PersonnelDelegantId == delegationEnt.PersonnelDelegantId || d.PersonnelDelegantId == delegationEnt.PersonnelDelegueId || d.PersonnelDelegueId == delegationEnt.PersonnelDelegantId)
              .Filter(d => (d.DateDeDebut <= delegationEnt.DateDeDebut && delegationEnt.DateDeDebut <= d.DateDeFin) || (d.DateDeDebut <= delegationEnt.DateDeFin && delegationEnt.DateDeFin <= d.DateDeFin) || (delegationEnt.DateDeDebut <= d.DateDeDebut && d.DateDeFin <= delegationEnt.DateDeFin))
              .Get()
              .Count();
        }

        /// <summary>
        /// Permet d'ajouter une délégation
        /// </summary>
        /// <param name="delegationEnt">Délégation</param>
        /// <returns>La délégation enregistrée</returns>
        public DelegationEnt AddDelegation(DelegationEnt delegationEnt)
        {
            delegationEnt.DateDeDebut = delegationEnt.DateDeDebut.Date;
            delegationEnt.DateDeFin = delegationEnt.DateDeFin.Date;
            Insert(delegationEnt);

            return delegationEnt;
        }

        /// <summary>
        /// Permet de modifier une délégation
        /// </summary>
        /// <param name="delegationEnt">Délégation</param>
        /// <returns>La délégation enregistrée</returns>
        public DelegationEnt UpdateDelegation(DelegationEnt delegationEnt)
        {
            delegationEnt.DateDeDebut = delegationEnt.DateDeDebut.Date;
            delegationEnt.DateDeFin = delegationEnt.DateDeFin.Date;
            Update(delegationEnt);

            return delegationEnt;
        }

        /// <summary>
        /// Permet d'activer et de désactiver une délégation si le jour de  début et de fin est aujourd'hui
        /// </summary>
        public void ActivateAndDesactivateDelegation()
        {
            List<DelegationEnt> delegationEntStartList = GetDelegationStartNow();
            List<DelegationEnt> delegationEntStopList = GetDelegationStopNow();

            foreach (DelegationEnt delegationEnt in delegationEntStopList)
            {
                DesactivateDelegation(delegationEnt);
            }
            foreach (DelegationEnt delegationEnt in delegationEntStartList)
            {
                delegationEnt.Activated = true;
                UpdateDelegation(delegationEnt);
                affectationSeuilUtilisateurRepository.DelegateAffectation(delegationEnt.DelegationId, delegationEnt.PersonnelDelegantId, delegationEnt.PersonnelDelegueId);
            }
        }

        /// <summary>
        /// Permet de désactiver une délégation
        /// </summary>
        /// <param name="delegationEnt">Délégation</param>
        /// <returns>La délégation enregistrée</returns>
        public DelegationEnt DesactivateDelegation(DelegationEnt delegationEnt)
        {
            delegationEnt.Activated = false;
            delegationEnt.DateDesactivation = DateTime.Now;
            UpdateDelegation(delegationEnt);
            affectationSeuilUtilisateurRepository.RecoverAffectation(delegationEnt.DelegationId);

            return delegationEnt;
        }


        /// <summary>
        /// Permet de supprimer une délégation en fonction de son identifiant
        /// </summary>
        /// <param name="delegationEnt">délégation</param>
        public void DeleteDelegationById(DelegationEnt delegationEnt)
        {
            delegationEnt.DateSuppression = DateTime.Now;
            Update(delegationEnt);
        }
    }
}
