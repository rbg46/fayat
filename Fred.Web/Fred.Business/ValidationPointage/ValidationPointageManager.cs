using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fred.Business.Rapport;
using Fred.Business.Rapport.Pointage;
using Fred.Business.Utilisateur;
using Fred.Business.ValidationPointage.Controles;
using Fred.Entities;
using Fred.Entities.CI;
using Fred.Entities.Personnel;
using Fred.Entities.Rapport;
using Fred.Entities.ValidationPointage;
using Fred.Framework.Extensions;
using Fred.Web.Shared.App_LocalResources;
using MoreLinq;

namespace Fred.Business.ValidationPointage
{
    public class ValidationPointageManager : IValidationPointageManager
    {
        private readonly IUtilisateurManager userManager;
        private readonly ILotPointageManager lotPointageManager;
        private readonly IControlePointageManager controlePointageManager;
        private readonly IRapportManager rapportManager;
        private readonly IPointageManager pointageManager;
        private readonly IRemonteeVracManager remonteeVracManager;
        private readonly ControleChantier controleChantier;

        public ValidationPointageManager(
            IUtilisateurManager userManager,
            ILotPointageManager lotPointageManager,
            IControlePointageManager controlePointageManager,
            IRapportManager rapportManager,
            IPointageManager pointageManager,
            IRemonteeVracManager remonteeVracManager,
            ControleChantier controleChantier)
        {
            this.userManager = userManager;
            this.lotPointageManager = lotPointageManager;
            this.controlePointageManager = controlePointageManager;
            this.rapportManager = rapportManager;
            this.pointageManager = pointageManager;
            this.remonteeVracManager = remonteeVracManager;
            this.controleChantier = controleChantier;
        }

        /// <inheritdoc />
        public async Task<ControlePointageEnt> ExecuteControleChantierAsync(int lotPointageId, int userId)
        {
            return await controleChantier.ExecuteAsync(lotPointageId, userId);
        }

        /// <summary>
        ///   Récupération de la liste des identifiants des utilisateurs ayant des pointages verrouillés
        /// </summary>
        /// <param name="currentUserId">Identifiant de l'utilisateur connecté</param>
        /// <param name="periode">Période choisie</param>
        /// <returns>Liste des identifiant utilisateur du même périmètre pour la même période</returns>
        private IEnumerable<int> GetAllUserIdInSamePerimeter(int currentUserId, DateTime periode)
        {
            // Rapports compris dans la période et le périmètre de l'utilisateur connecté ET verrouillés par un autre utilisateur
            return this.rapportManager
                   .GetRapportLightList(currentUserId, periode)
                   .Where(x => x.AuteurVerrouId.HasValue && x.AuteurVerrouId.Value != currentUserId)
                   .GroupBy(x => x.AuteurVerrouId)
                   .Select(x => x.Key.Value)
                   .ToList();
        }

        /// <inheritdoc />
        public int CountPointageNonVerrouille(DateTime periode)
        {
            int count = 0;
            int utilisateurId = this.userManager.GetContextUtilisateurId();

            // Rapports compris dans la période et le périmètre de l'utilisateur connecté ET verrouillés par l'utilisateur connecté
            IEnumerable<RapportEnt> unlockedRapports = this.rapportManager
                                                 .GetRapportLightList(utilisateurId, periode)
                                                 .Where(x => !x.AuteurVerrouId.HasValue);

            foreach (RapportEnt r in unlockedRapports.ToList())
            {
                count += this.pointageManager.CountPointage(r.RapportId);
            }

            return count;
        }

