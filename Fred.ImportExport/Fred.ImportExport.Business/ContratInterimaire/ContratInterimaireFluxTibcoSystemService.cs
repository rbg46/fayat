using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using Fred.Business.Budget;
using Fred.Business.CI;
using Fred.Business.Groupe;
using Fred.Business.Personnel;
using Fred.Business.Personnel.Interimaire;
using Fred.Business.Referential;
using Fred.Business.Societe;
using Fred.Business.Unite;
using Fred.Business.Utilisateur;
using Fred.Entities.CI;
using Fred.Entities.Personnel;
using Fred.Entities.Personnel.Interimaire;
using Fred.Framework.Exceptions;
using Fred.ImportExport.Business.Common;
using Fred.ImportExport.DataAccess.ContratInterimaire;
using Fred.ImportExport.DataAccess.ExternalService;
using Fred.ImportExport.DataAccess.ExternalService.GetDetailContratProxy;
using Fred.ImportExport.DataAccess.ExternalService.Tibco.ContratInterimaire;
using Fred.ImportExport.DataAccess.Interfaces;
using Fred.ImportExport.Entities.ImportExport;
using Fred.ImportExport.Framework.Exceptions;

namespace Fred.ImportExport.Business.ContratInterimaire
{
    public class ContratInterimaireFluxTibcoSystemService : IContratInterimaireFluxTibcoSystemService
    {
        private readonly IInterfaceTransfertDonneeRepository interfaceTransfertDonneeRepository;
        private readonly IPersonnelManager personnelManager;
        private readonly IPaysManager paysManager;
        private readonly ISocieteManager societeManager;
        private readonly IGroupeManager groupeManager;
        private readonly IContratInterimaireManager contratInterimaireManager;
        private readonly IContratInterimaireImportManager contratInterimaireImportManager;
        private readonly IEtatContratInterimaireManager etatContratInterimaireManager;
        private readonly IUniteManager uniteManager;
        private readonly IFournisseurManager fournisseurManager;
        private readonly ICIManager ciManager;
        private readonly IBudgetMainManager budgetMainManager;
        private readonly IUtilisateurManager utilisateurManager;
        private readonly ITranspoCodeEmploiToRessourceRepository transpoCodeEmploiToRessourceRepository;

        /// <summary>
        /// Obtient l'identifiant du job d'exprot (Code du flux d'export).
        /// </summary>
        public string ExportJobId { get; } = ConfigurationManager.AppSettings["Tibco:ExportPersonnel"];

        /// <summary>
        /// Liste des codes statut personnel.
        /// </summary>
        public List<int> ListStatut { get; } = new List<int>() { 1, 2, 3, 4, 5 };

        public ContratInterimaireFluxTibcoSystemService(
            IInterfaceTransfertDonneeRepository interfaceTransfertDonneeRepository,
            IPersonnelManager personnelManager,
            IPaysManager paysManager,
            ISocieteManager societeManager,
            IGroupeManager groupeManager,
            IContratInterimaireManager contratInterimaireManager,
            IContratInterimaireImportManager contratInterimaireImportManager,
            IEtatContratInterimaireManager etatContratInterimaireManager,
            IUniteManager uniteManager,
            IFournisseurManager fournisseurManager,
            ICIManager ciManager,
            IBudgetMainManager budgetMainManager,
            IUtilisateurManager utilisateurManager,
            ITranspoCodeEmploiToRessourceRepository transpoCodeEmploiToRessourceRepository)
        {
            this.interfaceTransfertDonneeRepository = interfaceTransfertDonneeRepository;
            this.personnelManager = personnelManager;
            this.paysManager = paysManager;
            this.societeManager = societeManager;
            this.groupeManager = groupeManager;
            this.contratInterimaireManager = contratInterimaireManager;
            this.contratInterimaireImportManager = contratInterimaireImportManager;
            this.etatContratInterimaireManager = etatContratInterimaireManager;
            this.uniteManager = uniteManager;
            this.fournisseurManager = fournisseurManager;
            this.ciManager = ciManager;
            this.budgetMainManager = budgetMainManager;
            this.utilisateurManager = utilisateurManager;
            this.transpoCodeEmploiToRessourceRepository = transpoCodeEmploiToRessourceRepository;
        }


