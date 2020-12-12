using System.Collections.Generic;
using System.Linq;
using Fred.Entities.CI;
using Fred.Entities.Personnel;
using Fred.Entities.Referential;
using Fred.Entities.Societe;
using Fred.ImportExport.Business.CI.AnaelSystem.Context;
using Fred.ImportExport.Business.CI.AnaelSystem.Context.Common;
using Fred.ImportExport.Models.Ci;

namespace Fred.ImportExport.Business.CI.AnaelSystem.Converter
{
    /// <summary>
    /// Convertisseur d'une entité fred en model SAP
    /// </summary>
    public class CiEntToSapModelConverter
    {

        /// <summary>
        /// Convertie un CIEnt en un CiStormModel
        /// </summary>
        /// <typeparam name="T">Parametre d'entrée</typeparam>
        /// <param name="context">context</param>
        /// <param name="societeContext">Le context de la societe</param>
        /// <returns>Une liste de CiStormModel</returns>
        public List<CiStormModel> ConvertCIEntToCiSapModels<T>(ImportCiContext<T> context, ImportCiSocieteContext societeContext) where T : class
        {
            var result = new List<CiStormModel>();

            foreach (var fredCi in societeContext.FredCis)
            {
                result.Add(ConvertCIEntToCiStormModel(context, societeContext, fredCi));
            }

            return result;
        }

        private CiStormModel ConvertCIEntToCiStormModel<T>(ImportCiContext<T> context, ImportCiSocieteContext societeContext, CIEnt fredCi) where T : class
        {
            var responsableChantier = fredCi.ResponsableChantierId == null ? null : context.Responsables.FirstOrDefault(x => x.PersonnelId == fredCi.ResponsableChantierId);

            var codeSocieteResponsableChantier = GetSocieteOf(context.SocietesOfResponsables, responsableChantier)?.CodeSocieteComptable;

            var responsableAdministratif = fredCi.ResponsableAdministratifId == null ? null : context.Responsables.FirstOrDefault(x => x.PersonnelId == fredCi.ResponsableAdministratifId);

            string codeEtablissement = GetEtablissementComptable(societeContext.EtablissementComptables, fredCi)?.Code;

            var codeSocieteResponsableAdministratif = GetSocieteOf(context.SocietesOfResponsables, responsableAdministratif)?.CodeSocieteComptable;

            var codePays = context.CiPays.FirstOrDefault(x => x.PaysId == fredCi.PaysId)?.Code;

            var adresse = fredCi.Adresse + " " + fredCi.Adresse2 + " " + fredCi.Adresse3;

            return new CiStormModel
            {
                CodeAffaire = fredCi.Code,
                Libelle = fredCi.Libelle,
                CodeEtablissement = codeEtablissement,
                DateOuverture = fredCi.DateOuverture,
                DateFermeture = fredCi.DateFermeture,
                CodeSociete = societeContext.Societe.CodeSocieteComptable,
                LibelleLong = fredCi.Libelle,
                CodeSocieteRespChantier = codeSocieteResponsableChantier,
                RespChantier = responsableChantier?.Matricule,
                CodeSocieteRespAdmin = codeSocieteResponsableAdministratif,
                RespAdmin = responsableAdministratif?.Matricule,
                Adresse = adresse.Trim(),
                Ville = fredCi.Ville,
                CodePostal = fredCi.CodePostal,
                CodePays = codePays
            };
        }

        private SocieteEnt GetSocieteOf(List<SocieteEnt> societesOfResponsables, PersonnelEnt responsableChantier)
        {
            if (responsableChantier == null)
            {
                return null;
            }
            return societesOfResponsables.FirstOrDefault(x => x.SocieteId == responsableChantier.SocieteId);
        }

        private EtablissementComptableEnt GetEtablissementComptable(List<EtablissementComptableEnt> etablissementComptables, CIEnt fredCi)
        {
            return etablissementComptables.FirstOrDefault(x => x.EtablissementComptableId == fredCi.EtablissementComptableId);
        }
    }
}
