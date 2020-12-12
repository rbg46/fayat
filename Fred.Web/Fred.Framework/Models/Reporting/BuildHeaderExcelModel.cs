namespace Fred.Framework.Models.Reporting
{
    /// <summary>
    /// Modèle contenant les informations de l'entête
    /// </summary>
    public class BuildHeaderExcelModel
    {
        /// <summary>
        /// Constructeur du modèle de construction de l'entête de l'édition
        /// </summary>
        /// <param name="titre">Titre de l'édition</param>
        /// <param name="sousTitre">Sous-titre de l'édition (Optionnel)</param>
        /// <param name="dateEdition">Libellé d'affichage de la date d'édition</param>
        /// <param name="editePar">Libellé de l'auteur de l'édition sous "dateEdition"</param>
        /// <param name="infoSupplementaire">Libelé d'informations supplémentaires présent sous "editePar" (Optionnel)</param>
        /// <param name="pathLogo">Chemin du logo</param>
        /// <param name="indexHeaderModel">Modèle contenant les index de colonne pour l'entête des éditions Excel</param>
        public BuildHeaderExcelModel(string titre, string sousTitre, string dateEdition, string editePar, string infoSupplementaire, string pathLogo, IndexHeaderExcelModel indexHeaderModel)
        {
            Titre = titre;
            SousTitre = sousTitre;
            DateEdition = dateEdition;
            EditePar = editePar;
            InfoSupplementaire = infoSupplementaire;
            IndexHeaderModel = indexHeaderModel;
            PathLogo = pathLogo;
        }

        /// <summary>
        /// Titre de l'édition
        /// </summary>
        public string Titre { get; }

        /// <summary>
        /// Sous-titre de l'édition
        /// </summary>
        public string SousTitre { get; }

        /// <summary>
        /// Libellé d'affichage de la date d'édition
        /// </summary>
        public string DateEdition { get; }

        /// <summary>
        /// Libellé de l'auteur de l'édition sous "dateEdition"
        /// </summary>
        public string EditePar { get; }

        /// <summary>
        /// Libelé d'informations supplémentaires présent sous "editePar"
        /// </summary>
        public string InfoSupplementaire { get; }

        /// <summary>
        /// Chemin du logo de la société
        /// </summary>
        public string PathLogo { get; }

        /// <summary>
        /// Libelé d'informations supplémentaires présent sous "editePar"
        /// </summary>
        public IndexHeaderExcelModel IndexHeaderModel { get; }
    }
}
