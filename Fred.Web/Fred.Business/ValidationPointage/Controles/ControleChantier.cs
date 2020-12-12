using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fred.Business.Notification;
using Fred.Business.Rapport.Pointage;
using Fred.Business.Utilisateur;
using Fred.Entities;
using Fred.Entities.Notification;
using Fred.Entities.Personnel;
using Fred.Entities.Rapport;
using Fred.Entities.Referential;
using Fred.Entities.Utilisateur;
using Fred.Entities.ValidationPointage;
using Fred.Framework.Exceptions;
using Fred.Framework.Extensions;
using Fred.Web.Shared.App_LocalResources;

namespace Fred.Business.ValidationPointage.Controles
{
    /// <summary>
    ///   Classe Controle Chantier
    /// </summary>
    public class ControleChantier
    {
        private readonly ILotPointageManager lotPointageManager;
        private readonly IControlePointageManager controlePointageManager;
        private readonly IUtilisateurManager utilisateurManager;
        private readonly IPointageManager pointageManager;
        private readonly INotificationManager notificationManager;

        // RG_661_033
        private const int MAXHEURESNORMALESMAJOREES = 16;
        private const int MAXHEURESNORMALES = 12;

        // RG_043 Rapport hebdo
        private const int MINMAXHEURESNORMALEABSENCESMAJOREESFES = 7;

        public ControleChantier(
            ILotPointageManager lotPointageManager,
            IControlePointageManager controlePointageManager,
            IUtilisateurManager utilisateurManager,
            IPointageManager pointageManager,
            INotificationManager notificationManager)
        {
            this.lotPointageManager = lotPointageManager;
            this.controlePointageManager = controlePointageManager;
            this.utilisateurManager = utilisateurManager;
            this.pointageManager = pointageManager;
            this.notificationManager = notificationManager;
        }

        /// <summary>
        ///   Lance le contrôle chantier d'un lot de pointage
        /// </summary>    
        /// <param name="lotPointageId">Identifiant du lot de pointage</param>    
        /// /// <param name="userId">Identifiant de l'utilisateur</param>
        /// <returns>Lot de pointage avec son statut</returns>
        public async Task<ControlePointageEnt> ExecuteAsync(int lotPointageId, int userId)
        {
            LotPointageEnt lotPointage = lotPointageManager.Get(lotPointageId);
            UtilisateurEnt utilisateur = await utilisateurManager.GetContextUtilisateurAsync();

            var ctrlPointage = new ControlePointageEnt
            {
                DateDebut = DateTime.UtcNow,
                AuteurCreationId = utilisateur.UtilisateurId,
                LotPointageId = lotPointage.LotPointageId,
                Statut = FluxStatus.InProgress.ToIntValue(),
                TypeControle = TypeControlePointage.ControleChantier.ToIntValue(),
                Erreurs = new List<ControlePointageErreurEnt>()
            };

            controlePointageManager.AddControlePointage(ctrlPointage);

            // vérifier les droits de l'utilisateur 
            // CSP : droit exécution que si lotPointage.AuteurCreationId == userId    
            // GSP : droit à tout   

            // Lance le contrôle chantier
            await ControleChantierJobAsync(ctrlPointage, lotPointage, userId);

            ctrlPointage.AuteurCreation = utilisateur;

            return ctrlPointage;
        }

        /// <summary>
        ///   Job Contrôle chantier
        /// </summary>
        /// <param name="ctrlPointage">Controle pointage</param>
        /// <param name="lotPointage">Lot de pointage</param>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        /// <returns>Tache</returns>        
        public async Task ControleChantierJobAsync(ControlePointageEnt ctrlPointage, LotPointageEnt lotPointage, int userId)
        {
            try
            {
                IEnumerable<RapportLigneEnt> allPointages = pointageManager.GetAllLockedPointages(lotPointage.Periode);

                // Exclure les pointages d'un personnel temporaire
                allPointages = allPointages.Where(p => p.PersonnelId.HasValue);

                // Boucle par jour (Regroupement par Date du Rapport)
                foreach (IGrouping<DateTime, RapportLigneEnt> dateRapportGroup in allPointages.GroupBy(x => x.DatePointage.Date))
                {
                    // Boucle par personnel (Regroupement par Personnel)
                    foreach (IGrouping<int?, RapportLigneEnt> persoGroup in dateRapportGroup.GroupBy(x => x.PersonnelId))
                    {
                        ControlePointagePersonnelJour(persoGroup, ctrlPointage);
                    }
                }
                ctrlPointage.Statut = FluxStatus.Done.ToIntValue();
                ctrlPointage = controlePointageManager.UpdateControlePointage(ctrlPointage);
                await notificationManager.CreateNotificationAsync(userId, TypeNotification.NotificationUtilisateur, string.Format(FeatureValidationPointage.VPManager_ControleChantierJobSuccess, ctrlPointage.DateDebut.ToLocalTime()));
            }
            catch (FredRepositoryException fre)
            {
                ctrlPointage.Statut = FluxStatus.Failed.ToIntValue();
                controlePointageManager.UpdateControlePointage(ctrlPointage);
                await notificationManager.CreateNotificationAsync(userId, TypeNotification.NotificationUtilisateur, string.Format(FeatureValidationPointage.VPManager_ControleChantierJobFailed, ctrlPointage.DateDebut.ToLocalTime()));
                throw new FredBusinessException(fre.Message, fre.InnerException);
            }
            catch (Exception e)
            {
                ctrlPointage.Statut = FluxStatus.Failed.ToIntValue();
                controlePointageManager.UpdateControlePointage(ctrlPointage);
                await notificationManager.CreateNotificationAsync(userId, TypeNotification.NotificationUtilisateur, string.Format(FeatureValidationPointage.VPManager_ControleChantierJobFailed, ctrlPointage.DateDebut.ToLocalTime()));
                throw new FredBusinessException(e.Message, e.InnerException);
            }
        }

