using System;
using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Facture;
using Fred.EntityFramework;
using Fred.Framework;
using Fred.Framework.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.FactureAR
{
    /// <summary>
    ///   Référentiel de données pour les factures
    /// </summary>
    public class FactureArRepository : FredRepository<FactureEnt>, IFactureArRepository
    {
        private readonly ILogManager logManager;

        public FactureArRepository(FredDbContext context, ILogManager logManager)
          : base(context)
        {
            this.logManager = logManager;
        }

        #region Facture Entête

        /// <summary>
        ///   Retourne la liste de toute les factures a rapprocher
        /// </summary>
        /// <returns>Une liste de facture a rapprocher triée par date de facture</returns>
        public IEnumerable<FactureEnt> GetAllFactureAr()
        {
            IEnumerable<FactureEnt> lstFacture = Context.FactureARs.OrderBy(s => s.DateFacture)
                                     .Include(f => f.Journal)
                                     .Include(f => f.Fournisseur)
                                     .Include(f => f.Societe)
                                     .Include(f => f.Etablissement)
                                     .AsNoTracking();
            return lstFacture;
        }

        /// <inheritdoc />
        public IEnumerable<FactureEnt> SearchFactureListWithFilter(List<int> userCiIdList, SearchFactureEnt filter, int page, int pageSize = 20)
        {
            IEnumerable<FactureEnt> factures = Query()
                                               .Include(f => f.Journal)
                                               .Include(f => f.Etablissement)
                                               .Include(f => f.Fournisseur)
                                               .Include(f => f.Societe)
                                               .Include(f => f.Devise)
                                               .Include(f => f.ListLigneFacture.Select(c => c.CI))
                                               .Filter(filter.GetPredicateWhere())
                                               .Filter(f => f.ListLigneFacture.AsQueryable().Count(x => x.AffaireId.HasValue && userCiIdList.Contains(x.AffaireId.Value)) > 0)
                                               .OrderBy(x => x.OrderBy(c => c.FactureId))
                                               .GetPage(page, pageSize)
                                               .ToList();
            return factures;
        }

        /// <summary>
        ///   Retourne la liste des factures AR pour un id de societe passé en parametre
        /// </summary>
        /// <param name="societeId">Identifiant unique de la societe</param>
        /// <returns>Une liste de facture AR triée par date de cloture</returns>
        public IEnumerable<FactureEnt> GetFactureArBySocieteId(int societeId)
        {
            return Context.FactureARs.OrderBy(s => s.DateFacture).Where(s => s.SocieteId.Equals(societeId));
        }

        /// <summary>
        ///   Retourne la liste des factures pour un code de societe passé en parametre
        /// </summary>
        /// <param name="societeId">Identifiant unique de la societe</param>
        /// <returns>Une liste de facture triée par date de facture</returns>
        public IEnumerable<FactureEnt> GetFactureArBySocieteCode(string societeId)
        {
            return Context.FactureARs.Include(s => s.Societe).Where(s => s.Societe.SocieteId.Equals(societeId)).OrderBy(s => s.DateFacture);
        }

        /// <summary>
        ///   Retourne une facture par son numero
        /// </summary>
        /// <param name="noFacture">Numero de la facture</param>
        /// <param name="societeId">Identifiant unique de la societe</param>
        /// <returns>Une facture AR</returns>
        public FactureEnt GetFactureArByNumero(string noFacture, int societeId)
        {
            try
            {
                FactureEnt resu = Context.FactureARs.FirstOrDefault(s => s.NoFacture.Equals(noFacture) && s.SocieteId.Value.Equals(societeId));
                return resu;
            }
            catch (Exception ex)
            {
                throw new FredRepositoryNotFoundException($"Facture introuvable,  Numéro : {noFacture}, société : {societeId}, Erreur : {ex.Message}", ex);
            }
        }

        /// <summary>
        ///   Insertion en base d'une facture à rapprocher
        /// </summary>
        /// <param name="facture">La facture AR à enregistrer</param>
        /// <returns>Retourne l'identifiant unique de la facture AR</returns>
        public int Add(FactureEnt facture)
        {
            if (Context.Entry(facture).State == EntityState.Detached)
            {
                DetachDependancies(facture, false);
            }
            try
            {
                //Validation avant insertion
                if (facture == null)
                {
                    throw new ArgumentNullException("facture");
                }
                if (facture.JournalId <= 0 && facture.Journal == null)
                {
                    throw new ArgumentNullException("facture.JournalId");
                }
                if (facture.ListLigneFacture == null || (facture.ListLigneFacture != null && !facture.ListLigneFacture.Any()))
                {
                    throw new ArgumentNullException("facture.JournalIdlistLigneFactureAR");
                }

                //Insertion des données
                Context.FactureARs.Add(facture);
                Context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException exception)
            {
                this.logManager.TraceException(exception.Message, exception);
                throw;
            }
            catch (Exception exception)
            {
                this.logManager.TraceException(exception.Message, exception);
                throw;
            }

            return facture.FactureId;
        }

        /// <summary>
        ///   Mise à jour d'une facture
        /// </summary>
        /// <param name="facture">La facture à mettre à jour</param>
        /// <returns>Facture mise à jour</returns>
        public FactureEnt UpdateFacture(FactureEnt facture)
        {
            DetachDependancies(facture, true);
            Update(facture);

            return facture;
        }

        #endregion Facture Entête

        #region Lignes Facture

        /// <summary>
        ///   Retourne la liste de tout les journaux
        /// </summary>
        /// <returns>Une liste de journal triée par date de cloture</returns>
        public IEnumerable<FactureLigneEnt> GetAllFactureLigneAr()
        {
            foreach (FactureLigneEnt factureLigne in Context.FactureLigneARs.OrderBy(s => s.FactureId))
            {
                yield return factureLigne;
            }
        }

        /// <summary>
        ///   Retourne la liste des lignes de facture AR pour un id de facture passé en parametre
        /// </summary>
        /// <param name="factureId">Identifiant unique de la facture AR</param>
        /// <returns>Une liste de ligne facture AR triée par id</returns>
        public IEnumerable<FactureLigneEnt> GetFactureLigneByFactureId(int factureId)
        {
            return Context.FactureLigneARs.OrderBy(s => s.LigneFactureId).Where(s => s.FactureId.Equals(factureId));
        }

        /// <summary>
        ///   Insertion en base d'une ligne de facture AR
        /// </summary>
        /// <param name="factureLigne">La ligne de facture AR à enregistrer</param>
        /// <returns>Retourne l'identifiant unique de la ligne de facture AR</returns>
        public int AddLigne(FactureLigneEnt factureLigne)
        {
            if (Context.Entry(factureLigne).State == EntityState.Detached)
            {
                DetachDependanciesLignes(factureLigne);
            }

            try
            {
                //Validation avant insertion
                if (factureLigne == null)
                {
                    throw new FredRepositoryException("Impossible d'ajouter une ligne, la facture n'est pas spécifiée.");
                }
                if (!factureLigne.FactureId.HasValue)
                {
                    throw new FredRepositoryException("Impossible d'ajouter une ligne, l'identifiant de la facture n'est pas spécifiée.");
                }

                if (factureLigne.DateCreation == DateTime.MinValue)
                {
                    factureLigne.DateCreation = DateTime.Now;
                }

                //Vérification du CI
                if (!factureLigne.AffaireId.HasValue || (factureLigne.AffaireId.HasValue && factureLigne.AffaireId.Value <= 0))
                {
                    throw new FredRepositoryException($"Impossible d'ajouter une ligne, la facture {factureLigne.FactureId} n'a pas de CI");
                }

                //Insertion des données
                Context.FactureLigneARs.Add(factureLigne);
                Context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException exception)
            {
                this.logManager.TraceException(exception.Message, exception);
                throw;
            }
            catch (Exception exception)
            {
                this.logManager.TraceException(exception.Message, exception);
                throw;
            }

            return factureLigne.FactureId.Value;
        }

        /// <summary>
        ///   Permet de détacher les entités dépendantes pour éviter de les prendre en compte dans la sauvegarde du contexte.
        /// </summary>
        /// <param name="facture">objet dont les dépendances sont à détacher</param>
        /// <param name="update">Boolean permettant de differencier un Update d'un Create</param>
        private void DetachDependancies(FactureEnt facture, bool update)
        {
            facture.Devise = null;
            facture.Etablissement = null;
            facture.Fournisseur = null;
            facture.Journal = null;
            facture.Societe = null;
            facture.UtilisateurCreation = null;
            facture.UtilisateurModification = null;
            facture.UtilisateurSupression = null;
            // Lors de la mise à jour on ne souhaite pas créer les lignes de facture déjà existantes, donc on les détachent
            if (update)
            {
                facture.ListLigneFacture = null;
            }
        }

        /// <summary>
        ///   Permet de détacher les entités dépendantes pour éviter de les prendre en compte dans la sauvegarde du contexte.
        /// </summary>
        /// <param name="factureLigne">objet dont les dépendances sont à détacher</param>
        private void DetachDependanciesLignes(FactureLigneEnt factureLigne)
        {
            factureLigne.CI = null;
            factureLigne.Facture = null;
            factureLigne.Nature = null;
            factureLigne.UtilisateurCreation = null;
            factureLigne.UtilisateurModification = null;
            factureLigne.UtilisateurSuppression = null;
        }

        #endregion Lignes Facture
    }
}