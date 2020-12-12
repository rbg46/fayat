using System.Collections.Generic;
using System.Linq;
using Fred.Entities.Rapport;
using Fred.Entities.Referential;
using Fred.Entities.ValidationPointage;

namespace Fred.ImportExport.Business.ValidationPointage.Fes.Common
{
    public class ValidationPointageFesFilter
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
                   .Where(x => !x.DateSuppression.HasValue && x.Personnel != null && x.Personnel.IsInterne
                               && ((filtre.TakeMatricule && filtre.Matricule != null) || filtre.SocieteId > 0 && x.Personnel.SocieteId == filtre.SocieteId)
                               && (!filtre.EtablissementPaieIdList.Any() || (x.Personnel.EtablissementPaieId.HasValue && filtre.EtablissementPaieIdList.Contains(x.Personnel.EtablissementPaieId.Value)))
                               && ((!filtre.TakeMatricule || filtre.Matricule == null) || x.Personnel.Matricule == filtre.Matricule)
                               && (!filtre.StatutPersonnelList.Any() || filtre.StatutPersonnelList.Contains(x.Personnel.Statut))
                               && x.Personnel.Societe != null && x.Personnel.Societe.Groupe != null && x.Personnel.Societe.Groupe.Code == Fred.Entities.Constantes.CodeGroupeFES)
                   .ToList();
        }

        /// <summary>
        ///     Filtre les primes
        /// </summary>
        /// <param name="rapportLigneList">Liste de ligne de rapport</param>      
        public void ApplyRapportLignePrimesFilter(IEnumerable<RapportLigneEnt> rapportLigneList)
        {
            foreach (var rapportLigne in rapportLigneList)
            {
                rapportLigne.ListRapportLignePrimes = ApplyRapportLignePrimeFilter(rapportLigne.ListRapportLignePrimes).ToList();
            }
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

        /// <summary>
        /// Filtre les rapportligne qui ne sont pas verrouillé, seul les RappportLignes verroulliées seront envoyées
        /// </summary>
        /// <param name="rapportLigneList">Liste de rapport lignes</param>
        /// <returns>Liste de rapport lignes verrouillées</returns>
        public IEnumerable<RapportLigneEnt> ApplyRapportLigneVerrouilleFilter(IEnumerable<RapportLigneEnt> rapportLigneList)
        {
            return rapportLigneList
                  .Where(x => x.LotPointageId != null)
                  .ToList();
        }
    }
}
