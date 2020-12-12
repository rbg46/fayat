using System.Diagnostics;

namespace Fred.Entities.RepriseDonnees.Excel
{
    /// <summary>
    /// Repersente un ci sur un fichier excel d'import pour la reprise de données
    /// </summary>
    [DebuggerDisplay("NumeroDeLigne = {NumeroDeLigne} CodeSociete = {CodeSociete} CodeCi = {CodeCi}")]
    public class RepriseExcelCi
    {

        /// <summary>
        /// Le numero de ligne du fichier excel
        /// </summary>
        public string NumeroDeLigne { get; set; }

        /// <summary>
        /// Obtient ou définit le code de ma société.
        /// </summary>
        public string CodeSociete { get; set; }


        /// <summary>
        /// Le code du ci
        /// </summary>
        public string CodeCi { get; set; }

        /// <summary>
        ///   Obtient ou définit l'Adresse de l'affaire.
        /// </summary>     
        public string Adresse { get; set; }

        /// <summary>
        ///   Obtient ou définit l'Adresse2 de l'affaire.
        /// </summary>      
        public string Adresse2 { get; set; }

        /// <summary>
        ///   Obtient ou définit l'Adresse de l'affaire.
        /// </summary>     
        public string Adresse3 { get; set; }

        /// <summary>
        ///   Obtient ou définit la ville de l'affaire.
        /// </summary>     
        public string Ville { get; set; }

        /// <summary>
        ///   Obtient ou définit le code postal de l'affaire.
        /// </summary>      
        public string CodePostal { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant du pays du CI
        /// </summary>       
        public string CodePays { get; set; }

        /// <summary>
        ///   Obtient ou définit l'entête sur l'Adresse de livraison de l'affaire
        /// </summary>      
        public string EnteteLivraison { get; set; }

        /// <summary>
        ///   Obtient ou définit l'Adresse de livraison de l'affaire.
        /// </summary>       
        public string AdresseLivraison { get; set; }

        /// <summary>
        ///   Obtient ou définit le code postal de livraison de l'affaire.
        /// </summary>       
        public string CodePostalLivraison { get; set; }

        /// <summary>
        ///   Obtient ou définit le code postal de livraison de l'affaire.
        /// </summary>      
        public string VilleLivraison { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant du pays de livraison du CI
        /// </summary>     
        public string CodePaysLivraison { get; set; }

        /// <summary>
        ///   Obtient ou définit l'Adresse de Facturation de l'affaire.
        /// </summary>      
        public string AdresseFacturation { get; set; }

        /// <summary>
        ///   Obtient ou définit le code postal de Facturation de l'affaire.
        /// </summary>       
        public string CodePostalFacturation { get; set; }

        /// <summary>
        ///   Obtient ou définit le code postal de Facturation de l'affaire.
        /// </summary>      
        public string VilleFacturation { get; set; }

        /// <summary>
        ///   Obtient ou définit l'identifiant du pays de facturation du CI
        /// </summary>       
        public string CodePaysFacturation { get; set; }

        /// <summary>
        ///   Obtient ou définit le responsable chantier
        /// </summary>       
        public string MatriculeResponsableChantier { get; set; }

        /// <summary>
        ///   Obtient ou définit le responsable administratif
        /// </summary>       
        public string MatriculeResponsableAdministratif { get; set; }

        /// <summary>
        ///   Obtient ou définit une valeur indiquant si la zone de deplacement est modifiable
        /// </summary>      
        public string ZoneModifiable { get; set; }

        /// <summary>
        /// Date d'ouverture du ci
        /// </summary>
        public string DateOuverture { get; set; }

        /// <summary>
        /// FacturationEtablissement
        /// </summary>
        public string FacturationEtablissement { get; set; }
        /// <summary>
        /// Longitude de localisation
        /// </summary>
        public string Longitude { get; set; }
        /// <summary>
        /// Latitude de localisation
        /// </summary>
        public string Latitude { get; set; }
    }
}
