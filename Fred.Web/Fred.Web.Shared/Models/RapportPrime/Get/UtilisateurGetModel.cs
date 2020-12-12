namespace Fred.Web.Shared.Models.RapportPrime.Get
{
    public class UtilisateurGetModel
    {
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string NomPrenom => Nom + " " + Prenom;
    }
}