using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Fred.Business.AffectationSeuilUtilisateur;
using Fred.Business.EtatPaie.ControlePointage;
using Fred.Business.FeatureFlipping;
using Fred.Business.Params;
using Fred.Business.Personnel;
using Fred.Business.Rapport.Pointage;
using Fred.Business.RapportPrime;
using Fred.Business.Referential;
using Fred.Business.Societe;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Entities.Personnel;
using Fred.Entities.Rapport;
using Fred.Entities.RapportPrime;
using Fred.Entities.Referential;
using Fred.Entities.Utilisateur;
using Fred.Framework.Extensions;
using Fred.Framework.FeatureFlipping;
using Fred.Framework.Reporting;
using Fred.Web.Models.Rapport;
using Fred.Web.Shared.Extentions;
using Fred.Web.Shared.Models.EtatPaie;
using Fred.Web.Shared.Models.ValidationPointage;
using Syncfusion.Pdf;
using Syncfusion.XlsIO;

namespace Fred.Business.EtatPaie
{
    public class EtatPaieManager : Manager<RapportLigneEnt, IPointageRepository>, IEtatPaieManager
    {
        private readonly IPointageManager pointageManager;
        private readonly IPersonnelManager personnelManager;
        private readonly IRapportPrimeLigneManager rapportPrimeLigneManager;
        private readonly IAffectationSeuilUtilisateurManager affectationSeuilUtilisateurManager;
        private readonly IParamsManager paramsManager;
        private readonly IUtilisateurManager utilisateurManager;
        private readonly IFeatureFlippingManager featureFlippingManager;
        private readonly ICodeZoneDeplacementManager codeZoneDeplacementManager;
        private readonly IControlePointagesManager controlePointagesManager;
        private readonly IVerificationDesTempsManager verificationDesTempsManager;
        private readonly IListePrimesManager listePrimesManager;
        private readonly IListeIndemniteDeplacementManager listeIndemniteDeplacementManager;
        private readonly IListeHeuresSpecifiquesManager listeHeuresSpecifiquesManager;
        private readonly IControlePointagesFesManager controlePointagesFesManager;
        private readonly IControlePointagesHebdomadaireManager controlePointagesHebdomadaireManager;
        private readonly ISalarieAcompteManager salarieAcompteManager;
        private readonly IListeAbsencesMensuellesManager listeAbsencesMensuellesManager;
        private readonly ISocieteManager societeManager;

        public EtatPaieManager(
            IUnitOfWork uow,
            IPointageRepository pointageRepository,
            IPointageManager pointageManager,
            IPersonnelManager personnelManager,
            IRapportPrimeLigneManager rapportPrimeLigneManager,
            IAffectationSeuilUtilisateurManager affectationSeuilUtilisateurManager,
            IParamsManager paramsManager,
            IUtilisateurManager utilisateurManager,
            IFeatureFlippingManager featureFlippingManager,
            ICodeZoneDeplacementManager codeZoneDeplacementManager,
            IControlePointagesManager controlePointagesManager,
            IVerificationDesTempsManager verificationDesTempsManager,
            IListePrimesManager listePrimesManager,
            IListeIndemniteDeplacementManager listeIndemniteDeplacementManager,
            IListeHeuresSpecifiquesManager listeHeuresSpecifiquesManager,
            IControlePointagesFesManager controlePointagesFesManager,
            IControlePointagesHebdomadaireManager controlePointagesHebdomadaireManager,
            ISalarieAcompteManager salarieAcompteManager,
            IListeAbsencesMensuellesManager listeAbsencesMensuellesManager,
            ISocieteManager societeManager)
          : base(uow, pointageRepository)
        {
            this.pointageManager = pointageManager;
            this.personnelManager = personnelManager;
            this.rapportPrimeLigneManager = rapportPrimeLigneManager;
            this.affectationSeuilUtilisateurManager = affectationSeuilUtilisateurManager;
            this.paramsManager = paramsManager;
            this.utilisateurManager = utilisateurManager;
            this.featureFlippingManager = featureFlippingManager;
            this.codeZoneDeplacementManager = codeZoneDeplacementManager;
            this.controlePointagesManager = controlePointagesManager;
            this.verificationDesTempsManager = verificationDesTempsManager;
            this.listePrimesManager = listePrimesManager;
            this.listeIndemniteDeplacementManager = listeIndemniteDeplacementManager;
            this.listeHeuresSpecifiquesManager = listeHeuresSpecifiquesManager;
            this.controlePointagesFesManager = controlePointagesFesManager;
            this.controlePointagesHebdomadaireManager = controlePointagesHebdomadaireManager;
            this.salarieAcompteManager = salarieAcompteManager;
            this.listeAbsencesMensuellesManager = listeAbsencesMensuellesManager;
            this.societeManager = societeManager;
        }

        /// <summary>
        /// Génère un pdf pour le controle des pointages
        /// </summary>
        /// <param name="etatPaieExportModel">Model de l'etat de paie pour l'export</param>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        /// <returns>Le fichier généré</returns>
        public MemoryStream GenerateControlePointages(EtatPaieExportModel etatPaieExportModel, int userId, string templateFolderPath)
        {
            var listePointageMensuel = LoadPointagesMensuels(etatPaieExportModel);

            return controlePointagesManager.GenerateMemoryStreamControlePointages(etatPaieExportModel, listePointageMensuel, userId, templateFolderPath);
        }

        /// <summary>
        /// Génère un pdf pour le controle des pointages
        /// </summary>
        /// <param name="etatPaieExportModel">Model de l'etat de paie pour l'export</param>
        /// <param name="userId">Identifiant de l'utilisateur</param>
        /// <returns>Le fichier généré</returns>
        public MemoryStream GenerateControlePointagesFes(EtatPaieExportModel etatPaieExportModel, int userId, string templateFolderPath)
        {
            List<PointageMensuelPersonEnt> listePointageMensuel = LoadPointagesMensuelsfes(etatPaieExportModel);
            //Récupérer les personnles qui n'ont aucun pointages
            listePointageMensuel = GetPersonnelWithOutPointages(listePointageMensuel, etatPaieExportModel);
            if (etatPaieExportModel.Tri)
            {
                listePointageMensuel = listePointageMensuel.OrderBy(x => x.Personnel != null ? x.Personnel.Matricule : string.Empty).ToList();
            }
            else
            {
                listePointageMensuel = listePointageMensuel.OrderBy(x => x.Personnel != null ? x.Personnel.Nom : string.Empty).ToList();
            }

            return controlePointagesFesManager.GenerateMemoryStreamControlePointagesFES(etatPaieExportModel, listePointageMensuel, userId, templateFolderPath);
        }

        /// <summary>
        /// Récupérer tout les personnles qui n'ont pas de pointages
        /// </summary>
        /// <param name="listePointageMensuel">la liste des pointagesMensules</param>
        /// <param name="etatPaieExportModel">Model de l'etat de paie pour l'export</param>
        /// <returns>la liste des personnels</returns>
        public List<PointageMensuelPersonEnt> GetPersonnelWithOutPointages(List<PointageMensuelPersonEnt> listePointageMensuel, EtatPaieExportModel etatPaieExportModel)
        {
            IEnumerable<int> listSummaryMensuelPersoId = listePointageMensuel.Select(p => p.Personnel.PersonnelId);
            IEnumerable<PersonnelEnt> listPersonnelActif = null;

            var societeId = societeManager.GetSocieteByOrganisationId(etatPaieExportModel.OrganisationId).SocieteId;
            var societe = societeManager.GetSocieteById(societeId, true);
            // recherche par Société pour FES, par CI pour les autres
            if (societe.Groupe.Code == Constantes.CodeGroupeFES)
            {
                listPersonnelActif = GetPersonnelFilteredBySocieteId(societeId, etatPaieExportModel);
            }
            else
            {
                IEnumerable<int> allCisByOrga = Managers.Utilisateur.GetAllCIIdbyOrganisation(etatPaieExportModel.OrganisationId);
                listPersonnelActif = Managers.Affectation.GetPersonneListByCiIdList(allCisByOrga, etatPaieExportModel.EtablissementPaieIdList, etatPaieExportModel);
            }

            if (!listPersonnelActif.IsNullOrEmpty())
            {
                List<PersonnelEnt> personnelNotRegisterdList = listPersonnelActif.Where(affec => !listSummaryMensuelPersoId.Contains(affec.PersonnelId) && (!etatPaieExportModel.StatutPersonnelList.Any() || etatPaieExportModel.StatutPersonnelList.Contains(affec.Statut))).ToList();
                personnelNotRegisterdList.ForEach(personnel =>
                    listePointageMensuel.Add(new PointageMensuelPersonEnt
                    {
                        ListHeuresTravaillees = new PointageMensuelPersonEnt.HeuresTravaillees(),
                        ListHeuresNormales = new PointageMensuelPersonEnt.HeuresNormales(),
                        ListHeuresMajo = new PointageMensuelPersonEnt.HeuresMajo(),
                        ListHeuresAbsence = new PointageMensuelPersonEnt.HeuresAbsence(),
                        ListHeuresPointees = new PointageMensuelPersonEnt.HeuresPointees(),
                        Personnel = personnel
                    }));
            }

            return listePointageMensuel;
        }

        /// <summary>
        /// Génère un flux de mémoire pour la vérification des temps
        /// </summary>
        /// <param name="etatPaieExportModel">Model de l'etat de paie pour l'export</param>
        /// <param name="userId">Identifiant de l'éditeur de l'édition</param>
        /// <returns>Le flux de mémoire généré</returns>
        public MemoryStream GenerateVerificationTemps(EtatPaieExportModel etatPaieExportModel, int userId, string templateFolderPath)
        {
            var listePointageMensuel = LoadVerifTemps(etatPaieExportModel);

            return verificationDesTempsManager.GenerateMemoryStreamVerificationDesTemps(etatPaieExportModel, listePointageMensuel, userId, templateFolderPath);
        }

        /// <summary>
        /// Liste du personnel filtré par société et etablissements comptables
        /// </summary>
        /// <param name="societeId">Id de société</param>
        /// <param name="etatPaieExportModel">Model de l'etat de paie pour l'export</param>
        /// <returns>Liste de personnel</returns>
        private IEnumerable<PersonnelEnt> GetPersonnelFilteredBySocieteId(int societeId, EtatPaieExportModel etatPaieExportModel)
        {
            return personnelManager.Get(new List<Expression<Func<PersonnelEnt, bool>>>
                {
                x => x.SocieteId == societeId,
                x => !etatPaieExportModel.EtablissementPaieIdList.Any() || etatPaieExportModel.EtablissementPaieIdList.Contains(x.EtablissementPaieId),
                x => !x.DateSortie.HasValue || (x.DateSortie.Value.Month >= etatPaieExportModel.Mois && x.DateSortie.Value.Year >= etatPaieExportModel.Annee)
            }, null,
            new List<Expression<Func<PersonnelEnt, object>>>
            {
                x => x.Societe.Organisation,
                x => x.Ressource,
                x => x.Manager,
                x => x.EtablissementPaie,
                x => x.EtablissementRattachement
            }
            );
        }

        /// <summary>
        /// Génère un flux de mémoire pour la liste des primes
        /// </summary>
        /// <param name="etatPaieExportModel">Model de l'etat de paie pour l'export</param>
        /// <param name="userId">Utilisateur qui édite le document</param>
        /// <returns>Le flux de mémoire généré</returns>
        public MemoryStream GenerateDocumentListePrimes(EtatPaieExportModel etatPaieExportModel, int userId, string templateFolderPath)
        {
            List<EtatPaieListePrimesModel> listePointageMensuel;
            if (etatPaieExportModel.FiltresPrimesMensuelles)
            {
                // on LoadPrimeMensuelle (new method à base de RapportPrimeEnt pour construire un EtatPaieListePrimesModel)
                listePointageMensuel = LoadPrimeMensuelle(etatPaieExportModel.Annee, etatPaieExportModel.Mois, etatPaieExportModel.Filtre, etatPaieExportModel.OrganisationId, etatPaieExportModel.Tri);
            }
            else
            {
                //Sinon on est sur les primes journalières/horaires donc sur des RapportEnt/PointageBase classiques
                listePointageMensuel = LoadPrime(etatPaieExportModel);
            }

            return listePrimesManager.GenerateMemoryStreamListePrimes(etatPaieExportModel, listePointageMensuel, userId, templateFolderPath);
        }

