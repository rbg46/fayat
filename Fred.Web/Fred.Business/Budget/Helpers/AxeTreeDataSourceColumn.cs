using System;
using System.Collections.Generic;

namespace Fred.Business.Budget.Helpers
{
    /// <summary>
    /// Décris une colonne 
    /// </summary>
    public class AxeTreeDataSourceColumn
    {
        /// <summary>
        /// Titre de la colonne
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Tableau d'objet représentant les paramètres additionnel 
        /// que l'utilisateur pourrait vouloir injecter dans la fonction de résolution des valeurs des cellules de la colonne
        /// </summary>
        public IEnumerable<object> AdditionnalParams { get; set; }

        /// <summary>
        /// Cette fonction sera appelée lors du calcul de la valeur de la cellule de la colonne 
        /// pour chaque niveau d'axe
        /// e.g 
        /// Lors du calcul de la valeur d'une colonne montant total pour l'axe T1 avec l'axe principal Tache, 
        /// l'opération (fournie par l'utilisateur) calculerait la valeur du montant total a partir des valeurs des autres cellules de la colonne 
        /// C'est à dire tous les enfants T2 et leurs enfants T3 et ainsi de suite.
        /// Si la valeur est null alors aucune action ne sera effecutée
        /// Les paramètres sont documentés dans la classe ComputeColumnValueForAxeParameters
        /// </summary>
        public Func<ComputeColumnValueForAxeParameters, object[], dynamic> ComputeCellValueFunc { get; set; }
    }
}
