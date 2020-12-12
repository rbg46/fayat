using System;

namespace Fred.Web.Dtos.Mobile
{
    /// <summary>
    /// Dto d'un matériel
    /// </summary>
    public class MaterielDto : DtoBase
    {
        private string code;

        /// <summary>
        /// Obtient ou définit l'identifiant unique d'un matériel.
        /// </summary>
        public int MaterielId { get; set; }

        /// <summary>
        /// Obtient ou définit l'Id de la société 
        /// </summary>
        public int SocieteId { get; set; }

        /// <summary>
        /// Obtient ou définit la société du matériel
        /// </summary>
        public SocieteDto Societe { get; set; }

        ///// <summary>
        ///// Obtient ou définit l'Id de la ResourceId 
        ///// </summary>
        //public int ResourceId { get; set; }

        /// <summary>
        /// Obtient ou définit le code du matériel
        /// </summary>
        public string Code
        {
            get
            {
                return this.code;
            }

            set
            {
                if (value.Length > 20)
                    value.Substring(1, 20);
                this.code = value;
            }
        }

        /// <summary>
        /// Obtient ou définit le libellé
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Obtient ou définit une valeur indiquant si Actif
        /// </summary>
        public bool Actif { get; set; }

        /// <summary>
        /// Obtient ou définit la date de création
        /// </summary>
        public DateTime? DateCreation { get; set; }

        /// <summary>
        /// Obtient ou définit la date de modification
        /// </summary>
        public DateTime? DateModification { get; set; }

        /// <summary>
        /// Obtient ou définit la date de suppression
        /// </summary>
        public DateTime? DateSuppression { get; set; }
    }
}