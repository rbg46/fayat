using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Business.Referential.IndemniteDeplacement.Features;
using Fred.DataAccess.Interfaces;
using Fred.Entities.CI;
using Fred.Entities.IndemniteDeplacement;
using Fred.Entities.Personnel;
using Fred.Entities.Rapport;
using Fred.Entities.Referential;

namespace Fred.Business.IndemniteDeplacement
{
    /// <summary>
    ///   Gestionnaire des indemnites de deplacement
    /// </summary>
    public class IndemniteDeplacementManager : Manager<IndemniteDeplacementEnt, IIndemniteDeplacementRepository>, IIndemniteDeplacementManager
    {
        private readonly ISearchFeature search;
        private readonly ICalculFeature calcul;
        private readonly ICrudFeature crud;
        private readonly ICrudWithCalculFeature crudWithCalcul;
        private readonly IExportKlm exportIndemnite;
        private readonly IRepository<IndemniteDeplacementCalculTypeEnt> indemniteDeplacementCalculTypeRepository;

        public IndemniteDeplacementManager(
            IUnitOfWork uow,
            IIndemniteDeplacementRepository indemniteDeplacementRepository,
            ICalculFeature calculFeature,
            ISearchFeature searchFeature,
            ICrudFeature crudFeature,
            ICrudWithCalculFeature crudWithCalculFeature,
            IExportKlm exportKlm,
            IRepository<IndemniteDeplacementCalculTypeEnt> indemniteDeplacementCalculTypeRepository)
          : base(uow, indemniteDeplacementRepository)
        {
            calcul = calculFeature;
            search = searchFeature;
            crud = crudFeature;
            crudWithCalcul = crudWithCalculFeature;
            exportIndemnite = exportKlm;
            this.indemniteDeplacementCalculTypeRepository = indemniteDeplacementCalculTypeRepository;
        }

        /// <summary>
        ///   Méthode de calcul d'une indemnité de déplacement pour un salarié et un CI suivant les règles de génération RAZEL-BEC
        /// </summary>
        /// <param name="personnel"> Personnel pour laquelle calculer l'indemnité </param>
        /// <param name="ci"> Affaire pour laquelle calculer l'indemnité </param>
        /// <returns> Retourne l'indemnité de déplacement calculée </returns>
        public IndemniteDeplacementEnt CalculIndemniteDeplacement(PersonnelEnt personnel, CIEnt ci)
        {
            return calcul.CalculIndemniteDeplacement(personnel, ci);
        }

        /// <inheritdoc />
        public IndemniteDeplacementEnt CalculIndemniteDeplacementGenerationPrevisionnelle(RapportLigneEnt dernierPointageReel, DateTime datePointagePrevisionnel)
        {
            return calcul.CalculIndemniteDeplacementGenerationPrevisionnelle(dernierPointageReel, datePointagePrevisionnel);
        }

        /// <inheritdoc />
        public CodeZoneDeplacementEnt GetZoneByDistance(int societeId, double distanceInKM)
        {
            return calcul.GetZoneByDistance(societeId, distanceInKM);

        }

        /// <inheritdoc />
        public int AddIndemniteDeplacement(IndemniteDeplacementEnt indemniteDeplacement)
        {
            return crud.AddIndemniteDeplacement(indemniteDeplacement);
        }

        /// <inheritdoc />
        public void DeleteIndemniteDeplacementById(int id)
        {
            crud.DeleteIndemniteDeplacementById(id);
        }

        /// <inheritdoc />
        public IEnumerable<IndemniteDeplacementEnt> GetIndemniteDeplacementByCi(int personnelID, int ciId)
        {
            return crud.GetIndemniteDeplacementByCi(personnelID, ciId);
        }

        /// <inheritdoc />
        public IQueryable<IndemniteDeplacementEnt> GetIndemniteDeplacementByPersonnel(int personnelID)
        {
            return crud.GetIndemniteDeplacementByPersonnel(personnelID);
        }

        /// <inheritdoc />
        public IndemniteDeplacementEnt GetIndemniteDeplacementByPersonnelIdAndCiId(int personnelId, int ciId)
        {
            return crud.GetIndemniteDeplacementByPersonnelIdAndCiId(personnelId, ciId);
        }

        /// <inheritdoc />
        public IEnumerable<IndemniteDeplacementEnt> GetIndemniteDeplacementList()
        {
            return crud.GetIndemniteDeplacementList();
        }

        /// <inheritdoc />
        public IEnumerable<IndemniteDeplacementEnt> GetIndemniteDeplacementListAll()
        {
            return crud.GetIndemniteDeplacementListAll();
        }

        /// <inheritdoc />
        public IEnumerable<IndemniteDeplacementEnt> GetIndemniteDeplacementByPersonnelListAll(int personnelID)
        {
            return crud.GetIndemniteDeplacementByPersonnelListAll(personnelID);
        }

        /// <inheritdoc />
        public IEnumerable<IndemniteDeplacementEnt> GetIndemniteDeplacementByPersonnelList(int personnelID)
        {
            return crud.GetIndemniteDeplacementByPersonnelList(personnelID);
        }

