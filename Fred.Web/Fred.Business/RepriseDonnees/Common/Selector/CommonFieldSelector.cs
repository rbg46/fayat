using Fred.Business.Organisation.Tree;
using Fred.Entites.RepriseDonnees.Commande;
using Fred.Entities.CI;
using Fred.Entities.Organisation.Tree;
using Fred.Entities.Referential;
using Fred.Framework.Extensions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Fred.Business.RepriseDonnees.Common.Selector
{
    /// <summary>
    /// Fait la selection d'element
    /// </summary>
    public class CommonFieldSelector
    {

        /// <summary>
        /// Recupere un ci en fonction du groupeId, de code de la societe et du code du CI. Puis selectionne le Ci dans la liste des CIs passé en parametre
        /// </summary>
        /// <param name="organisationTree">organisationTree</param>
        /// <param name="groupeId">groupeId</param>
        /// <param name="codeSociete"> code de la societe</param>
        /// <param name="codeCi">code du CI</param>
        /// <param name="cis">la liste des CIs ou l'on fait la selection du CIEnt</param>
        /// <returns>Un CIEnt</returns>
        public CIEnt GetCiOfDatabase(OrganisationTree organisationTree, int groupeId, string codeSociete, string codeCi, List<CIEnt> cis)
        {
            OrganisationBase ci = organisationTree.GetCi(groupeId, codeSociete, codeCi);

            if (ci == null)
            {
                return null;
            }

            return cis.FirstOrDefault(x => x.CiId == ci.Id);
        }

        /// <summary>
        /// Transforme une chaine en date
        /// </summary>
        /// <param name="date">la date type chaine de caractere</param>
        /// <returns>la date type DateTime</returns>
        public DateTime GetDate(string date)
        {
            return DateTime.ParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// ecupere la tache a partie de sont code en fonction de plusieurs parametres.
        /// </summary>
        /// <param name="ciId">ciId</param>
        /// <param name="codeTache">codeTache</param>
        /// <param name="tachesResponseUsedInExcel">tachesResponseUsedInExcel</param>
        /// <returns>une tache</returns>
        public TacheEnt GetTache(int? ciId, string codeTache, List<GetT3ByCodesOrDefaultResponse> tachesResponseUsedInExcel)
        {
            TacheEnt result = null;


            if (ciId != null)
            {
                if (!codeTache.IsNullOrEmpty())
                {
                    result = tachesResponseUsedInExcel.FirstOrDefault(x => string.Equals(x.Code, codeTache, StringComparison.OrdinalIgnoreCase) && x.CiId == ciId.Value)?.Tache;
                }
                else
                {
                    result = tachesResponseUsedInExcel.FirstOrDefault(x => x.CiId == ciId.Value && x.Tache != null && x.Tache.TacheParDefaut)?.Tache;
                }
            }
            return result;
        }

        /// <summary>
        /// Retourne un decimal a partir d'une chaine de charctere
        /// </summary>
        /// <param name="value">la chaine</param>
        /// <returns>la valeur decimal</returns>
        public decimal GetDecimal(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return 0;
            }
            var style = NumberStyles.Number;
            var culture = CultureInfo.CreateSpecificCulture("fr-FR");
            return decimal.Parse(value, style, culture);
        }

        /// <summary>
        /// Retourne  true si "O" ou false si "N"
        /// </summary>
        /// <param name="input">représente le choix</param>
        /// <returns>la valeur chaine converti en boolean </returns>
        public bool GetBoolean(string input)
        {
            input = input.Trim().ToUpper();

            if (input == "O")
            {
                return true;
            }
            if (input == "N")
            {
                return false;
            }
            if (input.IsNullOrEmpty())
            {
                return false;
            }
            return false;
        }

        /// <summary>
        /// Retourne la valeur chaine converti en double
        /// </summary>
        /// <param name="input">valeur double </param>
        /// <returns>la valeur chaine converti en double </returns>
        public double GetDouble(string input)
        {
            double output;
            var intFormatIsValid = double.TryParse(input, out output);
            if (intFormatIsValid)
            {
                return output;
            }
            else
            {
                return default(double);
            }

        }
    }
}
