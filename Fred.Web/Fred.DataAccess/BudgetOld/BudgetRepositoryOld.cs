using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Budget;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielFixe;
using Fred.Framework;
using Fred.Framework.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Fred.EntityFramework;

namespace Fred.DataAccess.Budget
{
    /// <summary>
    ///   Référentiel de données pour les Budgets.
    /// </summary>
    public class BudgetRepositoryOld : FredRepository<BudgetEnt>, IBudgetRepositoryOld
    {
        /// <summary>
        ///   Initialise une nouvelle instance de la classe <see cref="BudgetRepositoryOld" />.
        /// </summary>
        /// <param name="logMgr">Log Manager</param>
        public BudgetRepositoryOld(FredDbContext context)
          : base(context)
        {
        }

        /// <inheritdoc />
        public ICollection<TacheEnt> GetBudgetRevisionTaches(int revisionId)
        {
            try
            {
                return Context.Taches.Include("RessourceTaches.Ressource.ReferentielEtendus.ParametrageReferentielEtendus.Devise")
                                     .Include("RessourceTaches.Ressource.ReferentielEtendus.ParametrageReferentielEtendus.Unite")
                                     .Include("RessourceTaches.Ressource.ReferentielEtendus.ParametrageReferentielEtendus.Organisation")
                                     .Include("TacheRecettes")
                                     .Include("RessourceTaches.RessourceTacheDevises")
                                     .Include("RessourceTaches.Ressource.SousChapitre.Chapitre").ToList();
                //REWORK Data Budget : 
                //         .Where(t => t.BudgetRevisionId == revisionId).ToList()
            }
            catch (FredRepositoryException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new FredRepositoryException(e.Message, e);
            }
        }


        /// <inheritdoc />
        public ICollection<TacheEnt> GetBudgetRevisionTachesLevel4(int revisionId)
        {
            try
            {
                //REWORK Data Budget : 
                return Context.Taches.AsNoTracking().Include("RessourceTaches").Where(t => t.Niveau == 4).ToList(); //.Where(t => t.BudgetRevisionId == revisionId && t.Niveau==4).ToList()
            }
            catch (FredRepositoryException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new FredRepositoryException(e.Message, e);
            }
        }

        /// <inheritdoc />
        public TacheEnt GetTacheWithRessourceTaches(int tacheId)
        {
            try
            {
                return Context.Taches.Include("RessourceTaches.Ressource.ReferentielEtendus.ParametrageReferentielEtendus.Devise")
                                   .Include("RessourceTaches.Ressource.ReferentielEtendus.ParametrageReferentielEtendus.Unite")
                                   .Include("RessourceTaches.Ressource.ReferentielEtendus.ParametrageReferentielEtendus.Organisation")
                                   .Include("TacheRecettes")
                                   .Include("RessourceTaches.RessourceTacheDevises")
                                   .Include("RessourceTaches.Ressource.SousChapitre.Chapitre")
                                   .FirstOrDefault(t => t.TacheId == tacheId);
            }
            catch (FredRepositoryException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new FredRepositoryException(e.Message, e);
            }
        }

        /// <inheritdoc />
        public BudgetEnt GetBudget(int ciId)
        {
            try
            {
                return Context.Budgets
                 // REWORK Data Budget
                 //.Include("BudgetRevisions")
                 //.Include("BudgetRevisions.AuteurCreation.Personnel")
                 //.Include("BudgetRevisions.AuteurValidation.Personnel")
                 //.Include("BudgetRevisions.AuteurModification.Personnel")
                 .FirstOrDefault(b => b.CiId == ciId);
            }
            catch (FredRepositoryException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new FredRepositoryException(e.Message, e);
            }
        }

        /// <inheritdoc />
        public RessourceEnt AddRessource(RessourceEnt ressource)
        {
            Context.Ressources.Add(ressource);
            Context.SaveChanges();

            return ressource;
        }
    }
}