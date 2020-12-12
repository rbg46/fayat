using System.Collections.Generic;
using Fred.Entities.CI;
using Fred.Entities.Organisation.Tree;
using Fred.Entities.Personnel;
using Fred.Entities.Societe;
using Fred.ImportExport.Business.CI.WebApi.Context.Inputs;

namespace Fred.ImportExport.Business.CI.WebApi.Context.Models
{
    public class ImportCiByWebApiContext
    {
        /// <summary>
        /// Parametre d'entrée (liste de cis)
        /// </summary>
        public ImportCisByApiInputs Input { get; set; }
        /// <summary>
        /// L'arbre de fred
        /// </summary>
        public OrganisationTree OrganisationTree { get; internal set; }
        /// <summary>
        /// Societés necessaires a l'import
        /// </summary>
        public List<SocieteEnt> SocietesUsedInJson { get; internal set; } = new List<SocieteEnt>();

        /// <summary>
        /// Context par societe qui doit contenir les données necessaires a l'imports des ci
        /// </summary>
        public List<ImportCiByWebApiSocieteContext> SocietesContexts { get; internal set; } = new List<ImportCiByWebApiSocieteContext>();
        /// <summary>
        /// Les type de cis
        /// </summary>
        public List<CITypeEnt> CiTypes { get; internal set; } = new List<CITypeEnt>();

        /// <summary>
        /// listes des responsables affaire dans le json
        /// </summary>
        public List<PersonnelEnt> ResponsableAffairesUsedInJson { get; internal set; }

        /// <summary>
        /// Societe des responsable affaires
        /// </summary>
        public List<SocieteEnt> SocietesOfResponsableAffairesUsedInJson { get; internal set; }

    }
}
