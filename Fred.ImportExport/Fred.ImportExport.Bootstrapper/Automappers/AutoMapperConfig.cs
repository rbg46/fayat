using System;
using System.Linq;
using System.Text.RegularExpressions;
using AutoMapper;
using Fred.Entities.Adresse;
using Fred.Entities.Commande;
using Fred.Entities.Depense;
using Fred.Entities.Facturation;
using Fred.Entities.Groupe;
using Fred.Entities.Models;
using Fred.Entities.Models.Flux.Depense;
using Fred.Entities.Rapport;
using Fred.Entities.Referential;
using Fred.Entities.Societe;
using Fred.Framework.Models;
using Fred.ImportExport.Business.Moyen.ExportPointageMoyenEtl.Common;
using Fred.ImportExport.DataAccess.ExternalService.Tibco.Moyen;
using Fred.ImportExport.Models;
using Fred.ImportExport.Models.Ci;
using Fred.ImportExport.Models.Commande;
using Fred.ImportExport.Models.Depense;
using Fred.ImportExport.Models.Facturation;
using Fred.ImportExport.Models.Groupe;
using Fred.ImportExport.Models.Materiel;
using Fred.ImportExport.Models.Societe;

namespace Fred.ImportExport.Bootstrapper.Automappers
{
    public static class AutoMapperConfig
    {
#pragma warning disable S3776 // Cognitive Complexity of methods should not be too high
        public static IMapper CreateMapper()
#pragma warning restore S3776 // Cognitive Complexity of methods should not be too high
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<GroupeEnt, GroupeInterimaireModel>()
                    .ForMember(model => model.GroupeId, option => option.MapFrom(x => x.GroupeId))
                    .ForMember(model => model.Libelle, option => option.MapFrom(x => x.Libelle))
                    .ForMember(model => model.SocieteNotInterimaires, option => option.MapFrom(x => x.Societes));

                cfg.CreateMap<SocieteEnt, SocieteNotInterimaireModel>()
                    .ForMember(model => model.SocieteId, option => option.MapFrom(x => x.SocieteId))
                    .ForMember(model => model.Libelle, option => option.MapFrom(x => x.Libelle));

                //Commande Fred ==> STROM
                cfg.CreateMap<CommandeEnt, CommandeSapModel>()
                    .ForMember(model => model.CommandeTypeId, option => option.MapFrom(x => x.Type.CommandeTypeId))
                    .ForMember(model => model.StatutCommandeCode, option => option.MapFrom(x => x.StatutCommande.Code))
                    .ForMember(model => model.ContactPersonnel, option => option.MapFrom(x => $"{x.Contact.Prenom} {x.Contact.Nom}"))
                    .ForMember(model => model.SuiviPersonnel, option => option.MapFrom(x => $"{x.Suivi.Prenom} {x.Suivi.Nom}"))
                    .ForMember(model => model.CiCode, option => option.MapFrom(x => x.CI.Code))
                    .ForMember(model => model.EtablissementComptableCode, option => option.MapFrom(x => x.CI.EtablissementComptable.Code))
                    .ForMember(model => model.FournisseurCode, option => option.MapFrom(x => x.Fournisseur.Code))
                    .ForMember(model => model.FournisseurTypeSequence, option => option.MapFrom(x => x.Fournisseur.TypeSequence))
                    .ForMember(model => model.FournisseurPaysCode, option => option.MapFrom(x => x.FournisseurPays.Code))
                    .ForMember(model => model.FournisseurAdresse, option => option.MapFrom(x => x.Fournisseur.Adresse))
                    .ForMember(model => model.FournisseurCPostal, option => option.MapFrom(x => x.Fournisseur.CodePostal))
                    .ForMember(model => model.FournisseurVille, option => option.MapFrom(x => x.Fournisseur.Ville))
                    .ForMember(model => model.LivraisonPaysCode, option => option.MapFrom(x => x.LivraisonPays.Code))
                    .ForMember(model => model.FacturationPaysCode, option => option.MapFrom(x => x.FacturationPays.Code))
                    .ForMember(model => model.ValideurUtilisateur, option => option.MapFrom(x => $"{x.Valideur.Prenom} {x.Valideur.Nom}"))
                    .ForMember(model => model.DeviseIsoCode, option => option.MapFrom(x => x.Devise.IsoCode))
                    .ForMember(model => model.AuteurCreation, option => option.MapFrom(x => $"{x.AuteurCreation.Prenom} {x.AuteurCreation.Nom}"))
                    .ForMember(model => model.AuteurModification, option => option.MapFrom(x => $"{x.AuteurModification.Prenom} {x.AuteurModification.Nom}"))
                    .ForMember(model => model.NumeroCommandeExterne, option => option.MapFrom(x => x.NumeroCommandeExterne != null ? Regex.Replace(x.NumeroCommandeExterne, "[^a-zA-Z0-9///]", string.Empty).Trim().ToUpper() : x.NumeroCommandeExterne))
                    .ForMember(model => model.NumeroContratExterne, option => option.MapFrom(x => x.NumeroContratExterne != null ? Regex.Replace(x.NumeroContratExterne, "[^a-zA-Z0-9///]", string.Empty).Trim().ToUpper() : x.NumeroContratExterne))
                    .ForMember(model => model.AgenceCode, option => option.MapFrom(x => x.Agence.Code))
                    .ForMember(model => model.AgenceAdresse, option => option.MapFrom(x => x.Agence.Adresse.Ligne))
                    .ForMember(model => model.AgenceCPostal, option => option.MapFrom(x => x.Agence.Adresse.CodePostal))
                    .ForMember(model => model.AgenceVille, option => option.MapFrom(x => x.Agence.Adresse.Ville))
                    .ForMember(model => model.AgencePaysCode, option => option.MapFrom(x => x.Agence.Adresse.Pays.Code));

