using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Moyen;
using Fred.EntityFramework;
using Fred.Framework.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.AffectationMoyen
{
    /// <summary>
    /// Réprésente un référentiel de données des affectations des moyens
    /// </summary>
    public class AffectationMoyenRepository : FredRepository<AffectationMoyenEnt>, IAffectationMoyenRepository
    {
        public AffectationMoyenRepository(FredDbContext context)
          : base(context)
        {
        }


        /// <summary>
        /// Permet de récupérer la liste des affectation des moyens en fonction des critères de recherche.
        /// </summary>
        /// <param name="searchFilters">Filtres de recherche</param>
        /// <param name="affectationMoyenRolesFilters">Filtre en se basant sur les rôles de l'utilisateur</param>
        /// <param name="page">Page actuelle</param>
        /// <param name="pageSize">Taille de page</param>
        /// <returns>Retourne la liste filtré des affectations des moyens</returns>
        public IEnumerable<AffectationMoyenEnt> SearchWithFilters(SearchAffectationMoyenEnt searchFilters, AffectationMoyenRolesFiltersEnt affectationMoyenRolesFilters, int page, int pageSize)
        {
            return Query()
                       .Include(a => a.TypeAffectation.AffectationMoyenFamille)
                       .Include(a => a.Site)
                       .Include(a => a.Personnel)
                       .Include(a => a.MaterielLocation.Materiel.SiteAppartenance)
                       .Include(a => a.MaterielLocation.Materiel.Ressource.SousChapitre.Chapitre)
                       .Include(a => a.Ci)
                       .Include(a => a.Materiel.SiteAppartenance)
                       .Include(a => a.Materiel.Ressource.SousChapitre.Chapitre)
                       .Include(a => a.Conducteur)
                       .Include(a => a.Materiel.Societe)
                       .Include(a => a.Materiel.EtablissementComptable)
                       .Filter(affectationMoyenRolesFilters.GetPredicateWhere())
                       .Filter(searchFilters.GetPredicateWhere())
                       .OrderBy(e => e.OrderBy(a => a.Materiel.Code).ThenBy(a => a.DateDebut))
                       .GetPage(page, pageSize)
                       .ToList();
        }

        /// <inheritdoc />
        public IEnumerable<AffectationMoyenEnt> GetAffectationMoyens()
        {
            try
            {
                return Context.AffectationMoyens.AsNoTracking();
            }
            catch (Exception e)
            {
                throw new FredRepositoryException(e.Message, e);
            }
        }

        /// <inheritdoc />
        public AffectationMoyenEnt AddAffectationMoyen(AffectationMoyenEnt affectationMoyen)
        {
            try
            {
                Context.AffectationMoyens.Add(affectationMoyen);
                Context.SaveChanges();
                return affectationMoyen;
            }
            catch (Exception e)
            {
                throw new FredRepositoryException(e.Message, e);
            }
        }

        /// <summary>
        /// Permet de supprimer une affectation apres la suppression de location 
        /// </summary>
        /// <param name="idMaterielLocation">L'id du materiel a supprimer</param>
        public void DeleteAffectationMoyen(int idMaterielLocation)
        {
            try
            {
                AffectationMoyenEnt affectationMoyenEnt = Context.AffectationMoyens.Where(p => p.MaterielLocationId == idMaterielLocation).FirstOrDefault<AffectationMoyenEnt>();

                DeleteById(affectationMoyenEnt.AffectationMoyenId);
            }
            catch (Exception e)
            {
                throw new FredRepositoryException(e.Message, e);
            }
        }

        /// <summary>
        /// Get affectation moyen list by dates
        /// </summary>
        /// <param name="datesPredicate">Predicat à utiliser pour les dates des afféctations</param>
        /// <param name="typePredicate">Predicate à utiliser pour restreindre les types éligible au pointage matériel</param>
        /// <returns>La liste des affectations dans l'intervalle des dates défini par le Predicate</returns>
        public IEnumerable<AffectationMoyenEnt> GetPointageMoyenAffectations(
            Expression<Func<AffectationMoyenEnt, bool>> datesPredicate,
            Expression<Func<AffectationMoyenEnt, bool>> typePredicate)
        {
            return Query()
                       .Include(a => a.TypeAffectation)
                       .Include(a => a.Personnel)
                       .Include(a => a.Ci)
                       .Include(a => a.Materiel)
                       .Filter(datesPredicate)
                       .Filter(typePredicate)
                       .Get()
                       .AsNoTracking()
                       .ToList();
        }

        /// <summary>
        /// Retourne un enumerable des affectations moyens en fonction materielLocationId
        /// </summary>
        /// <param name="materielLocationId">Materiel location Id</param>
        /// <returns>Retourne un enumerable  des affectations associes a un materiel location</returns>
        public IEnumerable<AffectationMoyenEnt> GetAllAffectationByMaterielLocationId(int materielLocationId)
        {
            return Context.AffectationMoyens.Where(p => p.MaterielLocationId == materielLocationId).AsEnumerable<AffectationMoyenEnt>();
        }

        /// <summary>
        /// Ajouter une list des affectations moyens
        /// </summary>
        /// <param name="affectationMoyenList">List des affectations moyens</param>
        public void AddAffectationMoyenList(IEnumerable<AffectationMoyenEnt> affectationMoyenList)
        {
            try
            {
                Context.AffectationMoyens.AddRange(affectationMoyenList);
            }
            catch (Exception e)
            {
                throw new FredRepositoryException(e.Message, e);
            }
        }

        /// <summary>
        /// Récupére list des affectations moyen par identifiers
        /// </summary>
        /// <param name="affectationMoyenIds">List des Affectations moyen identifiers</param>
        /// <returns>List des affectations moyens</returns>
        public List<AffectationMoyenEnt> GetListAffectationMoyenByIds(IEnumerable<int> affectationMoyenIds)
        {
            return Context.AffectationMoyens.AsNoTracking().Where(x => affectationMoyenIds.Contains(x.AffectationMoyenId)).ToList();
        }

        /// <summary>
        /// Recuperer liste des affectations moyens idantifiants par personnel id
        /// </summary>
        /// <param name="personnelId">Personnel idantifiant</param>
        /// <returns>Liste des affectations moyen ids</returns>
        public List<int> GetAffectationMoyenIdListByPersonnelId(int personnelId)
        {
            return Context.AffectationMoyens.Where(x => x.PersonnelId.HasValue && x.PersonnelId.Value == personnelId).Select(x => x.AffectationMoyenId).ToList();
        }
    }
}
