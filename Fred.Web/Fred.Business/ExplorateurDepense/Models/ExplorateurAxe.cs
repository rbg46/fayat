using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Fred.Entities.Referential;

namespace Fred.Business.ExplorateurDepense
{
    /// <summary>
    ///   Model axe d'exploration
    /// </summary>
    [Serializable]
    [DebuggerDisplay("Key={Key} Libelle={Libelle} Type={Type} MontantHT={MontantHT} Code={Code}")]

    public class ExplorateurAxe
    {
        public string Key { get; set; }

        /// <summary>
        /// Obtient ou définit le code de l'axe
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Obtient ou définit le libellé de l'axe
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Obtient ou définit le Montant HT
        /// </summary>
        public decimal MontantHT { get; set; }

        /// <summary>
        /// Obtient ou définit la devise
        /// </summary>
        public DeviseEnt Devise { get; set; }

        /// <summary>
        /// Obtient ou définit le sous axe d'exploration
        /// </summary>
        public IEnumerable<ExplorateurAxe> SousExplorateurAxe { get; set; }

        /// <summary>
        /// Obtient ou définit le type de l'axe
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Obtient ou définit la liste des dépenses de l'axe
        /// </summary>
        public IEnumerable<ExplorateurDepenseGeneriqueModel> Depenses { get; set; }

        /// <summary>
        /// Obtient ou définit tous les code tâches de niveau 3
        /// </summary>
        public IEnumerable<string> AllT3Code { get; set; }

        /// <summary>
        /// Obtient ou définit tous les codes ressources
        /// </summary>
        public IEnumerable<string> AllRessourceCode { get; set; }

        /// <summary>
        /// Obtient ou définit le code du parent (Chapitre)
        /// </summary>
        public string CodeChapitreParent { get; set; }

        public ExplorateurAxe Parent { get; set; }

        /// <summary>
        /// Vide tous les listes "Depenses" de chaque sous axe d'exploration
        /// </summary>   
        public void ClearDepenses()
        {
            Depenses = null;
            if (SousExplorateurAxe != null)
            {
                foreach (var se in SousExplorateurAxe.ToList())
                {
                    se.ClearDepenses();
                }
            }
        }

        public string GetIdentifier()
        {
            var parents = GetParents();

            parents.Reverse();

            var parentKeys = parents.Select(x => x.Key).ToList();

            var result = string.Join(" ", parentKeys) + " " + this.Key;

            return result.Trim();
        }

        public List<ExplorateurAxe> GetParents()
        {
            List<ExplorateurAxe> list = new List<ExplorateurAxe>();
            if (Parent != null)
            {
                list.Add(Parent);
                list.AddRange(Parent.GetParents());
            }
            return list;
        }
    }
}