                //Commande STROM ==> Fred
                cfg.CreateMap<CommandeSapModel, CommandeEnt>()
                    .ForMember(model => model.AuteurCreation, option => option.NullSubstitute(null));

                //Commande Fred <==> STROM
                cfg.CreateMap<CommandeLigneEnt, CommandeLigneSapModel>()
                    .ForMember(model => model.UniteCode, option => option.MapFrom(x => x.Unite.Code))
                    .ForMember(model => model.RessourceCode, option => option.MapFrom(x => x.Ressource.Code))
                    .ForMember(model => model.RessourceLibelle, option => option.MapFrom(x => x.Ressource.Libelle))
                    .ForMember(model => model.TacheCode, option => option.MapFrom(x => x.Tache.Code))
                    .ForMember(model => model.TacheLibelle, option => option.MapFrom(x => x.Tache.Libelle))
                    .ForMember(model => model.NatureCode, option => option.Ignore())
                    .ReverseMap();

                //Depense Fred ==> STROM
                cfg.CreateMap<DepenseAchatEnt, ReceptionSapModel>()
                .ForMember(model => model.ReceptionId, option => option.MapFrom(x => x.DepenseId))
                .ForMember(model => model.RessourceCode, option => option.MapFrom(x => x.Ressource.Code))
                .ForMember(model => model.RessourceLibelle, option => option.MapFrom(x => x.Ressource.Libelle))
                .ForMember(model => model.TacheCode, option => option.MapFrom(x => x.Tache.Code))
                .ForMember(model => model.TacheLibelle, option => option.MapFrom(x => x.Tache.Libelle))
                .ForMember(model => model.AuteurCreation, option => option.MapFrom(x => $"{x.AuteurCreation.Prenom} {x.AuteurCreation.Nom}"))
                .ForMember(model => model.AuteurModification, option => option.MapFrom(x => $"{x.AuteurModification.Prenom} {x.AuteurModification.Nom}"))
                .ForMember(model => model.DateReception, option => option.MapFrom(x => x.Date))
                //BUG 9714 : si numero de ligne externe alors c'est une commande externe et donc on utilise le numero externe de la commande
                .ForMember(model => model.Numero, option => option.MapFrom(
                    x => !string.IsNullOrEmpty(x.CommandeLigne.NumeroCommandeLigneExterne)
                        ? x.CommandeLigne.Commande.NumeroCommandeExterne
                        : x.CommandeLigne.Commande.Numero))
                .ForMember(model => model.MatriculeSap, option => option.MapFrom(x => x.CommandeLigne.Commande.CommandeContratInterimaire.Interimaire.MatriculeExterne.FirstOrDefault(m => m.Source == "SAP") == null ? string.Empty : x.CommandeLigne.Commande.CommandeContratInterimaire.Interimaire.MatriculeExterne.FirstOrDefault(m => m.Source == "SAP").Matricule))
                .ForMember(model => model.MatriculePixid, option => option.MapFrom(x => x.CommandeLigne.Commande.CommandeContratInterimaire.Interimaire.MatriculeExterne.FirstOrDefault(m => m.Source == "PIXID") == null ? string.Empty : x.CommandeLigne.Commande.CommandeContratInterimaire.Interimaire.MatriculeExterne.FirstOrDefault(m => m.Source == "PIXID").Matricule))
                .ForMember(model => model.MatriculeDirectSkills, option => option.MapFrom(x => x.CommandeLigne.Commande.CommandeContratInterimaire.Interimaire.MatriculeExterne.FirstOrDefault(m => m.Source == "DIRECTSKILLS") == null ? string.Empty : x.CommandeLigne.Commande.CommandeContratInterimaire.Interimaire.MatriculeExterne.FirstOrDefault(m => m.Source == "DIRECTSKILLS").Matricule))
                .ForMember(model => model.CommandeLigneSap, options => options.MapFrom(x => x.CommandeLigne.NumeroCommandeLigneExterne));

