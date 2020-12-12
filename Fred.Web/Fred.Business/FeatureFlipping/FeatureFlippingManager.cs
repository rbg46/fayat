using System;
using System.Collections.Generic;
using Fred.Business.FeatureFlipping.Validators;
using Fred.Business.Utilisateur;
using Fred.DataAccess.Interfaces;
using Fred.Entities.FeatureFlipping;
using Fred.Framework.Extensions;
using Fred.Framework.FeatureFlipping;

namespace Fred.Business.FeatureFlipping
{
    /// <summary>
    /// Gestionnaire des FeatureFlipping
    /// </summary>
    public class FeatureFlippingManager : Manager<FeatureFlippingEnt, IFeatureFlippingRepository>, IFeatureFlippingManager
    {
        private readonly IUtilisateurManager utilisateurManager;

        public FeatureFlippingManager(
            IUnitOfWork uow,
            IFeatureFlippingRepository featureFlippingRepository,
            IFeatureFlippingValidator featureFlippingValidator,
            IUtilisateurManager utilisateurManager)
          : base(uow, featureFlippingRepository, featureFlippingValidator)
        {
            this.utilisateurManager = utilisateurManager;
        }

        /// <summary>
        /// Retourne les features flippings présentent en base
        /// </summary>
        /// <returns>Listes des features flippings</returns>
        public IEnumerable<FeatureFlippingEnt> GetFeatureFlippings()
        {
            return Repository.GetFeatureFlippings();
        }

        /// <summary>
        /// Mets à jour une FeatureFlipping en inversant son état.
        /// </summary>
        /// <param name="item">FeatureFliping à mettre à jour</param>
        /// <returns>La feature</returns>
        public FeatureFlippingEnt Update(FeatureFlippingEnt item)
        {
            item.DateActivation = DateTime.UtcNow;
            item.UserActivation = this.utilisateurManager.GetContextUtilisateur().Login;
            item.IsActived = !item.IsActived;
            BusinessValidation(item);
            // Database
            Repository.UpdateFeatureFlipping(item);
            Save();
            return item;
        }

        /// <summary>
        /// Indique si une feature est activée ou non
        /// </summary>
        /// <param name="code">Identifiant de la feature></param>
        /// <returns>Retourne true si la feature est activée, sinon false</returns>
        public bool IsActivated(EnumFeatureFlipping code)
        {
            FeatureFlippingEnt feature = Repository.GetFeatureFlipping(code.ToIntValue());
            return feature != null ? feature.IsActived : false;
        }
    }
}