        /// <summary>
        /// Exporte le personnel Fes dernièrement modifier vers Tibco
        /// </summary>
        /// <param name="lastExecutionDate">DateTime dernière date d'execution du flux</param>
        public void ImportContratInterimairePixid(DateTime? lastExecutionDate)
        {
            try
            {
                NLog.LogManager.GetCurrentClassLogger().Info($"[IMPORT_CONTRAT_INTERIMAIRE_PIXID] Lancement de l'import contrat interimaire PIXID de TIBCO vers FRED.");

                ImportListeContratATraiter(lastExecutionDate);

                ImportDetailContrat();

                NLog.LogManager.GetCurrentClassLogger().Info($"[IMPORT_CONTRAT_INTERIMAIRE_PIXID] L'import de TIBCO vers FRED s'est déroulé avec succès.");
            }
            catch (Exception e)
            {
                var msg = string.Format(IEBusiness.FluxErreurExport, ExportJobId);
                var exception = new FredBusinessException(msg, e);
                NLog.LogManager.GetCurrentClassLogger().Error($"[IMPORT_CONTRAT_INTERIMAIRE_PIXID] L'import de TIBCO vers FRED est en échec.");
                throw exception;
            }

        }

        private void ImportListeContratATraiter(DateTime? lastExecutionDate)
        {
            // RG_3465_018 : Etape A – Récupérer la liste des nouveaux contrats à charger depuis la dernière exécution (API GetListeContratATraiter)
            var contratInterimaireRepoExterne = new ImportContratInterimaireRepositoryExterne();
            if (!lastExecutionDate.HasValue)
            {
                lastExecutionDate = new DateTime(1900, 01, 01);
            }

            var result = contratInterimaireRepoExterne.GetListContratATraiter(lastExecutionDate.Value);
            if (!result.Message.ToLower().Equals(Constantes.Success))
            {
                throw new FredBusinessException($"Erreur Tibco - GetListContratATraiter : {result.Message}.");
            }

            if (result.IdContratsATraiter.Any())
            {
                List<InterfaceTransfertDonneeEnt> toAdd = new List<InterfaceTransfertDonneeEnt>();
                foreach (var item in result.IdContratsATraiter)
                {
                    toAdd.Add(new InterfaceTransfertDonneeEnt()
                    {
                        CodeInterface = Constantes.Pixid,
                        CodeOrganisation = Constantes.GroupeRazelBec,
                        DonneeType = Constantes.Contrat,
                        DonneeID = item.ToString(),
                        DateCreation = DateTime.UtcNow,
                        Statut = 0
                    });
                }

                interfaceTransfertDonneeRepository.AddList(toAdd);
            }
        }

