using System.Collections.Generic;
using Fred.Entities.Rapport;

namespace Fred.Business.Moyen.Common
{
    /// <summary>
    /// Process pointage moyen result
    /// </summary>
    internal class ProccesPointageMoyenResult
    {
        /// <summary>
        /// Process pointage moyen result
        /// </summary>
        /// <param name="rapportLigneEntsToCreate">Les lignes de rapport à créer</param>
        /// <param name="rapportLigneEntsToUpdate">Les lignes de rapport à modifier</param>
        /// <param name="personnelPointageErrors">Les pointages personnels manquants</param>
        internal ProccesPointageMoyenResult(
            IEnumerable<RapportLigneEnt> rapportLigneEntsToCreate,
            IEnumerable<RapportLigneEnt> rapportLigneEntsToUpdate,
            IEnumerable<PersonnelPointageError> personnelPointageErrors
            )
        {
            RapportLigneEntsToCreate = rapportLigneEntsToCreate;
            RapportLigneEntsToUpdate = rapportLigneEntsToUpdate;
            PersonnelPointageErrors = personnelPointageErrors;
        }

        /// <summary>
        /// Les lignes de rapports à créer
        /// </summary>
        internal IEnumerable<RapportLigneEnt> RapportLigneEntsToCreate { get; set; }

        /// <summary>
        /// Les ligne des rapport à modifier
        /// </summary>
        internal IEnumerable<RapportLigneEnt> RapportLigneEntsToUpdate { get; set; }

        /// <summary>
        /// les erreurs de pointage personnels
        /// </summary>
        internal IEnumerable<PersonnelPointageError> PersonnelPointageErrors { get; set; }
    }
}
