using System.Linq;
using AutoMapper;
using Fred.Business;
using Fred.Business.BaremeExploitation.ExportBaremeModel;
using Fred.Business.Budget.Extensions;
using Fred.Business.ExplorateurDepense;
using Fred.Business.FeatureFlipping.Model;
using Fred.Business.Habilitation.Models;
using Fred.Business.Rapport;
using Fred.Business.Rapport.Pointage;
using Fred.Business.Reception;
using Fred.Business.Referential.TypeRattachement;
using Fred.Business.Template.Models.Avis;
using Fred.Entities;
using Fred.Entities.Adresse;
using Fred.Entities.Affectation;
using Fred.Entities.Avis;
using Fred.Entities.Bareme;
using Fred.Entities.Bareme.Search;
using Fred.Entities.Budget;
using Fred.Entities.Budget.Recette;
using Fred.Entities.Budget.Search;
using Fred.Entities.Carburant;
using Fred.Entities.CI;
using Fred.Entities.Commande;
using Fred.Entities.CompteExploitation;
using Fred.Entities.DatesCalendrierPaie;
using Fred.Entities.DatesClotureComptable;
using Fred.Entities.Delegation;
using Fred.Entities.Depense;
using Fred.Entities.Directory;
using Fred.Entities.Facturation;
using Fred.Entities.Facture;
using Fred.Entities.Favori;
using Fred.Entities.FeatureFlipping;
using Fred.Entities.Fonctionnalite;
using Fred.Entities.Groupe;
using Fred.Entities.Habilitation;
using Fred.Entities.Holding;
using Fred.Entities.Image;
using Fred.Entities.Import;
using Fred.Entities.IndemniteDeplacement;
using Fred.Entities.Journal;
using Fred.Entities.Module;
using Fred.Entities.Notification;
using Fred.Entities.OperationDiverse;
using Fred.Entities.Organisation;
using Fred.Entities.OrganisationGenerique;
using Fred.Entities.Params;
using Fred.Entities.Permission;
using Fred.Entities.PermissionFonctionnalite;
using Fred.Entities.Personnel;
using Fred.Entities.Personnel.Interimaire;
using Fred.Entities.PointagePersonnel.Search;
using Fred.Entities.Pole;
using Fred.Entities.Rapport;
using Fred.Entities.Rapport.Search;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielEtendu;
using Fred.Entities.ReferentielFixe;
using Fred.Entities.RessourcesRecommandees;
using Fred.Entities.Role;
using Fred.Entities.RoleFonctionnalite;
using Fred.Entities.Search;
using Fred.Entities.Utilisateur;
using Fred.Entities.Valorisation;
using Fred.Entities.VerificationPointage;
using Fred.Web.Models;
using Fred.Web.Models.Affectation;
using Fred.Web.Models.AffectationInterimaire;
using Fred.Web.Models.Budget;
using Fred.Web.Models.Budget.Liste;
using Fred.Web.Models.Carburant;
using Fred.Web.Models.CI;
using Fred.Web.Models.CodeAbsence;
using Fred.Web.Models.CodeZoneDeplacement;
using Fred.Web.Models.Commande;
using Fred.Web.Models.DatesCalendrierPaie;
using Fred.Web.Models.DatesClotureComptable;
using Fred.Web.Models.Delegation;
using Fred.Web.Models.Depense;
using Fred.Web.Models.ExternalDirectory;
using Fred.Web.Models.Facture;
using Fred.Web.Models.Favori;
using Fred.Web.Models.FeatureFlipping;
using Fred.Web.Models.Fonctionnalite;
using Fred.Web.Models.Groupe;
using Fred.Web.Models.Holding;
using Fred.Web.Models.IndemniteDeplacement;
using Fred.Web.Models.Module;
using Fred.Web.Models.Notification;
using Fred.Web.Models.Organisation;
using Fred.Web.Models.OrganisationGenerique;
using Fred.Web.Models.Personnel;
using Fred.Web.Models.Personnel.Interimaire;
using Fred.Web.Models.Pole;
using Fred.Web.Models.Rapport;
using Fred.Web.Models.Referential;
using Fred.Web.Models.Referential.Light;
using Fred.Web.Models.ReferentielEtendu;
using Fred.Web.Models.ReferentielEtendu.Light;
using Fred.Web.Models.ReferentielFixe;
using Fred.Web.Models.ReferentielFixe.Light;
using Fred.Web.Models.Role;
using Fred.Web.Models.Search;
using Fred.Web.Models.Utilisateur;
using Fred.Web.Shared.Models;
using Fred.Web.Shared.Models.Affectation;
using Fred.Web.Shared.Models.Avis;
using Fred.Web.Shared.Models.Bareme;
using Fred.Web.Shared.Models.Bareme.Search;
using Fred.Web.Shared.Models.Budget;
using Fred.Web.Shared.Models.Budget.ControleBudgetaire;
using Fred.Web.Shared.Models.Budget.Recette;
using Fred.Web.Shared.Models.Budget.Search;
using Fred.Web.Shared.Models.Commande;
using Fred.Web.Shared.Models.CompteExploitation;
using Fred.Web.Shared.Models.DatesClotureComptable;
using Fred.Web.Shared.Models.Image;
using Fred.Web.Shared.Models.Import;
using Fred.Web.Shared.Models.IndemniteDeplacement;
using Fred.Web.Shared.Models.Moyen;
using Fred.Web.Shared.Models.OperationDiverse;
using Fred.Web.Shared.Models.Param;
using Fred.Web.Shared.Models.Personnel.Interimaire;
using Fred.Web.Shared.Models.PieceJointe;
using Fred.Web.Shared.Models.PointagePersonnel;
using Fred.Web.Shared.Models.Rapport;
using Fred.Web.Shared.Models.Rapport.ExportPointagePersonnel;
using Fred.Web.Shared.Models.Rapport.RapportHebdo;
using Fred.Web.Shared.Models.Rapport.Search;
using Fred.Web.Shared.Models.Referential;
using Fred.Web.Shared.Models.ReferentielEtendu;
using Fred.Web.Shared.Models.RessourceRecommandee;
using Fred.Web.Shared.Models.RoleFonctionnalite;
using Fred.Web.Shared.Models.Valorisation;

