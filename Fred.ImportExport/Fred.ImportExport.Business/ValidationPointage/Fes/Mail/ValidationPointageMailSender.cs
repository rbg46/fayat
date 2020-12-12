using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fred.Entities.Rapport;
using Fred.Entities.Referential;
using Fred.ImportExport.Business.ValidationPointage.Common;
using Fred.ImportExport.Business.ValidationPointage.Fes.Common;
namespace Fred.ImportExport.Business.ValidationPointage.Fes.Mail
{
    public static class ValidationPointageMailSender
    {
        internal static void Send(ValidationPointageContextData globalData, IEnumerable<RapportLigneEnt> rapportLignes, string query, IEnumerable<QueryInfo> queries, Dictionary<string, string> recipients)
        {
            var body = new StringBuilder();
            body.Append($"<b>JobId : {globalData.JobId}</b><br /><br />");
            body.AddPointages(rapportLignes, globalData.SocieteId);
            body.Append("<br /><br />");
            body.AddPrimes(rapportLignes);
            body.Append("<br /><br />");
            body.AddCodeAbstreintes(rapportLignes);
            body.Append("<br /><br />");
            body.AddAbstreintes(rapportLignes);
            body.Append("<br /><br />");
            body.AppendRequetesAS400(query, queries);

            // Envoie du mail
            body.Send(globalData.JobId, recipients);
        }

        internal static void SendInsertQuery(ValidationPointageContextData globalData, string query, string pathPointageFile, string pathPrimeFile, Dictionary<string, string> recipients)
        {
            var body = new StringBuilder();
            body.Append($"<b>JobId : {globalData.JobId}</b><br /><br />");

            // Envoie du mail
            body.SendInsertQuery(pathPointageFile, pathPrimeFile, globalData.JobId, recipients);
        }

        /// <summary>
        /// Ajoute les pointages au corp du mail.
        /// </summary>
        /// <param name="body">Le corp du mail.</param>
        /// <param name="pointages">Les pointages à ajouter.</param>
        /// <param name="societeId">L'identifiant de la société concernée.</param>
        public static void AddPointages(this StringBuilder body, IEnumerable<RapportLigneEnt> pointages, int societeId)
        {
            body.Append($"<b>Liste des pointages envoyés vers ANAEL ({pointages.Count()} pointage(s))</b><br />");

            if (pointages.Any())
            {
                body.Append(@"<table border=""1""><tbody>");
                body.AppendPointageEnTete();

                foreach (var pointage in pointages.OrderBy(p => p.DatePointage))
                {
                    body.AppendPointageLigne(pointage);
                }

                body.Append("</tbody></table>");
            }
        }

        public static void AddPrimes(this StringBuilder body, IEnumerable<RapportLigneEnt> rapportLignes)
        {
            var primes = rapportLignes.SelectMany(rl => rl.ListRapportLignePrimes).ToList();

            body.Append($"<b>Liste des RapportLignePrime envoyés vers ANAEL ({primes.Count})</b><br />");

            if (primes.Count > 0)
            {
                body.Append(@"<table border=""1""><tbody>");
                body.AppendPrimeEnTete();

                foreach (var prime in primes.OrderBy(p => p.RapportLigne.DatePointage))
                {
                    body.AppendPrimeLigne(prime, prime.RapportLigne);
                }

                body.Append("</tbody></table>");
            }
        }

        public static void AddCodeAbstreintes(this StringBuilder body, IEnumerable<RapportLigneEnt> rapportLignes)
        {
            var rapportLigneCodeAstreintes = rapportLignes.SelectMany(rl => rl.ListCodePrimeAstreintes).ToList();

            body.Append($"<b>Liste des RapportLigneCodeAstreinte envoyés vers ANAEL ({rapportLigneCodeAstreintes.Count})</b><br />");

            if (rapportLigneCodeAstreintes.Count > 0)
            {
                body.Append(@"<table border=""1""><tbody>");
                body.AppendCodeAbstreinteEnTete();

                foreach (var rapportLigneCodeAstreinte in rapportLigneCodeAstreintes.OrderBy(p => p.RapportLigne.DatePointage))
                {
                    body.AddCodeAbstreinte(rapportLigneCodeAstreinte);
                }

                body.Append("</tbody></table>");
            }
        }