        private void ImportDetailContrat()
        {
            // RG_3465_019 : Etape B – Charger unitairement chaque contrat (API GetDetailContrat)
            var toTreat = interfaceTransfertDonneeRepository.GetContratAtraiter(Constantes.Pixid, Constantes.GroupeRazelBec, Constantes.Contrat);
            if (toTreat.Any())
            {
                var motifRemplacementList = contratInterimaireManager.GetMotifRemplacement();
                var contratInterimaireRepoExterne = new ImportContratInterimaireRepositoryExterne();
                foreach (var contrat in toTreat)
                {
                    try
                    {
                        var contratPixid = contratInterimaireRepoExterne.GetDetailContratFromTibco(int.Parse(contrat.DonneeID));
                        if (!contratPixid.MessageCode.ToLower().Equals(Constantes.Success) ||
                            contratPixid.DetailContratOutRecord == null)
                        {
                            // Mettre à jour le statut de Interface transfert donnee à 2
                            UpdateTransfertStatut(contrat, contratPixid, (int)Fred.Entities.StatutTransfertDonnee.TreatmentFailure);
                            continue;
                        }

                        CheckPersonnelMondatoryFields(contratPixid.DetailContratOutRecord);
                        CheckContractMondatoryFields(contratPixid.DetailContratOutRecord);
                        var fredie = utilisateurManager.GetByLogin("fred_ie");
                        var personnelId = UpdateOrCreatePersonnel(contratPixid.DetailContratOutRecord, GetSocieteInterimaireByGroupeCode(Constantes.GroupeRazelBec), fredie.UtilisateurId);
                        var statut = UpdateOrCreateContrat(contratPixid.DetailContratOutRecord, Constantes.GroupeRazelBec, personnelId, motifRemplacementList, fredie.UtilisateurId);

                        // Si tout se passe bien => Mettre à jour le statut de Interface transfert donnee à 1 ou 3 si abandonné.
                        UpdateTransfertStatut(contrat, contratPixid, statut);
                    }
                    catch (Exception ex)
                    {
                        UpdateTransfertStatut(contrat, null, (int)Fred.Entities.StatutTransfertDonnee.TreatmentFailure, ex);
                    }
                }
            }
        }

        private void UpdateTransfertStatut(InterfaceTransfertDonneeEnt tranfert, GetDetailContratResult contratPixid, int statut, Exception exception = null)
        {
            tranfert.Statut = statut;
            tranfert.DateTraitement = DateTime.UtcNow;
            if (statut != (int)Fred.Entities.StatutTransfertDonnee.Treated)
            {
                string message = string.Empty;
                if (contratPixid != null)
                {
                    message = $"Tibco : {contratPixid.MessageCode} - {contratPixid.Message}";
                }
                else
                {
                    message = exception.Message;
                }

                tranfert.Message = string.IsNullOrEmpty(tranfert.Message) ? message : $"{tranfert.Message}{Environment.NewLine}{message}";
            }

            interfaceTransfertDonneeRepository.InterfaceTransfertDonneeUpdate(tranfert);
        }

        private void CheckPersonnelMondatoryFields(GetDetailContratOutRecord contratPixid)
        {
            if (string.IsNullOrEmpty(contratPixid.PersonnelNom) || string.IsNullOrEmpty(contratPixid.PersonnelPrenom))
            {
                throw new FredIeBusinessException("Impossible de charger le contrat : information(s) obligatoire(s) manquante(s) sur le Personnel (Nom, Prénom).");
            }
        }

        private void CheckContractMondatoryFields(GetDetailContratOutRecord contratPixid)
        {
            if (string.IsNullOrEmpty(contratPixid.ContratNumeroExterne) ||
                string.IsNullOrEmpty(contratPixid.ContratDateDebut) ||
                string.IsNullOrEmpty(contratPixid.ContratDateFin) ||
                string.IsNullOrEmpty(contratPixid.ContratTarif))
            {
                throw new FredIeBusinessException("Impossible de charger le contrat : information(s) obligatoire(s) manquante(s) sur le Contrat (Numéro, Date Début, Date Fin, Tarif).");
            }
        }

        private int UpdateOrCreatePersonnel(GetDetailContratOutRecord contratPixid, int? societeId, int fredIeUserId)
        {
            if (!string.IsNullOrEmpty(contratPixid.PersonnelMatriculeExterne))
            {
                var personnelId = UpdatePersonnel(contratPixid, fredIeUserId);
                if (personnelId.HasValue)
                {
                    return personnelId.Value;
                }
            }

            return AddPersonnel(contratPixid, societeId, fredIeUserId);
        }