namespace Bootstrapper.AutoMapper
{
    public static class AutoMapperConfig
    {
        public static IMapper CreateMapper()
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                CommandeMapper.Map(cfg);

                cfg.CreateMap<FilterChekingPointingModel, FilterChekingPointing>().ReverseMap();
                cfg.CreateMap<ChekingPointingMonth, ChekingPointingMonthModel>().ReverseMap();

                cfg.CreateMap<SearchDetailBudgetEnt, SearchDetailBudgetModel>().ReverseMap();

                cfg.CreateMap<SearchBaremeExploitationOrganisationEnt, SearchBaremeExploitationOrganisationModel>().ReverseMap();
                cfg.CreateMap<SearchBaremeExploitationCIEnt, SearchBaremeExploitationCIModel>().ReverseMap();
                cfg.CreateMap<CILightEnt, CIModel>().ReverseMap();
                cfg.CreateMap<SearchTacheEnt, SearchTacheModel>().ReverseMap();
                cfg.CreateMap<SearchCommandeEnt, SearchCommandeModel>().ReverseMap();
                cfg.CreateMap<SearchReceivableOrdersFilter, SearchReceivableOrdersFilterModel>().ReverseMap();
                cfg.CreateMap<AgenceLightEnt, AgenceLightModel>().ReverseMap();
                cfg.CreateMap<SearchAvancementEnt, SearchAvancementModel>().ReverseMap();
                cfg.CreateMap<SearchControleBudgetaireEnt, SearchControleBudgetaireModel>().ReverseMap();
                cfg.CreateMap<SearchControleBudgetaireFilterEnt, SearchControleBudgetaireFilterModel>().ReverseMap();
                cfg.CreateMap<SearchListeBudgetEnt, SearchListeBudgetModel>().ReverseMap();
                cfg.CreateMap<SearchBibliothequePrixEnt, SearchBibliothequePrixModel>().ReverseMap();
                cfg.CreateMap<SearchListePointagePersonnelEnt, SearchListePointagePersonnelModel>().ReverseMap();
                cfg.CreateMap<SystemeExterneLightEnt, SystemeExterneLightModel>().ReverseMap();
                cfg.CreateMap<SystemeExterneEnt, SystemeExterneLightModel>().ReverseMap();
                cfg.CreateMap<FournisseurLightEnt, FournisseurModel>().ReverseMap();
                cfg.CreateMap<AgenceModel, AgenceLightEnt>().ReverseMap();
                cfg.CreateMap<CILightEnt, CIModel>().ReverseMap();

