using System.Collections.Generic;
using System.Linq;
using Fred.Entities.CI;
using Fred.Entities.Referential;
using Fred.ImportExport.Models.Ci;
namespace Fred.ImportExport.Business.CI.AnaelSystem.Converter
{
    /// <summary>
    /// Convertisseur d'un model Anael en entité fred
    /// </summary>
    public class AnaelModelToCiEntConverter
    {
        private readonly string codeChantierFRED = "01";

        private readonly CiImportExportLogger logger;

        public AnaelModelToCiEntConverter(CiImportExportLogger logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Convertie une liste de model Anael en entité fred
        /// </summary>
        /// <param name="societeId">societeId</param>
        /// <param name="etablissementComptables">etablissementComptables de la societes</param>
        /// <param name="ciAnaelModels">Lmodel a convertir</param>
        /// <returns>Liste d'entites fred</returns>
        public List<CIEnt> ConvertAnaelModelToCiEnts(int societeId, List<EtablissementComptableEnt> etablissementComptables, List<CiAnaelModel> ciAnaelModels)
        {
            var result = new List<CIEnt>();

            foreach (var ciAnaelModel in ciAnaelModels)
            {
                result.Add(ConvertAnaelModelToCiEnt(societeId, etablissementComptables, ciAnaelModel));
            }

            return result;
        }


        private CIEnt ConvertAnaelModelToCiEnt(int societeId, List<EtablissementComptableEnt> etablissementComptables, CiAnaelModel ciAnaelModel)
        {
            var etablissementComptable = GetEtablissementComptable(etablissementComptables, ciAnaelModel);

            //Le fait de ne pas trouver l'etablissement comptable ne ploque pas l'import
            logger.WarnNoEtablissementComptableIfNecessary(etablissementComptable, ciAnaelModel);

            return new CIEnt
            {
                Code = ciAnaelModel.CodeAffaire,
                Libelle = string.IsNullOrEmpty(ciAnaelModel.LibelleLong) ? ciAnaelModel.Libelle : ciAnaelModel.LibelleLong,
                EtablissementComptableId = etablissementComptable?.EtablissementComptableId,
                Sep = false,
                DateOuverture = ciAnaelModel.DateOuverture,
                DateFermeture = ciAnaelModel.DateFermeture,
                ChantierFRED = ciAnaelModel.ChantierFRED == codeChantierFRED,
                SocieteId = societeId
            };
        }

        /// <summary>
        /// Recherche l'établissement comptable du ci Anael dans FRED
        /// </summary>
        /// <param name="etablissementComptables">listes des établissements comptables possibles</param>
        /// <param name="ciAnaelModel">CI anael</param>
        /// <returns>retourne l'établissement comptable</returns>
        /// <remarks>retourne l'établissement comptable paramétré dans FRED si pas trouvé</remarks>
        private EtablissementComptableEnt GetEtablissementComptable(List<EtablissementComptableEnt> etablissementComptables, CiAnaelModel ciAnaelModel)
        {
            return etablissementComptables.FirstOrDefault(x => x.Code?.Trim() == ciAnaelModel.CodeEtablissement?.Trim())
                ?? etablissementComptables.FirstOrDefault(x => x.DateSuppression == null);
        }
    }
}
