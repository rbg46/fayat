using System;
using System.Globalization;
using System.Linq;
using Fred.Business.Organisation.Tree;
using Fred.Entities.Organisation.Tree;

namespace Fred.Business.RepriseDonnees.Common.Validation
{
    /// <summary>
    /// Verificateur de certain champ/colonne des fichier d'imports
    /// </summary>
    public class CommonFieldsValidator
    {

        /// <summary>
        /// Determine si la code societe est valide pour un groupeId donné
        /// </summary>
        /// <param name="groupeId">groupeId</param>
        /// <param name="codeSociete">codeSociete</param>
        /// <param name="organisationTree">organisationTree</param>
        /// <returns>vrai si une societe du groupe a ce code</returns>
        public bool CodeSocieteIsValid(int groupeId, string codeSociete, OrganisationTree organisationTree)
        {
            var societesOfGroupe = organisationTree.GetAllSocietesForGroupe(groupeId);

            return societesOfGroupe.Select(x => x.Code).ToList().Contains(codeSociete);

        }

        /// <summary>
        ///  Determine si la code CI est valide pour un groupeId donné et un code societe 
        /// </summary>
        /// <param name="groupeId">groupeId</param>
        /// <param name="codeSociete">codeSociete</param>
        /// <param name="codeCi">codeCi</param>
        /// <param name="organisationTree">organisationTree</param>
        /// <returns>vrai si un appartient a la societe et au groupe</returns>
        public bool CodeCiIsValid(int groupeId, string codeSociete, string codeCi, OrganisationTree organisationTree)
        {
            var result = false;

            var societesOfGroupe = organisationTree.GetAllSocietesForGroupe(groupeId);

            var societe = societesOfGroupe.FirstOrDefault(s => s.Code == codeSociete);

            if (societe != null)
            {
                var cisOfSociete = organisationTree.GetAllCisOfSociete(societe.Id);

                result = cisOfSociete.Select(x => x.Code).ToList().Contains(codeCi);
            }

            return result;
        }

        /// <summary>
        /// Determine si la date est valide
        /// </summary>
        /// <param name="date">la date</param>
        /// <returns>true si la date est bien parsée</returns>
        public bool DateIsValid(string date)
        {
            DateTime outputDate;

            if (DateTime.TryParseExact(date, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out outputDate))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Permet de savoir si une chaine de charactere est convertible en decimal
        /// </summary>
        /// <param name="value">une chaine de charactere</param>
        /// <returns>vari si convertible en decimal</returns>
        public bool DecimalIsValid(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return true;
            }

            decimal temp;

            var style = NumberStyles.Number;
            var culture = CultureInfo.CreateSpecificCulture("fr-FR");
            if (decimal.TryParse(value, style, culture, out temp))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