                cfg.CreateMap<PersonnelLightEnt, PersonnelLightModel>().ReverseMap();
                cfg.CreateMap<CILightEnt, CILightModel>().ReverseMap();
                cfg.CreateMap<RessourceLightEnt, RessourceLightModel>().ReverseMap();
                cfg.CreateMap<TacheLightEnt, TacheLightModel>().ReverseMap();
                cfg.CreateMap<SearchOperationDiverseEnt, SearchOperationDiverseModel>().ReverseMap();
                cfg.CreateMap<SearchCompteExploitationEditionEnt, SearchCompteExploitationEditionModel>().ReverseMap();

                cfg.CreateMap<SearchReceptionnableResult, SearchReceptionnableResultModel>().ReverseMap();
                cfg.CreateMap<DepenseAchatEnt, DepenseAchatModel>().ReverseMap();
                cfg.CreateMap<DepenseAchatEnt, DepenseAchatLightModel>().ReverseMap();
                cfg.CreateMap<SearchDepenseEnt, SearchDepenseModel>().ReverseMap();
                cfg.CreateMap<StatutCommandeEnt, StatutCommandeModel>().ReverseMap();
                cfg.CreateMap<EquipeEnt, EquipeModel>().ReverseMap();
                cfg.CreateMap<AffectationEnt, AffectationModel>().ReverseMap();
                cfg.CreateMap<AstreinteEnt, AstreinteModel>()
                  .ForMember(dest => dest.AstreinteId, opt => opt.MapFrom(src => src.AstreintId))
                  .ReverseMap();
                cfg.CreateMap<AstreinteViewEnt, AstreinteViewModel>().ReverseMap();
                cfg.CreateMap<AffectationViewEnt, AffectationViewModel>().ReverseMap();
                cfg.CreateMap<CalendarAffectationViewEnt, CalendarAffectationViewModel>().ReverseMap();
                cfg.CreateMap<EquipePersonnelEnt, EquipePersonnelModel>().ReverseMap();

                PersonnelMapper.Map(cfg);
                ImportedEquipeMapper.Map(cfg);
                CalendarAffectationMapper.Map(cfg);

                cfg.CreateMap<ValorisationEnt, ValorisationModel>().ReverseMap();
                cfg.CreateMap<FournisseurEnt, FournisseurModel>().ReverseMap();
                cfg.CreateMap<FournisseurEnt, FournisseurLightModel>().ReverseMap();
                cfg.CreateMap<FournisseurLightEnt, FournisseurLightModel>().ReverseMap();
                cfg.CreateMap<TypeDepenseEnt, TypeDepenseModel>().ReverseMap();
                cfg.CreateMap<TacheEnt, TacheModel>().ReverseMap();
                cfg.CreateMap<UtilisateurEnt, UtilisateurModel>().ReverseMap(); //Problème de Build, suite au mofication du Bruno Build ICWeb_20160524.4
                cfg.CreateMap<UtilisateurEnt, UtilisateurLightModel2>().ReverseMap();
                cfg.CreateMap<DatesCalendrierPaieEnt, DatesCalendrierPaieModel>().ReverseMap();
                cfg.CreateMap<SearchCodeAbsenceEnt, SearchCodeAbsenceModel>().ReverseMap();
                cfg.CreateMap<SearchEtablissementComptableEnt, SearchEtablissementComptableModel>().ReverseMap();
                cfg.CreateMap<SearchMaterielEnt, SearchActiveModel>().ReverseMap();
                cfg.CreateMap<EtablissementComptableEnt, EtablissementComptableModel>().ReverseMap();
                cfg.CreateMap<EtablissementComptableEnt, EtablissementComptableLightModel>().ReverseMap();
                cfg.CreateMap<MaterielEnt, MaterielModel>().ReverseMap();
                cfg.CreateMap<PaysEnt, PaysModel>().ReverseMap();
                cfg.CreateMap<UtilisateurEnt, UtilisateurSansPersonnelModel>().ForMember(model => model.Personnel, option => option.Ignore()).ReverseMap();
                cfg.CreateMap<ExternalDirectoryEnt, ExternalDirectoryModel>().ReverseMap();
                cfg.CreateMap<SearchPersonnelEnt, SearchPersonnelModel>().ReverseMap();

