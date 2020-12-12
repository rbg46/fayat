using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Entities.Budget.Dao.BudgetComparaison.Comparaison;
using Fred.Entities.ReferentielFixe;
using Fred.EntityFramework;
using Fred.Framework;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.ReferentielFixe
{
    /// <summary>
    ///   Référentiel de données pour les chapitres
    /// </summary>
    public class ChapitreRepository : FredRepository<ChapitreEnt>, IChapitreRepository
    {
        private readonly FredDbContext context;

        /// <summary>
        ///   Initialise une nouvelle instance de la classe <see cref="ChapitreRepository" />.
        /// </summary>
        /// <param name="logMgr">Log Manager</param>
        /// <param name="context">Le contexte.</param>
        public ChapitreRepository(FredDbContext context)
          : base(context)
        {
            this.context = context;
        }

        /// <summary>
        ///   Appel la procédure stockée VerificationDeDependance qui permet de vérifier les dépendances
        /// </summary>
        /// <param name="chapter">élément à vérifier</param>
        /// <returns>Retourne Vrai si l'élément est supprimable, sinon Faux</returns>
        protected bool AppelTraitementSqlVerificationDesDependances(ChapitreEnt chapter)
        {
            bool isDeletable = false;
            SqlParameter[] parameters =
            {
        new SqlParameter("origTableName", "FRED_CHAPITRE"),
        new SqlParameter("exclusion", string.Empty),
        new SqlParameter("dependance", string.Empty),
        new SqlParameter("origineId", chapter.ChapitreId),
        new SqlParameter("delimiter", string.Empty),
        new SqlParameter("resu", SqlDbType.Int) { Direction = ParameterDirection.Output }
      };

            // ReSharper disable once CoVariantArrayConversion
            Context.Database.ExecuteSqlCommand("VerificationDeDependance @origTableName, @exclusion, @dependance, @origineId, @delimiter, @resu OUTPUT", parameters);

            // Vérifie s'il y a aucune dépendances (paramètre "resu")
            if (Convert.ToInt32(parameters.First(x => x.ParameterName == "resu").Value) == 0)
            {
                isDeletable = true;
            }

            return isDeletable;
        }

        /// <summary>
        ///   Retourne l'identifiant du Chapitre ajouté
        /// </summary>
        /// <param name="chapitreId">Identifiant du chapitre à ajouter.</param>
        /// <returns>Retourne une entité chapitre</returns>
        public ChapitreEnt GetById(int chapitreId)
        {
            return Context.Chapitres.Where(c => c.ChapitreId.Equals(chapitreId)).Include(c => c.SousChapitres).ThenInclude(s => s.Ressources).SingleOrDefault();
        }

        /// <summary>
        ///   Obtient la collection des chapitres non supprimés
        /// </summary>
        /// <returns>La collection des chapitres non supprimés</returns>
        public IEnumerable<ChapitreEnt> GetList()
        {
            foreach (ChapitreEnt chapitre in Context.Chapitres.Where(s => !s.DateSuppression.HasValue))
            {
                yield return chapitre;
            }
        }

        /// <summary>
        ///   Obtient la collection des chapitres (suppprimés inclus)
        /// </summary>
        /// <returns>La collection des chapitres</returns>
        public IEnumerable<ChapitreEnt> GetAllList()
        {
            foreach (ChapitreEnt chapitre in Context.Chapitres)
            {
                yield return chapitre;
            }
        }

        /// <summary>
        ///   Obtient la collection des chapitres appartenant à un groupe spécifié
        /// </summary>
        /// <param name="groupeId">Identifiant du groupe.</param>
        /// <returns>La collection des chapitres</returns>
        public IEnumerable<ChapitreEnt> GetChapitreListByGroupeId(int groupeId)
        {
            foreach (ChapitreEnt chapitre in Context.Chapitres.Include("SousChapitres.Ressources.Carburant.Unite").Include("SousChapitres.Ressources.TypeRessource").Where(r => r.GroupeId.Equals(groupeId) && !r.DateSuppression.HasValue))
            {
                yield return chapitre;
            }
        }

        /// <summary>
        ///   Indique si le code existe déjà pour les chapitres d'un groupe
        /// </summary>
        /// <param name="code">Chaine de caractère du code.</param>
        /// <param name="groupeId">Identifiant du groupe.</param>
        /// <returns>Vrai si le code existe, faux sinon</returns>
        public bool IsCodeChapitreExist(string code, int groupeId)
        {
            return Context.Chapitres.Any(c => c.Code == code && c.GroupeId == groupeId);
        }

        /// <summary>
        ///   Cherche une liste des chapitres.
        /// </summary>
        /// <param name="text">Le texte a chercher dans les propriétés des chapitres.</param>
        /// <returns>Une liste de chapitres.</returns>
        public IEnumerable<ChapitreEnt> SearchChapitres(string text)
        {
            var chapitres = QueryPagingHelper.ApplyScrollPaging(Context.Chapitres
              .Where(p => p.Code.ToLower().Contains(text.ToLower()) || p.Libelle.ToLower().Contains(text.ToLower()))
              .AsQueryable());
            return chapitres.ToList();
        }

        /// <inheritdoc /> 
        public IEnumerable<ChapitreEnt> SearchChapitres(string text, int groupeId)
        {
            var chapitres = QueryPagingHelper.ApplyScrollPaging(Context.Chapitres
              .Where(p => p.GroupeId.Equals(groupeId) && (p.Code.ToLower().Contains(text.ToLower()) || p.Libelle.ToLower().Contains(text.ToLower())))
              .AsQueryable());
            return chapitres.ToList();
        }

        /// <summary>
        /// Récupération des chapitres pour Fes .
        /// </summary>
        /// <returns>La liste des chapitres id pour Fes à considérer pour le module de gestion des moyens</returns>
        public IEnumerable<int> GetFesChapitreListMoyen()
        {
            return
                Query()
                .Filter(c => c.Code != null && Constantes.ChapitreMoyenFes.ChapitreMoyenFesCodeList.Any(f => f == c.Code))
                .Get()
                .Select(c => c.ChapitreId)
                .ToList();
        }

        /// <summary>
        ///   Détermine si un chapitre peut être supprimé ou pas (en fonction de ses dépendances)
        /// </summary>
        /// <param name="chapter">élément à vérifier</param>
        /// <returns>Retourne Vrai si l'élément est supprimable, sinon Faux</returns>
        public bool IsDeletable(ChapitreEnt chapter)
        {
            return AppelTraitementSqlVerificationDesDependances(chapter);
        }

        /// <summary>
        /// Récupère les chapitres pour la comparaison de budget.
        /// </summary>
        /// <param name="chapitreIds">Les identifiants des chapitres concernés.</param>
        /// <returns>Les chapitres.</returns>
        public List<AxeInfoDao> GetPourBudgetComparaison(IEnumerable<int> chapitreIds)
        {
            return context.Chapitres
                .Where(c => chapitreIds.Contains(c.ChapitreId))
                .Select(c => new AxeInfoDao
                {
                    Id = c.ChapitreId,
                    Code = c.Code,
                    Libelle = c.Libelle
                })
                .ToList();
        }
    }
}
