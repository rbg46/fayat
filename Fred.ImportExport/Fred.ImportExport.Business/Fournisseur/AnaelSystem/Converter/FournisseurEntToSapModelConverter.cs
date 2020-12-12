using System.Collections.Generic;
using AutoMapper;
using Fred.ImportExport.Business.Fournisseur.AnaelSystem.Context;
using Fred.ImportExport.Business.Fournisseur.AnaelSystem.Context.Common;
using Fred.ImportExport.Models;

namespace Fred.ImportExport.Business.Fournisseur.AnaelSystem.Converter
{
    /// <summary>
    /// Convertisseur d'une entité fred en model SAP
    /// </summary>
    public class FournisseurEntToSapModelConverter
    {
        private readonly IMapper mapper;

        public FournisseurEntToSapModelConverter(IMapper mapper)
        {
            this.mapper = mapper;
        }

        /// <summary>
        /// Convertie un CIEnt en un CiStormModel
        /// </summary>
        /// <param name="societeContext">Le context de la societe</param>
        /// <returns>Une liste de CiStormModel</returns>
        public List<FournisseurStormModel> ConvertFournisseurEntToFournisseurSapModels(ImportFournisseurSocieteContext societeContext)
        {
            var result = mapper.Map<List<FournisseurStormModel>>(societeContext.AnaelFournisseurs);

            return result;
        }
    }
}