                cfg.CreateMap<ExternalDirectoryEnt, ExternalDirectoryModel>().ReverseMap();
                cfg.CreateMap<DeviseEnt, DeviseModel>().ReverseMap();
                cfg.CreateMap<ModuleEnt, ModuleModel>().ReverseMap();
                cfg.CreateMap<FonctionnaliteEnt, FonctionnaliteModel>().ReverseMap();
                cfg.CreateMap<FonctionnaliteEnt, FonctionnaliteLightModel>().ReverseMap();
                cfg.CreateMap<RoleEnt, RoleModel>().ReverseMap();
                cfg.CreateMap<OrganisationEnt, OrganisationModel>().MaxDepth(3).ReverseMap().MaxDepth(3);
                cfg.CreateMap<OrganisationEnt, OrganisationLightModel>().MaxDepth(3).ReverseMap().MaxDepth(3);
                cfg.CreateMap<ParamValueEnt, ParamValueModel>()
                     .ForMember(dest => dest.Key, opt => opt.MapFrom(src => src.ParamKey.Libelle))
                     .ReverseMap()
                     .AfterMap((src, dest) => { dest.ParamKey = new ParamKeyEnt { Libelle = src.Key }; });
                cfg.CreateMap<OrganisationLightEnt, OrganisationLightModel>().ForMember(dest => dest.TypeOrganisationLibelle, opt => opt.MapFrom(src => src.TypeOrganisation)).ReverseMap();
                cfg.CreateMap<OrganisationLightEnt, OrganisationModel>().ReverseMap();
                cfg.CreateMap<OrganisationGeneriqueEnt, OrganisationGeneriqueModel>().ReverseMap();
                cfg.CreateMap<TypeOrganisationEnt, TypeOrganisationModel>().ReverseMap();
                cfg.CreateMap<OrganisationLienEnt, OrganisationLienModel>().ReverseMap();
                cfg.CreateMap<GroupeEnt, GroupeModel>().ReverseMap();
                cfg.CreateMap<BaremeExploitationCIEnt, BaremeExploitationCIModel>().ReverseMap();
                cfg.CreateMap<BaremeExploitationOrganisationEnt, BaremeExploitationOrganisationModel>().ReverseMap();
                cfg.CreateMap<SurchargeBaremeExploitationCIEnt, SurchargeBaremeExploitationCIModel>().ReverseMap();
                cfg.CreateMap<AffectationSeuilUtilisateurEnt, AffectationSeuilUtilisateurModel>().ReverseMap();
                cfg.CreateMap<AffectationGroupByOrganisationEnt, AffectationGroupByOrganisationModel>().ReverseMap();
                cfg.CreateMap<AffectationSeuilOrgaEnt, AffectationSeuilOrgaModel>().ReverseMap();
                cfg.CreateMap<HoldingEnt, HoldingModel>().ReverseMap();
                cfg.CreateMap<PoleEnt, PoleModel>().ReverseMap();
                cfg.CreateMap<CodeMajorationEnt, CodeMajorationModel>().ReverseMap();
                cfg.CreateMap<SeuilValidationEnt, SeuilValidationModel>().ReverseMap();
                cfg.CreateMap<CodeDeplacementEnt, CodeDeplacementModel>().ReverseMap();
                cfg.CreateMap<SearchCodeDeplacementEnt, SearchCodeDeplacementModel>().ReverseMap();
                cfg.CreateMap<EtablissementPaieEnt, EtablissementPaieModel>().ReverseMap();
                cfg.CreateMap<EtablissementPaieLightEnt, EtablissementPaieModel>().ReverseMap();
                cfg.CreateMap<PrimeEnt, PrimeModel>().ReverseMap();
                cfg.CreateMap<SearchPrimeEnt, SearchActiveModel>().ReverseMap();
                cfg.CreateMap<DatesClotureComptableEnt, DatesClotureComptableModel>().ReverseMap();
                cfg.CreateMap<CodeAbsenceEnt, CodeAbsenceModel>().ReverseMap();

                cfg.CreateMap<DuplicatePointageResult, PointageDuplicateResultModel>();
                cfg.CreateMap<DuplicateRapportResult, RapportDuplicateResultModel>();

                cfg.CreateMap<PointageMensuelEnt, PointageMensuelModel>().ReverseMap();
                cfg.CreateMap<PointageMensuelPersonEnt, PointageMensuelPersonModel>().ReverseMap();
                cfg.CreateMap<RapportEnt, RapportModel>().ReverseMap();
                cfg.CreateMap<RapportEnt, RapportLightModel>().ReverseMap();
                cfg.CreateMap<RapportLigneEnt, RapportLigneModel>().ForMember(model => model.Rapport, option => option.Ignore()).ReverseMap();
                cfg.CreateMap<RapportLigneEnt, RapportMoyenLigneModel>().ReverseMap();
                cfg.CreateMap<RapportLigneEnt, PointagePersonnelModel<CIModel>>().ReverseMap();
                cfg.CreateMap<RapportLigneEnt, PointagePersonnelModel<CIFullLibelleModel>>().ReverseMap();

