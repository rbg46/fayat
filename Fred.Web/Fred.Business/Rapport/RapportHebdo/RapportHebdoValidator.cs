using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Business.FeatureFlipping;
using Fred.Business.Referential;
using Fred.Entities.CI;
using Fred.Entities.Rapport;
using Fred.Framework.FeatureFlipping;
using Fred.Web.Shared.App_LocalResources;
using static Fred.Entities.Constantes;

namespace Fred.Business.Rapport.RapportHebdo
{
    /// <summary>
    /// Validateur des rapports hebdomadaires
    /// </summary>
    public class RapportHebdoValidator : ManagersAccess
    {
        private readonly IFeatureFlippingManager featureFlippingManager;

        #region Membres

        private readonly Dictionary<CIEnt, List<ErrorCode>> errors;
        private readonly RapportHebdoHelper helper;

        #endregion
        #region Constructeurs

        /// <summary>
        /// Constructeur.
        /// </summary>
        public RapportHebdoValidator(IFeatureFlippingManager featureFlippingManager, ICodeMajorationManager codeMajorationManager, IPrimeManager primeManager)
        {
            this.featureFlippingManager = featureFlippingManager;
            errors = new Dictionary<CIEnt, List<ErrorCode>>();
            helper = new RapportHebdoHelper(codeMajorationManager, primeManager);
        }

        #endregion
        #region Enumérations

        private enum ErrorCode
        {
            GdiAndThrr,
            NoGdiAndThra,
            GdiAndGdp,
            InvalidCiCP,
            InvalidGdiDepartement
        }

        #endregion
        #region Fonctions publiques

        /// <summary>
        /// Valide des rapports où des personnels sont pointés.
        /// </summary>
        /// <param name="personnelsRapports">Rapports où les personnels sont pointés.</param>
        /// <returns>True si les règles sont validées, sinon false.</returns>
        public bool Validate(PersonnelsRapports personnelsRapports)
        {
            var ret = true;
            bool? isVerificationCiCodePostalActive = null;

            foreach (var personnelRapports in personnelsRapports)
            {
                foreach (var rapport in personnelRapports.Rapports)
                {
                    foreach (var ligne in rapport.ListLignes.Where(l => l.PersonnelId.HasValue && l.PersonnelId.Value == personnelRapports.PersonnelId && l.DateSuppression == null))
                    {
                        if (ligne.Ci == null)
                        {
                            ligne.Ci = Managers.CI.FindById(ligne.CiId);
                        }
                        ret &= CheckRG(ligne, ref isVerificationCiCodePostalActive);
                    }
                }
            }
            return ret;
        }

        /// <summary>
        /// Valide une ligne de rapport.
        /// </summary>
        /// <param name="ligne">La ligne de rapport concernée.</param>
        /// <returns>True si les règles sont validées, sinon false.</returns>
        public bool Validate(RapportLigneEnt ligne)
        {
            bool? isVerificationCiCodePostalActive = null;

            if (ligne.PersonnelId != null && ligne.DateSuppression == null)
            {
                if (ligne.Ci == null)
                {
                    // Lazy loading à faire
                    ligne.Ci = Managers.CI.FindById(ligne.CiId);
                }

                return CheckRG(ligne, ref isVerificationCiCodePostalActive);
            }
            return true;
        }

        /// <summary>
        /// Retourne les messages d'erreur de validation.
        /// </summary>
        /// <returns>Les messages d'erreur de validation.</returns>
        public IEnumerable<string> GetErrorMessages()
        {
            foreach (var error in errors)
            {
                foreach (var errorCode in error.Value)
                {
                    yield return GetErrorMessage(error.Key, errorCode);
                }
            }
        }

        #endregion
        #region Fonctions privées

        /// <summary>
        /// Valide les rêgles de gestion.
        /// </summary>
        /// <param name="ligne">La ligne concernée.</param>
        /// <param name="isVerificationCiCodePostalActive">booléen feature flipping</param>
        /// <returns>True si les règles sont validées, sinon false.</returns>
        private bool CheckRG(RapportLigneEnt ligne, ref bool? isVerificationCiCodePostalActive)
        {
            var ret = true;
            ret &= CheckRG003(ligne);
            ret &= CheckRG004Dot1(ligne);
            ret &= CheckRG004Dot2(ligne);
            ret &= CheckRG004Dot3(ligne, ref isVerificationCiCodePostalActive);
            return ret;
        }

