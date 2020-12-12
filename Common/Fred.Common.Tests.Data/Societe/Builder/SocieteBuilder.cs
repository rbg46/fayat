using Fred.Common.Tests.Data.Groupe.Builder;
using Fred.Common.Tests.Data.Organisation.Builder;
using Fred.Common.Tests.EntityFramework;
using Fred.Entities;
using Fred.Entities.Groupe;
using Fred.Entities.Societe;

namespace Fred.Common.Tests.Data.Societe.Builder
{
    /// <summary>
    /// /Classe Builder de SocieteEnt
    /// </summary>
    public class SocieteBuilder : ModelDataTestBuilder<SocieteEnt>
    {
        public TypeSocieteBuilder Type => new TypeSocieteBuilder(Model);

        /// <summary>
        /// Obtient une société par défaut
        /// </summary>
        /// <returns>une société</returns>
        public SocieteEnt Prototype()
        {
            Model = New();
            Model.SocieteId = 1;
            Model.TypeSocieteId = (int)EnumSocieteType.IsINT;
            Model.Code = "RZB";
            var groupe = new GroupeBuilder();
            Model.Groupe = groupe.Prototype();
            Model.GroupeId = Model.Groupe.GroupeId;
            return Model;
        }

        /// <summary>
        /// Fluent Champ Groupe
        /// </summary>
        /// <param name="groupe">groupe à affecter</param>
        /// <returns>Builder pour fluent</returns>
        public SocieteBuilder Groupe(GroupeEnt groupe)
        {
            Model.Groupe = groupe;
            Model.GroupeId = groupe.GroupeId;
            return this;
        }

        /// <summary>
        /// Fluent Champ SocieteId
        /// </summary>
        /// <param name="groupe">id à affecter</param>
        /// <returns>Builder pour fluent</returns>
        public SocieteBuilder SocieteId(int id)
        {
            Model.SocieteId = id;
            return this;
        }

        /// <summary>
        /// Permet de construire une societe de Fayat TP
        /// </summary>
        /// <returns>societe FTP</returns>
        public SocieteEnt SocieteFTP()
        {
            New();
            Model.SocieteId = 1;
            Model.TypeSocieteId = (int)EnumSocieteType.IsINT;
            var groupe = new GroupeBuilder();
            Model.Groupe = groupe.FayatTP().Build();
            Model.GroupeId = Model.Groupe.GroupeId;
            return Model;
        }

        public SocieteBuilder Organisation(int organisationId, int pereId)
        {
            Model.OrganisationId = organisationId;
            Model.Organisation = new OrganisationBuilder()
                .OrganisationId(organisationId)
                .TypeOrganisationSociete()
                .PereId(pereId)
                .Build();
            return this;
        }
    }
}