                cfg.CreateMap<PointagePersonnelInfo, PointagePersonnelLoadModel<CIModel>>();
                cfg.CreateMap<PointagePersonnelInfo, PointagePersonnelLoadModel<CIFullLibelleModel>>();
                cfg.CreateMap<PointageAnticipeEnt, RapportLigneModel>().ReverseMap();
                cfg.CreateMap<RapportLignePrimeEnt, RapportLignePrimeModel>().ForMember(model => model.RapportLigne, option => option.Ignore()).ReverseMap();
                cfg.CreateMap<PointageAnticipePrimeEnt, RapportLignePrimeModel>().ForMember(model => model.RapportLigne, option => option.Ignore()).ReverseMap();
                cfg.CreateMap<RapportLigneTacheEnt, RapportLigneTacheModel>().ForMember(model => model.RapportLigne, option => option.Ignore()).ReverseMap();
                cfg.CreateMap<RapportLigneAstreinteEnt, RapportLigneAstreinteModel>().ReverseMap();
                cfg.CreateMap<RapportLigneMajorationEnt, RapportLigneMajorationModel>().ReverseMap();
                cfg.CreateMap<RapportTacheEnt, RapportTacheModel>().ForMember(model => model.Rapport, option => option.Ignore()).ReverseMap();
                cfg.CreateMap<FavoriEnt, FavoriModel>().ReverseMap();
                cfg.CreateMap<NotificationEnt, NotificationModel>().ReverseMap();
                cfg.CreateMap<CodeZoneDeplacementEnt, CodeZoneDeplacementModel>().ReverseMap();
                cfg.CreateMap<SearchCodeZoneDeplacementEnt, SearchCodeZoneDeplacementModel>().ReverseMap();
                cfg.CreateMap<IndemniteDeplacementEnt, IndemniteDeplacementModel>().ReverseMap();
                cfg.CreateMap<SearchIndemniteDeplacementEnt, SearchIndemniteDeplacementModel>().ReverseMap();
                cfg.CreateMap<SearchRapportEnt, SearchRapportModel>().ReverseMap();
                cfg.CreateMap<SearchRapportLigneEnt, SearchRapportLigneModel>().ReverseMap();
                cfg.CreateMap<RapportStatutEnt, RapportStatutModel>().ReverseMap();
                cfg.CreateMap<ChapitreEnt, ChapitreModel>().ReverseMap();
                cfg.CreateMap<ChapitreEnt, ChapitreLightModel>().ReverseMap();
                cfg.CreateMap<SousChapitreEnt, SousChapitreModel>().ReverseMap();
                cfg.CreateMap<SousChapitreEnt, SousChapitreLightModel>().ReverseMap();
                cfg.CreateMap<TypeRessourceEnt, TypeRessourceModel>().ReverseMap();
                cfg.CreateMap<RessourceEnt, RessourceModel>().ReverseMap();
                cfg.CreateMap<RessourceTacheEnt, RessourceTacheModelOld>().ReverseMap();
                cfg.CreateMap<RessourceEnt, RessourceLightModel>().ReverseMap();
                cfg.CreateMap<MajorationPersonnelCiEnt, MajorationPersonnelCiModel>().ReverseMap();
                cfg.CreateMap<NatureEnt, NatureModel>()
                   .ForMember(model => model.ResourceId, opt => opt.MapFrom(ent => ent.RessourceId))
                   .ReverseMap()
                   .ForMember(ent => ent.RessourceId, opt => opt.MapFrom(model => model.ResourceId));
                cfg.CreateMap<SearchCriteriaEnt<NatureEnt>, SearchValueAndSocietyActiveModel>().ReverseMap();
                cfg.CreateMap<PrimeRapportHebdoEnt, PrimeRapportHebdoModel>().ReverseMap();
                cfg.CreateMap<PrimesPersonnelsGetEnt, PrimesPersonnelsGetModel>().ReverseMap();
                cfg.CreateMap<PrimeAffectationEnt, PrimeAffectationModel>().ReverseMap();
                cfg.CreateMap<PrimePersonnelAffectationEnt, PrimePersonnelAffectationModel>().ReverseMap();
                cfg.CreateMap<MajorationPersonnelsGetEnt, MajorationPersonnelGetModel>().ReverseMap();
                cfg.CreateMap<MajorationAffectationEnt, MajorationAffectationModel>().ReverseMap();
                cfg.CreateMap<MajorationPersonnelAffectationEnt, MajorationPersonnelAffectationModel>().ReverseMap();

                JournalMapper.Map(cfg);

