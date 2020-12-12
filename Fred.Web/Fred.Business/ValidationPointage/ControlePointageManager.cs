using System.Collections.Generic;
using System.Linq;
using Fred.DataAccess.Interfaces;
using Fred.Entities.ValidationPointage;

namespace Fred.Business.ValidationPointage
{
    /// <summary>
    ///   Gestionnaire des Validation de pointage
    /// </summary>
    public class ControlePointageManager : Manager<ControlePointageEnt, IControlePointageRepository>, IControlePointageManager
    {
        private readonly IControlePointageRepository ctrlPointageRepo;
        private readonly IControlePointageErreurRepository ctrlPointageErreurRepo;

        public ControlePointageManager(IControlePointageValidator validator, IUnitOfWork uow, IControlePointageRepository ctrlPointageRepo, IControlePointageErreurRepository ctrlPointageErreurRepo)
          : base(uow, ctrlPointageRepo, validator)
        {
            this.ctrlPointageErreurRepo = ctrlPointageErreurRepo;
        }

        /// <inheritdoc />
        public ControlePointageEnt Get(int controlePointageId)
        {
            return Repository.Get(controlePointageId);
        }

        /// <inheritdoc />
        public ControlePointageEnt GetLatest(int lotPointageId, int typeControle)
        {
            return Repository.GetLatest(lotPointageId, typeControle);
        }

        /// <inheritdoc />
        public IEnumerable<ControlePointageEnt> GetAll()
        {
            return Repository.GetAll();
        }

        /// <inheritdoc />
        public IEnumerable<ControlePointageEnt> GetList(int lotPointageId)
        {
            return Repository.GetList(lotPointageId);
        }

        /// <inheritdoc />
        public IEnumerable<ControlePointageEnt> GetLatestList(List<int> listLotsPointagesIds)
        {
            return Repository.GetLatestList(listLotsPointagesIds);
        }

        /// <inheritdoc />
        public IEnumerable<ControlePointageEnt> GetList(int lotPointageId, int typeControle)
        {
            return Repository.GetList(lotPointageId, typeControle);
        }

        /// <inheritdoc />
        public ControlePointageEnt AddControlePointage(ControlePointageEnt controlePointage)
        {
            BusinessValidation(controlePointage);
            Repository.AddControlePointage(controlePointage);
            Save();

            return controlePointage;
        }

        /// <inheritdoc />
        public ControlePointageEnt UpdateControlePointage(ControlePointageEnt controlePointage)
        {
            BusinessValidation(controlePointage);
            Repository.UpdateControlePointage(controlePointage);
            Save();

            return controlePointage;
        }

        /// <inheritdoc />
        public ControlePointageEnt UpdateControlePointage(ControlePointageEnt controlePointage, int status)
        {
            controlePointage.Statut = status;
            BusinessValidation(controlePointage);
            Repository.UpdateControlePointage(controlePointage);
            Save();

            return controlePointage;
        }

        /// <inheritdoc />
        public void DeleteControlePointage(int controlePointageId)
        {
            Repository.DeleteControlePointage(controlePointageId);
            Save();
        }

        #region Gestion Controle Pointage Erreur 

        /// <inheritdoc />
        public ControlePointageErreurEnt AddControlePointageErreur(ControlePointageErreurEnt cpErreur)
        {
            ctrlPointageErreurRepo.AddControlePointageErreur(cpErreur);
            Save();

            return cpErreur;
        }

        /// <inheritdoc />
        public IEnumerable<PersonnelErreur<ControlePointageErreurEnt>> GetPersonnelErreurList(int controlePointageId, string searchText, int page, int pageSize)
        {
            IEnumerable<ControlePointageErreurEnt> erreurs = ctrlPointageErreurRepo.GetControlePointageErreurList(controlePointageId, searchText);
            var personnelErreurs = new List<PersonnelErreur<ControlePointageErreurEnt>>();
            var groupByPersonnel = erreurs.GroupBy(x => x.PersonnelId)
                                          .Skip((page - 1) * pageSize)
                                          .Take(pageSize);

            foreach (var g in groupByPersonnel.ToList())
            {
                var persoErreur = new PersonnelErreur<ControlePointageErreurEnt>
                {
                    PersonnelId = g.Key,
                    Personnel = g.FirstOrDefault().Personnel,
                    Erreurs = g.ToList()
                };
                personnelErreurs.Add(persoErreur);
            }

            return personnelErreurs;
        }

        /// <inheritdoc />
        public IEnumerable<PersonnelErreur<ControlePointageErreurEnt>> GetPersonnelErreurList(int controlePointageId)
        {
            IEnumerable<ControlePointageErreurEnt> erreurs = ctrlPointageErreurRepo.GetControlePointageErreurList(controlePointageId, string.Empty);
            var personnelErreurs = new List<PersonnelErreur<ControlePointageErreurEnt>>();
            var groupByPersonnel = erreurs.GroupBy(x => x.PersonnelId);

            foreach (var g in groupByPersonnel.ToList())
            {
                var persoErreur = new PersonnelErreur<ControlePointageErreurEnt>
                {
                    PersonnelId = g.Key,
                    Personnel = g.FirstOrDefault().Personnel,
                    Erreurs = g.ToList()
                };
                personnelErreurs.Add(persoErreur);
            }

            return personnelErreurs;
        }
        #endregion
    }
}