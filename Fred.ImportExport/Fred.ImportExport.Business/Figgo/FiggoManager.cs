using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fred.Business.Affectation;
using Fred.Business.Astreinte;
using Fred.Business.CI;
using Fred.Business.Params;
using Fred.Business.Personnel;
using Fred.Business.Rapport;
using Fred.Business.Rapport.Common.RapportParStatut;
using Fred.Business.Rapport.Pointage;
using Fred.Business.Referential;
using Fred.Business.Referential.CodeAbsence;
using Fred.Business.Referential.Tache;
using Fred.Business.Societe;
using Fred.Business.Utilisateur;
using Fred.Entities.Affectation;
using Fred.Entities.CI;
using Fred.Entities.Personnel;
using Fred.Entities.Rapport;
using Fred.Entities.Referential;
using Fred.Entities.Societe;
using Fred.Entities.Utilisateur;
using Fred.ImportExport.DataAccess.ExternalService;
using Fred.ImportExport.Models.Figgo;
using static Fred.Entities.Constantes;

namespace Fred.ImportExport.Business.Figgo
{
    /// <summary>
    /// Manager de l'import des Flux Figgo
    /// </summary>
    public class FiggoManager : IFiggoManager
    {
        private readonly IRapportManager rapportManager;
        private readonly IPointageManager pointageManager;
        private readonly IPersonnelManager personnelManager;
        private readonly ISocieteManager societeManager;
        private readonly IEtablissementPaieManager etablissementPaieManager;
        private readonly ICIManager ciManager;
        private readonly ICodeAbsenceManager codeAbsenceManager;
        private readonly IParamsManager paramsManager;
        private readonly IAffectationManager affectationManager;
        private readonly ICodeMajorationManager codeMajorationManager;
        private readonly IUtilisateurManager utilisateurManager;
        private readonly ITacheManager tacheManager;
        private readonly IAstreinteManager astreinteManager;

        public FiggoManager(
            IRapportManager rapportManager,
            IPointageManager pointageManager,
            IPersonnelManager personnelManager,
            ISocieteManager societeManager,
            IEtablissementPaieManager etablissementPaieManager,
            ICIManager ciManager,
            ICodeAbsenceManager codeAbsenceManager,
            IParamsManager paramsManager,
            IAffectationManager affectationManager,
            ICodeMajorationManager codeMajorationManager,
            IUtilisateurManager utilisateurManager,
            ITacheManager tacheManager,
            IAstreinteManager astreinteManager)
        {
            this.rapportManager = rapportManager;
            this.pointageManager = pointageManager;
            this.personnelManager = personnelManager;
            this.societeManager = societeManager;
            this.etablissementPaieManager = etablissementPaieManager;
            this.ciManager = ciManager;
            this.codeAbsenceManager = codeAbsenceManager;
            this.paramsManager = paramsManager;
            this.affectationManager = affectationManager;
            this.codeMajorationManager = codeMajorationManager;
            this.utilisateurManager = utilisateurManager;
            this.tacheManager = tacheManager;
            this.astreinteManager = astreinteManager;
        }

        /// <summary>
        /// Gestion des Absences Reçu par Figgo
        /// </summary>
        /// <param name="absence">absence reçu par figgo</param>
        /// <returns>Dictionnaire contenant un jsonErrorFiggo et true si il y'a une erreur</returns>
        public async Task<JsonErrorFiggo> ImportAbsenceFiggoAsync(FiggoAbsenceModel absence)
        {
            LogFiggo modalError = new LogFiggo();
            JsonErrorFiggo jsonErrorFiggo = new JsonErrorFiggo();
            Dictionary<JsonErrorFiggo, bool> dictionnairErreur = new Dictionary<JsonErrorFiggo, bool>();
            Dictionary<JsonErrorFiggo, bool> dictionnairSent;
            var fredIeUtilisateur = GetFredIeUtilisateur();
            dictionnairSent = CheckFonctionalError(absence, modalError, dictionnairErreur);
            if (dictionnairSent.FirstOrDefault().Value)
            {
                return dictionnairSent.FirstOrDefault().Key;
            }
            dictionnairSent.Clear();
            var cient = GetCiEntBySociete(absence);
            if (cient == null)
            {
                string errorMessage = " Ce personnel ne posséde pas de Ci Absence ";
                NLog.LogManager.GetCurrentClassLogger().Error("[FIGGO] : " + errorMessage);
                dictionnairSent.Add(FillJsonError(absence, false, errorMessage), true);
                return dictionnairSent.FirstOrDefault().Key;
            }
            var societe = GetSociete(absence.SocieteCode);
            var personnel = GetPersonnel(societe.SocieteId, absence.Matricule);
            int typePersonnel = RapportStatutHelper.GetTypePersonnel(personnel.Statut);
            RapportEnt rapport = rapportManager.GetRapportsByPersonnelIdAndDatePointagesFiggo(cient.CiId, absence.DateAbsence, personnel.PersonnelId, typePersonnel);
            //RG0-04
            if (CheckIfRapportExiste(rapport, personnel))
            {
                return CheckIfOtherPointageExist(absence, fredIeUtilisateur.UtilisateurId, personnel, rapport, typePersonnel).FirstOrDefault().Key;
            }
            else
            {
                var listCi = affectationManager.GetPersonnelAffectationCiList(personnel.PersonnelId).Select(x => x.CiId);
                List<RapportEnt> listRapport = rapportManager.GetAllRapportsByPersonnelIdAndDatePointagesFiggo(listCi.ToList(), absence.DateAbsence, personnel.PersonnelId, typePersonnel);
                var listOtherPointage = listRapport.Where(x => x.ListLignes.Any(xx => xx.DateSuppression == null && xx.PersonnelId.Equals(personnel.PersonnelId))).ToList();
                List<RapportLigneEnt> listrapportLignes = rapportManager.GetRapportLigneByDateAndPersonnelAndCi(absence.DateAbsence, personnel.PersonnelId, cient.CiId);
                if (CheckIfRapportLigneExiste(absence, listrapportLignes.Where(x => x.DateSuppression == null).ToList(), personnel.PersonnelId, listOtherPointage) && absence.AbsenceStatut != Constantes.Annulle)
                {
                    CreateRapportLigne(rapport, absence, fredIeUtilisateur.UtilisateurId);
                    modalError.NombreLigneOk += 1;
                    NLog.LogManager.GetCurrentClassLogger().Info("[FIGGO] : " + FredImportExportBusinessResources.ImportMessageOk);
                    dictionnairSent.Add(FillJsonError(absence, true, FredImportExportBusinessResources.ImportMessageOk), true);
                    return dictionnairSent.FirstOrDefault().Key;
                }
                else
                {
                    dictionnairSent = DeleteAbsence_006(absence, fredIeUtilisateur.PersonnelId, dictionnairErreur, typePersonnel);
                    if (dictionnairSent.FirstOrDefault().Value)
                    {
                        return dictionnairSent.FirstOrDefault().Key;
                    }
                    dictionnairSent.Clear();
                    dictionnairSent = await CheckUpdateIfPointageExiste_005Async(absence, fredIeUtilisateur.PersonnelId, dictionnairErreur);
                    if (dictionnairSent.FirstOrDefault().Value)
                    {
                        return dictionnairSent.FirstOrDefault().Key;
                    }
                }
            }
            return jsonErrorFiggo;
        }