        /// <inheritdoc />
        public IEnumerable<LotPointageEnt> GetAllLotPointage(int utilisateurId, DateTime periode)
        {
            var lotPointages = new List<LotPointageEnt>();
            List<int> othersUserIdInPerimeter = GetAllUserIdInSamePerimeter(utilisateurId, periode).ToList();
            othersUserIdInPerimeter.Add(utilisateurId);

            // Ajout des lots de pointages des autres utilisateurs ayant verrouillés des rapports dans la même période et le même périmètre de l'utilisateur connecté
            lotPointages.AddRange(lotPointageManager.GetLotPointageByListUserIdAndPeriode(othersUserIdInPerimeter, periode));

            if (lotPointages.Count > 0)
            {
                IEnumerable<ControlePointageEnt> listControlePointages = this.controlePointageManager.GetLatestList(lotPointages.Select(x => x.LotPointageId).ToList());
                // On met à jour la liste des contrôles pointages avec seulement les derniers contrôle de chaque type (Contrôle Chantier, Contrôle vrac, Remontée vrac)
                lotPointages.ForEach(x => x.ControlePointages = listControlePointages.Where(y => y.LotPointageId == x.LotPointageId).ToList());
            }

            return lotPointages;
        }

        /// <inheritdoc />
        public IEnumerable<LotPointageEnt> GetAllLotPointage(DateTime periode) => GetAllLotPointage(this.userManager.GetContextUtilisateurId(), periode);

        /// <inheritdoc />
        public PointageFiltre GetFilter(int typeControle)
        {
            if (TypeControlePointage.ControleVrac.ToIntValue() == typeControle)
            {
                if (userManager.GetContextUtilisateur().Personnel.Societe.Groupe.Code == Constantes.CodeGroupeFES)
                {
                    return new PointageFiltre
                    {
                        StatutPersonnelList = new List<string>() { Constantes.TypePersonnel.Ouvrier, Constantes.TypePersonnel.ETAM, Constantes.TypePersonnel.Cadre },
                        EtablissementPaieIdList = new List<int>(),
                        TakeMatricule = false,
                        TakeEtablissementPaieIdList = true,
                        TakeSocieteId = true,
                        TakeUpdateAbsence = false
                    };
                }
                return new PointageFiltre { EtablissementPaieIdList = new List<int>(), TakeMatricule = false, TakeEtablissementPaieIdList = true, TakeSocieteId = true, TakeUpdateAbsence = false };
            }
            else
            {
                if (userManager.GetContextUtilisateur().Personnel.Societe.Groupe.Code == Constantes.CodeGroupeFES)
                {
                    return new PointageFiltre
                    {
                        StatutPersonnelList = new List<string>() { Constantes.TypePersonnel.Ouvrier, Constantes.TypePersonnel.ETAM, Constantes.TypePersonnel.Cadre },
                        EtablissementPaieIdList = new List<int>(),
                        TakeMatricule = true,
                        TakeEtablissementPaieIdList = true,
                        TakeSocieteId = true,
                        TakeUpdateAbsence = true
                    };
                }
                return new PointageFiltre { EtablissementPaieIdList = new List<int>(), TakeMatricule = true, TakeEtablissementPaieIdList = true, TakeSocieteId = true, TakeUpdateAbsence = true };
            }
        }