        /// <summary>
        /// Génère un flux de mémoire pour la liste des IGD
        /// </summary>
        /// <param name="etatPaieExportModel">Model de l'etat de paie pour l'export</param>
        /// <param name="userId">Utilisateur qui édite le document</param>
        /// <returns>Le flux de mémoire généré</returns>
        public MemoryStream GenerateDocumentListeIGD(EtatPaieExportModel etatPaieExportModel, int userId, string templateFolderPath)
        {
            List<EtatPaieListeIgdModel> listePointageMensuel = LoadIGD(etatPaieExportModel);

            return listeIndemniteDeplacementManager.GenerateMemoryStreamListeIGD(etatPaieExportModel, listePointageMensuel, userId, templateFolderPath);
        }

        /// <inheritdoc />
        public MemoryStream GenerateDocumentListeHeuresSpecifiques(EtatPaieExportModel etatPaieExportModel, UtilisateurEnt user, string templateFolderPath)
        {
            List<EtatPaieListeCodeMajorationModel> listePointageMensuel = user.Personnel.Societe.Groupe.Code != "GFES" ? LoadCodeMajoration(etatPaieExportModel) : LoadCodeMajorationFes(etatPaieExportModel);

            return listeHeuresSpecifiquesManager.GenerateMemoryStreamListeHeuresSpecifiques(etatPaieExportModel, listePointageMensuel, user.UtilisateurId, templateFolderPath);
        }

        /// <summary>
        /// Génère un flux de mémoire pour la liste des salarie acompte
        /// </summary>
        /// <param name="etatPaieExportModel">Model de l'etat de paie pour l'export</param>
        /// <param name="user">Nom du l'utilisateur courant</param>
        /// <returns>Le flux de mémoire généré</returns>
        public MemoryStream GenerateSalarieAcompte(EtatPaieExportModel etatPaieExportModel, UtilisateurEnt user, string templateFolderPath)
        {
            List<PointageMensuelPersonEnt> mensuelPerson = LoadPointagesMensuelsfes(etatPaieExportModel);
            List<AffectationSeuilUtilisateurEnt> utilisateur = affectationSeuilUtilisateurManager.GetListByUtilisateurId(user.UtilisateurId);
            int? idorganisationPere = utilisateur.FirstOrDefault(i => i.Role.NiveauPaie >= 4)?.OrganisationId;
            List<SummaryMensuelPersoModel> finallist = LoadVerifTemps(etatPaieExportModel, idorganisationPere)?.ToList();

            return salarieAcompteManager.GenerateMemoryStreamSalarieAcompte(etatPaieExportModel, mensuelPerson, finallist, user, templateFolderPath);
        }

        /// <summary>
        /// Generate Excel List Absences Mensuels
        /// </summary>
        /// <param name="etatPaieExportModel">Model de l'etat de paie pour l'export</param>
        /// <param name="user">user</param>
        /// <returns>return MemoryStream</returns>
        public MemoryStream GenerateListeAbsencesMensuelles(EtatPaieExportModel etatPaieExportModel, UtilisateurEnt user, string templateFolderPath)
        {
            int?[] listOrganisationIds;
            var utilisateurRoles = this.affectationSeuilUtilisateurManager.GetListByUtilisateurId(user.UtilisateurId);
            if (user.Personnel.Societe.Groupe.Code != "GFES")
            {
                listOrganisationIds = utilisateurRoles.Where(i => i.Role.NiveauPaie >= 4)?.Select(u => u?.OrganisationId)?.Distinct().ToArray();
            }
            else
            {
                listOrganisationIds = utilisateurRoles.Select(u => u?.OrganisationId)?.Distinct().ToArray();
            }


            List<AbsenceLigne> listeAbsencesMensuels = LoadAbsencesMensuels(etatPaieExportModel, listOrganisationIds)?.ToList();

            return listeAbsencesMensuellesManager.GenerateMemoryStreamListeAbsencesMensuelles(etatPaieExportModel, listeAbsencesMensuels, user, templateFolderPath);
        }

        /// <summary>
        /// Genere un PDF ou Excel à partir d'un MemoryStream
        /// </summary>
        /// <param name="pdf">True si on doit générer un PDF</param>
        /// <param name="excelFormat">Formattage Excel</param>
        /// <param name="workbook">Objet Excel Workbook</param>
        /// <returns>MemoryStream</returns>
        private MemoryStream GeneratePdfOrExcel(bool pdf, ExcelFormat excelFormat, IWorkbook workbook)
        {
            MemoryStream stream = new MemoryStream();
            if (pdf)
            {
                PdfDocument pdfDoc = excelFormat.PrintExcelToPdfAutoFit(workbook);
                string cacheId = Guid.NewGuid().ToString();
                string tempFilename = Path.Combine(Path.GetTempPath(), cacheId + ".pdf");
                pdfDoc.Save(tempFilename);
                stream = excelFormat.ChargerFichier(tempFilename);
                File.Delete(tempFilename);
                pdfDoc.Close();
            }
            else
            {
                workbook.SaveAs(stream);
            }

            workbook.Close();

            return stream;
        }

        /// <summary>
        /// Retourne la liste des pointages mensuels
        /// </summary>
        /// <param name="etatPaieExportModel">Model de l'etat de paie pour l'export</param> 
        /// <returns>La liste des des pointages mensuels</returns>
        private List<PointageMensuelPersonEnt> LoadPointagesMensuelsfes(EtatPaieExportModel etatPaieExportModel)
        {
            var listePointageMensuelPerson = new List<PointageMensuelPersonEnt>();
            List<RapportLigneEnt> listRapportLigne = this.pointageManager
                                  .GetListePointageMensuelfes(etatPaieExportModel)
                                  .Where(p => p.Personnel != null)
                                  .ToList();

            var listGroupByPerso = listRapportLigne.GroupBy(p => p.PersonnelId).Select(g => new { PersonnelId = g.Key, ListPointages = g.ToArray() }).ToList();
            Dictionary<int, string> dico = InstanciateAndLoadDico();
            Dictionary<int, string> dicoBool = InstanciateAndLoadDico(true);

            // On traite les pointages de chaque Personnel
            foreach (var groupByPerso in listGroupByPerso)
            {
                var listAbsence = new ListAbsences();
                var listDeplacement = new ListDeplacements();
                var listPrimeJours = new ListPrimesJours();

                var listPointages = groupByPerso.ListPointages;
                var pointageMensuelPerson = new PointageMensuelPersonEnt();
                var premiereLigne = listPointages.First();
                string heursJour = "7";
                string personnelStatut = GetPersonnelStatut(premiereLigne.Personnel.Statut);
                bool isCadre = personnelStatut == Constantes.PersonnelStatutValue.Cadre;
                if (isCadre)
                {
                    heursJour = paramsManager.GetParamValue(premiereLigne.Personnel.Societe.Organisation.OrganisationId, "HeuresJourIAC") ?? (7d).ToString();
                }
                pointageMensuelPerson.Annee = etatPaieExportModel.Annee;
                pointageMensuelPerson.Mois = etatPaieExportModel.Mois;
                pointageMensuelPerson.Personnel = premiereLigne.Personnel;

                var listGroupByDate = groupByPerso.ListPointages.GroupBy(p => p.DatePointage).Select(g => new { Date = g.Key, ListPointages = g.ToArray() }).ToList();
                Dictionary<int, string> dicoIVD = new Dictionary<int, string>(dico);
                Dictionary<int, string> dicoHeuresNormales = new Dictionary<int, string>(dico);
                Dictionary<int, string> dicoHeureMajo = new Dictionary<int, string>(dico);
                Dictionary<int, string> dicoHeuresAPied = new Dictionary<int, string>(dico);
                Dictionary<int, string> dicoHeuresAbsence = new Dictionary<int, string>(dico);
                Dictionary<int, string> dicoHeuresPointees = new Dictionary<int, string>(dico);
                Dictionary<int, string> dicoHeureTravaillees = new Dictionary<int, string>(dico);
                Dictionary<int, string> dicoPrimeAstreintes = new Dictionary<int, string>(dico);
                Dictionary<int, string> dicoSortieAstreintes = new Dictionary<int, string>(dicoBool);
                Dictionary<string, double> totalprime = new Dictionary<string, double>();
                Dictionary<int, string> dicoCodeZoneDeplacement = null;

                // On traite les pointages d'une journée
                foreach (var groupByDate in listGroupByDate)
                {
                    int day = groupByDate.Date.Day;
                    double sumHeuresNormale = 0;
                    double sumHeuresMajoration = 0;
                    double sumHeuresAbsence = 0;
                    double sumMaterielMarche = 0;
                    double sumHeuresTravaillees = 0;
                    double sumHeuresAPieds = 0;
                    double sumHeuresPointees = 0;

                    foreach (var ligneRapport in groupByDate.ListPointages)
                    {

                        sumHeuresNormale += ligneRapport.ListRapportLigneTaches.Select(x => x.HeureTache).Sum();
                        sumHeuresMajoration += ligneRapport.ListRapportLigneMajorations.Select(x => x.HeureMajoration).Sum();
                        sumHeuresAbsence += ligneRapport.HeureAbsence;
                        sumMaterielMarche += ligneRapport.MaterielMarche;
                        // Gestion IVD
                        if (ligneRapport.DeplacementIV)
                        {
                            dicoIVD[day] = "X";
                        }

                        Parallel.Invoke(() => HandleAbsencePointageMensuelleFes(ligneRapport, new Dictionary<int, string>(dico), listAbsence, day, isCadre, heursJour),
                                        () => HandleDeplacementPointageMensuelleFes(ligneRapport, new Dictionary<int, string>(dico), listDeplacement, day),
                                        () => HandleCodeZoneDeplacementPointageMensuelleFes(ligneRapport, ref dicoCodeZoneDeplacement, day),
                                        () => HandlePrimePointageMensuelleFes(ligneRapport, totalprime, day, listPrimeJours, dico),
                                        () => HandleAstreintePointageMensuelleFes(ligneRapport, dicoSortieAstreintes, dicoPrimeAstreintes, day));
                        //list des tout les Astreintes
                        pointageMensuelPerson.ListRapportLigneAstreintes.AddRange(ligneRapport.ListRapportLigneAstreintes);

                    }

                    sumHeuresTravaillees = !isCadre ? sumHeuresNormale + sumHeuresMajoration : System.Math.Round((sumHeuresNormale + sumHeuresMajoration) / double.Parse(heursJour), 2);
                    sumHeuresAPieds = !isCadre ? sumHeuresTravaillees - sumMaterielMarche : System.Math.Round((sumHeuresTravaillees - sumMaterielMarche) / double.Parse(heursJour), 2);

                    var sumAbsence = !isCadre ? sumHeuresAbsence : System.Math.Round(sumHeuresAbsence / double.Parse(heursJour), 2);
                    sumHeuresPointees = sumHeuresTravaillees + sumAbsence;

                    dicoHeuresNormales[day] = !isCadre ? sumHeuresNormale.ToString() : System.Math.Round(sumHeuresNormale / double.Parse(heursJour), 2).ToString();
                    dicoHeureMajo[day] = !isCadre ? sumHeuresMajoration.ToString() : System.Math.Round(sumHeuresMajoration / double.Parse(heursJour), 2).ToString();
                    dicoHeuresAbsence[day] = !isCadre ? sumHeuresAbsence.ToString() : System.Math.Round(sumHeuresAbsence / double.Parse(heursJour), 2).ToString();
                    dicoHeuresAPied[day] = sumHeuresAPieds.ToString();
                    dicoHeuresPointees[day] = sumHeuresPointees.ToString();
                    dicoHeureTravaillees[day] = sumHeuresTravaillees.ToString();
                }

                // Chargement de la classe pointageMensuelPerson
                Parallel.Invoke(() => pointageMensuelPerson.LoadHeuresNormales(dicoHeuresNormales),
                    () => pointageMensuelPerson.LoadHeuresMajo(dicoHeureMajo),
                    () => pointageMensuelPerson.LoadHeuresAPied(dicoHeuresAPied),
                    () => pointageMensuelPerson.LoadHeuresAbsence(dicoHeuresAbsence),
                    () => pointageMensuelPerson.LoadHeuresPointees(dicoHeuresPointees),
                    () => pointageMensuelPerson.LoadHeuresTravaillees(dicoHeureTravaillees),
                    () => pointageMensuelPerson.LoadIVD(dicoIVD),
                    () => pointageMensuelPerson.LoadCodeZoneDeplacement(dicoCodeZoneDeplacement));

                if (dicoPrimeAstreintes.Any(i => i.Value != string.Empty))
                {
                    pointageMensuelPerson.LoadAstreinte(dicoPrimeAstreintes, "Prime d’astreinte");
                }
                if (dicoSortieAstreintes.Any(i => i.Value != "0"))
                {
                    pointageMensuelPerson.LoadAstreinte(dicoSortieAstreintes, "sortie d’astreinte");
                }

                foreach (Absence absence in listAbsence.ListAbsence)
                {
                    pointageMensuelPerson.LoadAbsence(absence.DicoAbsence, absence.CodeAbsence);
                }
                foreach (Deplacements deplacement in listDeplacement.ListDeplacement)
                {
                    pointageMensuelPerson.LoadDeplacement(deplacement.DicoDeplacement, deplacement.CodeDeplacement);
                }
                foreach (PrimeJours primeJour in listPrimeJours.ListPrimeJours)
                {
                    pointageMensuelPerson.LoadPrimes(primeJour.DicoPrimes, primeJour.CodePrime);
                }

                pointageMensuelPerson.Totalprime = totalprime;
                listePointageMensuelPerson.Add(pointageMensuelPerson);
            }

            return listePointageMensuelPerson;
        }


