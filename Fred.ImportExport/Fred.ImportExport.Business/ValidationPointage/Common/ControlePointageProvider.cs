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
    /// Provider qui fournie les données relative au controle du pointage
    /// </summary>
    public class ControlePointageProvider : IControlePointageProvider
    {
        private readonly IUtilisateurManager utilisateurManager;
        private readonly IControlePointageManager controlePointageManager;

        public ControlePointageProvider(IUtilisateurManager utilisateurManager, IControlePointageManager controlePointageManager)
        {
            this.utilisateurManager = utilisateurManager;
            this.controlePointageManager = controlePointageManager;
        }

        /// <summary>
        ///   Création du controlePointageEnt
        /// </summary>
        /// <param name="utilisateurId">Identifiant de l'utilisateur exécutant le contrôle</param>
        /// <param name="lotPointageId">Identifiant du lot de pointage contrôlé</param>
        /// <param name="typeControle">Type de contrôle</param>
        /// <returns>ControlePointageEnt</returns>
        public ControlePointageEnt CreateControlePointage(int utilisateurId, int lotPointageId, int typeControle)
        {
            ControlePointageEnt ctrlPointage = new ControlePointageEnt
            {
                DateDebut = DateTime.UtcNow,
                AuteurCreationId = utilisateurId,
                LotPointageId = lotPointageId,
                Statut = FluxStatus.InProgress.ToIntValue(),
                TypeControle = typeControle,
                Erreurs = new List<ControlePointageErreurEnt>()
            };

            this.controlePointageManager.AddControlePointage(ctrlPointage);
            ctrlPointage.AuteurCreation = this.utilisateurManager.GetById(utilisateurId);

            return ctrlPointage;
        }
    }
}
