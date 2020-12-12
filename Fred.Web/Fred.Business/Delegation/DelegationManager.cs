using System;
using System.Collections.Generic;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Delegation;
using Fred.Framework.Exceptions;

namespace Fred.Business.Delegation
{
    /// <summary>
    ///   Gestionnaire des délegation
    /// </summary>
    public class DelegationManager : Manager<DelegationEnt, IDelegationRepository>, IDelegationManager
    {
        public DelegationManager(IUnitOfWork uow, IDelegationRepository delegationRepository)
         : base(uow, delegationRepository)
        { }

        /// <summary>
        /// Permet de récupérer une liste de délégations lié à un personnel de référence
        /// </summary>
        /// <param name="id">Personnel de référence.</param>
        /// <returns>Les délégations pour un personnel de référence.</returns>
        public List<DelegationEnt> GetDelegationByPersonnelId(int id)
        {
            try
            {
                return Repository.GetDelegationByPersonnelId(id);
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }

        }

        /// <summary>
        /// Permet de récupérer si une délégation est active ou non dans une période donnée à un personnel délégué
        /// </summary>
        /// <param name="delegationEnt">identifiant unique de la délégation</param>
        /// <returns>0 si aucune délégation active dans la période sinon 1</returns>
        public int GetDelegationAlreadyActive(DelegationEnt delegationEnt)
        {
            try
            {
                return Repository.GetDelegationAlreadyActive(delegationEnt);
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }


        /// <summary>
        /// Permet d'ajouter une délégation
        /// </summary>
        /// <param name="delegationEnt">Délégation</param>
        /// <returns>La délégation enregistrée</returns>
        public DelegationEnt AddDelegation(DelegationEnt delegationEnt)
        {
            try
            {
                Repository.AddDelegation(delegationEnt);
                Save();

                ActivateAndDesactivateDelegation();

                return Repository.GetDelegationById(delegationEnt.DelegationId);
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Permet de modifier une délégation
        /// </summary>
        /// <param name="delegationEnt">Délégation</param>
        /// <returns>La délégation enregistrée</returns>
        public DelegationEnt UpdateDelegation(DelegationEnt delegationEnt)
        {
            try
            {
                Repository.UpdateDelegation(delegationEnt);
                Save();

                ActivateAndDesactivateDelegation();
                return delegationEnt;
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Permet d'activer et de désactiver une délégation si le jour de  début et de fin est aujourd'hui
        /// </summary>
        public void ActivateAndDesactivateDelegation()
        {
            try
            {
                Repository.ActivateAndDesactivateDelegation();
                Save();
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        /// <summary>
        /// Permet de désactiver une délégation
        /// </summary>
        /// <param name="delegationEnt">Délégation</param>
        /// <returns>La délégation modifiée</returns>
        public DelegationEnt DesactivateDelegation(DelegationEnt delegationEnt)
        {
            try
            {
                Repository.DesactivateDelegation(delegationEnt);
                Save();

                return Repository.GetDelegationById(delegationEnt.DelegationId);
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }


        /// <summary>
        /// Permet de supprimer une délégation en fonction de son identifiant 
        /// </summary>
        /// <param name="delegationEnt">délégation</param>
        public void DeleteDelegationById(DelegationEnt delegationEnt)
        {
            try
            {
                Repository.DeleteDelegationById(delegationEnt);
                Save();
            }
            catch (Exception e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }
    }
}
