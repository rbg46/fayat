using System;
using System.Linq;
using Fred.Business.Budget.Helpers;
using Fred.Entities.Referential;
using Fred.Framework.Exceptions;
using MoreLinq;

namespace Fred.Business.Budget.ControleBudgetaire.Helpers
{
    /// <summary>
    /// Tout comme la classe statique ComputeColumnValueForAxe du moteur de génération d'arbre.
    /// (Voir Budget.helpers.ComputeColumnValueForAxe) cette classe fournie des méthodes pour calculer 
    /// les valeurs des colonnes pour chaque axe. 
    /// </summary>
    public static class CustomComputeColumnValueForAxe
    {
        /// <summary>
        /// Les Quantités sont gérés d'une manière un peu spéciale
        /// Au niveau de la feuille, la quantité affichée est celle de la ressource associée à la tache
        /// Au niveau dit "supérieur" : 
        /// c'est à dire Niveau Ressource pour l'axe principal Ressource (on aura un arbre : Chapitre-SousChapitre-Ressource-T1-T2-T3)
        /// c'est à dire Niveau Tache pour l'axe principal Tache (on aura un arbre : T1-T2-T3-Chapitre-SousChapitre-Ressource)
        /// Dans ces niveaux la quantité est la somme des quantités des feuilles
        /// Dans les autre cas on n'affiche rien
        /// </summary>
        /// <param name="parameters">Les paramètres de la fonction, voir la clase ComputeColumnValueForAxeParameters pour plus de détail</param>
        /// <param name="additionnalParameters">Les paramètres additionnels, ajouté  lors de l'appelle a la fonction AddColumn</param>
        /// <returns>Une quantité décimal ou null (voir la description plus haut)</returns>
        public static dynamic GetQuantite(ComputeColumnValueForAxeParameters parameters, object[] additionnalParameters)
        {
            if (AxeTreeBuilder.IsLeafForDefaultDetails(parameters.CurrentAxe, parameters.AxePrincipal) && parameters.AllSubLines.Count() == 1)
            {
                //dans ce cas la on renvoi simplement la valeur de la colonne demandée
                var ressourceUnite = parameters.AllSubLines.Single()[parameters.ColumnName] as RessourceUnite;
                return ressourceUnite;
            }
            else
            {
                //Si on est ici c'est qu'on est sur un niveau dit "supérieur" ou chapitre MO Production
                //Ou encore que l'utilisateur a saisi deux fois la même ressource sur un même sous détail 
                //On doit donc calculer la somme des quantités

                //Ici on sait que les données de quantité ont été enregistré dans avec le type RessourceUnite
                //Selon la colonne la valeur peut ne pas être renseigné
                //e.g La quantité Budget pour un couple T3-Ressource sur lequel on a fait une dépense qui n'était pas budgété
                var resourceUnites = parameters.AllSubLines.Select(l => l[parameters.ColumnName])
                  .Cast<RessourceUnite>()
                  .Where(ru => ru != null);

                //Il est donc possible que toutes les lignes de lines soit des lignes de dépenses non budgétées
                if (!resourceUnites.Any())
                {
                    return null;
                }

                //Si toutes les unités sont identiques on peut réutiliser le signe pour l'unité de la valeur total 
                //Mais s'il y a des différences alors on doit utiliser le signe #
                var unite = resourceUnites.First().Unite;
                if (resourceUnites.DistinctBy(l => l.Unite).Count() > 1)
                {
                    //Si on est ici, c'est que différentes unités sont utilisées
                    //donc on doit utiliser le symble #
                    unite = "#";
                }

                var quantiteTotale = resourceUnites.Sum(ru => ru.Quantite);
                return new RessourceUnite()
                {
                    Quantite = quantiteTotale,
                    Unite = unite
                };
            }
        }

