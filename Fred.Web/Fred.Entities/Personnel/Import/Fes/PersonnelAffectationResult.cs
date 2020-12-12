using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Fred.Entities.Affectation;
using Fred.Entities.Personnel.Imports;

namespace Fred.Entities.Personnel.Import
{
    /// <summary>
    /// Resultat d'un transformation d'import de personnel
    /// </summary>
    [DebuggerDisplay("Affectation = {Affectation?.AffectationId} Personnel = {Personnel?.PersonnelId} ManagerHasChanged = {ManagerHasChanged}")]
    public class PersonnelAffectationResult
    {
        /// <summary>
        /// Personnel Correspondant au model anael
        /// </summary>
        public PersonnelEnt Personnel { get; set; }

        /// <summary>
        /// Model anael
        /// </summary>
        public PersonnelModel PersonnelModel { get; set; }

        /// <summary>
        /// Indique si le personnel est nouveau ou modifié
        /// </summary>
        public bool PersonnelIsNewOrModified { get; set; } = false;

        /// <summary>
        /// Indique si le personnel a un manager
        /// </summary>
        public bool HasManager
        {
            get
            {
                return Personnel.ManagerId != null;
            }
        }

        /// <summary>
        /// Indique si le manager n'existe pas dans Fred
        /// </summary>
        public bool ManagerIsNotCreatedInFred { get; set; }

        /// <summary>
        /// Manager du personnel
        /// </summary>
        public PersonnelEnt Manager { get; set; }

        /// <summary>
        /// Affectation correspondant au ci - personnel
        /// </summary>
        public List<AffectationEnt> Affectations { get; set; }

        /// <summary>
        /// Indique si l'affectation est nouvelle ou modifiée
        /// </summary>
        public bool AffectationIsNewOrModified { get; set; } = false;

        /// <summary>
        /// Permet de savoir si le manager ne peux pas etre affecté aau personnel
        /// </summary>
        public bool ManagerCanNotBeProcessed { get; set; }

        /// <summary>
        /// Indique si le personnel est nouveau 
        /// </summary>
        public bool PersonnelIsNew { get; set; }
        /// <summary>
        /// Indique si le personnel est  existant et  modifié 
        /// </summary>
        public bool PersonnelIsModified { get; set; }
        /// <summary>
        /// Indique si le personnel est existant et non modifié
        /// </summary>
        public bool PersonnelIsNotModified { get; set; }

        /// <summary>
        /// Fait le nettoyage necessaire avant la sauvegarde dans fred
        /// </summary>
        public void ManageAffectationBeforeInsertionInFred()
        {
            foreach (AffectationEnt affectation in Affectations)
            {
                if (affectation.AffectationIsNewOrModified)
                {
                    affectation.PersonnelId = this.Personnel.PersonnelId;
                    affectation.Personnel = null;
                }
            }
        }

        /// <summary>
        /// Verifie que l'on peux bien créer une affectation dans Fred
        /// </summary>
        /// <returns>vrai ou faux</returns>
        public IEnumerable<AffectationEnt> AffectationCanBeInsertedInFred()
        {
            return this.Affectations.Where(x => x.AffectationIsNewOrModified && x.PersonnelId > 0 && x.CiId > 0).ToList();
        }
    }
}
