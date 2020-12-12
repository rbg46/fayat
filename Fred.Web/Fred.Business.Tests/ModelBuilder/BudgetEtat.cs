using System;
using Fred.Entities.Budget;
using static Fred.Entities.Constantes;


namespace Fred.Business.Tests.ModelBuilder
{

    internal static class BudgetEtat
    {
        public static BudgetEtatEnt GetFakeBudgetEtatBrouillon()
        {

            return new BudgetEtatEnt
            {
                BudgetEtatId = 0,
                Code = EtatBudget.Brouillon,
                Libelle = "Brouillon"
            };


        }


        public static BudgetEtatEnt GetFakeBudgetEtatAValider()
        {
            return new BudgetEtatEnt
            {
                BudgetEtatId = 1,
                Code = EtatBudget.AValider,
                Libelle = "AValider"
            };
        }


        public static BudgetEtatEnt GetFakeBudgetEtatEnApplication()
        {
            return new BudgetEtatEnt
            {
                BudgetEtatId = 2,
                Code = EtatBudget.EnApplication,
                Libelle = "EnApplication"
            };
        }



        public static BudgetEtatEnt GetFakeBudgetEtatArchive()
        {
            return new BudgetEtatEnt
            {
                BudgetEtatId = 3,
                Code = EtatBudget.Archive,
                Libelle = "Archive"
            };
        }

        public static BudgetEtatEnt GetFakeBudgetEtatByCode(string code)
        {
            switch (code)
            {
                case EtatBudget.Brouillon:
                    return GetFakeBudgetEtatBrouillon();
                case EtatBudget.AValider:
                    return GetFakeBudgetEtatAValider();
                case EtatBudget.EnApplication:
                    return GetFakeBudgetEtatEnApplication();
                case EtatBudget.Archive:
                    return GetFakeBudgetEtatArchive();
                default:
                    throw new ArgumentException($"Test Unitaire : Code inconnu {code}");
            }
        }

    }

}
