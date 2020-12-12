using System.Linq;

namespace Fred.Business.Budget.Helpers
{
    /// <summary>
    /// Fournie des fonctions de bases pouvant être utilisée lors d'un appel a la fonction AddColumn de la source
    /// </summary>
    public static class ComputeColumnValueForAxe
    {
        /// <summary>
        /// Calcule la valeur de la cellule pour l'axe et la colonne donnée, en effectuant la somme des valeurs contenus dans cette colonne pour toutes les lignes
        /// Qui seront affiché en dessous de l'axe. 
        /// Si une des valeurs est nulle, elle est ignorée
        /// e.g 
        /// Colonne Montant pour l'axe T1, la fonction fera la somme des montants de toutes les lignes partageant le même T1
        /// </summary>
        /// <param name="parameters">Les paramètres de la fonction, voir la clase ComputeColumnValueForAxeParameters pour plus de détail</param>
        /// <param name="additionnalParams">Les paramètres additionnels, ajouté  lors de l'appelle a la fonction AddColumn</param>
        /// <returns>Une variable dynamic</returns>
        public static dynamic SumChildrenValues(ComputeColumnValueForAxeParameters parameters, object[] additionnalParams)
        {
            dynamic sum = 0;
            foreach (var line in parameters.AllSubLines)
            {
                if (line[parameters.ColumnName] != null)
                {
                    sum += line[parameters.ColumnName];
                }
            }
            return sum;
        }

        /// <summary>
        /// Cette fonction n'affiche la donnée que lorsque l'axe actuellement traité sera une feuille de l'arbre. Null pour les autres cas
        /// </summary>
        /// <param name="parameters">Les paramètres de la fonction, voir la clase ComputeColumnValueForAxeParameters pour plus de détail</param>
        /// <param name="additionnalParams">Les paramètres additionnels, ajouté  lors de l'appelle a la fonction AddColumn</param>
        /// <returns>Une variable dynamic</returns>
        public static dynamic DisplayOnlyInLeaves(ComputeColumnValueForAxeParameters parameters, object[] additionnalParams)
        {
            if (AxeTreeBuilder.IsLeafForDefaultDetails(parameters.CurrentAxe, parameters.AxePrincipal))
            {
                return parameters.AllSubLines.First()[parameters.ColumnName];
            }
            return null;
        }

        /// <summary>
        /// Calcule la moyenne des valeurs contenu pour cette colonne
        /// </summary>
        /// <param name="parameters">Les paramètres de la fonction, voir la clase ComputeColumnValueForAxeParameters pour plus de détail</param>
        /// <param name="additionnalParams">Les paramètres additionnels, ajouté  lors de l'appelle a la fonction AddColumn</param>
        /// <returns>La somme des valeurs de la colonne donnée dans la liste</returns>
        public static dynamic ComputeAverageValueOfColumn(ComputeColumnValueForAxeParameters parameters, object[] additionnalParams)
        {
            if (parameters.AnyNullValueForColumnInSubLines())
            {
                return null;
            }

            return ComputeAverageValueWithNullAsZero(parameters, additionnalParams);
        }

        /// <summary>
        /// Retourne la valeur moyenne des valeurs de la colonne en considérant les valeurs nulles comme étant 0
        /// </summary>
        /// <param name="parameters">Les paramètres de la fonction, voir la clase ComputeColumnValueForAxeParameters pour plus de détail</param>
        /// <param name="additionnalParameters">Les paramètres additionnels, ajouté  lors de l'appelle a la fonction AddColumn</param>
        /// <returns>Une chaine de charactères si l'axe correspond au niveau le plus bas, null sinon</returns>
        public static dynamic ComputeAverageValueWithNullAsZero(ComputeColumnValueForAxeParameters parameters, object[] additionnalParameters)
        {
            var sum = SumChildrenValues(parameters, additionnalParameters);
            return  sum / parameters.AllSubLines.Count();
        }

        /// <summary>
        /// Retourne le pourcentage avancement
        /// </summary>
        /// <param name="parameters">Les paramètres de la fonction, voir la clase ComputeColumnValueForAxeParameters pour plus de détail</param>
        /// <param name="additionnalParameters">Les paramètres additionnels, ajouté  lors de l'appelle a la fonction AddColumn</param>
        /// <returns>Une chaine de charactères si l'axe correspond au niveau le plus bas, null sinon</returns>
        public static dynamic ComputePourcentAvancement(ComputeColumnValueForAxeParameters parameters, object[] additionnalParameters)
        {
            dynamic sumBudget = 0;
            dynamic sumDad = 0;
            string nameColumnDad = string.Empty;

            if (parameters.ColumnName == "PourcentageAvancementMoisCourant")
            {
                nameColumnDad = "MontantDadMoisCourant";
            }
            else if(parameters.ColumnName == "PourcentageAvancement")
            {
                nameColumnDad = "MontantDad";
            }

            foreach (var subLine in parameters.AllSubLines)
            {
                if (subLine["MontantBudget"] != null)
                {
                    sumBudget += subLine["MontantBudget"];
                }
                if (subLine[nameColumnDad] != null)
                {
                    sumDad += subLine[nameColumnDad];
                }
            }

            return sumBudget == 0 ? 0 : sumDad / sumBudget * 100;
        }
    }
}
