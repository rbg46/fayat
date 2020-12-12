using System;
using AutoMapper;
using Fred.Business.Personnel;
using Fred.Business.Referential;
using Fred.Entities;
using Fred.Entities.Personnel;
using Fred.Web.Models.Personnel;
using Fred.Web.Shared.Models;
using Fred.Web.Shared.Models.Personnel.Light;

namespace Bootstrapper.AutoMapper
{
    public static class PersonnelMapper
    {
        public static void Map(IMapperConfiguration config)
        {
            config.CreateMap<PersonnelEnt, PersonnelModel>().AfterMap((src, dest) =>
            {
                // TSA: Mapping des type rattachement (Entité non présente en BD) 
                if (!string.IsNullOrEmpty(src.TypeRattachement))
                {
                    dest.TypeRattachementLibelle = TypeRattachementManager.GetLibelle(src.TypeRattachement);
                    dest.TypeRattachementModel = new TypeRattachementModel
                    {
                        TypeRattachementId = TypeRattachementManager.GetIdentifiant(src.TypeRattachement),
                        Code = src.TypeRattachement,
                        Libelle = dest.TypeRattachementLibelle
                    };
                }
                if (src.Pays != null)
                {
                    dest.PaysLibelle = src.Pays.Libelle;
                }
            }).AfterMap((src, dest) => dest.IsActif = src.GetPersonnelIsActive()).ReverseMap();

            config.CreateMap<SearchPersonnelsWithFiltersResult, SearchPersonnelsWithFiltersResultModel>().ReverseMap();

            config.CreateMap<PersonnelEnt, PersonnelLightModel2>().AfterMap((src, dest) =>
            {
                dest.CodeSocieteMatriculePrenomNom = src.Societe != null ? src.Societe.Code + " - " + src.Matricule + " - " + src.PrenomNom : dest.MatriculePrenomNom;
                dest.SocieteId = src.SocieteId;
                dest.GroupeId = src.Societe?.GroupeId;
            });

            config.CreateMap<PersonnelEnt, PersonnelLightForPickListModel>().ReverseMap();
            config.CreateMap<PersonnelLightForPickListEnt, PersonnelLightForPickListModel>().ReverseMap();

            config.CreateMap<PersonnelImageEnt, PersonnelImageModel>().AfterMap((src, dest) =>
            {
                dest.SignatureBase64 = src.Signature?.Length > 0 ? Convert.ToBase64String(src.Signature) : string.Empty;
                dest.PhotoProfilBase64 = src.PhotoProfil?.Length > 0 ? Convert.ToBase64String(src.PhotoProfil) : string.Empty;
            });

            config.CreateMap<PersonnelImageModel, PersonnelImageEnt>().AfterMap((src, dest) =>
            {
                dest.Signature = !string.IsNullOrEmpty(src.SignatureBase64) ? Convert.FromBase64String(src.SignatureBase64) : null;
                dest.PhotoProfil = !string.IsNullOrEmpty(src.PhotoProfilBase64) ? Convert.FromBase64String(src.PhotoProfilBase64) : null;
            });
        }
    }
}