        /// <summary>
        /// Retourne la liste des primes horaires et journalières
        /// </summary>
        /// <param name="etatPaieExportModel">Model de l'etat de paie pour l'export</param>
        /// <returns>La liste des primes</returns>
        private List<EtatPaieListePrimesModel> LoadPrime(EtatPaieExportModel etatPaieExportModel)
        {
            const string astreinteMensuelleCodePrime = "ASTREINTES";
            const string astreinteMensuelleUnite = "Jours";
            List<EtatPaieListePrimesModel> listPrime = null;

            var listRapportLigne = this.pointageManager
                                 .GetListePointageMensuel(etatPaieExportModel)
                                 .Where(p => p.ListePrimes.Count > 0)
                                 .Where(p => p.Personnel != null)
                                 .ToList();


            UtilisateurEnt currentUser = utilisateurManager.GetContextUtilisateur();

            var list = new List<EtatPaieListePrimesModel>();
            foreach (PointageBase pointage in listRapportLigne)
            {
                // Liste des jours d'astreintes pour FES.
                var listJoursAstreintes = new List<int>();
                if (pointage.ListePrimes.Any(x => x.Prime.Code == Constantes.CodePrime.ASTRS)
                  || pointage.ListePrimes.Any(x => x.Prime.Code == Constantes.CodePrime.ASTRWE))
                {
                    listJoursAstreintes.Add(pointage.DatePointage.Day);
                }

                foreach (PointagePrimeBase prime in pointage.ListePrimes.Where(x => x.IsChecked))
                {
                    var o = new EtatPaieListePrimesModel
                    {
                        Etablissement = pointage.Personnel.EtablissementPaie != null ? pointage.Personnel.EtablissementPaie.Code + " - " + pointage.Personnel.EtablissementPaie.Libelle : string.Empty,
                        Personnel = pointage.Personnel.CodeNomPrenom,
                        Affaire = pointage.Ci != null ? pointage.Ci.CodeLibelle : string.Empty,
                        CodePrime = prime.Prime.CodeLibelle,
                        Nom = pointage.Personnel.Nom,
                        Matricule = pointage.Personnel.Matricule,
                        Unite = UniteByPrimeType(prime),
                        Quantite = QuantiteByPrimeType(prime),
                        ListJoursAstreintes = listJoursAstreintes
                    };

                    list.Add(o);
                }
            }

            // Ajout des primes mensuelles comportant des astreintes pas de traitement pour FES
            if (currentUser.Personnel.Societe.Groupe.Code != Constantes.CodeGroupeFES)
            {
                var primeMensuelles = rapportPrimeLigneManager
                                            .GetListeRapportPrimeLigneByMonth(
                                                    etatPaieExportModel.Annee,
                                                    etatPaieExportModel.Mois,
                                                    etatPaieExportModel.Filtre,
                                                    etatPaieExportModel.OrganisationId,
                                                    etatPaieExportModel.Tri,
                                                    null)
                                            .Where(x => x.ListAstreintes.Any())
                                            .ToList();

                // Ajout des primes mensuelles d'astreinte
                foreach (var primeMensuelle in primeMensuelles)
                {
                    var listAstreintes = primeMensuelle.ListAstreintes.Select(x => x.Astreinte.DateAstreinte.Day).ToList();
                    list.Add(new EtatPaieListePrimesModel
                    {
                        Etablissement = primeMensuelle.Personnel.EtablissementPaie != null ? primeMensuelle.Personnel.EtablissementPaie.Code + " - " + primeMensuelle.Personnel.EtablissementPaie.Libelle : string.Empty,
                        Personnel = primeMensuelle.Personnel.CodeNomPrenom,
                        Affaire = primeMensuelle.Ci != null ? primeMensuelle.Ci.CodeLibelle : string.Empty,
                        CodePrime = astreinteMensuelleCodePrime,
                        Nom = primeMensuelle.Personnel.Nom,
                        Matricule = primeMensuelle.Personnel.Matricule,
                        Unite = astreinteMensuelleUnite,
                        Quantite = 0,
                        ListJoursAstreintes = listAstreintes
                    });
                }
            }
            listPrime = list
              .GroupBy(d => new
              {
                  etablissement = d.Etablissement,
                  personnel = d.Personnel,
                  affaire = d.Affaire,
                  code = d.CodePrime,
                  unite = d.Unite,
                  nom = d.Nom,
                  matricule = d.Matricule
              })
              .OrderBy(g => etatPaieExportModel.Tri ? g.Key.matricule.Trim().ToUpper() : g.Key.nom.Trim().ToUpper())
              .ThenBy(g => g.Key.code)
              .Where(g => g.Sum(d => d.Quantite).IsNotZero() || g.Key.code == astreinteMensuelleCodePrime)
              .Select(g => new EtatPaieListePrimesModel
              {
                  Etablissement = g.Key.etablissement,
                  Personnel = g.Key.personnel,
                  Affaire = g.Key.affaire,
                  CodePrime = g.Key.code,
                  Quantite = g.Key.code != astreinteMensuelleCodePrime ? g.Sum(d => d.Quantite) : g.SelectMany(x => x.ListJoursAstreintes).Distinct().Count(),
                  Unite = g.Key.unite,
                  JoursAstreintes = string.Join(",", g.SelectMany(x => x.ListJoursAstreintes).Distinct().OrderBy(day => day).ToArray())
              }).ToList();

            return listPrime;
        }

        /// <summary>
        /// Retourne la liste des primes mensuelles (d'un RapportPrime/Mensuel et non d'un Rapport classique/Journalier)
        /// </summary>
        /// <param name="annee">Année de filtrage pour l'édition</param>
        /// <param name="mois">Mois de filtrage pour l'édition</param>
        /// <param name="filtre">filtre sur le périmètre à appliqué pour l'édition</param>
        /// <param name="organisationId">Identifiant de l'organisation </param>
        /// <param name="tri">indique si on applique un tri</param>
        /// <returns>La liste des primes mensuelles</returns>
        private List<EtatPaieListePrimesModel> LoadPrimeMensuelle(int annee, int mois, TypeFiltreEtatPaie filtre, int organisationId, bool tri)
        {
            List<EtatPaieListePrimesModel> listPrimeMensuelle = null;

            List<RapportPrimeLigneEnt> listRapportPrimeLigne = rapportPrimeLigneManager.GetListeRapportPrimeLigneByMonth(annee, mois, filtre, organisationId, tri, null)
                                                              .Where(rpl => rpl.ListPrimes.Count > 0)
                                                              .Where(rpl => rpl.Personnel != null)
                                                              .ToList();

            List<EtatPaieListePrimesModel> list = new List<EtatPaieListePrimesModel>();

            foreach (RapportPrimeLigneEnt rapportPrimeLigne in listRapportPrimeLigne)
            {
                foreach (RapportPrimeLignePrimeEnt prime in rapportPrimeLigne.ListPrimes)
                {
                    EtatPaieListePrimesModel ligneEtat = GetLigneEtat(rapportPrimeLigne, prime);

                    list.Add(ligneEtat);
                }
            }

            listPrimeMensuelle = list
                    .GroupBy(d => new
                    {
                        etablissement = d.Etablissement,
                        personnel = d.Personnel,
                        affaire = d.Affaire,
                        code = d.CodePrime,
                        unite = d.Unite,
                        nom = d.Nom,
                        matricule = d.Matricule,
                        montant = d.Montant
                    })
                    .OrderBy(g => tri ? g.Key.matricule.Trim().ToUpper() : g.Key.nom.Trim().ToUpper())
                    .ThenBy(g => g.Key.code)
                    .Where(g => g.Sum(d => d.Quantite).IsNotZero())
                    .Select(g => new EtatPaieListePrimesModel
                    {
                        Etablissement = g.Key.etablissement,
                        Personnel = g.Key.personnel,
                        Affaire = g.Key.affaire,
                        CodePrime = g.Key.code,
                        Quantite = g.Sum(d => d.Quantite),
                        Unite = g.Key.unite,
                        Montant = g.Sum(d => d.Quantite) * g.Key.montant
                    }).ToList();

            return listPrimeMensuelle;
        }

        /// <summary>
        /// Permet de récupérer un objet EtatPaieListePrimeModel à partir d'une ligne de rapport de prime et de chacune de ses primes
        /// </summary>
        /// <param name="rapportPrimeLigne">Ligne de rapport de prime</param>
        /// <param name="prime">Prime de la ligne de rapport de prime</param>
        /// <returns>Un EtatPaieListePrimeModel</returns>
        private static EtatPaieListePrimesModel GetLigneEtat(RapportPrimeLigneEnt rapportPrimeLigne, RapportPrimeLignePrimeEnt prime)
        {
            return new EtatPaieListePrimesModel
            {
                Etablissement = rapportPrimeLigne.Personnel.EtablissementPaie != null ? rapportPrimeLigne.Personnel.EtablissementPaie.Code + " - " + rapportPrimeLigne.Personnel.EtablissementPaie.Libelle : string.Empty,
                Personnel = rapportPrimeLigne.Personnel.CodeNomPrenom,
                Affaire = rapportPrimeLigne.Ci != null ? rapportPrimeLigne.Ci.CodeLibelle : string.Empty,
                CodePrime = prime.Prime.CodeLibelle,
                Nom = rapportPrimeLigne.Personnel.Nom,
                Matricule = rapportPrimeLigne.Personnel.Matricule,
                Unite = "Mois",
                Quantite = prime.Montant != null && prime.Montant.IsNotZero() ? 1 : 0,
                Montant = prime.Montant != null ? (double)prime.Montant : 0
            };
        }

        /// <summary>
        /// Permet de retourner l'unité selon le type de prime
        /// </summary>
        /// <param name="pointagePrime">Le pointage de prime</param>
        /// <returns>unité</returns>
        private string UniteByPrimeType(PointagePrimeBase pointagePrime)
        {
            string unite = string.Empty;
            switch (pointagePrime.Prime.PrimeType)
            {
                case ListePrimeType.PrimeTypeHoraire:
                    unite = "Heures";
                    break;
                case ListePrimeType.PrimeTypeJournaliere:
                    unite = "Jours";
                    break;
                default:
                    // ListePrimeType.PrimeTypeMensuelle
                    unite = "Mois";
                    break;
            }
            return unite;
        }

        /// <summary>
        /// Permet de retourner la quantité selon le type de prime
        /// </summary>
        /// <param name="pointagePrime">Le pointage de prime</param>
        /// <returns>quantité</returns>
        private double QuantiteByPrimeType(PointagePrimeBase pointagePrime)
        {
            double quantite = 0;
            switch (pointagePrime.Prime.PrimeType)
            {
                case ListePrimeType.PrimeTypeHoraire:
                    quantite = (pointagePrime.HeurePrime == null ? 0 : (double)pointagePrime.HeurePrime);
                    break;
                case ListePrimeType.PrimeTypeJournaliere:
                    quantite = 1;
                    break;
                default:
                    // ListePrimeType.PrimeTypeMensuelle
                    quantite = 1;
                    break;
            }
            return quantite;
        }