        public static void AppendCodeAbstreinteEnTete(this StringBuilder body)
        {
            body.Append(@"
        <tr>
          <td>Date</td>
          <td>RapportLigneId</td>
          <td>Mat-Nom-Prenom</td>
          <td>Code</td>
          <td>Description</td>
          <td>EstSorti</td>         
          <td>Source</td>
        </tr>");
        }

        public static void AddCodeAbstreinte(this StringBuilder body, RapportLigneCodeAstreinteEnt rapportLigneCodeAstreinte)
        {
            body.Append($@"
        <tr>
          <td>{rapportLigneCodeAstreinte.RapportLigne.DatePointage.ToShortDateString()}</td>
          <td>{rapportLigneCodeAstreinte.RapportLigne.RapportLigneId}</td>
          <td>{rapportLigneCodeAstreinte.RapportLigne.Personnel?.CodeNomPrenom}</td>
          <td>{rapportLigneCodeAstreinte?.CodeAstreinte?.Code}</td>
          <td>{rapportLigneCodeAstreinte?.CodeAstreinte?.Description}</td>
          <td>{rapportLigneCodeAstreinte?.CodeAstreinte?.EstSorti}</td>         
          <td>FRED</td>
        </tr>");
        }

        public static void AddAbstreintes(this StringBuilder body, IEnumerable<RapportLigneEnt> rapportLignes)
        {
            var rapportLigneAstreintes = rapportLignes.SelectMany(rl => rl.ListRapportLigneAstreintes).ToList();

            body.Append($"<b>Liste des RapportLigneAstreinte envoyés vers ANAEL ({rapportLigneAstreintes.Count})</b><br />");

            if (rapportLigneAstreintes.Count > 0)
            {
                body.Append(@"<table border=""1""><tbody>");
                body.AppendAbstreinteEnTete();

                foreach (var rapportLigneAstreinte in rapportLigneAstreintes.OrderBy(p => p.RapportLigne.DatePointage))
                {
                    body.AddAbstreinte(rapportLigneAstreinte);
                }

                body.Append("</tbody></table>");
            }
        }

        public static void AppendAbstreinteEnTete(this StringBuilder body)
        {
            body.Append(@"
        <tr>
          <td>Date</td>
          <td>RapportLigneId</td>
          <td>Mat-Nom-Prenom</td>
          <td>Debut</td>
          <td>Fin</td>               
          <td>Source</td>
        </tr>");
        }

        public static void AddAbstreinte(this StringBuilder body, RapportLigneAstreinteEnt rapportLigneCodeAstreinte)
        {
            body.Append($@"
        <tr>
          <td>{rapportLigneCodeAstreinte.RapportLigne.DatePointage.ToShortDateString()}</td>
          <td>{rapportLigneCodeAstreinte.RapportLigne.RapportLigneId}</td>
          <td>{rapportLigneCodeAstreinte.RapportLigne.Personnel?.CodeNomPrenom}</td>
          <td>{rapportLigneCodeAstreinte?.DateDebutAstreinte}</td>
          <td>{rapportLigneCodeAstreinte?.DateFinAstreinte}</td>         
          <td>FRED</td>
        </tr>");
        }

        /// <summary>
        /// Ajoute les requêtes envoyées à l'AS400.
        /// </summary>
        /// <param name="body">Le corp du mail.</param>
        /// <param name="procQuery">Requête procédure AS400</param>      
        /// <param name="queries">Les requêtes pour l'insertion des prime et pointages.</param>
        private static void AppendRequetesAS400(this StringBuilder body, string procQuery, IEnumerable<QueryInfo> queries)
        {
            body.Append("<b>Requêtes SQL AS400</b><br />");
            body.Append("<br />");
            body.Append("Procédure : ").Append(procQuery);
            body.Append("<br />");

            foreach (var query in queries)
            {
                if (!query.IsComment)
                {
                    body.Append(query.Query);
                    body.Append("<br/><br/>");
                }
                else
                {
                    body.Append("-- ---------------------------------------");
                    body.Append(query.Comment);
                    body.Append("<br/><br/>");
                }
            }
        }
    }
}