                //Facturation STROM ==> Fred
                cfg.CreateMap<FacturationSapModel, FacturationEnt>()
                    .ForMember(x => x.DepenseAchatReceptionId, option => option.MapFrom(x => x.ReceptionId.HasValue && x.ReceptionId > 0 ? x.ReceptionId.Value : default(int?)))
                    .ForMember(x => x.DateCreation, option => option.UseValue(DateTime.UtcNow))
                    .ForMember(x => x.DatePieceSap, option => option.MapFrom(x => x.DateFacture ?? DateTime.UtcNow))
                    .ForMember(x => x.AuteurCreation, option => option.MapFrom(x => x.AuteurSap))
                    .ForMember(x => x.QuantiteFar, option => option.MapFrom(x => x.FarQuantite))
                    .ForMember(x => x.LitigeCode, option => option.MapFrom(x => x.CodeLitige));

                //Materiel STROM ==> Fred
                cfg.CreateMap<MaterielStormModel, MaterielEnt>();

                //Tâche des pointage materiel STROM ==> Fred
                cfg.CreateMap<RapportLigneTacheEnt, PointageTacheStormModel>();

                //Pointage materiel STROM ==> Fred
                cfg.CreateMap<RapportLigneEnt, PointageMaterielStormModel>()
                    .ForMember(x => x.CiCode, option => option.MapFrom(x => x.Ci.Code))
                    .ForMember(x => x.AuteurSocieteCode, option => option.MapFrom(x => x.AuteurCreation.Personnel.Societe.Code))
                    .ForMember(x => x.AuteurMatricule, option => option.MapFrom(x => x.AuteurCreation.Personnel.Matricule))
                    .ForMember(x => x.MaterielSocieteCode, option => option.MapFrom(x => x.Materiel.Societe.Code))
                    .ForMember(x => x.PersonnelMatricule, option => option.MapFrom(x => x.Personnel.Matricule))
                    .ForMember(x => x.CodeDeplacementCode, option => option.MapFrom(x => x.CodeDeplacement.Code))
                    .ForMember(x => x.Taches, option => option.MapFrom(x => x.ListRapportLigneTaches));

                cfg.CreateMap<PointageMaterielModel, PointageMaterielStormModel>();

                //Founisseur ANAEL => STORM
                cfg.CreateMap<FournisseurFredModel, FournisseurStormModel>()
                    .ForMember(model => model.ProfessionLiberale, option => option.MapFrom(x => x.IsProfessionLiberale));

                //Fournisseur entite => STORM
                cfg.CreateMap<FournisseurEnt, FournisseurStormModel>()
                    .ForMember(model => model.ProfessionLiberale, option => option.MapFrom(x => x.IsProfessionLiberale));

                //CI ANAEL => STROM
                cfg.CreateMap<CiModel, CiStormModel>();

