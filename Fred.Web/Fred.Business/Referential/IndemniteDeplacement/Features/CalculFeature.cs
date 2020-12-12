using System;
using System.Collections.Generic;
using Fred.Business.CI;
using Fred.Business.Personnel;
using Fred.Business.Referential;
using Fred.Business.Referential.CodeDeplacement;
using Fred.Business.Referential.IndemniteDeplacement;
using Fred.Business.Referential.IndemniteDeplacement.Features;
using Fred.Business.Referential.IndemniteDeplacement.Strategie;
using Fred.Business.Referential.TypeRattachement;
using Fred.DataAccess.Interfaces;
using Fred.Entities;
using Fred.Entities.CI;
using Fred.Entities.IndemniteDeplacement;
using Fred.Entities.Personnel;
using Fred.Entities.Rapport;
using Fred.Entities.Referential;
using Fred.Framework.Exceptions;
using Fred.Web.Shared.App_LocalResources;

namespace Fred.Business.IndemniteDeplacement
{
    /// <inheritdoc />
    public class CalculFeature : ManagerFeature<IIndemniteDeplacementRepository>, ICalculFeature
    {
        #region Champs

        private readonly ICodeZoneDeplacementManager codeZoneMgr;
        private readonly IPersonnelManager personnelManager;
        private readonly ICIManager cIManager;
        private readonly ICodeDeplacementManager codeDepMgr;

        #endregion
        #region Constructeur

        /// <summary>
        /// Instancie un nouvel objet CalculFeature
        /// </summary>
        /// <param name="repository">repo des indemnités de déplacment</param>
        /// <param name="codeZoneDeplacementManager">Manager des codes zones</param>
        /// <param name="personnelManager">Manager du personnel</param>
        /// <param name="cIManager">Manager des Cis</param>
        /// <param name="codeDeplacementManager">Manager des codes de déplacement</param>
        /// <param name="uow">Unit of work</param>
        public CalculFeature(IIndemniteDeplacementRepository repository,
                             ICodeZoneDeplacementManager codeZoneDeplacementManager,
                             IPersonnelManager personnelManager,
                             ICIManager cIManager,
                             ICodeDeplacementManager codeDeplacementManager,
                             IUnitOfWork uow)
              : base(uow, repository)
        {
            codeZoneMgr = codeZoneDeplacementManager;
            this.personnelManager = personnelManager;
            this.cIManager = cIManager;
            codeDepMgr = codeDeplacementManager;
        }

        #endregion
        #region Fonctions publiques

        /// <inheritdoc />
        public CodeZoneDeplacementEnt GetZoneByDistance(int societeId, double distanceInKM)
        {
            // Déclaration de la zone de retour
            CodeZoneDeplacementEnt zone = null;

            if (distanceInKM >= 0 && distanceInKM <= 5)
            {
                zone = codeZoneMgr.GetZoneBySocieteIdAndCode(societeId, IndemniteDeplacementConst.ConstCodeZone1A);
            }
            else if (distanceInKM > 5 && distanceInKM <= 10)
            {
                zone = codeZoneMgr.GetZoneBySocieteIdAndCode(societeId, IndemniteDeplacementConst.ConstCodeZone1B);
            }
            else if (distanceInKM > 10 && distanceInKM <= 20)
            {
                zone = codeZoneMgr.GetZoneBySocieteIdAndCode(societeId, IndemniteDeplacementConst.ConstCodeZone2);
            }
            else if (distanceInKM > 20 && distanceInKM <= 30)
            {
                zone = codeZoneMgr.GetZoneBySocieteIdAndCode(societeId, IndemniteDeplacementConst.ConstCodeZone3);
            }
            else if (distanceInKM > 30 && distanceInKM <= 40)
            {
                zone = codeZoneMgr.GetZoneBySocieteIdAndCode(societeId, IndemniteDeplacementConst.ConstCodeZone4);
            }
            else if (distanceInKM > 40 && distanceInKM <= 50)
            {
                zone = codeZoneMgr.GetZoneBySocieteIdAndCode(societeId, IndemniteDeplacementConst.ConstCodeZone5);
            }

            return zone;
        }

