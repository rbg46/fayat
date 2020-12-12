using Fred.DataAccess.Interfaces;
using Fred.Entities.CI;
using Fred.Entities.IndemniteDeplacement;
using Fred.Entities.Personnel;

namespace Fred.Business.IndemniteDeplacement
{
    /// <inheritdoc />
    public class CrudWithCalculFeature : ManagerFeature<IIndemniteDeplacementRepository>, ICrudWithCalculFeature
    {

        /// <summary>
        /// Instancie un nouvel object CrudWithCalculFeature
        /// </summary>
        /// <param name="calculFeature">u</param>
        /// <param name="repository">r</param>
        /// <param name="crudFeature">c</param>
        /// <param name="uow"> Unit of work</param>
        public CrudWithCalculFeature(ICalculFeature calculFeature, ICrudFeature crudFeature, IIndemniteDeplacementRepository repository, IUnitOfWork uow)
          : base(uow, repository)
        {
            Calcul = calculFeature;
            Crud = crudFeature;
        }

        private ICalculFeature Calcul { get; }
        private ICrudFeature Crud { get; }

        /// <inheritdoc />
        public IndemniteDeplacementEnt GetOrCreateIndemniteDeplacementByPersonnelAndCi(PersonnelEnt personnel, CIEnt ci, bool refresh)
        {
            // Ce code est pourri, il faut tout refaire, j'ai fais au plus simple, si quelqu'un a le courage de tout reprendre à zero...

            // Il faut que le personnel appartienne à un établissement de paie qui est paramétré dans la gestion des déplacements RZB
            if (!CanGetOrCreateIndemnite(personnel, ci))
            {
                return null;
            }

            if (refresh)
            {
                return RefreshIndemniteDeplacement(personnel, ci);
            }
            else
            {
                return GetOrCreateIndemniteDeplacement(personnel, ci);
            }
        }

        private IndemniteDeplacementEnt GetOrCreateIndemniteDeplacement(PersonnelEnt personnel, CIEnt ci)
        {
            // TFS 6842 : Si l'indemnité de déplacement n'existe pas alors on la crée sinon on retourne l'indémnité existante
            // Voir aussi US 6876 
            // Récupération de l'indémnité existante (si elle existe) et non supprimée logiquement.
            var indDepFromDb = Crud.GetIndemniteDeplacementByPersonnelIdAndCiId(personnel.PersonnelId, ci.CiId);
            if (indDepFromDb != null)
            {
                return indDepFromDb;
            }
            else
            {
                var indDepNew = Calcul.CalculIndemniteDeplacement(personnel, ci);
                if (IsCalculCreated(indDepNew) && IsCalculCoherent(indDepNew))
                {
                    //Si le résultat du calcul a donné un résultat cohérent alors on peut continuer
                    //Pour information, on crée ici une indé. de déplacement en base avec indication sur la saisie manuelle
                    indDepNew.SaisieManuelle = false;
                    Crud.AddIndemniteDeplacement(indDepNew);
                }
                return indDepNew;
            }

        }

        private IndemniteDeplacementEnt RefreshIndemniteDeplacement(PersonnelEnt personnel, CIEnt ci)
        {
            // Ce code est pourri, il faut tout refaire, j'ai fais au plus simple, si quelqu'un a le courage de tout reprendre à zero...

            // Voir US 6876 
            // Récupération de l'indémnité existante (si elle existe) et non supprimée logiquement.
            var indDepFromDb = Crud.GetIndemniteDeplacementByPersonnelIdAndCiId(personnel.PersonnelId, ci.CiId);

            bool exist = indDepFromDb != null;

            //Si l'indémnité de déplacement n'existe pas alors on la crée
            //OU si elle existe mais qu'il ne s'agit pas d'une saisie manuelle alors on la recalcule
            //SINON on retourne l'indémnité issue de la base de donnée
            if ((exist && !indDepFromDb.SaisieManuelle) || !exist)
            {
                var indDepNew = Calcul.CalculIndemniteDeplacement(personnel, ci);

                if (IsCalculCreated(indDepNew))
                {
                    //Si le résultat du calcul a donné un résultat cohérent alors on peut continuer
                    if (IsCalculCoherent(indDepNew))
                    {
                        //Pour information, on crée ici une indé. de déplacement en base avec indication sur la saisie manuelle
                        indDepNew.SaisieManuelle = false;
                        if (!exist)
                        {
                            Crud.AddIndemniteDeplacement(indDepNew);
                        }
                        else
                        {
                            Crud.UpdateIndemniteDeplacementWithHistorical(indDepNew, indDepFromDb);
                        }
                    }
                    //Si l'indémnité de déplacement existe mais que le résultat du recalcul n'est pas cohérent alors on la supprime logiquement
                    else
                    {
                        Crud.UpdateIndemniteDeplacementWithHistorical(indDepFromDb, toRemove: true);
                    }
                }

                return indDepNew;
            }

            return indDepFromDb;
        }


        /// <summary>
        /// Détermine si une indemnité de déplacement a été calculé/créé
        /// </summary>
        /// <param name="indDepNew">indemnité à vérifier</param>
        /// <returns>true si oui, sinon false</returns>
        private bool IsCalculCreated(IndemniteDeplacementEnt indDepNew)
        {
            return indDepNew != null && indDepNew.PersonnelId != 0 && indDepNew.CiId != 0;
        }


        /// <summary>
        /// Déterminie si un calcul a bien été effectué
        /// </summary>
        /// <param name="indDepNew">indemnité à vérifier</param>
        /// <returns>true si oui, sinon false</returns>
        private bool IsCalculCoherent(IndemniteDeplacementEnt indDepNew)
        {
            return indDepNew.CodeDeplacementId.HasValue || indDepNew.CodeZoneDeplacementId.HasValue;
        }


        /// <summary>
        /// Détermine s'il est possible de créer une indemnité
        /// </summary>
        /// <param name="personnel">Le personnel pour qui créer l'indemnité</param>
        /// <param name="ci">le ci</param>
        /// <returns>true si c'est possible, sinon false</returns>
        public static bool CanGetOrCreateIndemnite(PersonnelEnt personnel, CIEnt ci)
        {
            return ci != null && personnel != null && !string.IsNullOrEmpty(personnel.Societe.CodeSocietePaye) && personnel.IsInterne && !personnel.IsInterimaire;
        }
    }
}
