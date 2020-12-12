using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fred.Entities.RapportPrime;
using Fred.Framework.Exceptions;
using Fred.Framework.Services;

namespace Fred.ImportExport.Business.ValidationPointage
{
    internal static class PrimesMail
    {
        private static StringBuilder Header
        {
            get
            {
                return new StringBuilder("<tr>"
                + "<td> Mois </td>"
                 + "<td> Mat- Nom- Prenom </td>"
                 + "<td> CI </td>"
                 + "<td> Code </td>"
                 + "<td> Libelle </td>"
                 + "<td> Total </td>"
                 + "<td> Type </td>"
                 + "<td> Source</td>"
                 + "</tr>");
            }
        }

        internal static void Send(List<RapportPrimeLignePrimeEnt> listRapportPrimeLignePrimeEnt, string jobId)
        {
            var body = new StringBuilder();
            body.Append(GenerateContent(listRapportPrimeLignePrimeEnt));
            body.Send(jobId);
        }

        /// <summary>
        /// Générer le corps du mail pour des lignes de rapport de primes
        /// </summary>
        /// <param name="listRapportPrimeLignePrimeEnt">List de rapport de ligne prime</param>
        /// <returns>Contenu du mail</returns>
        internal static string GenerateContent(List<RapportPrimeLignePrimeEnt> listRapportPrimeLignePrimeEnt)
        {
            var body = new StringBuilder();
            // Traité que les primes validés.
            IEnumerable<RapportPrimeLignePrimeEnt> validatedPrimes = listRapportPrimeLignePrimeEnt.Where(x => x.RapportPrimeLigne != null && x.RapportPrimeLigne.IsValidated);
            int primeCount = validatedPrimes == null ? 0 : validatedPrimes.Count();
            string title = "Liste de primes mensuelles envoyées vers ANAEL (" + primeCount + " prime(s))";
            body.Append("<b>" + title + "</b>");
            body.Append("<br/><br/>");

            if (primeCount > 0)
            {
                body.Append(@"<table border=""1"" style=""width: 100 %"">");
                body.Append(Header);

                foreach (RapportPrimeLignePrimeEnt item in validatedPrimes)
                {
                    body.Append("<tr>");
                    body.Append("<td>" + item.RapportPrimeLigne?.DateCreation.Value.ToString("MM/yyyy") + "</td>");
                    body.Append("<td>" + item.RapportPrimeLigne?.Personnel?.Matricule + " - " + item.RapportPrimeLigne?.Personnel?.NomPrenom + "</td>");
                    body.Append("<td>" + item.RapportPrimeLigne?.Ci?.Code + " - " + item.RapportPrimeLigne?.Ci?.Libelle + "</td>");
                    body.Append("<td>" + item.Prime?.Code + "</td>");
                    body.Append("<td>" + item.Prime?.Libelle + "</td>");
                    body.Append("<td>" + item.Montant + "</td>");
                    body.Append("<td>Mois</td>");
                    body.Append("<td>FRED</td>");
                    body.Append("</tr>");
                }
                body.Append("</table>");
            }

            return body.ToString();
        }

        /// <summary>
        /// Envoie le mail
        /// </summary>
        /// <param name="body">Le corp du mail.</param>
        /// <param name="jobId">JobId.</param>
        private static void Send(this StringBuilder body, string jobId)
        {
            if (body.Length > 0)
            {
                try
                {
                    using (var sender = new SendMail())
                    {
                        sender.From("c.martageix@fci.fayat.com", "FREDIE");
                        sender.To("n.benichou@fayatit.fayat.com", "Niels BENICHOU");
                        sender.To("c.martageix@fci.fayat.com", "Carine MARTAGEIX");
                        sender.To("n.calloix@fci.fayat.com", "Nicolas CALLOIX");
                        sender.Subject = "[VALIDATION Primes] Primes / JobId : " + jobId;
                        sender.Body = body.ToString();
                        sender.EMail.IsBodyHtml = true;
                        sender.Send();
                    }
                }
                catch (FredTechnicalException e)
                {
                    NLog.LogManager.GetCurrentClassLogger().Error(e);
                }
            }
        }

    }
}
