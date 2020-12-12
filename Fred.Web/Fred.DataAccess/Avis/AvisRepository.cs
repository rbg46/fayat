using System;
using Fred.DataAccess.Common;
using Fred.Entities.Avis;
using Fred.EntityFramework;

namespace Fred.DataAccess.Avis
{
    /// <summary>
    /// Repository des avis
    /// </summary>
    public class AvisRepository : FredRepository<AvisEnt>, IAvisRepository
    {
        public AvisRepository(FredDbContext context)
          : base(context)
        {

        }

        /// <summary>
        /// Ajouter un avis
        /// </summary>
        /// <param name="avis">Avis à ajouter</param>
        /// <returns>Avis ajouté (attaché)</returns>
        public AvisEnt Add(AvisEnt avis)
        {
            // Affectation des champs de suivi
            avis.AuteurCreationId = avis.ExpediteurId;
            avis.AuteurModificationId = avis.ExpediteurId;
            avis.DateCreation = DateTime.UtcNow;
            avis.DateModification = avis.DateCreation;

            // Afin de ne pas attacher l'objet en paramètre
            AvisEnt avisToAdd = new AvisEnt()
            {
                AuteurCreationId = avis.AuteurCreationId,
                AuteurModificationId = avis.AuteurModificationId,
                Commentaire = avis.Commentaire,
                DateCreation = avis.DateCreation,
                DateModification = avis.DateModification,
                DestinataireId = avis.DestinataireId,
                ExpediteurId = avis.ExpediteurId,
                TypeAvis = avis.TypeAvis
            };

            // Retourner l'object attaché
            Context.Avis.Add(avisToAdd);

            return avisToAdd;
        }
    }
}