        /// <summary>
        /// Retourne la liste des IGD
        /// </summary>
        /// <param name="etatPaieExportModel">Model de l'etat de paie pour l'export</param>
        /// <returns>La liste des IGD</returns>
        private List<EtatPaieListeIgdModel> LoadIGD(EtatPaieExportModel etatPaieExportModel)
        {
            List<EtatPaieListeIgdModel> listIGD = null;

            var listRapportLigne = this.pointageManager
                                 .GetListePointageMensuel(etatPaieExportModel)
                                 .Where(p => p.CodeDeplacement != null && p.CodeDeplacement.IGD)
                                 .Where(p => p.Personnel != null)
                                 .ToList();

            var list = new List<EtatPaieListeIgdModel>();
            foreach (PointageBase pointage in listRapportLigne)
            {
                EtatPaieListeIgdModel o = new EtatPaieListeIgdModel
                {
                    Etablissement = pointage.Personnel.EtablissementPaie != null ? pointage.Personnel.EtablissementPaie.Code + " - " + pointage.Personnel.EtablissementPaie.Libelle : string.Empty,
                    Personnel = pointage.Personnel.CodeNomPrenom,
                    Affaire = pointage.Ci != null ? pointage.Ci.CodeLibelle : string.Empty,
                    CodeIGD = pointage.CodeDeplacement != null ? pointage.CodeDeplacement.CodeLibelle : string.Empty,
                    Quantite = 1,
                    Type = pointage.IsAnticipe ? "Prévisionnel" : "Réel",
                    Nom = pointage.Personnel.Nom,
                    Matricule = pointage.Personnel.Matricule
                };

                list.Add(o);
            }

            listIGD = list
              .GroupBy(d => new
              {
                  etablissement = d.Etablissement,
                  personnel = d.Personnel,
                  affaire = d.Affaire,
                  code = d.CodeIGD,
                  type = d.Type,
                  nom = d.Nom,
                  matricule = d.Matricule
              })
              .OrderBy(g => etatPaieExportModel.Tri ? g.Key.matricule.Trim().ToUpper() : g.Key.nom.Trim().ToUpper())
              .ThenBy(g => g.Key.code)
              .Where(g => g.Sum(d => d.Quantite).IsNotZero())
              .Select(g => new EtatPaieListeIgdModel
              {
                  Etablissement = g.Key.etablissement,
                  Personnel = g.Key.personnel,
                  Affaire = g.Key.affaire,
                  CodeIGD = g.Key.code,
                  Quantite = g.Sum(d => d.Quantite),
                  Type = g.Key.type
              }).ToList();

            return listIGD;
        }

        /// <summary>
        /// Retourne la liste des Code Majoration
        /// </summary>
        /// <param name="etatPaieExportModel">Model de l'etat de paie pour l'export</param>
        /// <returns>La liste des Code Majoration</returns>
        private List<EtatPaieListeCodeMajorationModel> LoadCodeMajoration(EtatPaieExportModel etatPaieExportModel)
        {
            List<EtatPaieListeCodeMajorationModel> listCodeMajoration = null;

            List<PointageBase> listRapportLigne = pointageManager.GetListePointageMensuel(etatPaieExportModel)
                                        .Where(p => p.CodeMajorationId != null)
                                        .Where(p => p.Personnel != null)
                                        .ToList();

            List<EtatPaieListeCodeMajorationModel> list = new List<EtatPaieListeCodeMajorationModel>();
            foreach (PointageBase pointage in listRapportLigne)
            {
                EtatPaieListeCodeMajorationModel ligneEtat = new EtatPaieListeCodeMajorationModel
                {
                    Etablissement = pointage.Personnel.EtablissementPaie != null ? pointage.Personnel.EtablissementPaie.Code + " - " + pointage.Personnel.EtablissementPaie.Libelle : string.Empty,
                    Personnel = pointage.Personnel.CodeNomPrenom,
                    Affaire = pointage.Ci != null ? pointage.Ci.CodeLibelle : string.Empty,
                    Nom = pointage.Personnel.Nom,
                    Matricule = pointage.Personnel.Matricule,
                    Quantite = pointage.HeureMajoration,
                    CodeMajoration = pointage.CodeMajoration != null ? pointage.CodeMajoration.Code + " - " + pointage.CodeMajoration.Libelle : string.Empty,
                    IsHeureNuit = pointage.CodeMajoration != null ? pointage.CodeMajoration.IsHeureNuit : false
                };

                list.Add(ligneEtat);
            }

            listCodeMajoration = list
                .GroupBy(x => new
                {
                    etablissement = x.Etablissement,
                    personnel = x.Personnel,
                    affaire = x.Affaire,
                    code = x.CodeMajoration,
                    nom = x.Nom,
                    matricule = x.Matricule,
                    isHeureNuit = x.IsHeureNuit
                })
                .OrderBy(x => etatPaieExportModel.Tri ? x.Key.matricule.Trim().ToUpper() : x.Key.nom.Trim().ToUpper())
                .ThenBy(x => x.Key.code)
                .Where(x => x.Sum(d => d.Quantite).IsNotZero())
                .Select(x => new EtatPaieListeCodeMajorationModel
                {
                    Etablissement = x.Key.etablissement,
                    Personnel = x.Key.personnel,
                    Affaire = x.Key.affaire,
                    CodeMajoration = x.Key.code,
                    Quantite = x.Sum(d => d.Quantite),
                    IsHeureNuit = x.Key.isHeureNuit
                }).ToList();

            return listCodeMajoration;
        }

        /// <summary>
        /// Retourne la liste des Code Majoration
        /// </summary>
        /// <param name="etatPaieExportModel">Model de l'etat de paie pour l'export</param>
        /// <returns>La liste des Code Majoration</returns>
        private List<EtatPaieListeCodeMajorationModel> LoadCodeMajorationFes(EtatPaieExportModel etatPaieExportModel)
        {
            List<EtatPaieListeCodeMajorationModel> listCodeMajoration = null;

            List<RapportLigneEnt> listRapportLigne = pointageManager.GetListePointageMensuelfes(etatPaieExportModel)
                                        .Where(p => p.ListRapportLigneMajorations != null && p.ListRapportLigneMajorations.Count > 0)
                                        .Where(p => p.Personnel != null)
                                        .ToList();

            List<EtatPaieListeCodeMajorationModel> list = new List<EtatPaieListeCodeMajorationModel>();
            foreach (RapportLigneEnt pointage in listRapportLigne)
            {
                foreach (var rapportLigneMajorations in pointage.ListRapportLigneMajorations)
                {
                    EtatPaieListeCodeMajorationModel ligneEtat = new EtatPaieListeCodeMajorationModel
                    {
                        Etablissement = pointage.Personnel.EtablissementPaie != null ? pointage.Personnel.EtablissementPaie.Code + " - " + pointage.Personnel.EtablissementPaie.Libelle : string.Empty,
                        Personnel = pointage.Personnel.CodeNomPrenom,
                        Affaire = pointage.Ci != null ? pointage.Ci.CodeLibelle : string.Empty,
                        Nom = pointage.Personnel.Nom,
                        Matricule = pointage.Personnel.Matricule,
                        Quantite = rapportLigneMajorations.HeureMajoration,
                        CodeMajoration = rapportLigneMajorations.CodeMajoration != null ? rapportLigneMajorations.CodeMajoration.Code + " - " + rapportLigneMajorations.CodeMajoration.Libelle : string.Empty,
                        IsHeureNuit = rapportLigneMajorations.CodeMajoration != null ? rapportLigneMajorations.CodeMajoration.IsHeureNuit : false
                    };

                    list.Add(ligneEtat);
                }
            }

            listCodeMajoration = list
                .GroupBy(x => new
                {
                    etablissement = x.Etablissement,
                    personnel = x.Personnel,
                    affaire = x.Affaire,
                    code = x.CodeMajoration,
                    nom = x.Nom,
                    matricule = x.Matricule,
                    isHeureNuit = x.IsHeureNuit
                })
                .OrderBy(x => etatPaieExportModel.Tri ? x.Key.matricule.Trim().ToUpper() : x.Key.nom.Trim().ToUpper())
                .ThenBy(x => x.Key.code)
                .Where(x => x.Sum(d => d.Quantite).IsNotZero())
                .Select(x => new EtatPaieListeCodeMajorationModel
                {
                    Etablissement = x.Key.etablissement,
                    Personnel = x.Key.personnel,
                    Affaire = x.Key.affaire,
                    CodeMajoration = x.Key.code,
                    Quantite = x.Sum(d => d.Quantite),
                    IsHeureNuit = x.Key.isHeureNuit
                }).ToList();

            return listCodeMajoration;
        }

        /// <summary>
        /// Retourne la liste des sommes d'heures travailées par jour
        /// </summary>
        /// <param name="etatPaieExportModel">Model de l'etat de paie pour l'export</param>
        /// <param name="organisationPere">Id de l'organisation </param>
        /// <returns>La liste des sommes d'heures travailées par jour</returns>
        private List<SummaryMensuelPersoModel> LoadVerifTemps(EtatPaieExportModel etatPaieExportModel, int? organisationPere = null)
        {
            UtilisateurEnt currentUser = Managers.Utilisateur.GetContextUtilisateur();

            var listeSymmaryMensuelPerso = new List<SummaryMensuelPersoModel>();
            if (currentUser.Personnel.Societe.Groupe.Code == Constantes.CodeGroupeFES)
            {
                var listRapportLigneFes = this.pointageManager
                                    .GetListePointageMensuelfes(etatPaieExportModel, organisationPere)
                                    .Where(p => p.Personnel != null)
                                    .ToList();

                var listGroupByPerso = listRapportLigneFes.GroupBy(p => p.PersonnelId).Select(g => new { PersonnelId = g.Key, ListPointages = g.ToArray() }).ToList();
                foreach (var groupByPerso in listGroupByPerso)
                {
                    double sumHeuresTravaillees = 0;
                    var summaryMensuelPerson = new SummaryMensuelPersoModel();
                    summaryMensuelPerson.DayDateTimeDictionnary = new Dictionary<int, DateTime>();
                    var listPointages = groupByPerso.ListPointages;
                    var premiereLigne = listPointages.First();
                    summaryMensuelPerson.Libelle = premiereLigne.Personnel.NomPrenom;
                    summaryMensuelPerson.Matricule = premiereLigne.Personnel.Matricule;
                    summaryMensuelPerson.PersonnelId = premiereLigne.Personnel.PersonnelId;
                    summaryMensuelPerson.Personnel = premiereLigne.Personnel;

                    // RG_5172_001-2
                    if (this.featureFlippingManager.IsActivated(EnumFeatureFlipping.EditionsPaieAmeliorations))
                    {
                        summaryMensuelPerson.PersonnelCodeSociete = premiereLigne.Personnel.Societe?.Code ?? string.Empty;
                    }

                    var listGroupByDate = groupByPerso.ListPointages.GroupBy(p => p.DatePointage.Date).Select(g => new { Date = g.Key, ListPointages = g.ToArray() }).ToList();

                    foreach (var groupByDate in listGroupByDate)
                    {
                        double heuresTravaillees = 0;
                        int day = groupByDate.Date.Day;
                        foreach (var pointage in groupByDate.ListPointages)
                        {
                            // pour FES, les champs pointage.HeureMajoration et HeuresTotalAstreintes sont toujours à 0,
                            // Il faut effectuer les calculs à partir de ListRapportLigneAstreintes et ListRapportLigneMajorations
                            double totalHeuresAstreinte = 0;
                            foreach (var astreinte in pointage.ListRapportLigneAstreintes)
                            {
                                totalHeuresAstreinte += (astreinte.DateFinAstreinte - astreinte.DateDebutAstreinte).TotalHours;
                            }

                            double totalHeuresMajoration = pointage.ListRapportLigneMajorations.Sum(x => x.HeureMajoration);

                            if (organisationPere != null)
                            {
                                heuresTravaillees += pointage.HeureNormale + totalHeuresMajoration + totalHeuresAstreinte;
                            }
                            else
                            {
                                heuresTravaillees += pointage.HeureNormale + totalHeuresMajoration + pointage.HeureAbsence + totalHeuresAstreinte;
                            }
                        }
                        sumHeuresTravaillees += heuresTravaillees;
                        summaryMensuelPerson.GetType().GetProperty("Jour" + day).SetValue(summaryMensuelPerson, heuresTravaillees.ToString());
                        summaryMensuelPerson.DayDateTimeDictionnary.Add(day, groupByDate.Date);
                    }
                    summaryMensuelPerson.TotalHeuresTravaillees = sumHeuresTravaillees.ToString();

                    listeSymmaryMensuelPerso.Add(summaryMensuelPerson);
                }

                if (this.featureFlippingManager.IsActivated(EnumFeatureFlipping.EditionsPaieAmeliorations))
                {
                    // RG_5172_001
                    var order = listeSymmaryMensuelPerso.OrderBy(x => x.PersonnelCodeSociete);
                    order = etatPaieExportModel.Tri ? order.ThenBy(x => x.Matricule) : order.ThenBy(x => x.Libelle);
                    return order.ToList();
                }
                else
                {
                    if (etatPaieExportModel.Tri)
                    {
                        return listeSymmaryMensuelPerso.OrderBy(x => x.Matricule).ToList();
                    }
                    else
                    {
                        return listeSymmaryMensuelPerso.OrderBy(x => x.Libelle).ToList();
                    }
                }
            }
            else
            {
                // YDY pour Population revoir la récupération des Id perso
                var listRapportLigne = this.pointageManager
                                    .GetListePointageMensuel(etatPaieExportModel)
                                    .Where(p => p.Personnel != null)
                                    .ToList();

                var listGroupByPerso = listRapportLigne.GroupBy(p => p.PersonnelId).Select(g => new { PersonnelId = g.Key, ListPointages = g.ToArray() }).ToList();
                foreach (var groupByPerso in listGroupByPerso)
                {
                    double sumHeuresTravaillees = 0;
                    var summaryMensuelPerson = new SummaryMensuelPersoModel();
                    summaryMensuelPerson.DayDateTimeDictionnary = new Dictionary<int, DateTime>();
                    var listPointages = groupByPerso.ListPointages;
                    var premiereLigne = listPointages.First();
                    summaryMensuelPerson.Libelle = premiereLigne.Personnel.NomPrenom;
                    summaryMensuelPerson.Matricule = premiereLigne.Personnel.Matricule;
                    summaryMensuelPerson.PersonnelId = premiereLigne.Personnel.PersonnelId;
                    summaryMensuelPerson.Personnel = premiereLigne.Personnel;

                    // RG_5172_001-2
                    if (this.featureFlippingManager.IsActivated(EnumFeatureFlipping.EditionsPaieAmeliorations))
                    {
                        summaryMensuelPerson.PersonnelCodeSociete = premiereLigne.Personnel.Societe?.Code ?? string.Empty;
                    }

                    var listGroupByDate = groupByPerso.ListPointages.GroupBy(p => p.DatePointage.Date).Select(g => new { Date = g.Key, ListPointages = g.ToArray() }).ToList();

                    foreach (var groupByDate in listGroupByDate)
                    {
                        double heuresTravaillees = 0;
                        int day = groupByDate.Date.Day;
                        foreach (var pointage in groupByDate.ListPointages)
                        {
                            if (organisationPere != null)
                            {
                                heuresTravaillees += pointage.HeureNormale + pointage.HeureMajoration;
                            }
                            else
                            {
                                heuresTravaillees += pointage.HeureNormale + pointage.HeureMajoration + pointage.HeureAbsence;
                            }
                        }
                        sumHeuresTravaillees += heuresTravaillees;
                        summaryMensuelPerson.GetType().GetProperty("Jour" + day).SetValue(summaryMensuelPerson, heuresTravaillees.ToString());
                        summaryMensuelPerson.DayDateTimeDictionnary.Add(day, groupByDate.Date);
                    }
                    summaryMensuelPerson.TotalHeuresTravaillees = sumHeuresTravaillees.ToString();

                    listeSymmaryMensuelPerso.Add(summaryMensuelPerson);
                }

                if (this.featureFlippingManager.IsActivated(EnumFeatureFlipping.EditionsPaieAmeliorations))
                {
                    // RG_5172_001
                    var order = listeSymmaryMensuelPerso.OrderBy(x => x.PersonnelCodeSociete);
                    order = etatPaieExportModel.Tri ? order.ThenBy(x => x.Matricule) : order.ThenBy(x => x.Libelle);
                    return order.ToList();
                }
                else
                {
                    if (etatPaieExportModel.Tri)
                    {
                        return listeSymmaryMensuelPerso.OrderBy(x => x.Matricule).ToList();
                    }
                    else
                    {
                        return listeSymmaryMensuelPerso.OrderBy(x => x.Libelle).ToList();
                    }
                }
            }
        }

