using System;

namespace Fred.Web.Shared.Models.Personnel.Excel
{
    /// <summary>
    /// Décrit le modèle attendu par le fichier excel d'export des personnels
    /// </summary>
    public class PersonnelExcelModel
    {
        /// <summary>
        /// Libellé de la société a laquelle l'utilisateur appartient
        /// </summary>
        public string Societe { get; set; }

        /// <summary>
        /// Matricule de l'utilisateur 
        /// </summary>
        public string Matricule { get; set; }

        /// <summary>
        /// Nom de l'utilisateur
        /// </summary>
        public string Nom { get; set; }

        /// <summary>
        /// Prénom de l'utilisateur
        /// </summary>
        public string Prenom { get; set; }

        /// <summary>
        /// Etablissement de paie a laquelle le personnel est rattaché
        /// </summary>
        public string EtablissementPaie { get; set; }

        /// <summary>
        /// O si le personnel est actif, N sinon
        /// </summary>
        public string IsActif { get; set; }

        /// <summary>
        /// O si le personnel est interne , N sinon
        /// </summary>
        public string IsInterne { get; set; }

        /// <summary>
        /// O si le personnel est interimaire, N sinon
        /// </summary>
        public string IsInterimaire { get; set; }

        /// <summary>
        /// O si le personnel est un utilisateur, N sinon
        /// </summary>
        public string IsUtilisateur { get; set; }

        /// <summary>
        /// Date d'entrée du personnel dans la société
        /// </summary>
        public string DateEntree { get; set; }

        /// <summary>
        /// Date de sortie du personnel (s'il a quitté la société)
        /// </summary>
        public string DateSortie { get; set; }

        /// <summary>
        /// Statut du personnel (Ouvrier, Cadre...)
        /// </summary>
        public string Statut { get; set; }

        /// <summary>
        /// Libellé de la ressourec associée au personnel
        /// </summary>
        public string Ressource { get; set; }

        /// <summary>
        /// Adresse 1 du personnel
        /// </summary>
        public string Adresse1 { get; set; }

        /// <summary>
        /// Adresse 2 du personnel
        /// </summary>
        public string Adresse2 { get; set; }

        /// <summary>
        /// Adresse 3 du personnel
        /// </summary>
        public string Adresse3 { get; set; }

        /// <summary>
        /// Code Postal du personnel
        /// </summary>
        public string CodePostal { get; set; }

        /// <summary>
        /// Ville du personnel
        /// </summary>
        public string Ville { get; set; }

        /// <summary>
        /// Pays du personnel
        /// </summary>
        public string Pays { get; set; }

        /// <summary>
        /// Email du personnel
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Organisation du personnel
        /// </summary>
        public string Organisation { get; set; }

        /// <summary>
        /// Role du personnel
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// Societe Habilitationdu personnel
        /// </summary>
        public string SocieteHabilitation { get; set; }

        /// <summary>
        /// Device du personnel lié à l'habilitation
        /// </summary>
        public string Device { get; set; }

        /// <summary>
        /// Seuil du personnel lié à l'habilitation
        /// </summary>
        public string Seuil { get; set; }
    }
}