                //Avenant Fred ==> STROM
                cfg.CreateMap<CommandeEnt, CommandeAvenantSapModel>()
                    .ForMember(model => model.CommandeTypeId, option => option.MapFrom(x => x.Type.CommandeTypeId))
                    .ForMember(model => model.StatutCommandeCode, option => option.MapFrom(x => x.StatutCommande.Code))
                    .ForMember(model => model.ContactPersonnel, option => option.MapFrom(x => $"{x.Contact.Prenom} {x.Contact.Nom}"))
                    .ForMember(model => model.SuiviPersonnel, option => option.MapFrom(x => $"{x.Suivi.Prenom} {x.Suivi.Nom}"))
                    .ForMember(model => model.CiCode, option => option.MapFrom(x => x.CI.Code))
                    .ForMember(model => model.EtablissementComptableCode, option => option.MapFrom(x => x.CI.EtablissementComptable.Code))
                    .ForMember(model => model.FournisseurCode, option => option.MapFrom(x => x.Fournisseur.Code))
                    .ForMember(model => model.FournisseurPaysCode, option => option.MapFrom(x => x.FournisseurPays.Code))
                    .ForMember(model => model.LivraisonPaysCode, option => option.MapFrom(x => x.LivraisonPays.Code))
                    .ForMember(model => model.FacturationPaysCode, option => option.MapFrom(x => x.FacturationPays.Code))
                    .ForMember(model => model.ValideurUtilisateur, option => option.MapFrom(x => $"{x.Valideur.Prenom} {x.Valideur.Nom}"))
                    .ForMember(model => model.DeviseIsoCode, option => option.MapFrom(x => x.Devise.IsoCode))
                    .ForMember(model => model.AuteurCreation, option => option.MapFrom(x => $"{x.AuteurCreation.Prenom} {x.AuteurCreation.Nom}"))
                    .ForMember(model => model.AuteurModification, option => option.MapFrom(x => $"{x.AuteurModification.Prenom} {x.AuteurModification.Nom}"))
                    .ForMember(model => model.NumeroCommandeExterne, option => option.MapFrom(x => x.NumeroCommandeExterne != null ? Regex.Replace(x.NumeroCommandeExterne, "[^a-zA-Z0-9///]", string.Empty).Trim().ToUpper() : x.NumeroCommandeExterne))
                    .ForMember(model => model.NumeroContratExterne, option => option.MapFrom(x => x.NumeroContratExterne != null ? Regex.Replace(x.NumeroContratExterne, "[^a-zA-Z0-9///]", string.Empty).Trim().ToUpper() : x.NumeroContratExterne))
                    .ForMember(model => model.AgenceCode, option => option.MapFrom(x => x.Agence.Code))
                    .ForMember(model => model.AgenceAdresse, option => option.MapFrom(x => x.Agence.Adresse.Ligne))
                    .ForMember(model => model.AgenceCPostal, option => option.MapFrom(x => x.Agence.Adresse.CodePostal))
                    .ForMember(model => model.AgenceVille, option => option.MapFrom(x => x.Agence.Adresse.Ville))
                    .ForMember(model => model.AgencePaysCode, option => option.MapFrom(x => x.Agence.Adresse.Pays.Code));


                //Ligne d'avenant Fred ==> STROM
                cfg.CreateMap<CommandeLigneEnt, CommandeLigneAvenantSapModel>()
                    .ForMember(model => model.UniteCode, option => option.MapFrom(x => x.Unite.Code))
                    .ForMember(model => model.RessourceCode, option => option.MapFrom(x => x.Ressource.Code))
                    .ForMember(model => model.RessourceLibelle, option => option.MapFrom(x => x.Ressource.Libelle))
                    .ForMember(model => model.TacheCode, option => option.MapFrom(x => x.Tache.Code))
                    .ForMember(model => model.TacheLibelle, option => option.MapFrom(x => x.Tache.Libelle))
                    .ForMember(model => model.NatureCode, option => option.Ignore())
                    .ForMember(model => model.AvenantNumero, option => option.MapFrom(x => x.AvenantLigne.Avenant.NumeroAvenant))
                    .ForMember(model => model.Diminution, option => option.MapFrom(x => x.AvenantLigne.IsDiminution));

                cfg.CreateMap<Result<string>, ResultModel<string>>().ReverseMap();
                cfg.CreateMap<Result<DepenseFluxResponseModel>, ResultModel<DepenseFluxResponseModel>>().ReverseMap();

                //Fournisseur Tous les sociétés ==> Fred
                cfg.CreateMap<FournisseurModel, FournisseurFredModel>();
                cfg.CreateMap<FournisseurFredModel, FournisseurEnt>();

                // Export des pointages des moyens
                cfg.CreateMap<EnvoiPointageMoyenResultModel, EnvoiPointageMoyenResult>().ReverseMap();

                cfg.CreateMap<ImportFournisseurModel, FournisseurEnt>();

                cfg.CreateMap<ImportFournisseurModel, AgenceEnt>()
                    .ForMember(model => model.Adresse, option => option.MapFrom(x => new AdresseEnt()
                    {
                        Ligne = x.Adresse,
                        CodePostal = x.CodePostal,
                        Ville = x.Ville,
                        PaysId = x.PaysId
                    }));

                cfg.CreateMap<ImportAgenceModel, AgenceEnt>()
                    .ForMember(model => model.Adresse, option => option.MapFrom(x => new AdresseEnt()
                    {
                        Ligne = x.Adresse,
                        CodePostal = x.CodePostal,
                        Ville = x.Ville,
                        PaysId = x.PaysId
                    }));
            });

            return config.CreateMapper();
        }
    }
}
