using AutoMapper;
using Fred.Business.Models.RepartitionEcart;
using Fred.Entities.ReferentielEtendu;
using Fred.Entities.RepartitionEcart;
using Fred.Web.Models.RepartitionEcart;
using Fred.Web.Shared.Models.OperationDiverse;

namespace Bootstrapper.AutoMapper
{

    /// <summary>
    /// Mapper pour les repartions d'ecarts
    /// </summary>
    public static class RepartitionEcartMapper
    {

        /// <summary>
        /// Map
        /// </summary>
        /// <param name="cfg">cfg</param>
        public static void Map(IMapperConfiguration cfg)
        {
            cfg.CreateMap<RepartitionEcartEnt, RepartitionEcartModel>().ReverseMap();
            cfg.CreateMap<RepartitionEcartWrapper, RepartitionEcartWrapperModel>().ReverseMap();

            cfg.CreateMap<ReferentielEtenduEnt, ReferentielEtenduForOdModel>().AfterMap((src, dest) =>
            {
                dest.ReferentielEtenduId = src.ReferentielEtenduId;
                dest.CodeRef = src?.Ressource.Code;
                dest.LibelleRef = src?.Ressource.Libelle;
                dest.ChapitreId = src?.Ressource?.SousChapitre.ChapitreId ?? 0;
            }).ReverseMap();
        }
    }
}
