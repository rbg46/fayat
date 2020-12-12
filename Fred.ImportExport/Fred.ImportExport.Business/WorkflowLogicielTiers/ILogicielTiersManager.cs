using Fred.ImportExport.Entities;

namespace Fred.ImportExport.Business.WorkflowLogicielTiers
{
    public interface ILogicielTiersManager
    {
        /// <summary>
        /// Retourne le logiciel ayant les informations passées en paramètre
        /// Si le logiciel n'existe pas déjà en base alors il est créé
        /// </summary>
        /// <param name="nomLogiciel">Nom du logiciel à récupérer</param>
        /// <param name="nomServeur">Serveur distant utilisé pour contacter le logiciel a récupérer</param>
        /// <param name="mandant">Mandant du logiciel</param>
        /// <returns>Retourne le logiciel demandé, jamais null</returns>
        LogicielTiersEnt GetOrCreateLogicielTiers(string nomLogiciel, string nomServeur, string mandant);
    }
}
