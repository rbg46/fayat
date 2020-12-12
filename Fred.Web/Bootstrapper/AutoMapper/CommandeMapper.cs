using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Fred.Business.CommandeEnergies;
using Fred.Business.SeuilValidation.Models;
using Fred.Entities;
using Fred.Entities.CI;
using Fred.Entities.Commande;
using Fred.Entities.Import;
using Fred.Entities.Referential;
using Fred.Entities.Utilisateur;
using Fred.Framework.Extensions;
using Fred.Web.Models;
using Fred.Web.Models.Commande;
using Fred.Web.Shared.Models;
using Fred.Web.Shared.Models.Commande;
using Fred.Web.Shared.Models.Commande.Light;
using Fred.Web.Shared.Models.Commande.List;
using Fred.Web.Shared.Models.Import;

namespace Bootstrapper.AutoMapper
{
    public static class CommandeMapper
    {
        public static void Map(IMapperConfiguration cfg)
        {
            List<int> displayedFacturationTypeCodes = new List<int>
            {
                FacturationType.Facturation.ToIntValue(),
                FacturationType.CoutAdditionnel.ToIntValue(),
                FacturationType.FacturationMontant.ToIntValue(),
                FacturationType.AvoirQuantite.ToIntValue(),
                FacturationType.AvoirMontant.ToIntValue()
            };

            cfg.CreateMap<CommandeEnt, CommandeModel>().ReverseMap();

            // le mapping des facturations comporte une spécificité            
            cfg.CreateMap<CommandeEnt, CommandeLightModel>().AfterMap((commandeEnt, commandeLightModel) =>
            {
                // Certains FacturationType ne doivent pas apparaitre au front (cf: ChargementProvisionFar, DechargementProvisionFar, etc..)
                commandeLightModel.Lignes?.ForEach(x =>
                    x.DepensesReception?.ForEach(y =>
                        y.FacturationsReception = y.FacturationsReception?.Where(z => displayedFacturationTypeCodes.Contains(z.FacturationType.Code)).ToList()));

                // Mapping du champ OldFournisseurCodeLibelle 
                commandeLightModel.OldFournisseurCodeLibelle = string.Concat(commandeEnt.OldFournisseur?.Code, " - ", commandeEnt.OldFournisseur?.Libelle);
            }
            ).ReverseMap();

            cfg.CreateMap<CommandeTypeEnt, CommandeTypeModel>().ReverseMap();
            cfg.CreateMap<CommandeLigneEnt, CommandeLigneModel>().ReverseMap();
            ////cfg.CreateMap<CommandeLigneEnt, CommandeLigneLightModel>().ForMember(model => model.Commande, option => option.Ignore()).ReverseMap();

            cfg.CreateMap<CommandeLigneEnt, CommandeLigneLightModel>().ReverseMap();
            cfg.CreateMap<CommandeEnt, CommandeListModel>();
            cfg.CreateMap<CIEnt, CIForCommandeListModel>();
            cfg.CreateMap<DeviseEnt, DeviseForCommandeListModel>();
            cfg.CreateMap<FournisseurEnt, FournisseurForCommandeListModel>();
            cfg.CreateMap<StatutCommandeEnt, StatutCommandeForCommandeListModel>();
            cfg.CreateMap<UtilisateurEnt, UtilisateurForCommandeListModel>();
            cfg.CreateMap<SystemeExterneEnt, SystemeExterneLightModel>().ReverseMap();

            // Avenant
            cfg.CreateMap<CommandeLigneAvenantEnt, CommandeAvenantLoad.LigneModel>().ReverseMap();
            cfg.CreateMap<CommandeAvenantEnt, CommandeAvenantLoad.AvenantModel>().ReverseMap();

            // Commande Energie
            cfg.CreateMap<TypeEnergieEnt, TypeEnergieModel>().ReverseMap();
            cfg.CreateMap<CommandeEnergie, CommandeEnergieItemModel>().ReverseMap();

            cfg.CreateMap<CommandeEnergie, CommandeEnt>().ReverseMap();
            cfg.CreateMap<CommandeEnergieLigne, CommandeLigneEnt>().ReverseMap();

            cfg.CreateMap<CommandeEnergie, CommandeEnergieModel>().ReverseMap();
            cfg.CreateMap<CommandeEnergieLigne, CommandeEnergieLigneModel>().ReverseMap();

            cfg.CreateMap<PersonnelWithPermissionAndSeuilValidationResult, PersonnelWithPermissionAndSeuilModel>().AfterMap((src, dest) =>
            {
                dest.Email = src.Email;
                dest.HaveMinimunSeuilValidation = src.HaveMinimunSeuilValidation;
                dest.Nom = src.Nom;
                dest.PersonnelId = src.PersonnelId;
                dest.Prenom = src.Prenom;
                dest.IdRef = src.PersonnelId.ToString();
                dest.CodeRef = src.SocieteCode + " - " + src.Matricule;
                dest.LibelleRef = src.Prenom + " " + src.Nom;
            });
            cfg.CreateMap<CommandeEnt, CloturerCommandeModel>();
        }
    }
}
