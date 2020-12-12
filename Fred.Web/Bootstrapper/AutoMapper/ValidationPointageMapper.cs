using AutoMapper;
using Fred.Entities;
using Fred.Entities.ValidationPointage;
using Fred.Framework.Extensions;
using Fred.Web.Shared.Models;
using System.Linq;

namespace Bootstrapper.AutoMapper
{
  public static class ValidationPointageMapper
  {
    public static void Map(IMapperConfiguration config)
    {
      config.CreateMap<LotPointageEnt, LotPointageModel>().AfterMap((src, dest) =>
      {
        if (dest.ControlePointages != null && dest.ControlePointages.Any())
        {
          dest.ControleChantier = dest.ControlePointages.FirstOrDefault(x => x.TypeControle == TypeControlePointage.ControleChantier.ToIntValue());
          dest.ControleVrac = dest.ControlePointages.FirstOrDefault(x => x.TypeControle == TypeControlePointage.ControleVrac.ToIntValue());
          dest.ControlePointages = null;
        }
        if (src.RapportLignes != null && src.RapportLignes.Any())
        {
          dest.NombrePointages = src.RapportLignes.Count;
        }
      });

      config.CreateMap<ControlePointageEnt, ControlePointageModel>().ReverseMap();
      config.CreateMap<ControlePointageErreurEnt, ControlePointageErreurModel>().ReverseMap();
      config.CreateMap<PointageFiltre, PointageFiltreModel>().ReverseMap();
      config.CreateMap<RemonteeVracEnt, RemonteeVracModel>().ReverseMap();
      config.CreateMap<RemonteeVracErreurEnt, RemonteeVracErreurModel>().ReverseMap();
      config.CreateMap<PersonnelErreur<ControlePointageErreurEnt>, PersonnelErreurModel<ControlePointageErreurModel>>().ReverseMap();
      config.CreateMap<PersonnelErreur<RemonteeVracErreurEnt>, PersonnelErreurModel<RemonteeVracErreurModel>>().ReverseMap();
      config.CreateMap<SearchValidationResult<ControlePointageErreurEnt>, SearchValidationResultModel<ControlePointageErreurModel>>().ReverseMap();
      config.CreateMap<SearchValidationResult<RemonteeVracErreurEnt>, SearchValidationResultModel<RemonteeVracErreurModel>>().ReverseMap();

    }

  }
}