        /// <inheritdoc />
        public IEnumerable<IndemniteDeplacementEnt> GetIndemniteDeplacementByCIListAll(int idCI)
        {
            return crud.GetIndemniteDeplacementByCIListAll(idCI);
        }

        /// <inheritdoc />
        public IEnumerable<IndemniteDeplacementEnt> GetIndemniteDeplacementByCIList(int idCI)
        {
            return crud.GetIndemniteDeplacementByCIList(idCI);
        }

        /// <inheritdoc />
        public int UpdateIndemniteDeplacement(IndemniteDeplacementEnt indemniteDeplacement)
        {
            return this.crud.UpdateIndemniteDeplacement(indemniteDeplacement);
        }

        /// <inheritdoc />
        public int UpdateIndemniteDeplacementWithHistorical(IndemniteDeplacementEnt indDepNew, IndemniteDeplacementEnt indDepOld = null, bool toRemove = false)
        {
            return crud.UpdateIndemniteDeplacementWithHistorical(indDepNew, indDepOld, toRemove);

        }

        /// <inheritdoc />
        public IndemniteDeplacementEnt GetIndemniteDeplacementById(int id)
        {
            return crud.GetIndemniteDeplacementById(id);
        }

        /// <inheritdoc />
        public IEnumerable<IndemniteDeplacementEnt> GetIndemniteDeplacementByPersonnelId(int personnelId)
        {
            return crud.GetIndemniteDeplacementByPersonnelId(personnelId);
        }

        /// <inheritdoc />
        public int ImportIndemniteDeplacementFromHolding(int holdingId)
        {
            return crud.ImportIndemniteDeplacementFromHolding(holdingId);
        }

        /// <inheritdoc />
        public bool IsIndemniteDeplacementUnique(int personnelId, int ciId, int indId = 0)
        {
            return crud.IsIndemniteDeplacementUnique(personnelId, ciId, indId);
        }

        /// <inheritdoc />
        public IndemniteDeplacementEnt GetNewIndemniteDeplacement()
        {
            return crud.GetNewIndemniteDeplacement();
        }

        /// <inheritdoc />
        public IEnumerable<IndemniteDeplacementEnt> SearchIndemniteDeplacementWithFilters(string text, SearchIndemniteDeplacementEnt filters, int personnelId)
        {
            return search.SearchIndemniteDeplacementWithFilters(text, filters, personnelId);
        }

        /// <inheritdoc />
        public IEnumerable<IndemniteDeplacementEnt> SearchIndemniteDeplacementAllByPersonnelId(int personnelId)
        {
            return search.SearchIndemniteDeplacementAllByPersonnelId(personnelId);
        }

        /// <inheritdoc />
        public IEnumerable<IndemniteDeplacementEnt> SearchIndemniteDeplacementAllWithFilters(string text, SearchIndemniteDeplacementEnt filters)
        {
            return search.SearchIndemniteDeplacementAllWithFilters(text, filters);
        }

        /// <inheritdoc />
        public SearchIndemniteDeplacementEnt GetDefaultFilter()
        {
            return search.GetDefaultFilter();
        }

        /// <inheritdoc />
        public IndemniteDeplacementEnt GetOrCreateIndemniteDeplacementByPersonnelAndCi(PersonnelEnt personnel, CIEnt ci, bool refresh)
        {
            return crudWithCalcul.GetOrCreateIndemniteDeplacementByPersonnelAndCi(personnel, ci, refresh);
        }

        /// <inheritdoc />
        public IndemniteDeplacementEnt GetCalculIndemniteDeplacement(IndemniteDeplacementEnt indemniteDeplacementCalcul)
        {
            return calcul.GetCalculIndemniteDeplacement(indemniteDeplacementCalcul);

        }

        /// <inheritdoc />
        public IEnumerable<IndemniteDeplacementEnt> GetIndemniteDeplacementForExportKlm(int societeId)
        {
            return exportIndemnite.GetIndemniteDeplacementForExportKlm(societeId);
        }

        /// <inheritdoc />
        public IEnumerable<IndemniteDeplacementCalculTypeEnt> GetCalculTypes()
        {
            return indemniteDeplacementCalculTypeRepository.Get();
        }

        /// <inheritdoc />
        /// <summary>
        /// Indique s'il est possible de calculer les indemnités de déplacement.
        /// </summary>
        /// <param name="personnel">Le personnel concerné.</param>
        /// <param name="ci">Le Ci concerné.</param>
        /// <returns>True s'il est possible de faire le calcul, sinon false et la liste d'erreurs</returns>
        public Tuple<bool, List<string>> CanCalculateIndemniteDeplacement(PersonnelEnt personnel, CIEnt ci)
        {
            if (CrudWithCalculFeature.CanGetOrCreateIndemnite(personnel, ci))
            {
                return calcul.CanCalculateIndemniteDeplacement(personnel, ci);
            }

            return new Tuple<bool, List<string>>(false, new List<string>());
        }
    }
}
