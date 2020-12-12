using System;
using System.Linq;
using Fred.Entities;
using Fred.Entities.Personnel;

namespace Fred.Business.Personnel.Import
{
    /// <summary>
    /// Helper qui determine si un personnel ANAEL est different d'un personnel FRED.
    /// Ce Helper copie aussi les proprieté du personnel ANAEN  vers le personnel FRED.
    /// </summary>
    public static class ChangeTrackerAndModifierImportHelper
    {

        /// <summary>
        /// Determine si l'adresse du personnel anael a changée.
        /// </summary>
        /// <param name="persoInterneFred">persoInterneFred</param>
        /// <param name="persoInterneAnael">persoInterneAnael</param>
        /// <returns>true si une propriete a changée</returns>
        public static bool PersonnelAdressHasChanged(PersonnelEnt persoInterneFred, PersonnelEnt persoInterneAnael)
        {
            return string.Compare(persoInterneFred.Adresse, persoInterneAnael.Adresse) != 0;
        }

        /// <summary>
        /// Determine si des propriétées autre que l'adresse ont changées.
        /// </summary>
        /// <param name="persoInterneFred">persoInterneFred</param>
        /// <param name="persoInterneAnael">persoInterneAnael</param>
        /// <returns>true si une propriete a changée</returns>
        public static bool PersonnelPropertiesOtherOfAdressAreChanged(PersonnelEnt persoInterneFred, PersonnelEnt persoInterneAnael)
        {
            if (string.Compare(persoInterneFred.Nom, persoInterneAnael.Nom) != 0)
            {
                return true;
            }
            if (string.Compare(persoInterneFred.Prenom, persoInterneAnael.Prenom) != 0)
            {
                return true;
            }
            if (persoInterneFred.EtablissementPaieId != persoInterneAnael.EtablissementPaieId)
            {
                return true;
            }
            if (DateTime.Compare(persoInterneFred.DateEntree ?? DateTime.MinValue, persoInterneAnael.DateEntree ?? DateTime.MinValue) != 0)
            {
                return true;
            }
            if (string.Compare(persoInterneFred.CategoriePerso, persoInterneAnael.CategoriePerso) != 0)
            {
                return true;
            }
            if (string.Compare(persoInterneFred.Statut, persoInterneAnael.Statut) != 0)
            {
                return true;
            }
            if (persoInterneFred.RessourceId != persoInterneAnael.RessourceId)
            {
                return true;
            }
            if (string.Compare(persoInterneFred.CodeEmploi, persoInterneAnael.CodeEmploi) != 0)
            {
                return true;
            }
            if (persoInterneFred.PaysId != persoInterneAnael.PaysId)
            {
                return true;
            }

            var dateSortieFredForCompare = persoInterneFred.DateSortie.HasValue ? (DateTime)persoInterneFred.DateSortie : DateTime.MinValue;
            var dateSortieAnaelForCompare = persoInterneAnael.DateSortie.HasValue ? (DateTime)persoInterneAnael.DateSortie : DateTime.MinValue;

            if (DateTime.Compare(dateSortieFredForCompare, dateSortieAnaelForCompare) != 0)
            {
                return true;
            }

            if (string.Compare(persoInterneFred.Email, persoInterneAnael.Email) != 0)
            {
                return true;
            }

            var matriculeExterne = persoInterneFred.MatriculeExterne?.FirstOrDefault(x => x.Source == Constantes.MatriculeExterneSapResource);
            string personnelMatricule = persoInterneFred.Matricule.Substring(Math.Max(0, persoInterneFred.Matricule.Length - 5));
            if (persoInterneFred.Societe.CodeSocietePaye == Constantes.CodeSocietePayeFTP && (matriculeExterne == null || matriculeExterne.Matricule != string.Concat("001", personnelMatricule)))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        ///  Copie l'adresse au personnel FRED depuis le personnel ANAEL
        /// </summary>
        /// <param name="persoInterneAnael">from</param>
        /// <param name="persoInterneFred">to</param>
        public static void CopyAddressProperties(PersonnelEnt persoInterneAnael, PersonnelEnt persoInterneFred)
        {
            if (persoInterneAnael != null && persoInterneFred != null)
            {
                persoInterneFred.Adresse1 = persoInterneAnael.Adresse1;
                persoInterneFred.Adresse2 = persoInterneAnael.Adresse2;
                persoInterneFred.Adresse3 = persoInterneAnael.Adresse3;
                persoInterneFred.CodePostal = persoInterneAnael.CodePostal;
                persoInterneFred.Ville = persoInterneAnael.Ville;
                persoInterneFred.PaysLabel = persoInterneAnael.PaysLabel;
                persoInterneFred.PaysId = persoInterneAnael.PaysId;
                persoInterneFred.DateModification = DateTime.UtcNow;
                persoInterneFred.UtilisateurIdModification = 1;
            }
        }

        /// <summary>
        /// Copie les données, autre que l'adresse, au personnel FRED depuis le personnel ANAEL
        /// </summary>
        /// <param name="persoInterneFred">persoInterneFred</param>
        /// <param name="persoInterneAnael">persoInterneAnael</param>
        public static void CopyPersonnelPropertiesOtherOfAdress(PersonnelEnt persoInterneFred, PersonnelEnt persoInterneAnael)
        {
            persoInterneFred.Nom = persoInterneAnael.Nom;
            persoInterneFred.Prenom = persoInterneAnael.Prenom;
            persoInterneFred.TypeRattachement = persoInterneAnael.TypeRattachement;
            persoInterneFred.DateEntree = persoInterneAnael.DateEntree;
            persoInterneFred.DateSortie = persoInterneAnael.DateSortie;
            persoInterneFred.EtablissementPaieId = persoInterneAnael.EtablissementPaieId;
            persoInterneFred.EtablissementRattachementId = persoInterneAnael.EtablissementRattachementId;
            persoInterneFred.CategoriePerso = persoInterneAnael.CategoriePerso;
            persoInterneFred.Statut = persoInterneAnael.Statut;
            persoInterneFred.RessourceId = persoInterneAnael.RessourceId;
            persoInterneFred.CodeEmploi = persoInterneAnael.CodeEmploi;
            persoInterneFred.DateModification = DateTime.UtcNow;
            persoInterneFred.UtilisateurIdModification = 1;
            persoInterneFred.Email = persoInterneFred.Email ?? persoInterneAnael.Email;
            persoInterneFred.PaysId = persoInterneAnael.PaysId;
            persoInterneFred.PaysLabel = persoInterneAnael.PaysLabel;
        }
    }
}
