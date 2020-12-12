using Fred.Entities.Referential;
using Fred.Entities.ReferentielFixe;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Fred.Business.Budget.Helpers
{
    /// <summary>
    /// Représente l'objet qui servira pour construire un objet AxeViewer
    /// Il encapsule une tache de niveau 3 et une Ressource ainsi qu'une série de valeurs calculée associée à ce couple
    /// Avec la définition d'un axe principal et d'un axe secondaire e.g Tache>Ressource ou Ressource>Tache
    /// </summary>
    public class AxeTreeDataSource
    {
        /// <summary>
        /// Liste des colonnes
        /// </summary>
        public ICollection<AxeTreeDataSourceColumn> Columns { get; private set; } = new List<AxeTreeDataSourceColumn>();

        /// <summary>
        /// Représente les valeurs de base qui seront affichées et qui serviront pour le calcul des parents
        /// pour ce couple Tache-Ressource
        /// e.g Dépense, Temps passé, Commentaire 
        /// </summary>
        public ICollection<AxeTreeDataSourceRow> Valeurs { get; set; } = new List<AxeTreeDataSourceRow>();

        /// <summary>
        /// Retourune une nouvelle ligne, vide lors d'une première utilisation de l'objet
        /// il faut fournir les données à afficher sur la dernière ligne de l'arbre
        /// </summary>
        /// <returns>Une nouvelle ligne</returns>
        public AxeTreeDataSourceRow NewRow()
        {
            return new AxeTreeDataSourceRow(Columns);
        }

        /// <summary>
        /// Ajoute une colonne a la source de donnée, avec la fonction permettant de résoudre les valeurs des cellules pour cette colonne
        /// Le param object de la Func permets d'injecter des paramètres additionnels à la fonction
        /// </summary>
        /// <param name="title">Titre de la colonne</param>
        /// <param name="computeCellValueFunc">Fonction à appeler lorsqu'il faudra calculer les valeurs des cellules des parents pour cette colonne. 
        /// Si la valeur est null alors la valeur null est affectée à cette colonne pour tous les parents</param>
        /// <param name="additionnalParams">Tous les paramètres additionnels qui seront injectés dans la fonction computeCellValueFunc</param>
        public void AddColumn(string title, Func<ComputeColumnValueForAxeParameters, object[], dynamic> computeCellValueFunc, params object[] additionnalParams)
        {
            Columns.Add(new AxeTreeDataSourceColumn()
            {
                Title = title,
                AdditionnalParams = additionnalParams,
                ComputeCellValueFunc = computeCellValueFunc
            });
        }
    }
}
