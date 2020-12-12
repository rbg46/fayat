using System.Collections.Generic;
using AutoMapper;
using Fred.Entities.Referential;
using Fred.ImportExport.Models;

namespace Fred.ImportExport.Business.Fournisseur.AnaelSystem.Converter
{
    /// <summary>
    /// Convertisseur d'un model Anael en entité fred
    /// </summary>
    public class FournisseurFredModelToFournisseurEntConverter
    {
        private readonly IMapper mapper;

        public FournisseurFredModelToFournisseurEntConverter(IMapper mapper)
        {
            this.mapper = mapper;
        }

        /// <summary>
        /// Convertie une liste de model Anael en entité fred
        /// </summary>
        /// <param name="groupeId">le groupe id</param>
        /// <param name="fournisseurFredModels">Lmodel a convertir</param>
        /// <returns>Liste d'entites fred</returns>
        public List<FournisseurEnt> ConvertFournisseurFredModelToFournisseurEnts(int groupeId, List<FournisseurFredModel> fournisseurFredModels)
        {
            var result = mapper.Map<List<FournisseurEnt>>(fournisseurFredModels);

            //Ajout du groupe en dur
            result.ForEach(x => x.GroupeId = groupeId);

            return result;
        }
    }
}
