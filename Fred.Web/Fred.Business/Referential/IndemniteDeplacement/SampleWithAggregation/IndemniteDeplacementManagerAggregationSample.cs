#pragma warning disable S125 // Code d'exemple

/*
 * Code d'exemple utilisant l'aggregation pour designer un manager
 * Pour l'héritage voir la classe IndemniteDeplacementManager
 * Pour la doc voir le fichier HowToWriteAManager.md
 *  
 * 
using Fred.DataAccess.Interfaces;
using Fred.Entities.IndemniteDeplacement;

namespace Fred.Business.IndemniteDeplacement
{
  public class IndemniteDeplacementManagerAggregationSample : Manager<IndemniteDeplacementEnt, IIndemniteDeplacementRepository>, IIndemniteDeplacementManagerAggregationSample
  {
    private readonly ICalculFeature calcul;
    private readonly ISearchFeature search;
    private readonly ICrudFeature crud;
    private readonly ICrudWithCalculFeature crudWithCalcul;


    public IndemniteDeplacementManagerAggregationSample(IUnitOfWork uow, ICalculFeature calculFeature, ISearchFeature searchFeature, ICrudFeature crudFeature, ICrudWithCalculFeature crudWithCalculFeature)
      : base(uow)
    {
      calcul = calculFeature;
      search = searchFeature;
      crud = crudFeature;
      crudWithCalcul = crudWithCalculFeature;
    }

    public ICalculFeature Calcul() { return calcul; }
    public ISearchFeature Search() { return search; }
    public ICrudFeature Crud() { return crud; }
    public ICrudWithCalculFeature CrudCalcul() { return crudWithCalcul; }
  }
}
*/

#pragma warning restore S125 // Code d'exemple