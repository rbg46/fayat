using System.Collections.Generic;
using System.Diagnostics;
using Fred.Entities;
using Fred.Entities.Organisation.Tree;
using Fred.Entities.Referential;
using Fred.Entities.Societe;

namespace Fred.ImportExport.Business.Fournisseur.AnaelSystem.Context.Common
{
    /// <summary>
    /// Context qui doit contenir les données necessaires a l'imports des ci
    /// </summary>
    /// <typeparam name="T">Type de parametre d'entrée</typeparam>
    [DebuggerDisplay("SocietesNeeded = {SocietesNeeded.Count} SocietesContexts = {SocietesContexts.Count} Responsables = {Responsables.Count} SocietesOfResponsables = {SocietesOfResponsables.Count} CiPays = {CiPays.Count}")]
    public class ImportFournisseurContext<T> where T : class
    {
        /// <summary>
        /// Parametre d'entrée qui vzrie en fonction du type d'import (excel, liste de ci ou societe)
        /// </summary>
        public T Input { get; set; }
        /// <summary>
        /// L'arbre de fred
        /// </summary>
        public OrganisationTree OrganisationTree { get; internal set; }
        /// <summary>
        /// Societés necessaires a l'import
        /// </summary>
        public List<SocieteEnt> SocietesNeeded { get; internal set; } = new List<SocieteEnt>();
        /// <summary>
        /// Context pour une societe qui doit contenir les données necessaires a l'imports des ci
        /// </summary>
        public List<ImportFournisseurSocieteContext> SocietesContexts { get; internal set; } = new List<ImportFournisseurSocieteContext>();
        /// <summary>
        /// Les pays des fournisseur necessaires pour l'envoie des infos a SAP
        /// </summary>
        public List<PaysEnt> FournisseurPays { get; internal set; } = new List<PaysEnt>();
        /// <summary>
        /// Le logger
        /// </summary>
        public FournisseurImportExportLogger Logger { get; internal set; }

        /// <summary>
        /// Liste des type de societe, permettra de savoir si la societe est une sep et donc de savoir si on doit envoyé les ci de la societe a sap
        /// </summary>
        public List<TypeSocieteEnt> TypeSocietes { get; internal set; }

        /// <summary>
        /// Listes des codes sequences
        /// </summary>
        public List<string> TypeSequences { get; set; }
    }
}
