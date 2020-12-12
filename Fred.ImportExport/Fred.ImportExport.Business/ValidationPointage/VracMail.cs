using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using Fred.Entities.Rapport;
using Fred.Entities.RapportPrime;
using Fred.Entities.Referential;
using Fred.Framework.Exceptions;
using Fred.Framework.Services;

namespace Fred.ImportExport.Business.ValidationPointage
{
    internal static class VracMail
    {


#pragma warning disable S107 // Methods should not have too many parameters
        /// <summary>
        ///   Méthode temporaire
        /// </summary>
        /// <param name="fredPointages">Liste des pointages et primes FRED.</param>
        /// <param name="listPrimeMensuelle">Liste des primes mensuelles</param>
        /// <param name="rvgPointagesAndPrimes">Liste des pointages et primes RVG. Peut être null.</param>
        /// <param name="jobId">JobId.</param>
        /// <param name="periode">Période.</param>
        /// <param name="nomUtilisateur">Nom de l'utilisateur ayant lancé l'opération.</param>
        /// <param name="societeId">L'identifiant de la société.</param>
        /// <param name="controleHelper">Opérations communes au contrôle et à la remontée vrac.</param>
        /// <param name="procQuery">Requête procédure AS400</param>
        /// <param name="recipients">Liste des destinataires du mail</param>
        /// <param name="codeSocietePaye">Code Société Paye sélectionnée où l'on effectue le contrôle</param>
        public static void Send(IEnumerable<RapportLigneEnt> fredPointages, List<RapportPrimeLignePrimeEnt> listPrimeMensuelle, RvgPointagesAndPrimes rvgPointagesAndPrimes, string jobId, DateTime periode, string nomUtilisateur, int societeId, ControleHelper controleHelper, string procQuery, Dictionary<string, string> recipients, string codeSocietePaye)
#pragma warning restore S107 // Methods should not have too many parameters
        {
            List<VracItem> pointages, primes;
            FillVracItems(fredPointages, rvgPointagesAndPrimes, out pointages, out primes);

            var body = new StringBuilder();
            body.Append($"<b>JobId : {jobId}</b><br /><br />");
            body.AppendPointages(pointages, societeId);
            body.Append("<br /><br />");
            body.Append(PrimesMail.GenerateContent(listPrimeMensuelle));
            body.Append("<br /><br />");
            body.AppendPrimes(primes);
            body.Append("<br /><br />");

            (IEnumerable<string> pointageQueries, IEnumerable<string> primeQueries) = controleHelper.GetPointageAndPrimeQueries(jobId, periode, nomUtilisateur, fredPointages, rvgPointagesAndPrimes, codeSocietePaye);
            IEnumerable<string> insertQueries = pointageQueries.Concat(primeQueries);

            body.AppendRequetesAS400(procQuery, () => insertQueries);

            // Envoie du mail
            body.Send(jobId, recipients);
        }


        /// <summary>
        /// Rempli les items du vrac.
        /// Permet d'avoir des listes communes aux pointages et primes FRED et RVG pour les trier par date.
        /// </summary>
        /// <param name="fredPointages">Liste des pointages et primes FRED.</param>
        /// <param name="rvgPointagesAndPrimes">Liste des pointages et primes RVG. Peut être null.</param>
        /// <param name="pointages">Liste des pointages sous forme d'item de vrac.</param>
        /// <param name="primes">Liste des primes sous forme d'item de vrac.</param>
        private static void FillVracItems(IEnumerable<RapportLigneEnt> fredPointages, RvgPointagesAndPrimes rvgPointagesAndPrimes, out List<VracItem> pointages, out List<VracItem> primes)
        {
            pointages = new List<VracItem>();
            primes = new List<VracItem>();

            foreach (RapportLigneEnt pointage in fredPointages)
            {
                pointages.Add(new VracItem(pointage.DatePointage, pointage));

                foreach (RapportLignePrimeEnt prime in pointage.ListRapportLignePrimes)
                {
                    primes.Add(new VracItem(pointage.DatePointage, prime, pointage));
                }
            }

            if (rvgPointagesAndPrimes != null)
            {
                foreach (RvgPointage pointage in rvgPointagesAndPrimes.Pointages)
                {
                    var datePointage = new DateTime(pointage.AnneeRapport, pointage.MoisRapport, pointage.JourRapport);
                    pointages.Add(new VracItem(datePointage, pointage));
                }

                foreach (RvgPrime prime in rvgPointagesAndPrimes.Primes)
                {
                    primes.Add(new VracItem(prime.Date, prime));
                }
            }
        }

