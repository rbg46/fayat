using Fred.Entities;
using Fred.Framework.Extensions;

namespace Fred.Web.Models.Organisation
{
    /// <summary>
    /// Une représentation légère de l'entité Organisation
    /// </summary>
    public class OrganisationLightModel : Referential.IReferentialModel
    {
        /// <summary>
        /// Obtient ou définit l'identifiant de l'organisation
        /// </summary>
        public int OrganisationId { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant de l'organisation parente
        /// </summary>
        public int? PereId { get; set; }

        /// <summary>
        /// Obtient ou définit l'identifiant du type de l'organisation
        /// </summary>
        public int TypeOrganisationId { get; set; }

        /// <summary>
        /// Obtient ou définit le libellé type organisation.
        /// </summary>
        public string TypeOrganisationLibelle { get; set; }

        /// <summary>
        /// Obtient ou définit le code de l'organisation
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///   Obtient ou définit les inetiales de la société
        /// </summary>
        public string CodeSociete { get; set; }

        /// <summary>
        /// Obtient ou définit le libellé
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Obtient ou définit les codes parent
        /// </summary>
        public string CodeParents { get; set; }

        /// <summary>
        /// Obtient ou définit le code parent direct
        /// </summary>
        public string CodeParent { get; set; }

        /// <summary>
        /// Obtient l'id du référentiel
        /// </summary>
        public string IdRef => this.OrganisationId.ToString();

        /// <summary>
        /// Définit le code du référentiel
        /// </summary>
        public string CodeRef => TypeOrganisationId == (int)OrganisationType.Ci ? $"{this.CodeSociete} - {this.Code}" : $"{this.TypeOrganisationLibelle} - {this.Code}";

        /// <summary>
        /// Obtient l'id du référentiel
        /// </summary>
        public string LibelleRef => this.Libelle;

        /// <summary>
        /// Champ formatte : Code + Libelle
        /// </summary>
        public string CodeLibelle => $"{this.Code} - {this.Libelle}";

        /// <summary>
        /// Champ formatte : CodeParent + Libelle
        /// </summary>
        public string CodeParentLibelle => $"{this.CodeParent} - {this.Libelle}";

        /// <summary>
        /// Champ formatte : CodeParents simplifie a partir de la societe
        /// </summary>
        public string CodeParentsSimplifie
        {
            get
            {
                if (!string.IsNullOrEmpty(this.CodeParents))
                {
                    var codes = this.CodeParents.Split('|');

                    if (codes.Length > 3)
                    {
                        //Enleve juste les codes superflus => Pole, Groupe et Holding
                        var codeToRemove = codes[0] + '|' + codes[1] + '|' + codes[2] + '|';
                        return CodeParents.Replace(codeToRemove, "");
                    }
                    else
                    {
                        return string.Empty;
                    }
                }
                return string.Empty;
            }
        }
    }
}
