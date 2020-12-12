using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Budget;
using Fred.Framework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Fred.EntityFramework;

namespace Fred.DataAccess.Budget
{
    /// <inheritdoc />
    public class RessourceTacheRepositoryOld : FredRepository<RessourceTacheEnt>, IRessourceTacheRepositoryOld
    {
        /// <summary>
        ///   Initialise une nouvelle instance de la classe <see cref="RessourceTacheRepositoryOld" />.
        /// </summary>
        /// <param name="logMgr">Log Manager</param>
        public RessourceTacheRepositoryOld(FredDbContext context)
          : base(context) { }

        /// <inheritdoc />
        public ICollection<RessourceTacheEnt> UpdateRessourceTaches(int tacheId, List<RessourceTacheEnt> ressourceTaches)
        {
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    // On récupére la liste courante.
                    var oldRessourceTaches = Context.RessourceTaches.Include("RessourceTacheDevises").Where(r => r.TacheId == tacheId).ToList();

                    //Suppression des anciennes ressources tâches
                    var ressourceTachesToRemove = oldRessourceTaches.Except(ressourceTaches, new RessourceTacheComparer());
                    foreach (var ressourceTache in ressourceTachesToRemove)
                    {
                        //Suppression des ressources tâches devises     
                        Context.RessourceTacheDevises.RemoveRange(ressourceTache.RessourceTacheDevises);

                        Context.RessourceTaches.Remove(ressourceTache);
                        Context.SaveChanges();
                    }

                    // Mise à jour des ressources tâches
                    var ressourceTachesToModify = ressourceTaches.Where(r => r.RessourceTacheId != 0);
                    foreach (var ressourceTache in ressourceTachesToModify)
                    {
                        var oldRessourceTache = Context.RessourceTaches.Find(ressourceTache.RessourceTacheId);
                        if (oldRessourceTache == null)
                        {
                            throw new DataException($"La ressource tâche ayant l'identifiant '{ressourceTache.RessourceTacheId}' est inexistante.");
                        }

                        Context.Entry(oldRessourceTache).CurrentValues.SetValues(ressourceTache);

                        // Mise à jour des des ressources tâches devises
                        foreach (var ressourceTacheDevise in ressourceTache.RessourceTacheDevises)
                        {
                            UpdateRessourceTacheDevise(ressourceTacheDevise);
                        }

                        Context.SaveChanges();
                    }

                    // Ajout des tâches
                    var ressourceTachesToAdd = ressourceTaches.Except(oldRessourceTaches, new RessourceTacheComparer());
                    foreach (var ressourceTache in ressourceTachesToAdd)
                    {
                        ressourceTache.Ressource = null;
                        Context.RessourceTaches.Add(ressourceTache);
                        Context.SaveChanges();
                    }

                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                }

                return ressourceTaches;
            }
        }

        /// <summary>
        /// Met a jour RessourceTacheDeviseEnt
        /// </summary>
        /// <param name="ressourceTacheDevise">ressourceTacheDevise a mettre a jour</param>
        public void UpdateRessourceTacheDevise(RessourceTacheDeviseEnt ressourceTacheDevise)
        {
            var oldRessourceTacheDevise = Context.RessourceTacheDevises.Find(ressourceTacheDevise.RessourceTacheDeviseId);
            if (oldRessourceTacheDevise == null)
            {
                Context.RessourceTacheDevises.Add(ressourceTacheDevise);
            }
            else
            {
                Context.Entry(oldRessourceTacheDevise).CurrentValues.SetValues(ressourceTacheDevise);
            }

            Context.SaveChanges();
        }
    }
}
