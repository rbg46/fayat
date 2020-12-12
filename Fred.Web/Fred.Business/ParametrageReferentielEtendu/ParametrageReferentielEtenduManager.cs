using System;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities.ReferentielEtendu;

namespace Fred.Business
{
    /// <summary>
    ///   Gestionnaire des ParametrageReferentielEtendus
    /// </summary>
    public class ParametrageReferentielEtenduManager : Manager<ParametrageReferentielEtenduEnt, IParametrageReferentielEtenduRepository>, IParametrageReferentielEtenduManager
    {
        private readonly IParametrageReferentielEtenduRepository parametrageReferentielEtenduRepository;
        private readonly IUtilisateurManager utilisateurManager;

        public ParametrageReferentielEtenduManager(
            IUnitOfWork uow,
            IParametrageReferentielEtenduRepository parametrageReferentielEtenduRepository,
            IUtilisateurManager utilisateurManager)
          : base(uow, parametrageReferentielEtenduRepository)
        {
            this.parametrageReferentielEtenduRepository = parametrageReferentielEtenduRepository;
            this.utilisateurManager = utilisateurManager;
        }

        /// <summary>
        ///   Supprime un ParametrageReferentielEtendu
        /// </summary>
        /// <param name="parametrageReferentielEtenduId">ID du ParametrageReferentielEtendu à supprimé</param>
        public void DeleteById(int parametrageReferentielEtenduId)
        {
            Repository.DeleteById(parametrageReferentielEtenduId);
            Save();
        }

        /// <summary>
        ///   Ajoute ou met à jour un nouveau ParametrageReferentielEtenduEnt
        /// </summary>
        /// <param name="parametrageReferentielEtendu"> Paramétrage à ajouter ou mettre à jour </param>
        /// <returns> ParametrageReferentielEtenduEnt ajouté ou mis à jour </returns>
        public ParametrageReferentielEtenduEnt AddOrUpdateParametrageReferentielEtendu(ParametrageReferentielEtenduEnt parametrageReferentielEtendu)
        {
            var auteurId = this.utilisateurManager.GetContextUtilisateurId();

            if (parametrageReferentielEtendu.ParametrageReferentielEtenduId == 0)
            {
                parametrageReferentielEtendu.AuteurCreationId = auteurId;
                parametrageReferentielEtendu.DateCreation = DateTime.Now;
                parametrageReferentielEtenduRepository.InsertParametrageReferentielEtendu(parametrageReferentielEtendu);
                Save();
            }
            else
            {
                parametrageReferentielEtendu.AuteurModificationId = auteurId;
                parametrageReferentielEtendu.DateModification = DateTime.Now;
                parametrageReferentielEtenduRepository.UpdateParametrageReferentielEtendu(parametrageReferentielEtendu);
                Save();
            }

            return parametrageReferentielEtendu;
        }
    }
}
