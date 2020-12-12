using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Fred.DataAccess.Common;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Entities.Bareme;
using Fred.Entities.CI;
using Fred.Entities.Commande;
using Fred.Entities.Depense;
using Fred.Entities.Personnel;
using Fred.Entities.Rapport;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielFixe;
using Fred.Entities.Societe;
using Fred.Entities.Valorisation;
using Fred.EntityFramework;
using Fred.Framework;
using Fred.Framework.Exceptions;
using Fred.Framework.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Fred.DataAccess.Commande
{
    /// <summary>
    /// Repository Commande Energie
    /// </summary>
    public class CommandeEnergieRepository : FredRepository<CommandeEnt>, ICommandeEnergieRepository
    {
        private readonly FredDbContext context;

        /// <summary>
        ///   Initialise une nouvelle instance de la classe <see cref="CommandeEnergieRepository" /> class.
        /// </summary>
        /// <param name="logMgr">logeur</param>        
        /// <param name="context">FredDbcontext</param>
        public CommandeEnergieRepository(FredDbContext context)
           : base(context)
        {
            this.context = context;
        }

        /// <summary>
        /// Récupération d'un CI avec ses Surcharge Bareme Exploitation et Bareme Exploitation
        /// </summary>
        /// <param name="ciId">Identifiant du CI</param>
        /// <param name="periode">Période choisie</param>
        /// <param name="personnelIds">Liste d'identifiants des personnels</param>
        /// <param name="materielIds">Liste d'identifiants de matériels</param>
        /// <param name="referentielEtenduIds">Liste d'identifiants du référentiel étendu</param>
        /// <returns>CI</returns>
        public CIEnt GetCi(int ciId, DateTime periode, List<int> personnelIds, List<int> materielIds, List<int> referentielEtenduIds)
        {
            var persoIds = personnelIds ?? Enumerable.Empty<int>();
            var materIds = materielIds ?? Enumerable.Empty<int>();
            var refEtIds = referentielEtenduIds ?? Enumerable.Empty<int>();

            var query = context.CIs
                              .Where(x => x.CiId == ciId)
                              .Select(x => new
                              {
                                  x.CiId,
                                  SurchargeBaremeExploitationCIs = x.SurchargeBaremeExploitationCIs.Where(s => s.CIId == ciId
                                                                                                              && s.PeriodeDebut <= periode
                                                                                                              && !s.PeriodeFin.HasValue
                                                                                                              && (!persoIds.Any() || (s.PersonnelId.HasValue && persoIds.Contains(s.PersonnelId.Value)))
                                                                                                              && (!materIds.Any() || (s.MaterielId.HasValue && materIds.Contains(s.MaterielId.Value))))
                                                                                                    .Select(p => new
                                                                                                    {
                                                                                                        p.CIId,
                                                                                                        p.Prix,
                                                                                                        p.Unite,
                                                                                                        p.PersonnelId,
                                                                                                        p.MaterielId
                                                                                                    }),
                                  BaremeExploitationCIs = x.BaremeExploitationCIs.Where(b => b.CIId == ciId
                                                                                       && periode >= b.PeriodeDebut
                                                                                       && (!b.PeriodeFin.HasValue || periode < b.PeriodeFin.Value)
                                                                                       && b.ReferentielEtenduId.HasValue
                                                                                       && (!refEtIds.Any() || refEtIds.Contains(b.ReferentielEtenduId.Value)))
                                                                                  .Select(p => new
                                                                                  {
                                                                                      p.CIId,
                                                                                      p.Prix,
                                                                                      p.PrixChauffeur,
                                                                                      p.PrixConduite,
                                                                                      p.ReferentielEtenduId,
                                                                                      p.Unite
                                                                                  })
                              })
                              .FirstOrDefault();

            return new CIEnt
            {
                CiId = query.CiId,
                SurchargeBaremeExploitationCIs = query.SurchargeBaremeExploitationCIs.Select(p => new SurchargeBaremeExploitationCIEnt
                {
                    CIId = p.CIId,
                    Prix = p.Prix,
                    Unite = p.Unite,
                    PersonnelId = p.PersonnelId,
                    MaterielId = p.MaterielId
                })
                .ToList(),
                BaremeExploitationCIs = query.BaremeExploitationCIs.Select(p => new BaremeExploitationCIEnt
                {
                    CIId = p.CIId,
                    Prix = p.Prix,
                    ReferentielEtenduId = p.ReferentielEtenduId,
                    Unite = p.Unite,
                    PrixChauffeur = p.PrixChauffeur,
                    PrixConduite = p.PrixConduite
                })
                .ToList()
            };
        }

        /// <summary>
        /// Récupération d'une query RapportLigneEnt
        /// </summary>
        /// <param name="filters">Filtres choisis</param>
        /// <returns>Query</returns>
        public IQueryable<RapportLigneEnt> GetPointagesMateriels(Expression<Func<RapportLigneEnt, bool>> filters)
        {
            return context.RapportLignes.Include(x => x.Materiel).ThenInclude(x => x.Societe)
                                        .Include(x => x.Materiel).ThenInclude(x => x.Ressource).ThenInclude(x => x.ReferentielEtendus)
                                        .Include(x => x.Rapport)
                                        .Where(filters);

        }

        public IQueryable<RapportLigneEnt> GetPointagesPersonnels(Expression<Func<RapportLigneEnt, bool>> filters)
        {
            return context.RapportLignes.Include(x => x.Personnel).ThenInclude(x => x.Societe)
                                        .Include(x => x.Personnel).ThenInclude(x => x.Ressource).ThenInclude(x => x.ReferentielEtendus)
                                        .Include(x => x.Rapport)
                                        .Where(filters);

        }

        public IQueryable<RapportLigneEnt> GetPointagesInterimaires(Expression<Func<RapportLigneEnt, bool>> filters)
        {
            return context.RapportLignes.Include(x => x.Contrat).ThenInclude(x => x.Ressource)
                                        .Include(x => x.Contrat).ThenInclude(x => x.Unite)
                                        .Include(x => x.Contrat).ThenInclude(x => x.Interimaire)
                                        .Include(x => x.Rapport)
                                        .Where(filters);

        }

        /// <summary>
        /// Vérifie si un numéro de commande externe existe déjà en BD
        /// Renvoie le dernier numéro de commande externe existant
        /// </summary>
        /// <param name="numeroCommandeExterne">Numéro commande externe à vérifier</param>
        /// <returns>Le dernier numéro de commande externe</returns>
        public string GetLastByNumeroCommandeExterne(string numeroCommandeExterne)
        {
            return context.Commandes
                              .Where(x => x.NumeroCommandeExterne.Contains(numeroCommandeExterne))
                              .OrderByDescending(x => x.CommandeId)
                              .Select(x => x.NumeroCommandeExterne)
                              .FirstOrDefault();
        }

        /// <summary>
        /// Récupération du détail d'une commande énergie
        /// </summary>
        /// <param name="commandeId">Identifiant de la commande</param>
        /// <returns>Commande énergie</returns>
        public CommandeEnt GetCommandeEnergie(int commandeId)
        {
            var query = context.Commandes.Where(x => x.CommandeId == commandeId && !x.DateSuppression.HasValue && x.IsEnergie);

            var commande = query.Select(x => new
            {
                x.CommandeId,
                x.Numero,
                x.NumeroCommandeExterne,
                x.CiId,
                x.StatutCommandeId,
                x.StatutCommande,
                x.TypeId,
                x.Date,
                x.HangfireJobId,
                CI = new
                {
                    x.CI.CiId,
                    x.CI.Code,
                    x.CI.Libelle,
                    x.CI.SocieteId,
                    Societe = new
                    {
                        x.CI.Societe.SocieteId,
                        x.CI.Societe.Code,
                        x.CI.Societe.Libelle
                    }
                },
                x.FournisseurId,
                Fournisseur = new
                {
                    x.Fournisseur.FournisseurId,
                    x.Fournisseur.Code,
                    x.Fournisseur.Libelle,
                    x.Fournisseur.GroupeId
                },
                x.TypeEnergie,
                x.TypeEnergieId,
                Lignes = x.Lignes.Select(y => new
                {
                    y.TacheId,
                    y.Tache,
                    y.RessourceId,
                    y.Ressource,
                    y.Unite,
                    y.UniteId,
                    y.PersonnelId,
                    y.Personnel,
                    y.MaterielId,
                    y.Materiel,
                    y.PUHT,
                    y.Quantite,
                    y.Libelle,
                    y.CommandeLigneId,
                    y.CommandeId,
                    y.Commentaire
                })
            })
            .FirstOrDefault();

            return new CommandeEnt
            {
                CommandeId = commande.CommandeId,
                Numero = commande.Numero,
                NumeroCommandeExterne = commande.NumeroCommandeExterne,
                StatutCommandeId = commande.StatutCommandeId,
                StatutCommande = new StatutCommandeEnt
                {
                    StatutCommandeId = commande.StatutCommandeId.Value,
                    Code = commande.StatutCommande.Code,
                    Libelle = commande.StatutCommande.Libelle
                },
                TypeId = commande.TypeId,
                CiId = commande.CiId,
                Date = commande.Date,
                HangfireJobId = commande.HangfireJobId,
                CI = new CIEnt
                {
                    CiId = commande.CI.CiId,
                    Code = commande.CI.Code,
                    Libelle = commande.CI.Libelle,
                    SocieteId = commande.CI.SocieteId,
                    Societe = new SocieteEnt
                    {
                        SocieteId = commande.CI.Societe.SocieteId,
                        Code = commande.CI.Societe.Code,
                        Libelle = commande.CI.Societe.Libelle
                    }
                },
                FournisseurId = commande.FournisseurId,
                Fournisseur = new FournisseurEnt
                {
                    FournisseurId = commande.Fournisseur.FournisseurId,
                    Code = commande.Fournisseur.Code,
                    Libelle = commande.Fournisseur.Libelle,
                    GroupeId = commande.Fournisseur.GroupeId
                },
                TypeEnergie = commande.TypeEnergie,
                TypeEnergieId = commande.TypeEnergieId,
                Lignes = commande.Lignes.Select(y => new CommandeLigneEnt
                {
                    TacheId = y.TacheId,
                    Tache = y.TacheId.HasValue ? new TacheEnt
                    {
                        TacheId = y.Tache.TacheId,
                        Code = y.Tache.Code,
                        Libelle = y.Tache.Libelle
                    }
                    : null,
                    RessourceId = y.RessourceId,
                    Ressource = y.RessourceId.HasValue ? new RessourceEnt
                    {
                        RessourceId = y.Ressource.RessourceId,
                        Code = y.Ressource.Code,
                        Libelle = y.Ressource.Libelle
                    }
                    : null,
                    Unite = y.Unite,
                    UniteId = y.UniteId,
                    PersonnelId = y.PersonnelId,
                    Personnel = y.PersonnelId.HasValue ? new PersonnelEnt
                    {
                        PersonnelId = y.Personnel.PersonnelId,
                        Nom = y.Personnel.Nom,
                        Prenom = y.Personnel.Prenom,
                        Matricule = y.Personnel.Matricule,
                    }
                    : null,
                    MaterielId = y.MaterielId,
                    Materiel = y.MaterielId.HasValue ? new MaterielEnt
                    {
                        MaterielId = y.Materiel.MaterielId,
                        Code = y.Materiel.Code,
                        Libelle = y.Materiel.Libelle
                    }
                    : null,
                    Quantite = y.Quantite,
                    PUHT = y.PUHT,
                    Libelle = y.Libelle,
                    CommandeLigneId = y.CommandeLigneId,
                    CommandeId = y.CommandeId,
                    Commentaire = y.Commentaire
                }).ToList()
            };
        }

        /// <summary>
        /// Ajout des réceptions en BD 
        /// </summary>
        /// <param name="receptions">Liste des réceptions</param>      
        public void AddRangeReception(List<DepenseAchatEnt> receptions)
        {
            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    context.ChangeTracker.AutoDetectChangesEnabled = false;
                    context.DepenseAchats.AddRange(receptions);
                    context.SaveChanges();
                    trans.Commit();

                }
                catch (Exception e)
                {
                    trans.Rollback();
                    throw new FredRepositoryException(e.Message, e);
                }
                finally
                {
                    context.ChangeTracker.AutoDetectChangesEnabled = true;
                }
            }
        }

        /// <summary>
        /// Récupération des lignes de commande énergie pour réception
        /// </summary>
        /// <param name="commandeId">Identifiant de la commande énergie</param>
        /// <returns>Liste de lignes de commandes</returns>
        public List<CommandeLigneEnt> GetCommandeLignesForReception(int commandeId)
        {
            var query = context.CommandeLigne.Where(x => x.CommandeId == commandeId
                                                             && x.PUHT != 0
                                                             && x.Quantite != 0
                                                             && x.AllDepenses.Count == 0);

            var lignes = query.Select(x => new
            {
                x.Commande.CiId,
                x.Commande.FournisseurId,
                x.Commande.Date,
                x.Commande.CommentaireInterne,
                x.Commande.DeviseId,
                x.CommandeId,
                x.TacheId,
                x.RessourceId,
                x.UniteId,
                x.PersonnelId,
                x.PUHT,
                x.Quantite,
                x.Libelle,
                x.CommandeLigneId,
                x.MaterielId,
                PersonnelSocieteCode = x.Personnel.Societe.Code,
                x.Personnel.Matricule,
                MaterielSocieteCode = x.Materiel.Societe.Code,
                MaterielCode = x.Materiel.Code
            }).ToList();

            return lignes.Select(x => new CommandeLigneEnt
            {
                CommandeLigneId = x.CommandeLigneId,
                CommandeId = x.CommandeId,
                TacheId = x.TacheId,
                RessourceId = x.RessourceId,
                UniteId = x.UniteId,
                PersonnelId = x.PersonnelId,
                PUHT = x.PUHT,
                Quantite = x.Quantite,
                Libelle = x.Libelle,
                MaterielId = x.MaterielId,
                Commande = new CommandeEnt
                {
                    CommandeId = x.CommandeId,
                    CiId = x.CiId,
                    FournisseurId = x.FournisseurId,
                    Date = x.Date,
                    CommentaireInterne = x.CommentaireInterne,
                    DeviseId = x.DeviseId
                },
                Materiel = new MaterielEnt
                {
                    Code = x.MaterielCode,
                    Societe = new SocieteEnt { Code = x.MaterielSocieteCode }
                },
                Personnel = new PersonnelEnt
                {
                    Matricule = x.Matricule,
                    Societe = new SocieteEnt { Code = x.PersonnelSocieteCode }
                }
            })
            .ToList();
        }

        /// <summary>
        /// Récupération des réceptions éligibles à l'envoie vers SAP d'une commande énergie
        /// </summary>
        /// <param name="commandeId">Identifiant de la commande</param>
        /// <returns>Liste de Réceptions</returns>
        public List<DepenseAchatEnt> GetReceptionsForSap(int commandeId)
        {
            int depenseTypeReceptionCode = DepenseType.Reception.ToIntValue();

            return context.DepenseAchats.Where(x => x.DepenseType.Code == depenseTypeReceptionCode
                                                        && !x.DateSuppression.HasValue
                                                        && string.IsNullOrEmpty(x.HangfireJobId)
                                                        && x.CommandeLigne.CommandeId == commandeId
                                                        && x.CommandeLigne.Commande.IsEnergie
                                                        && !x.CommandeLigne.Commande.DateSuppression.HasValue)
                                    .AsNoTracking()
                                    .ToList();
        }

        /// <summary>
        /// Récupération des lignes de commandes pour l'annulation des valorisations après réception
        /// Chaque personnel/Matériel possède sa liste de Valorisation sur le CI et sur la période de la commande
        /// </summary>
        /// <param name="commandeId">Identifiant de la commande énergie</param>        
        /// <returns>Liste des lignes de commandes</returns>
        public List<CommandeLigneEnt> GetCommandeLignesForValorisation(int commandeId)
        {
            var cmdInfo = context.Commandes.Where(x => x.CommandeId == commandeId).Select(x => new { x.CiId, x.Date }).FirstOrDefault();

            var query = context.CommandeLigne
                                        .Where(x => x.CommandeId == commandeId
                                                    && (x.MaterielId.HasValue || x.PersonnelId.HasValue))
                                        .Select(x => new
                                        {
                                            x.CommandeLigneId,
                                            x.PersonnelId,
                                            x.MaterielId,
                                            CiId = x.Commande.CiId.Value,
                                            x.Commande.Date,
                                            TypeEnergieCode = x.Commande.TypeEnergie.Code,
                                            PersonnelValorisations = x.Personnel.Valorisations.DefaultIfEmpty()
                                                                                              .Where(y => y.CiId == cmdInfo.CiId && ((100 * y.Date.Year) + y.Date.Month) == ((100 * cmdInfo.Date.Year) + cmdInfo.Date.Month))
                                                                                              .Select(p => new
                                                                                              {
                                                                                                  p.RapportId,
                                                                                                  p.RapportLigneId,
                                                                                                  p.ChapitreId,
                                                                                                  p.SousChapitreId,
                                                                                                  p.ReferentielEtenduId,
                                                                                                  p.BaremeId,
                                                                                                  p.UniteId,
                                                                                                  p.DeviseId,
                                                                                                  p.PUHT,
                                                                                                  p.Quantite,
                                                                                                  p.Date
                                                                                              }),
                                            MaterielValorisations = x.Materiel.Valorisations.DefaultIfEmpty()
                                                                                               .Where(y => y.CiId == cmdInfo.CiId && ((100 * y.Date.Year) + y.Date.Month) == ((100 * cmdInfo.Date.Year) + cmdInfo.Date.Month))
                                                                                               .Select(p => new
                                                                                               {
                                                                                                   p.RapportId,
                                                                                                   p.RapportLigneId,
                                                                                                   p.ChapitreId,
                                                                                                   p.SousChapitreId,
                                                                                                   p.ReferentielEtenduId,
                                                                                                   p.BaremeId,
                                                                                                   p.UniteId,
                                                                                                   p.DeviseId,
                                                                                                   p.PUHT,
                                                                                                   p.Quantite,
                                                                                                   p.Date
                                                                                               })
                                        })
                                        .Where(x => x.PersonnelValorisations.Any() || x.MaterielValorisations.Any())
                                        .ToList();


            return query.Select(x => new CommandeLigneEnt
            {
                CommandeLigneId = x.CommandeLigneId,
                Commande = new CommandeEnt { CommandeId = commandeId, CiId = x.CiId, Date = x.Date, TypeEnergie = new TypeEnergieEnt { Code = x.TypeEnergieCode } },
                PersonnelId = x.PersonnelId,
                Personnel = x.PersonnelId.HasValue ? new PersonnelEnt
                {
                    PersonnelId = x.PersonnelId.Value,
                    Valorisations = x.PersonnelValorisations?.Select(p => new ValorisationEnt
                    {
                        RapportId = p.RapportId,
                        RapportLigneId = p.RapportLigneId,
                        ChapitreId = p.ChapitreId,
                        SousChapitreId = p.SousChapitreId,
                        ReferentielEtenduId = p.ReferentielEtenduId,
                        BaremeId = p.BaremeId,
                        UniteId = p.UniteId,
                        DeviseId = p.DeviseId,
                        PUHT = p.PUHT,
                        Quantite = p.Quantite,
                        Date = p.Date
                    })
                    .OrderBy(c => c.Date).ToList()
                }
                : null,
                MaterielId = x.MaterielId,
                Materiel = x.MaterielId.HasValue ? new MaterielEnt
                {
                    MaterielId = x.MaterielId.Value,
                    Valorisations = x.MaterielValorisations?.Select(p => new ValorisationEnt
                    {
                        RapportId = p.RapportId,
                        RapportLigneId = p.RapportLigneId,
                        ChapitreId = p.ChapitreId,
                        SousChapitreId = p.SousChapitreId,
                        ReferentielEtenduId = p.ReferentielEtenduId,
                        BaremeId = p.BaremeId,
                        UniteId = p.UniteId,
                        DeviseId = p.DeviseId,
                        PUHT = p.PUHT,
                        Quantite = p.Quantite,
                        Date = p.Date
                    })
                    .OrderBy(c => c.Date).ToList()
                }
                : null
            })
            .ToList();
        }

        /// <summary>
        /// Ajout des valorsations en BD
        /// </summary>
        /// <param name="valorisations">Liste des valorisations</param>
        /// <returns>Liste des valorisations ajoutées</returns>
        public List<ValorisationEnt> AddRangeValorisation(List<ValorisationEnt> valorisations)
        {
            using (var trans = context.Database.BeginTransaction())
            {
                try
                {
                    context.Valorisations.AddRange(valorisations);

                    context.SaveChanges();

                    trans.Commit();

                    return valorisations;
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    throw new FredRepositoryException(e.Message, e);
                }
            }
        }
    }
}
