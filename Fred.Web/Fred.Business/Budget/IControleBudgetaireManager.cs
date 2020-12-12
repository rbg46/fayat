using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Business.Budget.ControleBudgetaire.Models;
using Fred.Entities.Budget;
using Fred.Framework.Exceptions;
using Fred.Web.Shared.Models.Budget.ControleBudgetaire;

namespace Fred.Business.Budget
{
    /// <summary>
    /// Manager des entités controle budgétaire
    /// </summary>
    public interface IControleBudgetaireManager : IManager<ControleBudgetaireEnt>
    {
        /// <summary>
        /// Récupère les donnés du controle budgétaire pour le budget en application du ci donné avec les axe donné 
        /// </summary>
        /// <param name="filter">ci dont on doit récupérer le budget en application</param>
        /// <returns>Retourne le controle budgétaire</returns>
        Task<ControleBudgetaireModel> GetControleBudgetaireAsync(ControleBudgetaireLoadModel filter);

        /// <summary>
        /// Sauvegarde les modifications apportées par le controle budgétaire 
        /// Les modifications portent sur l'ajustement, les commetaires sur l'ajustement.
        /// </summary>
        /// <remarks>La fonction va supprimer toutes les données connues pour ce controle budgétaire. Il est donc très important
        /// d'appeler cette fonction avec toutes les données même celles qui n'ont pas été modifiées.</remarks>
        /// <param name="saveModel">Un objet contenant les données à sauvegarder</param>
        /// <returns>Le nouvel etat du controle budgetaire</returns>
        ChangeEtatResultModel SauveControleBudgetaire(ControleBudgetaireSaveModel saveModel);

        /// <summary>
        /// Change l'état du controle budgétaire. Il est fortement conseillée d'appeler avant la méthode PeutPasserAEtat
        /// pour savoir si l'opération est possible
        /// </summary>
        /// <param name="budgetId">id du budget dont on recherche le controle budgétaire a valider</param>
        /// <param name="periode">periode d'application du controle budgétaire</param>
        /// <param name="codeEtat">Code etat de destination, les codes supportés sont 
        /// Brouillon : BR
        /// à valider : AV
        /// Validé : EA
        /// </param>
        /// <returns>Renvoi le nouvel etat du controle budgétaire</returns>
        /// <exception cref="FredBusinessMessageResponseException">Cette exception est levée si l'operation est impossible e.g passage d'un etat BR a l'état EA</exception>
        ChangeEtatResultModel ChangeEtatControleBudgetaire(int budgetId, int periode, string codeEtat);

        /// <summary>
        /// Renvoi true si le controle budgetaire rattaché au budget id donné peut être passé à l'état passé en paramètre
        /// </summary>
        /// <param name="budgetId">Budget id dont on recherche le controle budgetaire</param>
        /// <param name="periode">Periode d'application du controle budgétaire</param>
        /// <param name="etatCodeDestination">Code Etat tel que decrit dans la classe EtatBudget vers lequel on veut passer le budet</param>
        /// <returns>Un objet indiquant si toutes les conditions sont respectées</returns>
        ChangeEtatResultModel PeutPasserAEtat(int budgetId, int periode, string etatCodeDestination);

        /// <summary>
        /// Cette fonction supprime toutes les valeurs de controle budgétaire saisies pour ce budget
        /// </summary>
        /// <param name="budgetId">id du budget dont on supprime les valeurs</param>
        /// <param name="periode">Periode pour laquelle on supprime les données, si le paramètre est null alors toutes les données sont supprimées sans regarder la période</param>
        void SupprimeValeursControleBudgetairePourBudgetEtPeriode(int budgetId, int? periode = null);

        /// <summary>
        /// Cette fonction supprime le controle budgétaire et toutes ses valeurs pour ce budget et cette periode
        /// </summary>
        /// <param name="budgetId">id du budget dont on supprime les valeurs</param>
        /// <param name="periode">Periode pour laquelle on supprime les données, si le paramètre est null alors toutes les données sont supprimées sans regarder la période</param>
        void SupprimeControleBudgetairePourBudgetEtPeriode(int budgetId, int? periode = null);

        /// <summary>
        /// Renvoi true si le controle budgétaire est validé pour la période donnée
        /// </summary>
        /// <param name="budgetId">id du budget dont on veut connaitre l'état du controle budgétaire</param>
        /// <param name="periode">periode au format YYYYMM</param>
        /// <returns>un booléen : true si le controle budgétaire est validé, false sinon</returns>
        bool IsControleBudgetaireValide(int budgetId, int periode);

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
        /// Renvoi les valeurs du controle budgétaire pour le budget donnée à la période donné
        /// </summary>
        /// <param name="budgetId">id du budget en application ayant un controle budgétaire enregistré à la période donnée</param>
        /// <param name="periode">periode contenant un controle budgétaire</param>
        /// <returns>Un tableau de ControleBudgetaireValeursEnt contenant les valeurs, null si aucun controle budgétaire n'existe pour ce budget à cette période</returns>
        IEnumerable<ControleBudgetaireValeursEnt> GetControleBudgetaireValeurs(int budgetId, int periode);

        /// <summary>
        /// Retourne la dernière période de controle budgétaire avec valeurs 
        /// avant la période max si celle ci est définie
        /// </summary>
        /// <param name="budgetId">id du budget en application ayant un controle budgétaire enregistré à la période donnée</param>
        /// <param name="maxPeriode">periode avant laquelle effectuer la recherche</param>
        /// <returns>Période</returns>
        int? GetControleBudgetaireLatestPeriode(int budgetId, int? maxPeriode);
        
        /// <summary>
        /// Pour le controle budgétaire du budget ayant l'id donné et jusqu'a la période donnée
        /// la fonction retourne les périodes de tous les controles budgétaires à l'état brouillon jusqu'au dernier controle budgétaire
        /// existant ou validé
        /// e.g Imaginons un controle budgétaire validé au 01/2018 puis un budget brouillon au 02 et 03 2018
        /// Appeler cette fonction avec comme paramètre 04/2018 retournera 02/2018 et 03/2018
        /// Appeler cette fonction avec 01/2018 renverra une liste vide
        /// </summary>
        /// <param name="budgetId">Id du budget dont on analyse le controle budgétaire</param>
        /// <param name="periode">Periode d'analyse</param>
        /// <returns>une liste de periode au format YYYYMM</returns>
        IEnumerable<int> GetAllPeriodesBetweenPeriodeAndLastValidation(int budgetId, int periode);

        /// <summary>
        /// Retourne un controle budgétaire validé pour un CI et une période donnée
        /// </summary>
        /// <param name="ciId">Id du CI</param>
        /// <param name="periode">période</param>
        /// <returns>Controle Budgétaire</returns>
        BudgetEnt GetBudgetForCiAndPeriode(int ciId, int periode);

        /// <summary>
        /// retourne la liste des periodes de controle budgétaires validés pour le budget
        /// </summary>
        /// <param name="budgetId">Id de budget</param>
        /// <returns>List de périodes numériques</returns>
        List<int> GetListPeriodeControleBudgetaireValide(int budgetId);
    }
}
