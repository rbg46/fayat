using System;
using System.Collections.Generic;
using Fred.Business.RepriseDonnees.Ci.Models;
using Fred.Business.RepriseDonnees.Ci.Selector;
using Fred.Business.RepriseDonnees.Common.Selector;
using Fred.Entities.CI;
using Fred.Entities.RepriseDonnees.Excel;
using Fred.Framework.Extensions;

namespace Fred.Business.RepriseDonnees.Ci.Mapper
{
    public class CiDataMapper : ICiDataMapper
    {
        private readonly IZoneModifiableSelectorService zoneModifiableSelectorService;
        private readonly IPersonnelSelectorService personnelSelectorService;
        private readonly IPaysSelectorService paysSelectorService;
        private readonly ICiSelectorService ciSelectorService;
        private readonly IFacturationEtablissementSelectorService facturationEtablissementSelectorService;

        public CiDataMapper(
            IZoneModifiableSelectorService zoneModifiableSelectorService,
            IPersonnelSelectorService personnelSelectorService,
            IPaysSelectorService paysSelectorService,
            ICiSelectorService ciSelectorService,
            IFacturationEtablissementSelectorService facturationEtablissementSelectorService)
        {
            this.zoneModifiableSelectorService = zoneModifiableSelectorService;
            this.personnelSelectorService = personnelSelectorService;
            this.paysSelectorService = paysSelectorService;
            this.ciSelectorService = ciSelectorService;
            this.facturationEtablissementSelectorService = facturationEtablissementSelectorService;
        }

        /// <summary>
        /// Affecte les nouvelles valeurs a certains champs des cis
        /// </summary>
        /// <param name="context">context contenant les data necessaire a l'import</param>
        /// <param name="repriseExcelCis">les ci sous la forme excel</param>
        /// <returns>Liste de ci avec certains champs modifiés</returns>
        public List<CIEnt> Map(ContextForImportCi context, List<RepriseExcelCi> repriseExcelCis)
        {
            var result = new List<CIEnt>();
            CommonFieldSelector commonSelector = new CommonFieldSelector();
            foreach (var repriseExcelCi in repriseExcelCis)
            {
                var fredCi = ciSelectorService.GetCiOfDatabase(repriseExcelCi, context);
                if (fredCi != null)
                {
                    var pays = paysSelectorService.GetPaysByCode(context.PaysUsedInExcel, repriseExcelCi.CodePays);
                    var paysLivraison = paysSelectorService.GetPaysByCode(context.PaysUsedInExcel, repriseExcelCi.CodePaysLivraison);
                    var paysFacturation = paysSelectorService.GetPaysByCode(context.PaysUsedInExcel, repriseExcelCi.CodePaysFacturation);
                    var zoneModifiable = zoneModifiableSelectorService.GetZoneModifiable(repriseExcelCi);
                    var facturationEtablissement = facturationEtablissementSelectorService.GetFacturationEtablissement(repriseExcelCi);
                    var responsableChantier = personnelSelectorService.GetPersonnel(repriseExcelCi.CodeSociete, repriseExcelCi.MatriculeResponsableChantier, context);
                    var responsableAdministratif = personnelSelectorService.GetPersonnel(repriseExcelCi.CodeSociete, repriseExcelCi.MatriculeResponsableAdministratif, context);


                    fredCi.Adresse = repriseExcelCi.Adresse.Truncate(500);
                    fredCi.Adresse2 = repriseExcelCi.Adresse2;
                    fredCi.Adresse3 = repriseExcelCi.Adresse3;
                    fredCi.Ville = repriseExcelCi.Ville.Truncate(500);
                    fredCi.CodePostal = repriseExcelCi.CodePostal.Truncate(20);
                    fredCi.PaysId = pays?.PaysId;

                    fredCi.EnteteLivraison = repriseExcelCi.EnteteLivraison.Truncate(100);
                    fredCi.AdresseLivraison = repriseExcelCi.AdresseLivraison.Truncate(500);
                    fredCi.CodePostalLivraison = repriseExcelCi.CodePostalLivraison.Truncate(20);
                    fredCi.VilleLivraison = repriseExcelCi.VilleLivraison.Truncate(500);
                    fredCi.PaysLivraisonId = paysLivraison?.PaysId;

                    fredCi.AdresseFacturation = repriseExcelCi.AdresseFacturation.Truncate(500);
                    fredCi.CodePostalFacturation = repriseExcelCi.CodePostalFacturation.Truncate(20);
                    fredCi.VilleFacturation = repriseExcelCi.VilleFacturation.Truncate(500);
                    fredCi.PaysFacturationId = paysFacturation?.PaysId;


                    fredCi.ResponsableAdministratifId = responsableAdministratif?.PersonnelId;
                    fredCi.ResponsableChantier = responsableChantier?.PrenomNom.Truncate(100);
                    fredCi.ZoneModifiable = zoneModifiable;

                    fredCi.DateUpdate = DateTime.UtcNow;
                    fredCi.DateOuverture = DateTime.ParseExact(repriseExcelCi.DateOuverture, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    fredCi.FacturationEtablissement = facturationEtablissement;

                    fredCi.LongitudeLocalisation = commonSelector.GetDouble(repriseExcelCi.Longitude);
                    fredCi.LatitudeLocalisation = commonSelector.GetDouble(repriseExcelCi.Latitude);


                    result.Add(fredCi);
                }
            }

            return result;
        }


    }
}