        private Dictionary<JsonErrorFiggo, bool> CheckIfOtherPointageExist(FiggoAbsenceModel absence, int utilisateurId, PersonnelEnt personnel, RapportEnt rapport, int typePersonnel)
        {
            Dictionary<JsonErrorFiggo, bool> dictionnairSent = new Dictionary<JsonErrorFiggo, bool>();
            RapportLigneEnt rapportLigne;
            rapportLigne = MapRapportLigneModel(absence, utilisateurId);
            var listCi = affectationManager.GetPersonnelAffectationCiList(personnel.PersonnelId).Select(x => x.CiId);
            List<RapportEnt> listRapport = rapportManager.GetAllRapportsByPersonnelIdAndDatePointagesFiggo(listCi.ToList(), absence.DateAbsence, personnel.PersonnelId, typePersonnel);
            var listRapports = listRapport.Where(x => x.ListLignes.Any(xx => xx.DateSuppression == null)).ToList();
            if (listRapports.Any() && listRapports.Any(x => x.ListLignes.Any(rpl => rpl.PersonnelId.Equals(personnel.PersonnelId))))
            {
                dictionnairSent = CheckIFdayVerrouille(listRapport, absence, personnel.PersonnelId);
                if (dictionnairSent.FirstOrDefault().Value)
                {
                    return dictionnairSent;
                }
                else
                {
                    return CheckAddOrDeleteExistinPointage(listRapport, absence, utilisateurId, rapportLigne, personnel, rapport);
                }
            }
            else
            {
                dictionnairSent = DeleteAbsence_006(absence, utilisateurId, dictionnairSent, typePersonnel);
                if (dictionnairSent.FirstOrDefault().Value)
                {
                    return dictionnairSent;
                }
                dictionnairSent.Clear();
                CreateRapportEnt(absence, utilisateurId, rapportLigne);
                NLog.LogManager.GetCurrentClassLogger().Info("[FIGGO] : " + FredImportExportBusinessResources.ImportMessageOk);
                dictionnairSent.Add(FillJsonError(absence, true, FredImportExportBusinessResources.ImportMessageOk), true);
                return dictionnairSent;
            }
        }
        private Dictionary<JsonErrorFiggo, bool> CheckAddOrDeleteExistinPointage(List<RapportEnt> listRapport, FiggoAbsenceModel absence, int utilisateurId, RapportLigneEnt rapportLigne, PersonnelEnt personnel, RapportEnt rapport)
        {
            Dictionary<JsonErrorFiggo, bool> dictionnairSent = new Dictionary<JsonErrorFiggo, bool>();
            dictionnairSent = DeleteAbsence_006(absence, utilisateurId, dictionnairSent, RapportStatutHelper.GetTypePersonnel(personnel.Statut));
            if (dictionnairSent.FirstOrDefault().Value)
            {
                return dictionnairSent;
            }
            else
            {
                return AddOrDeleteExistinPointage(listRapport, absence, utilisateurId, rapportLigne, personnel, rapport);
            }
        }

