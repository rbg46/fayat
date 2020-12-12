using System;
using System.Linq;
using Fred.Business.CI;
using Fred.Business.DatesClotureComptable;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Entities.Rapport;
using Fred.Entities.Utilisateur;

namespace Fred.Business.Rapport
{

    /// <inheritdoc />
    public class UtilitiesFeature : ManagerFeature<IRapportRepository>, IUtilitiesFeature
    {

        private readonly IUtilisateurManager userManager;
        private readonly ICIManager ciManager;
        private readonly IDatesClotureComptableManager datesClotureComptableManager;

        /// <summary>
        /// Instancie un nouvel object CrudFeature
        /// </summary>
        /// <param name="repository">Repository</param>
        /// <param name="userManager">Mangaer utilisateur</param>
        /// <param name="ciManager">Manager des CI</param>
        /// <param name="datesClotureComptableManager">Manager des Dates de cloture comptable</param>
        /// <param name="uow"> Unit of work</param>
        public UtilitiesFeature(IRapportRepository repository, IUtilisateurManager userManager, ICIManager ciManager, IDatesClotureComptableManager datesClotureComptableManager, IUnitOfWork uow)
          : base(uow, repository)
        {
            this.userManager = userManager;
            this.ciManager = ciManager;
            this.datesClotureComptableManager = datesClotureComptableManager;
        }

        /// <summary>
        ///   Détermine si le rapport peut être supprimé ou non
        /// </summary>
        /// <param name="rapport">rapport pour lequel déterminer la possibilité de suppression</param>
        /// <param name="userConnected">User connecté pour tracer la suppression</param>
        /// <returns>Booleen indiquant si le rapport peut être supprimé</returns>
        public bool GetCanBeDeleted(RapportEnt rapport, UtilisateurEnt userConnected)
        {
            var isPeriodeNotCloture = !IsTodayInPeriodeCloture(rapport);
            var isRapportNotVerrouille = !IsStatutVerrouille(rapport);
            var isRoleChantier = this.userManager.IsRoleChantier(userConnected.UtilisateurId, rapport.CiId);
            bool isNotValidationSuperieur = false;
            if (IsStatutValide(rapport))
            {
                isNotValidationSuperieur = IsNotValidationSuperieurForDeleteRapport(rapport, userConnected.UtilisateurId) ? true : GetLastValidadeurChantier(rapport) == userConnected.UtilisateurId;
            }
            else
            {
                isNotValidationSuperieur = true;
            }

            return (isPeriodeNotCloture && isRapportNotVerrouille && isRoleChantier && isNotValidationSuperieur);

        }

        private bool IsNotValidationSuperieurForDeleteRapport(RapportEnt rapport, int userId)
        {
            int higherPaieLevel = userManager.GetHigherPaieLevel(userId, rapport.CI?.Organisation?.OrganisationId);

            if (rapport.DateValidationDRC.HasValue && higherPaieLevel <= Constantes.NiveauPaie.LevelDRC)
            {
                return false;
            }
            else if (rapport.DateValidationCDT.HasValue && higherPaieLevel <= Constantes.NiveauPaie.LevelCDT)
            {
                return false;
            }

            return true;
        }

        private int GetLastValidadeurChantier(RapportEnt rapport)
        {
            var id = 0;

            if (rapport.DateValidationDRC.HasValue)
            {
                id = rapport.ValideurDRCId.Value;
            }
            else if (rapport.DateValidationCDT.HasValue)
            {
                id = rapport.ValideurCDTId.Value;
            }
            else if (rapport.DateValidationCDC.HasValue)
            {
                id = rapport.ValideurCDCId.Value;
            }

            return id;
        }

