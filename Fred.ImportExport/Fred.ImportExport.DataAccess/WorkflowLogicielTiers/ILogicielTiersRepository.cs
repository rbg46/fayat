using System.Collections.Generic;
using Fred.ImportExport.Entities;

namespace Fred.ImportExport.DataAccess.WorkflowLogicielTiers
{
    public interface ILogicielTiersRepository
    {
        /// <summary>
        /// Retourne le logiciel dont le nom est la version est passée en paramètre
        /// </summary>
        /// <param name="nomLogiciel">Nom du logiciel à récupérer</param>
        /// <param name="nomServeur">Serveur distant utilisé pour contacter le logiciel a récupérer</param>
        /// <param name="mandant">Mandant du logiciel</param>
        /// <returns>Un logiciel, potentiellement null</returns>
        LogicielTiersEnt GetLogicielTiers(string nomLogiciel, string nomServeur, string mandant);

        /// <summary>
        /// Créé le logiciel
        /// </summary>
        /// <param name="nomLogiciel">Nom du logiciel</param>
        /// <param name="nomServeur">Serveur distant utilisé pour contacter le logiciel</param>
        /// <param name="mandant">Mandant du logiciel</param>
        /// <returns>le logiciel nouvellement créé</returns>
        LogicielTiersEnt CreateLogicielTiers(string nomLogiciel, string nomServeur, string mandant);
    }
}
