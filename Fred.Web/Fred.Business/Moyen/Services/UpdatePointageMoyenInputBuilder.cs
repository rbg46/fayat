using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Business.AffectationMoyen;
using Fred.Business.Rapport;
using Fred.Entities;
using Fred.Entities.Moyen;
using Fred.Framework.DateTimeExtend;
using Fred.Web.Shared.Extentions;

namespace Fred.Business.Moyen
{
    public class UpdatePointageMoyenInputBuilder : ManagersAccess, IUpdatePointageMoyenInputBuilder
    {
        private readonly IAffectationMoyenManager affectationMoyenManager;
        private readonly IRapportManager rapportManager;

        public UpdatePointageMoyenInputBuilder(IAffectationMoyenManager affectationMoyenManager, IRapportManager rapportManager)
        {
            this.affectationMoyenManager = affectationMoyenManager;
            this.rapportManager = rapportManager;
        }

        /// <summary>
        /// Génération de la requéte pour initier l'update des pointage des moyens
        /// Cette méthode vérifie qu'on a des rapports pour les Cis de génération dans l'interval des dates , 
        /// si les rapports n'existent pas elle les crée .
        /// </summary>
        /// <param name="startDate">Date de début de génération</param>
        /// <param name="endDate">Date de fin de génération</param>
        /// <param name="affectationMoyenList">La liste des affectations</param>
        /// <param name="dateTimeExtendManager">Date time extension manager</param>
        /// <param name="moyenPointageHelper">Moyen filter helper</param>
        /// <returns>La requéte pour générer le pointage des moyens</returns>
        public UpdateMoyenPointageInput GetPointageMoyenRequest(
            DateTime startDate,
            DateTime endDate,
            IEnumerable<AffectationMoyenEnt> affectationMoyenList,
            IDateTimeExtendManager dateTimeExtendManager,
            MoyenPointageHelper moyenPointageHelper)
        {
            UpdateMoyenPointageInput request = new UpdateMoyenPointageInput();

            if (affectationMoyenList.IsNullOrEmpty())
            {
                return request;
            }

            request.StartDate = startDate;
            request.EndDate = endDate;

            List<AffectationMoyenTypeCode> affectationMoyenTypeCodes = moyenPointageHelper.GetRestitutionAndMaintenanceCodes().ToList();
            IEnumerable<string> restitutionMaintenanceCodes = affectationMoyenTypeCodes.Select(r => r.ToString());
            IEnumerable<string> ciMaintenanceAndRestitutionCodes = affectationMoyenManager.GetRestitutionAndMaintenanceCiCodes(restitutionMaintenanceCodes);
            request.RestitutionAndMaintenanceCiList = Managers.CI.GetCiByCodeList(ciMaintenanceAndRestitutionCodes);
            affectationMoyenTypeCodes.Add(AffectationMoyenTypeCode.CI);
            IEnumerable<int> ciTypeAffectationList = affectationMoyenTypeCodes.Select(o => (int)o);
            IEnumerable<AffectationMoyenEnt> ciAffectationMoyenList = affectationMoyenList.Where(w => ciTypeAffectationList.Contains(w.AffectationMoyenTypeId));

            request.FillCiAffectationsWorkingDaysDictionnary(
                ciAffectationMoyenList,
                dateTimeExtendManager,
                startDate,
                endDate,
                moyenPointageHelper);

            List<int> affectationCiList = ciAffectationMoyenList.Where(o => o.CiId.HasValue).Select(a => a.CiId.Value).ToList();
            IEnumerable<int> maintenanceOrRestitutionCiList = request.RestitutionAndMaintenanceCiList.Select(x => x.CiId);

            if (!maintenanceOrRestitutionCiList.IsNullOrEmpty())
            {
                affectationCiList.AddRange(maintenanceOrRestitutionCiList);
            }

            request.CiAffectationRapportList = rapportManager.GetRapportListWithRapportLignesBetweenDatesByCiList(
                affectationCiList,
                startDate,
                endDate);
            return request;
        }
    }
}
