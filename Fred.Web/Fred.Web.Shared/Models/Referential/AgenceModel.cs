using Fred.Web.Shared.Models.Referential;

namespace Fred.Web.Models.Referential
{
    public class AgenceModel
    {
        /// <summary>
        /// Obtient ou définit l'identifiant unique d'une agence
        /// </summary>
        public int AgenceId { get; set; }

        /// <summary>
        /// Obtient ou définit le code d'une agence
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Obtient ou définit le libellé d'une agence
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Obtient ou définit le téléphone de l'agence
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        /// Obtient ou définit le fax de l'agence
        /// </summary>
        public string Fax { get; set; }

        /// <summary>
        /// Obtient ou définit l'email de l'agence
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Obtient ou définit le SIRET de l'agence
        /// </summary>
        public string SIRET { get; set; }

        /// <summary>
        /// Obtient une concaténation du code et du libellé
        /// </summary>
        public string CodeLibelle => this.Code + " - " + this.Libelle;

        public AdresseModel Adresse { get; set; }
    }
}
