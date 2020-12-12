using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fred.Business.Budget.Avancement.Excel
{
    /// <summary>
    /// Définie les deux segments d'une ligne de l'export excel de l'avancement
    /// </summary>
    internal enum LineSection
    {
        /// <summary>
        /// La section Header comporte : le code de la tache, son libelle, un commentaire, l'unité, ainsi que les montants, quantité et pu budgétés
        /// </summary>
        Header,

        /// <summary>
        /// Le footer contient toutes les données relatives à l'avancement
        /// </summary>
        Footer
    }

}
