using System;

namespace Fred.ImportExport.Models.Moyen
{
    /// <summary>
    /// Représente un model pour un moyen.
    /// </summary>
    public class MoyenModel
    {
        /// <summary>
        /// Obtient ou définit le code de ma société.
        /// </summary>
        public string CodeSociete { get; set; }

        /// <summary>
        /// Obtient ou définit le code de l'établissement.
        /// </summary>
        public string CodeEtablissement { get; set; }

        /// <summary>
        /// Obtient ou définit le code du moyen.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Obtient ou définit le libellé.
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Obtient ou définit le code d'une ressource.
        /// </summary>
        public string CodeRessource { get; set; }

        /// <summary>
        /// Obtient ou définit le commmentaire.
        /// </summary>
        public string Commentaire { get; set; }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si Actif
        /// </summary>
        public bool IsActif { get; set; }

        /// <summary>
        /// Obtient ou définit le code du site.
        /// </summary>
        public string CodeSiteAppartenance { get; set; }

        /// <summary>
        /// Obtient ou définit l'immatriculation machine 
        /// </summary>
        public string Immatriculation { get; set; }

        /// <summary>
        /// Obtient ou définit la date de création
        /// </summary>
        public DateTime? DateCreation { get; set; }

        /// <summary>
        /// Obtient ou définit la date de modification
        /// </summary>
        public DateTime? DateModification { get; set; }

        /// <summary>
        /// Obtient ou définit si le moyen est en location.
        /// </summary>
        public bool IsLocation { get; set; }

        /// <summary>
        /// Obtient ou définit si le moyen est importé.
        /// </summary>
        public bool IsImported { get; set; }

        /// <summary>
        /// Obtient ou définit le fabriquant du moyen
        /// </summary>
        public string Fabriquant { get; set; }

        /// <summary>
        /// Obtient ou définit la date de mise en service
        /// </summary>
        public DateTime? DateMiseEnService { get; set; }
    }
}