        /// <inheritdoc />
        public IndemniteDeplacementEnt CalculIndemniteDeplacement(PersonnelEnt personnel, CIEnt ci)
        {
            // Vérification des paramètres d'entrée
            if (personnel == null)
            {
                throw new ArgumentNullException(nameof(personnel));
            }

            if (ci == null)
            {
                throw new ArgumentNullException(nameof(ci));
            }

            // Déclaration de l'indemnité de retour
            var personnelDataBase = personnelManager.GetPersonnel(personnel.PersonnelId);
            personnel.EtablissementRattachement = personnelDataBase.EtablissementRattachement ?? personnelDataBase.EtablissementPaie;
            personnel.EtablissementRattachementId = personnelDataBase.EtablissementRattachementId ?? personnelDataBase.EtablissementPaieId;
            personnel.EtablissementPaie = personnelDataBase.EtablissementPaie;
            personnel.EtablissementPaieId = personnelDataBase.EtablissementPaieId;
            personnel.Societe = personnelDataBase.Societe;
            // Vérification de présence des dépendances
            if (personnel.EtablissementRattachement == null && personnel.EtablissementPaie == null)
            {
                return null;
            }

            // Vérification de présence de l'identifiant société d'appartenance
            if (personnel.Societe == null)
            {
                return null;
            }

            // Si l'établissement de rattachement du personnel ne gère pas les règles de déplacement RAZEL-BEC : on ne lance pas le calcul
            if (!personnel.EtablissementRattachement.GestionIndemnites)
            {
                return null;
            }

            var indemnite = new IndemniteDeplacementEnt();
            indemnite.Personnel = personnel;
            indemnite.PersonnelId = personnel.PersonnelId;

            var ciDatabase = cIManager.GetCIById(ci.CiId);
            indemnite.CI = ciDatabase;
            indemnite.CiId = ciDatabase.CiId;

            // Calcul des distances kilométriques relatives à l'indemnité
            indemnite = CalculKm(indemnite);

            // Calcul le code et la zone de déplacement
            CalculCodeEtZoneDeplacement(indemnite);

            indemnite.DateDernierCalcul = DateTime.UtcNow;

            return indemnite;
        }

        /// <inheritdoc />
        public IndemniteDeplacementEnt CalculIndemniteDeplacementGenerationPrevisionnelle(RapportLigneEnt dernierPointageReel, DateTime datePointagePrevisionnel)
        {
            // Vérification des paramètres d'entrée
            if (dernierPointageReel == null)
            {
                throw new ArgumentNullException(nameof(dernierPointageReel));
            }

            // Récupération des informations du dernier pointage
            var ci = dernierPointageReel.Ci;
            var personnel = dernierPointageReel.Personnel;

            if (personnel == null)
            {
                throw new FredBusinessException("Personnel non renseigné.");
            }

            if (ci == null)
            {
                throw new FredBusinessException("Ci non renseigné.");
            }

            // Déclaration de l'indemnité de retour
            var indemnite = new IndemniteDeplacementEnt();

            // Vérification de présence des dépendances
            if (personnel.EtablissementRattachement == null)
            {
                return indemnite;
            }

            // Vérification de présence de l'identifiant société d'appartenance
            if (!personnel.SocieteId.HasValue)
            {
                return indemnite;
            }

            // Si l'établissement de rattachement du personnel ne gère pas les règles de déplacement RAZEL-BEC : on ne lance pas le calcul
            if (!personnel.EtablissementRattachement.GestionIndemnites)
            {
                return indemnite;
            }

            // Initialisation de l'indemnite de retour
            indemnite = new IndemniteDeplacementEnt();
            indemnite.Personnel = personnel;
            indemnite.PersonnelId = personnel.PersonnelId;
            indemnite.CI = ci;
            indemnite.CiId = ci.CiId;

            // Calcul le code et la zone de déplacement
            CalculCodeEtZoneDeplacementGenerationPrevisionnelle(indemnite, dernierPointageReel, datePointagePrevisionnel);

            return indemnite;
        }

