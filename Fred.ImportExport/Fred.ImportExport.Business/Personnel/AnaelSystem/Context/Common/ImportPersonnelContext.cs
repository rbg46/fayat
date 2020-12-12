using System.Collections.Generic;
using System.Diagnostics;
using Fred.Entities;
using Fred.Entities.Referential;
using Fred.Entities.ReferentielFixe;
using Fred.Entities.Societe;
using Fred.ImportExport.Business.Personnel.AnaelSystem.Logger;

namespace Fred.ImportExport.Business.Personnel.AnaelSystem.Context.Common
{
    /// <summary>
    /// Context qui doit contenir les données necessaires a l'imports des ci
    /// </summary>
    /// <typeparam name="T">Type de parametre d'entrée</typeparam>
    [DebuggerDisplay("SocietesNeeded = {SocietesNeeded.Count} SocietesContexts = {SocietesContexts.Count} Responsables = {Responsables.Count} SocietesOfResponsables = {SocietesOfResponsables.Count} CiPays = {CiPays.Count}")]
    public class ImportPersonnelContext<T> where T : class
    {
        /// <summary>
        /// Parametre d'entrée qui vzrie en fonction du type d'import (excel, liste de ci ou societe)
        /// </summary>
        public T Input { get; set; }

        /// <summary>
        /// Societés necessaires a l'import
        /// </summary>
        public List<SocieteEnt> SocietesNeeded { get; internal set; } = new List<SocieteEnt>();

        /// <summary>
        /// Context pour une societe qui doit contenir les données necessaires a l'imports des ci
        /// </summary>
        public List<ImportPersonnelSocieteContext> SocietesContexts { get; internal set; } = new List<ImportPersonnelSocieteContext>();

        /// <summary>
        /// Les pays des cis necessaires pour l'envoie des infos a SAP
        /// </summary>
        public List<PaysEnt> PersonnelPays { get; internal set; } = new List<PaysEnt>();

        /// <summary>
        /// Le logger
        /// </summary>
        public PersonnelImportExportLogger Logger { get; internal set; }

        /// <summary>
        /// Ressources
        /// </summary>
        public List<RessourceEnt> Ressoures { get; internal set; } = new List<RessourceEnt>();

        /// <summary>
        /// Liste des type de societe, permettra de savoir si la societe est une sep et donc de savoir si on doit envoyé les ci de la societe a sap
        /// </summary>
        public List<TypeSocieteEnt> TypeSocietes { get; internal set; } = new List<TypeSocieteEnt>();
    }
}