        /// <summary>
        /// Retourne la liste des pointages mensuels
        /// </summary>
        /// <param name="etatPaieExportModel">Model de l'etat de paie pour l'export</param>
        /// <returns>La liste des des pointages mensuels</returns>
        private List<PointageMensuelPersonEnt> LoadPointagesMensuels(EtatPaieExportModel etatPaieExportModel)
        {
            var listePointageMensuelPerson = new List<PointageMensuelPersonEnt>();
            var listRapportLigne = this.pointageManager
                                  .GetListePointageMensuel(etatPaieExportModel)
                                  .Where(p => p.Personnel != null)
                                  .ToList();

            var listGroupByPerso = listRapportLigne.GroupBy(p => p.PersonnelId).Select(g => new { PersonnelId = g.Key, ListPointages = g.ToArray() }).ToList();

            // On traite les pointages de chaque Personnel
            foreach (var groupByPerso in listGroupByPerso)
            {
                var listAbsence = new ListAbsences();
                var listDeplacement = new ListDeplacements();
                var listPrimeJours = new ListPrimesJours();
                var dicoIVD = new Dictionary<int, string>();
                var listPointages = groupByPerso.ListPointages;
                var pointageMensuelPerson = new PointageMensuelPersonEnt();
                var premiereLigne = listPointages.First();

                pointageMensuelPerson.Annee = etatPaieExportModel.Annee;
                pointageMensuelPerson.Mois = etatPaieExportModel.Mois;
                pointageMensuelPerson.Personnel = premiereLigne.Personnel;

                var listGroupByDate = groupByPerso.ListPointages.GroupBy(p => p.DatePointage.Date).Select(g => new { Date = g.Key, ListPointages = g.ToArray() }).ToList();
                Dictionary<int, string> dicoHeuresNormales = new Dictionary<int, string>();
                Dictionary<int, string> dicoHeureMajo = new Dictionary<int, string>();
                Dictionary<int, string> dicoHeuresAPied = new Dictionary<int, string>();
                Dictionary<int, string> dicoHeuresAbsence = new Dictionary<int, string>();
                Dictionary<int, string> dicoHeuresPointees = new Dictionary<int, string>();
                Dictionary<int, string> dicoHeureTravaillees = new Dictionary<int, string>();
                Dictionary<int, string> dicoCodeZoneDeplacement = null;

                LoadDico(dicoHeuresNormales);
                LoadDico(dicoHeureMajo);
                LoadDico(dicoHeuresAPied);
                LoadDico(dicoHeuresAbsence);
                LoadDico(dicoHeuresPointees);
                LoadDico(dicoHeureTravaillees);
                LoadDico(dicoIVD);

                // On traite les pointages d'une journée
                foreach (var groupByDate in listGroupByDate)
                {
                    int day = groupByDate.Date.Day;
                    double sumHeuresNormale = 0;
                    double sumHeuresMajoration = 0;
                    double sumHeuresAbsence = 0;
                    double sumMaterielMarche = 0;
                    double sumHeuresTravaillees = 0;
                    double sumHeuresAPieds = 0;
                    double sumHeuresPointees = 0;

                    foreach (var ligneRapport in groupByDate.ListPointages)
                    {
                        sumHeuresNormale += ligneRapport.HeureNormale;
                        sumHeuresMajoration += ligneRapport.HeureMajoration;
                        sumHeuresAbsence += ligneRapport.HeureAbsence;
                        sumMaterielMarche += ligneRapport.MaterielMarche;
                        // Gestion IVD
                        if (ligneRapport.DeplacementIV)
                        {
                            dicoIVD[day] = "X";
                        }

                        // Gestion des absences
                        if (ligneRapport.CodeAbsenceId.HasValue)
                        {
                            if (!listAbsence.Contains(ligneRapport.CodeAbsence.Code))
                            {
                                var absences = new Absence();
                                LoadDico(absences.DicoAbsence);
                                absences.CodeAbsence = ligneRapport.CodeAbsence.Libelle;
                                absences.DicoAbsence[day] = ligneRapport.HeureAbsence.ToString();
                                listAbsence.Add(absences);
                            }
                            else
                            {
                                var abs = listAbsence.Get(ligneRapport.CodeAbsence.Code);
                                if (abs != null)
                                {
                                    abs.DicoAbsence[day] = ligneRapport.HeureAbsence.ToString();
                                }
                            }
                        }

                        // Gestion des déplacements
                        if (ligneRapport.CodeDeplacementId.HasValue)
                        {
                            if (!listDeplacement.Contains(ligneRapport.CodeDeplacement.Code))
                            {
                                var deplacements = new Deplacements();
                                LoadDico(deplacements.DicoDeplacement);
                                deplacements.CodeDeplacement = ligneRapport.CodeDeplacement.Code;
                                deplacements.DicoDeplacement[day] = "X";
                                listDeplacement.Add(deplacements);
                            }
                            else
                            {
                                var dep = listDeplacement.Get(ligneRapport.CodeDeplacement.Code);
                                if (dep != null)
                                {
                                    dep.DicoDeplacement[day] = "X";
                                }
                            }
                        }

                        // Gestion des déplacements
                        if (ligneRapport.CodeZoneDeplacementId.HasValue)
                        {
                            if (dicoCodeZoneDeplacement == null)
                            {
                                dicoCodeZoneDeplacement = new Dictionary<int, string>();
                                LoadDico(dicoCodeZoneDeplacement);
                            }

                            if (dicoCodeZoneDeplacement[day] != string.Empty)
                            {
                                string codeZone1 = dicoCodeZoneDeplacement[day].Replace('*', ' ');
                                string codeZone2 = ligneRapport.CodeZoneDeplacement.Code;
                                string codeZoneBiggest = codeZoneDeplacementManager.GetCompareBiggestCodeZondeDeplacement(codeZone1, codeZone2, ligneRapport.Ci.SocieteId.Value);
                                dicoCodeZoneDeplacement[day] = codeZoneBiggest + "*";
                            }
                            else
                            {
                                dicoCodeZoneDeplacement[day] = ligneRapport.CodeZoneDeplacement.Code;
                            }
                        }

                        // Gestion des primes
                        foreach (var prime in ligneRapport.ListePrimes.Where(p => p.IsChecked || p.HeurePrime > 0))
                        {
                            if (!listPrimeJours.Contains(prime.Prime.Code))
                            {
                                var primeJours = new PrimeJours();
                                LoadDico(primeJours.DicoPrimes);
                                primeJours.CodePrime = prime.Prime.Code;
                                if (prime.Prime.PrimeType == ListePrimeType.PrimeTypeHoraire && prime.HeurePrime > 0)
                                {
                                    primeJours.DicoPrimes[day] = prime.HeurePrime.ToString();
                                }
                                else if (prime.Prime.PrimeType == ListePrimeType.PrimeTypeJournaliere && prime.IsChecked)
                                {
                                    primeJours.DicoPrimes[day] = "X";
                                }

                                listPrimeJours.Add(primeJours);
                            }
                            else
                            {
                                var prim = listPrimeJours.Get(prime.Prime.Code);
                                if (prim != null)
                                {
                                    if (prime.Prime.PrimeType == ListePrimeType.PrimeTypeHoraire && prime.HeurePrime > 0)
                                    {
                                        prim.DicoPrimes[day] = prime.HeurePrime.ToString();
                                    }
                                    else if (prime.Prime.PrimeType == ListePrimeType.PrimeTypeJournaliere && prime.IsChecked)
                                    {
                                        prim.DicoPrimes[day] = "X";
                                    }
                                }
                            }
                        }
                    }
                    sumHeuresTravaillees = sumHeuresNormale + sumHeuresMajoration;
                    sumHeuresAPieds = sumHeuresTravaillees - sumMaterielMarche;
                    sumHeuresPointees = sumHeuresTravaillees + sumHeuresAbsence;

                    dicoHeuresNormales[day] = sumHeuresNormale.ToString();
                    dicoHeureMajo[day] = sumHeuresMajoration.ToString();
                    dicoHeuresAPied[day] = sumHeuresAPieds.ToString();
                    dicoHeuresAbsence[day] = sumHeuresAbsence.ToString();
                    dicoHeuresPointees[day] = sumHeuresPointees.ToString();
                    dicoHeureTravaillees[day] = sumHeuresTravaillees.ToString();
                }

                // Chargement de la classe pointageMensuelPerson
                pointageMensuelPerson.LoadHeuresNormales(dicoHeuresNormales);
                pointageMensuelPerson.LoadHeuresMajo(dicoHeureMajo);
                pointageMensuelPerson.LoadHeuresAPied(dicoHeuresAPied);
                pointageMensuelPerson.LoadHeuresAbsence(dicoHeuresAbsence);
                pointageMensuelPerson.LoadHeuresPointees(dicoHeuresPointees);
                pointageMensuelPerson.LoadHeuresTravaillees(dicoHeureTravaillees);
                pointageMensuelPerson.LoadIVD(dicoIVD);
                pointageMensuelPerson.LoadCodeZoneDeplacement(dicoCodeZoneDeplacement);

                foreach (var absence in listAbsence.ListAbsence)
                {
                    pointageMensuelPerson.LoadAbsence(absence.DicoAbsence, absence.CodeAbsence);
                }
                foreach (var deplacement in listDeplacement.ListDeplacement)
                {
                    pointageMensuelPerson.LoadDeplacement(deplacement.DicoDeplacement, deplacement.CodeDeplacement);
                }
                foreach (var primeJour in listPrimeJours.ListPrimeJours)
                {
                    pointageMensuelPerson.LoadPrimes(primeJour.DicoPrimes, primeJour.CodePrime);
                }

                listePointageMensuelPerson.Add(pointageMensuelPerson);
            }

            return listePointageMensuelPerson;
        }

