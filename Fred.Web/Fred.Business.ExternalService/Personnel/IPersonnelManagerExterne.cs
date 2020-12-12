using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Entities.Personnel;
using Fred.Web.Models.Societe;

namespace Fred.Business.ExternalService.Personnel
{
    /// <summary>
    /// Manager qui gere les interaction avec fred ie pour les Personnels
    /// </summary>
    public interface IPersonnelManagerExterne : IService
    {
        /// <summary>
        /// Action qui est executée lors d'une mise a jour d'un personnel depuis l'interface utilisateur
        /// Ici, je passe en parametre une Func de PersonnelEnt car je ne voulais pas modifié le code existant 
        /// et execute le code apres avoir recupere le ci avant l'action de mise a jour
        /// </summary>
        /// <param name="personnelId">L'id du personnel</param>
        /// <param name="updateAction">Action de mise a jour cote fred</param>
        /// <returns>Le personnel mis a jour</returns>
        Task<PersonnelEnt> OnUpdatePersonnelAsync(int personnelId, Func<PersonnelEnt> updateAction);

        /// <summary>
        /// Export des réceptions Intérimaires
        /// </summary>
        /// <param name="societes">Liste des sociétés</param>
        Task ExportReceptionInterimairesAsync(List<SocieteModel> societes);
    }
}
