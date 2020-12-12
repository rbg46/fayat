using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fred.Business.Affectation;
using Fred.Business.DatesClotureComptable;
using Fred.Business.Rapport.Pointage.PointagePersonnel.Interfaces;
using Fred.Business.Rapport.RapportHebdo;
using Fred.Business.RapportTache;
using Fred.Business.Referential;
using Fred.Business.Referential.CodeDeplacement;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Rapport;

namespace Fred.Business.Rapport.Pointage.PointagePersonnel
{
    public class PointagePersonnelTransformerForViewService : IPointagePersonnelTransformerForViewService
    {
        private readonly IAffectationManager affectationManager;
        private readonly IDatesClotureComptableManager datesClotureComptableManager;
        private readonly IRapportTacheManager rapportTacheManager;
        private readonly IPointageRepository pointageRepository;
        private readonly IPointagePersonnelGlobalDataProvider pointagePersonnelGlobalDataProvider;
        private readonly ICodeMajorationManager codeMajorationManager;
        private readonly ICodeZoneDeplacementManager codeZoneDeplacementManager;
        private readonly IPrimeManager primeManager;
        private readonly ICodeDeplacementManager codeDeplacementManager;


        public PointagePersonnelTransformerForViewService(
            IAffectationManager affectationManager,
            IDatesClotureComptableManager datesClotureComptableManager,
            IRapportTacheManager rapportTacheManager,
            IPointageRepository pointageRepository,
            IPointagePersonnelGlobalDataProvider pointagePersonnelGlobalDataProvider,
            ICodeMajorationManager codeMajorationManager,
            ICodeZoneDeplacementManager codeZoneDeplacementManager,
            IPrimeManager primeManager,
            ICodeDeplacementManager codeDeplacementManager)
        {
            this.affectationManager = affectationManager;
            this.datesClotureComptableManager = datesClotureComptableManager;
            this.rapportTacheManager = rapportTacheManager;
            this.pointageRepository = pointageRepository;
            this.pointagePersonnelGlobalDataProvider = pointagePersonnelGlobalDataProvider;
            this.codeMajorationManager = codeMajorationManager;
            this.codeZoneDeplacementManager = codeZoneDeplacementManager;
            this.primeManager = primeManager;
            this.codeDeplacementManager = codeDeplacementManager;
        }

        /// <summary>
        /// Retourne la liste des pointages pour un personnel et pour un mois donné.
        /// </summary>
        /// <param name="personnelId">personnelId</param>
        /// <param name="periode">periode</param>
        /// <returns>Liste de pointage personnel formatté pour la vue et avec les messages d'erreurs</returns>
        public async Task<PointagePersonnelInfo> GetListPointagesForViewAsync(int personnelId, DateTime periode)
        {
            PointagePersonnelInfo pointagePersonnelInfo = new PointagePersonnelInfo();
            pointagePersonnelInfo.Pointages = pointageRepository.GetListPointagesOnMonthForPersonnelId(personnelId, periode).Where(p => !p.IsDeleted).ToList();
            RapportHebdoDeplacement deplacement = new RapportHebdoDeplacement(codeMajorationManager,codeZoneDeplacementManager,primeManager,codeDeplacementManager);
            pointagePersonnelInfo.CodeDeplacementReadonly = deplacement.UtilisateurConnecteIsGFES;
            pointagePersonnelInfo.ShowSaisieManuelle = deplacement.UtilisateurConnecteIsGFES;
            pointagePersonnelInfo.ShowDeplacement = !deplacement.UtilisateurConnecteIsGFES || deplacement.PersonnelIsOuvrier(personnelId);

            if (!pointagePersonnelInfo.Pointages.Any())
            {
                return pointagePersonnelInfo;
            }

            // Particularité FES : le code de déplacement le plus favorable doit être indiqué explicitement à l'écran
            deplacement.UpdateCodeFavorable(personnelId, pointagePersonnelInfo.Pointages);
            var pointagePersonnelGlobalData = await pointagePersonnelGlobalDataProvider.GetDataForFormatRapportLignesForViewAsync(pointagePersonnelInfo.Pointages, personnelId, periode).ConfigureAwait(false);
            FormatRapportLignesForView(pointagePersonnelGlobalData, pointagePersonnelInfo.Pointages);

            return pointagePersonnelInfo;
        }

        private void FormatRapportLignesForView(PointagePersonnelGlobalData pointagePersonnelGlobalData, IEnumerable<RapportLigneEnt> rapportLignes)
        {
            foreach (RapportLigneEnt pointage in rapportLignes)
            {
                FormatRapportLigneForView(pointagePersonnelGlobalData, pointage);
            }
        }

        private void FormatRapportLigneForView(PointagePersonnelGlobalData pointagePersonnelGlobalData, RapportLigneEnt pointage)
        {

            // Permet de récuperer PrenomNomTemporaire à partir de Personnel.PrenomNom
            if (pointage.Personnel != null)
            {
                pointage.PrenomNomTemporaire = pointage.Personnel.PrenomNom;
            }

            // Permet de récuperer MaterielNomTemporaire à partir de Materiel.LibelleLong
            if (pointage.Materiel != null)
            {
                pointage.MaterielNomTemporaire = pointage.Materiel.LibelleLong;
            }
            // Affectation de la cloture comptable
            pointage.Cloture = this.datesClotureComptableManager.IsPeriodClosed(pointagePersonnelGlobalData.DatesClotureComptablesForCisOfPointages, pointage.CiId, pointage.DatePointage.Year, pointage.DatePointage.Month);
            // Affectation du périmètre
            pointage.MonPerimetre = IsInMyPerimetre(pointagePersonnelGlobalData.CurrentUserCiIds, pointage);
            // Affectation des astreintes - Spécifique FES
            var astreinte = affectationManager.GetAstreinte(pointagePersonnelGlobalData.AstreintesOfPersonnelOnCisOnDates, pointage.Ci.CiId, pointagePersonnelGlobalData.PersonnelId, pointage.DatePointage);
            if (astreinte != null)
            {
                pointage.HasAstreinte = true;
                pointage.AstreinteId = astreinte.AstreintId;
            }
            else
            {
                pointage.HasAstreinte = false;
            }
            // Affectation des commentaires
            foreach (RapportLigneTacheEnt ligneTache in pointage.ListRapportLigneTaches)
            {
                ligneTache.Commentaire = rapportTacheManager.GetCommentairesByRapportIdAndTacheId(pointagePersonnelGlobalData.RapportsTachesOfRapportsOfRapportsLignes, pointage.RapportId, ligneTache.TacheId);
            }
        }

        /// <summary>
        /// Evalue l'appartenance d'un pointage au périmètre de l'utilisateur
        /// </summary>
        /// <param name="userCiIds">ci ids de l'utilisateur courant</param>
        /// <param name="pointage">Le pointage à évaluer</param>
        /// <returns>Retourne vrai si le pointage est rattaché à un CI faisant parti du périmètre de l'utilisateur, faux sinon</returns>
        private bool IsInMyPerimetre(IEnumerable<int> userCiIds, RapportLigneEnt pointage)
        {
            return userCiIds.Contains(pointage.CiId);
        }
    }
}