        /// <summary>
        /// Retourne la liste des pointages mensuels
        /// </summary>
        /// <param name="date">date de filtrage pour l'édition</param>
        /// <param name="etatPaieExportModel">Model de l'etat de paie pour l'export</param>
        /// <returns>La liste des des pointages mensuels</returns>
        private List<PointageMensuelPersonEnt> LoadPointagesHebdomadaire(DateTime date, EtatPaieExportModel etatPaieExportModel)
        {
            var listePointageHebdomadairePerson = new List<PointageMensuelPersonEnt>();

            var listRapportLigne = this.pointageManager
                                  .GetListePointageHebdomadaire(date.Year, date.Month, date.Day, etatPaieExportModel)
                                  .Where(p => p.Personnel != null)
                                  .ToList();

            Dictionary<int, int> joursNumber = new Dictionary<int, int>();
            for (int i = 0; i < 7; i++)
            {
                joursNumber.Add(date.AddDays(i).Day, i + 1);
            }

            var listGroupByPerso = listRapportLigne.GroupBy(p => p.PersonnelId).Select(g => new { PersonnelId = g.Key, ListPointages = g.ToArray() }).ToList();

            // On traite les pointages de chaque Personnel
            foreach (var groupByPerso in listGroupByPerso)
            {
                var listAbsence = new ListAbsences();
                var listDeplacement = new ListDeplacements();
                var listPrimeJours = new ListPrimesJours();
                var dicoIVD = new Dictionary<int, string>();
                var listPointages = groupByPerso.ListPointages;
                var pointageHebdomadairePerson = new PointageMensuelPersonEnt();
                var premiereLigne = listPointages.First();
                bool isCadre = GetPersonnelStatut(premiereLigne.Personnel.Statut) == Constantes.PersonnelStatutValue.Cadre;

                double heuresJour = 7;//default 7
                if (isCadre)
                {
                    string defaultHeuresJour = paramsManager.GetParamValue(premiereLigne.Personnel.Societe.Organisation.OrganisationId, "HeuresJourIAC");
                    heuresJour = !defaultHeuresJour.IsNullOrEmpty() ? double.Parse(defaultHeuresJour) : heuresJour;
                }

                pointageHebdomadairePerson.Annee = date.Year;
                pointageHebdomadairePerson.Mois = date.Month;
                pointageHebdomadairePerson.Personnel = premiereLigne.Personnel;

                var listGroupByDate = groupByPerso.ListPointages.GroupBy(p => p.DatePointage).Select(g => new { Date = g.Key, ListPointages = g.ToArray() }).ToList();
                Dictionary<int, string> dicoHeuresNormales = new Dictionary<int, string>();
                Dictionary<int, string> dicoHeureMajo = new Dictionary<int, string>();
                Dictionary<int, string> dicoHeuresAPied = new Dictionary<int, string>();
                Dictionary<int, string> dicoHeuresAbsence = new Dictionary<int, string>();
                Dictionary<int, string> dicoHeuresPointees = new Dictionary<int, string>();
                Dictionary<int, string> dicoHeureTravaillees = new Dictionary<int, string>();
                Dictionary<int, string> dicoPrimeAstreintes = new Dictionary<int, string>();
                Dictionary<int, string> dicoSortieAstreintes = new Dictionary<int, string>();
                Dictionary<string, double> totalprime = new Dictionary<string, double>();


                LoadDico(dicoHeuresNormales, true);
                LoadDico(dicoHeureMajo, true);
                LoadDico(dicoHeuresAPied, true);
                LoadDico(dicoHeuresAbsence, true);
                LoadDico(dicoHeuresPointees, true);
                LoadDico(dicoHeureTravaillees, true);
                LoadDico(dicoPrimeAstreintes);
                LoadDico(dicoSortieAstreintes, true);
                LoadDico(dicoIVD);

                // On traite les pointages d'une journée
                foreach (var groupByDate in listGroupByDate)
                {
                    int day = joursNumber[groupByDate.Date.Day];
                    double sumHeuresNormale = 0;
                    double sumHeuresMajoration = 0;
                    double sumHeuresAbsence = 0;
                    double sumMaterielMarche = 0;
                    double sumHeuresTravaillees = 0;
                    double sumHeuresAPieds = 0;
                    double sumHeuresPointees = 0;

                    foreach (var ligneRapport in groupByDate.ListPointages)
                    {
                        sumHeuresNormale += ligneRapport.HeureNormale;
                        sumHeuresMajoration += ligneRapport.HeureMajoration;
                        sumHeuresAbsence += ligneRapport.HeureAbsence;
                        sumMaterielMarche += ligneRapport.MaterielMarche;
                        // Gestion IVD
                        if (ligneRapport.DeplacementIV)
                        {
                            dicoIVD[day] = "X";
                        }

                        // Gestion des absences
                        if (ligneRapport.CodeAbsenceId.HasValue)
                        {
                            if (!listAbsence.Contains(ligneRapport.CodeAbsence.Code))
                            {
                                var absences = new Absence();
                                LoadDico(absences.DicoAbsence);
                                absences.CodeAbsence = ligneRapport.CodeAbsence.Code;
                                absences.DicoAbsence[day] = CalculValeurAffichee(isCadre, ligneRapport.HeureAbsence, heuresJour).ToString();
                                listAbsence.Add(absences);
                            }
                            else
                            {
                                var abs = listAbsence.Get(ligneRapport.CodeAbsence.Code);
                                if (abs != null)
                                {
                                    abs.DicoAbsence[day] = CalculValeurAffichee(isCadre, ligneRapport.HeureAbsence, heuresJour).ToString();
                                }
                            }
                        }

                        // Gestion des déplacements
                        if (ligneRapport.CodeDeplacementId.HasValue)
                        {
                            if (!listDeplacement.Contains(ligneRapport.CodeDeplacement.Code))
                            {
                                var deplacements = new Deplacements();
                                LoadDico(deplacements.DicoDeplacement);
                                deplacements.CodeDeplacement = ligneRapport.CodeDeplacement.Code;
                                deplacements.DicoDeplacement[day] = "X";
                                listDeplacement.Add(deplacements);
                            }
                            else
                            {
                                var dep = listDeplacement.Get(ligneRapport.CodeDeplacement.Code);
                                if (dep != null)
                                {
                                    dep.DicoDeplacement[day] = "X";
                                }
                            }
                        }

                        // Gestion des primes
                        foreach (var prime in ligneRapport.ListePrimes.Where(p => p.IsChecked || p.HeurePrime > 0))
                        {

                            if ((prime.Prime.Code == "GDI" || prime.Prime.Code == "GDP") && !totalprime.ContainsKey("IGD"))
                            {
                                totalprime.Add("IGD", 0);
                            }
                            else if (prime.Prime.Code.Contains("IPD") && !totalprime.ContainsKey("IPD"))
                            {
                                totalprime.Add("IPD", 0);
                            }
                            else if (!totalprime.ContainsKey(prime.Prime.CodeLibelle) && !(prime.Prime.Code == "GDI" || prime.Prime.Code == "GDP" || prime.Prime.Code.Contains("IPD")))
                            {
                                totalprime.Add(prime.Prime.CodeLibelle, 0);
                            }

                            if (!listPrimeJours.Contains(prime.Prime.CodeLibelle))
                            {
                                var primeJours = new PrimeJours();
                                LoadDico(primeJours.DicoPrimes);
                                primeJours.CodePrime = prime.Prime.CodeLibelle;
                                if (prime.Prime.PrimeType == ListePrimeType.PrimeTypeHoraire && prime.HeurePrime > 0)
                                {
                                    primeJours.DicoPrimes[day] = prime.HeurePrime.ToString();
                                    totalprime[prime.Prime.CodeLibelle] += prime.HeurePrime.Value;
                                }
                                else if (prime.Prime.PrimeType == ListePrimeType.PrimeTypeJournaliere && prime.IsChecked)
                                {

                                    if (prime.Prime.Code == "GDI")
                                    {
                                        primeJours.DicoPrimes[day] = "I";
                                        primeJours.CodePrime = "IGD";
                                        totalprime["IGD"] += 1;
                                    }
                                    else if (prime.Prime.Code == "GDP")
                                    {
                                        primeJours.DicoPrimes[day] = "P";
                                        primeJours.CodePrime = "IGD";
                                        totalprime["IGD"] += 1;
                                    }
                                    else if (prime.Prime.Code.Contains("IPD"))
                                    {
                                        primeJours.DicoPrimes[day] = Regex.Split(prime.Prime.Code, "IPD")?[1];
                                        primeJours.CodePrime = "IPD";
                                        totalprime["IPD"] += 1;
                                    }

                                    else
                                    {
                                        primeJours.DicoPrimes[day] = "X";
                                        totalprime[prime.Prime.CodeLibelle] += 1;
                                    }
                                }

                                listPrimeJours.Add(primeJours);
                            }
                            else
                            {
                                var prim = listPrimeJours.Get(prime.Prime.CodeLibelle);
                                if (prim != null)
                                {
                                    if (prime.Prime.PrimeType == ListePrimeType.PrimeTypeHoraire && prime.HeurePrime > 0)
                                    {
                                        prim.DicoPrimes[day] = prime.HeurePrime.ToString();
                                        totalprime[prime.Prime.CodeLibelle] += prime.HeurePrime.Value;
                                    }
                                    else if (prime.Prime.PrimeType == ListePrimeType.PrimeTypeJournaliere && prime.IsChecked)
                                    {

                                        if (prime.Prime.Code == "GDI")
                                        {
                                            prim.DicoPrimes[day] = "I";
                                            prim.CodePrime = "IGD";
                                            totalprime["IGD"] += 1;
                                        }
                                        else if (prime.Prime.Code == "GDP")
                                        {
                                            prim.DicoPrimes[day] = "P";
                                            prim.CodePrime = "IGD";
                                            totalprime["IGD"] += 1;
                                        }
                                        else if (prime.Prime.Code.Contains("IPD"))
                                        {
                                            prim.DicoPrimes[day] = Regex.Split(prime.Prime.Code, "IPD")?[1];
                                            prim.CodePrime = "IPD";
                                            totalprime["IPD"] += 1;
                                        }

                                        else
                                        {
                                            prim.DicoPrimes[day] = "X";
                                            totalprime[prime.Prime.CodeLibelle] += 1;
                                        }
                                    }
                                }
                            }

                        }
                        foreach (var item in ligneRapport.ListCodePrimeAstreintes)
                        {
                            if (item.CodeAstreinte.EstSorti)
                            {
                                dicoSortieAstreintes[day] = (double.Parse(dicoSortieAstreintes[day]) + ligneRapport.HeuresTotalAstreintes).ToString();
                            }
                            else
                            {
                                dicoPrimeAstreintes[day] = "X";
                            }
                        }
                    }

                    sumHeuresTravaillees = CalculValeurAffichee(isCadre, sumHeuresNormale + sumHeuresMajoration, heuresJour);
                    sumHeuresAPieds = sumHeuresTravaillees - CalculValeurAffichee(isCadre, sumMaterielMarche, heuresJour);
                    sumHeuresPointees = sumHeuresTravaillees + CalculValeurAffichee(isCadre, sumHeuresAbsence, heuresJour);

                    dicoHeuresNormales[day] = CalculValeurAffichee(isCadre, sumHeuresNormale, heuresJour).ToString();
                    dicoHeureMajo[day] = CalculValeurAffichee(isCadre, sumHeuresMajoration, heuresJour).ToString();
                    dicoHeuresAbsence[day] = CalculValeurAffichee(isCadre, sumHeuresAbsence, heuresJour).ToString();
                    dicoHeuresAPied[day] = sumHeuresAPieds.ToString();
                    dicoHeuresPointees[day] = sumHeuresPointees.ToString();
                    dicoHeureTravaillees[day] = sumHeuresTravaillees.ToString();
                }

                // Chargement de la classe pointageMensuelPerson
                pointageHebdomadairePerson.LoadHeuresNormales(dicoHeuresNormales);
                pointageHebdomadairePerson.LoadHeuresMajo(dicoHeureMajo);
                pointageHebdomadairePerson.LoadHeuresAPied(dicoHeuresAPied);
                pointageHebdomadairePerson.LoadHeuresAbsence(dicoHeuresAbsence);
                pointageHebdomadairePerson.LoadHeuresPointees(dicoHeuresPointees);
                pointageHebdomadairePerson.LoadHeuresTravaillees(dicoHeureTravaillees);

                if (dicoPrimeAstreintes.Any(i => i.Value != string.Empty))
                {
                    pointageHebdomadairePerson.LoadAstreinte(dicoPrimeAstreintes, "Prime d’astreinte");
                }
                if (dicoSortieAstreintes.Any(i => i.Value != "0"))
                {
                    pointageHebdomadairePerson.LoadAstreinte(dicoSortieAstreintes, "sortie d’astreinte");
                }

                pointageHebdomadairePerson.LoadIVD(dicoIVD);
                foreach (var absence in listAbsence.ListAbsence)
                {
                    pointageHebdomadairePerson.LoadAbsence(absence.DicoAbsence, absence.CodeAbsence);
                }
                foreach (var deplacement in listDeplacement.ListDeplacement)
                {
                    pointageHebdomadairePerson.LoadDeplacement(deplacement.DicoDeplacement, deplacement.CodeDeplacement);
                }
                foreach (var primeJour in listPrimeJours.ListPrimeJours)
                {
                    pointageHebdomadairePerson.LoadPrimes(primeJour.DicoPrimes, primeJour.CodePrime);
                }

                pointageHebdomadairePerson.Totalprime = totalprime;
                listePointageHebdomadairePerson.Add(pointageHebdomadairePerson);
            }

            return listePointageHebdomadairePerson;
        }

