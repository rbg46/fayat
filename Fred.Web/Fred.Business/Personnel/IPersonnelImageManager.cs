using System.Drawing;
using Fred.Entities;

namespace Fred.Business.Personnel
{
    /// <summary>
    ///   Gestionnaire des images du personnel.
    /// </summary>
    public interface IPersonnelImageManager : IManager<PersonnelImageEnt>
    {
        /// <summary>
        /// Permet de récupérer la signature d'un personnel
        /// </summary>
        /// <param name="personnelId">L'identifiant du personnel</param>
        /// <param name="width">Largeur</param>
        /// <param name="height">Hauteur</param>
        /// <returns>L'image de la signature</returns>
        byte[] GetSignature(int personnelId, int? width, int? height);

        /// <summary>
        ///   Récupération de l'entité contenant les images d'un personnel (signature et photo de profil)
        /// </summary>
        /// <param name="personnelId">Identifiant du personnel</param>
        /// <returns>Entité PersonnelImage</returns>
        PersonnelImageEnt GetPersonnelImage(int personnelId);

        /// <summary>
        ///   Ajout ou modification des images du personnel
        /// </summary>
        /// <param name="persoImage">PersonnelImage</param>
        /// <returns>Entité persoImage crée ou mise à jour</returns>
        PersonnelImageEnt AddOrUpdatePersonnelImage(PersonnelImageEnt persoImage);
    }
}