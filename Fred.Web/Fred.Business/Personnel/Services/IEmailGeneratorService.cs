namespace Fred.Business.Personnel
{
    public interface IEmailGeneratorService
    {
        /// <summary>
        /// Génération d'un email à partir du nom et prénom
        /// </summary>
        /// <param name="nom">nom</param>
        /// <param name="prenom">prenom</param>
        /// <param name="codeSocietePaye">codeSocietePaye</param>
        /// <param name="societeId">societeId</param>
        /// <returns>l'email généré</returns>
        string GenerateEmail(string nom, string prenom, string codeSocietePaye, int societeId);
    }
}
