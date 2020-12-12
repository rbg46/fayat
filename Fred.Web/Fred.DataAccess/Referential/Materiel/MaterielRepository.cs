using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Moyen;
using Fred.Entities.Referential;
using Fred.EntityFramework;
using Fred.Framework;
using Fred.Framework.Exceptions;
using Fred.Web.Shared.Models.Materiel.Search;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.Referential.Materiel
{
    public class MaterielRepository : FredRepository<MaterielEnt>, IMaterielRepository
    {
        private readonly ILogManager logManager;

        public MaterielRepository(FredDbContext context, ILogManager logManager)
          : base(context)
        {
            this.logManager = logManager;
        }

        /// <summary>
        /// Appel à une procédure stockée
        /// </summary>
        /// <param name="materielId">Id du materiel a vérifier les dépendances</param>
        /// <returns>Renvoie un bool</returns>
        protected bool AppelTraitementSqlVerificationDesDependances(int materielId)
        {
            int? rapportLigneId;
            rapportLigneId = GetRapportLigneIdBymateriel(materielId);

            int nbcmd = 0;

            DbConnection sqlConnection = Context.Database.GetDbConnection();
            DbCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandText = "VerificationDeDependance";
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandTimeout = 240;

            sqlCommand.Parameters.Add(new SqlParameter("@origTableName", "FRED_MATERIEL"));
            sqlCommand.Parameters.Add(new SqlParameter("@exclusion", string.Empty));
            sqlCommand.Parameters.Add(new SqlParameter("@dependance", rapportLigneId.HasValue ? $"'FRED_FACTURE',{rapportLigneId}" : string.Empty));
            sqlCommand.Parameters.Add(new SqlParameter("@origineId", materielId));
            sqlCommand.Parameters.Add(new SqlParameter("@delimiter", '|'));
            sqlCommand.Parameters.Add(new SqlParameter("@resu", SqlDbType.Int) { Direction = ParameterDirection.Output });

            sqlConnection.Open();
            sqlCommand.ExecuteReader();
            sqlConnection.Close();

            nbcmd = (int)sqlCommand.Parameters["@resu"].Value;

            if (nbcmd == 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Permet de récupérer l'id d'une ligne d'un rapport lié au code de déplacement spécifiée.
        /// </summary>
        /// <param name="id">Identifiant du code de déplacement</param>
        /// <returns>Retourne l'identifiant de la 1ere ligne de rapport</returns>
        private int? GetRapportLigneIdBymateriel(int? id)
        {
            if (id == null)
            {
                return null;
            }

            return Context.RapportLignes.Where(s => s.MaterielId == id).Select(s => s.RapportLigneId).FirstOrDefault();
        }

        /// <summary>
        /// Ajout d'un matériel
        /// </summary>
        /// <param name="materiel">Matériel à ajouter</param>
        /// <returns>materiel</returns>
        public MaterielEnt AddMateriel(MaterielEnt materiel)
        {
            try
            {
                materiel.Ressource = null;
                materiel.Fournisseur = null;

                Insert(materiel);
            }
            catch (DbUpdateConcurrencyException exception)
            {
                this.logManager.TraceException(exception.Message, exception);
                throw;
            }

            return materiel;
        }

        /// <summary>
        /// Supprime un matériel
        /// </summary>
        /// <param name="id">L'identifiant du matériel à supprimer</param>
        public void DeleteMaterielById(int id)
        {
            MaterielEnt materiel = Context.Materiels.Find(id);
            if (materiel == null)
            {
                System.Data.DataException objectNotFoundException = new System.Data.DataException();
                this.logManager.TraceException(objectNotFoundException.Message, objectNotFoundException);
                throw objectNotFoundException;
            }

            Context.Materiels.Remove(materiel);

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

        /// <summary>
        /// Vérifie qu'un materiel peut être supprimé
        /// </summary>
        /// <param name="materiel">Le materiel à vérifier</param>
        /// <returns>True = suppression ok</returns>
        public bool IsDeletable(MaterielEnt materiel)
        {
            return AppelTraitementSqlVerificationDesDependances(materiel.MaterielId);
        }

        /// <summary>
        /// Retourne la liste des materiels
        /// </summary>
        /// <returns>Liste des Rapports.</returns>
        public IQueryable<MaterielEnt> GetDefaultQuery()
        {
            return Context.Materiels.Include(o => o.Societe)
                          .Include(o => o.Ressource)
                          .Include(o => o.AuteurCreation.Personnel)
                          .Include(o => o.AuteurModification.Personnel)
                          .Include(o => o.AuteurSuppression.Personnel)
                          .Where(o => !o.DateSuppression.HasValue);
        }

        /// <summary>
        /// Matériel via societeId
        /// </summary>
        /// <param name="societeId">idSociete de la société</param>
        /// <returns>Renvoie une liste de materiel</returns>
        public IEnumerable<MaterielEnt> GetMaterielBySocieteId(int societeId)
        {
            return Context.Materiels.Where(x => x.SocieteId == societeId);
        }

        /// <summary>
        /// La liste de tous les matériels actifs.
        /// </summary>
        /// <returns>Renvoie la liste des matériels actifs</returns>
        public IEnumerable<MaterielEnt> GetMaterielList()
        {
            foreach (MaterielEnt materiel in GetDefaultQuery().Where(y => y.Actif.Equals(true)))
            {
                yield return materiel;
            }
        }

        /// <summary>
        /// Retourne la liste de tous les matériels(actif et inactif).
        /// </summary>
        /// <returns>List de tous les matériels</returns>
        public IEnumerable<MaterielEnt> GetMaterielListAll()
        {
            var defaultQuery = GetDefaultQuery();

            foreach (MaterielEnt materiel in defaultQuery)
            {
                yield return materiel;
            }
        }

        /// <summary>
        /// Retourne la liste de tous les matériels(actif et inactif) pour la synchronisation avec le mobile.
        /// </summary>
        /// <returns>List de tous les matériels</returns>
        public IEnumerable<MaterielEnt> GetMaterielListAllSync()
        {
            return Context.Materiels.AsNoTracking();
        }

        /// <summary>
        /// Permet de connaître l'existence d'un matériel depuis son code.
        /// </summary>
        /// <param name="idCourant">id courant</param>
        /// <param name="codeMateriel">code Materiel</param>
        /// <param name="societeId">Id societe</param>
        /// <returns>Retourne vrai si le code passé en paramètre existe déjà, faux sinon</returns>
        public bool IsMaterielExistsByCode(int idCourant, string codeMateriel, int societeId)
        {
            if (societeId > 0)
            {
                if (idCourant != 0)
                {
                    return Context.Materiels.Where(x => x.SocieteId == societeId).Any(s => s.Code == codeMateriel && s.SocieteId != idCourant);
                }

                return Context.Materiels.Where(x => x.SocieteId == societeId).Any(s => s.Code == codeMateriel);
            }

            return false;
        }

        public async Task<IEnumerable<MaterielSearchModel>> SearchMaterielsAsync(MaterielSearchParameter parameter)
        {
            string searchText = parameter.SearchText;

            return await Context.Materiels
                .Where(m => !m.MaterielLocation
                         && m.SocieteId == parameter.SocieteId
                         && (string.IsNullOrEmpty(searchText) || m.Code.Contains(searchText) || m.Libelle.Contains(searchText)))
                .OrderBy(m => m.Code)
                .Skip(parameter.PageIndex * parameter.PageSize)
                .Take(parameter.PageSize)
                .Select(m => new MaterielSearchModel
                {
                    Id = m.MaterielId,
                    Code = m.Code,
                    Libelle = m.Libelle,
                    EtablissementComptable = m.EtablissementComptable != null ? $"{m.EtablissementComptable.Code} - {m.EtablissementComptable.Libelle}" : null,
                    IsActif = m.Actif,
                    IsStorm = m.IsStorm
                })
                .ToListAsync()
                .ConfigureAwait(false);
        }

        /// <summary>
        /// Sauvegarde les modifications d'un materiel
        /// </summary>
        /// <param name="materiel">Matériel à modifier</param>
        public void UpdateMateriel(MaterielEnt materiel)
        {
            try
            {
                Context.Entry(materiel).State = EntityState.Modified;
                Context.SaveChanges();
            }
            catch (Exception exception)
            {
                this.logManager.TraceException(exception.Message, exception);
                throw;
            }
        }

        /// <summary>
        /// Ligne de recherche
        /// </summary>
        /// <param name="text">Le text recherché</param>
        /// <returns>Renvoie une liste</returns>
        public IEnumerable<MaterielEnt> SearchLight(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return QueryPagingHelper.ApplyScrollPaging(GetMaterielList().AsQueryable());
            }

            return QueryPagingHelper.ApplyScrollPaging(Context.Materiels.Where(m => m.Actif)
                                                              .Include(x => x.Societe)
                                                              .Include(x => x.Ressource)
                                                              .Where(c => c.Code.ToLower().Contains(text.ToLower())
                                                                          || c.Libelle.ToLower().Contains(text.ToLower()))
                                                              .AsQueryable());
        }

        /// <summary>
        /// Matériel via l'id
        /// </summary>
        /// <param name="id">Id du matérial</param>
        /// <returns>Renvoie un matériel</returns>
        public MaterielEnt GetMaterielById(int id)
        {
            return Context.Materiels.AsNoTracking()
                .Where(m => m.MaterielId == id)
                .Include(m => m.Ressource).AsNoTracking()
                .Include(m => m.Fournisseur)
                .Include(e => e.EtablissementComptable)
                .FirstOrDefault();
        }

        public MaterielEnt GetMaterielByIdWithCommandes(int id)
        {
            return Context.Materiels
                .AsNoTracking()
                .Include(m => m.CommandeLignes)
                .ThenInclude(c => c.Unite)
                .FirstOrDefault(m => m.MaterielId == id);
        }

        public int GetRessourceIdByMaterielId(int materielId)
        {
            return Context.Materiels.Where(m => m.MaterielId == materielId).Select(x => x.RessourceId).FirstOrDefault();
        }

        /// <summary>
        /// Matériel via l'id avec la société
        /// </summary>
        /// <param name="id">Id du matérial</param>
        /// <returns>Renvoie un matériel</returns>
        public MaterielEnt GetMaterielByIdWithSociete(int id)
        {
            return Context.Materiels.AsNoTracking()
                .Where(m => m.MaterielId == id)
                .Include(m => m.Ressource).AsNoTracking()
                .Include(m => m.Fournisseur).AsNoTracking()
                .Include(m => m.Societe).AsNoTracking().FirstOrDefault();
        }

        /// <summary>
        /// Vérifie s'il l'entité est déja utilisée
        /// </summary>
        /// <param name="id">Id de l'entité à vérifié</param>
        /// <returns>True = déjà Utilisée</returns>
        public bool IsAlreadyUsed(int id)
        {
            return !AppelTraitementSqlVerificationDesDependances(id);
        }

        /// <summary>
        /// Permet de récupérer la liste de tous les matériels en fonction des critères de recherche.
        /// </summary>
        /// <param name="societeId"> Identifiant de la société </param>
        /// <param name="predicate">Filtres de recherche sur tous les matériels</param>
        /// <returns>Retourne la liste filtrée de tous les matériels</returns>
        public IEnumerable<MaterielEnt> GetMateriels(int societeId, Expression<Func<MaterielEnt, bool>> predicate)
        {
            // Attention ne pas mettre d'includes !! Si besoin faire une nouvelle fonction.
            // Une sauvegarde est effectuée apres la recuperation de cette entité.    
            var result = Context.Materiels
                        .Where(c => c.SocieteId == societeId)
                        .Where(predicate)
                        .AsNoTracking()
                        .ToList();
            return result;
        }

        #region Methodes de FES

        /// <summary>
        /// Retourne un moyen par son code.
        /// </summary>
        /// <param name="code">Le code du moyen.</param>
        /// <returns>Un moyen.</returns>
        public MaterielEnt GetMoyen(string code)
        {
            try
            {
                IEnumerable<MaterielEnt> materials = Context.Materiels
                                            .Where(c => c.Code == code)
                                            .AsNoTracking();

                if (materials.Count() > 1)
                {
                    throw new FredRepositoryException($"Erreur de doublon : plusieurs moyens ont été trouvés pour le code ({code}).");
                }

                return materials.FirstOrDefault();
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

        /// <summary>
        /// Récupération du moyen en fonction de son code et l'id de la société
        /// </summary>
        /// <param name="code">Code du moyen</param>
        /// <param name="societeId">Id de la société</param>
        /// <returns>Un objet materiel Ent qui corresponds au code et l'id envoyés</returns>
        public MaterielEnt GetMoyen(string code, int societeId)
        {
            try
            {
                IEnumerable<MaterielEnt> materials = Query()
                                .Filter(m => m.Code == code && m.SocieteId == societeId)
                                .Get().AsNoTracking();

                if (materials.Count() > 1)
                {
                    throw new FredRepositoryException($"Erreur de doublon : plusieurs moyens ont été trouvés pour le couple (code, sociedId) ({code}, {societeId}).");
                }

                return materials.FirstOrDefault();
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

        /// <summary>
        /// Récupération du moyen en fonction de son code et l'id de la société
        /// </summary>
        /// <param name="code">Code du moyen</param>
        /// <param name="societeId">Id de la société</param>
        /// <param name="etablissementComptableId">Id de l'établiessement comptable</param>
        /// <returns>Un objet materiel Ent qui corresponds au code et l'id envoyés</returns>
        public MaterielEnt GetMoyen(string code, int societeId, int? etablissementComptableId)
        {
            try
            {
                IEnumerable<MaterielEnt> materials = null;

                if (etablissementComptableId.HasValue)
                {
                    materials = Query()
                                .Filter(m => m.Code == code && m.SocieteId == societeId && m.EtablissementComptableId == etablissementComptableId)
                                .Get().AsNoTracking();

                    if (materials.Count() > 1)
                    {
                        throw new FredRepositoryException($"Erreur de doublon : plusieurs moyens ont été trouvés pour le couple (code, sociedId, Etablissement) ({code}, {societeId}, {etablissementComptableId}.");
                    }

                    return materials.FirstOrDefault();
                }
                else
                {
                    return GetMoyen(code, societeId);
                }

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

        /// <summary>
        /// Permet d'ajouter ou de mettre à jour un moyen.
        /// </summary>
        /// <param name="material">Un moyen.</param>
        /// <returns>Le moyen ajouté ou mis à jour.</returns>
        public MaterielEnt AddOrUpdateMoyen(MaterielEnt material)
        {
            try
            {
                if (material.MaterielId.Equals(0))
                {
                    Context.Materiels.Add(material);
                }
                else
                {
                    Context.Entry(material).State = EntityState.Modified;
                }

                Context.SaveChanges();
                return material;
            }
            catch (Exception e)
            {
                throw new FredRepositoryException(e.Message, e);
            }
        }

        /// <summary>
        /// Chercher des moyens en fonction des critéres fournies en paramètres
        /// </summary>
        /// <param name="filters">Les filtres de recherche</param>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <param name="chapitresIds">Liste des chapitres id pour FES</param>
        /// <returns>Liste des moyens</returns>
        public IEnumerable<MaterielEnt> SearchLightForMoyen(SearchMoyenEnt filters, int page, int pageSize, IEnumerable<int> chapitresIds)
        {
            try
            {
                return Query()
                        .Include(c => c.Ressource.SousChapitre.Chapitre)
                        .Include(m => m.Societe)
                        .Include(m => m.EtablissementComptable)
                        .Filter(filters.GetPredicateWhere())
                        .Filter(c => chapitresIds.Any(f => f == c.Ressource.SousChapitre.Chapitre.ChapitreId))
                        .Filter(filters.GetIsLocationPredicate())
                        .OrderBy(e => e.OrderBy(a => a.Code))
                        .GetPage(page, pageSize)
                        .AsEnumerable();
            }
            catch (Exception e)
            {
                throw new FredRepositoryException(e.Message, e);
            }
        }

        /// <summary>
        /// Chercher des immatriculations des moyens en fonction des critéres fournies en paramètres 
        /// </summary>
        /// <param name="filters">Les filtres de recherche</param>
        /// <param name="chapitresIds">les Ids des chapitres moyens </param>
        /// <param name="page">Numéro de page</param>
        /// <param name="pageSize">Taille de la page</param>
        /// <returns>Liste des moyens</returns>
        public IEnumerable<MaterielEnt> SearchLightForImmatriculation(SearchImmatriculationMoyenEnt filters, IEnumerable<int> chapitresIds, int page = 1, int pageSize = 20)
        {
            try
            {
                return Query()
                        .Include(c => c.Ressource.SousChapitre.Chapitre)
                        .Filter(filters.GetPredicateWhere())
                        .Filter(c => chapitresIds.Any(f => f == c.Ressource.SousChapitre.Chapitre.ChapitreId))
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
        /// Get moyen for fiche generique
        /// </summary>
        /// <param name="page">page to use </param>
        /// <param name="pageSize">Page size</param>
        /// <param name="text">text of serach by ressource  code</param>
        /// <returns>iEnumerable of MoyenEnt</returns>
        public IEnumerable<MaterielEnt> GetMoyenForFicheGenerique(int page = 1, int pageSize = 30, string text = null)
        {
            return Query()
                    .Include(a => a.Ressource)
                    .Filter(a => a.Ressource != null && a.IsImported && a.Actif && a.IsLocation)
                    .Filter(a => string.IsNullOrEmpty(text) ||
                                        (a.Ressource.Code != null
                                            && a.Ressource.Code.ToUpper().Contains(text.ToUpper())) || (a.Ressource != null && a.Ressource.Libelle != null && a.Ressource.Libelle.ToUpper().Contains(text.ToUpper())))
                    .OrderBy(a => a.OrderBy(r => r.Ressource.Code))
                    .Get()
                    .AsEnumerable();
        }

        #endregion

        public Dictionary<(string code, int societeId), int> GetMaterielIdsByCodeAndSocieteId(IEnumerable<MaterielEnt> materiels)
        {
            var materielsCodeAndSocieteId = materiels.Select(m => new { m.Code, m.SocieteId });

            return Context.Materiels
                .Select(m => new { m.MaterielId, m.Code, m.SocieteId })
                .GroupBy(m => new { m.Code, m.SocieteId }, m => m.MaterielId)
                .Where(g => materielsCodeAndSocieteId.Contains(g.Key))
                .ToDictionary(g => (g.Key.Code, g.Key.SocieteId), g => g.FirstOrDefault());
        }

        public Task<MaterielEnt> GetMaterielDetailByIdAsync(int id)
        {
            return Context.Materiels
                .Where(m => m.MaterielId == id)
                .Include(m => m.Ressource)
                .Include(m => m.Fournisseur)
                .Include(m => m.EtablissementComptable)
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }
    }
}