        /// <summary>
        ///   Contrôle tous les pointages pour une personne et sur une journée (Tous CI confondus)
        /// </summary>
        /// <param name="rapportLigneList">Liste des pointages d'un personnel sur une journée (tous CI confondus)</param>
        /// <param name="controlePointage">Contrôle Pointage</param>        
        private void ControlePointagePersonnelJour(IEnumerable<RapportLigneEnt> rapportLigneList, ControlePointageEnt controlePointage)
        {
            double totalHeuresNormalesMajorees = 0;
            double totalHeuresNormales = 0;
            double totalHeuresMajorees = 0;
            var primeIdList = new List<int>();
            RapportLigneEnt rl = rapportLigneList.FirstOrDefault();
            DateTime date = rl.DatePointage;

            foreach (RapportLigneEnt ligneRapport in rapportLigneList.ToList())
            {
                totalHeuresNormales += ligneRapport.HeureNormale;
                totalHeuresMajorees += ligneRapport.HeureMajoration;
                primeIdList.AddRange(ligneRapport.ListePrimes.Where(x => (x.IsChecked && x.Prime.PrimeType == ListePrimeType.PrimeTypeJournaliere) || (x.HeurePrime > 0 && x.Prime.PrimeType == ListePrimeType.PrimeTypeHoraire)).Select(x => x.PrimeId));
            }

            if (totalHeuresMajorees > 0)
            {
                totalHeuresNormalesMajorees = totalHeuresMajorees + totalHeuresNormales;
            }

            if (controlePointage.LotPointageId == rl.LotPointageId)
            {
                // CTRL_CHT_001: Le total des heures d'une journée (heures normales + majorées) d'un Personnel doit être <= 16h, tous CI confondus.
                if (totalHeuresNormalesMajorees > MAXHEURESNORMALESMAJOREES)
                {
                    controlePointageManager.AddControlePointageErreur(NewControlePointageErreur(date, controlePointage.ControlePointageId, rl.Personnel, FeatureValidationPointage.VPManager_CTRL_CHT_001));
                }

                // CTRL_CHT_002 : Le nombre d'heures normales d'une journée d'un Personnel doit être <= 12h, tous CI confondus.
                if (totalHeuresNormales > MAXHEURESNORMALES)
                {
                    controlePointageManager.AddControlePointageErreur(NewControlePointageErreur(date, controlePointage.ControlePointageId, rl.Personnel, FeatureValidationPointage.VPManager_CTRL_CHT_002));
                }

                // CTRL_CHT_003 : La même prime ne peut pas être attribuée plus d'une fois sur une même journée au même Personnel, tous CI confondus.
                if (primeIdList.Count != primeIdList.Distinct().Count())
                {
                    controlePointageManager.AddControlePointageErreur(NewControlePointageErreur(date, controlePointage.ControlePointageId, rl.Personnel, FeatureValidationPointage.VPManager_CTRL_CHT_003));
                }

                // CTRL RapportHebdo RG_043	Pour chaque ouvrier et chaque journée pointée, la somme des heures de travail et d’absence doit être ≥ 7 h pour FES
                if (rl.Ci?.Societe?.Groupe != null && rl.Ci.Societe.Groupe.Code.Trim().Equals(FeatureRapport.Code_Groupe_FES))
                {
                    CheckTotalHoursForFes(controlePointage, rl, date);
                }
            }
        }

        /// <summary>
        ///   Récupère un nouveau controlePointageErreur
        /// </summary>
        /// <param name="date">Date du rapport</param>
        /// <param name="ctrlPointageId">Identifiant du controle pointage erreur</param>
        /// <param name="p">Personnel</param>
        /// <param name="message">Message d'erreur</param>
        /// <returns>Nouveau ControlePointageErreur</returns>
        private ControlePointageErreurEnt NewControlePointageErreur(DateTime date, int ctrlPointageId, PersonnelEnt p, string message)
        {
            return new ControlePointageErreurEnt
            {
                DateRapport = date,
                Message = message,
                ControlePointageId = ctrlPointageId,
                PersonnelId = p.PersonnelId,
                Personnel = p
            };
        }

        /// <summary>
        /// Check total hours for FES
        /// </summary>
        /// <param name="controlePointage">Controle pointage</param>
        /// <param name="rapportLigne">Rapport ligne</param>
        /// <param name="date">Date chantier</param>
        private void CheckTotalHoursForFes(ControlePointageEnt controlePointage, RapportLigneEnt rapportLigne, DateTime date)
        {
            double totalHoursWorkAndAbsenceWithMajoration = pointageManager.GetTotalHoursWorkAndAbsenceWithMajoration(rapportLigne.Personnel.PersonnelId, date);
            if (totalHoursWorkAndAbsenceWithMajoration < MINMAXHEURESNORMALEABSENCESMAJOREESFES)
            {
                controlePointageManager.AddControlePointageErreur(NewControlePointageErreur(date, controlePointage.ControlePointageId, rapportLigne.Personnel, string.Format(FeatureValidationPointage.VPManager_CTRL_RHebdo_043, MINMAXHEURESNORMALEABSENCESMAJOREESFES - totalHoursWorkAndAbsenceWithMajoration)));
            }
        }
    }
}