        /// <summary>
        /// Ajoute les pointages au corp du mail.
        /// </summary>
        /// <param name="body">Le corp du mail.</param>
        /// <param name="pointages">Les pointages à ajouter.</param>
        /// <param name="societeId">L'identifiant de la société concernée.</param>
        private static void AppendPointages(this StringBuilder body, List<VracItem> pointages, int societeId)
        {
            body.Append($"<b>Liste des pointages envoyés vers ANAEL ({pointages.Count} pointage(s))</b><br />");

            if (pointages.Count > 0)
            {
                body.Append(@"<table border=""1""><tbody>");
                body.AppendPointageEnTete();

                foreach (var pointage in pointages.OrderBy(p => p.Date))
                {
                    var fredPointage = pointage.Item as RapportLigneEnt;
                    if (fredPointage != null)
                    {
                        body.AppendPointageLigne(fredPointage);
                    }
                    else
                    {
                        var rvgPointage = pointage.Item as RvgPointage;
                        if (rvgPointage != null)
                        {
                            body.AppendPointageLigne(rvgPointage, pointage.Date, societeId);
                        }
                    }
                }
                body.Append("</tbody></table>");
            }
        }

        /// <summary>
        /// Ajoute l'en-tête des pointages au corp du mail.
        /// </summary>
        /// <param name="body">Le corp du mail.</param>
        public static void AppendPointageEnTete(this StringBuilder body)
        {
            body.Append(@"
        <tr>
          <td>Date</td>
          <td>RapportLigneId</td>
          <td>RapportId</td>
          <td>Mat-Nom-Prenom</td>
          <td>Soc Id</td>
          <td>Etab Id</td>
          <td>CI</td>
          <td>Hrs</td>
          <td>Code Maj</td>
          <td>Hrs Maj</td>
          <td>Code Abs</td>
          <td>Hrs abs</td>
          <td>Code dep</td>
          <td>Code Zone Dep</td>
          <td>IVD</td>
          <td>Num Sem Int</td>
          <td>Source</td>
        </tr>");
        }

        /// <summary>
        /// Ajoute une ligne de pointage FRED au corp du mail.
        /// </summary>
        /// <param name="body">Le corp du mail.</param>
        /// <param name="pointage">Le pointage concerné</param>
        public static void AppendPointageLigne(this StringBuilder body, RapportLigneEnt pointage)
        {
            string codeCI = pointage.Ci?.CompteInterneSep != null ? pointage.Ci.CompteInterneSep.Code : pointage.Ci.Code;

            body.Append($@"
        <tr>
          <td>{pointage.DatePointage.ToShortDateString()}</td>
          <td>{pointage.RapportLigneId}</td>
          <td>{pointage.RapportId}</td>
          <td>{pointage.Personnel?.CodeNomPrenom}</td>
          <td>{pointage.Personnel?.SocieteId}</td>
          <td>{pointage.Personnel?.EtablissementPaieId}</td>
          <td>{codeCI}</td>
          <td>{pointage.HeureNormale}</td>
          <td>{pointage.CodeMajoration?.Code}</td>
          <td>{pointage.HeureMajoration}</td>
          <td>{pointage.CodeAbsence?.Code}</td>
          <td>{pointage.HeureAbsence}</td>
          <td>{pointage.CodeDeplacement?.Code}</td>
          <td>{pointage.CodeZoneDeplacement?.Code}</td>
          <td>{pointage.DeplacementIV}</td>
          <td>{pointage.NumSemaineIntemperieAbsence}</td>
          <td>FRED</td>
        </tr>");
        }



        /// <summary>
        /// Ajoute une ligne de pointage RVG au corp du mail.
        /// </summary>
        /// <param name="body">Le corp du mail.</param>
        /// <param name="pointage">Le pointage concerné</param>
        /// <param name="datePointage">La date du pointage.</param>
        /// <param name="societeId">L'identifiant de la société concernée.</param>
        private static void AppendPointageLigne(this StringBuilder body, RvgPointage pointage, DateTime datePointage, int societeId)
        {
            var voyageDetente = pointage.VoyageDetente == "1";
            body.Append($@"
        <tr>
          <td>{datePointage.ToShortDateString()}</td>
          <td></td>
          <td></td>
          <td>{pointage.MatriculePersonnel}</td>
          <td>{societeId}</td>
          <td>{pointage.EtablissementPaieIdPersonnel}</td>
          <td>{pointage.CodeAffaire}</td>
          <td>{pointage.HeureNormale}</td>
          <td>{pointage.CodeMajoration}</td>
          <td>{pointage.HeureMajoration}</td>
          <td>{pointage.CodeAbsence}</td>
          <td>{pointage.HeureAbsence}</td>
          <td>{pointage.CodeDeplacement}</td>
          <td>{pointage.CodeZoneDeplacement}</td>
          <td>{voyageDetente}</td>
          <td>{pointage.NumSemaineIntemperieAbsence}</td>
          <td>RVG</td>
        </tr>");
        }

        /// <summary>
        /// Ajoute les primes au corp du mail.
        /// </summary>
        /// <param name="body">Le corp du mail.</param>
        /// <param name="primes">Les primes à ajouter.</param>
        private static void AppendPrimes(this StringBuilder body, List<VracItem> primes)
        {
            body.Append($"<b>Liste des primes journalières envoyées vers ANAEL ({primes.Count} prime(s))</b><br />");

            if (primes.Count > 0)
            {
                body.Append(@"<table border=""1""><tbody>");
                body.AppendPrimeEnTete();

                foreach (var prime in primes.OrderBy(p => p.Date))
                {
                    var fredPrime = prime.Item as RapportLignePrimeEnt;
                    if (fredPrime != null)
                    {
                        body.AppendPrimeLigne(fredPrime, prime.Tag as RapportLigneEnt);
                    }
                    else
                    {
                        var rvgPrime = prime.Item as RvgPrime;
                        if (rvgPrime != null)
                        {
                            body.AppendPrimeLigne(rvgPrime);
                        }
                    }
                }

                body.Append("</tbody></table>");
            }
        }

        /// <summary>
        /// Ajoute l'en-tête des primes au corp du mail.
        /// </summary>
        /// <param name="body">Le corp du mail.</param>
        public static void AppendPrimeEnTete(this StringBuilder body)
        {
            body.Append(@"
        <tr>
          <td>Date</td>
          <td>RapportLigneId</td>
          <td>Mat-Nom-Prenom</td>
          <td>CI</td>
          <td>Code</td>
          <td>Libelle</td>
          <td>Heure</td>
          <td>Type</td>
          <td>Source</td>
        </tr>");
        }

        /// <summary>
        /// Ajoute une ligne de prime FRED au corp du mail.
        /// </summary>
        /// <param name="body">Le corp du mail.</param>
        /// <param name="prime">La prime concernée.</param>
        /// <param name="pointage">Le pointage correspondant.</param>
        public static void AppendPrimeLigne(this StringBuilder body, RapportLignePrimeEnt prime, RapportLigneEnt pointage)
        {
            string type = string.Empty;
            string heure = string.Empty;
            switch (prime.Prime.PrimeType)
            {
                case ListePrimeType.PrimeTypeHoraire:
                    type = "Heures";
                    heure = prime.HeurePrime.HasValue ? prime.HeurePrime.Value.ToString().Replace(",", ".") : "0.0";
                    break;
                case ListePrimeType.PrimeTypeJournaliere:
                    type = "Jours";
                    heure = "1.0";
                    break;
                default:
                    // ListePrimeType.PrimeTypeMensuelle
                    type = "Mois";
                    heure = "1.0";
                    break;
            }

            string codeCI = pointage.Ci?.CompteInterneSep != null ? pointage.Ci.CompteInterneSep.Code : pointage.Ci.Code;
            string libelleCI = pointage.Ci?.CompteInterneSep != null ? pointage.Ci.CompteInterneSep.Libelle : pointage.Ci.Libelle;

            body.Append($@"
        <tr>
          <td>{pointage.DatePointage.ToShortDateString()}</td>
          <td>{pointage.RapportLigneId}</td>
          <td>{pointage.Personnel?.CodeNomPrenom}</td>
          <td>{codeCI + " - " + libelleCI}</td>
          <td>{prime.Prime?.Code}</td>
          <td>{prime.Prime?.Libelle}</td>
          <td>{heure}</td>
          <td>{type}</td>
          <td>FRED</td>
        </tr>");
        }

        /// <summary>
        /// Ajoute une ligne de prime RVG au corp du mail.
        /// </summary>
        /// <param name="body">Le corp du mail.</param>
        /// <param name="prime">La prime concernée.</param>
        private static void AppendPrimeLigne(this StringBuilder body, RvgPrime prime)
        {
            var heure = prime.TypeHoraire ? prime.Quantite.ToString().Replace(",", ".") : "1.0";
            var type = prime.TypeHoraire ? "Heures" : "Jours";
            body.Append($@"
        <tr>
          <td>{prime.Date.ToShortDateString()}</td>
          <td></td>
          <td>{prime.MatriculePersonnel}</td>
          <td>{prime.Code}</td>
          <td>{prime.Libelle}</td>
          <td>{heure}</td>
          <td>{type}</td>
          <td>RVG</td>
        </tr>");
        }



        /// <summary>
        /// Ajoute les requêtes envoyées à l'AS400.
        /// </summary>
        /// <param name="body">Le corp du mail.</param>
        /// <param name="procQuery">Requête procédure AS400</param>
        /// <param name="getPointageAndPrimeQueries">La fonction de récupération des requêtes.</param>
        private static void AppendRequetesAS400(this StringBuilder body, string procQuery, Func<IEnumerable<string>> getPointageAndPrimeQueries)
        {
            body.Append("<b>Requêtes SQL AS400</b><br />");
            body.Append("<br />");
            body.Append("Procédure : ").Append(procQuery);
            body.Append("<br />");

            foreach (var query in getPointageAndPrimeQueries())
            {
                body.Append(query);
                body.Append("<br/><br/>");
            }
        }

        /// <summary>
        /// Envoie le mail
        /// </summary>
        /// <param name="body">Le corp du mail.</param>
        /// <param name="jobId">JobId.</param>
        /// <param name="recipients">Liste des destinataires du mail</param>
        public static void Send(this StringBuilder body, string jobId, Dictionary<string, string> recipients)
        {
            try
            {
                using (var sender = new SendMail())
                {
                    sender.From("c.martageix@fci.fayat.com", "FREDIE");

                    foreach (var r in recipients)
                    {
                        sender.To(r.Value, r.Key);
                    }

                    sender.Subject = "[VALIDATION POINTAGE] Debug Pointages / JobId : " + jobId;
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

        /// <summary>
        /// Envoi un mail avec en piece jointe la liste des parametres utiliser pour les insertion envoyer à Anaël
        /// </summary>
        /// <param name="body">Corp du mail</param>
        /// <param name="pathPointageFile">Chemin du fichier de paramètres Pointage</param>
        /// <param name="pathPrimeFile">Chemin du fichier de paramètres Prime</param>
        /// <param name="jobId">Identifiant du Job</param>
        /// <param name="recipients">Liste des destinataires du mail</param>
        public static void SendInsertQuery(this StringBuilder body, string pathPointageFile, string pathPrimeFile, string jobId, Dictionary<string, string> recipients)
        {
            try
            {
                using (var sender = new SendMail())
                {
                    sender.From("c.martageix@fci.fayat.com", "FREDIE");

                    foreach (var r in recipients)
                    {
                        sender.To(r.Value, r.Key);
                    }

                    sender.Subject = "[INSERTION DES POINTAGES] Debug Pointages / JobId : " + jobId;
                    sender.Body = body.ToString();
                    if (!string.IsNullOrEmpty(pathPointageFile))
                    {
                        sender.Attachments.Add(new Attachment(pathPointageFile, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"));
                    }
                    if (!string.IsNullOrEmpty(pathPrimeFile))
                    {
                        sender.Attachments.Add(new Attachment(pathPrimeFile, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"));
                    }
                    sender.EMail.IsBodyHtml = true;
                    sender.Send();
                    sender.Dispose();
                    if (!string.IsNullOrEmpty(pathPointageFile))
                    {
                        File.Delete(pathPointageFile);
                    }
                    if (!string.IsNullOrEmpty(pathPrimeFile))
                    {
                        File.Delete(pathPrimeFile);
                    }

                }
            }
            catch (FredTechnicalException e)
            {
                NLog.LogManager.GetCurrentClassLogger().Error(e);
            }
        }

        /// <summary>
        /// Représente un élement du vrac.
        /// </summary>
        private class VracItem
        {
            /// <summary>
            /// Constructeur.
            /// </summary>
            /// <param name="date">La date de pointage de l'élément.</param>
            /// <param name="item">L'élément.</param>
            public VracItem(DateTime date, object item)
            {
                Date = date;
                Item = item;
            }

            /// <summary>
            /// Constructeur.
            /// </summary>
            /// <param name="date">La date de pointage de l'élément.</param>
            /// <param name="item">L'élément.</param>
            /// <param name="tag">Le tag pour stocker une variable utilisateur.</param>
            public VracItem(DateTime date, object item, object tag)
              : this(date, item)
            {
                Tag = tag;
            }

            /// <summary>
            /// La date de pointage de l'élément.
            /// </summary>
            public DateTime Date { get; private set; }

            /// <summary>
            /// L'élement concerné.
            /// </summary>
            public object Item { get; private set; }

            /// <summary>
            /// Le tag pour stocker une variable utilisateur.
            /// </summary>
            public object Tag { get; set; }
        }
    }
}
