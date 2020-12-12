using System;
using Fred.Entities.Personnel;
using Fred.Web.Shared.App_LocalResources;

namespace Fred.Business.Personnel.Extensions
{
    /// <summary>
    /// Classe d'extension permettant de gérer des personnels
    /// </summary>
    public static class PersonnelExtension
    {
        /// <summary>
        /// Retourne true si le personnel est aussi un utilisateur
        /// </summary>
        /// <param name="this">personnel à analyser</param>
        /// <returns>un booléen jamais null</returns>
        public static bool IsUtilisateur(this PersonnelEnt @this)
        {
            return @this.Utilisateur != null && @this.Utilisateur.DateSupression == null;
        }

        /// <summary>
        /// Retourne une chaine de charactères : Cadre, Ouvrirer... correspondant au code statut du personnel
        /// </summary>
        /// <param name="this">Personnel a analyser</param>
        /// <returns>une chaine de caractère, null si le code est inconnu</returns>
        /// <exception cref="ArgumentException">Cette exception est lancée si le code statut est inconnu</exception>
        public static string GetPersonnelStatutString(this PersonnelEnt @this)
        {
            switch (@this.Statut)
            {
                case "1":
                    return FeaturePersonnel.Personnel_PartialGeneral_StatutOuvrier;
                case "2":
                    return FeaturePersonnel.Personnel_PartialGeneral_StatutEtamChantier;
                case "3":
                    return FeaturePersonnel.Personnel_PartialGeneral_StatutCadre;
                case "4":
                    return FeaturePersonnel.Personnel_PartialGeneral_StatutEtamBureau;
                case "5":
                    return FeaturePersonnel.Personnel_PartialGeneral_StatutEtamArticle36;
                default:
                    return null;
            }
        }

    }
}
