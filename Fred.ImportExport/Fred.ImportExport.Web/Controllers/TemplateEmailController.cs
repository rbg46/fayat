using System.Web.Mvc;
using Fred.Entities.ActivitySummary;

namespace Fred.ImportExport.Web.Controllers
{
    public class TemplateEmailController : Controller
    {
        /// <summary>
        /// Cette page n'a d'utilité que pour faire le disgn de template de mail.
        /// Elle n'est pas presente dans les menus
        /// </summary>
        /// <returns>Le mail qui sera envoyé pour le récapitulatif des activités en cours</returns>
        public ActionResult ActivitiesAndJalonsTemplate()
        {
            // données de demo 
            var model = new ActivitySummaryResult();
            model.UsersActivities.Add(new UserActivitySummary
            {
                CiId = 123456,
                Labelle = "123457 CI12",
                NombreRapportsAvalide1 = 5,
                ColorRapportsAvalide1 = ColorActivity.ColorOrange,
                NombreCommandeAValider = null,
                NombreReceptionsAviser = null,
                NombreBudgetAvalider = null,
                NombreAvancementAvalider = null,
                NombreControleBudgetaireAvalider = null,
            });
            model.UsersActivities.Add(new UserActivitySummary
            {
                CiId = 123457,
                Labelle = "123457 CI13",
                NombreRapportsAvalide1 = null,
                NombreCommandeAValider = 4,
                ColorCommandeAValider = ColorActivity.ColorRed,
                NombreReceptionsAviser = 2,
                NombreBudgetAvalider = null,
                NombreAvancementAvalider = null,
                NombreControleBudgetaireAvalider = null,
            });
            model.UsersActivities.Add(new UserActivitySummary
            {
                CiId = 123458,
                Labelle = "123457 CI14",
                NombreRapportsAvalide1 = null,
                NombreCommandeAValider = null,
                NombreReceptionsAviser = null,
                NombreBudgetAvalider = 1,
                ColorReceptionsAviser = ColorActivity.ColorWhite,
                NombreAvancementAvalider = null,
                NombreControleBudgetaireAvalider = null,
            });
            model.UsersActivities.Add(new UserActivitySummary
            {
                CiId = 123459,
                Labelle = "123457 CI15",
                NombreRapportsAvalide1 = null,
                NombreCommandeAValider = null,
                NombreReceptionsAviser = 12,
                NombreBudgetAvalider = null,
                NombreAvancementAvalider = 1,
                ColorReceptionsAviser = ColorActivity.ColorRed,
                ColorBudgetAvalider = ColorActivity.ColorBlue,
                NombreControleBudgetaireAvalider = null,
            });


            model.UsersCiJalons.Add(new UserJalonSummary
            {
                CiId = 123459,
                Labelle = "123457 CI16",
                JalonTransfertFar = new System.DateTime(2018, 12, 1),
                ColorJalonTransfertFar = ColorJalon.ColorBlue,
                JalonClotureDepense = new System.DateTime(2018, 12, 1),
                JalonAvancementvalider = new System.DateTime(2018, 12, 1),
                JalonValidationControleBudgetaire = new System.DateTime(2018, 12, 1),
                JalonCiCloturer = new System.DateTime(2018, 12, 1),
            });

            model.UsersCiJalons.Add(new UserJalonSummary
            {
                CiId = 123459,
                Labelle = "123457 CI17",
                JalonTransfertFar = new System.DateTime(2018, 11, 1),
                JalonClotureDepense = new System.DateTime(2018, 11, 1),
                ColorJalonClotureDepense = ColorJalon.ColorGreen,
                JalonAvancementvalider = new System.DateTime(2018, 11, 1),
                JalonValidationControleBudgetaire = new System.DateTime(2018, 10, 1),
                JalonCiCloturer = new System.DateTime(2018, 10, 1),
            });
            model.UsersCiJalons.Add(new UserJalonSummary
            {
                CiId = 123459,
                Labelle = "123457 CI18",
                JalonTransfertFar = new System.DateTime(2018, 12, 1),
                JalonClotureDepense = new System.DateTime(2018, 11, 1),
                JalonAvancementvalider = new System.DateTime(2018, 11, 1),
                ColorJalonAvancementvalider = ColorJalon.ColorOrange,
                JalonValidationControleBudgetaire = new System.DateTime(2018, 11, 1),
                JalonCiCloturer = new System.DateTime(2018, 11, 1),
            });

            model.UsersCiJalons.Add(new UserJalonSummary
            {
                CiId = 123459,
                Labelle = "123457 CI19",
                JalonTransfertFar = null,
                JalonClotureDepense = new System.DateTime(2018, 12, 1),
                JalonAvancementvalider = null,
                JalonValidationControleBudgetaire = new System.DateTime(2018, 11, 1),
                ColorJalonValidationControleBudgetaire = ColorJalon.ColorRed,
                JalonCiCloturer = new System.DateTime(2018, 11, 1),
            });
            return View(model);
        }
    }
}
