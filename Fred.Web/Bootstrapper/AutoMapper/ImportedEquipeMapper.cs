using AutoMapper;
using Fred.Entities.Personnel;
using Fred.Web.Shared.Models.Affectation;

namespace Bootstrapper.AutoMapper
{
    /// <summary>
    /// Mapp a personnel entity to imported equipe model
    /// </summary>
    public static class ImportedEquipeMapper
    {
        #region public method

        public static void Map(IMapperConfiguration config)
        {
            config.CreateMap<PersonnelEnt, ImportedEquipeModel>().ForMember(dest => dest.CodeSociete, opts => opts.MapFrom(src => src.Societe != null ? src.Societe.Code : string.Empty));
            config.CreateMap<PersonnelEnt, ImportedEquipeModel>().AfterMap(AfterMap).ReverseMap();
        }

        #endregion

        #region private method

        /// <summary>
        /// After map method
        /// </summary>
        /// <param name="personnel">Personnel entity object</param>
        /// <param name="importedEquipe">Imported equipe model</param>
        private static void AfterMap(PersonnelEnt personnel, ImportedEquipeModel importedEquipe)
        {
            if (personnel != null)
            {
                importedEquipe.Statut = Helper.GetPersonnelStatut(personnel.Statut);
            }
        }

        #endregion
    }
}
