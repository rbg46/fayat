using System.Collections.Generic;
using System.Linq;
using Fred.Framework.Exceptions;
using Fred.ImportExport.DataAccess.Common.STAIR;
using Fred.ImportExport.Entities.Stair;

namespace Fred.ImportExport.Business.Stair
{
    public class SphinxFormulaireManager : AbstractStairManager
    {
        private readonly SphinxFormulaireRepository sphinxFormulaireRepository;

        public SphinxFormulaireManager()
     : base()
        {
            this.sphinxFormulaireRepository = new SphinxFormulaireRepository(UnitOfWork);
        }

        public List<StairSphinxFormulaireEnt> GetAll()
        {
            try
            {
                return this.sphinxFormulaireRepository.Get().ToList();
            }
            catch (FredRepositoryException e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }


        public IEnumerable<StairSphinxFormulaireEnt> GetAll(IEnumerable<int> sfIds)
        {
            try
            {
                return this.sphinxFormulaireRepository.Get().Where(x => sfIds.Contains(x.SPHINXFormulaireId));
            }
            catch (FredRepositoryException e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        public StairSphinxFormulaireEnt Get(int formulaireId)
        {
            try
            {
                return this.sphinxFormulaireRepository.GetById(formulaireId);
            }
            catch (FredRepositoryException e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        public StairSphinxFormulaireEnt Get(string formulaireTitre, string dateCreation)
        {
            try
            {
                return this.sphinxFormulaireRepository.GetByTitreAndDateCreation(formulaireTitre, dateCreation);
            }
            catch (FredRepositoryException e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        public IEnumerable<StairSphinxFormulaireEnt> Get(string formulaireTitre)
        {
            try
            {
                return this.sphinxFormulaireRepository.GetByTitre(formulaireTitre);
            }
            catch (FredRepositoryException e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        public StairSphinxFormulaireEnt Add(string titreFormulaire, int nombreEnregistrement, string dateCreationFormulaire, string dateDerniereReponse, bool isOpen)
        {
            var sf = new StairSphinxFormulaireEnt
            {
                TitreFormulaire = titreFormulaire,
                NombreEnregistrement = nombreEnregistrement,
                DateCreationFormulaire = dateCreationFormulaire,
                DateDerniereReponse = dateDerniereReponse,
                IsOpen = isOpen
            };
            this.sphinxFormulaireRepository.Add(sf);
            this.UnitOfWork.Save();
            return sf;

        }

        public void Add(IEnumerable<StairSphinxFormulaireEnt> sfs)
        {
            foreach (StairSphinxFormulaireEnt sf in sfs)
            {
                this.sphinxFormulaireRepository.Add(sf);
            }
            this.UnitOfWork.Save();
        }

        public void Remove(StairSphinxFormulaireEnt formulaire)
        {
            try
            {
                this.sphinxFormulaireRepository.Delete(formulaire);
            }
            catch (FredRepositoryException e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

    }
}
