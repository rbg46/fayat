using System;
using System.Collections.Generic;
using Fred.Business.Utilisateur;
using Fred.Business.ValidationPointage;
using Fred.Entities;
using Fred.Entities.ValidationPointage;
using Fred.Framework.Extensions;

namespace Fred.ImportExport.Business.ValidationPointage.Common
{
    /// <summary>
    /// Provider qui fournie les données relative a la remontée vrac
    /// </summary>
    public class RemonteeVracProvider : IRemonteeVracProvider
    {
        private readonly IUtilisateurManager utilisateurManager;
        private readonly IRemonteeVracManager remonteeVracManager;

        public RemonteeVracProvider(IUtilisateurManager utilisateurManager, IRemonteeVracManager remonteeVracManager)
        {
            this.utilisateurManager = utilisateurManager;
            this.remonteeVracManager = remonteeVracManager;
        }

        /// <summary>
        ///   Création d'une RemonteeVracEnt
        /// </summary>
        /// <param name="utilisateurId">Identifiant de l'utilisateur exécutant le contrôle</param>
        /// <param name="periode">Période choisie</param>    
        /// <returns>ControlePointageEnt</returns>
        public RemonteeVracEnt CreateRemonteeVrac(int utilisateurId, DateTime periode)
        {
            var remonteeVrac = new RemonteeVracEnt
            {
                AuteurCreationId = utilisateurId,
                DateDebut = DateTime.UtcNow,
                Periode = periode,
                Statut = FluxStatus.InProgress.ToIntValue(),
                Erreurs = new List<RemonteeVracErreurEnt>()
            };
            this.remonteeVracManager.AddRemonteeVrac(remonteeVrac);
            remonteeVrac.AuteurCreation = this.utilisateurManager.GetById(utilisateurId);

            return remonteeVrac;
        }
    }
}
