using Fred.Entities.Utilisateur;

namespace Fred.Entities.Favori
{
    /// <summary>
    ///   Représente un favori
    /// </summary>
    public class FavoriEnt
    {
        /// <summary>
        ///   Obtient ou définit l'identifiant unique d'un favori.
        /// </summary>
        public int FavoriId { get; set; }

        /// <summary>
        ///   Obtient ou définit l'id utilisateur auquel est rattaché un favori.
        /// </summary>
        public int UtilisateurId { get; set; }

        /// <summary>
        ///   Obtient ou définit le libelle à afficher sur le favori
        /// </summary>
        public string Libelle { get; set; }

        /// <summary>
        ///   Obtient ou définit la couleur du favori
        /// </summary>
        public string Couleur { get; set; }

        /// <summary>
        ///   Obtient ou définit le type du favori
        /// </summary>
        public string TypeFavori { get; set; }

        /// <summary>
        ///   Obtient ou définit l'url du favori
        /// </summary>
        public string UrlFavori { get; set; }

        /// <summary>
        ///   Obtient ou définit le filtre de recherche au format binary
        /// </summary>
        public byte[] Search { get; set; }

        /// <summary>
        /// Parent Utilisateur pointed by [FRED_FAVORI_UTILISATEUR].([UtilisateurId]) (fkFavorisUtilisateur)
        /// </summary>
        public virtual UtilisateurEnt Utilisateur { get; set; }
    }
}