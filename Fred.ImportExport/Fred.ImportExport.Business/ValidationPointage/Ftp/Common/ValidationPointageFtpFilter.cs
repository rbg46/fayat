using System.Collections.Generic;
using System.Linq;
using Fred.Entities.Rapport;
using Fred.Entities.Referential;
using Fred.Entities.ValidationPointage;

namespace Fred.ImportExport.Business.ValidationPointage.Ftp.Common
{
    public class ValidationPointageFtpFilter
    {

        /// <summary>
        ///   Applique le filtre sur les lignes de rapports (pointages) de FRED
        /// </summary>
        /// <param name="rapportLigneList">Liste des lignes de rapport</param>
        /// <param name="filtre">Filtre</param>
        /// <returns>Liste triée</returns>
        public IEnumerable<RapportLigneEnt> ApplyRapportLigneFilter(IEnumerable<RapportLigneEnt> rapportLigneList, PointageFiltre filtre)
        {
            return rapportLigneList
                   .Where(x => (!x.DateSuppression.HasValue && x.Personnel != null && x.Personnel.IsInterne)
                               && ((filtre.TakeMatricule && filtre.Matricule != null) || filtre.SocieteId > 0 && x.Personnel.SocieteId == filtre.SocieteId)
                               && (!filtre.EtablissementPaieIdList.Any() || (x.Personnel.EtablissementPaieId.HasValue && filtre.EtablissementPaieIdList.Contains(x.Personnel.EtablissementPaieId.Value)))
                               && ((!filtre.TakeMatricule || filtre.Matricule == null) || x.Personnel.Matricule == filtre.Matricule)
                               && x.Personnel.Societe != null && x.Personnel.Societe.Groupe != null && x.Personnel.Societe.Groupe.Code == Fred.Entities.Constantes.CodeGroupeFTP)
                   .ToList();
        }

        /// <summary>
        ///     Filtre les primes
        /// </summary>
        /// <param name="rapportLigneList">Liste de ligne de rapport</param>   
        /// <returns>Liste triée</returns>
        public IEnumerable<RapportLigneEnt> ApplyRapportLignePrimesFilter(IEnumerable<RapportLigneEnt> rapportLigneList)
        {
            foreach (var rapportLigne in rapportLigneList)
            {
                rapportLigne.ListRapportLignePrimes = ApplyRapportLignePrimeFilter(rapportLigne.ListRapportLignePrimes).ToList();
            }

            return rapportLigneList;
        }

        /// <summary>
        ///     Filtre les primes
        /// </summary>
        /// <param name="rapportLignePrimes">Liste de ligne rapport/prime</param>
        /// <returns>Liste de rapport ligne prime</returns>
        private IEnumerable<RapportLignePrimeEnt> ApplyRapportLignePrimeFilter(IEnumerable<RapportLignePrimeEnt> rapportLignePrimes)
        {
            if (rapportLignePrimes == null)
            {
                return new List<RapportLignePrimeEnt>();
            }

            return rapportLignePrimes.Where(x => (x.IsChecked && x.Prime.PrimeType == ListePrimeType.PrimeTypeJournaliere)
                                                 || (x.HeurePrime > 0 && x.Prime.PrimeType == ListePrimeType.PrimeTypeHoraire)
                                                 || (x.Prime.PrimeType == ListePrimeType.PrimeTypeMensuelle));
        }
    }
}