        /// <inheritdoc />
        public IndemniteDeplacementEnt GetCalculIndemniteDeplacement(IndemniteDeplacementEnt indemniteDeplacementCalcul)
        {
            var indDep = CalculIndemniteDeplacement(indemniteDeplacementCalcul.Personnel, indemniteDeplacementCalcul.CI);

            if (indDep != null)
            {
                indDep.IndemniteDeplacementId = indemniteDeplacementCalcul.IndemniteDeplacementId;
                indDep.AuteurCreation = indemniteDeplacementCalcul.AuteurCreation;
                indDep.DateCreation = indemniteDeplacementCalcul.DateCreation;
            }

            return indDep;
        }

        /// <summary>
        /// Indique s'il est possible de calculer les indemnités de déplacement.
        /// </summary>
        /// <param name="personnel">Le personnel concerné.</param>
        /// <param name="ci">Le Ci concerné.</param>
        /// <returns>True s'il est possible de faire le calcul, sinon false et la liste d'erreurs</returns>
        public Tuple<bool, List<string>> CanCalculateIndemniteDeplacement(PersonnelEnt personnel, CIEnt ci)
        {
            Tuple<bool, List<string>> controle = new Tuple<bool, List<string>>(false, new List<string>());

            if (personnel == null)
            {
                return controle;
            }
            if (ci == null)
            {
                return controle;
            }

            Action<string> addMessage = x => { if (!string.IsNullOrEmpty(x)) { controle.Item2.Add(x); } };

            var chantier = IndemniteDeplacementCalculator.GetGeographicCoordinate(ci);
            if (chantier == null)
            {
                addMessage(FeatureIndemniteDeplacement.IndemniteDeplacement_Warning_No_GPS_Ci);
            }

            switch (personnel.TypeRattachement)
            {
                case TypeRattachement.Agence:
                    addMessage(GetMessageControleAgence(personnel.EtablissementPaie));
                    break;
                case TypeRattachement.Secteur:
                    addMessage(GetMessageControleSecteur(personnel.EtablissementRattachement));
                    break;
                default: //domicile
                    addMessage(GetMessageControlePersonnel(personnel));
                    break;
            }

            return controle;
        }

        private string GetMessageControlePersonnel(PersonnelEnt personnel)
        {
            return IndemniteDeplacementCalculator.GetGeographicCoordinate(personnel) == null ?
                 FeatureIndemniteDeplacement.IndemniteDeplacement_Warning_No_GPS_Personnel :
                 string.Empty;
        }

        private string GetMessageControleSecteur(EtablissementPaieEnt secteur)
        {
            return secteur == null ?
                FeatureIndemniteDeplacement.IndemniteDeplacement_Warning_No_Secteur :
                IndemniteDeplacementCalculator.GetGeographicCoordinate(secteur) == null ?
                    FeatureIndemniteDeplacement.IndemniteDeplacement_Warning_No_GPS_Secteur :
                    string.Empty;
        }

        private string GetMessageControleAgence(EtablissementPaieEnt agence)
        {
            return agence == null ?
                 FeatureIndemniteDeplacement.IndemniteDeplacement_Warning_No_Agence :
                 IndemniteDeplacementCalculator.GetGeographicCoordinate(agence) == null ?
                    FeatureIndemniteDeplacement.IndemniteDeplacement_Warning_No_GPS_Agence :
                    string.Empty;
        }

        #endregion
        #region Fonctions privées

        #region Code et zone déplacement

        /// <summary>
        /// Calcule le code déplacement et la zone d'une indemnité.
        /// </summary>
        /// <param name="indemnite">Indemnité concernée.</param>
        private void CalculCodeEtZoneDeplacement(IndemniteDeplacementEnt indemnite)
        {
            indemnite.CodeZoneDeplacement = null;
            indemnite.CodeDeplacement = null;
            double nombreKm;
            if (indemnite.Personnel.Societe.IndemniteDeplacementCalculTypeId == (int)IndemniteDeplacementCalculType.KilometreVolOiseau)
            {
                nombreKm = indemnite.NombreKilometreVOChantierRattachement ?? 0;
            }
            else
            {
                nombreKm = indemnite.NombreKilometres;
            }

            ////////////////////////////////////////////////
            //// BIZZRULES de génération des indemnités ////
            ////////////////////////////////////////////////
            if (indemnite.Personnel.Societe.Groupe.Code.Equals(Constantes.CodeGroupeFTP))
            {
                indemnite.CodeZoneDeplacement = codeZoneMgr.GetCodeZoneDeplacementByKm(indemnite.Personnel.Societe.SocieteId, nombreKm);
            }
            else if (indemnite.Personnel.EtablissementRattachement.HorsRegion)
            {
                CalculCodeEtZoneDeplacementRattachementHorsRegion(indemnite, nombreKm);
            }
            else
            {
                CalculCodeEtZoneDeplacementRattachementEnRegion(indemnite, nombreKm);
            }
            ////////////////////////////////////////////////////
            //// Fin BIZZRULES de génération des indemnités ////
            ////////////////////////////////////////////////////

            // Mise à jour des identifiants
            UpdateCodeDeplacementIdEtZoneId(indemnite);
        }

