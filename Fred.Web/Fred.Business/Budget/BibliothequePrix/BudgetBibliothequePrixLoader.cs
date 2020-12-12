using System;
using System.Linq;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Entities.CI;
using Fred.Web.Shared.App_LocalResources;
using static Fred.Web.Shared.Models.Budget.BibliothequePrix.BibliothequePrixLoad;

namespace Fred.Business.Budget.BibliothequePrix
{
    /// <summary>
    /// Permet de charger une bibliothèque des prix.
    /// </summary>
    public class BudgetBibliothequePrixLoader : BudgetBibliothequePrixLoaderBase
    {
        #region Membres

        private readonly int organisationId;
        private readonly int? deviseId;
        private readonly IOrganisationRepository organisationRepository;
        private readonly ISocieteDeviseRepository societeDeviseRepository;
        private readonly ICIRepository ciRepository;
        private readonly IRessourceRepository ressourceRepository;

        private ResultModel ret;
        private OrganisationModel organisationCible;

        #endregion
        #region Constructeur

        public BudgetBibliothequePrixLoader(
            int organisationId,
            int? deviseId,
            IOrganisationRepository organisationRepository,
            ISocieteDeviseRepository societeDeviseRepository,
            ICIRepository ciRepository,
            IRessourceRepository ressourceRepository,
            IUniteRepository uniteRepository,
            IUniteSocieteRepository uniteSocieteRepository,
            IBudgetBibliothequePrixRepository budgetBibliothequePrixRepository) : base(uniteRepository, uniteSocieteRepository, budgetBibliothequePrixRepository)
        {
            this.organisationId = organisationId;
            this.deviseId = deviseId;
            this.organisationRepository = organisationRepository;
            this.societeDeviseRepository = societeDeviseRepository;
            this.ciRepository = ciRepository;
            this.ressourceRepository = ressourceRepository;
        }

        #endregion
        #region Chargement

        /// <summary>
        /// Charge la bibliothèque des prix.
        /// </summary>
        /// <returns>Le résultat du chargement.</returns>
        public ResultModel Load()
        {
            organisationCible = null;
            OrganisationModel etablissementOrganisation = null;
            ret = new ResultModel();

            // Charge la bibliothèque des prix
            if (!LoadOrganisationCible()
             || !LoadOrganisationEtablissement(out etablissementOrganisation)
             || !LoadSociete()
             || !LoadUnites()
             || !LoadDevises()
             || !LoadChapitres())
            {
                return ret;
            }

            // Charge les éléments des organisations
            if (etablissementOrganisation != null)
            {
                ret.Organisations.Add(etablissementOrganisation);
                LoadItems(etablissementOrganisation);
            }

            ret.Organisations.Add(organisationCible);
            LoadItems(organisationCible);

            return ret;
        }

