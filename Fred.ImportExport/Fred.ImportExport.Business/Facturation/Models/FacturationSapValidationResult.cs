using Fred.Entities.CI;
using Fred.Entities.Commande;
using Fred.Entities.Depense;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielFixe;
using Fred.Entities.Societe;

namespace Fred.ImportExport.Business.Facturation
{
    /// <summary>
    ///     Représente la classe de résultat de la validation d'une ligne de facturation issue de SAP
    ///     Elle permet d'obtenir tous les objets dont on a testé l'existance dans FRED
    /// </summary>
    public class FacturationSapValidationResult
    {
        /// <summary>
        ///     Obtient ou définit la Réception
        /// </summary>
        public DepenseAchatEnt Reception { get; set; }

        /// <summary>
        ///     Obtient ou définit la Commande
        /// </summary>
        public CommandeEnt Commande { get; set; }

        /// <summary>
        ///     Obtient ou définit la Devise
        /// </summary>
        public DeviseEnt Devise { get; set; }

        /// <summary>
        ///     Obtient ou définit le Fournisseur
        /// </summary>
        public FournisseurEnt Fournisseur { get; set; }

        /// <summary>
        ///     Obtient ou définit la Nature Analytique
        /// </summary>
        public NatureEnt Nature { get; set; }

        /// <summary>
        ///     Obtient ou définit le Chantier CI
        /// </summary>
        public CIEnt CI { get; set; }

        /// <summary>
        ///     Obtient ou définit la société
        /// </summary>
        public SocieteEnt Societe { get; set; }

        /// <summary>
        ///     Obtient ou définit la Ressource
        /// </summary>
        public RessourceEnt Ressource { get; set; }

        /// <summary>
        ///     Obtient ou définit la Tâche
        /// </summary>
        public TacheEnt Tache { get; set; }
    }
}
