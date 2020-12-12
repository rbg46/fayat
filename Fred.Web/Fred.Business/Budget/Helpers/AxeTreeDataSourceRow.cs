using System.Collections.Generic;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielFixe;
using Fred.Framework.Exceptions;

namespace Fred.Business.Budget.Helpers
{
    /// <summary>
    /// Représente et contient une valeur liée au couple T3-Ressource qui contient une liste d'instances de cette classe
    /// </summary>
    public class AxeTreeDataSourceRow
    {
        /// <summary>
        /// Lie une colonne a une valeur
        /// </summary>
        private readonly Dictionary<string, dynamic> columnNameToValueDictionary = new Dictionary<string, dynamic>();

        /// <summary>
        /// Instancie un objet element, ne pas utiliser directement
        /// </summary>
        /// <param name="columns">liste des colonnes supportées</param>
        public AxeTreeDataSourceRow(IEnumerable<AxeTreeDataSourceColumn> columns)
        {
            foreach (var c in columns)
            {
                columnNameToValueDictionary.Add(c.Title, null);
            }
        }

        /// <summary>
        /// La tache de niveau 3
        /// </summary>
        public TacheEnt Tache3 { get; set; }

        /// <summary>
        /// La ressource 
        /// </summary>
        public RessourceEnt Ressource { get; set; }

        /// <summary>
        /// Passerelle vers l'accesseur [] du dictionnaire de valeurs. 
        /// Si pour une raison ou pou une autre, la colonne demandée n'est pas renseignée dans le dictionnaire,
        /// Alors la valeur null est retournée
        /// </summary>
        /// <param name="columnName">nom de la colonne a lire</param>
        /// <returns>return l'objet contenu dans la colonne donnée</returns>
        /// <exception cref="FredBusinessException">Si la colonne n'a pas été déclarée avec la fonction AddColumn le getter lancera une exception</exception>
        public dynamic this[string columnName]
        {
            get
            {
                dynamic value = null;
                columnNameToValueDictionary.TryGetValue(columnName, out value);
                return value;
            }
            set
            {
                columnNameToValueDictionary[columnName] = value;
            }
        }
    }
}