        /// <summary>
        /// Valide la RG-003.
        /// -> Une prime GDI / GDP ne peut pas être sélectionnée si une majoration THRR existe.
        /// </summary>
        /// <param name="ligne">La ligne concernée.</param>
        /// <returns>True si la règle est validée, sinon false.</returns>
        private bool CheckRG003(RapportLigneEnt ligne)
        {
            if ((helper.HasPrime(ligne, CodePrime.GDI) || helper.HasPrime(ligne, CodePrime.GDP)) && helper.HasMajoration(ligne, CodesMajoration.THRR))
            {
                AddError(ligne.Ci, ErrorCode.GdiAndThrr);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Valide la RG-004-1.
        /// -> La prime GDI / GDP est obligatoire si une majoration THRA existe.
        /// </summary>
        /// <param name="ligne">La ligne concernée.</param>
        /// <returns>True si la règle est validée, sinon false.</returns>
        private bool CheckRG004Dot1(RapportLigneEnt ligne)
        {
            if (helper.HasMajoration(ligne, CodesMajoration.THRA) && !helper.HasPrime(ligne, CodePrime.GDI) && !helper.HasPrime(ligne, CodePrime.GDP))
            {
                AddError(ligne.Ci, ErrorCode.NoGdiAndThra);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Valide la RG-004-2 (et RG-007-1).
        /// -> Les primes GDI et GDP ne doivent pas exister ensemble.
        /// </summary>
        /// <param name="ligne">La ligne concernée.</param>
        /// <returns>True si la règle est validée, sinon false.</returns>
        private bool CheckRG004Dot2(RapportLigneEnt ligne)
        {
            if (helper.HasPrime(ligne, CodePrime.GDI) && helper.HasPrime(ligne, CodePrime.GDP))
            {
                AddError(ligne.Ci, ErrorCode.GdiAndGdp);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Valide la RG-004-3 (et RG-007-2).
        /// -> La prime GDI / GDP dépend du code postal du chantier.
        /// </summary>
        /// <param name="ligne">La ligne concernée.</param>
        /// <param name="isVerificationCiCodePostalActive">booléen feature flipping</param>
        /// <returns>True si la règle est validée, sinon false.</returns>
        private bool CheckRG004Dot3(RapportLigneEnt ligne, ref bool? isVerificationCiCodePostalActive)
        {
            if (isVerificationCiCodePostalActive == null)
            {
                isVerificationCiCodePostalActive = featureFlippingManager.IsActivated(EnumFeatureFlipping.VerificationCodePostalCiPrimeGDIGDP);
            }
            if (!isVerificationCiCodePostalActive.Value)
            {
                return true;
            }

            if (helper.HasPrime(ligne, CodePrime.GDI) || helper.HasPrime(ligne, CodePrime.GDP))
            {
                if (ligne.Ci.CodePostal == null || ligne.Ci.CodePostal.Length < 2)
                {
                    AddError(ligne.Ci, ErrorCode.InvalidCiCP);
                    return false;
                }
                else
                {
                    var ciIsIdf = helper.IsCodePostalIdf(ligne.Ci.CodePostal);
                    if ((ciIsIdf && helper.HasPrime(ligne, CodePrime.GDP)) || (!ciIsIdf && helper.HasPrime(ligne, CodePrime.GDI)))
                    {
                        AddError(ligne.Ci, ErrorCode.InvalidGdiDepartement);
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Ajoute une erreur de validation.
        /// </summary>
        /// <param name="ci">Le CI concerné.</param>
        /// <param name="errorCode">Le code de l'erreur.</param>
        private void AddError(CIEnt ci, ErrorCode errorCode)
        {
            var errorCodes = errors.FirstOrDefault(e => e.Key.CiId == ci.CiId).Value;
            if (errorCodes == null)
            {
                errorCodes = new List<ErrorCode>();
                errorCodes.Add(errorCode);
                errors.Add(ci, errorCodes);
            }
            else if (!errorCodes.Contains(errorCode))
            {
                errorCodes.Add(errorCode);
            }
        }

        /// <summary>
        /// Retourne le message d'une l'erreur.
        /// </summary>
        /// <param name="ci">Le CI concerné.</param>
        /// <param name="errorCode">Le code de l'erreur.</param>
        /// <returns>Le message de l'erreur</returns>
        private string GetErrorMessage(CIEnt ci, ErrorCode errorCode)
        {
            switch (errorCode)
            {
                case ErrorCode.GdiAndThrr:
                    return string.Format(FeatureRapportHebdo.RapportHebdoValidator_Prime_GDI_Majo_THRR_Erreur, ci.Code);

                case ErrorCode.NoGdiAndThra:
                    return string.Format(FeatureRapportHebdo.RapportHebdoValidator_Majo_THRA_Prime_GDI_Erreur, ci.Code);

                case ErrorCode.GdiAndGdp:
                    return string.Format(FeatureRapportHebdo.RapportHebdoValidator_Prime_GDI_GDP_Erreur, ci.Code);

                case ErrorCode.InvalidCiCP:
                    return string.Format(FeatureRapportHebdo.RapportHebdoValidator_Code_Postal_Erreur, ci.Code);

                case ErrorCode.InvalidGdiDepartement:
                    return string.Format(FeatureRapportHebdo.RapportHebdoValidator_Prime_Departement_Erreur, ci.Code);

                default:
                    throw new NotImplementedException();
            }
        }

        #endregion
    }
}
