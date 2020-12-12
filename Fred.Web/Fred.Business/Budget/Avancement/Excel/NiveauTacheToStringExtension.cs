using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fred.Framework.Exceptions;
using Fred.Web.Shared.Models.Budget;

namespace Fred.Business.Budget.Avancement.Excel
{
    /// <summary>
    /// Propose des fonctions permettant de manipuler l'enumération des niveaux de taches 
    /// pour faciliter leur gestion pendantl la génération de l'export excel de l'avancement
    /// </summary>
    public static class NiveauTacheToStringExtension
    {
        /// <summary>
        /// Fonction permettant de passer d'un niveau de tache à sa représentation sous forme de chaine de caractère 
        /// telle qu'attendue et utilisée par le fichier excel
        /// </summary>
        /// <param name="niveau">le niveau de tache T1, T2, T3, T4</param>
        /// <returns>une chaine de caractère</returns>
        /// <exception cref="FredBusinessException">Cette exception est levée si le niveau de tache donné n'est pas connu de la fonction</exception>
        public static string ToFriendlyName(this NiveauxTaches niveau)
        { 
            switch (niveau)
            {
                case NiveauxTaches.T1:
                    return "T1";

                case NiveauxTaches.T2:
                    return "T2";

                case NiveauxTaches.T3:
                    return "T3";

                case NiveauxTaches.T4:
                    return "T4";
                default:
                    throw new FredBusinessException("Niveau de tache inconnu");
            }
        }
    }
}