        private Dictionary<JsonErrorFiggo, bool> AddOrDeleteExistinPointage(List<RapportEnt> listRapport, FiggoAbsenceModel absence, int utilisateurId, RapportLigneEnt rapportLigne, PersonnelEnt personnel, RapportEnt rapport)
        {
            Dictionary<JsonErrorFiggo, bool> dictionnairSent = new Dictionary<JsonErrorFiggo, bool>();
            if (absence.Quantite == Constantes.DAY)
            {
                return ProcessExistingPointageDay(listRapport, utilisateurId, absence, rapportLigne, personnel.PersonnelId);
            }
            double total = 0;
            var code = GetListCodeMajorationBySociete(absence).Select(m => m.CodeMajorationId);
            foreach (var item in listRapport)
            {
                foreach (var rapportlignes in item.ListLignes.Where(x => x.DateSuppression == null && x.PersonnelId.Equals(personnel.PersonnelId)))
                {
                    total += rapportlignes.HeureAbsence + rapportlignes.ListRapportLigneTaches.Sum(x => x.HeureTache) + rapportlignes.ListRapportLigneMajorations.Where(m => m.CodeMajorationId.Equals(code.Any())).Sum(m => m.HeureMajoration);
                }
            }
            if (total <= GetHeureAbsence(absence))
            {
                if (rapport != null)
                {
                    return UpdateOrCreateRapportLigne(rapport, absence, dictionnairSent, utilisateurId);
                }
                else
                {
                    CreateRapportEnt(absence, utilisateurId, rapportLigne);
                    NLog.LogManager.GetCurrentClassLogger().Info("[FIGGO] : " + FredImportExportBusinessResources.ImportMessageOk);
                    dictionnairSent.Add(FillJsonError(absence, true, FredImportExportBusinessResources.ImportMessageOk), true);
                    return dictionnairSent;
                }
            }
            if (total > GetHeureAbsence(absence))
            {
                foreach (var item in listRapport)
                {
                    if (item.ListLignes != null && item.ListLignes.Any())
                    {
                        pointageManager.DeletePointageList(item.ListLignes, true, utilisateurId);
                    }
                }

                var defaultCi = affectationManager.GetDefaultCi(personnel.PersonnelId);
                //Mise à jour les heures sur tâche sur le CI par défaut du personnel 
                CreateRapportEnt(absence, utilisateurId, rapportLigne, defaultCi.CiId, personnel.PersonnelId);
                CreateRapportEnt(absence, utilisateurId, rapportLigne);
                string errorMessage = "Le pointage saisi dans FRED du personnel " + absence.Matricule + " de la société " + absence.SocieteCode + " pour la journée du " + absence.DateAbsence.ToShortDateString() + " a été supprimé  ";
                NLog.LogManager.GetCurrentClassLogger().Info("[FIGGO] : " + errorMessage);
                dictionnairSent.Add(FillJsonError(absence, true, errorMessage), true);
                return dictionnairSent;
            }

            dictionnairSent.Add(FillJsonError(absence, false, null), false);
            return dictionnairSent;
        }
        private Dictionary<JsonErrorFiggo, bool> ProcessExistingPointageDay(List<RapportEnt> listRapport, int utilisateurId, FiggoAbsenceModel absence, RapportLigneEnt rapportLigne, int personnelId)
        {
            Dictionary<JsonErrorFiggo, bool> dictionnairSent = new Dictionary<JsonErrorFiggo, bool>();
            foreach (var item in listRapport)
            {
                IEnumerable<RapportLigneEnt> pointagesToDelete = item.ListLignes.Where(x => x.PersonnelId == personnelId);
                if (pointagesToDelete != null && pointagesToDelete.Any())
                {
                    pointageManager.DeletePointageList(pointagesToDelete, true, utilisateurId);
                }
            }
            CreateRapportEnt(absence, utilisateurId, rapportLigne);
            string errorMessage = "Le pointage saisi dans FRED du personnel " + absence.Matricule + " de la société " + absence.SocieteCode + " pour la journée du " + absence.DateAbsence.ToShortDateString() + " a été supprimé  ";
            NLog.LogManager.GetCurrentClassLogger().Info("[FIGGO] : " + errorMessage);
            dictionnairSent.Add(FillJsonError(absence, true, errorMessage), true);
            return dictionnairSent;
        }
        private Dictionary<JsonErrorFiggo, bool> CheckIFdayVerrouille(List<RapportEnt> listRapport, FiggoAbsenceModel absence, int personnelId)
        {
            Dictionary<JsonErrorFiggo, bool> dictionnairSent = new Dictionary<JsonErrorFiggo, bool>();
            foreach (var rapport in listRapport)
            {
                if (rapport.ListLignes.Any(x => x.RapportLigneStatutId == Constantes.RapportVerrouilleId && x.PersonnelId.Equals(personnelId)) && absence.AbsenceStatut != Constantes.Annulle)
                {
                    string errorMessage = "L'absence du " + absence.DateAbsence.ToShortDateString() + " du matricule " + absence.Matricule + " de la société " + absence.SocieteCode + " n'a pas pu être intégrée car la journée est verrouillé ";
                    NLog.LogManager.GetCurrentClassLogger().Error("[FIGGO] : " + errorMessage);
                    dictionnairSent.Add(FillJsonError(absence, false, errorMessage), true);
                    return dictionnairSent;
                }
                if (rapport.ListLignes.Any(x => x.RapportLigneStatutId == Constantes.RapportVerrouilleId && x.PersonnelId.Equals(personnelId)) && absence.AbsenceStatut == Constantes.Annulle)
                {
                    string errorMessage = "L’absence du " + absence.DateAbsence.ToShortDateString() + " du matricule " + absence.Matricule + " de la société" + absence.SocieteCode + " n’a pas pu être supprimée car la journée est verrouillée ";
                    NLog.LogManager.GetCurrentClassLogger().Error("[FIGGO] : " + errorMessage);
                    dictionnairSent.Add(FillJsonError(absence, false, errorMessage), true);
                    return dictionnairSent;
                }
            }
            dictionnairSent.Add(FillJsonError(absence, false, null), false);
            return dictionnairSent;
        }
        private Dictionary<JsonErrorFiggo, bool> CheckFonctionalError(FiggoAbsenceModel absenceFiggomodel, LogFiggo logFiggo, Dictionary<JsonErrorFiggo, bool> dictionnairErreur)
        {
            JsonErrorFiggo jsonErrorFiggo = new JsonErrorFiggo();
            Dictionary<JsonErrorFiggo, bool> dictionnairSent;
            dictionnairSent = CheckIfFormatDateError(absenceFiggomodel, dictionnairErreur);
            if (dictionnairSent.FirstOrDefault().Value)
            {
                return dictionnairSent;
            }
            dictionnairSent.Clear();
            //Pour la societe du personnel
            dictionnairSent = CheckIfSocieteExist(absenceFiggomodel, logFiggo, dictionnairErreur);
            if (dictionnairSent.FirstOrDefault().Value)
            {
                return dictionnairSent;
            }
            dictionnairSent.Clear();
            dictionnairSent = CheckIfPersonnelExist(GetSocieteId(absenceFiggomodel.SocieteCode).Value, logFiggo, dictionnairErreur, absenceFiggomodel);//Pour le personnel 
            if (dictionnairSent.ContainsValue(true))
            {
                return dictionnairSent;
            }
            dictionnairSent.Clear();
            dictionnairSent = CheckIfCodeAbsenceExist(absenceFiggomodel, logFiggo, dictionnairErreur);
            if (dictionnairSent.FirstOrDefault().Value)
            {
                return dictionnairSent;
            }
            dictionnairSent.Clear();
            dictionnairSent = CheckIfPersonnelHasDefaultCi(absenceFiggomodel, logFiggo, dictionnairErreur);
            if (dictionnairSent.FirstOrDefault().Value)
            {
                return dictionnairSent;
            }
            dictionnairSent.Clear();
            dictionnairSent = CheckIfStatutAbsenceExist(absenceFiggomodel, dictionnairErreur);
            if (dictionnairSent.FirstOrDefault().Value)
            {
                return dictionnairSent;
            }
            dictionnairSent.Clear();
            dictionnairSent = CheckIfQuantiteExiste(absenceFiggomodel, logFiggo, dictionnairErreur);
            if (dictionnairSent.FirstOrDefault().Value)
            {
                return dictionnairSent;
            }
            dictionnairSent.Clear();
            if (!string.IsNullOrEmpty(absenceFiggomodel.SocieteCodeValideur))
            {
                dictionnairSent = CheckIfSocieteValideurExist(absenceFiggomodel, logFiggo, dictionnairErreur);   //Pour la societe du valideur
                if (dictionnairSent.FirstOrDefault().Value)
                {
                    return dictionnairSent;
                }
                dictionnairSent.Clear();
                dictionnairSent = CheckIfValideurExist(GetSocieteId(absenceFiggomodel.SocieteCodeValideur).Value, logFiggo, dictionnairErreur, absenceFiggomodel);  //Pour le Valideur  
                if (dictionnairSent.FirstOrDefault().Value)
                {
                    return dictionnairSent;
                }
            }
            dictionnairSent.Add(jsonErrorFiggo, false);
            return dictionnairSent;
        }
        private Dictionary<JsonErrorFiggo, bool> CheckIfFormatDateError(FiggoAbsenceModel absenceFiggomodel, Dictionary<JsonErrorFiggo, bool> dictionnairErreur)
        {
            if (absenceFiggomodel.DateAbsence.Hour != 0 || absenceFiggomodel.DateAbsence.Minute != 0 || absenceFiggomodel.DateAbsence.Second != 0)
            {
                string errorMessage = "L'absence " + absenceFiggomodel.AbsenceCode + " du " + absenceFiggomodel.DateAbsence + " du matricule " + absenceFiggomodel.Matricule + " de la société " + absenceFiggomodel.SocieteCode + " ne respecte pas le format de date d'absence AAAA-MM-DDT00:00:00 ";
                NLog.LogManager.GetCurrentClassLogger().Error("[FIGGO] : " + errorMessage);
                dictionnairErreur.Add(FillJsonError(absenceFiggomodel, false, errorMessage), true);
                return dictionnairErreur;
            }
            dictionnairErreur.Add(FillJsonError(absenceFiggomodel, false, null), false);
            return dictionnairErreur;

        }
        private Dictionary<JsonErrorFiggo, bool> CheckIfStatutAbsenceExist(FiggoAbsenceModel absenceModel, Dictionary<JsonErrorFiggo, bool> dictionnairErreur)
        {
            JsonErrorFiggo jsonErrorFiggo = new JsonErrorFiggo();
            if (absenceModel.AbsenceStatut == Constantes.Annulle || absenceModel.AbsenceStatut == Constantes.Valide || absenceModel.AbsenceStatut == Constantes.ENCOURS)
            {
                dictionnairErreur.Add(jsonErrorFiggo, false);
                return dictionnairErreur;
            }
            string errorMessage = "Le statut " + absenceModel.AbsenceStatut + " n’est pas reconnu ";
            NLog.LogManager.GetCurrentClassLogger().Error("[FIGGO] : " + errorMessage);
            dictionnairErreur.Add(FillJsonError(absenceModel, false, errorMessage), true);
            return dictionnairErreur;
        }
        private Dictionary<JsonErrorFiggo, bool> CheckIfQuantiteExiste(FiggoAbsenceModel absenceModel, LogFiggo logFiggo, Dictionary<JsonErrorFiggo, bool> dictionnairErreur)
        {
            JsonErrorFiggo jsonErrorFiggo = new JsonErrorFiggo();
            if (absenceModel.Quantite == Constantes.DAY || absenceModel.Quantite == Constantes.PM || absenceModel.Quantite == Constantes.AM)
            {
                dictionnairErreur.Add(jsonErrorFiggo, false);
                return dictionnairErreur;
            }
            string errorMessage = " Quantité " + absenceModel.Quantite + " inconnue  ";
            NLog.LogManager.GetCurrentClassLogger().Error("[FIGGO] : " + errorMessage);
            logFiggo.NombreLigneError += 1;
            dictionnairErreur.Add(FillJsonError(absenceModel, false, errorMessage), true);
            return dictionnairErreur;
        }
        private Dictionary<JsonErrorFiggo, bool> CheckIfPersonnelHasDefaultCi(FiggoAbsenceModel absenceModel, LogFiggo logFiggo, Dictionary<JsonErrorFiggo, bool> dictionnairErreur)
        {
            JsonErrorFiggo jsonErrorFiggo = new JsonErrorFiggo();
            if (string.IsNullOrEmpty(absenceModel.SocieteCode) || !CheckDefaultCi(absenceModel))
            {
                string errorMessage = "Le matricule " + absenceModel.Matricule + " du personnel de la société " + absenceModel.SocieteCode + " n’a pas de CI par défaut";
                NLog.LogManager.GetCurrentClassLogger().Error("[FIGGO] : " + errorMessage);
                logFiggo.NombreLigneError += 1;

                dictionnairErreur.Add(FillJsonError(absenceModel, false, errorMessage), true);
                return dictionnairErreur;
            }
            dictionnairErreur.Add(jsonErrorFiggo, false);
            return dictionnairErreur;
        }
        /// <summary>
        /// verifier si le personnel a ci par defaut
        /// </summary>
        /// <param name="absenceModel">Model reçu par figgo</param>
        /// <returns>true si le personnel a un ci par defaut</returns>
        private bool CheckDefaultCi(FiggoAbsenceModel absenceModel)
        {
            var personnelId = GetPersonnelId(absenceModel);
            var defaultCi = affectationManager.GetDefaultCi(personnelId.Value);
            if (defaultCi != null)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Verifie si le code absence existe dans le referentiel Fred
        /// </summary>
        /// <param name="absenceModel">absence reçu par figgo</param>
        /// <param name="logFiggo">log figgo</param>
        /// <param name="dictionnairErreur">dictionnaire</param>
        /// <returns>Dictionnaire contenant un jsonErrorFiggo et true si il y'a une erreur</returns>
        private Dictionary<JsonErrorFiggo, bool> CheckIfCodeAbsenceExist(FiggoAbsenceModel absenceModel, LogFiggo logFiggo, Dictionary<JsonErrorFiggo, bool> dictionnairErreur)
        {
            var code = codeAbsenceManager.GetCodeAbsenceByCode(absenceModel.AbsenceCode);
            JsonErrorFiggo jsonErrorFiggo = new JsonErrorFiggo();
            if (code == null)
            {
                string errorMessage = "Le code absence " + absenceModel.AbsenceCode + " n’est pas reconnu ";
                NLog.LogManager.GetCurrentClassLogger().Error("[FIGGO] : " + errorMessage);
                logFiggo.NombreLigneError += 1;
                dictionnairErreur.Add(FillJsonError(absenceModel, false, errorMessage), true);
                return dictionnairErreur;
            }
            dictionnairErreur.Add(jsonErrorFiggo, false);
            return dictionnairErreur;
        }
        /// <summary>
        /// Verifie si la societe du valideur existe
        /// </summary>
        /// <param name="absenceModel">absence reçu par figgo</param>
        /// <param name="logFiggo">log figgo</param>
        /// <param name="dictionnairErreur">dictionnaire</param>
        /// <returns>Dictionnaire contenant un jsonErrorFiggo et true si il y'a une erreur</returns>
        private Dictionary<JsonErrorFiggo, bool> CheckIfSocieteValideurExist(FiggoAbsenceModel absenceModel, LogFiggo logFiggo, Dictionary<JsonErrorFiggo, bool> dictionnairErreur)
        {
            JsonErrorFiggo jsonErrorFiggo = new JsonErrorFiggo();
            SocieteEnt societe = GetSociete(absenceModel.SocieteCodeValideur);
            if (societe == null)
            {
                string errorMessage = "La société " + absenceModel.SocieteCodeValideur + " du valideur n’existe pas ";
                NLog.LogManager.GetCurrentClassLogger().Error("[FIGGO] : " + errorMessage);
                logFiggo.NombreLigneError += 1;
                dictionnairErreur.Add(FillJsonError(absenceModel, false, errorMessage), true);
                return dictionnairErreur;
            }
            dictionnairErreur.Add(jsonErrorFiggo, false);
            return dictionnairErreur;
        }
        /// <summary>
        /// Verifie si la societe  existe
        /// </summary>
        /// <param name="absenceModel">absence reçu par figgo</param>
        /// <param name="logFiggo">log figgo</param>
        /// <param name="dictionnairErreur">dictionnaire</param>
        /// <returns>Dictionnaire contenant un jsonErrorFiggo et true si il y'a une erreur</returns>
        private Dictionary<JsonErrorFiggo, bool> CheckIfSocieteExist(FiggoAbsenceModel absenceModel, LogFiggo logFiggo, Dictionary<JsonErrorFiggo, bool> dictionnairErreur)
        {
            JsonErrorFiggo jsonErrorFiggo = new JsonErrorFiggo();
            SocieteEnt societe = GetSociete(absenceModel.SocieteCode);
            if (societe == null)
            {
                string errorMessage = "La société " + absenceModel.SocieteCode + " du personnel n’existe pas ";
                NLog.LogManager.GetCurrentClassLogger().Error("[FIGGO] : " + errorMessage);
                logFiggo.NombreLigneError += 1;
                dictionnairErreur.Add(FillJsonError(absenceModel, false, errorMessage), true);
                return dictionnairErreur;
            }
            dictionnairErreur.Add(jsonErrorFiggo, false);
            return dictionnairErreur;
        }

        private JsonErrorFiggo FillJsonError(FiggoAbsenceModel absenceModel, bool integrationOk, string errorMessage = null)
        {
            JsonErrorFiggo jsonErrorFiggo = new JsonErrorFiggo();
            jsonErrorFiggo.SocieteCode = absenceModel.SocieteCode;
            jsonErrorFiggo.Matricule = absenceModel.Matricule;
            jsonErrorFiggo.AbsenceCode = absenceModel.AbsenceCode;
            jsonErrorFiggo.DateAbsence = absenceModel.DateAbsence;
            if (integrationOk)
            {
                jsonErrorFiggo.Information = errorMessage;
            }
            else
            {
                jsonErrorFiggo.Erreur = errorMessage;
            }
            return jsonErrorFiggo;
        }
        /// <summary>
        /// Verifie si le valideur existe
        /// </summary>
        /// <param name="societeId">identifiant de la societe</param>
        /// <param name="logFiggo">model pour les log</param>
        /// <param name="dictionnairErreur">Dictionnaire contenant un jsonErreurFiggo et true si il y'a une erreur </param>
        /// <param name="absenceFiggomodel">absence reçu de figgo</param>
        /// <returns>Dictionnaire contenant un jsonErrorFiggo et true si il y'a une erreur</returns>
        private Dictionary<JsonErrorFiggo, bool> CheckIfPersonnelExist(int societeId, LogFiggo logFiggo, Dictionary<JsonErrorFiggo, bool> dictionnairErreur, FiggoAbsenceModel absenceFiggomodel)
        {
            PersonnelEnt personnel = personnelManager.GetPersonnel(societeId, absenceFiggomodel.Matricule);
            JsonErrorFiggo jsonErrorFiggo = new JsonErrorFiggo();
            if (personnel == null)
            {
                string errorMessage = "Le matricule " + absenceFiggomodel.Matricule + " du personnel de la société " + absenceFiggomodel.SocieteCode + " n’existe pas ";
                NLog.LogManager.GetCurrentClassLogger().Error("[FIGGO] : " + errorMessage);
                logFiggo.NombreLigneError += 1;
                dictionnairErreur.Add(FillJsonError(absenceFiggomodel, false, errorMessage), true);
                return dictionnairErreur;
            }
            dictionnairErreur.Add(jsonErrorFiggo, false);
            return dictionnairErreur;
        }
        /// <summary>
        /// Verifie si le valideur existe
        /// </summary>
        /// <param name="societeId">identifiant de la societe</param>
        /// <param name="logFiggo">model pour les log</param>
        /// <param name="dictionnairErreur">Dictionnaire contenant un jsonErreurFiggo et true si il y'a une erreur </param>
        /// <param name="absenceFiggomodel">absence reçu de figgo</param>
        /// <returns>Dictionnaire contenant un jsonErrorFiggo et true si il y'a une erreur</returns>
        private Dictionary<JsonErrorFiggo, bool> CheckIfValideurExist(int societeId, LogFiggo logFiggo, Dictionary<JsonErrorFiggo, bool> dictionnairErreur, FiggoAbsenceModel absenceFiggomodel)
        {
            PersonnelEnt personnel = personnelManager.GetPersonnel(societeId, absenceFiggomodel.MatriculeValideur);
            JsonErrorFiggo jsonErrorFiggo = new JsonErrorFiggo();
            if (personnel == null)
            {
                string errorMessage = "Le matricule " + absenceFiggomodel.MatriculeValideur + " du valideur de la société " + absenceFiggomodel.SocieteCodeValideur + " n’existe pas ";
                NLog.LogManager.GetCurrentClassLogger().Error("[FIGGO] : " + errorMessage);

                logFiggo.NombreLigneError += 1;

                dictionnairErreur.Add(FillJsonError(absenceFiggomodel, false, errorMessage), true);
                return dictionnairErreur;
            }
            dictionnairErreur.Add(jsonErrorFiggo, false);
            return dictionnairErreur;
        }
        private Dictionary<JsonErrorFiggo, bool> DeleteAbsence_006(FiggoAbsenceModel absenceFiggomodel, int? currentuser, Dictionary<JsonErrorFiggo, bool> dictionnairErreur, int typePersonnel)
        {
            JsonErrorFiggo jsonErrorFiggo = new JsonErrorFiggo();
            var code = codeAbsenceManager.GetCodeAbsenceByCode(absenceFiggomodel.AbsenceCode);
            if (absenceFiggomodel.DateSuppression.HasValue || absenceFiggomodel.AbsenceStatut == Constantes.Annulle)
            {
                var ciAbsence = GetCiEntBySociete(absenceFiggomodel);
                var rapportEnt = rapportManager.GetRapportsByCilIdAndDatePointagesFiggo(ciAbsence.CiId, absenceFiggomodel.DateAbsence, typePersonnel);
                if (rapportEnt == null || rapportEnt.ListLignes.Any(x => x.DateSuppression.HasValue && x.TypeAbsence == absenceFiggomodel.Quantite && x.CodeAbsenceId == code.CodeAbsenceId) && !rapportEnt.ListLignes.Any(x => !x.DateSuppression.HasValue && x.TypeAbsence == absenceFiggomodel.Quantite && x.CodeAbsenceId == code.CodeAbsenceId))
                {
                    string errorMessage = "L’annulation de l’absence " + absenceFiggomodel.AbsenceCode + " pointée sur le personnel " + absenceFiggomodel.Matricule + " de la société " + absenceFiggomodel.SocieteCode + " pour la date du " + absenceFiggomodel.DateAbsence.ToShortDateString() + " n’a pas pu être traitée car celle-ci n’existe pas dans FRED ";
                    NLog.LogManager.GetCurrentClassLogger().Error("[FIGGO] : " + errorMessage);
                    dictionnairErreur.Add(FillJsonError(absenceFiggomodel, false, errorMessage), true);
                    return dictionnairErreur;
                }
                if (rapportEnt.ListLignes.Any(x => x.RapportLigneStatutId == Constantes.RapportVerrouilleId))
                {
                    string errorMessage = "L’absence du " + absenceFiggomodel.DateAbsence.ToShortDateString() + " du matricule " + absenceFiggomodel.Matricule + " de la société " + absenceFiggomodel.SocieteCode + " n’a pas pu être supprimée car la journée est verrouillée ";
                    NLog.LogManager.GetCurrentClassLogger().Error("[FIGGO] : " + errorMessage);
                    dictionnairErreur.Add(FillJsonError(absenceFiggomodel, false, errorMessage), true);
                    return dictionnairErreur;
                }
                if (rapportEnt.RapportStatutId != Constantes.RapportVerrouilleId)
                {
                    return DeleteAbsenceOrThrowNotExist(dictionnairErreur, absenceFiggomodel, currentuser, ciAbsence.CiId, code.CodeAbsenceId);
                }
            }
            dictionnairErreur.Add(jsonErrorFiggo, false);
            return dictionnairErreur;
        }
        public Dictionary<JsonErrorFiggo, bool> DeleteAbsenceOrThrowNotExist(Dictionary<JsonErrorFiggo, bool> dictionnairErreur, FiggoAbsenceModel absenceFiggomodel, int? currentuser, int ciId, int codeAbsenceId)
        {
            var personnelId = GetPersonnelId(absenceFiggomodel);
            var listpointageCiAbsence = rapportManager.GetRapportLigneByCiAndPersonnels(ciId, personnelId.Value, absenceFiggomodel.DateAbsence);
            var pointageCiAbsences = listpointageCiAbsence.FirstOrDefault(x => x.TypeAbsence == absenceFiggomodel.Quantite && x.DateSuppression == null && x.CodeAbsenceId == codeAbsenceId);
            if (pointageCiAbsences == null)
            {
                string errorMessage = "L’annulation de l’absence " + absenceFiggomodel.AbsenceCode + " pointée sur le personnel " + absenceFiggomodel.Matricule + " de la société " + absenceFiggomodel.SocieteCode + " pour la date du " + absenceFiggomodel.DateAbsence.ToShortDateString() + " n’a pas pu être traitée car celle-ci n’existe pas dans FRED ";
                NLog.LogManager.GetCurrentClassLogger().Error("[FIGGO] : " + errorMessage);
                dictionnairErreur.Add(FillJsonError(absenceFiggomodel, false, errorMessage), true);
                return dictionnairErreur;
            }

            pointageCiAbsences.DateModification = DateTime.Now;
            pointageCiAbsences.AuteurModificationId = currentuser;
            pointageManager.DeletePointage(pointageCiAbsences, true, currentuser);

            string message = "Suppression Ok";
            NLog.LogManager.GetCurrentClassLogger().Info("[FIGGO] : " + message);
            dictionnairErreur.Add(FillJsonError(absenceFiggomodel, true, message), true);
            return dictionnairErreur;
        }
        public static string GetStatut(string statutId)
        {
            switch (statutId)
            {
                case "1":
                    return "HeuresJourOuvrierETAM";
                case "2":
                    return "HeuresJourOuvrierETAM";
                case "3":
                    return "HeuresJourIAC";
                case "4":
                    return "HeuresJourOuvrierETAM";
                case "5":
                    return "HeuresJourOuvrierETAM";
                default:
                    return string.Empty;
            }
        }
        private void CreateRapportEnt(FiggoAbsenceModel absenceFiggomodel, int? currentuser, RapportLigneEnt rapportLigne, int? ciId = null, int? personnelId = null)
        {
            PersonnelEnt personnel = GetPersonnel(GetSocieteId(absenceFiggomodel.SocieteCode).Value, absenceFiggomodel.Matricule);
            if (ciId != null && personnelId != null)
            {
                RapportEnt rapport = MapRapportModel(absenceFiggomodel, currentuser, ciId);
                rapportLigne.HeureAbsence = 0;
                rapportLigne.CodeAbsenceId = null;
                rapportLigne.TypeAbsence = null;
                rapportLigne.CiId = ciId.Value;
                rapport.ListLignes.Add(rapportLigne);
                TacheEnt tacheDefaut = tacheManager.GetTacheParDefaut(ciId.Value);
                double heureJournaliereParametre = GetHeureAbsence(absenceFiggomodel);
                rapport = rapportManager.AddTacheToRapportFiggo(rapport, tacheDefaut, heureJournaliereParametre);
                rapport = RapportStatutHelper.CheckPersonnelStatut(rapport, personnel.Statut);
                foreach (var item in rapport.ListLignes)
                {
                    item.HeureNormale = heureJournaliereParametre;
                }
                rapportManager.AddRapportFiggo(rapport);
            }
            else
            {
                RapportEnt rapport = MapRapportModel(absenceFiggomodel, currentuser, ciId);
                rapport = RapportStatutHelper.CheckPersonnelStatut(rapport, personnel.Statut);
                var rapportligneAbsence = MapRapportLigneModel(absenceFiggomodel, currentuser);
                rapportligneAbsence.CiId = rapport.CiId;
                rapport.ListLignes.Add(rapportligneAbsence);
                rapportManager.AddRapportFiggo(rapport);
            }
        }
        /// <summary>
        /// Verifie si il existe un pointage pour une journee
        /// </summary>
        /// <param name="absenceFiggomodel">Model reçu par Figgo</param>
        /// <param name="currentuser">identifiant de FredIe</param>
        /// <param name="dictionnairErreur">dictionnaire</param>
        /// <returns>Dictionnaire contenant un jsonErrorFiggo et true si il y'a une erreur</returns>
        private async Task<Dictionary<JsonErrorFiggo, bool>> CheckUpdateIfPointageExiste_005Async(FiggoAbsenceModel absenceFiggomodel, int? currentuser, Dictionary<JsonErrorFiggo, bool> dictionnairErreur = null)
        {
            JsonErrorFiggo jsonErrorFiggo = new JsonErrorFiggo();
            Dictionary<JsonErrorFiggo, bool> dictionnaire;
            PersonnelEnt personnel = GetPersonnel(GetSocieteId(absenceFiggomodel.SocieteCode).Value, absenceFiggomodel.Matricule);
            if (personnel == null || personnel.PersonnelId.Equals(0))
            {
                string errorMessage = "Le personnel de la societe " + absenceFiggomodel.SocieteCode + " avec le matricule " + absenceFiggomodel.Matricule + " n'existe pas ";
                NLog.LogManager.GetCurrentClassLogger().Error("[FIGGO] : " + errorMessage);
                dictionnairErreur.Add(FillJsonError(absenceFiggomodel, false, errorMessage), true);
                return dictionnairErreur;
            }
            var ciEnt = GetCiEntBySociete(absenceFiggomodel).CiId;
            int typePersonnel = RapportStatutHelper.GetTypePersonnel(personnel.Statut);
            RapportEnt rapport = rapportManager.GetRapportsByPersonnelIdAndDatePointagesFiggo(ciEnt, absenceFiggomodel.DateAbsence, personnel.PersonnelId, typePersonnel);
            var listCi = affectationManager.GetPersonnelAffectationCiList(personnel.PersonnelId).Select(x => x.CiId).ToList();
            listCi.Add(ciEnt);
            List<RapportEnt> listRapport = rapportManager.GetAllRapportsByPersonnelIdAndDatePointagesFiggo(listCi, absenceFiggomodel.DateAbsence, personnel.PersonnelId, typePersonnel);
            if (rapport.ListLignes.Any(x => x.RapportLigneStatutId == Constantes.RapportVerrouilleId))
            {
                string errorMessage = "L'absence du " + absenceFiggomodel.DateAbsence.ToShortDateString() + " du matricule " + absenceFiggomodel.Matricule + " de la societe " + absenceFiggomodel.SocieteCode + " n'a pas pu être intégrée car la journée est verrouillé ";
                NLog.LogManager.GetCurrentClassLogger().Error("[FIGGO] : " + errorMessage);
                dictionnairErreur.Add(FillJsonError(absenceFiggomodel, false, errorMessage), true);
                return dictionnairErreur;
            }
            dictionnaire = CheckIfDatePointageNotLocked(rapport, absenceFiggomodel, currentuser, dictionnairErreur, personnel.PersonnelId, typePersonnel);
            if (dictionnaire.FirstOrDefault().Value)
            {
                return dictionnaire;
            }
            if (rapport.RapportStatutId != Constantes.RapportVerrouilleId && absenceFiggomodel.Quantite == Constantes.DAY && rapport.ListLignes.Any())
            {
                await DeleteRapportLigneAsync(rapport, currentuser, personnel.PersonnelId);   //Supprime les lignes d'absence
                if (listRapport.Any())
                {
                    foreach (var item in listRapport.Where(x => x.DateSuppression == null))
                    {
                        await DeleteRapportLigneAsync(item, currentuser, personnel.PersonnelId); //Supprime les lignes de pointage sur tout les ci
                    }
                }

                CreateRapportLigne(rapport, absenceFiggomodel, currentuser);
                string errorMessage = "Le pointage saisi dans FRED du personnel " + absenceFiggomodel.Matricule + " de la société " + absenceFiggomodel.SocieteCode + " pour la journée du " + absenceFiggomodel.DateAbsence.ToShortDateString() + " a été supprimé  ";
                NLog.LogManager.GetCurrentClassLogger().Info("[FIGGO] : " + errorMessage);
                dictionnairErreur.Add(FillJsonError(absenceFiggomodel, true, errorMessage), true);
                return dictionnairErreur;
            }
            dictionnairErreur.Add(jsonErrorFiggo, false);
            return dictionnairErreur;
        }
        private async Task DeleteRapportLigneAsync(RapportEnt rapport, int? currentuser, int personnelId)
        {
            IEnumerable<RapportLigneEnt> poinatgesToDelete = rapport.ListLignes.Where(x => x.PersonnelId == personnelId);
            if (poinatgesToDelete != null && poinatgesToDelete.Any())
            {
                pointageManager.DeletePointageList(poinatgesToDelete, true, currentuser);
            }

            // Suppression de l'affectation astreinte si existe
            //Récupérer l'affectation
            AffectationEnt affectation = affectationManager.GetAffectationByCiAndPersonnel(rapport.CiId, personnelId);
            if (affectation != null)
            {
                //Récupérer l'astreinte                
                AstreinteEnt astreinte = await astreinteManager.GetOrNewAstreinteAsync(affectation, rapport.DateChantier);
                if (astreinte != null)
                {
                    astreinteManager.Delete(astreinte);
                }
            }
        }
        /// <summary>
        /// Permet de creer un nouveau rapport ligne
        /// </summary>
        /// <param name="rapportEnt">rapport Entite</param>
        /// <param name="absenceFiggomodel">Absence reçu par figgo</param>
        /// <param name="currentuser">utilisateur Fred Ie</param>
        private void CreateRapportLigne(RapportEnt rapportEnt, FiggoAbsenceModel absenceFiggomodel, int? currentuser)
        {
            RapportLigneEnt rapportLigne;
            rapportLigne = MapRapportLigneModel(absenceFiggomodel, currentuser, rapportEnt);
            pointageManager.InsertRapportLigneFiggo(rapportLigne);
        }
        /// <summary>
        /// Permet de verifier si la date de pointage n'est pas verrouillé
        /// </summary>
        /// <param name="rapportEnt">rapport entite</param>
        /// <param name="absenceFiggomodel">absence reçu par figgo</param>
        /// <param name="currentuser">utilisateur fred ie</param>
        /// <param name="dictionnairErreur">dictionnaire d erreur</param>
        /// <param name="personnelId">identifiant du personnel</param>
        /// <returns>dictionnaire</returns>
        private Dictionary<JsonErrorFiggo, bool> CheckIfDatePointageNotLocked(RapportEnt rapportEnt, FiggoAbsenceModel absenceFiggomodel, int? currentuser, Dictionary<JsonErrorFiggo, bool> dictionnairErreur, int personnelId, int typePersonnel)
        {
            Dictionary<JsonErrorFiggo, bool> dictionnaire = new Dictionary<JsonErrorFiggo, bool>();
            if (rapportEnt.ListLignes.Any(x => x.RapportLigneStatutId != Constantes.RapportVerrouilleId) && (absenceFiggomodel.Quantite == Constantes.AM || absenceFiggomodel.Quantite == Constantes.PM))
            {
                return CheckTotalHoursInPointageDay(rapportEnt, absenceFiggomodel, currentuser, dictionnairErreur, personnelId, typePersonnel);
            }
            dictionnaire.Add(FillJsonError(absenceFiggomodel, false, null), false);
            return dictionnaire;
        }
        /// <summary>
        /// Recupére les codes de majoration d'une societe
        /// </summary>
        /// <param name="absenceFiggomodel">absence reçu par figgo</param>
        /// <returns>liste de code majorations</returns>
        private IEnumerable<CodeMajorationEnt> GetListCodeMajorationBySociete(FiggoAbsenceModel absenceFiggomodel)
        {
            List<CodeMajorationEnt> listCodeMajorations = new List<CodeMajorationEnt>();
            if (absenceFiggomodel.SocieteCode != null)
            {
                return codeMajorationManager.GetCodeMajorationListByGroupeIdAndIsHeurNuit(GetSociete(absenceFiggomodel.SocieteCode).GroupeId);
            }
            return listCodeMajorations;
        }
        /// <summary>
        /// verifie le total des heures pour une journee
        /// </summary>
        /// <param name="rapport"> rapport ligne</param>
        /// <param name="absenceFiggomodel">absence reçu par figgo</param>
        /// <param name="currentuser">utilisateur fred ie</param>
        /// <param name="dictionnairErreur">dictionnaire</param>
        /// <param name="personnelId">identifiant du personnel</param>
        /// <returns>dictionnaire envoye</returns>
        private Dictionary<JsonErrorFiggo, bool> CheckTotalHoursInPointageDay(RapportEnt rapport, FiggoAbsenceModel absenceFiggomodel, int? currentuser, Dictionary<JsonErrorFiggo, bool> dictionnairErreur, int personnelId, int typePersonnel)
        {
            double total = 0;
            if (rapport.ListLignes.Any(r => r.TypeAbsence == absenceFiggomodel.Quantite && r.DateSuppression == null))
            {
                return UpdateOrCreateRapportLigne(rapport, absenceFiggomodel, dictionnairErreur, currentuser);
            }
            var code = GetListCodeMajorationBySociete(absenceFiggomodel).Select(m => m.CodeMajorationId);
            var listCi = affectationManager.GetPersonnelAffectationCiList(personnelId).Select(x => x.CiId);
            List<RapportEnt> listRapport = rapportManager.GetAllRapportsByPersonnelIdAndDatePointagesFiggo(listCi.ToList(), absenceFiggomodel.DateAbsence, personnelId, typePersonnel);
            listRapport.Add(rapport);
            foreach (var rapports in listRapport)
            {
                foreach (var item in rapports.ListLignes.Where(x => x.DateSuppression == null && x.PersonnelId.Equals(personnelId)))
                {
                    total += item.HeureAbsence + item.ListRapportLigneTaches.Sum(x => x.HeureTache) + item.ListRapportLigneMajorations.Where(m => m.CodeMajorationId.Equals(code.Any())).Sum(m => m.HeureMajoration);
                }
            }
            if (total <= GetHeureAbsence(absenceFiggomodel) && rapport.ListLignes.Any())
            {
                return UpdateOrCreateRapportLigne(rapport, absenceFiggomodel, dictionnairErreur, currentuser);
            }
            if (total > GetHeureAbsence(absenceFiggomodel) && rapport.ListLignes.Any())
            {
                foreach (var item in listRapport)
                {
                    IEnumerable<RapportLigneEnt> poitangesToDelete = item.ListLignes.Where(x => x.PersonnelId.Equals(personnelId));
                    if (poitangesToDelete != null && poitangesToDelete.Any())
                    {
                        pointageManager.DeletePointageList(poitangesToDelete, true, currentuser);
                    }
                }

                var defaultCi = affectationManager.GetDefaultCi(personnelId);
                //Mise à jour les heures sur tâche sur le CI par défaut du personnel 
                var rapportLigne = MapRapportLigneModel(absenceFiggomodel, currentuser, rapport);
                if (rapport == null)
                {
                    CreateRapportEnt(absenceFiggomodel, currentuser, rapportLigne, defaultCi.CiId, personnelId);
                }
                else
                {
                    pointageManager.AddPointageWithSave(rapportLigne);
                }

                string errorMessage = "Le pointage saisi dans FRED du personnel " + absenceFiggomodel.Matricule + " de la société " + absenceFiggomodel.SocieteCode + " pour la journée du " + absenceFiggomodel.DateAbsence.ToShortDateString() + " a été supprimé  ";
                NLog.LogManager.GetCurrentClassLogger().Info("[FIGGO] : " + errorMessage);
                dictionnairErreur.Add(FillJsonError(absenceFiggomodel, true, errorMessage), true);
                return dictionnairErreur;
            }
            dictionnairErreur.Add(FillJsonError(absenceFiggomodel, true, null), false);
            return dictionnairErreur;
        }
        private Dictionary<JsonErrorFiggo, bool> UpdateOrCreateRapportLigne(RapportEnt rapport, FiggoAbsenceModel absenceFiggomodel, Dictionary<JsonErrorFiggo, bool> dictionnairErreur, int? currentuser)
        {
            var pointageCiAbsence = rapport.ListLignes.FirstOrDefault(x => x.TypeAbsence == absenceFiggomodel.Quantite && x.DateSuppression == null);
            if (pointageCiAbsence != null)
            {
                pointageCiAbsence.DateModification = DateTime.Now;
                pointageCiAbsence.AuteurModificationId = currentuser.Value;
                pointageCiAbsence.CodeAbsenceId = GetCodeAbsencelId(absenceFiggomodel);
                pointageManager.UpdatePointageForFiggo(pointageCiAbsence);
                string message = " Mise a jour de l'absence effectué ";
                NLog.LogManager.GetCurrentClassLogger().Info("[FIGGO] : " + message);
                dictionnairErreur.Add(FillJsonError(absenceFiggomodel, true, message), true);
                return dictionnairErreur;
            }
            CreateRapportLigne(rapport, absenceFiggomodel, currentuser);
            NLog.LogManager.GetCurrentClassLogger().Info("[FIGGO] : " + FredImportExportBusinessResources.ImportMessageOk);
            dictionnairErreur.Add(FillJsonError(absenceFiggomodel, true, FredImportExportBusinessResources.ImportMessageOk), true);
            return dictionnairErreur;
        }
        /// <summary>
        /// RG-03
        /// </summary>
        /// <param name="absenceModel">model d'absence recu de Figgo</param>
        /// <param name="currentuser">Utilisateur FredIe</param>
        /// <param name="ciId">identifiant du ci</param>
        /// <returns>rapport</returns>
        private RapportEnt MapRapportModel(FiggoAbsenceModel absenceModel, int? currentuser, int? ciId = null)
        {
            var rapport = new RapportEnt();
            rapport.AuteurCreationId = currentuser;
            rapport.DateChantier = absenceModel.DateAbsence;
            rapport.DateCreation = absenceModel.DateCreation;
            rapport.DateModification = absenceModel.DateModification;
            rapport.DateSuppression = absenceModel.DateSuppression;
            if (ciId != null)
            {
                rapport.CiId = ciId.Value;
                return rapport;
            }
            rapport.CiId = GetCiEntBySociete(absenceModel).CiId;
            return rapport;
        }
        /// <summary>
        /// RG-03
        /// </summary>
        /// <param name="absenceModel">model d'absence recu de Figgo</param>
        /// <param name="currentuser">Utilisateur FredIe</param>
        /// <param name="rapport">pour la recuperation de l'id du rapport pere</param>
        /// <returns>rapportLigne</returns>
        private RapportLigneEnt MapRapportLigneModel(FiggoAbsenceModel absenceModel, int? currentuser, RapportEnt rapport = null)
        {
            var rapportLigne = new RapportLigneEnt();
            rapportLigne.PersonnelId = GetPersonnelId(absenceModel);
            rapportLigne.CodeAbsenceId = GetCodeAbsencelId(absenceModel);
            rapportLigne.AuteurCreationId = currentuser;
            rapportLigne.AuteurModificationId = currentuser;
            rapportLigne.HeureAbsence = GetHeureAbsence(absenceModel);
            rapportLigne.DatePointage = absenceModel.DateAbsence;
            rapportLigne.DateCreation = absenceModel.DateCreation;
            rapportLigne.DateModification = absenceModel.DateModification;
            rapportLigne.RapportLigneStatutId = Constantes.RapportValide2;
            rapportLigne.ValideurId = GetValideurId(absenceModel, currentuser);
            rapportLigne.DateValidation = absenceModel.DateValidation;
            rapportLigne.Commentaire = absenceModel.Commentaire;
            rapportLigne.TypeAbsence = absenceModel.Quantite;
            if (rapport != null)
            {
                rapportLigne.RapportId = rapport.RapportId;
                rapportLigne.CiId = rapport.CiId;
                return rapportLigne;
            }
            rapportLigne.CiId = GetCiEntBySociete(absenceModel).CiId;
            return rapportLigne;
        }
        /// <summary>
        /// verifie si un rapport existe
        /// </summary>
        /// <param name="rapport">rapport</param>
        /// <param name="personnel">personnel</param>
        /// <returns>false if rapport Existe</returns>
        private bool CheckIfRapportExiste(RapportEnt rapport, PersonnelEnt personnel)
        {
            if (rapport != null)
            {
                return CheckIfStatutPersonnelExistInRapport(rapport, personnel.Statut);
            }
            return true;
        }

        private bool CheckIfRapportLigneExiste(FiggoAbsenceModel absenceModel, List<RapportLigneEnt> listrapportLignes, int personnelId, List<RapportEnt> list)
        {
            if ((listrapportLignes.Any() && absenceModel.Quantite == Constantes.DAY) || listrapportLignes.Where(a => a.PersonnelId.Equals(personnelId)).Any(x => (x.TypeAbsence == absenceModel.Quantite || x.TypeAbsence == Constantes.DAY || x.RapportLigneStatutId == Constantes.RapportVerrouilleId) && !x.DateSuppression.HasValue) || list.Count > 0)
            {
                return false;// <returns>False if Existe</returns>
            }
            return true;
        }
        private PersonnelEnt GetPersonnel(int societeId, string matricule)
            => personnelManager.GetPersonnel(societeId, matricule);
        private int? GetSocieteId(string societeCode)
        {
            return societeManager.GetSocieteByCodeSocieteComptables(societeCode)?.SocieteId;
        }
        private SocieteEnt GetSociete(string societeCode)
        {
            return societeManager.GetSocieteByCodeSocieteComptables(societeCode);
        }
        private CIEnt GetCiEntBySociete(FiggoAbsenceModel absenceModel)
        {
            CIEnt ci = null;
            var societeId = GetSocieteId(absenceModel.SocieteCode);
            if (societeId.HasValue)
            {
                ci = ciManager.GetCIByCodeAndSocieteId("ABSENCES", societeId.Value);
            }

            return ci;
        }
        private int? GetPersonnelId(FiggoAbsenceModel absenceModel)
        {
            var societeId = GetSocieteId(absenceModel.SocieteCode).Value;
            var personnelId = GetPersonnel(societeId, absenceModel.Matricule);
            if (personnelId == null)
            {
                return 0;
            }
            return personnelId.PersonnelId;
        }
        private int GetCodeAbsencelId(FiggoAbsenceModel absenceModel)
            => codeAbsenceManager.GetCodeAbsenceByCode(absenceModel.AbsenceCode).CodeAbsenceId;
        private double GetHeureAbsence(FiggoAbsenceModel absenceModel)
        {
            var societe = GetSociete(absenceModel.SocieteCode);
            if (societe != null)
            {
                string personnelStatut = GetStatut(GetPersonnel(societe.SocieteId, absenceModel.Matricule).Statut);
                int idKey = paramsManager.GetParamKeyIdByLibelle(personnelStatut);
                float paramValue = GetParamValue(societe.Organisation.OrganisationId, idKey);
                if (absenceModel.Quantite == Constantes.AM)
                {
                    return paramValue / 2;
                }
                if (absenceModel.Quantite == Constantes.PM)
                {
                    return paramValue / 2;
                }
                if (absenceModel.Quantite == Constantes.DAY)
                {
                    return paramValue;
                }
            }
            return 0;
        }
        private int GetParamValue(int organisationId, int idKey)
        => Convert.ToInt32(paramsManager.GetParamsValueByOrganisationIdAndParamsKeyId(organisationId, idKey));
        private int GetValideurId(FiggoAbsenceModel absenceModel, int? currentuser)
        {
            var societeId = GetSocieteId(absenceModel.SocieteCodeValideur);
            if (societeId.HasValue)
            {
                var personnel = GetPersonnel(societeId.Value, absenceModel.MatriculeValideur);
                if (personnel != null && !personnel.PersonnelId.Equals(0))
                {
                    var utilisateur = utilisateurManager.GetUtilisateurById(personnel.PersonnelId);
                    if (utilisateur != null)
                    {
                        return utilisateur.UtilisateurId;
                    }
                }
            }
            return currentuser.Value;
        }
        private UtilisateurEnt GetFredIeUtilisateur()
        {
            return utilisateurManager.GetByLogin("fred_ie");
        }
        private bool CheckIfStatutPersonnelExistInRapport(RapportEnt rapport, string statut = null)
        {
            if (rapport != null)
            {
                if (statut == PersonnelStatutValue.Ouvrier || statut == TypePersonnel.Ouvrier)
                {
                    return !rapport.TypeStatutRapport.Equals(TypeRapportStatut.Ouvrier);
                }
                if (statut == PersonnelStatutValue.ETAM || statut == TypePersonnel.ETAM)
                {
                    return !rapport.TypeStatutRapport.Equals(TypeRapportStatut.ETAM);
                }
                if (statut == PersonnelStatutValue.Cadre || statut == TypePersonnel.Cadre)
                {
                    return !rapport.TypeStatutRapport.Equals(TypeRapportStatut.Cadre);
                }
            }
            return true;
        }
    }
}
