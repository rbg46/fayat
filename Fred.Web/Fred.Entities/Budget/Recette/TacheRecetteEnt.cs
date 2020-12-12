using Fred.Entities.Referential;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fred.Entities.Budget.Recette
{
    /// <summary>
    ///   Représente uneliaison tache recette.
    /// </summary>
    public class TacheRecetteEnt : ICloneable
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'une liaison tache recette.
        /// </summary>
        public int TacheRecetteId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de la tâche à laquelle cette recette appartient
        /// </summary>
        public int TacheId { get; set; }

        /// <summary>
        ///   Obtient ou définit la tâche à laquelle cette recette appartient
        /// </summary>
        public TacheEnt Tache { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant de la Devise attachée à cette recette
        /// </summary>
        public int DeviseId { get; set; }

        /// <summary>
        ///   Obtient ou définit la devise attachée à cette recette
        /// </summary>
        public DeviseEnt Devise { get; set; }

        /// <summary>
        ///   Obtient ou définit la recette
        /// </summary>
        public double? Recette { get; set; }

        /// <summary>
        /// Clone
        /// </summary>
        /// <returns>Nouvelle référence</returns>
        public object Clone()
        {
            TacheRecetteEnt newTacheRecette = (TacheRecetteEnt)this.MemberwiseClone();
            return newTacheRecette;
        }
    }
}
