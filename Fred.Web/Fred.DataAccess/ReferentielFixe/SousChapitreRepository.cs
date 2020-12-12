using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Budget.Dao.BudgetComparaison.Comparaison;
using Fred.Entities.ReferentielFixe;
using Fred.EntityFramework;
using Fred.Framework;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.ReferentielFixe
{
    /// <summary>
    ///   z
    ///   Référentiel de données pour les sousChapitres
    /// </summary>
    public class SousChapitreRepository : FredRepository<SousChapitreEnt>, ISousChapitreRepository
    {
        private readonly ILogManager logManager;

        public SousChapitreRepository(FredDbContext context, ILogManager logManager)
          : base(context)
        {
            this.logManager = logManager;
        }

        /// <summary>
        ///   Retourne l'identifiant du SousChapitre ajouté
        /// </summary>
        /// <param name="sousChapitreEnt">SousChapitre à ajouter.</param>
        /// <returns>l'identifiant du SousChapitre ajouté</returns>
        public int Add(SousChapitreEnt sousChapitreEnt)
        {
            Context.SousChapitres.Add(sousChapitreEnt);

            try
            {
                Context.SaveChanges();
            }
            catch (Exception exception)
            {
                this.logManager.TraceException(exception.Message, exception);
                throw;
            }

            return sousChapitreEnt.SousChapitreId;
        }

        /// <summary>
        ///   Appel la procédure stockée VerificationDeDependance qui permet de vérifier les dépendances
        /// </summary>
        /// <param name="subChapter">Elément à vérifier</param>
        /// <returns>Retourne Vrai si l'élément est supprimable, sinon Faux</returns>
        protected bool AppelTraitementSqlVerificationDesDependances(SousChapitreEnt subChapter)
        {
            bool isDeletable = false;
            SqlParameter[] parameters =
            {
        new SqlParameter("origTableName", "FRED_SOUS_CHAPITRE"),
        new SqlParameter("exclusion", string.Empty),
        new SqlParameter("dependance", string.Empty),
        new SqlParameter("origineId", subChapter.SousChapitreId),
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
        ///   Retourne l'identifiant du SousChapitre ajouté
        /// </summary>
        /// <param name="sousChapitreId">Identifiant du sousChapitre à ajouter.</param>
        /// <returns>Retourne une entité sousChapitre</returns>
        public SousChapitreEnt GetById(int sousChapitreId)
        {
            return
              Context.SousChapitres.Include(s => s.Ressources)
                     .SingleOrDefault(s => s.SousChapitreId.Equals(sousChapitreId));
        }

        /// <summary>
        ///   Obtient la collection des sousChapitres non supprimés
        /// </summary>
        /// <returns>La collection des sousChapitres non supprimés</returns>
        public IEnumerable<SousChapitreEnt> GetList()
        {
            foreach (SousChapitreEnt sousChapitre in Context.SousChapitres.Where(s => !s.DateSuppression.HasValue))
            {
                yield return sousChapitre;
            }
        }

        /// <summary>
        ///   Obtient la collection des sous-chapitres (suppprimés inclus)
        /// </summary>
        /// <returns>La collection des sous-chapitres</returns>
        public IEnumerable<SousChapitreEnt> GetAllList()
        {
            foreach (SousChapitreEnt sousChapitre in Context.SousChapitres)
            {
                yield return sousChapitre;
            }
        }

        /// <summary>
        ///   Obtient la collection des sous-chapitres appartenant à un chapitre spécifié
        /// </summary>
        /// <param name="chapitreId">Identifiant du chapitre.</param>
        /// <returns>La collection des sous-chapitres</returns>
        public IEnumerable<SousChapitreEnt> GetListByChapitreId(int chapitreId)
        {
            foreach (SousChapitreEnt sousChapitre in Context.SousChapitres.Where(r => r.ChapitreId.Equals(chapitreId) && !r.DateSuppression.HasValue))
            {
                yield return sousChapitre;
            }
        }

        /// <summary>
        ///   Indique si le code existe déjà pour les sous-chapitres d'un groupe
        /// </summary>
        /// <param name="code">Chaine de caractère du code.</param>
        /// <param name="groupeId">Identifiant du groupe.</param>
        /// <returns>Vrai si le code existe, faux sinon</returns>
        public bool IsCodeSousChapitreExist(string code, int groupeId)
        {
            return Context.SousChapitres.Include(s => s.Chapitre)
                                        .Any(c => c.Chapitre != null
                                                && c.Chapitre.GroupeId == groupeId
                                                && c.Code == code);
        }

        /// <summary>
        ///   Cherche une liste des sousChapitres.
        /// </summary>
        /// <param name="text">Le texte a chercher dans les propriétés des sousChapitres.</param>
        /// <returns>Une liste de sousChapitres.</returns>
        public IEnumerable<SousChapitreEnt> SearchSousChapitres(string text)
        {
            var sousChapitres = QueryPagingHelper.ApplyScrollPaging(Context.SousChapitres.Where(p => p.Code.ToLower().Contains(text.ToLower()) || p.Libelle.ToLower().Contains(text.ToLower()))
                                                                           .AsQueryable());
            return sousChapitres.ToList();
        }

        /// <summary>
        ///   Cherche une liste des sousChapitres.
        /// </summary>
        /// <param name="groupId">Groupe Id du chapitre.</param>
        /// <returns>Une liste de sousChapitres.</returns>
        public IEnumerable<SousChapitreEnt> SearchSousChapitres(int groupId)
        {
            return QueryPagingHelper.ApplyScrollPaging(Context.SousChapitres.Where(p => p.Chapitre.GroupeId.Equals(groupId)));
        }

        /// <summary>
        ///   Cherche une liste des sousChapitres.
        /// </summary>
        /// <param name="text">Le texte a chercher dans les propriétés des sousChapitres.</param>
        /// <param name="groupId">Groupe Id du chapitre.</param>
        /// <returns>Une liste de sousChapitres.</returns>
        public IEnumerable<SousChapitreEnt> SearchSousChapitres(string text, int groupId)
        {
            var sousChapitres = QueryPagingHelper.ApplyScrollPaging(Context.SousChapitres.Where(p => p.ChapitreId.Equals(groupId) && (p.Code.ToLower().Contains(text.ToLower()) || p.Libelle.ToLower().Contains(text.ToLower())))
                                                                           .AsQueryable());
            return sousChapitres.ToList();
        }

        /// <summary>
        ///   Détermine si un sous chapitre peut être supprimé ou pas (en fonction de ses dépendances)
        /// </summary>
        /// <param name="subChapter">Elément à vérifier</param>
        /// <returns>Retourne Vrai si l'élément est supprimable, sinon Faux</returns>
        public bool IsDeletable(SousChapitreEnt subChapter)
        {
            return AppelTraitementSqlVerificationDesDependances(subChapter);
        }

        /// <summary>
        /// Récupère les sous-chapitres pour la comparaison de budget.
        /// </summary>
        /// <param name="sousChapitreIds">Les identifiants des sous-chapitres concernés.</param>
        /// <returns>Les sous-chapitres.</returns>
        public List<AxeInfoDao> GetPourBudgetComparaison(IEnumerable<int> sousChapitreIds)
        {
            return Context.SousChapitres
                .Where(sc => sousChapitreIds.Contains(sc.SousChapitreId))
                .Select(sc => new AxeInfoDao
                {
                    Id = sc.SousChapitreId,
                    Code = sc.Code,
                    Libelle = sc.Libelle
                })
                .ToList();
        }
    }
}
