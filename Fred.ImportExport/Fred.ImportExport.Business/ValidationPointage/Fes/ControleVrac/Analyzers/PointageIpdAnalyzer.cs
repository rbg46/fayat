using System.Collections.Generic;
using System.Linq;
using Fred.Entities.Rapport;

namespace Fred.ImportExport.Business.ValidationPointage.Fes.ControleVrac.Analyzers
{


    /// <summary>
    ///   Dans le cadre du périmètre FRED FES, le calcul des IPD s’applique automatiquement en définissant une zone(entre 1 et 6 actuellement)
    ///   à la suite d’un pointage sur un CI.Avec le fonctionnement actuel, pour chaque CI pointé, une zone IPD pourrait y être attachée.
    ///   L’interface impactée lors de l’envoi des pointages d’heures travaillées est INTEI.SRAPFREP.
    ///     Pour FES, l’indemnité IPD est unique pour un personnel et une journée pointée.Dans le cas où pour une journée pointée,
    ///     il existe différentes zone IPD, il faudra redescendre en paie, l’indemnité zone la plus grande.Dans le modèle de donnée FRED, dans la table FRED_RAPPORT_LIGNE,
    ///     les indemnités IPD sont enregistrées dans les champs FRED_RAPPORT_LIGNE.CodeDeplacementId(via la jointure dans la table FRED_CODE_DEPLACEMENT) 
    ///     et FRED_RAPPORT_LIGNE.CodeZoneDeplacementId(via la jointure dans la table FRED_CODE_ZONE_DEPLACEMENT).
    /// Le calcul des IPD dans FRED se faisant sur tous les CI pointés, la règle qui s’applique à la gestion des IPD est la suivante :
    /// • Si pour un personnel et pour une journée pointée, un seul CI est affecté alors pas de conditions particulières.
    /// • Dans le cas d’un pointage sur plusieurs CI, il ne faut envoyer en paye qu’une seule IPD et donc prendre le max du champ FRED_CODE_ZONE_DEPLACEMENT.Code.
    /// Pour les autres CI pointés dans la même journée, le code déplacement ainsi que le code zone déplacement devront être vide.
    /// </summary>
    public class PointageIpdAnalyzer
    {

        public List<PointageIpdResult> GetPointageIpdInfos(List<RapportLigneEnt> rapportLignes)
        {
            var ipdResults = GetIpdResults(rapportLignes);

            return ipdResults;
        }

        public string GetBiggestCodeDeplacement(List<RapportLigneEnt> rapportLignes, RapportLigneEnt rapportLigne)
        {

            if (MustApplyIpdRules(rapportLignes, rapportLigne))
            {
                var groupedByPersonnelAndDay = GetGroupedByPersonelAndDay(rapportLignes, rapportLigne);
                var biggestIpd = GetTheStrongestIpd(groupedByPersonnelAndDay);
                if (biggestIpd != null)
                {
                    return biggestIpd.CodeDeplacement.Code;
                }
                else
                {
                    throw new Fred.Framework.Exceptions.FredBusinessException("Cas normalement impossible. Car si on doit appliquer les regles ipd, il y a forcement un ipd qui est le plus fort.");
                }
            }
            else
            {
                return rapportLigne.CodeDeplacement != null ? rapportLigne.CodeDeplacement.Code : "00";
            }
        }

        public bool MustApplyIpdRules(List<RapportLigneEnt> rapportLignes, RapportLigneEnt rapportLigne)
        {
            if (!RapportLigneIsIpd(rapportLigne))
            {
                return false;
            }

            var groupedByPersonnelAndDay = GetGroupedByPersonelAndDay(rapportLignes, rapportLigne);

            return groupedByPersonnelAndDay.Count > 1;
        }

        private List<RapportLigneEnt> GetGroupedByPersonelAndDay(List<RapportLigneEnt> rapportLignes, RapportLigneEnt rapportLigne)
        {
            var ipdRapportLignes = rapportLignes.Where(rl => RapportLigneIsIpd(rl)).ToList();

            var groupedByPersonnelAndDay = ipdRapportLignes.GroupBy(rl => new { rl.DatePointage.Year, rl.DatePointage.Month, rl.DatePointage.Day, rl.PersonnelId }).ToList();

            var group = groupedByPersonnelAndDay
                .FirstOrDefault(g => g.Key.PersonnelId == rapportLigne.PersonnelId &&
                                    rapportLigne.DatePointage.Year == g.Key.Year &&
                                    rapportLigne.DatePointage.Month == g.Key.Month &&
                                    rapportLigne.DatePointage.Day == g.Key.Day);

            return group.ToList();
        }

        public List<PointageIpdResult> GetIpdResults(List<RapportLigneEnt> rapportLignes)
        {
            var result = new List<PointageIpdResult>();

            var rapportsLignesWithMiltipleIpd = rapportLignes.Where(rl => MustApplyIpdRules(rapportLignes, rl)).ToList();

            var groupeRapportLignes = rapportsLignesWithMiltipleIpd.GroupBy(rl => new { rl.DatePointage.Year, rl.DatePointage.Month, rl.DatePointage.Day, rl.PersonnelId });

            foreach (var group in groupeRapportLignes)
            {
                var ipdPointages = group.ToList();
                var pointageResults = SetIpdRules(ipdPointages);
                result.AddRange(pointageResults);
            }
            return result;
        }

        private List<PointageIpdResult> SetIpdRules(List<RapportLigneEnt> ipdPointages)
        {
            var result = new List<PointageIpdResult>();
            var rapportLigneWithBiggestIpd = GetTheStrongestIpd(ipdPointages);
            var biggestIpdResult = new PointageIpdResult()
            {
                IsBigestIpd = true,
                RapportLigne = rapportLigneWithBiggestIpd
            };
            result.Add(biggestIpdResult);
            var rapportLigneWithWeakestIpds = GetTheWeakestIpd(ipdPointages, rapportLigneWithBiggestIpd);
            foreach (var rapportLigneWithWeakestIpd in rapportLigneWithWeakestIpds)
            {
                var theWeakestIpdResult = new PointageIpdResult()
                {
                    IsBigestIpd = false,
                    RapportLigne = rapportLigneWithWeakestIpd
                };
                result.Add(theWeakestIpdResult);
            }
            return result;
        }


        private bool RapportLigneIsIpd(RapportLigneEnt rapportLigne)
        {
            if (rapportLigne.CodeDeplacement == null)
            {
                return false;
            }
            return !rapportLigne.CodeDeplacement.IGD;
        }

        private RapportLigneEnt GetTheStrongestIpd(IEnumerable<RapportLigneEnt> rapportLignes)
        {
            var codeDeplacements = rapportLignes
               .Where(l => l.CodeDeplacement != null)
               .Select(l => l.CodeDeplacement)
               .GroupBy(cd => cd.CodeDeplacementId)
               .Select(g => g.First())
               .OrderByDescending(l => l.KmMini);

            var biggestCodeDeplacement = codeDeplacements.FirstOrDefault();

            return rapportLignes.FirstOrDefault(rl => rl.CodeDeplacementId == biggestCodeDeplacement.CodeDeplacementId);
        }

        private List<RapportLigneEnt> GetTheWeakestIpd(IEnumerable<RapportLigneEnt> rapportLignes, RapportLigneEnt theStrongestIpd)
        {
            return rapportLignes.Except(new List<RapportLigneEnt> { theStrongestIpd }).ToList();
        }

    }
}
