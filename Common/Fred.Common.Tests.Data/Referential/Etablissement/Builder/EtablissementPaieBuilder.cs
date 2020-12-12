using Fred.Common.Tests.Data.Societe.Builder;
using Fred.Common.Tests.EntityFramework;
using Fred.Entities.Referential;
using Fred.Entities.Societe;

namespace Fred.Common.Tests.Data.Referential.Etablissement.Builder
{
    public class EtablissementPaieBuilder : ModelDataTestBuilder<EtablissementPaieEnt>
    {
        private readonly SocieteBuilder SocietyBuilder = new SocieteBuilder();

        /// <summary>
        /// Obtient un Etablissement par default
        /// </summary>
        /// <returns>établissement prototype</returns>
        public EtablissementPaieEnt Prototype()
        {
            New();
            Model.EtablissementPaieId = 1;
            Model.Code = "E001";
            var etabCompta = new EtablissementComptableBuilder().Prototype();
            Model.EtablissementComptable = etabCompta;
            Model.EtablissementComptableId = etabCompta.EtablissementComptableId;
            Model.Latitude = 0;
            Model.Longitude = 0;
            Model.Societe = SocietyBuilder.Prototype();
            return Model;
        }

        /// <summary>
        /// fluent du champ GestionIndemnites
        /// </summary>
        /// <returns>fluent builder</returns>
        public EtablissementPaieBuilder GestionIndemnites()
        {
            Model.GestionIndemnites = true;
            return this;
        }

        public EtablissementPaieBuilder EtablissementPaieId(int id)
        {
            Model.EtablissementPaieId = id;
            return this;
        }

        public EtablissementPaieBuilder Code(string code)
        {
            Model.Code = code;
            return this;
        }

        public EtablissementPaieBuilder IsAgenceDeRattachement()
        {
            Model.IsAgenceRattachement = true;
            return this;
        }

        public EtablissementPaieBuilder IsNotAgenceDeRattachement()
        {
            Model.IsAgenceRattachement = false;
            return this;
        }

        public EtablissementPaieBuilder Active()
        {
            Model.Actif = true;
            return this;
        }

        public EtablissementPaieBuilder NotActive()
        {
            Model.Actif = false;
            return this;
        }

        /// <summary>
        /// Fluent Champ Societe
        /// </summary>
        /// <param name="groupe">societe à affecter</param>
        /// <returns>Builder pour fluent</returns>
        public EtablissementPaieBuilder Societe(SocieteEnt societe)
        {
            Model.Societe = societe;
            Model.SocieteId = societe.SocieteId;
            return this;
        }

        /// <summary>
        /// Permet de géolocaliser la ci à bordeaux
        /// </summary>
        /// <returns>fluent builder</returns>
        public EtablissementPaieBuilder LocalisationBordeaux()
        {
            Model.Latitude = 44.7828;
            Model.Longitude = -0.5795;
            return this;
        }


        /// <summary>
        /// Permet de géolocaliser à Arcachon
        /// </summary>
        /// <returns>fluent builder</returns>
        public EtablissementPaieBuilder LocalisationArcachon()
        {
            Model.Latitude = 44.5973;
            Model.Longitude = -1.17;
            return this;
        }

        /// <summary>
        /// Permet de situer l'établissement hors de la région
        /// </summary>
        /// <returns>fluent builder</returns>
        public EtablissementPaieBuilder HorsRegion()
        {
            Model.HorsRegion = true;
            return this;
        }

        /// <summary>
        /// Permet de situer l'établissement dans la région
        /// </summary>
        /// <returns>fluent builder</returns>
        public EtablissementPaieBuilder EnRegion()
        {
            Model.HorsRegion = false;
            return this;
        }

        public EtablissementPaieBuilder Adresse(string adresse)
        {
            Model.Adresse = adresse;
            return this;
        }
    }
}
