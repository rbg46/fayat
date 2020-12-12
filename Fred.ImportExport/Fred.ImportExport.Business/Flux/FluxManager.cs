using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using Fred.Framework.Exceptions;
using Fred.ImportExport.DataAccess.Interfaces;
using Fred.ImportExport.Entities.ImportExport;

namespace Fred.ImportExport.Business.Flux
{
    public class FluxManager : IFluxManager
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IFluxRepository fluxRepository;

        public FluxManager(IUnitOfWork unitOfWork, IFluxRepository fluxRepository)
        {
            this.unitOfWork = unitOfWork;
            this.fluxRepository = fluxRepository;
        }

        public List<FluxEnt> GetAll()
        {
            try
            {
                return fluxRepository.Get().ToList();
            }
            catch (FredRepositoryException e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        public List<FluxEnt> Search(string sort, string sortdir, int skip, int pageSize, out int totalRecord)
        {
            try
            {
                IQueryable<FluxEnt> flux = fluxRepository.Get();

                // Filtres
                // DM : AJOUTER LE FILTRE

                totalRecord = flux.Count();

                // Ordre
                flux = flux.OrderBy(sort + " " + sortdir);

                // Pagination
                if (pageSize > 0)
                {
                    flux = flux.Skip(skip).Take(pageSize);
                }

                return flux.ToList();
            }
            catch (FredRepositoryException e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        public void UpdateActive(int id, bool isActif)
        {
            try
            {
                FluxEnt flux = fluxRepository.GetById(id);
                if (flux == null)
                {
                    return;
                }

                flux.IsActif = isActif;
                fluxRepository.Update(flux);
                unitOfWork.Save();
            }
            catch (FredRepositoryException e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        public virtual FluxEnt GetByCode(string codeFlux)
        {
            try
            {
                return fluxRepository.Get().FirstOrDefault(x => x.Code.Equals(codeFlux, StringComparison.CurrentCultureIgnoreCase));
            }
            catch (FredRepositoryException e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }

        public virtual bool Update(FluxEnt flux)
        {
            try
            {
                if (flux == null)
                {
                    return false;
                }

                fluxRepository.Update(flux);
                unitOfWork.Save();

                return true;
            }
            catch (FredRepositoryException e)
            {
                throw new FredBusinessException(e.Message, e);
            }
        }
    }
}