        private double CalculValeurAffichee(bool isCadre, double valeur, double heuresJour)
        {
            return !isCadre ? valeur : Math.Round(valeur / heuresJour, 2);
        }

        /// <summary>
        /// Retourne la liste des Absences Mensuels
        /// </summary>
        /// <param name="etatPaieExportModel">Model de l'etat de paie pour l'export</param>
        /// <param name="organisationPeres">list des Organisations ids </param>
        /// <returns>La liste des Absences mensuels</returns>
        private List<AbsenceLigne> LoadAbsencesMensuels(EtatPaieExportModel etatPaieExportModel, int?[] organisationPeres)
        {
            List<AbsenceLigne> listAbsenceLigne = new List<AbsenceLigne>();
            List<PointageBase> listRapportLigne = new List<PointageBase>();

            //récupérer tout les poitages concernent les organisations  
            foreach (var idOrganisationPere in organisationPeres)
            {
                listRapportLigne.AddRange(this.pointageManager
                                  .GetListePointageMensuel(etatPaieExportModel, idOrganisationPere)
                                  .Where(p => p.Personnel != null)
                                  .ToList());
            }
            // supprimer des doublons
            listRapportLigne = listRapportLigne.GroupBy(r => r.PointageId).Select(i => i.First()).ToList();


            var listGroupByPerso = listRapportLigne.GroupBy(p => p.PersonnelId).Select(g => new { PersonnelId = g.Key, ListPointages = g.ToArray() }).ToList();

            // On traite les pointages de chaque Personnel
            foreach (var groupByPerso in listGroupByPerso)
            {
                var listAbsencesD = new ListAbsences();
                var listPointages = groupByPerso.ListPointages;
                var premiereLigne = listPointages.First();
                bool isCadre = GetPersonnelStatut(premiereLigne.Personnel.Statut) == Constantes.PersonnelStatutValue.Cadre;

                string heursJour = "7";//default 7
                if (isCadre)
                {
                    heursJour = paramsManager.GetParamValue(premiereLigne.Personnel.Societe.Organisation.OrganisationId, "HeuresJourIAC") ?? (7d).ToString();
                }

                var listGroupByDate = groupByPerso.ListPointages.GroupBy(p => p.DatePointage).Select(g => new { Date = g.Key, ListPointages = g.ToArray() }).ToList();

                Dictionary<int, string> dicoHeuresAbsence = new Dictionary<int, string>();
                LoadDico(dicoHeuresAbsence);
                foreach (var groupByDate in listGroupByDate)
                {
                    int day = groupByDate.Date.Day;
                    double sumHeuresAbsence = 0;
                    foreach (var ligneRapport in groupByDate.ListPointages)
                    {
                        sumHeuresAbsence += ligneRapport.HeureAbsence;
                        if (ligneRapport.CodeAbsenceId.HasValue)
                        {

                            if (!listAbsencesD.Contains(ligneRapport.CodeAbsence.Code, ligneRapport.Ci.Libelle))
                            {
                                var absencesD = new Absence();
                                LoadDico(absencesD.DicoAbsence);
                                absencesD.CodeAbsence = ligneRapport.CodeAbsence.Code ?? string.Empty;
                                absencesD.LibelleCodeAbsence = ligneRapport.CodeAbsence.Libelle ?? string.Empty;
                                absencesD.CILibelle = ligneRapport.Ci.Libelle ?? string.Empty;
                                absencesD.DicoAbsence[day] = ligneRapport.HeureAbsence.ToString();
                                listAbsencesD.Add(absencesD);
                            }
                            else
                            {
                                var abs = listAbsencesD.Get(ligneRapport.CodeAbsence.Code, ligneRapport.Ci.Libelle);
                                if (abs != null)
                                {
                                    abs.DicoAbsence[day] = ligneRapport.HeureAbsence.ToString();
                                }
                            }
                        }

                    }
                    dicoHeuresAbsence[day] = sumHeuresAbsence.ToString();
                }

                foreach (var absence in listAbsencesD.ListAbsence)
                {
                    AbsenceLigne absenceL = new AbsenceLigne();

                    absenceL.Etablissement = premiereLigne.Personnel.EtablissementPaie ?? new EtablissementPaieEnt() { Libelle = string.Empty };
                    absenceL.Personnel = premiereLigne.Personnel.PrenomNom;
                    absenceL.Statut = GetPersonnelStatut(premiereLigne.Personnel.Statut);
                    absenceL.CILibelle = absence.CILibelle;
                    absenceL.Code = absence.LibelleCodeAbsence;

                    double sumHeuresLigne = 0d;
                    var inPlage = false;

                    for (int i = 1; i <= 31; i++)
                    {
                        if (!string.IsNullOrEmpty(absence.DicoAbsence[i]) && double.Parse(absence.DicoAbsence[i]) > 0d && inPlage == false)
                        {
                            inPlage = true;
                            absenceL.Du = new DateTime(etatPaieExportModel.Annee, etatPaieExportModel.Mois, i).ToString("dd/MM/yyyy");
                            sumHeuresLigne += double.Parse(absence.DicoAbsence[i]);
                            if (i == 31)
                            {
                                inPlage = false;
                                if (string.IsNullOrEmpty(absenceL.Au))
                                {
                                    absenceL.Au = absenceL.Du;
                                }
                                //"IAC" pointent en jours (ou fraction de journée parmi 0.25, 0.5, 0.75 et 1) ex : 7h=1j
                                absenceL.Heures = absenceL.Statut != Constantes.PersonnelStatutValue.Cadre ? sumHeuresLigne.ToString() + " H" : System.Math.Round((sumHeuresLigne / double.Parse(heursJour)), 2).ToString() + " J";
                                listAbsenceLigne.Add(absenceL);

                                absenceL = new AbsenceLigne();

                                absenceL.Etablissement = premiereLigne.Personnel.EtablissementPaie ?? new EtablissementPaieEnt() { Libelle = string.Empty };
                                absenceL.Personnel = premiereLigne.Personnel.PrenomNom;
                                absenceL.Statut = GetPersonnelStatut(premiereLigne.Personnel.Statut);
                                absenceL.CILibelle = absence.CILibelle;
                                absenceL.Code = absence.LibelleCodeAbsence;
                                sumHeuresLigne = 0d;
                            }
                        }
                        else if (!string.IsNullOrEmpty(absence.DicoAbsence[i]) && double.Parse(absence.DicoAbsence[i]) > 0d && inPlage == true)
                        {
                            absenceL.Au = new DateTime(etatPaieExportModel.Annee, etatPaieExportModel.Mois, i).ToString("dd/MM/yyyy");
                            sumHeuresLigne += double.Parse(absence.DicoAbsence[i]);
                            if (i == 31)
                            {
                                inPlage = false;
                                if (string.IsNullOrEmpty(absenceL.Au))
                                {
                                    absenceL.Au = absenceL.Du;
                                }
                                //"IAC" pointent en jours (ou fraction de journée parmi 0.25, 0.5, 0.75 et 1) 7h=1j
                                absenceL.Heures = absenceL.Statut != Constantes.PersonnelStatutValue.Cadre ? sumHeuresLigne.ToString() + " H" : System.Math.Round((sumHeuresLigne / double.Parse(heursJour)), 2).ToString() + " J";
                                listAbsenceLigne.Add(absenceL);

                                absenceL = new AbsenceLigne();

                                absenceL.Etablissement = premiereLigne.Personnel.EtablissementPaie ?? new EtablissementPaieEnt() { Libelle = string.Empty };
                                absenceL.Personnel = premiereLigne.Personnel.PrenomNom;
                                absenceL.Statut = GetPersonnelStatut(premiereLigne.Personnel.Statut);
                                absenceL.CILibelle = absence.CILibelle;
                                absenceL.Code = absence.LibelleCodeAbsence;
                                sumHeuresLigne = 0d;
                            }
                        }
                        else if ((string.IsNullOrEmpty(absence.DicoAbsence[i]) || double.Parse(absence.DicoAbsence[i]) <= 0) && inPlage == true)
                        {
                            inPlage = false;
                            if (string.IsNullOrEmpty(absenceL.Au))
                            {
                                absenceL.Au = absenceL.Du;
                            }

                            absenceL.Heures = absenceL.Statut != Constantes.PersonnelStatutValue.Cadre ? sumHeuresLigne.ToString() + " H" : (System.Math.Round((sumHeuresLigne / double.Parse(heursJour)) * 4, MidpointRounding.ToEven) / 4).ToString() + " J";
                            listAbsenceLigne.Add(absenceL);

                            absenceL = new AbsenceLigne();

                            absenceL.Etablissement = premiereLigne.Personnel.EtablissementPaie ?? new EtablissementPaieEnt() { Libelle = string.Empty };
                            absenceL.Personnel = premiereLigne.Personnel.PrenomNom;
                            absenceL.Statut = GetPersonnelStatut(premiereLigne.Personnel.Statut);
                            absenceL.CILibelle = absence.CILibelle;
                            absenceL.Code = absence.LibelleCodeAbsence;
                            sumHeuresLigne = 0d;
                        }
                    }
                }
            }

            return listAbsenceLigne;
        }

        /// <summary>
        /// Génère un pdf pour le controle des pointages Hebdomadaire
        /// </summary>
        /// <param name="etatPaieExportModel">Model de l'etat de paie pour l'export</param>
        /// <param name="user">user</param>
        /// <returns>Le fichier pdf généré</returns>
        public MemoryStream GenerateControlePointagesHebdomadaire(EtatPaieExportModel etatPaieExportModel, UtilisateurEnt user, string templateFolderPath)
        {
            DateTime firstDay = etatPaieExportModel.Date.Value.AddDays(1);
            var listePointageMensuel = LoadPointagesHebdomadaire(firstDay, etatPaieExportModel);
            //le tableau inclut le samedi et le dimanche qui précèdent la semaine de travail affichée
            //Récupére les personnels qui n'ont pas de pointage et qui sont actif
            listePointageMensuel = GetPersonnelWithOutPointages(listePointageMensuel, etatPaieExportModel);
            return controlePointagesHebdomadaireManager.GenerateMemoryStreamControlePointagesHebdomadaire(etatPaieExportModel, listePointageMensuel, firstDay, user.UtilisateurId, templateFolderPath);
        }

        /// <summary>
        /// Get personnel statut by statut id
        /// </summary>
        /// <param name="statutId">Statut identifier</param>
        /// <returns>Statut</returns>
        private static string GetPersonnelStatut(string statutId)
        {
            switch (statutId)
            {
                case "1":
                    return Constantes.PersonnelStatutValue.Ouvrier;
                case "2":
                    return Constantes.PersonnelStatutValue.ETAM;
                case "3":
                    return Constantes.PersonnelStatutValue.Cadre;
                case "4":
                    return Constantes.PersonnelStatutValue.ETAM;
                case "5":
                    return Constantes.PersonnelStatutValue.ETAM;
                default:
                    return string.Empty;
            }
        }

        /// <summary>
        /// Charge un dictionnaire avec un index pour chaque jour du mois et une chaine de caractère vide pour valeur
        /// </summary>
        /// <param name="dico">Dictionnaire à mettre à jour</param>
        /// <param name="byZero">Charge dictionnaire avec des Zeros</param>
        private void LoadDico(Dictionary<int, string> dico, bool byZero = false)
        {
            for (int i = 1; i <= 31; i++)
            {
                dico[i] = byZero ? "0" : string.Empty;
            }
        }

        /// <summary>
        /// Instanciate and Charge un dictionnaire avec un index pour chaque jour du mois et une chaine de caractère vide pour valeur
        /// </summary>
        /// <param name="byZero">Charge dictionnaire avec des Zeros</param>
        /// <returns>Dico</returns>
        private Dictionary<int, string> InstanciateAndLoadDico(bool byZero = false)
        {
            Dictionary<int, string> dico = new Dictionary<int, string>();
            for (int i = 1; i <= 31; i++)
            {
                dico[i] = byZero ? "0" : string.Empty;
            }
            return dico;
        }

