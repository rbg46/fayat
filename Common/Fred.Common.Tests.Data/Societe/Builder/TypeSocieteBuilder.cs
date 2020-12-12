using Fred.Entities;
using Fred.Entities.Societe;

namespace Fred.Common.Tests.Data.Societe.Builder
{
    /// <summary>
    /// Classe Builder de TypeSocieteEnt
    /// </summary>
    public class TypeSocieteBuilder : SocieteBuilder
    {
        public TypeSocieteBuilder(SocieteEnt societe)
        {
            Model = societe;
        }

        /// <summary>
        /// Fluent Champ Code
        /// </summary>
        /// <param name="code">code à affecter</param>
        /// <returns>TypeSocieteBuilder pour Fluent</returns>
        public TypeSocieteBuilder Code(string code)
        {
            Model.Code = code;
            return this;
        }

        public TypeSocieteBuilder Interne()
        {
            var typeId = 1;
            Model.TypeSociete = new TypeSocieteEnt { Code = Constantes.TypeSociete.Interne, Libelle = "Interne", TypeSocieteId = typeId };
            Model.TypeSocieteId = typeId;
            return this;
        }

        public TypeSocieteBuilder Partenaire()
        {
            var typeId = 2;
            Model.TypeSociete = new TypeSocieteEnt { Code = Constantes.TypeSociete.Partenaire, Libelle = "Partenaire", TypeSocieteId = typeId };
            Model.TypeSocieteId = typeId;
            return this;
        }

        public TypeSocieteBuilder Sep()
        {
            var typeId = 3;
            Model.TypeSociete = new TypeSocieteEnt { Code = Constantes.TypeSociete.Sep, Libelle = "SEP", TypeSocieteId = typeId };
            Model.TypeSocieteId = typeId;
            return this;
        }
    }

}
