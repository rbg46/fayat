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
            L'entité NLogEnt représente une ligne dans la table des logs.
            Cette entité est déclarée dans les entités de Fred Web, et elle est aussi utilisée dans les entités de Fred IE....
            Suite à la migration EF 6 -> EF Core, les Data Annotation ont été remplacées par la déclaration fluent de EF
            Hors cela n'est pas fait dans Fred IE.
            Donc EF dans Fred IE considère que l'entité NLogEnt est une nouvelle entitée.
            Correction : 
                Création d'une entité NLogFredIeEnt dans Fred IE qui pointe sur la table Nlog                   (pour la différentier de Fred Web)
                Refacto de l'utilisation du nom de la classe NlogEnt dans FredIE pour utiliser NLogFredIeEnt  
                Création d'une migration, mais vide, car la création de la table est déjà effectuée dans la migration initiale.
            */
        }

        public override void Down()
        {
            // voir le comment dans Up.
        }
    }
}