        /// <summary>
        /// Calcule le code déplacement et la zone d'une indemnité pour un établissement de rattachement hors region.
        /// </summary>
        /// <param name="indemnite">Indemnité concernée.</param>
        /// <param name="km">nombre de kilometre calculé</param>
        private void CalculCodeEtZoneDeplacementRattachementHorsRegion(IndemniteDeplacementEnt indemnite, double km)
        {
            if (km < 50)
            {
                indemnite.CodeZoneDeplacement = GetZoneByDistance(indemnite.Personnel.Societe.SocieteId, km);
            }
            else
            {
                CalculCodeEtZoneDeplacementRattachementHorsRegionEgalOuPlusDe50Km(indemnite, km);
            }
        }

        /// <summary>
        /// Calcule le code déplacement et la zone d'une indemnité pour un établissement de rattachement hors region à moins de 50 km du domicile
        /// </summary>
        /// <param name="indemnite">Indemnité concernée.</param>
        /// <param name="km">nombre de kilometre calculé</param>
        private void CalculCodeEtZoneDeplacementRattachementHorsRegionEgalOuPlusDe50Km(IndemniteDeplacementEnt indemnite, double km)
        {
            if (km < 100)
            {
                indemnite.CodeDeplacement = codeDepMgr.GetCodeDeplacementByCode(IndemniteDeplacementConst.ConstCodeDeplacement40);
            }
            else
            {
                indemnite.CodeDeplacement = codeDepMgr.GetCodeDeplacementByCode(IndemniteDeplacementConst.ConstCodeDeplacement50);
            }
        }

        /// <summary>
        /// Calcule le code déplacement et la zone d'une indemnité pour un établissement de rattachement en region.
        /// </summary>
        /// <param name="indemnite">Indemnité concernée.</param>
        /// <param name="km">nombre de kilometre calculé</param>
        private void CalculCodeEtZoneDeplacementRattachementEnRegion(IndemniteDeplacementEnt indemnite, double km)
        {
            if (km <= 50)
            {
                indemnite.CodeZoneDeplacement = GetZoneByDistance(indemnite.Personnel.Societe.SocieteId, km);
            }
            else
            {
                indemnite.CodeDeplacement = codeDepMgr.GetCodeDeplacementByCode(IndemniteDeplacementConst.ConstCodeDeplacement50);
            }
        }

        #endregion
        #region Code et zone déplacement génération prévisionnelle

        /// <summary>
        /// Calcule le code déplacement et la zone d'une indemnité en prévisionnel pour un salarié et un CI suivant les règles de génération RAZEL-BEC.
        /// </summary>
        /// <param name="indemnite">Indemnité concernée.</param>
        /// <param name="dernierPointageReel">Dernier pointage réel de la période de paie pour un salarié</param>
        /// <param name="datePointagePrevisionnel">Date de génération du prochain pointage prévisionnelle</param>
        private void CalculCodeEtZoneDeplacementGenerationPrevisionnelle(IndemniteDeplacementEnt indemnite, RapportLigneEnt dernierPointageReel, DateTime datePointagePrevisionnel)
        {
            indemnite.CodeDeplacement = null;
            indemnite.CodeZoneDeplacement = null;
            var codepDernierPointage = dernierPointageReel.CodeDeplacement;

            ////////////////////////////////////////////////
            //// BIZZRULES de génération des indemnités ////
            ////////////////////////////////////////////////

            // Recopie de la zone
            indemnite.CodeZoneDeplacement = dernierPointageReel.CodeZoneDeplacement;

            if (codepDernierPointage != null)
            {
                // Exception codes 51 / 61
                if (codepDernierPointage.Code.Equals(IndemniteDeplacementConst.ConstCodeDeplacement51) || codepDernierPointage.Code.Equals(IndemniteDeplacementConst.ConstCodeDeplacement61))
                {
                    CalculCodeEtZoneDeplacementGenerationPrevisionnelleCode51Et61(indemnite, datePointagePrevisionnel);
                }

                // Si le derniere pointage contient une IGD
                if (codepDernierPointage.IGD)
                {
                    CalculCodeEtZoneDeplacementGenerationPrevisionnelleIgd(indemnite, datePointagePrevisionnel);
                }
            }

            ////////////////////////////////////////////////////
            //// Fin BIZZRULES de génération des indemnités ////
            ////////////////////////////////////////////////////

            // Mise à jour des identifiants
            UpdateCodeDeplacementIdEtZoneId(indemnite);
        }