        /// <inheritdoc />
        public SearchValidationResult<ControlePointageErreurEnt> GetAllPersonnelList(int controlePointageId, string searchText, int page, int pageSize)
        {
            var result = new SearchValidationResult<ControlePointageErreurEnt>();
            ControlePointageEnt ctrlPointage = this.controlePointageManager.Get(controlePointageId);
            IEnumerable<PersonnelErreur<ControlePointageErreurEnt>> personnelKO = this.controlePointageManager.GetPersonnelErreurList(controlePointageId);
            LotPointageEnt lotPointage = this.lotPointageManager.Get(ctrlPointage.LotPointageId);
            var personnelOK = new List<PersonnelErreur<ControlePointageErreurEnt>>();
            List<PersonnelErreur<ControlePointageErreurEnt>> tmp = null;

            // Récupération du personnels sans erreur
            if (lotPointage != null && lotPointage.RapportLignes != null)
            {
                foreach (var rp in lotPointage.RapportLignes.Where(x => x.Personnel != null).GroupBy(x => x.PersonnelId))
                {
                    foreach (var p in rp.ToList())
                    {
                        var persoErreur = new PersonnelErreur<ControlePointageErreurEnt>
                        {
                            Personnel = p.Personnel,
                            PersonnelId = p.PersonnelId.Value
                        };

                        if (!personnelKO.Any(x => x.PersonnelId == p.PersonnelId) && !personnelOK.Any(x => x.PersonnelId == p.PersonnelId))
                        {
                            personnelOK.Add(persoErreur);
                        }
                    }
                }
            }

            // Concaténation du personnel OK et KO
            tmp = GetErreurAffaire(personnelKO).Concat(personnelOK).Concat(personnelKO).Where(x => string.IsNullOrEmpty(searchText)
                                                                  || x.Personnel.Matricule.Contains(searchText)
                                                                  || x.Personnel.Nom.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0
                                                                  || x.Personnel.Prenom.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0).ToList();
            // Pagination
            result.TotalPersonnelCount = tmp.Count;
            result.TotalErreurCount = personnelKO.Sum(x => x.Erreurs.Count);
            result.Erreurs = tmp
                             .Skip((page - 1) * pageSize)
                             .Take(pageSize)
                             .OrderBy(x => x.Personnel.Matricule).ThenBy(y => y.Personnel.Prenom).ThenBy(y => y.Personnel.Nom);

            return result;
        }

