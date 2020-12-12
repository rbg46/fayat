using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities.ValidationPointage;

namespace Fred.Business.ValidationPointage
{
    /// <summary>
    ///   Gestionnaire des Lots de pointage
    /// </summary>
    public class RemonteeVracManager : Manager<RemonteeVracEnt, IRemonteeVracRepository>, IRemonteeVracManager
    {
        private readonly IUtilisateurManager userManager;
        private readonly IRemonteeVracErreurRepository remonteeVracErreurRepo;

        public RemonteeVracManager(
            IUtilisateurManager userManager,
            IRemonteeVracRepository remonteeVracRepository,
            IRemonteeVracValidator validator,
            IUnitOfWork uow,
            IRemonteeVracErreurRepository remonteeVracErreurRepo)
          : base(uow, remonteeVracRepository, validator)
        {
            this.userManager = userManager;
            this.remonteeVracErreurRepo = remonteeVracErreurRepo;
        }

        /// <inheritdoc/>
        public RemonteeVracEnt AddRemonteeVrac(RemonteeVracEnt remonteeVrac)
        {
            this.BusinessValidation(remonteeVrac);
            this.Repository.AddRemonteeVrac(remonteeVrac);
            Save();

            return remonteeVrac;
        }

        /// <inheritdoc/>
        public void DeleteremonteeVrac(int remonteeVracId)
        {
            this.Repository.DeleteRemonteeVrac(remonteeVracId);
            Save();
        }

        /// <inheritdoc/>
        public RemonteeVracEnt Get(int remonteeVracId) => Repository.Get(remonteeVracId);

        /// <inheritdoc/>
        public RemonteeVracEnt GetLatest(int utilisateurId, DateTime periode) => this.Repository.GetLatest(utilisateurId, periode);

        /// <inheritdoc/>
        public RemonteeVracEnt GetLatest(DateTime periode)
        {
            int currentUserId = this.userManager.GetContextUtilisateurId();
            return this.Repository.GetLatest(currentUserId, periode);
        }

        /// <inheritdoc/>
        public IEnumerable<RemonteeVracEnt> GetAll() => Repository.GetAll();

        /// <inheritdoc/>
        public RemonteeVracEnt UpdateRemonteeVrac(RemonteeVracEnt remonteeVrac)
        {
            this.BusinessValidation(remonteeVrac);
            this.Repository.UpdateRemonteeVrac(remonteeVrac);
            Save();

            return remonteeVrac;
        }

        #region RemonteeVracErreurEnt

        /// <inheritdoc />
        public RemonteeVracErreurEnt AddRemonteeVracErreur(RemonteeVracErreurEnt rvErreur)
        {
            remonteeVracErreurRepo.AddRemonteeVracErreur(rvErreur);
            Save();

            return rvErreur;
        }

        /// <inheritdoc />
        public int CountRemonteeVracErreur(int remonteeVracId, string searchText)
        {
            return remonteeVracErreurRepo.Get(remonteeVracId, searchText).ToList().Count;
        }

        /// <inheritdoc />
        public IEnumerable<PersonnelErreur<RemonteeVracErreurEnt>> GetPersonnelErreurList(int remonteeVracId, string searchText, int page, int pageSize)
        {
            IEnumerable<RemonteeVracErreurEnt> erreurs = remonteeVracErreurRepo.Get(remonteeVracId, searchText);
            var personnelErreurs = new List<PersonnelErreur<RemonteeVracErreurEnt>>();
            var groupByPersonnel = erreurs.GroupBy(x => x.PersonnelId)
                                          .Skip((page - 1) * pageSize)
                                          .Take(pageSize);

            foreach (var g in groupByPersonnel.ToList())
            {
                var persoErreur = new PersonnelErreur<RemonteeVracErreurEnt>
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
        public IEnumerable<PersonnelErreur<RemonteeVracErreurEnt>> GetPersonnelErreurList(int remonteeVracId)
        {
            IEnumerable<RemonteeVracErreurEnt> erreurs = remonteeVracErreurRepo.Get(remonteeVracId, string.Empty);
            var personnelErreurs = new List<PersonnelErreur<RemonteeVracErreurEnt>>();
            var groupByPersonnel = erreurs.GroupBy(x => x.PersonnelId);

            foreach (var g in groupByPersonnel.ToList())
            {
                var persoErreur = new PersonnelErreur<RemonteeVracErreurEnt>
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
        public RemonteeVracEnt UpdateRemonteeVrac(RemonteeVracEnt remonteeVrac, int status)
        {
            remonteeVrac.Statut = status;
            return this.UpdateRemonteeVrac(remonteeVrac);
        }
        #endregion
    }
}