        private int? UpdatePersonnel(GetDetailContratOutRecord contratPixid, int fredIeUserId)
        {
            var personnel = personnelManager.GetPersonnelInterimaireByExternalMatriculeAndGroupeId(contratPixid.PersonnelMatriculeExterne, Constantes.GroupeRazelBec, Constantes.Pixid);
            if (personnel != null)
            {
                // RG_3465_025 - Personnel existant trouvé – Rafraîchissement des données du Personnel
                if (personnel.TimestampImport < ulong.Parse(contratPixid.Timestamp))
                {
                    personnel.Nom = contratPixid.PersonnelNom;
                    personnel.Prenom = contratPixid.PersonnelPrenom;
                    personnel.CodePostal = contratPixid.PersonnelAdresseCP;
                    personnel.Ville = contratPixid.PersonnelAdresseVille;
                    var pays = paysManager.GetByCode(contratPixid.PersonnelAdressePaysISO);
                    personnel.PaysId = pays?.PaysId;
                    personnel.PaysLabel = pays?.Libelle;
                    personnel.TimestampImport = ulong.Parse(contratPixid.Timestamp);
                    personnel.UtilisateurIdModification = fredIeUserId;
                    var contratDateDebut = DateTime.Parse(contratPixid.ContratDateDebut);
                    if (contratDateDebut < personnel.DateEntree)
                    {
                        personnel.DateEntree = contratDateDebut;
                    }

                    personnelManager.Update(personnel, fredIeUserId);
                }
                return personnel.PersonnelId;
            }

            return null;
        }

        private int AddPersonnel(GetDetailContratOutRecord contratPixid, int? societeId, int fredIeUserId)
        {
            // RG_3465_024 – Pas de Personnel existant trouvé – Création d’un nouveau Personnel
            var newPersonnel = new PersonnelEnt();
            newPersonnel.IsInterimaire = true;
            newPersonnel.IsInterne = false;
            newPersonnel.Nom = contratPixid.PersonnelNom;
            newPersonnel.Prenom = contratPixid.PersonnelPrenom;
            newPersonnel.DateEntree = DateTime.Parse(contratPixid.ContratDateDebut);
            newPersonnel.CodePostal = contratPixid.PersonnelAdresseCP;
            newPersonnel.Ville = contratPixid.PersonnelAdresseVille;
            var pays = paysManager.GetByCode(contratPixid.PersonnelAdressePaysISO);
            newPersonnel.PaysId = pays?.PaysId;
            newPersonnel.PaysLabel = pays?.Libelle;
            newPersonnel.SocieteId = societeId;
            newPersonnel.TimestampImport = ulong.Parse(contratPixid.Timestamp);
            newPersonnel.UtilisateurIdCreation = fredIeUserId;
            newPersonnel.MatriculeExterne = new List<MatriculeExterneEnt>()
                    {
                        new MatriculeExterneEnt()
                        {
                            Matricule = contratPixid.PersonnelMatriculeExterne,
                            Source = Constantes.Pixid
                        }
                    };

            newPersonnel = personnelManager.Add(newPersonnel, fredIeUserId);
            return newPersonnel.PersonnelId;
        }

        private int UpdateOrCreateContrat(GetDetailContratOutRecord contratPixid, string groupeCode, int personnelId, List<MotifRemplacementEnt> motifRemplacementList, int fredIeUserId)
        {
            var contratInterimaire = contratInterimaireManager.GetContratInterimaireByNumeroContratAndGroupeCode(contratPixid.ContratNumeroExterne, groupeCode);
            if (contratInterimaire != null)
            {
                return UpdateContrat(contratInterimaire, contratPixid, personnelId);
            }
            else
            {
                AddContrat(contratPixid, groupeCode, personnelId, motifRemplacementList, fredIeUserId);
                return (int)Fred.Entities.StatutTransfertDonnee.Treated;
            }
        }

