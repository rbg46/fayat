using System;
using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Moyen;
using Fred.EntityFramework;
using Fred.Framework;
using Fred.Framework.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.Referential.Materiel
{
    /// <summary>
    ///   Représente un référentiel de données pour les matériels
    /// </summary>
    [System.Runtime.InteropServices.Guid("899CEA30-F035-4F3C-981B-B23D049CCE8A")]
    public class MaterielLocationRepository : FredRepository<MaterielLocationEnt>, IMaterielLocationRepository
    {
        private readonly ILogManager logManager;

        public MaterielLocationRepository(FredDbContext context, ILogManager logManager)
          : base(context)
        {
            this.logManager = logManager;
        }

        /// <summary>
        /// Ajout d'un matériel de type location
        /// </summary>
        /// <param name="materielLocation">Object matériel location à ajouter</param>
        /// <returns>Identifiant du matériel ajouté</returns>
        public int AddMaterielLocation(MaterielLocationEnt materielLocation)
        {
            if (materielLocation == null)
            {
                throw new FredRepositoryException($"Erreur ajout d'un materiel location null");
            }

            try
            {
                Context.MaterielLocation.Add(materielLocation);
                Context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException exception)
            {
                this.logManager.TraceException(exception.Message, exception);
                throw;
            }

            return materielLocation.MaterielLocationId;
        }

        /// <summary>
        /// Update d'un matériel de type location
        /// </summary>
        /// <param name="materielLocation">Object matériel location à modifier</param>
        /// <returns>Identifiant du matériel modifie</returns>
        public int UpdateMaterielLocation(MaterielLocationEnt materielLocation)
        {
            if (materielLocation == null)
            {
                throw new FredRepositoryException($"Erreur mise à jour d'un materiel location null");
            }
            try
            {
                Context.MaterielLocation.Attach(materielLocation);
                Context.Entry(materielLocation).State = EntityState.Modified;
                Context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException exception)
            {
                this.logManager.TraceException(exception.Message, exception);
                throw;
            }
            return materielLocation.MaterielLocationId;
        }

        /// <summary>
        /// Chercher des immatriculations des moyens en fonction des critéres fournies en paramètres 
        /// </summary>
        /// <param name="filters">Les filtres de recherche</param>
        /// <param name="chapitresIds">les Ids des chapitres moyens </param>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>        
        /// <returns>Liste des moyens</returns>
        public IEnumerable<MaterielLocationEnt> SearchLightForImmatriculation(SearchImmatriculationMoyenEnt filters, IEnumerable<int> chapitresIds, int page = 1, int pageSize = 20)
        {
            try
            {

                return Query()
                        .Filter(filters.GetPredicateWhereFromMaterielLocation())
                        .Include(c => c.Materiel.Ressource.SousChapitre.Chapitre)
                        .Filter(c => chapitresIds.Any(f => f == c.Materiel.Ressource.SousChapitre.Chapitre.ChapitreId))
                        .OrderBy(e => e.OrderBy(a => a.Immatriculation))
                        .GetPage(page, pageSize)
                        .ToList();


            }
            catch (Exception e)
            {
                throw new FredRepositoryException(e.Message, e);
            }
        }

        /// <summary>
        /// Retourne toutes les locations qui ont une date de suppression null 
        /// </summary>
        /// <returns>Retourne un enumerable des Locations</returns>
        public IEnumerable<MaterielLocationEnt> GetAllActiveLocation()
        {
            try
            {
                var query = Context.MaterielLocation.Where(m => m.DateSuppression == null);
                query.Include(m => m.Materiel.EtablissementComptable).Load();
                query.Include(m => m.Materiel.Ressource).Load();
                query.Include(m => m.Materiel.Societe).Load();
                return query;
            }
            catch (Exception e)
            {
                throw new FredRepositoryException(e.Message, e);
            }
        }

        /// <summary>
        /// Supprimer une location 
        /// </summary>
        /// <param name="materielLocationId">L'id du materiel a supprimer</param>
        public void DeleteMaterielLocation(int materielLocationId)
        {
            try
            {
                DeleteById(materielLocationId);
            }
            catch (Exception e)
            {
                throw new FredRepositoryException(e.Message, e);
            }
        }
    }
}
