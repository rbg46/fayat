using Fred.Entities.Utilisateur;

namespace Fred.Common.Tests.Data.Utilisateur.Mock
{
    public static class UtilisateurMocks
    {
        public const int Utilisateur_ID_THOMAS = 1;
        public const int Utilisateur_ID_DUPONT = 2;
        public const int Utilisateur_ID_MARTIN = 3;


        public static UtilisateurEnt CreateThomas()
        {
            return new UtilisateurEnt
            {
                UtilisateurId = Utilisateur_ID_THOMAS,
            };
        }


        public static UtilisateurEnt CreateUserDupont()
        {
            return new UtilisateurEnt
            {
                UtilisateurId = Utilisateur_ID_DUPONT,
            };
        }

        public static UtilisateurEnt CreateUserMartin()
        {
            return new UtilisateurEnt
            {
                UtilisateurId = Utilisateur_ID_MARTIN,
            };
        }
    }
}
