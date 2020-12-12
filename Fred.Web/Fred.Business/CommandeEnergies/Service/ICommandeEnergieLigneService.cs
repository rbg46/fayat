using System;
using System.Collections.Generic;
using Fred.Entities.Referential;

namespace Fred.Business.CommandeEnergies
{
    /// <summary>
    /// Interface Service Commande Ligne Energie
    /// </summary>
    public interface ICommandeEnergieLigneService : IService
    {
        /// <summary>
        /// Génération des lignes de commande énergie pour un commande énergie en fonction du type d'énergie
        /// </summary>
        /// <param name="typeEnergie">Type d'énergie</param>
        /// <param name="ciId">Identifiant du CI SEP</param>
        /// <param name="periode">Période sélectionnée</param>        
        /// <param name="tache">Tache par défaut : 999998</param>
        /// <param name="unite">Unite par défaut : H</param>
        /// <param name="societeParticipanteIds">Liste d'identifiants de sociétés participantes à la SEP</param>
        /// <returns>Liste de lignes de commande</returns>
        List<CommandeEnergieLigne> GetGeneratedCommandeEnergieLignes(TypeEnergieEnt typeEnergie, int ciId, DateTime periode, TacheEnt tache, UniteEnt unite, List<int> societeParticipanteIds);

        /// <summary>
        /// Champs calculés
        /// </summary>
        /// <param name="typeEnergie">Type d'énergie</param>
        /// <param name="commandeEnergieLignes">Ligne Commande énergie en BD</param>
        /// <param name="generatedCommandeEnergieLignes">Liste des lignes énergie générés en fonction des pointages</param>
        void ComputeCalculatedFields(TypeEnergieEnt typeEnergie, List<CommandeEnergieLigne> commandeEnergieLignes, List<CommandeEnergieLigne> generatedCommandeEnergieLignes);
    }
}