        private int UpdateContrat(ContratInterimaireEnt contratInterimaire, GetDetailContratOutRecord contratPixid, int personnelId)
        {
            CheckContratInterimaireId(contratInterimaire, personnelId);
            // RG_3465_036 - Contrat existant trouvé – Rafraîchissement de certaines données du Contrat
            if (!contratInterimaire.TimestampImport.HasValue || contratInterimaire.TimestampImport < ulong.Parse(contratPixid.Timestamp))
            {
                DateTime datefin;
                if (DateTime.TryParse(contratPixid.ContratDateFin, out datefin))
                {
                    contratInterimaire.DateFin = datefin;
                }

                int souplesse = 0;
                if (int.TryParse(contratPixid.ContratJoursSouplesse, out souplesse))
                {
                    contratInterimaire.Souplesse = souplesse;
                }

                contratInterimaireManager.UpdateContratInterimaire(contratInterimaire);

                contratInterimaireImportManager.AddContratInterimaireImportList(contratInterimaire.ContratInterimaireId,
                                                                                ulong.Parse(contratPixid.Timestamp),
                                                                                new List<string>()
                                                                                {
                                                                                    $"{DateTime.UtcNow.ToString("JJ/MM/AA HH :mm")} – Import du contrat depuis PIXID."
                                                                                });

                return (int)Fred.Entities.StatutTransfertDonnee.Treated;
            }
            else
            {
                return (int)Fred.Entities.StatutTransfertDonnee.Abandoned;
            }
        }

        private void AddContrat(GetDetailContratOutRecord contratPixid, string groupeCode, int personnelId, List<MotifRemplacementEnt> motifRemplacementList, int fredIeUserId)
        {
            var importList = new List<string>();
            importList.Add($"{DateTime.UtcNow.ToString("dd/MM/yy HH :mm")} – Import du contrat depuis PIXID.");
            // RG_3465_027 – Pas de contrat existant trouvé - Création d’un nouveau Contrat
            var newContratInterimaire = new ContratInterimaireEnt();
            newContratInterimaire.InterimaireId = personnelId;
            newContratInterimaire.EtatContratId = etatContratInterimaireManager.GetEtatContratInterimaireByCode(Fred.Entities.Constantes.EtatContratInterimaire.Valid).EtatContratInterimaireId; // Validé
            newContratInterimaire.FournisseurId = GetFournisseurId(newContratInterimaire, contratPixid.FournisseurSIREN, groupeCode, importList);
            newContratInterimaire.SocieteId = GetSocieteId(newContratInterimaire, contratPixid.SocieteContractanteSIREN, groupeCode, importList);
            newContratInterimaire.NumContrat = contratPixid.ContratNumeroExterne;
            DateTime dateDebut;
            if (DateTime.TryParse(contratPixid.ContratDateDebut, out dateDebut))
            {
                newContratInterimaire.DateDebut = dateDebut;
            }

            DateTime dateFin;
            if (DateTime.TryParse(contratPixid.ContratDateFin, out dateFin))
            {
                newContratInterimaire.DateFin = dateFin;
            }

            newContratInterimaire.Energie = false;
            CIEnt ci = GetCiId(newContratInterimaire, contratPixid.CIImputationContrat, importList);
            newContratInterimaire.CiId = ci?.CiId;
            newContratInterimaire.RessourceId = GetRessourceId(newContratInterimaire, contratPixid.ContratCodeEmploiExterne, importList);
            TextInfo myTI = new CultureInfo("en-US", false).TextInfo;
            newContratInterimaire.Source = myTI.ToTitleCase(Constantes.Pixid.ToLower());
            newContratInterimaire.Qualification = contratPixid.ContratQualification;
            newContratInterimaire.Statut = GetStatutId(contratPixid.ContratCodeStatutInterne, contratPixid.ContratCodeStatutExterne, importList);
            newContratInterimaire.TarifUnitaire = newContratInterimaire.Valorisation = GetTarifUnitaire(contratPixid, importList);
            int souplesse = 0;
            if (int.TryParse(contratPixid.ContratJoursSouplesse, out souplesse))
            {
                newContratInterimaire.Souplesse = souplesse;
            }

            newContratInterimaire.MotifRemplacementId = GetMotifRemplacementId(contratPixid.ContratCodeMotifInterne, contratPixid.ContratCodeMotifExterne, importList, motifRemplacementList);
            newContratInterimaire.UniteId = uniteManager.GetUnite("H").UniteId;
            newContratInterimaire.DateCreation = DateTime.UtcNow;
            newContratInterimaire.FournisseurReferenceExterne = contratPixid.FournisseurSIREN;
            newContratInterimaire.ZonesDeTravail = new List<ZoneDeTravailEnt>();
            if (ci != null && ci.EtablissementComptableId.HasValue)
            {
                newContratInterimaire.ZonesDeTravail.Add(new ZoneDeTravailEnt()
                {
                    EtablissementComptableId = ci.EtablissementComptableId.Value
                });
            }

            var contratInterimaire = contratInterimaireManager.AddContratInterimaire(newContratInterimaire, fredIeUserId);

            contratInterimaireImportManager.AddContratInterimaireImportList(contratInterimaire.ContratInterimaireId, ulong.Parse(contratPixid.Timestamp), importList);
        }

