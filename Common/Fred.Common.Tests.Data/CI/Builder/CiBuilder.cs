using Fred.Common.Tests.Data.Referential.Etablissement.Builder;
using Fred.Common.Tests.Data.Societe.Builder;
using Fred.Common.Tests.EntityFramework;
using Fred.Entities.CI;
using Fred.Entities.Societe;

namespace Fred.Common.Tests.Data.CI.Builder
{
    public class CiBuilder : ModelDataTestBuilder<CIEnt>
    {
        /// <summary>
        /// Obtient une ci avec valeurs par défault
        /// </summary>
        /// <returns>ci prototype</returns>
        public CIEnt Prototype()
        {
            Model.CiId = 1;
            Model.SocieteId = 1;
            Model.Societe = new SocieteBuilder().Prototype();
            Model.EtablissementComptable = new EtablissementComptableBuilder().Prototype();
            return Model;
        }

        /// <summary>
        /// Permet de définie l'identifiant
        /// </summary>
        /// <param name="id">identifiant</param>
        /// <returns>fluent buildr</returns>
        public CiBuilder CiId(int id)
        {
            Model.CiId = id;
            return this;
        }

        /// <summary>
        /// Definit la societe
        /// </summary>
        /// <param name="societe">societe</param>
        /// <returns>fluent builder</returns>
        public CiBuilder Societe(SocieteEnt societe)
        {
            Model.Societe = societe;
            return this;
        }

        /// <summary>
        /// Permet de géolocaliser la ci à Arcachon
        /// </summary>
        /// <returns>fluent builder</returns>
        public CiBuilder LocalisationArcachon()
        {
            Model.LatitudeLocalisation = 44.5973;
            Model.LongitudeLocalisation = -1.17;
            return this;
        }
    }
}
