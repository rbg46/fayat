using Fred.Entities.Fonctionnalite;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Fred.Entities.Module
{
    /// <summary>
    ///   Représente un module.
    /// </summary>
    [DebuggerDisplay("ID = {ModuleId} Code = {Code} Libelle = {Libelle} DateSuppression = {DateSuppression} ")]
    public class ModuleEnt
    {
        private DateTime? dateSuppression;

        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un module.
        /// </summary>
        public int ModuleId { get; set; }

        /// <summary>
        ///   Obtient ou définit le code d'un module
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        ///   Obtient ou définit le libellé d'un module
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        ///   Obtient ou définit le libellé d'un module
        /// </summary>  
        public string Description { get; set; }

        /// <summary>
        ///   Obtient ou définit la date de suppression d'un module
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
        ///   Obtient ou définit la liste des fonctionnalités d'un module
        /// </summary>
        public ICollection<FonctionnaliteEnt> Fonctionnalites { get; set; }
    }
}