        private int? GetFournisseurId(ContratInterimaireEnt contratInterimaire, string fournisseurSIREN, string groupeCode, List<string> importList)
        {
            var fournisseur = fournisseurManager.GetBySirenAndGroupeCode(fournisseurSIREN, groupeCode);
            if (fournisseur?.Count == 1)
            {
                return fournisseur.First().FournisseurId;
            }

            fournisseur = fournisseurManager.GetByReferenceSystemInterimaireAndGroupeCode(fournisseurSIREN, groupeCode);
            if (fournisseur?.Count == 1)
            {
                return fournisseur.First().FournisseurId;
            }

            importList.Add($"Echec de la reconnaissance du Fournisseur référencé ‘[{fournisseurSIREN}]’.");
            contratInterimaire.EtatContratId = etatContratInterimaireManager.GetEtatContratInterimaireByCode(Fred.Entities.Constantes.EtatContratInterimaire.Blocked).EtatContratInterimaireId; // Bloqué

            return null;
        }

        private int? GetSocieteId(ContratInterimaireEnt contratInterimaire, string siren, string groupeCode, List<string> importList)
        {
            var societe = societeManager.GetSocieteBySirenAndGroupeCode(siren, groupeCode);
            if (societe?.Count == 1)
            {
                return societe.First().SocieteId;
            }

            importList.Add($"Echec de la reconnaissance de la société référencée ‘[{siren}]’.");
            contratInterimaire.EtatContratId = etatContratInterimaireManager.GetEtatContratInterimaireByCode(Fred.Entities.Constantes.EtatContratInterimaire.Blocked).EtatContratInterimaireId; // Bloqué
            return null;
        }

        private void CheckContratInterimaireId(ContratInterimaireEnt contratInterimaire, int personnelId)
        {
            if (personnelId != contratInterimaire.InterimaireId)
            {
                throw new FredIeBusinessException("Le contrat est déjà lié à un autre Personnel.");
            }
        }

        private CIEnt GetCiId(ContratInterimaireEnt contratInterimaire, string cIImputationContrat, List<string> importList)
        {
            if (contratInterimaire.SocieteId.HasValue)
            {
                var ci = ciManager.GetCIByCodeAndSocieteId(cIImputationContrat, contratInterimaire.SocieteId.Value);
                if (ci != null)
                {
                    return ci;
                }

                importList.Add($"Echec de la reconnaissance du CI référencé ‘[{cIImputationContrat}]’.");
                contratInterimaire.EtatContratId = etatContratInterimaireManager.GetEtatContratInterimaireByCode(Fred.Entities.Constantes.EtatContratInterimaire.Blocked).EtatContratInterimaireId; ; // Bloqué
            }
            return null;
        }