                cfg.CreateMap<ReferentielEtenduEnt, ReferentielEtenduModel>().ReverseMap();
                cfg.CreateMap<RessourceRecommandeeEnt, RessourceRecommandeeModel>().ReverseMap();
                cfg.CreateMap<UniteReferentielEtenduEnt, UniteReferentielEtenduModel>().ReverseMap();
                cfg.CreateMap<ReferentielEtenduEnt, ReferentielEtenduLightModel>().ReverseMap();
                cfg.CreateMap<ParametrageReferentielEtenduEnt, ParametrageReferentielEtenduModel>().ReverseMap();
                cfg.CreateMap<ParametrageReferentielEtenduEnt, ParametrageReferentielEtenduLightModel>().ReverseMap();
                cfg.CreateMap<OrganisationEnt, OrganisationLight>().ReverseMap();
                cfg.CreateMap<CarburantEnt, CarburantModel>().ReverseMap();
                cfg.CreateMap<AbstractSearch, ISearchValueModel>().ReverseMap();
                cfg.CreateMap<SearchDeviseEnt, SearchDeviseModel>().ReverseMap();
                cfg.CreateMap<FactureEnt, FactureModel>().ReverseMap();
                cfg.CreateMap<SearchFactureEnt, SearchFactureModel>().ReverseMap();
                cfg.CreateMap<FactureLigneEnt, FactureLigneModel>().ReverseMap();
                cfg.CreateMap<SearchCriteriaEnt<JournalEnt>, SearchValueAndSocietyActiveModel>().ReverseMap();
                cfg.CreateMap<DelegationEnt, DelegationModel>().ReverseMap();
                cfg.CreateMap<ContratInterimaireEnt, ContratInterimaireModel>().ReverseMap();
                cfg.CreateMap<ZoneDeTravailEnt, ZoneDeTravailModel>().ReverseMap();
                cfg.CreateMap<MatriculeExterneEnt, MatriculeExterneModel>().ReverseMap();
                cfg.CreateMap<PersonnelTotalHourByDayEnt, PersonnelTotalHourByDayModel>().ReverseMap();
                cfg.CreateMap<RapportHebdoPersonnelWithAllCiEnt, RapportHebdoPersonnelWithAllCiModel>().ReverseMap();
                cfg.CreateMap<RapportHebdoPersonnelWithTotalHourEnt, RapportHebdoPersonnelWithTotalHourModel>().ReverseMap();
                cfg.CreateMap<ExportPointagePersonnelFilterModel, ExportAnalytiqueFilterModel>().ReverseMap();
                cfg.CreateMap<PersonnelTotalHourByDayAndByCiEnt, PersonnelTotalHourByDayAndByCiModel>().ReverseMap();

                cfg.CreateMap<DepenseTemporaireEnt, DepenseTemporaireModel>().ReverseMap();
                cfg.CreateMap<SearchDepenseTemporaireEnt, SearchDepenseTemporaireModel>().ReverseMap();
                cfg.CreateMap<SearchEtablissementPaieEnt, SearchActiveModel>().ReverseMap();
                cfg.CreateMap<TypeRattachement, TypeRattachementModel>().ReverseMap();
                cfg.CreateMap<BudgetRecetteEnt, BudgetRecetteModel>().ReverseMap();
                cfg.CreateMap<BudgetEnt, ListeBudgetModel>()
                  .ForMember(dest => dest.BudgetEtat, opt => opt.MapFrom(src => src.BudgetEtat))
                  .ForMember(dest => dest.BudgetId, opt => opt.MapFrom(src => src.BudgetId))
                  .ForMember(dest => dest.Commentaire, opt => opt.MapFrom(src => src.GetDernierCommentaire()))
                  .ForMember(dest => dest.Createur, opt => opt.MapFrom(src => src.GetCreateur()))
                  .ForMember(dest => dest.DateCreation, opt => opt.MapFrom(src => src.GetDateCreation()))
                  .ForMember(dest => dest.AuteurDeniereModification, opt => opt.MapFrom(src => src.GetAuteurDerniereModification()))
                  .ForMember(dest => dest.DateDerniereModification, opt => opt.MapFrom(src => src.GetDateDerniereModification()))
                  .ForMember(dest => dest.DateSuppression, opt => opt.MapFrom(src => src.DateSuppressionBudget))
                  .ForMember(dest => dest.Valideur, opt => opt.MapFrom(src => src.GetValideurBudget()))
                  .ForMember(dest => dest.DateValidation, opt => opt.MapFrom(src => src.GetDateValidation()))
                  .ForMember(dest => dest.MontantTotal, opt => opt.MapFrom(src => src.CalculMontantTotalT4()))
                  .ForMember(dest => dest.Partageable, opt => opt.MapFrom(src => src.IsBudgetPartageableParUtilisateurConnecte()))
                  .ForMember(dest => dest.Partage, opt => opt.MapFrom(src => src.Partage))
                  .ForMember(dest => dest.PeriodeDebutFormatted, opt => opt.MapFrom(src => src.FormatPeriode(src.PeriodeDebut)))
                  .ForMember(dest => dest.PeriodeFinFormatted, opt => opt.MapFrom(src => src.FormatPeriode(src.PeriodeFin)))
                  .ForMember(dest => dest.PeriodeDebut, opt => opt.MapFrom(src => src.PeriodeDebut))
                  .ForMember(dest => dest.PeriodeFin, opt => opt.MapFrom(src => src.PeriodeFin))
                  .ForMember(dest => dest.Version, opt => opt.MapFrom(src => src.Version))
                  .ForMember(dest => dest.Workflows, opt => opt.MapFrom(src => src.Workflows))
                  .ForMember(dest => dest.Recettes, opt => opt.MapFrom(src => src.Recette))
                  .ForMember(dest => dest.SommeRecettes, opt => opt.MapFrom(src => src.GetSommeRecettes()))
                  .ForMember(dest => dest.CopyHistos, opt => opt.MapFrom(src => src.CopyHistos))
                  .AfterMap((src, dest) => dest.Workflows = dest.Workflows.OrderByDescending(w => w.Date));

