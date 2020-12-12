using Fred.Common.Tests.EntityFramework;
using Fred.Entities;
using Fred.Web.Models.Groupe;
using Fred.Web.Models.Societe;

namespace Fred.Common.Tests.Data.Societe.Builder
{
    /// <summary>
    /// /Classe Builder de SocieteEnt
    /// </summary>
    public class SocieteModelBuilder : ModelDataTestBuilder<SocieteModel>
    {
        /// <summary>
        /// Obtient une société par défaut
        /// </summary>
        /// <returns>une société</returns>
        public SocieteModel Prototype()
        {
            Model = New();
            Model.SocieteId = 1;
            Model.TypeSocieteId = (int)EnumSocieteType.IsINT;
            Model.GroupeId = 1;
            return Model;
        }

        /// <summary>
        /// Fluent Champ Groupe
        /// </summary>
        /// <param name="groupe">groupe à affecter</param>
        /// <returns>Builder pour fluent</returns>
        public SocieteModelBuilder Groupe(GroupeModel groupe)
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
        public SocieteModelBuilder SocieteId(int id)
        {
            Model.SocieteId = id;
            return this;
        }
    }
}
