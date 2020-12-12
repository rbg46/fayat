using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Entities.Rapport;
using Fred.Entities.Referential;
using Fred.ImportExport.Business.ValidationPointage.Fes.ControleVrac.Analyzers;

namespace Fred.ImportExport.Business.ValidationPointage.Fes
{
    /// <summary>
    ///    Recuperes les infos pour les astreintes de fes
    /// 
    /// </summary>
    public class AstreinteAnalyser
    {

        public List<AstreinteResult> GetAstreinteInfos(List<RapportLigneEnt> rapportLignes)
        {
            var result = new List<AstreinteResult>();

            foreach (var rapportLigne in rapportLignes)
            {
                var sortiesAstreintes = rapportLigne.ListCodePrimeAstreintes.Where(rlca => PersonnelHasSortieAstreinte(rlca));

                var sortieAstreinteGroups = sortiesAstreintes.GroupBy(rlca => rlca.CodeAstreinte.Code);

                foreach (var sortieAstreinteGroup in sortieAstreinteGroups)
                {
                    var totalHours = GetTotalHoursSortieAstreinte(sortieAstreinteGroup.ToList());
                    var astreinteResult = new AstreinteResult();
                    astreinteResult.HasAstreinte = true;
                    astreinteResult.HasSortieAstreinte = true;
                    astreinteResult.Quantite = !totalHours.Equals(0.0) ? totalHours.ToString().Replace(",", ".") : "0.0";
                    astreinteResult.RapportLigne = rapportLigne;
                    astreinteResult.Code = sortieAstreinteGroup.Key;
                    result.Add(astreinteResult);
                }
            }
            return result;
        }

        private double GetTotalHoursSortieAstreinte(List<RapportLigneCodeAstreinteEnt> rapportLigneCodeAstreintes)
        {

            var listAstreinte = rapportLigneCodeAstreintes.Select(rlca => rlca.RapportLigneAstreinte).ToList();
            TimeSpan total = default(TimeSpan);
            foreach (var item in listAstreinte)
            {
                total += item.DateFinAstreinte - item.DateDebutAstreinte;
            }
            return total.TotalHours;
        }

        private bool PersonnelHasSortieAstreinte(RapportLigneCodeAstreinteEnt rapportLigneCodeAstreinte)
        {
            return rapportLigneCodeAstreinte.RapportLigneAstreinte != null;
        }

    }
}
