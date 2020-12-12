namespace Fred.Business.Budget.Avancement.Excel
{
    /// <summary>
    /// Cette classe décrit par une série de propriétés le format du fichier excel de l'avancement
    /// Comme par exemple la colonne contenant les valeurs de DAD au mois courant
    /// </summary>
    internal static class AvancementExcelDescriber
    {
        /// <summary>
        /// La colonne contenant les code tache
        /// </summary>
        internal const char CodeColumn = 'A';

        /// <summary>
        /// La colonne contenant les libellés
        /// </summary>
        internal const char LibelleColumn = 'B';

        /// <summary>
        /// La colonne contenant les commentaire
        /// </summary>
        internal const char CommentaireColumn = 'C';

        /// <summary>
        /// La colonne contenant les unités
        /// </summary>
        internal const char UniteColumn = 'D';

        /// <summary>
        /// La colonne contenant les quantités budgétées
        /// </summary>
        internal const char QuantiteColumn = 'F';

        /// <summary>
        /// La colonne contenant les prix unitaires budgétés
        /// </summary>
        internal const char PuColumn = 'G';

        /// <summary>
        /// Colonne contenant les montants
        /// </summary>
        internal const char MontantColumn = 'H';

        /// <summary>
        /// Colonne contenant les avancements au mois précédent, les cellules de cette colonne peuvent être vide
        /// </summary>
        internal const char AvancementMoisPrecedentColumn = 'J';

        /// <summary>
        /// Colonne contenant l'unité associée à l'avancement mois précedent
        /// Cette colonne contient soit % pour un T1-T2-T3, soit l'unité affichée dans la colonne Unite
        /// </summary>
        internal const char AvancementMoisPrecedentUniteColumn = 'K';

        /// <summary>
        /// Colonne contenant les avancements au mois courant
        /// </summary>
        internal const char AvancementMoisCourantColumn = 'L';

        /// <summary>
        /// Colonne contenant l'unité associée à l'avancement mois courant
        /// Cette colonne contient soit % pour un T1-T2-T3, soit l'unité affichée dans la colonne Unite et dans la colonne AvancementMoisPrecedentUnite
        /// </summary>
        internal const char AvancementMoisCourantUniteColumn = 'M';

        /// <summary>
        /// Cette colonne contient l'écart entre les deux avancements 
        /// </summary>
        internal const char EcartAvancementColumn = 'N';

        /// <summary>
        /// Colonne contenant l'unité associée à l'écart
        /// Cette colonne contient soit % pour un T1-T2-T3, soit l'unité affichée dans la colonne Unite
        /// </summary>
        internal const char EcartAvancementUniteColumn = 'O';

        /// <summary>
        /// Colonne contenant les Dad pour le mois précédent
        /// </summary>
        internal const char DadMoisPrecedentColumn = 'Q';

        /// <summary>
        /// Colonne contenant les Dad pour le mois courant
        /// </summary>
        internal const char DadMoisCourantColumn = 'R';

        /// <summary>
        /// Colonne contenant les écarts entre les deux DAD
        /// </summary>
        internal const char EcartDadColumn = 'S';

        /// <summary>
        /// Cellule dans laquelle sera affichée le libellé du CI contenant l'avancement
        /// </summary>
        internal const string CelluleLibelleCi = "C1";

        /// <summary>
        /// Cellule dans laquelle sera affichée la date de publication de l'export 
        /// </summary>
        internal const string CelluleDateEdition = "M1";

        /// <summary>
        /// Cellule, entête de la colonne contenant les avancements mois précedent.
        /// Cette cellule doit recevoir une valeur du type Novembre 2018 ou Février 2006
        /// </summary>
        internal const string CelluleMoisPrecedentAvancement = "J4";

        /// <summary>
        /// Cellule, entête de la colonne contenant les DAD mois précedent.
        /// Cette cellule doit recevoir une valeur du type Novembre 2018 ou Février 2006
        /// </summary>
        internal const string CelluleMoisPrecedentDad = "Q4";

        /// <summary>
        /// Cellule, entête de la colonne contenant les avancements du mois courant.
        /// Cette cellule doit recevoir une valeur du type Novembre 2018 ou Février 2006
        /// </summary>
        internal const string CelluleMoisCourantAvancement = "L4";

        /// <summary>
        /// Cellule, entête de la colonne contenant les DAD mois courant.
        /// Cette cellule doit recevoir une valeur du type Novembre 2018 ou Février 2006
        /// </summary>
        internal const string CelluleMoisCourantDad = "R4";
        /// <summary>
        /// Cette valeur vient du template de l'excel, cela représente la ligne (basé sur 1) à partir de laquelle sont injectées
        /// </summary>
        internal const int ExcelDataListeStartLine = 5;

        /// <summary>
        /// Cela veut dire qu'il y a aura X lignes d'écart entre la dernière lignes des données et la ligne des totaux
        /// </summary>
        internal const int LigneTotauxStartAfterEndLine = 1;

    }
}
