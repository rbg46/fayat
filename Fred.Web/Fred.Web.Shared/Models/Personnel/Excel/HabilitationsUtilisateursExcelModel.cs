using System;

namespace Fred.Web.Shared.Models.Personnel.Excel
{
    /// <summary>
    /// Décrit le modèle attendu par le fichier excel d'export des personnels
    /// </summary>
    public class HabilitationsUtilisateursExcelModel
    {
    

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
        /// Libellé de la ressourec associée au personnel
        /// </summary>
        public string Ressource { get; set; }

        /// <summary>
        /// Etablissement de paie a laquelle le personnel est rattaché
        /// </summary>
        public string EtablissementPaie { get; set; }

        /// <summary>
        /// Date d'entrée du personnel dans la société
        /// </summary>
        public string DateEntree { get; set; }

        /// <summary>
        /// Date de sortie du personnel (s'il a quitté la société)
        /// </summary>
        public string DateSortie { get; set; }

        /// <summary>
        /// Libellé de la société a laquelle l'utilisateur appartient
        /// </summary>
        public string Societe { get; set; }

        /// <summary>
        /// Role de l'utilisateur 
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// Organisation de l'utilisateur
        /// </summary>
        public string Organisation { get; set; }

        /// <summary>
        /// Devise
        /// </summary>
        public string Devise { get; set; }

        /// <summary>
        /// Seuil
        /// </summary>
        public string Seuil { get; set; }

    }
}
