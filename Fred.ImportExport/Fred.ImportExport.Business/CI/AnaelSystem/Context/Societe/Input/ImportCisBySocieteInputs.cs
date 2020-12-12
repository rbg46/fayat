using System;
using System.Diagnostics;
using Fred.Entities.Societe;

namespace Fred.ImportExport.Business.CI.AnaelSystem.Context.Societe.Input
{
    /// <summary>
    /// Container des parametres d'entrée pour l'import par societe
    /// </summary>
    [DebuggerDisplay("Societe = {Societe?.Code} CodeSocieteCompatble = {CodeSocieteCompatble} IsFullImport = {IsFullImport}  dateDerniereExecution = {DateDerniereExecution}  SendToSap = {SendToSap}")]
    public class ImportCisBySocieteInputs
    {
        /// <summary>
        /// Le codeSocieteComptable 
        /// </summary>
        public string CodeSocieteCompatble { get; set; }

        /// <summary>
        /// Societe dont t'on doit faire l'import
        /// </summary>
        public SocieteEnt Societe { get; internal set; }

        /// <summary>
        /// Permet de savoir si on doit faire un import complet
        /// </summary>
        public bool IsFullImport { get; set; }

        /// <summary>
        /// Date du dernier import réussit
        /// </summary>
        public DateTime? DateDerniereExecution { get; set; }

    }
}
