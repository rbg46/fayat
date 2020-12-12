using System;
using System.Collections.Generic;
using System.Linq;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities.IndemniteDeplacement;
using Fred.Framework.Extensions;

namespace Fred.Business.IndemniteDeplacement
{

    /// <inheritdoc />
    public class CrudFeature : ManagerFeature<IIndemniteDeplacementRepository>, ICrudFeature
    {

        private readonly IUtilisateurManager userManager;


        /// <summary>
        /// Instancie un nouvel object CrudFeature
        /// </summary>
        /// <param name="repository">r</param>
        /// <param name="userManager">u</param>
        /// <param name="uow"> Unit of work</param>
        public CrudFeature(IIndemniteDeplacementRepository repository, IUtilisateurManager userManager, IUnitOfWork uow)
          : base(uow, repository)
        {
            this.userManager = userManager;
        }



        /// <inheritdoc />
        public IndemniteDeplacementEnt GetNewIndemniteDeplacement()
        {
            return new IndemniteDeplacementEnt { SaisieManuelle = true };
        }


        /// <inheritdoc />
        public int AddIndemniteDeplacement(IndemniteDeplacementEnt indemniteDeplacement)
        {
            indemniteDeplacement.DateCreation = DateTime.UtcNow;
            indemniteDeplacement.AuteurCreation = this.userManager.GetContextUtilisateurId();

            int resu = Repository.AddIndemniteDeplacement(indemniteDeplacement);

            if (resu > -1)
            {
                indemniteDeplacement.IndemniteDeplacementId = resu;
            }

            return resu;
        }


        /// <inheritdoc />
        public void DeleteIndemniteDeplacementById(int id)
        {
            IndemniteDeplacementEnt indemniteDep = this.Repository.FindById(id);
            if (indemniteDep != null)
            {
                indemniteDep.DateSuppression = DateTime.UtcNow;
                indemniteDep.AuteurSuppression = this.userManager.GetContextUtilisateurId();
                this.UpdateIndemniteDeplacement(indemniteDep);
            }
        }


        /// <inheritdoc />
        public IEnumerable<IndemniteDeplacementEnt> GetIndemniteDeplacementByCi(int personnelID, int ciId)
        {
            return Repository.GetIndemniteDeplacementByCi(personnelID, ciId);
        }

        /// <inheritdoc />
        public IQueryable<IndemniteDeplacementEnt> GetIndemniteDeplacementByPersonnel(int personnelID)
        {
            return Repository.GetIndemniteDeplacementByPersonnel(personnelID);
        }

        /// <inheritdoc />
        public IndemniteDeplacementEnt GetIndemniteDeplacementByPersonnelIdAndCiId(int personnelId, int ciId)
        {
            return Repository.GetIndemniteDeplacementByPersonnelIdAndCiId(personnelId, ciId);
        }

        /// <inheritdoc />
        public IEnumerable<IndemniteDeplacementEnt> GetIndemniteDeplacementList()
        {
            return Repository.GetIndemniteDeplacementList();
        }

        /// <inheritdoc />
        public IEnumerable<IndemniteDeplacementEnt> GetIndemniteDeplacementListAll()
        {
            return Repository.GetIndemniteDeplacementListAll();
        }

        /// <inheritdoc />
        public IEnumerable<IndemniteDeplacementEnt> GetIndemniteDeplacementByPersonnelListAll(int personnelID)
        {
            return Repository.GetIndemniteDeplacementByPersonnelListAll(personnelID);
        }

        /// <inheritdoc />
        public IEnumerable<IndemniteDeplacementEnt> GetIndemniteDeplacementByPersonnelList(int personnelID)
        {
            return Repository.GetIndemniteDeplacementByPersonnelList(personnelID);
        }

        /// <inheritdoc />
        public IEnumerable<IndemniteDeplacementEnt> GetIndemniteDeplacementByCIListAll(int idCI)
        {
            return Repository.GetIndemniteDeplacementByCiListAll(idCI);
        }

        /// <inheritdoc />
        public IEnumerable<IndemniteDeplacementEnt> GetIndemniteDeplacementByCIList(int idCI)
        {
            return Repository.GetIndemniteDeplacementByCiList(idCI);
        }

        /// <inheritdoc />
        public int UpdateIndemniteDeplacement(IndemniteDeplacementEnt indemniteDeplacement)
        {
            return Repository.UpdateIndemniteDeplacement(indemniteDeplacement);
        }



