using System;
using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Referential;
using Fred.Entities.Societe;
using Fred.EntityFramework;
using Fred.Framework;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.Societe
{
    /// <summary>
    ///   Référentiel de données pour les sociétés.
    /// </summary>
    public class SocieteDeviseRepository : FredRepository<SocieteDeviseEnt>, ISocieteDeviseRepository
    {
        private readonly ILogManager logManager;

        public SocieteDeviseRepository(FredDbContext context, ILogManager logManager)
          : base(context)
        {
            this.logManager = logManager;
        }

        /// <summary>
        ///   Retourne la liste de devise de la société passée en paramètre
        /// </summary>
        /// <param name="idSociete"> Identifiant de la société </param>
        /// <returns> Liste de toutes les sociétés/Devises </returns>
        public IEnumerable<DeviseEnt> GetDeviseListBySociete(int idSociete)
        {
            return
              Context.SocietesDevises.Where(x => x.SocieteId.Equals(idSociete))
                     .Select(x => x.Devise);
        }

        /// <summary>
        ///   Retourne la liste de devise de reference de la société passée en paramètre
        /// </summary>
        /// <param name="idSociete"> Identifiant de la société </param>
        /// <returns> Liste de toutes les sociétés/Devises </returns>
        public IQueryable<DeviseEnt> GetListDeviseRefBySociete(int idSociete)
        {
            return
              Context.SocietesDevises.Where(x => x.SocieteId.Equals(idSociete) && x.DeviseDeReference)
                     .Select(x => x.Devise);
        }

        /// <summary>
        ///   Retourne la liste de devise secondaire de la société passée en paramètre
        /// </summary>
        /// <param name="idSociete"> Identifiant de la société </param>
        /// <returns> Liste de toutes les sociétés/Devises </returns>
        public IQueryable<DeviseEnt> GetListDeviseSecBySociete(int idSociete)
        {
            return Context.SocietesDevises.Where(x => x.SocieteId.Equals(idSociete) && !x.DeviseDeReference)
                          .Select(x => x.Devise);
        }

        /// <summary>
        ///   Ajout une nouvelle association entre société et devise
        /// </summary>
        /// <param name="societeDeviseEnt"> Association société devise à ajouter</param>
        /// <returns> L'identifiant de la société ajoutée</returns>
        public int Add(SocieteDeviseEnt societeDeviseEnt)
        {
            try
            {
                Context.SocietesDevises.Add(societeDeviseEnt);
                Context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException exception)
            {
                this.logManager.TraceException(exception.Message, exception);
                throw;
            }

            return societeDeviseEnt.SocieteId;
        }

        /// <summary>
        ///   Suppression des association devise societe par id societe
        /// </summary>
        /// <param name="idSociete">Id de la societe</param>
        public void DeleteByIdSociete(int idSociete)
        {
            foreach (SocieteDeviseEnt societeDevise in Context.SocietesDevises.Where(x => x.SocieteId.Equals(idSociete)))
            {
                Context.SocietesDevises.Remove(societeDevise);
            }

            try
            {
                Context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException exception)
            {
                this.logManager.TraceException(exception.Message, exception);
                throw;
            }
        }

        /// <inheritdoc />
        public void DeleteById(int societeDeviseId)
        {
            SocieteDeviseEnt societeDevise = FindById(societeDeviseId);
            if (societeDevise != null)
            {
                Delete(societeDevise);
            }
        }

        /// <inheritdoc />
        public IEnumerable<SocieteDeviseEnt> AddOrUpdate(IEnumerable<SocieteDeviseEnt> societeDeviseList)
        {
            foreach (var societeDevise in societeDeviseList.ToList())
            {
                if (societeDevise.Devise != null)
                {
                    Context.Devise.Attach(societeDevise.Devise);
                }

                if (societeDevise.SocieteDeviseId.Equals(0))
                {
                    Context.SocietesDevises.Add(societeDevise);
                }
                else
                {
                    Context.SocietesDevises.Update(societeDevise);
                }
            }
            return societeDeviseList;
        }

        /// <summary>
        ///   Log une erreur se déclenchant dans le manager
        /// </summary>
        /// <param name="exception"> Exception pourtant le message d'erreur </param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Ne pas passer de littéraux en paramètres localisés", MessageId = "Fred.Framework.ILogManager.TraceException(System.String,System.Exception,System.Collections.Generic.IDictionary<System.String,System.String>)", Justification = "Log")]
        public void LogManagerException(Exception exception)
        {
            this.logManager.TraceException(string.Format("{0} - {1}", "Manager exception", exception.Message), exception);
        }

        /// <inheritdoc />
        public IEnumerable<SocieteDeviseEnt> GetSocieteDeviseList(int societeId)
        {
            return Query().Include(d => d.Devise).Filter(s => s.SocieteId.Equals(societeId)).Get().AsNoTracking();
        }
    }
}