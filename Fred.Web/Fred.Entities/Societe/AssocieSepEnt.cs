using System.Collections.Generic;
using Fred.Entities.Referential;
using Fred.Entities.Societe;

namespace Fred.Entities
{
    /// <summary>
    ///     Représente un associé SEP
    /// </summary>
    public class AssocieSepEnt
    {
        /// <summary>
        ///   Identifiant unique de l'entité
        /// </summary>
        public int AssocieSepId { get; set; }

        /// <summary>
        ///     Obtient ou définit l'identifiant de la société parent
        /// </summary>
        public int SocieteId { get; set; }

        /// <summary>
        ///     Obtient ou définit la société parent
        /// </summary>
        public SocieteEnt Societe { get; set; }

        /// <summary>
        ///     Obtient ou définit l'identifiant de la société associée
        /// </summary>
        public int SocieteAssocieeId { get; set; }

        /// <summary>
        ///     Obtient ou définit la société associée
        /// </summary>
        public SocieteEnt SocieteAssociee { get; set; }

        /// <summary>
        ///     Obtient ou définit l'identifiant du type de participation de la société (Gérant, Mandataire, Associé)
        /// </summary>
        public int TypeParticipationSepId { get; set; }

        /// <summary>
        ///     Obtient ou définit le type de participation de la société (Gérant, Mandataire, Associé)
        /// </summary>
        public TypeParticipationSepEnt TypeParticipationSep { get; set; }

        /// <summary>
        ///   Obtient ou définit la quote-part de la société dans la SEP
        /// </summary>    
        public decimal QuotePart { get; set; }

        /// <summary>
        ///     Obtient ou définit l'identifiant du fournisseur
        /// </summary>
        public int FournisseurId { get; set; }

        /// <summary>
        ///     Obtient ou défintit le fournisseur
        /// </summary>
        public FournisseurEnt Fournisseur { get; set; }

        /// <summary>
        ///     Obtient ou définit l'identifiant l'associe SEP parent
        /// </summary>
        public int? AssocieSepParentId { get; set; }

        /// <summary>
        ///     Obtient ou définit l'associe SEP parent
        /// </summary>
        public AssocieSepEnt AssocieSepParent { get; set; }

        /// <summary>
        ///     Liste des Associés Sep enfant
        /// </summary>
        public ICollection<AssocieSepEnt> AssocieSepChildren { get; set; }

        /// <summary>
        /// Clear all properties except AssocieSepParent
        /// </summary>
        public void ClearProperties()
        {
            Fournisseur = null;
            Societe = null;
            SocieteAssociee = null;
            TypeParticipationSep = null;

            if (AssocieSepParent != null)
            {
                AssocieSepParent.ClearProperties();
            }
        }
    }
}
