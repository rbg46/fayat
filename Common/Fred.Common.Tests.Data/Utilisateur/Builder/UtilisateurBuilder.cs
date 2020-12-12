using Fred.Common.Tests.Data.Personnel.Builder;
using Fred.Common.Tests.EntityFramework;
using Fred.Entities.Personnel;
using Fred.Entities.Utilisateur;

namespace Fred.Common.Tests.Data.Utilisateur.Mock
{
    public class UtilisateurBuilder : ModelDataTestBuilder<UtilisateurEnt>
    {
        /// <summary>
        /// Obtient un utilisateur par défaut
        /// </summary>
        /// <returns>un utilisateur</returns>
        public UtilisateurEnt Prototype()
        {
            New();
            Model.UtilisateurId = 1;
            var personnel = new PersonnelBuilder();
            Model.Personnel = personnel.Prototype();
            return Model;
        }

        /// <summary>
        /// UtilisateurId
        /// </summary>
        /// <param name="utilisateurId"></param>
        /// <returns>utilisateur builder</returns>
        public UtilisateurBuilder UtilisateurId(int utilisateurId)
        {
            Model.UtilisateurId = utilisateurId;
            return this;
        }


        /// <summary>
        /// Fluent Champ personnel
        /// </summary>
        /// <param name="groupe">personnel à affecter</param>
        /// <returns>Builder pour fluent</returns>
        public UtilisateurBuilder Personnel(PersonnelEnt personnel)
        {
            Model.Personnel = personnel;
            return this;
        }

        /// <summary>
        /// Obtient un utilisateur super admin
        /// </summary>
        /// <returns>un utilisateur</returns>
        public UtilisateurEnt SuperAdmin()
        {
            New();
            Model.UtilisateurId = 1;
            Model.SuperAdmin = true;
            var personnel = new PersonnelBuilder();
            Model.Personnel = personnel.Prototype();
            return Model;
        }

    }
}
