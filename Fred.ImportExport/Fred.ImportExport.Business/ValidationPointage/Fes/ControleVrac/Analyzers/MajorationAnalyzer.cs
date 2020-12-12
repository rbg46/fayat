using System.Collections.Generic;
using Fred.Entities.Rapport;

namespace Fred.ImportExport.Business.ValidationPointage.Fes.ControleVrac.Analyzers
{
    /// <summary>
    /// La gestion des majorations dans le périmètre de FES suit le même processus que les primes et les astreintes.
    /// A la différence des primes les majorations sont toujours de type horaire. 
    ///    Lors du pointage d’un personnel, plusieurs majorations peuvent être pointées pour une journée.
    ///    L’interface impactée lors de l’envoi des pointages d’heures travaillées est INTEI.PRIMNEWR.
    ///   Les majorations associées à un rapport de pointage peuvent être remontées grâce à la
    ///   jointure FRED_RAPPORT_LIGNE.RapportLigneId = FRED_RAPPORT_LIGNE_MAJORATION.RapportLigneId.
    ///   Le code de la majoration peut être identifié grâce au champ FRED_CODE_MAJORATION.Code.
    ///   Dans l’interface ANAEL, le type de majoration(champ PRUNI dans l’interface INTEI.PRIMNEWR) sera égale à « Heures » et la quantité = FRED_RAPPORT_LIGNE_MAJORATION.HeureMajoration.
    /// </summary>
    public class MajorationAnalyzer
    {

        public List<MajorationResult> GetMajorationInfos(List<RapportLigneEnt> rapportLignes)
        {
            var astreinteResults = GetMajorationResults(rapportLignes);

            return astreinteResults;
        }

        private List<MajorationResult> GetMajorationResults(List<RapportLigneEnt> rapportLignes)
        {
            var result = new List<MajorationResult>();

            foreach (var rapportLigne in rapportLignes)
            {
                foreach (var rapportLigneMajoration in rapportLigne.ListRapportLigneMajorations)
                {
                    var majorationResult = GetMajorationResult(rapportLigneMajoration);
                    result.Add(majorationResult);
                }
            }
            return result;
        }


        private MajorationResult GetMajorationResult(RapportLigneMajorationEnt rapportLigneMajoration)
        {
            var result = new MajorationResult();
            result.RapportLigneMajoration = rapportLigneMajoration;
            result.RapportLigne = rapportLigneMajoration.RapportLigne;
            result.Code = GetCode(rapportLigneMajoration);
            result.Quantite = GetQuantite(rapportLigneMajoration);
            return result;
        }


        private string GetCode(RapportLigneMajorationEnt rapportLigneMajoration)
        {
            var result = rapportLigneMajoration?.CodeMajoration.Code;

            return result == null ? string.Empty : result;
        }

        private string GetQuantite(RapportLigneMajorationEnt rapportLigneMajoration)
        {
            var result = rapportLigneMajoration?.HeureMajoration;

            return result.Equals(0) ? "0.0" : result.ToString().Replace(",", ".");
        }
    }
}
