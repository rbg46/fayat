using Fred.Entities.Budget;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielEtendu;
using Fred.Entities.ReferentielFixe;
using System.Collections.Generic;

namespace Fred.Business.Budget
{
  /// <summary>
  ///   Interface de gestion d'un budget
  /// </summary>
  public interface IBudgetManagerOld : IManager<BudgetEnt>
  {
    /// <summary>
    /// Permet de créer une révision d'un budget
    /// </summary>
    /// <param name="budget">Budget associé</param>
    /// <param name="revisionNumber">Numéro de révision</param>
    /// <returns>Révision</returns>
    BudgetRevisionEnt CreateBudgetRevision(BudgetEnt budget, int revisionNumber);

    /// <summary>
    /// Permet de mettre à jour une révision d'un budget
    /// </summary>
    /// <param name="revision">Révision à mettre à jour</param>
    /// <returns>la révision MAJ</returns>
    BudgetRevisionEnt UpdateBudgetRevision(BudgetRevisionEnt revision);

    /// <summary>
    /// Permet de récupoérer un budget avec ses révision pour un CI
    /// </summary>
    /// <param name="ciId">Identifiant du CI</param>
    /// <returns>BudgetEnt</returns>
    BudgetEnt GetBudget(int ciId);

    /// <summary>
    /// Permet de récupérer une liste de tâches appartenant à une révision
    /// </summary>
    /// <param name="budgetRevisionId">Identifiant de la révision</param>
    /// <param name="ciId">Identifiant du Ci</param>
    /// <param name="flatList">Indique si on souihaite une liste hiérachique ou non</param>
    /// <returns>Liste de tâches</returns>
    ICollection<TacheEnt> GetBudgetRevisionTaches(int budgetRevisionId, int ciId, bool flatList = false);

    /// <summary>
    /// Permet de récupérer la liste des chapitres, sous-chapitres, ressources, référentiel étendus et paramétrages associés
    /// </summary>
    /// <param name="ciId">Identifiant du Ci</param>
    /// <param name="filter">Filtre sur les libellés</param>
    /// <param name="page">Numéro de page</param>
    /// <param name="pageSize">Nombre d'éléments par page</param>
    /// <returns>Liste de chapitres</returns>
    IEnumerable<ChapitreEnt> GetChapitres(int ciId, string filter, int page, int pageSize);

    /// <summary>
    /// Permet de récupérer une tâche avec la liste des ressources associés jusqu'aux devises
    /// </summary>
    /// <param name="tacheId">Identifiant de la tâche</param>
    /// <param name="ciId">Identifiant du Ci</param>
    /// <returns>tâche</returns>
    TacheEnt GetTacheWithRessourceTaches(int tacheId, int ciId);

    /// <summary>
    /// Permet de mettre à jour une tache T4 ainsi que les ressources taches associés
    /// </summary>
    /// <param name="tacheT4">Tache à mettre à jour</param>
    /// <param name="ciId">Identifiant du CI</param>
    /// <returns>la tache MAJ</returns>
    TacheEnt UpdateTacheWithRessourceTaches(TacheEnt tacheT4, int ciId);

    /// <summary>
    /// Permet de mettre à jour une liste de tâche d'un budget
    /// </summary>
    /// <param name="taches">Liste de tâches</param>
    /// <returns>-1 si erreur</returns>
    int UpdateBudgetRevisionTaches(ICollection<TacheEnt> taches);

    /// <summary>
    /// Permet de sauvegarder un détail de ressource
    /// </summary>
    /// <param name="ressource">ressource à insérer</param>
    /// <returns>ressource insérée</returns>
    RessourceEnt AddRessource(RessourceEnt ressource);

    /// <summary>
    /// Permet de MAJ un détail de ressource
    /// </summary>
    /// <param name="ressource">ressource à MAJ</param>
    /// <returns>ressource MAJ</returns>
    RessourceEnt UpdateRessource(RessourceEnt ressource);

    /// <summary>
    /// Permet de mettre à jour une liste de ressources
    /// </summary>
    /// <param name="ressources">La liste des ressouces</param>
    /// <returns>La liste de ressouce à jour</returns>
    ICollection<RessourceEnt> UpdateRessources(ICollection<RessourceEnt> ressources);

    /// <summary>
    /// Peremt de mettre à jour le référenteiel etendu
    /// </summary>
    /// <param name="param">objet à MAJ</param>
    /// <returns>L'objet MAJ</returns>
    ParametrageReferentielEtenduEnt UpdateParamRefEtendu(ParametrageReferentielEtenduEnt param);
  }
}