        /// <summary>
        /// retourne un flag indiquant si les quantités / PU doivent etre affichées
        /// </summary>
        /// <param name="parameters">Les paramètres de la fonction, voir la clase ComputeColumnValueForAxeParameters pour plus de détail</param>
        /// <param name="additionnalParameters">Les paramètres additionnels, ajouté  lors de l'appelle a la fonction AddColumn</param>
        /// <returns>Un booleen sous forme de int</returns>
        public static dynamic GetDisplayQtePuFlag(ComputeColumnValueForAxeParameters parameters, object[] additionnalParameters)
        {
            if (AxeTreeBuilder.IsLeafForDefaultDetails(parameters.CurrentAxe, parameters.AxePrincipal) && parameters.AllSubLines.Count() == 1
                || AxeTreeBuilder.IsNiveauSuperieurForDefaultDetails(parameters.CurrentAxe, parameters.AxePrincipal)
                || AxeTreeBuilder.IsNiveauChapitreMoProductionOrUnder(parameters.CurrentAxe, parameters.AllSubLines)
                || AxeTreeBuilder.IsLeafForDefaultDetails(parameters.CurrentAxe, parameters.AxePrincipal))
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Retourne le prix unitaire moyen de toutes les couples T3-Ressources contenus dans lines
        /// </summary>
        /// <param name="parameters">Les paramètres de la fonction, voir la clase ComputeColumnValueForAxeParameters pour plus de détail</param>
        /// <param name="additionnalParameters">Les paramètres additionnels, ajouté  lors de l'appelle a la fonction AddColumn</param>
        /// <returns>Renvoi un decimal contenant le prix unitaire moyen, potentiellement null</returns>
        public static dynamic GetPu(ComputeColumnValueForAxeParameters parameters, object[] additionnalParameters)
        {
            var columnNameMontant = additionnalParameters[0] as string;
            var columnNameQuantite = additionnalParameters[1] as string;

            decimal? totalMontant = parameters.AllSubLines.Sum(x => (x[columnNameMontant] as decimal?));
            decimal? totalQuantite = parameters.AllSubLines.Sum(x => (x[columnNameQuantite] as RessourceUnite)?.Quantite);
            decimal? pu = null;

            //dans ce cas la on renvoi simplement la valeur de la colonne demandée
            if (totalQuantite > 0)
            {
                pu = totalMontant / totalQuantite;
            }

            return pu;

        }

        /// <summary>
        /// Retourne un booléen valant true si la tache correspondant à l'axe en cours a été créée après la mise en application du budget
        /// false sinon
        /// </summary>
        /// <param name="parameters">Les paramètres de la fonction, voir la clase ComputeColumnValueForAxeParameters pour plus de détail</param>
        /// <param name="additionnalParameters">Les paramètres additionnels, ajouté  lors de l'appelle a la fonction AddColumn</param>
        /// <returns>Un booléen, null si l'axe courant n'est pas une tache</returns>
        public static dynamic IsNewTache(ComputeColumnValueForAxeParameters parameters, object[] additionnalParameters)
        {
            if (!AxeTreeBuilder.IsAxeTache(parameters.CurrentAxe))
            {
                return null;
            }

            TacheEnt tache = null;
            switch (parameters.CurrentAxe)
            {
                case AxeTypes.T1:
                    tache = parameters.AllSubLines.First().Tache3.Parent.Parent;
                    break;
                case AxeTypes.T2:
                    tache = parameters.AllSubLines.First().Tache3.Parent;
                    break;
                case AxeTypes.T3:
                    tache = parameters.AllSubLines.First().Tache3;
                    break;
                default:
                    throw new FredTechnicalException("L'axe est considéré comme étant une tâche mais n'est pas un T1, T2, T3");
            }

            var dateMiseEnApplicationBudget = (DateTime)additionnalParameters[0];
            return tache.DateCreation > dateMiseEnApplicationBudget;
        }

        /// <summary>
        /// Retourne le commentaire de l'ajustement lié à ce couple T3-Ressource
        /// Le commentaire de l'ajustement n'est visible qu'au niveau le plus bas de l'arbre
        /// </summary>
        /// <remarks>
        /// Cette fonction est identique à la fonction DisplayOnlyInLeaves nonobstant une différence fondamentale 
        /// la fonction DisplayOnlyInLeaves lancera une exception si le paramètre AllSubLines contient plusieurs valeurs 
        /// Alors que cette fonction ne le fait pas, elle prend le premier element de la liste.
        /// Cette fonction est utilisée car actuellement une ressource peut être saisie deux fois dans le même sous détail d'un budget
        /// ce qui fait qu'avec l'axe principal TacheRessource on aura au niveau de la feuille non pas une mais deux lignes (une pour chaque apparition de la ressource dans le sous détail)
        /// Ce qui provequerait un crash dans DisplayOnlyInLeaves. Ca ne pose pas de problème dans le cas du commentaire de l'ajustement car ce commentaire est saisi 
        /// pour un couple T3Id-RessourceId et dans notre cas on traiterait deux fois la même ressource donc le même Id
        /// </remarks>
        /// <param name="parameters">Les paramètres de la fonction, voir la clase ComputeColumnValueForAxeParameters pour plus de détail</param>
        /// <param name="additionnalParameters">Les paramètres additionnels, ajouté  lors de l'appelle a la fonction AddColumn</param>
        /// <returns>Une chaine de charactères si l'axe correspond au niveau le plus bas, null sinon</returns>
        public static dynamic GetAjustementCommentaire(ComputeColumnValueForAxeParameters parameters, object[] additionnalParameters)
        {
            if (AxeTreeBuilder.IsLeafForDefaultDetails(parameters.CurrentAxe, parameters.AxePrincipal))
            {
                return parameters.AllSubLines.First()[parameters.ColumnName];
            }
            return null;
        }

        public static dynamic GetProjectionLineaire(ComputeColumnValueForAxeParameters parameters,
            object[] additionnalParameters)
        {
            return parameters.AllSubLines.Sum(p => p[parameters.ColumnName] as decimal?);
        }
    }

}
