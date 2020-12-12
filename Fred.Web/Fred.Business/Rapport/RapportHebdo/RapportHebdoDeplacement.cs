using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Business.Personnel;
using Fred.Business.Referential;
using Fred.Business.Referential.CodeDeplacement;
using Fred.Business.Referential.IndemniteDeplacement.Features;
using Fred.Entities;
using Fred.Entities.CI;
using Fred.Entities.Personnel;
using Fred.Entities.Rapport;
using Fred.Entities.Referential;
using Fred.Entities.Societe;
using Fred.Framework.Tool;
using Fred.Web.Shared.App_LocalResources;
using MoreLinq;
using static Fred.Entities.Constantes;

namespace Fred.Business.Rapport.RapportHebdo
{
    /// <summary>
    /// Permet de calculer les codes et zones de déplacement pour un personnel FES.
    /// </summary>
    public class RapportHebdoDeplacement : ManagersAccess
    {
        private readonly ICodeZoneDeplacementManager codeZoneDeplacementManager;
        private readonly ICodeDeplacementManager codeDeplacementManager;

        #region Members

        private readonly List<PersonnelInfo> personnels;
        private readonly List<CIInfo> cis;
        private readonly List<SocieteInfo> societes;
        private readonly List<CodeEtZoneInfo> codeEtZoneInfos;
        private readonly RapportHebdoHelper helper;

        private bool? lazyUtilisateurConnecteIsGFES;

        #endregion
        #region Constructeurs

        /// <summary>
        /// Constructeur.
        /// </summary>
        public RapportHebdoDeplacement(ICodeMajorationManager codeMajorationManager, ICodeZoneDeplacementManager codeZoneDeplacementManager,IPrimeManager primeManager,ICodeDeplacementManager codeDeplacementManager)
        {
            this.codeZoneDeplacementManager = codeZoneDeplacementManager;
            this.codeDeplacementManager = codeDeplacementManager;
            personnels = new List<PersonnelInfo>();
            cis = new List<CIInfo>();
            societes = new List<SocieteInfo>();
            codeEtZoneInfos = new List<CodeEtZoneInfo>();
            helper = new RapportHebdoHelper(codeMajorationManager, primeManager);
        }

        #endregion
        #region Properties

        /// <summary>
        /// Indique si l'utilisateur connecté est du groupe FES.
        /// </summary>
        public bool UtilisateurConnecteIsGFES
        {
            get
            {
                if (!lazyUtilisateurConnecteIsGFES.HasValue)
                {
                    lazyUtilisateurConnecteIsGFES = Managers.Utilisateur.IsUtilisateurOfGroupe(Constantes.CodeGroupeFES);
                }
                return lazyUtilisateurConnecteIsGFES.Value;
            }
        }

        /// <summary>
        /// Les messages d'alerte.
        /// </summary>
        public List<string> Warnings { get; private set; } = new List<string>();

        #endregion
        #region Public functions

        /// <summary>
        /// Met à jour les codes et les zones de déplacement.
        /// </summary>
        /// <param name="personnelsRapports">Les rapports des personnels à traiter.</param>
        /// <param name="updateSaisieManuelle">Indique si la mise à jour se fait même si la zone a été saisie manuellement.</param>
        /// <remarks>L'utilisateur connecté doit être du groupe FES et les personnels doivent être des ouvriers.</remarks>
        public void UpdateCodeEtZone(PersonnelsRapports personnelsRapports, bool updateSaisieManuelle)
        {
            Warnings = new List<string>();
            if (personnelsRapports == null || !personnelsRapports.Any() || !UtilisateurConnecteIsGFES)
            {
                return;
            }

            foreach (var personnelRapports in personnelsRapports)
            {
                UpdateCodeEtZone(personnelRapports, updateSaisieManuelle);
            }
        }

        /// <summary>
        /// Met à jour les codes et les zones de déplacement.
        /// </summary>
        /// <param name="ligne">La ligne de rapport concernée.</param>
        /// <param name="updateSaisieManuelle">Indique si la mise à jour se fait même si la zone a été saisie manuellement.</param>
        /// <remarks>L'utilisateur connecté doit être du groupe FES et le personnel doit être un ouvrier.</remarks>
        public void UpdateCodeEtZone(RapportLigneEnt ligne, bool updateSaisieManuelle)
        {
            Warnings = new List<string>();
            UpdateCodeEtZone(ligne, updateSaisieManuelle, false);
        }

