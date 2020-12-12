using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Fred.Framework.Exceptions;
using Fred.ImportExport.Entities.Stair;

namespace Fred.ImportExport.DataAccess.Common.STAIR
{
    public class SphinxFormulaireRepository : AbstractRepositoryStair<StairSphinxFormulaireEnt>
    {
        /// <summary>
        ///   Initialise une nouvelle instance.
        /// </summary>
        /// <param name="unitOfWorkStair">L'instance de UnifOfWork</param>
        public SphinxFormulaireRepository(UnitOfWorkStair unitOfWorkStair)
          : base(unitOfWorkStair)
        {
        }



        public IEnumerable<StairSphinxFormulaireEnt> GetByTitre(string titre)
        {
            try
            {
                return Get().Where(x => x.TitreFormulaire == titre);
            }
            catch (Exception e)
            {
                throw new FredRepositoryException(e.Message, e);
            }

        }

        public StairSphinxFormulaireEnt GetByTitreAndDateCreation(string titre, string dateCreation)
        {
            try
            {
                var includes = new List<Expression<Func<StairSphinxFormulaireEnt, object>>>
                {
                    f => f.Questions.Select(q => q.Reponses)
                };

                return Get(includes).FirstOrDefault(x => x.TitreFormulaire == titre && x.DateCreationFormulaire == dateCreation);
            }
            catch (Exception e)
            {
                throw new FredRepositoryException(e.Message, e);
            }

        }

    }
}
