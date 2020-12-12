using System;
using Fred.Entities.Referential;

namespace Fred.Web.Shared.Models.Nature
{
    /// <summary>
    /// Modèle de la famille
    /// </summary>
    public class NatureFamilleOdModel
    {
        /// <summary>
        /// Identifiant de la nature
        /// </summary>
        public int IdNature { get; set; }

        /// <summary>
        /// Code de la nature
        /// </summary>
        public string NatureAnalytique { get; set; }

        /// <summary>
        /// Code de la nature (Mappé avec la vue FamilleOperationDiverse\Index.cshtml, à ne pas renommer)
        /// </summary>
        public string Code => NatureAnalytique;

        /// <summary>
        /// Libelle de la nature
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        /// Identifiant de la famille d'OD avec commande
        /// </summary>
        public int ParentFamilyODWithOrder { get; set; }

        /// <summary>
        /// Identifiant de la famille d'OD sans commande
        /// </summary>
        public int ParentFamilyODWithoutOrder { get; set; }

        /// <summary>
        /// Code de la famille d'OD avec commande
        /// </summary>
        public string CodeFamilleODWithOrder { get; set; }

        /// <summary>
        /// Code de la famille d'OD sans commande
        /// </summary>
        public string CodeFamilleODWhitoutOrder { get; set; }

        /// <summary>
        /// Obtient ou définit si la nature est cochée dans l'écran.
        /// </summary>
        public bool Checked { get; set; }

        /// <summary>
        /// Obtient ou définit si la nature est sélectionnée dans l'écran.
        /// </summary>
        public bool Selected { get; set; }

        /// <summary>
        /// Obtient ou définit le type de nature.
        /// </summary>
        public string TypeNature { get; set; }

        /// <summary>
        /// Obtient ou définit la date de création de la nature
        /// </summary>
        public DateTime? DateCreation { get; set; }

        /// <summary>
        /// Identifiant de la société
        /// </summary>
        public int SocieteId { get; set; }

        /// <summary>
        /// Code de la société
        /// </summary>
        public string SocieteCode { get; set; }

        /// <summary>
        /// Nature
        /// </summary>
        public NatureEnt Nature { get; set; }
    }
}
