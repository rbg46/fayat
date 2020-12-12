using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fred.Business.Budget.Helpers
{
    /// <summary>
    /// Classe static proposant des fonctions permettant de manipuler les numéros de version du budget
    /// </summary>
    public static class VersionHelper
    {

        /// <summary>
        /// Incrémente le numéro de version mineure d'un budget
        /// </summary>
        /// <param name="version">Numéro de version à incrémenter e.g 1.0 ou 2.34</param>
        /// <returns>Retourne le nnuméro de version incrémenté e.g 1.1 ou 2.35</returns>
        public static string IncrementVersionMineur(string version)
        {
            var versionIncrementee = string.Empty;
            if (!string.IsNullOrEmpty(version) && version.Contains("."))
            {
                var indexOfVersionOfPoint = version.IndexOf('.');
                var versionMajeureActuelle = int.Parse(version.Substring(0, indexOfVersionOfPoint));
                var versionMineureActuelle = int.Parse(version.Substring(indexOfVersionOfPoint + 1));
                versionIncrementee = versionMajeureActuelle + "." + (versionMineureActuelle + 1).ToString();
            }
            return versionIncrementee;
        }


        /// <summary>
        /// incrémente le numéro de version majeure d'un budget et remets la version mineure à 0
        /// </summary>
        /// <param name="version">la version à mettre à jour</param>
        /// <returns>La nouvelle version</returns>
        public static string IncrementVersionMajeure(string version)
        {
            var versionIncrementee = string.Empty;
            if (!string.IsNullOrEmpty(version) && version.Contains("."))
            {
                var versionMajeureActuelle = int.Parse(version.Substring(0, version.IndexOf('.')));
                versionIncrementee = (versionMajeureActuelle + 1).ToString() + ".0";
            }
            return versionIncrementee;
        }
    }
}
