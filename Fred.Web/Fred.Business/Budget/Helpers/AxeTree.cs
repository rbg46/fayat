using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace Fred.Business.Budget.Helpers
{

    /// <summary>
    /// Model de l'arbre
    /// Améliorer ça en s'inspirant de LINQ et et de AutoMapper pour que ce soit fortement typé. (Dès que j'ai le temps)
    /// </summary>
    public class AxeTreeModel
    {
        /// <summary>
        /// Représente la liste des valeurs (le 1er élément étant la colonne 0, le 2ème la colonne 1...)
        /// </summary>
        public Dictionary<string, dynamic> Valeurs { get; set; } = new Dictionary<string, dynamic>();

        /// <summary>
        /// Libellé de la ligne de l'axe
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Code de la ligne
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Id de la T3 associée à cette branche (ou feuille) de l'arbre.
        /// Cette variable n'est rempli que si  AxeType vaut T3 ou Ressource. 
        /// </summary>
        public int TacheId { get; set; }

        /// <summary>
        /// Id de la Ressource associée à cette branche (ou feuille) de l'arbre
        /// Cette variable n'est rempli que si  AxeType vaut T3 ou Ressource. 
        /// </summary>
        public int RessourceId { get; set; }

        /// <summary>
        /// La liste des branches de cette axe
        /// </summary>
        public ICollection<AxeTreeModel> SousAxe { get; set; }

        /// <summary>
        /// Le type de l'axe T1 ou T2 ou T3, Chapitre ....
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public AxeTypes AxeType { get; set; }

        /// <summary>
        /// Représente les données à plats (non ordonnées) utilisées pour l'arbre 
        /// </summary>
        [JsonIgnore]
        public IEnumerable<AxeTreeDataSourceRow> Source { get; set; }

    }
}
