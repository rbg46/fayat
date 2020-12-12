using System.Collections.Generic;
using System.Linq;
using Fred.Entities.Rapport;
using Fred.Entities.Referential;

namespace Fred.ImportExport.Business.ValidationPointage.Fes.ControleVrac.Analyzers
{
    /// <summary>
    /// Alimentation des données de prime(RG-002)
    ///     Dans le cadre du pointage d’une prime, il existe 2 types de prime :
    /// • Prime journalière  code prime et unique pour une journée de pointage
    /// • Prime horaire(non utilisé par FES pour le moment)  code prime associé à un nombre d’heure
    /// Lors du pointage d’un personnel, plusieurs primes peuvent être pointées pour une journée 
    /// mais selon le type de prime un même code prime ne peut être cumulable pour une personne pour une même journée.
    /// L’interface impactée lors de l’envoi des pointages d’heures travaillées est INTEI.PRIMNEWR.
    /// Les primes associées à un rapport de pointage peuvent être remontées grâce à la jointure FRED_RAPPORT_LIGNE.RapportLigneId = FRED_RAPPORT_LIGNE_PRIME.RapportLigneId.
    /// Le pointage des primes dans FRED pouvant être dupliqué sur des CI différents, la règle qui s’applique à la gestion des primes est la suivante :
    /// • Si le type de prime pointé est de type journalier, ce code prime ne peut être déversé en paie qu’une seule fois pour un personnel pour la journée pointée.
    /// Le type de prime peut être identifié grâce au champ FRED_PRIME.PrimeType = 0.Dans l’interface ANAEL pour ce type de prime l’unité sera égale à « Jours » et la quantité = 1.
    /// • Pour les autres types de prime, pas de conditions particulières.
    /// </summary>
    public class PrimeAnalyser
    {
        public List<PrimeResult> GetPrimesInfos(List<RapportLigneEnt> rapportLignes)
        {
            var result = new List<PrimeResult>();

            var allRapportLignePrimes = rapportLignes.SelectMany(rl => rl.ListRapportLignePrimes).ToList();

            var allPrimesFiltered = SelectOnlyOnePrimeTypeDayForOneDayOnePersonnel(allRapportLignePrimes);

            foreach (var rapportLignePrime in allPrimesFiltered.ToList())
            {
                var primeResult = GetPrimeResult(rapportLignePrime);

                result.Add(primeResult);
            }

            return result;
        }


        private List<RapportLignePrimeEnt> SelectOnlyOnePrimeTypeDayForOneDayOnePersonnel(List<RapportLignePrimeEnt> rapportLignePrimes)
        {
            var result = new List<RapportLignePrimeEnt>();

            foreach (var rapportLignePrime in rapportLignePrimes)
            {
                if (rapportLignePrime.Prime.PrimeType == ListePrimeType.PrimeTypeJournaliere)
                {
                    if (!AlreadyContainsOnePrimeTypeDayForOneDayOnePersonnel(result, rapportLignePrime))
                    {

                        result.Add(rapportLignePrime);
                    }
                }
                else
                {
                    result.Add(rapportLignePrime);
                }


            }

            return result;
        }

        private bool AlreadyContainsOnePrimeTypeDayForOneDayOnePersonnel(List<RapportLignePrimeEnt> rapportLignePrimes, RapportLignePrimeEnt rapportLignePrime)
        {
            return rapportLignePrimes.Any(rlp => rlp.RapportLigne.PersonnelId == rapportLignePrime.RapportLigne.PersonnelId &&
                                          rlp.RapportLigne.DatePointage.Year == rapportLignePrime.RapportLigne.DatePointage.Year &&
                                          rlp.RapportLigne.DatePointage.Month == rapportLignePrime.RapportLigne.DatePointage.Month &&
                                          rlp.RapportLigne.DatePointage.Day == rapportLignePrime.RapportLigne.DatePointage.Day &&
                                          rlp.Prime.Code == rapportLignePrime.Prime.Code);
        }

        private PrimeResult GetPrimeResult(RapportLignePrimeEnt rapportLignePrime)
        {
            var result = new PrimeResult();
            result.RapportLignePrime = rapportLignePrime;
            result.RapportLigne = rapportLignePrime.RapportLigne;
            result.Code = GetCode(rapportLignePrime);
            result.Libelle = GetLibelle(rapportLignePrime);
            result.Unite = GetUnite(rapportLignePrime);
            result.Quantite = GetQuantite(rapportLignePrime);
            return result;
        }


        private string GetCode(RapportLignePrimeEnt rapportLignePrime)
        {
            return rapportLignePrime.Prime.Code.Trim();
        }

        private string GetLibelle(RapportLignePrimeEnt rapportLignePrime)
        {
            return rapportLignePrime.Prime.Libelle;
        }

        private string GetQuantite(RapportLignePrimeEnt rapportLignePrime)
        {
            var result = string.Empty;
            switch (rapportLignePrime.Prime.PrimeType)
            {
                case ListePrimeType.PrimeTypeHoraire:
                    result = rapportLignePrime.HeurePrime.HasValue ? rapportLignePrime.HeurePrime.Value.ToString().Replace(",", ".") : "0.0";
                    break;
                case ListePrimeType.PrimeTypeJournaliere:
                    result = "1.0";
                    break;
                default:
                    result = "1.0";
                    break;
            }
            return result;
        }


        private string GetUnite(RapportLignePrimeEnt rapportLignePrime)
        {
            var result = string.Empty;
            switch (rapportLignePrime.Prime.PrimeType)
            {
                case ListePrimeType.PrimeTypeHoraire:
                    result = "Heures";
                    break;
                case ListePrimeType.PrimeTypeJournaliere:
                    result = "Jours";
                    break;
                default:
                    result = "Mois";
                    break;
            }
            return result;
        }

    }
}
