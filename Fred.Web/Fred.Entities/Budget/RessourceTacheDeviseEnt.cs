using Fred.Entities.Referential;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fred.Entities.Budget
{
    /// <summary>
    ///   Représente Une liaison ressourTache et Devise
    /// </summary>
    public class RessourceTacheDeviseEnt : ICloneable
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'une entité RessourceTacheDevise
        /// </summary>
        public int RessourceTacheDeviseId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de la RessourceTache
        /// </summary>
        public int RessourceTacheId { get; set; }

        /// <summary>
        ///   Obtient ou définit la RessourceTache
        /// </summary>
        public RessourceTacheEnt RessourceTache { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de la devise
        /// </summary>
        public int DeviseId { get; set; }

        /// <summary>
        ///   Obtient ou définit la devise
        /// </summary>
        public DeviseEnt Devise { get; set; }

        /// <summary>
        /// Recopie du prix unitaire de la ressource si personnalisé
        /// </summary>
        public double? PrixUnitaire { get; set; }

        /// <summary>
        /// Clone
        /// </summary>
        /// <returns>Nouvelle référence</returns>
        public object Clone()
        {
            RessourceTacheDeviseEnt newRessTacheDevise = (RessourceTacheDeviseEnt)this.MemberwiseClone();

            return newRessTacheDevise;
        }
    }
}