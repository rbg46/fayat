using System.Collections.Generic;
using System.Linq;

namespace Fred.Business.Budget.Helpers
{
    /// <summary>
    /// Cette classe décrit les différents paramètres qui seront passés au fonction de résolution des valeurs des cellules de l'arbre
    /// Chaque colonne spécifiant sa propre fonction de résolution pour ses cellules
    /// La classe propose egalement des fonction utilitaires permettant de manipuler les paramètres
    /// </summary>
    public class ComputeColumnValueForAxeParameters
    {
        /// <summary>
        /// Toutes les lignes décrites dans la source de données (AxeTreeDataSource) qui se situeront sous l'axe courant dans l'arbre
        /// </summary>
        public IEnumerable<AxeTreeDataSourceRow> AllSubLines { get; set; }

        /// <summary>
        /// L'axe dont les cellules sont actuellements en train d'être résolues
        /// </summary>
        public AxeTypes CurrentAxe { get; set; }

        /// <summary>
        /// Le nom de la colonne dont on résoud les valeurs des cellules 
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// L'axe principal utilisé pour la construction de l'arbre, cette propriété recevra la valeur du paramètre AxePrincipal du contructeur de l'AxeTreeBuilder
        /// </summary>
        public AxePrincipal AxePrincipal { get; set; }

        /// <summary>
        /// Détails des axes de l'arbre, cette propriété reçoit la valeur du paramètre detailsAxeCustom du constructeur de l'AxeTreeBuilder
        /// </summary>
        public AxeTypes[] DetailAxeCustom { get; set; }

        /// <summary>
        /// Cette fonction retourne true si dans les lignes qui se situeront sous l'axe courant (AllSubLines) 
        /// Une des colonnes a la valeur null
        /// </summary>
        /// <returns>un booléen</returns>
        public bool AnyNullValueForColumnInSubLines()
        {
            return AllSubLines.Any(s => s[ColumnName] == null);
        }

    }
}
