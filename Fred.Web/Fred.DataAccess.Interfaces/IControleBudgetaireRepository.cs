
using System;
using System.Collections.Generic;
using Fred.Entities.Budget;

namespace Fred.DataAccess.Interfaces

{
    /// <summary>
    /// Interface manipulant des entités du controle budgétaire
    /// </summary>
    public interface IControleBudgetaireRepository : IRepository<ControleBudgetaireEnt>
    {/// <summary>
     /// Supprime toutes lignes de la table ayant ce budgetid
     /// </summary>
     /// <param name="budgetId">l'id du budget lié au controle budgétaire dont on va supprimer les valeurs</param>
     /// <param name="periode">Periode dont on supprime les valeurs. Si la variable est null alors toutes les données seront supprimées</param>
        void RemoveAllValeursForBudgetEtPeriode(int budgetId, int? periode = null);

        /// <summary>
        /// Supprime un controle budgétaire et toutes ses valeurs pour un budget et une periode donnée
        /// </summary>
        /// <param name="budgetId">id du budget dont on supprime le controle budgétaire</param>
        /// <param name="periode">Periode dont on supprime les valeurs. Si la variable est null alors toutes les données seront supprimées</param>
        void RemoveControleBudgetairePourBudgetEtPeriode(int budgetId, int? periode = null);

        /// <summary>
        /// Renvoi true s'il existe un controle budgétaire pour de budget et cette periode
        /// </summary>
        /// <param name="budgetId">le budget id auquel serait rattaché le controle budgétaire</param>
        /// <returns>true s'il existe un controle budgétaire pour ce budget et cette periode, false sinon</returns>
        bool Any(int budgetId);

        /// <summary>
        /// Retourne la valeurs contenues pour cette tache et cette ressource  pour la periode
        /// </summary>
        /// <param name="controleBudgetaireId">Id du controle budgétaire dont on regarde les valeurs</param>
        /// <param name="periode">periode dont on récupère les valeurs</param>
        /// <param name="tache3Id">id de la tache de niveau 3 liée à la valeur</param>
        /// <param name="ressourceId">id de la ressource liée à la valeur</param>
        /// <returns>retourne la valeur si elle existe, null sinon</returns>
        ControleBudgetaireValeursEnt GetValeursForPeriodeByTacheEtRessource(int controleBudgetaireId, int periode, int tache3Id, int ressourceId);


        /// <summary>
        /// Retourne la valeurs contenue pour cette tache et cette periode dans la liste des valeurs du controle budgétaire donné
        /// </summary>
        /// <param name="controleBudgetaireId">Id du controle budgétaire dont on regarde les valeurs</param>
        /// <param name="periode">mois de création de la valeur</param>
        /// <returns>retourne la valeur si elle existe, null sinon</returns>
        IEnumerable<ControleBudgetaireValeursEnt> GetValeursForPeriode(int controleBudgetaireId, int periode);

        /// <summary>
        /// Retourne le controle budgetaire ayant l'id passé en paramètre
        /// </summary>
        /// <param name="controleBudgetaireId">L'id du controle budgétaire a récupérer</param>
        /// <param name="periode">Periode dont on récupère le controle budgétaire</param>
        /// <param name="avecValeurs">par défaut à false, si true alors la fonction inclue la liste des valeurs du controle budgétaire</param>
        /// <returns>Le controle budgétaire ayant l'id donné, null si aucun controle n'existe ayant cet id</returns>
        ControleBudgetaireEnt GetControleBudgetaire(int controleBudgetaireId, int periode, bool avecValeurs = false);

        /// <summary>
        /// Retourne le controle budgétaire ayant le budet Id donné
        /// </summary>
        /// <param name="budgetId">l'id du budget auquel est rattaché ce controle budgétaire</param>
        /// <param name="periode">Periode dont on récupère le controle budgétaire</param>
        /// <param name="avecValeurs">par défaut à false, si true alors la fonction inclue la liste des valeurs du controle budgétaire</param>
        /// <returns>le controle budgétaire s'il existe, null sinon</returns>
        ControleBudgetaireEnt GetControleBudgetaireByBudgetId(int budgetId, int periode, bool avecValeurs = false);

        /// <summary>
        /// Renvoi le code de l'état du controle budgétaire
        /// </summary>
        /// <param name="budgetId">id du budget dont on veut connaitre l'état du controle budgétaire</param>
        /// <param name="periode">periode au format YYYYMM</param>
        /// <returns>une chaine de caractère décrivant l'état</returns>
        /// <exception cref="InvalidOperationException">La fonction renverra cette exception si aucun controle budgétaire n'existe pour le budget et la période donnés</exception>
        string GetEtatControleBudgetaire(int budgetId, int periode);

        /// <summary>
        /// Cette fonction renvoie la date de validation la plus récente d'un controle budgétaire pour ce CI
        /// Si l'argument periode est renseigné alors la fonction renverra la dernière date de validation antérieure à la période donnée
        /// </summary>
        /// <param name="ciId">id du CI dont on veut la date de validation</param>
        /// <param name="periode">Periode délimitant la dernière validation à obtenir</param>
        /// <returns>Un entier au format YYYYMM potentiellement 0 si aucun controle budgétaire n'a été validé pour ce budget</returns>
        int GetLastValidationPeriode(int ciId, int? periode = null);

        /// <summary>
        /// Retourne la dernière période de controle budgétaire avec valeurs 
        /// avant la période max si celle ci est définie
        /// </summary>
        /// <param name="budgetId">id du budget en application ayant un controle budgétaire enregistré à la période donnée</param>
        /// <param name="maxPeriode">periode avant laquelle effectuer la recherche</param>
        /// <returns>Période</returns>
        int? GetControleBudgetaireLatestPeriode(int budgetId, int? maxPeriode);

        /// <summary>
        /// Retourne le controle budgétaire pour le CI et la période
        /// Si plusieurs controles budgétaires existent pour le CI/période, retourne le plus récent 
        /// </summary>
        /// <param name="ciId">Id du CI</param>
        /// <param name="periode">Période</param>
        /// <returns>Controle budgétaire</returns>
        ControleBudgetaireEnt GetControleBudgetaireValideForCiAndPeriode(int ciId, int periode);

        /// <summary>
        /// retourne la liste des periodes de controle budgétaires validés pour le budget
        /// </summary>
        /// <param name="budgetId">Id de budget</param>
        /// <returns>List de périodes numériques</returns>
        List<int> GetListPeriodeControleBudgetaireValide(int budgetId);
    }
}
