using Fred.Web.Models.ReferentielFixe;

namespace Fred.Web.Shared.Models.Moyen
{
    /// <summary>
    /// Model pour la fiche générique
    /// </summary>
    public class FicheGeneriqueModel
    {
        /// <summary>
        /// Obtient ou définit l'entité ressource
        /// </summary>
        public RessourceModel Ressource { get; set; }

        /// <summary>
        /// Obtient le code référentiel d'un moyen.
        /// </summary>
        public string CodeRef
        {
            get
            {
                return Ressource?.Code;
            }
        }

        /// <summary>
        /// Obtient le libelle référentiel d'un moyen.
        /// </summary>
        public string LibelleRef
        {
            get
            {
                return Ressource?.Libelle;
            }
        }
    }
}
