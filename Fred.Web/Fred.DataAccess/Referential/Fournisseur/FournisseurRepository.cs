using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Referential;
using Fred.EntityFramework;
using Fred.Framework.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Fred.DataAccess.Referential.Fournisseur
{
    /// <summary>
    ///   Référentiel de données pour les fournisseurs.
    /// </summary>
    public class FournisseurRepository : FredRepository<FournisseurEnt>, IFournisseurRepository
    {
        private readonly IGroupeRepository groupeRepo;

        public FournisseurRepository(FredDbContext context, IGroupeRepository groupeRepo)
          : base(context)
        {
            this.groupeRepo = groupeRepo;
        }

        /// <summary>
        ///   Déterminer si l'utilisateur a le droit d'accéder à cette entité.
        /// </summary>
        /// <param name="entity">L'entité concernée.</param>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        public override void CheckAccessToEntity(FournisseurEnt entity, int userId)
        {
            groupeRepo.CheckAccessToEntity(entity.GroupeId, userId);
        }

        /// <summary>
        ///   Retourne la liste des fournisseurs.
        /// </summary>
        /// <param name="groupeId">Identifiant du groupe</param>
        /// <returns>Liste des fournisseurs.</returns>
        public IEnumerable<FournisseurEnt> GetFournisseurList(int groupeId)
        {
            return Query().Include(p => p.Pays).Filter(g => g.GroupeId.Equals(groupeId)).Get().AsNoTracking();
        }

        /// <summary>
        ///   Retourne la liste des fournisseurs.
        /// </summary>
        /// <returns>Liste des fournisseurs.</returns>
        public IEnumerable<FournisseurEnt> GetFournisseurList()
        {
            foreach (var fournisseur in Context.Fournisseurs.Include(x => x.Pays).AsNoTracking())
            {
                yield return fournisseur;
            }
        }

        /// <inheritdoc/>
        public int? GetAgenceIdByCodeAndGroupe(string agenceCode, int groupeId)
        {
            return Context.Agences
                .FirstOrDefault(a => a.Code == agenceCode
                    && a.Fournisseur.GroupeId == groupeId)
                ?.AgenceId;
        }

        /// <summary>
        ///   Retourne le fournisseur dont portant l'identifiant unique indiqué et le son groupe ID
        /// </summary>
        /// <param name="fournisseurId">Identifiant du fournisseur à retrouver.</param>
        /// <param name="groupeId">Identifiant</param>
        /// <returns>Le fournisseur retrouvé, sinon nulle.</returns>
        public FournisseurEnt GetFournisseur(int fournisseurId, int groupeId)
        {
            return Query().Include(p => p.Pays).Get().AsNoTracking().SingleOrDefault(f => f.GroupeId.Equals(groupeId) && f.FournisseurId.Equals(fournisseurId));
        }

        /// <summary>
        ///   Retourne le fournisseur dont portant l'identifiant unique indiqué.
        /// </summary>
        /// <param name="fournisseurId">Identifiant du fournisseur à retrouver.</param>    
        /// <returns>Le fournisseur retrouvé, sinon nulle.</returns>
        public FournisseurEnt GetFournisseur(int fournisseurId)
        {
            return Query().Include(p => p.Pays).Get().AsNoTracking().SingleOrDefault(f => f.FournisseurId.Equals(fournisseurId));
        }

        /// <summary>
        ///   Ajout un nouveau fournisseur
        /// </summary>
        /// <param name="fournisseurEnt">Fournisseur à ajouter</param>
        /// <returns>Fournisseur ajouté.</returns>
        public FournisseurEnt AddFournisseur(FournisseurEnt fournisseurEnt)
        {
            Insert(fournisseurEnt);

            return fournisseurEnt;
        }

        /// <summary>
        ///   Sauvegarde les modifications d'un fournisseur.
        /// </summary>
        /// <param name="fournisseurEnt">Fournisseur à modifier</param>
        /// <returns>Fournisseur mis à jour.</returns>
        public FournisseurEnt UpdateFournisseur(FournisseurEnt fournisseurEnt)
        {
            Update(fournisseurEnt);

            return fournisseurEnt;
        }

        /// <summary>
        ///   Supprime un fournisseur
        /// </summary>
        /// <param name="id">L'identifiant du fournisseur à supprimer</param>
        public void DeleteFournisseurById(int id)
        {
            FournisseurEnt fournisseur = GetFournisseur(id);
            if (fournisseur != null)
            {
                Delete(fournisseur);
            }
        }

        /// <summary>
        ///   Cherche une liste de fournisseur.
        /// </summary>
        /// <param name="text">Le texte a chercher dans les propriétés des fournisseurs.</param>
        /// <param name="groupeId">Identifiant du groupe</param>
        /// <returns>Une liste de fournisseur.</returns>
        public IEnumerable<FournisseurEnt> SearchFournisseurs(string text, int groupeId)
        {
            return Query()
                    .Include(p => p.Pays)
                    .Filter(f => string.IsNullOrEmpty(text) || f.Code.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0 || f.Libelle.IndexOf(text, StringComparison.OrdinalIgnoreCase) >= 0)
                    .Filter(f => f.GroupeId.Equals(groupeId))
                    .Get()
                    .AsNoTracking();
        }

        /// <inheritdoc/>
        public FournisseurEnt GetFournisseurLight(string code, string typeSequence, int groupeId)
        {
            return Query()
                   .Include(p => p.Pays)
                   .Filter(g => g.GroupeId == groupeId && g.Code == code.Trim() && g.TypeSequence == typeSequence.Trim())
                   .Get()
                   .AsNoTracking()
                   .FirstOrDefault();
        }

        /// <inheritdoc/>
        public IEnumerable<FournisseurEnt> GetFournisseurLight(int groupeId)
        {
            return Query().Filter(g => g.GroupeId == groupeId).Get().AsNoTracking();
        }

        /// <inheritdoc/>
        public FournisseurEnt GetFournisseurByCodeAndGroupeId(int groupeId, string code)
        {
            return Query().Filter(g => g.GroupeId == groupeId && g.Code == code).Get().AsNoTracking().FirstOrDefault();
        }

        /// <summary>
        ///   Retourne le fournisseur portant le code indiqué.
        /// </summary>
        /// <param name="fournisseurCode">Code du fournisseur dont l'identifiant est à retrouver.</param>
        /// <returns>Fournisseur retrouvé, null sinon</returns>
        public int? GetFournisseurIdByCode(string fournisseurCode)
        {
            int? fournisseurId = null;

            FournisseurEnt fournisseur = Context.Fournisseurs.Include(p => p.Pays).AsNoTracking().FirstOrDefault(f => f.Code == fournisseurCode);

            if (fournisseur != null)
            {
                fournisseurId = fournisseur.FournisseurId;
            }

            return fournisseurId;
        }

        /// <inheritdoc/>
        public void AddOrUpdateFournisseurList(IEnumerable<FournisseurEnt> fournisseurs)
        {
            /* using (var ctxt = new FredDbContext()) 
             * N'a pas été utilisé ici car les mises à jour ne fonctionnent pas avec ce nouveau contexte...
             * ....mystère et boule de gomme d'entity framework.....
             */
            using (var dbContextTransaction = Context.Database.BeginTransaction())
            {
                try
                {
                    // disable detection of changes
                    Context.ChangeTracker.AutoDetectChangesEnabled = false;
                    int count = 0;
                    const int commitCount = 100;

                    // Mise à jour des fournisseurs
                    foreach (FournisseurEnt f in fournisseurs.Where(x => x.FournisseurId > 0))
                    {
                        ++count;
                        Update(f);

                        // A chaque 100 opérations, on sauvegarde le contexte.
                        if (count % commitCount == 0)
                        {
                            Context.SaveChanges();
                        }
                    }

                    // Ajout des fournisseurs
                    var addedList = fournisseurs.Where(x => x.FournisseurId == 0);
                    Context.Fournisseurs.AddRange(addedList);

                    Context.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (DbUpdateException ex)
                {
                    StringBuilder errorMessage = new StringBuilder();
                    errorMessage.AppendLine($"DbUpdateException error details - {ex?.InnerException?.InnerException?.Message}");

                    foreach (EntityEntry entry in ex.Entries)
                    {
                        errorMessage.AppendFormat("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", entry.Entity.GetType().Name, entry.State);
                    }

                    throw new FredRepositoryException(errorMessage.ToString(), ex);
                }
                catch (Exception e)
                {
                    dbContextTransaction.Rollback();
                    throw new FredRepositoryException(e.Message, e);
                }
                finally
                {
                    // re-enable detection of changes
                    Context.ChangeTracker.AutoDetectChangesEnabled = true;
                }
            }
        }

        /// <summary>
        ///   obtient une liste de fournisseur de type ETT
        /// </summary>
        /// <param name="groupeId">Identifiant du groupe</param>
        /// <returns>Une liste de fournisseur.</returns>
        public List<FournisseurEnt> GetFournisseurETT(int groupeId)
        {
            return Query()
                    .Include(p => p.Pays)
                    .Get()
                    .AsNoTracking()
                    .Where(f => f.TypeTiers == "I")
                    .Where(f => f.GroupeId == groupeId)
                    .ToList();
        }

        /// <summary>
        /// Mise à jour d'un fournisseur
        /// </summary>
        /// <param name="fournisseurId">id fournisseur</param>
        /// <param name="fournisseur">Fournisseur à Mettre à jour</param>
        public void UpdateFournisseur(int fournisseurId, FournisseurEnt fournisseur)
        {
            Update(GetAndUpdate(fournisseurId, fournisseur));
        }

        /// <summary>
        /// Ajout un nouveau fournisseur sans action save
        /// </summary>
        /// <param name="fournisseur">fournisseur à ajouter</param>
        public void AddFournisseurWithoutSaving(FournisseurEnt fournisseur)
        {
            Insert(fournisseur);
        }

        /// <summary>
        /// Mettre à jour fournisseur
        /// </summary>
        /// <param name="fournisseurId">id fournisseur à mettre à jour</param>
        /// <param name="fournisseur">fournisseur à mettre à jour</param>
        /// <returns>Fournisseur à jour</returns>
        private FournisseurEnt GetAndUpdate(int fournisseurId, FournisseurEnt fournisseur)
        {
            var entity = Context.Fournisseurs.Find(fournisseurId);

            entity.Libelle = fournisseur.Libelle;
            entity.Adresse = fournisseur.Adresse;
            entity.CodePostal = fournisseur.CodePostal;
            entity.Ville = fournisseur.Ville;
            entity.SIRET = fournisseur.SIRET;
            entity.SIREN = fournisseur.SIREN;
            entity.Telephone = fournisseur.Telephone;
            entity.Fax = fournisseur.Fax;
            entity.Email = fournisseur.Email;
            entity.TypeTiers = fournisseur.TypeTiers;
            entity.DateCloture = fournisseur.DateCloture;
            entity.DateOuverture = fournisseur.DateOuverture;
            entity.ModeReglement = fournisseur.ModeReglement;
            entity.RegleGestion = fournisseur.RegleGestion;
            entity.PaysId = fournisseur.PaysId;
            entity.IsProfessionLiberale = fournisseur.IsProfessionLiberale;

            return entity;
        }

        /// <summary>
        /// Return A List Of Fournisseur By List code 
        /// </summary>
        /// <param name="listOfCode">List Of Code</param>
        /// <returns>List Of Fournisseur</returns>
        public IEnumerable<FournisseurEnt> GetAllIdFournisseurForListOfCode(List<string> listOfCode)
        {
            return Context.Fournisseurs.Where(x => listOfCode.Contains(x.Code));
        }

        /// <summary>
        /// Retourne le fournisseur par fournisseur SIREN et code groupe.
        /// </summary>
        /// <param name="fournisseurSIREN">SIREN Fournisseur.</param>
        /// <param name="groupeCode">Code groupe.</param>
        /// <returns>Le fournisseur retrouvé, sinon nulle.</returns>
        public List<FournisseurEnt> GetBySirenAndGroupeCode(string fournisseurSIREN, string groupeCode)
        {
            return Context.Fournisseurs.Where(x => x.SIREN == fournisseurSIREN && x.Groupe.Code == groupeCode)?.ToList();
        }

        /// <summary>
        /// Retourne le fournisseur par reference systeme interimaire SIREN et code groupe.
        /// </summary>
        /// <param name="fournisseurSIREN">SIREN Fournisseur.</param>
        /// <param name="groupeCode">Code groupe.</param>
        /// <returns>Le fournisseur retrouvé, sinon nulle.</returns>
        public List<FournisseurEnt> GetByReferenceSystemInterimaireAndGroupeCode(string fournisseurSIREN, string groupeCode)
        {
            return Context.Fournisseurs.Where(x => x.ReferenceSystemeInterimaire == fournisseurSIREN && x.Groupe.Code == groupeCode)?.ToList();
        }
    }
}