                cfg.CreateMap<BudgetCopyHistoEnt, BudgetCopyHistoModel>();
                cfg.CreateMap<CIEnt, BudgetCopyHistoCIModel>();

                cfg.CreateMap<ControleBudgetaireValeursEnt, ControleBudgetaireValeursModel>().ReverseMap();

                cfg.CreateMap<ControleBudgetaireValeursEnt, TacheRessourceAjustementModel>()
                  .ForMember(dest => dest.Tache3Id, opt => opt.MapFrom(src => src.TacheId))
                  .ForMember(dest => dest.RessourceId, opt => opt.MapFrom(src => src.RessourceId))
                  .ForMember(dest => dest.Ajustement, opt => opt.MapFrom(src => src.Ajustement));

                cfg.CreateMap<BudgetEtatEnt, BudgetEtatModel>().ReverseMap();
                cfg.CreateMap<BudgetWorkflowEnt, BudgetWorkflowModel>().ReverseMap();
                cfg.CreateMap<BudgetRevisionEnt, BudgetRevisionModelOld>().ReverseMap();
                cfg.CreateMap<ContratInterimaireEnt, AffectationInterimaireModel>().ReverseMap();
                cfg.CreateMap<SearchFournisseurEnt, SearchFournisseurModel>().ReverseMap();
                cfg.CreateMap<TacheRecetteEnt, TacheRecetteModelOld>().ReverseMap();
                cfg.CreateMap<RessourceTacheDeviseEnt, RessourceTacheDeviseModelOld>().ReverseMap();
                cfg.CreateMap<TacheEnt, TacheLightModel>().ReverseMap();
                cfg.CreateMap<DeviseEnt, DeviseLightModel>().ReverseMap();
                cfg.CreateMap<UtilisateurEnt, UtilisateurLightModel>().ReverseMap();
                cfg.CreateMap<PersonnelEnt, PersonnelLightModel>().ReverseMap();
                cfg.CreateMap<UniteEnt, UniteModel>().ReverseMap();
                cfg.CreateMap<UniteEnt, UniteLightModel>().ReverseMap();
                cfg.CreateMap<CarburantOrganisationDeviseEnt, CarburantOrganisationDeviseModel>().ReverseMap();
                cfg.CreateMap<ParametrageReferentielEtenduExportEnt, ParametrageReferentielEtenduExportModel>();
                cfg.CreateMap<PeriodeClotureEnt, PeriodeClotureModel>();
                cfg.CreateMap<MaterielEnt, MaterielDetailModel>().ReverseMap();
                cfg.CreateMap<MaterielEnt, MaterielDetailsPostModel>().ReverseMap();
                cfg.CreateMap<ImageEnt, ImageModel>().ReverseMap();
                ValidationPointageMapper.Map(cfg);
                AuthentificationLogMapper.Map(cfg);
                CIMapper.Map(cfg);