        /// <summary>
        /// Met à jour le code de déplacement le plus favorable pour les pointages indiqués pour le personnel.
        /// </summary>
        /// <param name="personnelId">L'identifiant du personnel.</param>
        /// <param name="lignes">Les pointages du personnel concerné.</param>
        /// <remarks>L'utilisateur connecté doit être du groupe FES et le personnel doit être un ouvrier.</remarks>
        public void UpdateCodeFavorable(int personnelId, List<RapportLigneEnt> lignes)
        {
            Warnings = new List<string>();
            if (lignes == null || !lignes.Any() || !UtilisateurConnecteIsGFES)
            {
                return;
            }

            // Tous les pointages doivent concerner le personnel
            if (lignes.Any(l => !l.PersonnelId.HasValue || l.PersonnelId.Value != personnelId))
            {
                throw new ArgumentException(FeatureRapportHebdo.RapportHebdoDeplacement_Personnel_Absent, nameof(lignes));
            }

            var personnelInfo = GetPersonnelInfo(lignes[0].Personnel, personnelId);
            if (personnelInfo.IsOuvrier)
            {
                foreach (var pointagesParJour in lignes.GroupBy(l => l.DatePointage))
                {
                    if (pointagesParJour.Any())
                    {
                        var codeDeplacementPlusFavorable = GetCodeDeplacementPlusFavorable(pointagesParJour, true);
                        if (codeDeplacementPlusFavorable != null)
                        {
                            pointagesParJour
                                .Where(l => l.CodeDeplacementId == codeDeplacementPlusFavorable.CodeDeplacementId)
                                .ForEach(l => l.CodeDeplacementPlusFavorable = true);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Indique si un personnel est un ouvrier.
        /// </summary>
        /// <param name="personnelId">L'identifiant du personnel.</param>
        /// <returns>True si le personnel est un ouvrier, sinon false.</returns>
        public bool PersonnelIsOuvrier(int personnelId)
        {
            var personnelInfo = GetPersonnelInfo(null, personnelId);
            return personnelInfo.IsOuvrier;
        }

        /// <summary>
        /// Met à jour le code de déplacement en fonction du code de la zone de déplacement du pointage.
        /// </summary>
        /// <param name="ligne">Le pointage concerné.</param>
        /// <remarks>L'utilisateur connecté doit être du groupe FES et le personnel doit être un ouvrier.</remarks>
        public void UpdateCodeFromZone(RapportLigneEnt ligne)
        {
            Warnings = new List<string>();
            if (ligne == null || !ligne.PersonnelId.HasValue || !UtilisateurConnecteIsGFES)
            {
                return;
            }

            var personnelInfo = GetPersonnelInfo(ligne.Personnel, ligne.PersonnelId.Value);
            if (personnelInfo.IsOuvrier)
            {
                ligne.CodeZoneDeplacementId = null;
                ligne.CodeDeplacement = null;
                ligne.CodeDeplacementId = null;

                if (ligne.CodeZoneDeplacement != null)
                {
                    ligne.CodeZoneDeplacementId = ligne.CodeZoneDeplacement.CodeZoneDeplacementId;
                    ligne.CodeDeplacement = GetCodeIpdFromZone(ligne.CodeZoneDeplacement);
                    if (ligne.CodeDeplacement != null)
                    {
                        ligne.CodeDeplacementId = ligne.CodeDeplacement.CodeDeplacementId;
                    }
                }
            }
        }

        /// <summary>
        /// Indique s'il est possible de calculer les indemnités de déplacement.
        /// </summary>
        /// <param name="ligne">Le pointage concerné.</param>
        /// <returns>True si le calcul est possible, sinon false.</returns>
        public bool CanCalculateIndemniteDeplacement(RapportLigneEnt ligne)
        {
            Warnings = new List<string>();
            UpdateCodeEtZone(ligne, false, true);
            return Warnings.Count == 0;
        }

        #endregion
        #region Private functions

        /// <summary>
        /// Met à jour les codes et les zones de déplacement.
        /// </summary>
        /// <param name="ligne">La ligne de rapport concernée.</param>
        /// <param name="updateSaisieManuelle">Indique si la mise à jour se fait même si la zone a été saisie manuellement.</param>
        /// <param name="onlyCheck">Indique s'il s'agit juste de la vérification si le calcul est possible ou non.</param>
        /// <remarks>L'utilisateur connecté doit être du groupe FES et le personnel doit être un ouvrier.</remarks>
        private void UpdateCodeEtZone(RapportLigneEnt ligne, bool updateSaisieManuelle, bool onlyCheck)
        {
            if (ligne.PersonnelId.HasValue)
            {
                var personnelInfo = GetPersonnelInfo(ligne.Personnel, ligne.Personnel.PersonnelId);
                if (personnelInfo.IsOuvrier)
                {
                    var personnelATravailleCeJour = personnelInfo.ATravaille(ligne);
                    if (onlyCheck && !personnelATravailleCeJour)
                    {
                        return;
                    }

                    CodeEtZoneInfo codeEtZone = null;
                    if (personnelATravailleCeJour)
                    {
                        var societeInfo = personnelInfo.GetSocieteInfo();
                        var ciInfo = GetCIInfo(ligne.Ci, ligne.CiId);
                        codeEtZone = GetCodeEtZone(personnelInfo, ciInfo, societeInfo);
                    }
                    else
                    {
                        Warnings.Add(FeatureRapportHebdo.RapportHebdo_Warning_IPD_PersonnelPasTravaille);
                    }

                    UpdateCodeEtZone(ligne, codeEtZone, updateSaisieManuelle, onlyCheck);
                }
            }
        }

        /// <summary>
        /// Met à jour les codes et les zones de déplacement.
        /// </summary>
        /// <param name="personnelRapports">Les rapports d'un personnel à traiter.</param>
        /// <param name="updateSaisieManuelle">Indique si la mise à jour se fait même si la zone a été saisie manuellement.</param>
        /// <remarks>L'utilisateur connecté doit être du groupe FES et les personnels doivent être des ouvriers.</remarks>
        private void UpdateCodeEtZone(PersonnelRapports personnelRapports, bool updateSaisieManuelle)
        {
            var personnelInfo = GetPersonnelInfo(null, personnelRapports.PersonnelId);
            if (personnelInfo.IsOuvrier)
            {
                foreach (var rapport in personnelRapports.Rapports)
                {
                    var lignes = personnelInfo.GetRapportLignes(rapport);
                    if (lignes.Any())
                    {
                        // Ici le personnel est présent sur au moins une ligne du rapport
                        CodeEtZoneInfo codeEtZone = null;
                        if (personnelInfo.ATravaille(lignes))
                        {
                            // Ici le personnel a travaillé sur au moins une ligne du rapport
                            var societeInfo = personnelInfo.GetSocieteInfo();
                            var ciInfo = GetCIInfo(rapport.CI, rapport.CiId);
                            codeEtZone = GetCodeEtZone(personnelInfo, ciInfo, societeInfo);
                        }

                        foreach (var ligne in lignes)
                        {
                            UpdateCodeEtZone(ligne, codeEtZone, updateSaisieManuelle, false);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Met à jour les codes et les zones de déplacement.
        /// </summary>
        /// <param name="ligne">La ligne de pointage concernée.</param>
        /// <param name="codeEtZone">Les codes et les zones de déplacement.</param>
        /// <param name="updateSaisieManuelle">Indique si la mise à jour se fait même si la zone a été saisie manuellement.</param>
        /// <param name="onlyCheck">Indique s'il s'agit juste de la vérification si le calcul est possible ou non.</param>
        private void UpdateCodeEtZone(RapportLigneEnt ligne, CodeEtZoneInfo codeEtZone, bool updateSaisieManuelle, bool onlyCheck)
        {
            if (!ligne.CodeZoneDeplacementSaisiManuellement || updateSaisieManuelle)
            {
                if (!onlyCheck)
                {
                    ligne.CodeZoneDeplacement = null;
                    ligne.CodeZoneDeplacementId = null;
                    ligne.CodeDeplacement = null;
                    ligne.CodeDeplacementId = null;
                    ligne.CodeZoneDeplacementSaisiManuellement = false;
                }

                // RG-004
                // -> L'IPD ne doit pas être calculée si une majoration THRA est sélectionnée
                // En fait, l'IPD ne doit pas être calculée si une prime GDI ou GDP existe
                //  De plus, une prime GDI ou GDP doit exister si une majoration THRA existe
                if (helper.HasPrime(ligne, CodePrime.GDI) || helper.HasPrime(ligne, CodePrime.GDP))
                {
                    return;
                }

                // RG-003A
                // -> Si une majoration THRR est pointée il faut affecter la zone IPD la plus grande.
                if (helper.HasMajoration(ligne, CodesMajoration.THRR))
                {
                    SetCodeEtZonePlusFavorable(codeEtZone.SocieteInfo, ligne, onlyCheck);
                }
                else if (codeEtZone?.Zone != null && codeEtZone.PersonnelInfo.ATravaille(ligne))
                {
                    SetCodeEtZone(codeEtZone, ligne, onlyCheck);
                }
            }
        }

        /// <summary>
        /// Met le code et la zone la plus favorable dans le pointage indiqué.
        /// </summary>
        /// <param name="societe">Les informations sur la société.</param>
        /// <param name="ligne">Le pointage concerné.</param>
        /// <param name="onlyCheck">Indique s'il s'agit juste de la vérification si le calcul est possible ou non.</param>
        private void SetCodeEtZonePlusFavorable(SocieteInfo societe, RapportLigneEnt ligne, bool onlyCheck)
        {
            if (onlyCheck)
            {
                societe.CheckCodeEtZoneDeplacementPlusFavorable();
            }
            else
            {
                var zone = societe.CodeZoneDeplacementPlusFavorable;
                var code = societe.CodeDeplacementPlusFavorable;
                ligne.CodeZoneDeplacement = zone;
                ligne.CodeZoneDeplacementId = zone?.CodeZoneDeplacementId;
                ligne.CodeDeplacement = code;
                ligne.CodeDeplacementId = code?.CodeDeplacementId;
            }
        }

        /// <summary>
        /// Met le code et la zone indiqué dans le pointage indiqué.
        /// </summary>
        /// <param name="codeEtZone">Les informations sur le code et la zone.</param>
        /// <param name="ligne">Le pointage concerné.</param>
        /// <param name="onlyCheck">Indique s'il s'agit juste de la vérification si le calcul est possible ou non.</param>
        private void SetCodeEtZone(CodeEtZoneInfo codeEtZone, RapportLigneEnt ligne, bool onlyCheck)
        {
            if (onlyCheck)
            {
                codeEtZone.Check();
            }
            else
            {
                ligne.CodeZoneDeplacement = codeEtZone.Zone;
                ligne.CodeZoneDeplacementId = codeEtZone.Zone.CodeZoneDeplacementId;
                ligne.CodeDeplacement = codeEtZone.Code;
                ligne.CodeDeplacementId = codeEtZone.Code?.CodeDeplacementId;
            }
        }

        /// <summary>
        /// Retourne les informations sur un personnel.
        /// </summary>
        /// <param name="personnel">Le personnel, peut être null .</param>
        /// <param name="personnelId">L'identifiant du personnel, utilisé si le paramètre "personnel" est null.</param>
        /// <returns>Les informations sur le personnel.</returns>
        private PersonnelInfo GetPersonnelInfo(PersonnelEnt personnel, int personnelId)
        {
            var ret = personnels.FirstOrDefault(i => i.PersonnelId == personnelId);
            if (ret == null)
            {
                ret = new PersonnelInfo(personnel, personnelId, this);
                personnels.Add(ret);
            }
            return ret;
        }

        /// <summary>
        /// Retourne les informations sur une société.
        /// </summary>
        /// <param name="societe">La société.</param>
        /// <returns>Les informations sur la société.</returns>
        private SocieteInfo GetSocieteInfo(SocieteEnt societe)
        {
            var ret = societes.FirstOrDefault(i => i.SocieteId == societe.SocieteId);
            if (ret == null)
            {
                ret = new SocieteInfo(societe, this);
                societes.Add(ret);
            }
            return ret;
        }

        /// <summary>
        /// Retourne les informations sur un CI.
        /// </summary>
        /// <param name="ci">Le CI, peut être null.</param>
        /// <param name="ciId">L'identifiant du CI, utilisé si le paramètre "ci" est null.</param>
        /// <returns>Les informations sur le CI.</returns>
        private CIInfo GetCIInfo(CIEnt ci, int ciId)
        {
            var ret = cis.FirstOrDefault(i => i.CIId == ciId);
            if (ret == null)
            {
                ret = new CIInfo(ci, ciId, this);
                cis.Add(ret);
            }
            return ret;
        }

        /// <summary>
        /// Retourne les informations sur les codes et les zones de déplacement.
        /// </summary>
        /// <param name="personnelInfo">Les informations sur un personnel.</param>
        /// <param name="ciInfo">Les informations sur un CI.</param>
        /// <param name="societeInfo">Les informations sur une société.</param>
        /// <returns>Les informations sur les codes et les zones de déplacement.</returns>
        private CodeEtZoneInfo GetCodeEtZone(PersonnelInfo personnelInfo, CIInfo ciInfo, SocieteInfo societeInfo)
        {
            var ret = codeEtZoneInfos.FirstOrDefault(i => i.PersonnelInfo.PersonnelId == personnelInfo.PersonnelId && i.CIInfo.CIId == ciInfo.CIId);
            if (ret == null)
            {
                ret = new CodeEtZoneInfo(personnelInfo, ciInfo, societeInfo, this);
                codeEtZoneInfos.Add(ret);
            }
            return ret;
        }

        /// <summary>
        /// Retourne le code de déplacement le plus favorable.
        /// </summary>
        /// <param name="lignes">Lignes de rapport concernées.</param>
        /// <param name="nullIfOnlyOneDistinctCodeExists">Retournera null si uniquement un seul code de déplacement existe dans les pointages.</param>
        /// <returns>Le code de déplacement le plus favorable ou null si pas de déplacement / un seul code de déplacement distinct existe (en fonction du paramètre nullIfOnlyOneCodeExists).</returns>
        private static CodeDeplacementEnt GetCodeDeplacementPlusFavorable(IEnumerable<RapportLigneEnt> lignes, bool nullIfOnlyOneDistinctCodeExists)
        {
            var codeDeplacements = lignes
                .Where(l => l.CodeDeplacement != null)
                .Select(l => l.CodeDeplacement)
                .DistinctBy(cd => cd.CodeDeplacementId)
                .OrderByDescending(l => l.KmMini);

            if (nullIfOnlyOneDistinctCodeExists)
            {
                return codeDeplacements.Count() > 1 ? codeDeplacements.First() : null;
            }
            return codeDeplacements.FirstOrDefault();
        }

        /// <summary>
        /// Retourne le code déplacement IPD correspondant à la zone indiquée.
        /// </summary>
        /// <param name="codeZoneDeplacement">La zone concernée.</param>
        /// <returns>Le code déplacement IPD correspondant.</returns>
        private CodeDeplacementEnt GetCodeIpdFromZone(CodeZoneDeplacementEnt codeZoneDeplacement)
        {
            var codeDeplacements = codeDeplacementManager.GetCodeDeplacementList(codeZoneDeplacement.SocieteId, true);
            return codeDeplacements.FirstOrDefault(cd => !cd.IGD && cd.KmMini == codeZoneDeplacement.KmMini && cd.KmMaxi == codeZoneDeplacement.KmMaxi);
        }

        #endregion
        #region Classes

        /// <summary>
        /// Représente les informations d'un personnel.
        /// </summary>
        public class PersonnelInfo : ManagersAccess
        {
            private readonly RapportHebdoDeplacement rhd;
            private PersonnelEnt lazyPersonnel;
            private bool? lazyIsOuvrier;
            private GeographicCoordinate lazyRattachementGeographicCoordinate;
            private bool rattachementGeographicCoordinateCalculated;
            private SocieteInfo lazySocieteInfo;

            /// <summary>
            /// Constructeur.
            /// </summary>
            /// <param name="personnel">Le personnel, peut être null.</param>
            /// <param name="personnelId">L'identifiant du personnel, utilisé si le paramètre "personnel" est null.</param>
            /// <param name="rhd">Le <see cref="RapportHebdoDeplacement"/> parent.</param>
            public PersonnelInfo(PersonnelEnt personnel, int personnelId, RapportHebdoDeplacement rhd)
            {
                lazyPersonnel = personnel;
                PersonnelId = personnelId;
                this.rhd = rhd;
            }

            /// <summary>
            /// Identifiant du personnel.
            /// </summary>
            public int PersonnelId { get; private set; }

            /// <summary>
            /// Le personnel.
            /// </summary>
            private PersonnelEnt Personnel
            {
                get
                {
                    if (lazyPersonnel == null)
                    {
                        lazyPersonnel = Managers.Personnel.GetPersonnel(PersonnelId);
                    }
                    return lazyPersonnel;
                }
            }

            /// <summary>
            /// Indique si le personnel est un ouvrier.
            /// </summary>
            public bool IsOuvrier
            {
                get
                {
                    if (!lazyIsOuvrier.HasValue)
                    {
                        lazyIsOuvrier = PersonnelManager.GetStatut(Personnel.Statut) == Constantes.PersonnelStatutValue.Ouvrier;
                    }
                    return lazyIsOuvrier.Value;
                }
            }

            /// <summary>
            /// Retourne les lignes d'un rapport où le personnel est pointé.
            /// </summary>
            /// <param name="rapport">Le rapport concerné.</param>
            /// <returns>Les lignes du rapport où le personnel est pointé.</returns>
            public IEnumerable<RapportLigneEnt> GetRapportLignes(RapportEnt rapport)
            {
                foreach (var ligne in rapport.ListLignes)
                {
                    if (ligne.DateSuppression == null && ligne.PersonnelId == PersonnelId)
                    {
                        yield return ligne;
                    }
                }
            }

            /// <summary>
            /// Indique si le personnel a travaillé.
            /// </summary>
            /// <param name="lignes">Les lignes de rapport concernées.</param>
            /// <returns>True si le personnel a travaillé, sinon false.</returns>
            public bool ATravaille(IEnumerable<RapportLigneEnt> lignes)
            {
                return lignes.Any(p => ATravaille(p));
            }

            /// <summary>
            /// Indique si le personnel a travaillé.
            /// </summary>
            /// <param name="ligne">La ligne de pointage concerné.</param>
            /// <returns>True si le personnel a travaillé, sinon false.</returns>
            public bool ATravaille(RapportLigneEnt ligne)
            {
                if (ligne.DateSuppression != null || ligne.PersonnelId != PersonnelId)
                {
                    return false;
                }

                if (ligne.ListRapportLigneTaches != null && ligne.ListRapportLigneTaches.Any(t => t.HeureTache > 0))
                {
                    return true;
                }

                if (ligne.ListRapportLigneMajorations != null && ligne.ListRapportLigneMajorations.Any(m => m.HeureMajoration > 0))
                {
                    return true;
                }

                if (ligne.ListRapportLigneAstreintes != null && ligne.ListRapportLigneAstreintes.Any())
                {
                    return true;
                }

                return false;
            }

            /// <summary>
            /// Retourne la coordonnée géographique du lieu de rattachement du personnel.
            /// </summary>
            /// <returns>La coordonnée géographique du lieu de rattachement du personnel, peut être null.</returns>
            public GeographicCoordinate GetRattachementGeographicCoordinate()
            {
                if (!rattachementGeographicCoordinateCalculated)
                {
                    lazyRattachementGeographicCoordinate = IndemniteDeplacementCalculator.GetOrigine(Personnel);
                    if (lazyRattachementGeographicCoordinate == null)
                    {
                        rhd.Warnings.Add(FeatureRapportHebdo.RapportHebdo_Warning_IPD_Rattachement);
                    }
                    rattachementGeographicCoordinateCalculated = true;
                }
                return lazyRattachementGeographicCoordinate;
            }

            /// <summary>
            /// Retourne les informations sur la société du personnel.
            /// </summary>
            /// <returns>Les informations sur la société du personnel.</returns>
            public SocieteInfo GetSocieteInfo()
            {
                if (lazySocieteInfo == null)
                {
                    lazySocieteInfo = rhd.GetSocieteInfo(Personnel.Societe);
                }
                return lazySocieteInfo;
            }
        }

        /// <summary>
        /// Représente les informations d'un CI.
        /// </summary>
        public class CIInfo : ManagersAccess
        {
            private readonly RapportHebdoDeplacement rhd;
            private CIEnt lazyCI;
            private GeographicCoordinate lazyGeographicCoordinate;
            private bool geographicCoordinateCalculated;

            /// <summary>
            /// Constructeur.
            /// </summary>
            /// <param name="ci">Le CI, peut être null.</param>
            /// <param name="ciId">L'identifiant du CI, utilisé si le paramètre "ci" est null.</param>
            /// <param name="rhd">Le <see cref="RapportHebdoDeplacement"/> parent.</param>
            public CIInfo(CIEnt ci, int ciId, RapportHebdoDeplacement rhd)
            {
                lazyCI = ci;
                CIId = ciId;
                this.rhd = rhd;
            }

            /// <summary>
            /// Identifiant du CI.
            /// </summary>
            public int CIId { get; private set; }

            /// <summary>
            /// Le CI.
            /// </summary>
            private CIEnt CI
            {
                get
                {
                    if (lazyCI == null)
                    {
                        lazyCI = Managers.CI.FindById(CIId);
                    }
                    return lazyCI;
                }
            }

            /// <summary>
            /// Retourne la coordonnée géographique du CI.
            /// </summary>
            /// <returns>La coordonnée géographique du CI, peut être null.</returns>
            public GeographicCoordinate GetGeographicCoordinate()
            {
                if (!geographicCoordinateCalculated)
                {
                    lazyGeographicCoordinate = IndemniteDeplacementCalculator.GetGeographicCoordinate(CI);
                    if (lazyGeographicCoordinate == null)
                    {
                        rhd.Warnings.Add(string.Format(FeatureRapportHebdo.RapportHebdo_Warning_IPD_CI, CI.Code));
                    }
                    geographicCoordinateCalculated = true;
                }
                return lazyGeographicCoordinate;
            }
        }

        /// <summary>
        /// Représente les informations d'une société.
        /// </summary>
        public class SocieteInfo : ManagersAccess
        {
            private readonly SocieteEnt societe;
            private readonly RapportHebdoDeplacement rhd;
            private bool calculTypeChecked;
            private CodeZoneDeplacementEnt lazyCodeZoneDeplacementPlusFavorable;
            private bool codeZoneDeplacementPlusFavorableCalculated;
            private CodeDeplacementEnt lazyCodeDeplacementPlusFavorable;
            private bool codeDeplacementPlusFavorableCalculated;

            /// <summary>
            /// Constructeur.
            /// </summary>
            /// <param name="societe">La société, peut être null.</param>
            /// <param name="rhd">Le <see cref="RapportHebdoDeplacement"/> parent.</param>
            public SocieteInfo(SocieteEnt societe, RapportHebdoDeplacement rhd)
            {
                this.societe = societe;
                this.rhd = rhd;
            }

            /// <summary>
            /// L'identifiant de la société.
            /// </summary>
            public int SocieteId { get { return societe.SocieteId; } }

            /// <summary>
            /// Le code zone de déplacement le plus favorable pour cette société.
            /// </summary>
            public CodeZoneDeplacementEnt CodeZoneDeplacementPlusFavorable
            {
                get
                {
                    if (!codeZoneDeplacementPlusFavorableCalculated)
                    {
                        var zones = rhd.codeZoneDeplacementManager.GetCodeZoneDeplacementBySocieteId(SocieteId, true);
                        lazyCodeZoneDeplacementPlusFavorable = zones.MaxBy(z => z.KmMaxi).FirstOrDefault();
                        codeZoneDeplacementPlusFavorableCalculated = true;
                    }
                    return lazyCodeZoneDeplacementPlusFavorable;
                }
            }

            /// <summary>
            /// Le code de déplacement le plus favorable pour cette société.
            /// </summary>
            public CodeDeplacementEnt CodeDeplacementPlusFavorable
            {
                get
                {
                    if (!codeDeplacementPlusFavorableCalculated)
                    {
                        lazyCodeDeplacementPlusFavorable = rhd.GetCodeIpdFromZone(CodeZoneDeplacementPlusFavorable);
                        codeDeplacementPlusFavorableCalculated = true;
                    }
                    return lazyCodeDeplacementPlusFavorable;
                }
            }

            /// <summary>
            /// Retourne le code zone de déplacement.
            /// </summary>
            /// <param name="personnelInfo">Les informations sur le personnel.</param>
            /// <param name="ciInfo">Les informations sur le CI.</param>
            /// <returns>Le code zone de déplacement.</returns>
            public CodeZoneDeplacementEnt GetCodeZoneDeplacement(PersonnelInfo personnelInfo, CIInfo ciInfo)
            {
                CodeZoneDeplacementEnt zone = null;
                var origine = personnelInfo.GetRattachementGeographicCoordinate();
                var destination = ciInfo.GetGeographicCoordinate();

                if (!calculTypeChecked)
                {
                    if (societe.IndemniteDeplacementCalculTypeId == null)
                    {
                        rhd.Warnings.Add(FeatureRapportHebdo.RapportHebdo_Warning_IPD_SocieteCalculType);
                    }
                    calculTypeChecked = true;
                }

                if (origine != null && destination != null && societe.IndemniteDeplacementCalculTypeId != null)
                {
                    zone = new IndemniteDeplacementCalculator(rhd.codeZoneDeplacementManager).GetZone(societe, origine, destination);
                    if (zone == null)
                    {
                        rhd.Warnings.Add(FeatureRapportHebdo.RapportHebdo_Warning_IPD_Zone);
                    }
                }

                return zone;
            }

            /// <summary>
            /// Indique si les codes et zones sont bien configurés.
            /// </summary>
            /// <returns>True si les codes et zones sont bien configurés, sinon false.</returns>
            public bool CheckCodeEtZoneDeplacementPlusFavorable()
            {
                return CodeZoneDeplacementPlusFavorable != null && CodeDeplacementPlusFavorable != null;
            }
        }

        /// <summary>
        /// Représente un code et une zone de déplacement.
        /// </summary>
        public class CodeEtZoneInfo : ManagersAccess
        {
            private readonly RapportHebdoDeplacement rhd;
            private bool zoneCalculated;
            private CodeZoneDeplacementEnt lazyZone;
            private bool codeCalculated;
            private CodeDeplacementEnt lazyCode;

            /// <summary>
            /// Constructeur.
            /// </summary>
            /// <param name="personnelInfo">Les informations sur le personnel.</param>
            /// <param name="ciInfo">Les informations sur le CI.</param>
            /// <param name="societeInfo">Les informations sur la société.</param>
            /// <param name="rhd">Le <see cref="RapportHebdoDeplacement"/> parent.</param>
            public CodeEtZoneInfo(PersonnelInfo personnelInfo, CIInfo ciInfo, SocieteInfo societeInfo, RapportHebdoDeplacement rhd)
            {
                PersonnelInfo = personnelInfo;
                CIInfo = ciInfo;
                SocieteInfo = societeInfo;
                this.rhd = rhd;
            }

            /// <summary>
            /// Les informations sur le personnel.
            /// </summary>
            public PersonnelInfo PersonnelInfo { get; private set; }

            /// <summary>
            /// Les informations sur le CI.
            /// </summary>
            public CIInfo CIInfo { get; private set; }

            /// <summary>
            /// Les informations sur la société.
            /// </summary>
            public SocieteInfo SocieteInfo { get; private set; }

            /// <summary>
            /// Le code zone de déplacement.
            /// </summary>
            public CodeZoneDeplacementEnt Zone
            {
                get
                {
                    if (!zoneCalculated)
                    {
                        lazyZone = SocieteInfo.GetCodeZoneDeplacement(PersonnelInfo, CIInfo);
                        zoneCalculated = true;
                    }
                    return lazyZone;
                }
            }

            /// <summary>
            /// Le code de déplacement.
            /// </summary>
            public CodeDeplacementEnt Code
            {
                get
                {
                    if (!codeCalculated)
                    {
                        var zone = Zone;        // Evite les appels récurrents au lazy Zone
                        if (zone != null)
                        {
                            lazyCode = rhd.GetCodeIpdFromZone(zone);
                            if (lazyCode == null)
                            {
                                rhd.Warnings.Add(string.Format(FeatureRapportHebdo.RapportHebdo_Warning_IPD_CodeDeplacement, zone.Code));
                            }
                        }
                        codeCalculated = true;
                    }
                    return lazyCode;
                }
            }

            /// <summary>
            /// Indique si les codes et zones sont bien configurés.
            /// </summary>
            /// <returns>True si les codes et zones sont bien configurés, sinon false.</returns>
            public bool Check()
            {
                return Zone != null && Code != null;
            }
        }

        #endregion
    }
}
