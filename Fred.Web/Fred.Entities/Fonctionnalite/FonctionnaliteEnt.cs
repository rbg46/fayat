using Fred.Entities.Module;
using System;
using System.Diagnostics;

namespace Fred.Entities.Fonctionnalite
{
    /// <summary>
    ///   Représente un fonctionnalité.
    /// </summary>
    [DebuggerDisplay("FonctionnaliteId = {FonctionnaliteId} ModuleId = {ModuleId} Libelle = {Libelle} DateSuppression = {DateSuppression} ")]
    public class FonctionnaliteEnt
    {
        private DateTime? dateSuppression;

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'une fonctionnalité.
        /// </summary>
        public int FonctionnaliteId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un module.
        /// </summary>
        public int ModuleId { get; set; }

        /// <summary>
        ///   Obtient ou définit le code d'une fonctionnalité
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///   Obtient ou définit le libellé d'une fonctionnalité.
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si la fonctionnalité est hors organisation ou non
        /// </summary>
        public bool HorsOrga { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de suppression d'une fonctionnalité
        /// </summary>
        public DateTime? DateSuppression
        {
            get
            {
                return (dateSuppression.HasValue) ? DateTime.SpecifyKind(dateSuppression.Value, DateTimeKind.Utc) : default(DateTime?);
            }
            set
            {
                dateSuppression = (value.HasValue) ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : default(DateTime?);
            }
        }

        /// <summary>
        /// Parent Module pointed by [FRED_FONCTIONNALITE].([ModuleId]) (FK_FONCTIONNALITE_MODULE)
        /// </summary>
        public virtual ModuleEnt Module { get; set; }

        /// <summary>
        ///   Ce champ est calculé, il indique le mode de la fonctionnalité qui se trouve sur l'entité RoleFonctionnaliteEnt
        /// </summary>
        public FonctionnaliteTypeMode Mode { get; set; }

        /// <summary>
        ///   Obtient ou définit le libellé d'un module
        /// </summary>  
        public string Description { get; set; }
    }
}