using Fred.Entites;

namespace Fred.Business.Authentification
{
    /// <summary>
    ///   Gestionnaire de l'Authentification.
    /// </summary>
    public interface IAuthentificationManager : IManager
    {
        /// <summary>
        ///   Authentifie un utlisateur par son Login et son Mot de passe.
        /// </summary>
        /// <param name="login">Login de l'utilisateur</param>
        /// <param name="password">Mot de passe de l'utilisateur</param>
        /// <returns>Le status de l'authentification</returns>
        AuthenticationStatus Authenticate(string login, string password);

        /// <summary>
        ///   Authentifie un utilisateur par son login ou email pour réinitialiser son mot de passe 
        /// </summary>
        /// <param name="login">Nom d'utilisateur</param>
        /// <param name="email">email de l'utilisateur</param>
        /// <returns>Le status de l'authentification</returns>
        AuthenticationStatus AuthenticateForResetPassword(string login, string email);

        /// <summary>
        /// Verification des mots de passe
        /// </summary>
        /// <param name="password">Mot de passe</param>
        /// <param name="passwordVerify">Vérification mot de passe</param>
        /// <returns>AuthenticationStatus</returns>
        AuthenticationStatus PasswordVerify(string password, string passwordVerify);
    }
}