        /// <summary>
        /// Calcule le code déplacement et la zone d'une indemnité en prévisionnel pour un salarié et un CI suivant les règles de génération RAZEL-BEC pour les codes 51 et 61.
        /// </summary>
        /// <param name="indemnite">Indemnité concernée.</param>
        /// <param name="datePointagePrevisionnel">Date de génération du prochain pointage prévisionnelle</param>
        private void CalculCodeEtZoneDeplacementGenerationPrevisionnelleCode51Et61(IndemniteDeplacementEnt indemnite, DateTime datePointagePrevisionnel)
        {
            // Vendredi => 61
            if (datePointagePrevisionnel.DayOfWeek.Equals(DayOfWeek.Friday))
            {
                // Récupération du codep en fonction de la société du salarié courant
                indemnite.CodeDeplacement = codeDepMgr.GetCodeDeplacementByCode(IndemniteDeplacementConst.ConstCodeDeplacement61);
            }
            else
            {
                // Reste de la semaine => 51
                indemnite.CodeDeplacement = codeDepMgr.GetCodeDeplacementByCode(IndemniteDeplacementConst.ConstCodeDeplacement51);
            }
        }

        /// <summary>
        /// Calcule le code déplacement et la zone d'une indemnité en prévisionnel pour un salarié et un CI suivant les règles de génération RAZEL-BEC pour les IGD.
        /// </summary>
        /// <param name="indemnite">Indemnité concernée.</param>
        /// <param name="datePointagePrevisionnel">Date de génération du prochain pointage prévisionnelle</param>
        private void CalculCodeEtZoneDeplacementGenerationPrevisionnelleIgd(IndemniteDeplacementEnt indemnite, DateTime datePointagePrevisionnel)
        {
            // Récupération de l'indemnité de déplacement paramétrée
            var indemniteDernierReel = Repository.GetIndemniteDeplacementByPersonnelIdAndCiId(indemnite.PersonnelId, indemnite.CiId);
            if (indemniteDernierReel != null && indemniteDernierReel.CodeDeplacement != null)
            {
                // SI IGD 40
                if (indemniteDernierReel.CodeDeplacement.Code.Equals(IndemniteDeplacementConst.ConstCodeDeplacement40))
                {
                    // Vendredi => 45
                    if (datePointagePrevisionnel.DayOfWeek.Equals(DayOfWeek.Friday))
                    {
                        indemnite.CodeDeplacement = codeDepMgr.GetCodeDeplacementByCode(IndemniteDeplacementConst.ConstCodeDeplacement45);
                    }
                    else
                    {
                        // Reste de la semaine => 40
                        indemnite.CodeDeplacement = codeDepMgr.GetCodeDeplacementByCode(IndemniteDeplacementConst.ConstCodeDeplacement40);
                    }
                }

                // SI IGD 50
                if (indemniteDernierReel.CodeDeplacement.Code.Equals(IndemniteDeplacementConst.ConstCodeDeplacement50))
                {
                    // Vendredi => 55
                    if (datePointagePrevisionnel.DayOfWeek.Equals(DayOfWeek.Friday))
                    {
                        indemnite.CodeDeplacement = codeDepMgr.GetCodeDeplacementByCode(IndemniteDeplacementConst.ConstCodeDeplacement55);
                    }
                    else
                    {
                        // Reste de la semaine => 50
                        indemnite.CodeDeplacement = codeDepMgr.GetCodeDeplacementByCode(IndemniteDeplacementConst.ConstCodeDeplacement50);
                    }
                }
            }
        }

