using Fred.Business.Images;
using Fred.Business.Personnel;
using Fred.Business.Utilisateur;
using Fred.Entities.Commande;
using Fred.Entities.Societe;
using Fred.Framework.Export.Order.Models;

namespace Fred.Business.Commande.Reporting
{
    /// <summary>
    ///   Service de gestion de l'export pdf d'une commande
    /// </summary>
    public interface ICommandeExportService
    {
        /// <summary>
        ///   Converti une commande au format PDF selon le template
        /// </summary>
        /// <param name="commande">Commande</param>
        /// <param name="exportOptions">Options d'export</param>
        /// <param name="imageManager">Manager des images</param>
        /// <returns>tableau de byte</returns>
        byte[] ToPdf(CommandeExportModel commande, CommandeExportOptions exportOptions, IImageManager imageManager, IUtilisateurManager userManager);

        /// <summary>
        ///   Convertit un objet CommandeEnt en CommandeExportModel
        /// </summary>
        /// <param name="commande">CommandeEnt</param>
        /// <param name="societe">Societe du CI</param>
        /// <param name="personnelImageManager">Gestionnaire des personnels</param>
        /// <param name="imageManager">Manager des images</param>
        /// <returns>CommandeExportModel</returns>
        CommandeExportModel Convert(CommandeEnt commande, SocieteEnt societe, IPersonnelImageManager personnelImageManager, IImageManager imageManager, IUtilisateurManager userManager);
    }
}
