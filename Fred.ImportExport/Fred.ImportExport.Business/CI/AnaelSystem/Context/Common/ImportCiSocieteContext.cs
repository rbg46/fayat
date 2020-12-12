using System.Collections.Generic;
using System.Diagnostics;
using Fred.Entities;
using Fred.Entities.CI;
using Fred.Entities.Referential;
using Fred.Entities.Societe;
using Fred.ImportExport.Models.Ci;

namespace Fred.ImportExport.Business.CI.AnaelSystem.Context.Common
{
    /// <summary>
    /// Contient les données necessaires pour l'importation d'une societe
    /// </summary>
    [DebuggerDisplay("Societe = {Societe?.Code} EtablissementComptables = {EtablissementComptables.Count} AnaelCis = {AnaelCis.Count} FredCis = {FredCis.Count}")]

    public class ImportCiSocieteContext
    {
        /// <summary>
        /// La Societe 
        /// </summary>
        public SocieteEnt Societe { get; set; }
        /// <summary>
        /// Les EtablissementComptables de la societe
        /// </summary>
        public List<EtablissementComptableEnt> EtablissementComptables { get; internal set; } = new List<EtablissementComptableEnt>();
        /// <summary>
        /// Contient soit l'ensemble des cis d'anael pour chaque societe soit seulement ceux qui sont demandés
        /// </summary>
        public List<CiAnaelModel> AnaelCis { get; internal set; } = new List<CiAnaelModel>();
        /// <summary>
        /// le cis Fred
        /// </summary>
        public List<CIEnt> FredCis { get; internal set; } = new List<CIEnt>();
        /// <summary>
        /// Charge les type de societes, permettra de savoir si on doit envoyé les données de ci a SAP
        /// </summary>
        public List<TypeSocieteEnt> TypeSocietes { get; internal set; } = new List<TypeSocieteEnt>();
    }
}