        #endregion

        /// <summary>
        /// Calcul les kilometres Orthrodomie et routier entre le chantier, le domicile et l'agence.
        /// </summary>
        /// <param name="indemniteDeplacement">L'indemnite de déplacement concernée.</param>
        /// <returns>L'indemnite de déplacement avec les kilometres Orthrodomie et routier calculés.</returns>
        private IndemniteDeplacementEnt CalculKm(IndemniteDeplacementEnt indemniteDeplacement)
        {
            if (indemniteDeplacement.Personnel == null)
            {
                return indemniteDeplacement;
            }
            //Domicile / chantier
            indemniteDeplacement.NombreKilometreVODomicileChantier = new TypeRattachementCalcul<CalculChantierDomicile<CalculOrthrodromie>>().CalculKilometre(indemniteDeplacement.CI, indemniteDeplacement.Personnel);

            switch (indemniteDeplacement.Personnel.TypeRattachement)
            {
                case TypeRattachement.Agence:
                    indemniteDeplacement.NombreKilometreVOChantierRattachement = new TypeRattachementCalcul<CalculChantierAgence<CalculOrthrodromie>>().CalculKilometre(indemniteDeplacement.CI, indemniteDeplacement.Personnel);
                    indemniteDeplacement.NombreKilometreVODomicileRattachement = new TypeRattachementCalcul<CalculDomicileAgence<CalculOrthrodromie>>().CalculKilometre(indemniteDeplacement.CI, indemniteDeplacement.Personnel);
                    indemniteDeplacement.NombreKilometres = new TypeRattachementCalcul<CalculChantierAgence<CalculRoutiere>>().CalculKilometre(indemniteDeplacement.CI, indemniteDeplacement.Personnel);
                    break;
                case TypeRattachement.Secteur:
                    indemniteDeplacement.NombreKilometreVOChantierRattachement = new TypeRattachementCalcul<CalculChantierSecteur<CalculOrthrodromie>>().CalculKilometre(indemniteDeplacement.CI, indemniteDeplacement.Personnel);
                    indemniteDeplacement.NombreKilometreVODomicileRattachement = new TypeRattachementCalcul<CalculDomicileSecteur<CalculOrthrodromie>>().CalculKilometre(indemniteDeplacement.CI, indemniteDeplacement.Personnel);
                    indemniteDeplacement.NombreKilometres = new TypeRattachementCalcul<CalculChantierSecteur<CalculRoutiere>>().CalculKilometre(indemniteDeplacement.CI, indemniteDeplacement.Personnel);
                    break;
                default: //domicile
                    indemniteDeplacement.NombreKilometreVOChantierRattachement = new TypeRattachementCalcul<CalculChantierDomicile<CalculOrthrodromie>>().CalculKilometre(indemniteDeplacement.CI, indemniteDeplacement.Personnel);
                    indemniteDeplacement.NombreKilometres = new TypeRattachementCalcul<CalculChantierDomicile<CalculRoutiere>>().CalculKilometre(indemniteDeplacement.CI, indemniteDeplacement.Personnel);
                    break;
            }
            return indemniteDeplacement;
        }

        /// <summary>
        /// Met à jour les identifiants du code déplacement et de la zone.
        /// </summary>
        /// <param name="indemnite">Indemnité concernée.</param>
        private void UpdateCodeDeplacementIdEtZoneId(IndemniteDeplacementEnt indemnite)
        {
            // Mise à jour de l'identifiant zone déplacement
            if (indemnite.CodeZoneDeplacement != null)
            {
                indemnite.CodeZoneDeplacementId = indemnite.CodeZoneDeplacement.CodeZoneDeplacementId;
            }
            else
            {
                indemnite.CodeZoneDeplacementId = null;
            }

            // Mise à jour de l'identifiant code déplacement
            if (indemnite.CodeDeplacement != null)
            {
                indemnite.CodeDeplacementId = indemnite.CodeDeplacement.CodeDeplacementId;
            }
            else
            {
                indemnite.CodeDeplacementId = null;
            }
        }

        #endregion
    }
}