        /// <inheritdoc />
        public int UpdateIndemniteDeplacementWithHistorical(IndemniteDeplacementEnt indDepNew, IndemniteDeplacementEnt indDepOld = null, bool toRemove = false)
        {
            if (indDepNew != null)
            {
                int auteurId = this.userManager.GetContextUtilisateurId();

                if (toRemove)
                {
                    indDepNew.DateSuppression = DateTime.UtcNow;
                    indDepNew.AuteurSuppression = auteurId;
                    return this.UpdateIndemniteDeplacement(indDepNew);
                }
                else
                {
                    if (indDepOld == null)
                    {
                        indDepOld = GetIndemniteDeplacementByPersonnelIdAndCiId(indDepNew.PersonnelId, indDepNew.CiId);
                    }

                    // Avant de continuer et de persister la nouvelle indé issue d'un recalcul, on va vérifier si le résultat du calcul est différent du précédent
                    // Si NON alors il est inutile de persister une indé identique sauf si celle-ci est une saisie manuelle.
                    if (!HasSamePropertiesValue(indDepOld, indDepNew))
                    {
                        if (indDepOld != null)
                        {
                            //Suppression logique de l'indé existante
                            indDepOld.DateSuppression = DateTime.UtcNow;
                            indDepOld.AuteurSuppression = auteurId;
                            UpdateIndemniteDeplacement(indDepOld);
                        }

                        //Ajout de la nouvelle indé          
                        return AddIndemniteDeplacement(new IndemniteDeplacementEnt()
                        {
                            CiId = indDepNew.CiId,
                            CodeDeplacementId = indDepNew.CodeDeplacementId,
                            CodeZoneDeplacementId = indDepNew.CodeZoneDeplacementId,
                            DateDernierCalcul = indDepNew.DateDernierCalcul,
                            IVD = indDepNew.IVD,
                            NombreKilometres = indDepNew.NombreKilometres,
                            NombreKilometreVOChantierRattachement = indDepNew.NombreKilometreVOChantierRattachement,
                            NombreKilometreVODomicileChantier = indDepNew.NombreKilometreVODomicileChantier,
                            NombreKilometreVODomicileRattachement = indDepNew.NombreKilometreVODomicileRattachement,
                            PersonnelId = indDepNew.PersonnelId,
                            SaisieManuelle = indDepNew.SaisieManuelle
                        }
                        );
                    }

                    indDepOld.DateModification = DateTime.UtcNow;
                    indDepOld.AuteurModification = auteurId;
                    indDepOld.DateDernierCalcul = indDepNew.DateDernierCalcul;
                    return UpdateIndemniteDeplacement(indDepOld);
                }
            }

            return -1;
        }


        /// <inheritdoc />
        public IndemniteDeplacementEnt GetIndemniteDeplacementById(int id)
        {
            return Repository.GetIndemniteDeplacementById(id);
        }

        /// <inheritdoc />
        public IEnumerable<IndemniteDeplacementEnt> GetIndemniteDeplacementByPersonnelId(int personnelId)
        {
            return Repository.GetIndemniteDeplacementByPersonnelId(personnelId);
        }





        /// <inheritdoc />
        public int ImportIndemniteDeplacementFromHolding(int holdingId)
        {
            return Repository.ImportIndemniteDeplacementFromHolding(holdingId);
        }



        /// <inheritdoc />
        public bool IsIndemniteDeplacementUnique(int personnelId, int ciId, int indId = 0)
        {
            var existings = Repository.GetIndemniteDeplacementByPersonnelList(personnelId);

            if (existings.Any())
            {
                var forOurCi = existings.Where(e => e.CiId == ciId).ToList();

                int count = forOurCi.Count;

                if (count == 0)
                {
                    return true;
                }

                if (count == 1 && forOurCi[0].IndemniteDeplacementId == indId)
                {
                    return true;
                }

                return false;
            }

            return true;
        }




        /// <summary>
        ///   Comparaison de valeurs de propriétés de l'objet Indémnité de déplacement
        /// </summary>
        /// <param name="indA">Indemnite A</param>
        /// <param name="indB">Indemnite B</param>
        /// <returns>return true si les properties sont identiques</returns>    
        private bool HasSamePropertiesValue(IndemniteDeplacementEnt indA, IndemniteDeplacementEnt indB)
        {
            return indA != null && indB != null &&
                   indA.CiId == indB.CiId &&
                   indA.NombreKilometres.IsEqual(indB.NombreKilometres)
                   && indA.NombreKilometreVOChantierRattachement.IsEqual(indB.NombreKilometreVOChantierRattachement) &&
                   indA.NombreKilometreVODomicileChantier.IsEqual(indB.NombreKilometreVODomicileChantier) &&
                   indA.NombreKilometreVODomicileRattachement.IsEqual(indB.NombreKilometreVODomicileRattachement) &&
                   indA.IVD == indB.IVD &&
                   indA.CodeDeplacementId == indB.CodeDeplacementId &&
                   indA.CodeZoneDeplacementId == indB.CodeZoneDeplacementId &&
                   indA.SaisieManuelle == indB.SaisieManuelle;
        }
    }
}
