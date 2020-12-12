using System.Collections.Generic;
using System.Linq;
using Fred.Entities.Rapport;
using Fred.ImportExport.Business.ValidationPointage.Fes.ControleVrac.Analyzers;

namespace Fred.ImportExport.Business.ValidationPointage.Fes.ControleVrac
{
    /// <summary>
    /// Dans le cadre du périmètre FRED FES, les CI sur lesquels le personnel effectue son pointage sont de 3 types :
    /// • Section
    /// • Affaire
    /// • Etude
    /// Le pointage des heures travaillées se fait sur un CI, dans FRED le rapport ligne correspond à une clé matricule/code CI/date de pointage.
    /// L’interface impactée lors de l’envoi des pointages d’heures travaillées est INTEI.SRAPFREP.
    /// Dans le cas d’un pointage sur un CI de type étude, ce n’est pas le code du CI qui devra être envoyé vers la paye mais le code de la tâche 
    /// qui correspond à un code étude (correspond à une tâche rattachée au CI dans FRED se trouvant dans le champ Code de la table FRED_TACHE).
    /// La règle qui s’applique sur le type de CI est la suivante :
    /// • Si le type de CI sur lequel le personnel a pointé est de type « étude » (FRED_CI.TypeCI = (SELECT CITypeId FROM FRED_CI_TYPE WHERE Code LIKE ‘E’))
    /// alors dans le flux de pointage vers la paie, ce n’est pas le code du CI qui sera envoyé mais le code de la tâche.
    /// La tâche liée au CI pointé pourra être retrouvée via la jointure FRED_RAPPORT_LIGNE.RapportLigneId = FRED_RAPPORT_LIGNE_TACHE.RapportLigneId.
    /// Le nombre d’heure normale remonté, proviendra également du nombre d’heures pointés au niveau de la tâche.
    /// o Dans le cas d’un pointage sur plusieurs tâches sur un CI de type études, il faudra envoyer un pointage sur chaque tâches, donc on aura un rapport ligne par tache.
    /// o Comme un rapport ligne sera envoyé autant de fois que de tâche différente, tous les attributs suivants ne sont à envoyer qu’une fois
    /// (code absence, code déplacement, voyage détente, heure absence, code zone déplacement, code majoration)
    /// • Sinon c’est le code du CI qui est envoyé à la paie
    /// 
    /// </summary>
    public class PointageTypeEtudeAnalyser
    {
        public bool MustApplyCiTypeEtudeRules(RapportLigneEnt pointage)
        {
            return pointage?.Ci?.CIType?.Code == "E";
        }

        public List<PointageTypeEtudeResult> GetPointageTypeEtudeInfos(List<RapportLigneEnt> rapportLignes)
        {
            var rapportLignesTypeEtudes = rapportLignes.Where(rl => MustApplyCiTypeEtudeRules(rl)).ToList();

            var astreinteResults = GetTypeEtudeResults(rapportLignesTypeEtudes);

            return astreinteResults;
        }

        private List<PointageTypeEtudeResult> GetTypeEtudeResults(List<RapportLigneEnt> rapportLignes)
        {
            var result = new List<PointageTypeEtudeResult>();


            foreach (var rapportLigne in rapportLignes)
            {
                var firstRapportLigneTache = rapportLigne.ListRapportLigneTaches.FirstOrDefault();
                if (firstRapportLigneTache != null)
                {
                    var pointageTypeEtudeResult = GetPointageTypeEtudeResult(firstRapportLigneTache, isFirst: true);
                    result.Add(pointageTypeEtudeResult);


                    var lastRapportLigneTaches = rapportLigne.ListRapportLigneTaches.Skip(1).ToList();
                    foreach (var lastRapportLigneTache in lastRapportLigneTaches)
                    {
                        var lastPointageTypeEtudeResult = GetPointageTypeEtudeResult(lastRapportLigneTache, isFirst: false);
                        result.Add(lastPointageTypeEtudeResult);
                    }
                }
                else if(rapportLigne.CodeAbsenceId.HasValue)
                {
                    result.Add(GetPointageTypeEtudeResultWithoutTache(rapportLigne));
                }
            }
            return result;
        }

        private PointageTypeEtudeResult GetPointageTypeEtudeResult(RapportLigneTacheEnt rapportLigneTache, bool isFirst)
        {
            var result = new PointageTypeEtudeResult();
            result.RapportLigneTache = rapportLigneTache;
            result.RapportLigne = rapportLigneTache.RapportLigne;
            result.Code = GetCode(rapportLigneTache);
            result.Quantite = GetQuantite(rapportLigneTache);
            result.IsFistResult = isFirst ? true : false;
            return result;
        }

        /// <summary>
        /// Get pointage type etude without tache
        /// </summary>
        /// <param name="rapportLigne">Rapport ligne</param>
        /// <returns>PointageTypeEtudeResult</returns>
        private PointageTypeEtudeResult GetPointageTypeEtudeResultWithoutTache(RapportLigneEnt rapportLigne)
        {
            var result = new PointageTypeEtudeResult();
            result.RapportLigne = rapportLigne;
            result.Code = rapportLigne.Ci?.Code;
            result.IsFistResult = true;
            result.Quantite = "0.0";
            return result;
        }

        private string GetCode(RapportLigneTacheEnt rapportLigneTache)
        {
            var result = rapportLigneTache?.Tache.Code;

            return result == null ? string.Empty : result;
        }

        private string GetQuantite(RapportLigneTacheEnt rapportLigneTache)
        {
            var result = rapportLigneTache?.HeureTache;

            return result.Equals(0) ? "0.0" : result.ToString().Replace(",", ".");
        }
    }
}
