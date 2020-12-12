using System.Collections.Generic;
using Fred.Entities.CI;
using Fred.Entities.Commande;
using Fred.Entities.Depense;
using Fred.Entities.Referential;
using Fred.Entities.Valorisation;

namespace Fred.Business.CommandeEnergies
{
    /// <summary>
    /// Service mapper des commandes énergies
    /// </summary>
    public interface ICommandeEnergieMapperService : IService
    {
        /// <summary>
        /// Transforme une liste de pointages personnel en liste de ligne de commandes
        /// </summary>
        /// <param name="pointages">Liste de pointages personnel</param>
        /// <param name="ci">CI</param>
        /// <param name="tache">Tache</param>
        /// <param name="unite">Unite</param>
        /// <returns>Liste de Commande Energlie Ligne</returns>
        List<CommandeEnergieLigne> RapportLignePersonnelToCommandeEnergieLigne(List<PointagePersonnelCommandeEnergie> pointages, CIEnt ci, TacheEnt tache, UniteEnt unite);

        /// <summary>
        /// Transforme une liste de pointages matériel en liste de ligne de commandes
        /// </summary>
        /// <param name="pointages">Liste de pointages matériel</param>
        /// <param name="ci">CI</param>
        /// <param name="tache">Tache</param>
        /// <param name="unite">Unite</param>
        /// <returns>Liste de Commande Energlie Ligne</returns>
        List<CommandeEnergieLigne> RapportLigneMaterielToCommandeEnergieLigne(List<PointageMaterielCommandeEnergie> pointages, CIEnt ci, TacheEnt tache, UniteEnt unite);

        /// <summary>
        /// Création des réceptions à partir des lignes de commandes
        /// </summary>
        /// <param name="lignes">Liste de lignes de commandes énergie</param>
        /// <param name="utilisateurId">Identifiant utilisateur</param>
        /// <param name="depenseTypeReceptionId">Identifiant type réception</param>
        /// <returns>Liste de dépenses achats</returns>
        List<DepenseAchatEnt> CommandeLigneEntToDepenseAchatEnt(List<CommandeLigneEnt> lignes, int utilisateurId, int depenseTypeReceptionId);

        /// <summary>
        /// Mapping CommandeLigneEnt vers ValorisationEnt
        /// </summary>
        /// <param name="lignes">Lignes de commande</param>
        /// <param name="tache">Tache système</param>
        /// <returns>Liste de valorisations</returns>
        List<ValorisationEnt> CommandeLigneEntToValorisationEnt(List<CommandeLigneEnt> lignes, TacheEnt tache);
    }
}
