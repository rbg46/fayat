namespace Fred.Web.Shared.Models.Commande
{
    public class PersonnelWithPermissionAndSeuilModel
    {

        public string IdRef { get; set; }

        public string CodeRef { get; set; }

        public string LibelleRef { get; set; }

        /// <summary>
        /// PersonnelId
        /// </summary>
        public int PersonnelId { get; set; }

        /// <summary>
        /// HaveMinimunSeuilValidation
        /// </summary>
        public bool HaveMinimunSeuilValidation { get; set; }

        /// <summary>
        /// Nom
        /// </summary>
        public string Nom { get; set; }

        /// <summary>
        /// Prenom
        /// </summary>
        public string Prenom { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }

    }
}