        /// <summary>
        /// Méthode de génération d'un excel pour les primes
        /// </summary>
        /// <param name="insertPrimeParameters">Liste des paramètres pour la requête d'insertion des primes</param>
        /// <returns>Un workbook</returns>
        public string BuildInsertQueryPrimeParameters(List<InsertQueryPrimeParametersModel> insertPrimeParameters)
        {
            string pathExcel = AppDomain.CurrentDomain.BaseDirectory + "/Templates/TemplateInsertQueriesPrime.xlsx";
            var timeStampStr = DateTime.Now.ToString("yyyy_MM_dd_H_mm");
            string pathTempFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Temp", timeStampStr + "_InsertPrimeParameters.xlsx");
            ExcelFormat excelFormat = new ExcelFormat();
            IWorkbook workbook = excelFormat.OpenTemplateWorksheet(pathExcel);
            excelFormat.InitVariables(workbook);
            excelFormat.AddVariable("Parameters", insertPrimeParameters);
            excelFormat.ApplyVariables();
            var memStream = GeneratePdfOrExcel(false, excelFormat, workbook);
            ////write to file
            FileStream file = new FileStream(pathTempFile, FileMode.Create, FileAccess.ReadWrite);
            memStream.WriteTo(file);
            file.Close();
            memStream.Close();
            return pathTempFile;
        }

        /// <summary>
        /// Méthode de génération d'un excel pour les pointages
        /// </summary>
        /// <param name="insertPointageParameters">Liste des paramètres pour la requête d'insertion des pointages</param>
        /// <returns>Un workbook</returns>
        public string BuildInsertQueryPointageParameters(List<InsertQueryPointageParametersModel> insertPointageParameters)
        {
            string pathExcel = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "TemplateInsertQueriesPointage.xlsx");
            var timeStampStr = DateTime.Now.ToString("yyyy_MM_dd_H_mm");
            string pathTempFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Temp", timeStampStr + "_InsertPointageParameters.xlsx");
            ExcelFormat excelFormat = new ExcelFormat();
            IWorkbook workbook = excelFormat.OpenTemplateWorksheet(pathExcel);
            excelFormat.InitVariables(workbook);
            excelFormat.AddVariable("Parameters", insertPointageParameters);
            excelFormat.ApplyVariables();
            var memStream = GeneratePdfOrExcel(false, excelFormat, workbook);
            ////write to file
            FileStream file = new FileStream(pathTempFile, FileMode.Create, FileAccess.ReadWrite);
            memStream.WriteTo(file);
            file.Close();
            memStream.Close();
            return pathTempFile;
        }

        /// <summary>
        /// Handle absence pour l'export pointage Mensuelle FES
        /// </summary>
        /// <param name="ligneRapport">Rapport Ligne</param>
        /// <param name="dico">Dictionnaiare</param>
        /// <param name="listAbsence">List des absences</param>
        /// <param name="day">day of week</param>
        /// <param name="isCadre">Indique si le personnel possède le statut cadre</param>
        /// <param name="heuresParJour">Quota d'heures journalier</param>
        private void HandleAbsencePointageMensuelleFes(RapportLigneEnt ligneRapport, Dictionary<int, string> dico, ListAbsences listAbsence, int day, bool isCadre, string heuresParJour)
        {
            if (ligneRapport.CodeAbsenceId.HasValue)
            {
                if (!listAbsence.Contains(ligneRapport.CodeAbsence.Code))
                {
                    var absences = new Absence();
                    absences.DicoAbsence = dico;
                    absences.CodeAbsence = ligneRapport.CodeAbsence.Code;
                    absences.DicoAbsence[day] = !isCadre ? ligneRapport.HeureAbsence.ToString() : Math.Round(ligneRapport.HeureAbsence / double.Parse(heuresParJour), 2).ToString();
                    listAbsence.Add(absences);
                }
                else
                {
                    var abs = listAbsence.Get(ligneRapport.CodeAbsence.Code);
                    if (abs != null)
                    {
                        abs.DicoAbsence[day] = !isCadre ? ligneRapport.HeureAbsence.ToString() : Math.Round(ligneRapport.HeureAbsence / double.Parse(heuresParJour), 2).ToString();
                    }
                }
            }
        }

        /// <summary>
        /// Handle déplacement pour l'export pointage Mensuelle FES
        /// </summary>
        /// <param name="ligneRapport">Rapport ligne</param>
        /// <param name="dico">Dictionnaiare</param>
        /// <param name="listDeplacement">List des déplacements</param>
        /// <param name="day">day of week</param>
        private void HandleDeplacementPointageMensuelleFes(RapportLigneEnt ligneRapport, Dictionary<int, string> dico, ListDeplacements listDeplacement, int day)
        {
            if (ligneRapport.CodeDeplacementId.HasValue)
            {
                if (!listDeplacement.Contains(ligneRapport.CodeDeplacement.Code))
                {
                    var deplacements = new Deplacements();
                    deplacements.DicoDeplacement = dico;
                    deplacements.CodeDeplacement = ligneRapport.CodeDeplacement.Code;
                    deplacements.DicoDeplacement[day] = "X";
                    listDeplacement.Add(deplacements);
                }
                else
                {
                    var dep = listDeplacement.Get(ligneRapport.CodeDeplacement.Code);
                    if (dep != null)
                    {
                        dep.DicoDeplacement[day] = "X";
                    }
                }
            }
        }

        /// <summary>
        /// Handle déplacement pour l'export pointage Mensuelle FES
        /// </summary>
        /// <param name="ligneRapport">Rapport ligne</param>
        /// <param name="dicoCodeZoneDeplacement">dictionnaire des codes zone déplacement</param>
        /// <param name="day">day of week</param>
        private void HandleCodeZoneDeplacementPointageMensuelleFes(RapportLigneEnt ligneRapport, ref Dictionary<int, string> dicoCodeZoneDeplacement, int day)
        {
            if (ligneRapport.CodeZoneDeplacementId.HasValue)
            {
                if (dicoCodeZoneDeplacement == null)
                {
                    dicoCodeZoneDeplacement = new Dictionary<int, string>();
                    LoadDico(dicoCodeZoneDeplacement);
                }
                if (dicoCodeZoneDeplacement[day] != string.Empty)
                {
                    string codeZone1 = dicoCodeZoneDeplacement[day];
                    string codeZone2 = ligneRapport.CodeZoneDeplacement.Code;
                    dicoCodeZoneDeplacement[day] = codeZoneDeplacementManager.GetCompareBiggestCodeZondeDeplacement(codeZone1, codeZone2, ligneRapport.Ci.SocieteId.Value);
                }
                else
                {
                    dicoCodeZoneDeplacement[day] = ligneRapport.CodeZoneDeplacement.Code;
                }
            }
        }

        /// <summary>
        /// Handle Prime pour l'export pointage Mensuelle FES
        /// </summary>
        /// <param name="ligneRapport">Rapport ligne</param>
        /// <param name="totalprime">total des primes</param>
        /// <param name="day">day of week</param>
        /// <param name="listPrimeJours">List des primes par jours</param>
        /// <param name="dico">Dictionnaire</param>
        private void HandlePrimePointageMensuelleFes(RapportLigneEnt ligneRapport, Dictionary<string, double> totalprime, int day, ListPrimesJours listPrimeJours, Dictionary<int, string> dico)
        {
            foreach (var prime in ligneRapport.ListePrimes.Where(p => p.IsChecked || p.HeurePrime > 0))
            {
                if ((prime.Prime.Code == "GDI" || prime.Prime.Code == "GDP") && !totalprime.ContainsKey("IGD"))
                {
                    totalprime.Add("IGD", 0);
                }
                else if (prime.Prime.Code.Contains("IPD") && !totalprime.ContainsKey("IPD"))
                {
                    totalprime.Add("IPD", 0);
                }
                else if (!totalprime.ContainsKey(prime.Prime.CodeLibelle) && !(prime.Prime.Code == "GDI" || prime.Prime.Code == "GDP" || prime.Prime.Code.Contains("IPD")))
                {
                    totalprime.Add(prime.Prime.CodeLibelle, 0);
                }

                if (!listPrimeJours.Contains(prime.Prime.CodeLibelle))
                {
                    var primeJours = new PrimeJours();
                    primeJours.DicoPrimes = new Dictionary<int, string>(dico);
                    primeJours.CodePrime = prime.Prime.CodeLibelle;
                    if (prime.Prime.PrimeType == ListePrimeType.PrimeTypeHoraire && prime.HeurePrime > 0)
                    {
                        primeJours.DicoPrimes[day] = prime.HeurePrime.ToString();
                        totalprime[prime.Prime.CodeLibelle] += prime.HeurePrime.Value;
                    }
                    else if (prime.Prime.PrimeType == ListePrimeType.PrimeTypeJournaliere && prime.IsChecked)
                    {
                        if (prime.Prime.Code == "GDI")
                        {
                            primeJours.DicoPrimes[day] = "I";
                            primeJours.CodePrime = "IGD";
                            totalprime["IGD"] += 1;
                        }
                        else if (prime.Prime.Code == "GDP")
                        {
                            primeJours.DicoPrimes[day] = "P";
                            primeJours.CodePrime = "IGD";
                            totalprime["IGD"] += 1;
                        }
                        else if (prime.Prime.Code.Contains("IPD"))
                        {
                            primeJours.DicoPrimes[day] = Regex.Split(prime.Prime.Code, "IPD")?[1];
                            primeJours.CodePrime = "IPD";
                            totalprime["IPD"] += 1;
                        }

                        else
                        {
                            primeJours.DicoPrimes[day] = "X";
                            totalprime[prime.Prime.CodeLibelle] += 1;
                        }
                    }

                    listPrimeJours.Add(primeJours);
                }
                else
                {
                    var prim = listPrimeJours.Get(prime.Prime.CodeLibelle);
                    if (prim != null)
                    {
                        if (prime.Prime.PrimeType == ListePrimeType.PrimeTypeHoraire && prime.HeurePrime > 0)
                        {
                            prim.DicoPrimes[day] = prime.HeurePrime.ToString();
                            totalprime[prime.Prime.CodeLibelle] += prime.HeurePrime.Value;
                        }
                        else if (prime.Prime.PrimeType == ListePrimeType.PrimeTypeJournaliere && prime.IsChecked)
                        {
                            if (prime.Prime.Code == "GDI")
                            {
                                prim.DicoPrimes[day] = "I";
                                prim.CodePrime = "IGD";
                                totalprime["IGD"] += 1;
                            }
                            else if (prime.Prime.Code == "GDP")
                            {
                                prim.DicoPrimes[day] = "P";
                                prim.CodePrime = "IGD";
                                totalprime["IGD"] += 1;
                            }
                            else if (prime.Prime.Code.Contains("IPD"))
                            {
                                prim.DicoPrimes[day] = Regex.Split(prime.Prime.Code, "IPD")?[1];
                                prim.CodePrime = "IPD";
                                totalprime["IPD"] += 1;
                            }

                            else
                            {
                                prim.DicoPrimes[day] = "X";
                                totalprime[prime.Prime.CodeLibelle] += 1;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// pour l'export pointage Mensuelle FES
        /// </summary>
        /// <param name="ligneRapport">Rapport ligne</param>
        /// <param name="dicoSortieAstreintes">Dictionnaire des sorties astreintes</param>
        /// <param name="dicoPrimeAstreintes">Dictionnaire des primes astreintes</param>
        /// <param name="day">day of week</param>
        private void HandleAstreintePointageMensuelleFes(RapportLigneEnt ligneRapport, Dictionary<int, string> dicoSortieAstreintes, Dictionary<int, string> dicoPrimeAstreintes, int day)
        {
            // test if is FES
            if (ligneRapport.Ci?.Societe?.Groupe != null && ligneRapport.Ci.Societe.Groupe.Code.Equals("GFES"))
            {
                double totalHeuresAstreinte = 0;
                foreach (var astreinte in ligneRapport.ListRapportLigneAstreintes)
                {
                    totalHeuresAstreinte += (astreinte.DateFinAstreinte - astreinte.DateDebutAstreinte).TotalHours;
                }
                ligneRapport.HeuresTotalAstreintes = totalHeuresAstreinte;
            }


            foreach (var item in ligneRapport.ListCodePrimeAstreintes)
            {
                if (item.CodeAstreinte.EstSorti)
                {
                    dicoSortieAstreintes[day] = (double.Parse(dicoSortieAstreintes[day]) + ligneRapport.HeuresTotalAstreintes).ToString();
                }
                else
                {
                    dicoPrimeAstreintes[day] = "X";
                }
            }
        }
    }
}

#pragma warning restore S3776
