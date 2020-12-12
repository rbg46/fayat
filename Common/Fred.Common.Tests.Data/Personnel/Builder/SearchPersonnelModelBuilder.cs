
using System;
using Fred.Business.Personnel;
using Fred.Common.Tests.EntityFramework;

namespace Fred.Common.Tests.Data.Personnel.Builder
{
    public class SearchPersonnelModelBuilder : ModelDataTestBuilder<SearchLightPersonnelModel>
    {

        /// <summary>
        /// instancie une instance de SearchLightPersonnelModel
        /// </summary>
        /// <returns>SearchLightPersonnelModel</returns>
        public override SearchLightPersonnelModel New()
        {
            base.New();
            Model.CiId = 1;
            Model.Page = 1;
            Model.PageSize = 25;
            return Model;
        }

        /// <summary>
        /// Fluent Champ DateChantier
        /// </summary>
        /// <param name="annee">Annee de la date</param>
        /// <param name="mois">Mois de la date</param>
        /// <param name="jour">Jour de la date</param>
        /// <returns></returns>
        public SearchPersonnelModelBuilder DateChantier(int annee, int mois, int jour)
        {
            Model.DateChantier = new DateTime(annee, mois, jour);
            return this;
        }

        /// <summary>
        /// Fluent Champ DateDebutChantier
        /// </summary>
        /// <param name="periode">periode chnatier</param>
        /// <returns></returns>
        public SearchPersonnelModelBuilder PeriodeChantier(int periode)
        {
            Model.PeriodeChantier = periode;
            return this;
        }



    }
}