                cfg.CreateMap<PermissionEnt, PermissionModel>().ReverseMap();
                cfg.CreateMap<PermissionFonctionnaliteEnt, PermissionFonctionnaliteModel>().ReverseMap();
                cfg.CreateMap<PermissionEnt, PermissionForTagHelperModel>().ReverseMap();
                cfg.CreateMap<HabilitationEnt, HabilitationForTagHelperModel>().ReverseMap();

                cfg.CreateMap<RoleFonctionnaliteEnt, RoleFonctionnaliteModel>().AfterMap((src, dest) =>
                    {
                        dest.ModuleCode = src?.Fonctionnalite?.Module?.Code;
                        dest.ModuleLibelle = src?.Fonctionnalite?.Module?.Libelle;
                        dest.ModuleDescription = src?.Fonctionnalite?.Module?.Description;
                    }).ReverseMap();

                cfg.CreateMap<FacturationEnt, FacturationModel>().ReverseMap();
                cfg.CreateMap<FacturationTypeEnt, FacturationTypeModel>().ReverseMap();
                cfg.CreateMap<DepenseTypeEnt, DepenseTypeModel>().ReverseMap();

                ObjectifFlashMapper.Map(cfg);
                RepartitionEcartMapper.Map(cfg);

                cfg.CreateMap<Axe, AxeModel>().ReverseMap();
                cfg.CreateMap<ExplorateurAxe, ExplorateurAxeModel>().ReverseMap();
                cfg.CreateMap<ExplorateurDepenseGeneriqueModel, ExplorateurDepenseModel>().ReverseMap();
                cfg.CreateMap<SearchExplorateurDepense, SearchExplorateurDepenseModel>().ReverseMap();
                cfg.CreateMap<ExplorateurDepenseResult, ExplorateurDepenseResultModel>().ReverseMap();

                cfg.CreateMap<TableauReceptionResult, TableauReceptionResultModel>().ReverseMap();

                cfg.CreateMap<FamilleOperationDiverseEnt, FamilleOperationDiverseModel>().ReverseMap();
                cfg.CreateMap<FamilleOperationDiverseEnt, FamilleOperationDiverseOrderModel>().ReverseMap();

                cfg.CreateMap<OperationDiverseEnt, OperationDiverseAbonnementModel>().ReverseMap();

                cfg.CreateMap<FeatureFlippingEnt, FeatureFlippingModel>().ReverseMap();
                cfg.CreateMap<FeatureFlippingEnt, FeatureFlippingForTagHelperModel>().ReverseMap();

                RemplacementTacheMapper.Map(cfg);

                cfg.CreateMap<RapportHebdoSyntheseMensuelleEnt, RapportHebdoSyntheseMensuelleModel>().ReverseMap();

                SocieteMapper.Map(cfg);
                MoyenMapper.Map(cfg);



                cfg.CreateMap<IndemniteDeplacementCalculTypeEnt, IndemniteDeplacementCalculTypeModel>();
                cfg.CreateMap<SearchRapportListWithFilterResult, SearchRapportListWithFilterResultModel>().ReverseMap();
                cfg.CreateMap<PieceJointeEnt, PieceJointeModel>().ReverseMap();
                cfg.CreateMap<RapportHebdoNewPointageStatutEnt, RapportHebdoNewPointageStatutModel>().ReverseMap();
                cfg.CreateMap<PieceJointeCommandeEnt, PieceJointeCommandeModel>().ReverseMap();
                cfg.CreateMap<PieceJointeReceptionEnt, PieceJointeReceptionModel>().ReverseMap();
                cfg.CreateMap<AgenceEnt, AgenceModel>().ReverseMap();
                cfg.CreateMap<AdresseEnt, AdresseModel>().ReverseMap();
                cfg.CreateMap<AvisEnt, AvisModel>().ReverseMap();
                cfg.CreateMap<AvisCommandeEnt, AvisCommandeModel>().ReverseMap();
                cfg.CreateMap<AvisCommandeAvenantEnt, AvisCommandeAvenantModel>().ReverseMap();

                // Mapping des modèles des templates
                cfg.CreateMap<AvisEnt, AvisTemplateModel>().ReverseMap();

                cfg.CreateMap<BaremeExploitationOrganisationEnt, SocieteExportModel>().ReverseMap();
                cfg.CreateMap<BaremeExploitationOrganisationEnt, PuoExportModel>().ReverseMap();
                cfg.CreateMap<BaremeExploitationOrganisationEnt, UoExportModel>().ReverseMap();
                cfg.CreateMap<BaremeExploitationOrganisationEnt, EtablissementExportModel>().ReverseMap();
            });

            return config.CreateMapper();
        }
    }
}
