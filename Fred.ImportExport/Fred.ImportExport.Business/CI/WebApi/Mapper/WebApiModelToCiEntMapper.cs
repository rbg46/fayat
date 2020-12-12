using System.Collections.Generic;
using Fred.Entities.CI;
using Fred.Entities.Personnel;
using Fred.Entities.Referential;
using Fred.Entities.Societe;
using Fred.ImportExport.Business.CI.WebApi.Context.Models;
using Fred.ImportExport.Models.Ci;

namespace Fred.ImportExport.Business.CI.WebApi.Mapper
{
    /// <summary>
    /// Effectue le mapping entre des ci web api et cient
    /// </summary>
    public class WebApiModelToCiEntMapper
    {
        /// <summary>
        /// Effectue le mapping 
        /// </summary>
        /// <param name="context">context</param>
        /// <returns>des ents</returns>
        public List<CIEnt> ConvertWebApiModelToCiEnts(ImportCiByWebApiSocieteContext context)
        {
            var result = new List<CIEnt>();

            foreach (var webApiCi in context.CisToImport)
            {
                var etablissementComptable = GetEtablissementComptable(context.EtablissementComptables, webApiCi);

                var ciType = GetCiType(context.CiTypes, webApiCi);

                var responsableAdministratif = GetResponsableAdministratif(context.ResponsableAffairesUsedInJson, context.SocietesOfResponsableAffairesUsedInJson, webApiCi);

                result.Add(ConvertWebApiModelToCiEnt(context.Societe?.SocieteId, etablissementComptable, responsableAdministratif, ciType, webApiCi));
            }

            return result;
        }

        private CIEnt ConvertWebApiModelToCiEnt(int? societeId, EtablissementComptableEnt etablissementComptable, PersonnelEnt responsableAdministratif, CITypeEnt ciType, WebApiCiModel webApiCi)
        {
            return new CIEnt
            {
                Description = webApiCi.Description,
                SocieteId = societeId,
                EtablissementComptableId = etablissementComptable?.EtablissementComptableId,
                Libelle = webApiCi.Libelle,
                Adresse = webApiCi.Adresse,
                CodePostal = webApiCi.CodePostal,
                Ville = webApiCi.Ville,
                AdresseFacturation = webApiCi.Adresse,
                CodePostalFacturation = webApiCi.CodePostal,
                VilleFacturation = webApiCi.Ville,
                AdresseLivraison = webApiCi.Adresse,
                CodePostalLivraison = webApiCi.CodePostal,
                VilleLivraison = webApiCi.Ville,
                DateOuverture = webApiCi.DateOuverture,
                DateFermeture = webApiCi.DateFermeture,
                Code = webApiCi.Code,
                CodeExterne = webApiCi.CodeExterne,
                ChantierFRED = true,
                ResponsableAdministratifId = responsableAdministratif?.PersonnelId,
                CITypeId = ciType?.CITypeId
            };
        }

        private EtablissementComptableEnt GetEtablissementComptable(List<EtablissementComptableEnt> etablissementComptables, WebApiCiModel webApiCi)
        {
            return etablissementComptables.Find(x => x.Code?.Trim() == webApiCi.CodeEtablissementComptable?.Trim());
        }

        private CITypeEnt GetCiType(List<CITypeEnt> ciTypes, WebApiCiModel webApiCi)
        {
            return ciTypes.Find(x => x.Code == webApiCi.CiType);
        }

        private PersonnelEnt GetResponsableAdministratif(List<PersonnelEnt> responsableAffairesUsedInJson, List<SocieteEnt> societesOfResponsableAffairesUsedInJson, WebApiCiModel webApiCi)
        {
            PersonnelEnt result = null;
            if (!string.IsNullOrEmpty(webApiCi.MatriculeResponsableAffaire) && !string.IsNullOrEmpty(webApiCi.SocieteCodeResponsableAffaire))
            {
                var societeResponsableCi = societesOfResponsableAffairesUsedInJson.Find(x => x.CodeSocieteComptable == webApiCi.SocieteCodeResponsableAffaire && x.Active);
                if (societeResponsableCi != null)
                {
                    result = responsableAffairesUsedInJson.Find(x => x.SocieteId == societeResponsableCi.SocieteId && x.Matricule == webApiCi.MatriculeResponsableAffaire);
                }
            }
            return result;
        }
    }
}
