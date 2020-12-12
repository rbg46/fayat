using System.Collections.Generic;
using Fred.Web.Models;

namespace Fred.Web.Shared.Models
{
    public class AssocieSepModel
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
        ///     Obtient ou définit l'identifiant de la société associée
        /// </summary>
        public int SocieteAssocieeId { get; set; }

        /// <summary>
        ///     Obtient ou définit la société parent
        /// </summary>        
        public SocieteLightModel SocieteAssociee { get; set; }

        /// <summary>
        ///     Obtient ou définit l'identifiant du type de participation de la société (Gérant, Mandataire, Associé)
        /// </summary>
        public int TypeParticipationSepId { get; set; }

        /// <summary>
        ///     Obtient ou définit le type de participation de la société (Gérant, Mandataire, Associé)
        /// </summary>        
        public TypeParticipationSepModel TypeParticipationSep { get; set; }

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
        public FournisseurLightModel Fournisseur { get; set; }

        /// <summary>
        ///     Obtient ou définit l'identifiant l'associe SEP parent
        /// </summary>
        public int? AssocieSepParentId { get; set; }

        /// <summary>
        ///     Obtient ou définit l'associe SEP parent
        /// </summary>        
        public AssocieSepModel AssocieSepParent { get; set; }

        /// <summary>
        ///     Liste des Associés Sep enfant
        /// </summary>
        public ICollection<AssocieSepModel> AssocieSepChildren { get; set; }
    }
}