        /// <summary>
        /// Charge l'organisation cible (établissement ou CI)
        /// </summary>
        /// <returns>True en cas de succès, sinon false.</returns>
        private bool LoadOrganisationCible()
        {
            organisationCible = GetOrganisation(organisationId);
            if (organisationCible == null)
            {
                ret.Erreur = FeatureBudgetBibliothequePrix.Budget_BibliothequePrix_Chargement_Erreur_OrganisationNexistePas;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Charge l'organisation de l'établissement, le cas échéant.
        /// </summary>
        /// <param name="etablissementOrganisation">L'organisation de l'établissement, peut-être null.</param>
        /// <returns>True en cas de succès, sinon false.</returns>
        private bool LoadOrganisationEtablissement(out OrganisationModel etablissementOrganisation)
        {
            // Si l'organisation demandée est un CI, l'organisation de l'établissement parent doit exister
            etablissementOrganisation = null;
            if (organisationCible.Type == (int)OrganisationType.Ci)
            {
                var etablissementOrganisationId = organisationRepository.GetParentId(organisationId, OrganisationType.Etablissement);
                if (etablissementOrganisationId.HasValue)
                {
                    etablissementOrganisation = GetOrganisation(etablissementOrganisationId.Value);
                    if (etablissementOrganisation == null)
                    {
                        ret.Erreur = FeatureBudgetBibliothequePrix.Budget_BibliothequePrix_Chargement_Erreur_OrganisationEtablissementNexistePas;
                        return false;
                    }
                }
                else
                {
                    ret.Erreur = FeatureBudgetBibliothequePrix.Budget_BibliothequePrix_Chargement_Erreur_OrganisationEtablissementNexistePas;
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Charge la société.
        /// </summary>
        /// <returns>True en cas de succès, sinon false.</returns>
        private bool LoadSociete()
        {
            var societeId = organisationRepository.GetSocieteId(organisationId);

            if (!societeId.HasValue)
            {
                ret.Erreur = FeatureBudgetBibliothequePrix.Budget_BibliothequePrix_Chargement_Erreur_SocieteNexistePas;
                return false;
            }

            ret.SocieteId = societeId.Value;
            return true;
        }

        /// <summary>
        /// Charge les unités.
        /// </summary>
        /// <returns>True en cas de succès, sinon false.</returns>
        private bool LoadUnites()
        {
            ret.Unites = GetUnites(ret.SocieteId, UniteModel.Selector);

            foreach (var unite in ret.Unites)
            {
                unite.UniteSociete = true;
            }

            if (ret.Unites.Count == 0)
            {
                ret.Erreur = FeatureBudgetBibliothequePrix.Budget_BibliothequePrix_Chargement_Erreur_SocieteAucuneUnite;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Charge les devises.
        /// </summary>
        /// <returns>True en cas de succès, sinon false.</returns>
        private bool LoadDevises()
        {
            // Ajoute les devises
            if (organisationCible.Type == (int)OrganisationType.Etablissement)
            {
                // Pour un établissement on récupère les devises de la société parente
                return LoadSocieteDevises(ret.SocieteId);
            }
            else if (organisationCible.Type == (int)OrganisationType.Ci)
            {
                // Pour un CI on récupère les devises de ce CI
                return LoadCiDevises(organisationCible.TargetId);
            }
            return false;
        }

        /// <summary>
        /// Charge les devises d'une société.
        /// </summary>
        /// <param name="societeId">L'identifiant de la société.</param>
        /// <returns>True en cas de succès, sinon false.</returns>
        private bool LoadSocieteDevises(int societeId)
        {
            var devises = societeDeviseRepository.Get()
                .Where(sd => sd.SocieteId == societeId && sd.Devise.Active)
                .Select(sd => new
                {
                    sd.DeviseDeReference,
                    Model = new DeviseModel
                    {
                        DeviseId = sd.DeviseId,
                        Symbole = string.IsNullOrEmpty(sd.Devise.Symbole) ? sd.Devise.IsoCode : sd.Devise.Symbole,
                        Libelle = sd.Devise.Libelle
                    }
                })
                .ToList();

            if (devises.Count == 0)
            {
                ret.Erreur = FeatureBudgetBibliothequePrix.Budget_BibliothequePrix_Chargement_Erreur_SocieteAucuneDevise;
                return false;
            }

            ret.Devises.AddRange(devises.Select(d => d.Model));
            return LoadDeviseToUse(() => devises.FirstOrDefault(d => d.DeviseDeReference)?.Model.DeviseId);
        }

        /// <summary>
        /// Charge les devises d'un CI.
        /// </summary>
        /// <param name="ciId">L'identifiant du CI.</param>
        /// <returns>True en cas de succès, sinon false.</returns>
        private bool LoadCiDevises(int ciId)
        {
            // Attention : il faut caster en FredRepository<CIEnt> pour accéder à la bonne function Get
            // Sans le cast, Get sera -> IEnumerable<CIEnt> Get(bool onlyChantierFred = false)
            var devises = ((IRepository<CIEnt>)ciRepository).Get()
                .Where(c => c.CiId == ciId)
                .SelectMany(c => c.CIDevises
                    .Select(cd => new
                    {
                        cd.Reference,
                        cd.Devise.Active,
                        Model = new DeviseModel
                        {
                            DeviseId = cd.DeviseId,
                            Symbole = string.IsNullOrEmpty(cd.Devise.Symbole) ? cd.Devise.IsoCode : cd.Devise.Symbole,
                            Libelle = cd.Devise.Libelle
                        }
                    })
                    .Where(d => d.Active)
                )
                .ToList();

            if (devises.Count == 0)
            {
                ret.Erreur = FeatureBudgetBibliothequePrix.Budget_BibliothequePrix_Chargement_Erreur_CiAucuneDevise;
                return false;
            }

            ret.Devises.AddRange(devises.Select(d => d.Model));
            return LoadDeviseToUse(() => devises.FirstOrDefault(d => d.Reference)?.Model.DeviseId);
        }

        /// <summary>
        /// Charge la devise utilisée.
        /// </summary>
        /// <param name="getDeviseIdDeReference">Fonction qui retourne l'identifiant de la devise de référence.</param>
        /// <returns>True en cas de succès, sinon false.</returns>
        private bool LoadDeviseToUse(Func<int?> getDeviseIdDeReference)
        {
            // Si la devise est indiquée, elle doit exister dans la liste
            if (deviseId != null)
            {
                var deviseToUse = ret.Devises.FirstOrDefault(d => d.DeviseId == deviseId);
                if (deviseToUse == null)
                {
                    ret.Erreur = FeatureBudgetBibliothequePrix.Budget_BibliothequePrix_Chargement_Erreur_DeviseNonDisponible;
                    return false;
                }
                ret.DeviseId = deviseToUse.DeviseId;
            }

            // Si la devise n'est pas indiquée, on utilise celle par défaut si elle existe ou la première de la liste
            else
            {
                ret.DeviseId = getDeviseIdDeReference() ?? ret.Devises[0].DeviseId;
            }

            return true;
        }

        /// <summary>
        /// Charge les chapitres.
        /// </summary>
        /// <returns>True en cas de succès, sinon false.</returns>
        private bool LoadChapitres()
        {
            int? ciId = null;
            if (organisationCible.Type == (int)OrganisationType.Ci)
            {
                ciId = organisationCible.TargetId;
            }

            var ressources = ressourceRepository.GetListBySocieteIdWithSousChapitreEtChapitre(ret.SocieteId, ciId).ToList();
            ret.Chapitres = ressources
                .Select(r => r.SousChapitre.Chapitre)
                .Distinct()
                .Select(ChapitreModel.Selector).ToList();

            return true;
        }

        /// <summary>
        /// Charge les éléments d'une organisation.
        /// </summary>
        /// <param name="organisation">L'organisation concernée.</param>
        private void LoadItems(OrganisationModel organisation)
        {
            // Note : si ret.DeviseId est null c'est que cette fonction n'aurait pas dûe être appelée
            organisation.Items = GetItems(organisation.OrganisationId, ret.DeviseId.Value, ItemModel.Selector);

            foreach (var item in organisation.Items)
            {
                // NPI : ici les 2 dates ont un kind unspecified... pourquoi ?!
                item.DateCreation = item.DateCreation.HasValue ? DateTime.SpecifyKind(item.DateCreation.Value, DateTimeKind.Utc) : (DateTime?)null;
                item.DateModification = item.DateModification.HasValue ? DateTime.SpecifyKind(item.DateModification.Value, DateTimeKind.Utc) : (DateTime?)null;

                if (item.UniteId.HasValue && !ret.Unites.Any(u => u.UniteId == item.UniteId))
                {
                    var unite = GetUnite(item.UniteId.Value, UniteModel.Selector);
                    unite.UniteSociete = false;
                    ret.Unites.Add(unite);
                }
            }
        }

        #endregion
        #region Autre

        /// <summary>
        /// Retourne une organisation (établissement ou CI) en fonction de son identifiant.
        /// </summary>
        /// <param name="organisationId">Identifiant de l'organisation concernée.</param>
        /// <returns>L'organisation.</returns>
        private OrganisationModel GetOrganisation(int organisationId)
        {
            var organisation = organisationRepository.Get()
                .Where(o => o.OrganisationId == organisationId)
                .Select(o => new
                {
                    Etablissement = o.TypeOrganisationId == (int)OrganisationType.Etablissement ? o.Etablissement : null,
                    CI = o.TypeOrganisationId == (int)OrganisationType.Ci ? o.CI : null,
                    Model = new OrganisationModel
                    {
                        OrganisationId = o.OrganisationId,
                        Type = o.TypeOrganisationId,
                        TypeCode = o.TypeOrganisation.Code
                    }
                })
                .FirstOrDefault();

            if (organisation == null)
            {
                return null;
            }

            if (organisation.Etablissement != null)
            {
                organisation.Model.TargetId = organisation.Etablissement.EtablissementComptableId;
                organisation.Model.Code = organisation.Etablissement.Code;
            }
            else if (organisation.CI != null)
            {
                organisation.Model.TargetId = organisation.CI.CiId;
                organisation.Model.Code = organisation.CI.Code;
            }
            else
            {
                return null;
            }

            return organisation.Model;
        }

        #endregion
    }
}
