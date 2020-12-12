using System.Collections.Generic;
using Fred.Web.Models.Organisation;
using Fred.Web.Models.Pole;
using Fred.Web.Models.Referential;
using Fred.Web.Models.Role;
using Fred.Web.Models.Societe;

namespace Fred.Web.Models.Groupe
{
    public class GroupeModel : IReferentialModel
    {
        /// <summary>
        /// Obtient ou définit l'identifiant unique d'un Groupe.
        /// </summary>
        public int GroupeId { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant unique de l'organisation du groupe
        /// </summary>
        public int OrganisationId { get; set; }

        /// <summary>
        /// Obtient ou définit l'organisation du groupe
        /// </summary>
        public OrganisationModel Organisation { get; set; }

        /// <summary>
        /// Obtient ou définit le code d'un Groupe.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Obtient ou définit le libellé d'un Groupe.
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant unique d'un groupe.
        /// </summary>
        public int PoleId { get; set; }

        /// <summary>
        /// Obtient ou définit l'objet groupe attaché à un rôle
        /// </summary>
        public PoleModel Pole { get; set; }

        /// <summary>
        /// Obtient une concaténation du code et du libellé
        /// </summary>
        public string CodeLibelle => this.Code + " - " + this.Libelle;

        /// <summary>
        /// Obtient ou définit la liste des types d'organisations.
        /// </summary>
        public virtual ICollection<TypeOrganisationModel> TypesOrganisations { get; set; }

        /// <summary>
        /// Obtient ou définit la liste des rôles que possèdent cette organisation
        /// </summary>
        public virtual ICollection<RoleModel> Roles { get; set; }

        /// <summary>
        /// Obtient l'id du référentiel
        /// </summary>
        public string IdRef => this.GroupeId.ToString();

        /// <summary>
        /// Obtient le libellé référentiel
        /// </summary>
        public string LibelleRef => this.Libelle.ToString();

        /// <summary>
        /// Obtient le code référentiel
        /// </summary>
        public string CodeRef => this.Code.ToString();

        /// <summary>
        /// Child Societes where [FRED_SOCIETE].[GroupeId] point to this entity (FK_FRED_SOCIETE_GROUPE)
        /// </summary>
        public virtual ICollection<SocieteModel> Societes { get; set; }
    }
}
