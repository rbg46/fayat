using Fred.Entities;

namespace Fred.Business.Common
{
    /// <summary>
    /// Classe utilitaire pour gestion des proprietes du personnel
    /// </summary>
    public static class PersonnelUtils
    {
        /// <summary>
        ///   Retourne le numéro de colonne en fonction du statut du personnel
        /// </summary>
        /// <param name="statutId">type statut</param>
        /// <returns>Le numéro de colonne</returns>
        public static string GetStatutCodeFromStatutId(string statutId)
        {
            switch (statutId)
            {
                case "1":
                    return "O";
                case "2":
                case "4":
                case "5":
                    return "E";
                case "3":
                    return "C";
                default:
                    return null;
            }
        }

        /// <summary>
        /// Get personnel statut by statut id
        /// </summary>
        /// <param name="statutId">Statut identifier</param>
        /// <returns>Statut</returns>
        public static string GetStatutLibelleFromStatutId(string statutId)
        {
            switch (statutId)
            {
                case "1":
                    return Constantes.PersonnelStatutValue.Ouvrier;
                case "2":
                case "4":
                case "5":
                    return Constantes.PersonnelStatutValue.ETAM;
                case "3":
                    return Constantes.PersonnelStatutValue.Cadre;
                default:
                    return string.Empty;
            }
        }
    }
}