        private int? GetRessourceId(ContratInterimaireEnt contratInterimaire, string contratCodeEmploiExterne, List<string> importList)
        {
            var transpo = transpoCodeEmploiToRessourceRepository.GetByCodeSocieteAndCodeEmploi("RZB", contratCodeEmploiExterne);
            if (transpo == null)
            {
                importList.Add($"Echec de la reconnaissance du Code Emploi référencé ‘[{contratCodeEmploiExterne}]’.");
                contratInterimaire.EtatContratId = etatContratInterimaireManager.GetEtatContratInterimaireByCode(Fred.Entities.Constantes.EtatContratInterimaire.Blocked).EtatContratInterimaireId; // Bloqué
                return null;
            }

            var ressource = budgetMainManager.GetRessourceByCodeAndGroupeCode(transpo.CodeRessource, Constantes.GroupeRazelBec);
            if (ressource?.Count == 1)
            {
                return ressource.First().RessourceId;
            }

            importList.Add($"Echec de la reconnaissance du Code Emploi référencé ‘[{contratCodeEmploiExterne}]’.");
            contratInterimaire.EtatContratId = etatContratInterimaireManager.GetEtatContratInterimaireByCode(Fred.Entities.Constantes.EtatContratInterimaire.Blocked).EtatContratInterimaireId; // Bloqué
            return null;
        }

        private decimal GetTarifUnitaire(GetDetailContratOutRecord contratPixid, List<string> importList)
        {
            decimal contratTarif = 0;
            if ((decimal.TryParse(contratPixid.ContratTarif, NumberStyles.Float, CultureInfo.CreateSpecificCulture("en-GB"), out contratTarif) && contratTarif == 0) ||
                    contratPixid.ContratDevise != Constantes.EURO ||
                    contratPixid.ContratPeriode != Constantes.Hourly)
            {
                importList.Add($"Echec de la récupération du Tarif avec les valeurs ‘[{contratPixid.ContratTarif}]’, ‘[{contratPixid.ContratDevise}]’ et ‘[{contratPixid.ContratPeriode}]’.");
                return 0;
            }

            return contratTarif;
        }

        private int? GetStatutId(string contratCodeStatutInterne, string contratCodeStatutExterne, List<string> importList)
        {
            if (!string.IsNullOrEmpty(contratCodeStatutInterne))
            {
                int statut;
                if (int.TryParse(contratCodeStatutInterne, out statut) &&
                    ListStatut.Contains(statut))
                {
                    return statut;
                }
                else
                {
                    importList.Add($"Echec de la reconnaissance du Statut Externe référencé ‘[{contratCodeStatutExterne}]’ car la valeur Interne ‘[{contratCodeStatutInterne}]’ renvoyée par TIBCO est inconnue. ");
                    return null;
                }
            }
            else
            {
                importList.Add($"Echec de la reconnaissance du Statut Externe référencé ‘[{contratCodeStatutExterne}]’.");
                return null;
            }
        }

        private int? GetMotifRemplacementId(string contratCodeMotifInterne, string contratCodeMotifExterne, List<string> importList, List<MotifRemplacementEnt> motifRemplacementList)
        {
            if (!string.IsNullOrEmpty(contratCodeMotifInterne))
            {
                var motif = motifRemplacementList.FirstOrDefault(x => x.Code == contratCodeMotifInterne);
                if (motif != null)
                {
                    return motif.MotifRemplacementId;
                }
                else
                {
                    importList.Add($"Echec de la reconnaissance du Motif Externe référencé ‘[{contratCodeMotifExterne}]’ car la valeur Interne ‘[{contratCodeMotifInterne}]’ renvoyée par TIBCO est inconnue.");
                    return null;
                }
            }
            else
            {
                importList.Add($"Echec de la reconnaissance du Motif Externe référencé ‘[{contratCodeMotifExterne}]’.");
                return null;
            }
        }

        private int? GetSocieteInterimaireByGroupeCode(string groupeCode)
        {
            var groupe = groupeManager.GetGroupeByCode(groupeCode);
            var societe = societeManager.GetSocieteInterim(groupe.GroupeId);
            return societe?.SocieteId;
        }
    }
}
