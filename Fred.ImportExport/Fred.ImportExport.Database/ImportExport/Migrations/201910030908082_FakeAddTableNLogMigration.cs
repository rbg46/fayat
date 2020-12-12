namespace Fred.ImportExport.Database.ImportExport.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FakeAddTableNLogMigration : DbMigration
    {
        public override void Up()
        {
            // Pourquoi cette migration ?
            /*
            L'entit� NLogEnt repr�sente une ligne dans la table des logs.
            Cette entit� est d�clar�e dans les entit�s de Fred Web, et elle est aussi utilis�e dans les entit�s de Fred IE....
            Suite � la migration EF 6 -> EF Core, les Data Annotation ont �t� remplac�es par la d�claration fluent de EF
            Hors cela n'est pas fait dans Fred IE.
            Donc EF dans Fred IE consid�re que l'entit� NLogEnt est une nouvelle entit�e.
            Correction : 
                Cr�ation d'une entit� NLogFredIeEnt dans Fred IE qui pointe sur la table Nlog                   (pour la diff�rentier de Fred Web)
                Refacto de l'utilisation du nom de la classe NlogEnt dans FredIE pour utiliser NLogFredIeEnt  
                Cr�ation d'une migration, mais vide, car la cr�ation de la table est d�j� effectu�e dans la migration initiale.
            */
        }

        public override void Down()
        {
            // voir le comment dans Up.
        }
    }
}
