using Fred.Web.Models;
using Fred.Web.Models.ReferentielFixe.Light;

namespace Fred.Web.Shared.Models.Personnel.Light
{
    public class PersonnelLightForPickListModel
    {
        /// <summary>
        /// Obtient ou définit l'identifiant unique du membre du personnel
        /// </summary>
        public int PersonnelId { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant unique de la société.
        /// </summary>
        public int? SocieteId { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant unique de la société.
        /// </summary>
        public SocieteLightModel Societe { get; set; }

        /// <summary>
        /// Obtient ou définit l'id ressource du personnel.
        /// </summary>
        public int? RessourceId { get; set; }

        /// <summary>
        /// Obtient ou définit l'objet ressource d'une ligne de commande.
        /// </summary>
        public RessourceLightModel Ressource { get; set; }

        /// <summary>
        /// Obtient ou définit le matricule du membre du personnel
        /// </summary>
        public string Matricule { get; set; }

        /// <summary>
        /// Obtient ou définit le nom du membre du personnel
        /// </summary>
        public string Nom { get; set; }

        /// <summary>
        /// Obtient ou définit le prénom du membre du personnel
        /// </summary>
        public string Prenom { get; set; }

        /// <summary>
        /// Obtient le libellé référentiel d'un personnel.
        /// </summary>
        public string LibelleRef => Prenom + " " + Nom;

        /// <summary>
        /// Obtient le code référentiel d'un personnel.
        /// </summary>
        public string CodeRef
        {
            get
            {
                if (Societe != null)
                {
                    if (Ressource != null)
                    {
                        return Societe.Code + " - " + Matricule + " - " + Ressource.Code;
                    }
                    return Societe.Code + " - " + Matricule;
                }
                return Matricule;
            }
        }

        public string CodeLibelleRef => CodeRef + " - " + LibelleRef;
    }
}
