using System;
using Fred.Entities.Adresse;
using Fred.Entities.EntityBase;

namespace Fred.Entities.Referential
{
    /// <summary>
    /// Représente une agence
    /// </summary>
    public class AgenceEnt : AuditableEntity, IEquatable<AgenceEnt>
    {
        /// <summary>
        /// Obtient ou définit l'identifiant unique d'une agence
        /// </summary>
        public int? AgenceId { get; set; }

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
        /// Obtient ou définit le SIRET de l'agence
        /// </summary>
        public DateTime? DateCloture { get; set; }

        /// <summary>
        /// Obtient ou définit l'id de l'adresse
        /// </summary>
        public int? AdresseId { get; set; }

        /// <summary>
        /// Obtient ou définit l'id de l'adresse
        /// </summary>
        public AdresseEnt Adresse { get; set; }

        /// <summary>
        /// Obtient ou définit l'id du fournisseur auquel est liée l'agence
        /// </summary>
        public int FournisseurId { get; set; }

        /// <summary>
        /// Obtient ou définit le fournisseur auquel est liée l'agence
        /// </summary>
        public FournisseurEnt Fournisseur { get; set; }

        /// <summary>
        /// envoie l'égalité entre deux entités agences
        /// </summary>
        /// <param name="other">AgenceEnt à comparer</param>
        /// <returns>retourne vrai si l'égalité est juste</returns>
        public bool Equals(AgenceEnt other)
        {
            return this.Code == other.Code
            && this.Libelle == other.Libelle
            && this.Telephone == other.Telephone
            && this.Fax == other.Fax
            && this.Email == other.Email
            && this.SIRET == other.SIRET
            && this.Adresse.Equals(other.Adresse);
        }
    }
}