        /// <summary>
        ///   Détermine si le rapport peut être validé ou non
        /// </summary>
        /// <param name="rapport">rapport pour lequel déterminer la possibilité de valider</param>
        /// <returns>Booleen indiquant si le rapport peut être validé</returns>
        /// <remarks>Le validateur de rapport doit être utilisé avant d'utiliser cette fonction.</remarks>
        public bool GetCanBeValidated(RapportEnt rapport)
        {
            UtilisateurEnt userConnected = this.userManager.GetContextUtilisateur();
            if (rapport.CI == null || rapport.CI.Organisation == null)
            {
                rapport.CI = this.ciManager.GetCIById(rapport.CiId);
            }
            if (rapport.ListErreurs.Count != 0 || (IsStatutVerrouille(rapport) || IsTodayInPeriodeCloture(rapport) ||
              !this.userManager.IsRoleChantier(userConnected.UtilisateurId, rapport.CiId)) || GetValidationSuperieur(rapport))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        ///   Détermine si le rapport peut être Edité ou non
        /// </summary>
        /// <param name="rapport">rapport pour lequel déterminer la possibilité d'Editer</param>
        /// <returns>Booleen indiquant si le rapport peut être édité</returns>
        public bool GetValidationSuperieur(RapportEnt rapport)
        {
            int userId = userManager.GetContextUtilisateurId();
            if (rapport == null)
            {
                return false;
            }

            int higherPaieLevel = userManager.GetHigherPaieLevel(userId, rapport.CI?.Organisation?.OrganisationId);
            return IsStatutValide(rapport)
                && (rapport.DateValidationDRC != null && higherPaieLevel < Constantes.NiveauPaie.LevelDRC
                    || rapport.DateValidationCDT != null && higherPaieLevel < Constantes.NiveauPaie.LevelCDT
                    || rapport.DateValidationCDC != null && higherPaieLevel < Constantes.NiveauPaie.LevelCDC);
        }

        /// <summary>
        ///   Détermine si le statut du rapport est Verrouillé
        /// </summary>
        /// <param name="rapport">rapport pour lequel déterminer des rapports</param>
        /// <returns>booléen indiquant si le statut du rapport est verrouillé</returns>
        public bool IsStatutVerrouille(RapportEnt rapport)
        {
            if (rapport != null)
            {
                return rapport.RapportStatutId == RapportStatutEnt.RapportStatutVerrouille.Key;
            }

            return false;
        }

        /// <summary>
        ///   Renvoi vrai si aujourd'hui est dans la periode de clôture
        /// </summary>
        /// <param name="rapport">Le rapport</param>
        /// <returns>Booleen indiquant si aujourd'hui est dans la periode de clôture</returns>
        public bool IsTodayInPeriodeCloture(RapportEnt rapport)
        {
            return this.datesClotureComptableManager.IsPeriodClosed(rapport.CiId, rapport.DateChantier.Year, rapport.DateChantier.Month);
        }

        /// <summary>
        ///   Calcul le statut courant d'un rapport
        /// </summary>
        /// <param name="rapport">rapport à partir uquel déterminer le statut</param>
        public void SetStatut(RapportEnt rapport)
        {
            rapport.IsStatutEnCours = rapport.RapportStatutId == RapportStatutEnt.RapportStatutEnCours.Key;
            rapport.IsStatutValide = IsStatutValide(rapport);
            rapport.IsStatutVerrouille = IsStatutVerrouille(rapport);
        }

        /// <summary>
        ///   Détermine si le rapport peut être verrouillé ou pas
        /// </summary>
        /// <param name="rapport">rapport pour lequel déterminer la possibilité de verrouiller</param>
        /// <returns>Booleen indiquant si le rapport peut être verrouillé</returns>
        /// <remarks>Le validateur de rapport doit être utilisé avant d'utiliser cette fonction.</remarks>
        public bool GetCanBeLocked(RapportEnt rapport)
        {
            string codeGroupe = userManager.GetContextUtilisateur()?.Personnel?.Societe?.Groupe?.Code;
            bool hasPersonnelTemporaire = rapport?.ListLignes?.Any(rl => rl.PersonnelId == null && !string.IsNullOrEmpty(rl.PrenomNomTemporaire) && !rl.IsDeleted) ?? false;
            bool CanBeLocked = codeGroupe == Constantes.CodeGroupeFTP ? !hasPersonnelTemporaire : true;
            return CanBeLocked;
        }

        /// <summary>
        ///   Détermine si le rappport à été soumis à une validation
        /// </summary>
        /// <param name="rapport">rapport pour lequel déterminer des rapports</param>
        /// <returns>retourne Vrai si le rapport contient une date de validation, Faux sinon</returns>
        private bool IsStatutValide(RapportEnt rapport)
        {
            if (rapport != null)
            {
                return rapport.DateValidationCDC != null || rapport.DateValidationCDT != null || rapport.DateValidationDRC != null
                    || rapport.RapportStatutId == RapportStatutEnt.RapportStatutValide2.Key;
            }

            return false;
        }
    }
}
