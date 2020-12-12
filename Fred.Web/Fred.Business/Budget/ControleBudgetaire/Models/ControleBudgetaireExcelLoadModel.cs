using System.Collections.Generic;
using Fred.Business.Budget.Helpers;

namespace Fred.Business.Budget.ControleBudgetaire.Models
{
    /// <summary>
    /// Modèle permettant de charger l'export excel
    /// </summary>
    public class ControleBudgetaireExcelLoadModel
    {
        /// <summary>
        /// True si l'export doit être fait vers un PDF
        /// </summary>
        public bool IsPdfConverted { get; set; }

        /// <summary>
        /// Id du budget en application qu'on exporte
        /// </summary>
        public int BudgetId { get; set; }

        /// <summary>
        /// Liste des identifiants de CI supplémentaires
        /// </summary>
        public List<int> CiIdList { get; set; }

        /// <summary>
        /// Période jusqu'a laquelle et pour laquelle on va récupérer des données
        /// </summary>
        public int Periode { get; set; }

        /// <summary>
        /// Axe principal choisi
        /// </summary>
        public AxePrincipal AxePrincipal { get; set; }

        /// <summary>
        /// Liste des axes à afficher
        /// </summary>
        public AxeTypes[] AxeAffichees { get; set; }

        /// <summary>
        /// Arbre actuellement affiché, si cette valeur est nulle alors tous les axes sont affichés normalement
        /// sinon cette valeur peut être utilisée pour "masquer" certaines lignes 
        /// e.g pour un axe principal Tache et axes affichés T1 - T2 - Chapitre
        /// Tree peut valoir : T1- T2 | T1-T2-Chapitre
        /// Le fichier exporté suivra alors cette arborescence.
        /// Si Tree ne respecte pas l'arborescence spécifiée par AxePrincipal et Axes Affichés
        /// par exemple pour un axe principal Tache et axes affichés T1 - T2 - Chapitre
        /// si Tree vaut : Chapitre - Sous CHapitre - T1 (Ici l'axe principal est clairement Chapitre et non Tache)
        /// le comportement est indéfini
        /// </summary>
        public IEnumerable<AxeTreeLightModel> Tree { get; set; }
    }


}