        /// <inheritdoc/>
        public SearchValidationResult<RemonteeVracErreurEnt> GetRemonteeVracErreurList(int remonteeVracId, string searchText, int page, int pageSize)
        {
            var result = new SearchValidationResult<RemonteeVracErreurEnt>();
            IEnumerable<PersonnelErreur<RemonteeVracErreurEnt>> personnels = this.remonteeVracManager.GetPersonnelErreurList(remonteeVracId)
                                                                             .Where(x => string.IsNullOrEmpty(searchText)
                                                                                         || x.Personnel.Matricule.Contains(searchText)
                                                                                         || x.Personnel.Nom.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0
                                                                                         || x.Personnel.Prenom.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                                                                             .ToList();

            result.TotalPersonnelCount = personnels.Count();
            result.TotalErreurCount = personnels.Sum(x => x.Erreurs.Count);
            result.Erreurs = personnels.Skip((page - 1) * pageSize)
                             .Take(pageSize)
                             .OrderBy(x => x.Personnel.Matricule).ThenBy(y => y.Personnel.Prenom).ThenBy(y => y.Personnel.Nom);

            return result;
        }

        /// <inheritdoc/>
        public byte[] GetControlePointageErreurPdf(int controlePointageId)
        {
            ControlePointageEnt ctrlPointage = this.controlePointageManager.Get(controlePointageId);
            IEnumerable<PersonnelErreur<ControlePointageErreurEnt>> personnelKO = this.controlePointageManager.GetPersonnelErreurList(controlePointageId);
            return ControlePointageErreurExport.ToPdf(personnelKO, ctrlPointage.TypeControle, ctrlPointage.LotPointage.Periode);
        }

        /// <inheritdoc/>
        public string GetControlePointageErreurFilename(int controlePointageId)
        {
            ControlePointageEnt ctrlPointage = this.controlePointageManager.Get(controlePointageId);
            string exportFilename = ctrlPointage.TypeControle == TypeControlePointage.ControleChantier.ToIntValue() ? FeatureValidationPointage.VPManager_ControleChantierExportFilename : FeatureValidationPointage.VPManager_ControleVracExportFilename;
            return string.Format(exportFilename,
                                 ctrlPointage.AuteurCreation.Personnel.Nom,
                                 string.Format("{0:yyyyMMdd}", ctrlPointage.DateDebut),
                                 string.Format("{0:HHmm}", ctrlPointage.DateDebut));
        }

        /// <inheritdoc/>
        public byte[] GetRemonteeVracErreurPdf(int remonteeVracId)
        {
            IEnumerable<PersonnelErreur<RemonteeVracErreurEnt>> personnelKO = this.remonteeVracManager.GetPersonnelErreurList(remonteeVracId);
            return RemonteeVracErreurExport.ToPdf(personnelKO);
        }

        /// <inheritdoc/>
        public string GetRemonteeVracErreurFilename(int remonteeVracId)
        {
            RemonteeVracEnt remonteeVrac = this.remonteeVracManager.Get(remonteeVracId);
            return string.Format(FeatureValidationPointage.VPManager_RemonteeVracExportFilename,
                                 remonteeVrac?.AuteurCreation?.Personnel.Nom,
                                 string.Format("{0:yyyyMMdd}", remonteeVrac?.DateDebut),
                                 string.Format("{0:HHmm}", remonteeVrac?.DateDebut));
        }

        private List<PersonnelErreur<ControlePointageErreurEnt>> GetErreurAffaire(IEnumerable<PersonnelErreur<ControlePointageErreurEnt>> personnelKO)
        {
            List<PersonnelErreur<ControlePointageErreurEnt>> erreursAffaires = new List<PersonnelErreur<ControlePointageErreurEnt>>();
            List<ControlePointageErreurEnt> erreurList = new List<ControlePointageErreurEnt>();

            foreach (var pKO in personnelKO.Where(x => x.Erreurs.Any(a => !a.DateRapport.HasValue && !string.IsNullOrEmpty(a.CodeCi.Trim()))).ToList())
            {
                foreach (var a in pKO.Erreurs.Where(x => !x.DateRapport.HasValue && !string.IsNullOrEmpty(x.CodeCi.Trim())).ToList())
                {
                    var err = new ControlePointageErreurEnt
                    {
                        CodeCi = a.CodeCi.Trim(),
                        ControlePointageErreurId = a.ControlePointageErreurId,
                        ControlePointageId = a.ControlePointageId,
                        ControlePointage = a.ControlePointage,
                        Message = a.Message.Trim()
                    };

                    if (!erreurList.Any(c => c.CodeCi.Trim() == err.CodeCi.Trim() && c.Message.Trim() == err.Message.Trim()))
                    {
                        erreurList.Add(err);
                    }
                }
            }

            erreurList.ForEach(e =>
            {
                var newP = new PersonnelErreur<ControlePointageErreurEnt>
                {
                    Personnel = new PersonnelEnt { Nom = e.CodeCi, Prenom = e.CodeCi, Matricule = string.Empty },
                    PersonnelId = 0,
                    Erreurs = new List<ControlePointageErreurEnt> { e }
                };

                erreursAffaires.Add(newP);
            });

            return erreursAffaires;
        }

        /// <inheritdoc/>
        public List<CIEnt> VerificationCiSep(int? lotPointageId, DateTime? periode, PointageFiltre filtre)
        {
            if (lotPointageId.HasValue)
            {
                LotPointageEnt lotPointage = lotPointageManager.Get(lotPointageId.Value);
                periode = lotPointage.Periode;
            }

            List<RapportLigneEnt> rapportLignes = pointageManager.GetAllLockedPointagesForSocieteOrEtablissementForVerificationCiSep((DateTime)periode, filtre.SocieteId, filtre.EtablissementPaieIdList).ToList();
            List<CIEnt> listeCiSepNonConfigurer = new List<CIEnt>();
            foreach (var rapportLigne in rapportLignes)
            {
                if (rapportLigne.Ci.Societe.TypeSociete.Code == Constantes.TypeSociete.Sep && rapportLigne.Ci.CompteInterneSepId == null)
                {
                    listeCiSepNonConfigurer.Add(rapportLigne.Ci);
                }
            }

            return listeCiSepNonConfigurer.DistinctBy(x => x.CiId).ToList();
        }
    }
}
