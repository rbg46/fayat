using System;
using Fred.Entities.ValidationPointage;
using Fred.ImportExport.Business.Anael;
using Fred.ImportExport.Business.ValidationPointage.Common;
using Fred.ImportExport.Business.ValidationPointage.Fes.ControleVrac.Interfaces;

namespace Fred.ImportExport.Business.ValidationPointage.Fes.ControleVrac
{
    public class ControleVracFesQueryBuilder : IControleVracFesQueryBuilder
    {
        private const string CallControleVracQuery = "CALL INTEI.VRAFR29C ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}')";
        private const string SelectControleVracErreurQuery = "SELECT EDTETB, EDTAFF, EDTMAT, EDTNOM, EDTPRE, EDTAAP, EDTMMP, EDTJJP, EDTERR FROM INTEI.ERR_{0}";

        private readonly IEtablissementFormator etablissementFormator;

        public ControleVracFesQueryBuilder(IEtablissementFormator etablissementFormator)
        {
            this.etablissementFormator = etablissementFormator;
        }

        public string GetControleVracErreurQuery(string nomUtilisateur)
        {
            string tableErreurPrefixe = (nomUtilisateur.Length > 6) ? nomUtilisateur.Substring(0, 6) : nomUtilisateur;
            return string.Format(SelectControleVracErreurQuery, tableErreurPrefixe);
        }

        public string BuildControleVracQuery(ValidationPointageContextData globalData, DateTime periode, PointageFiltre filtre)
        {
            var etsCodeList = etablissementFormator.ConcatEtablissementPaieCode(filtre.EtablissementPaieIdList);
            var localPeriode = periode.ToLocalTime();
            var nomUtilisateur = (globalData.NomUtilisateur.Length > 10) ? globalData.NomUtilisateur.Substring(0, 10) : globalData.NomUtilisateur;

            // jobId>Identifiant du job hangfire en cours : utilisé pour avoir un identifiant unique lors du contrôle vrac. 
            // Contrainte du programme INTRB.VRA0029C : l'identifiant du lot doit être unique à chaque exécution. 
            // Or dans FRED, le lot de pointage (LotPointageId) d'un utilisateur est unique pour un mois donné et ne change jamais.
            // Donc ici, on utilisera le jobId de Hangfire (qui est différent à chaque exécution) comme paramètre du programme AS400.
            var jobId = globalData.JobId.PadLeft(10, ' ');

            return string.Format(CallControleVracQuery,
                           globalData.CodeSocietePaye,
                           localPeriode.Month.ToString("00"),
                           localPeriode.Year,
                           globalData.CodeSocietePaye,
                           etsCodeList.PadRight(10, ' '),
                           nomUtilisateur.FormatUsername(),
                           jobId);
        }
    }
}
