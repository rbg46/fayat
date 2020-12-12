using System.Collections.Generic;
using System.Diagnostics;
using Fred.Entities.Referential;
using Fred.Entities.Societe;
using Fred.ImportExport.Models;

namespace Fred.ImportExport.Business.Fournisseur.AnaelSystem.Context
{
    /// <summary>
    /// Contient les données necessaires pour l'importation d'une societe
    /// </summary>
    [DebuggerDisplay("Societe = {Societe?.Code} EtablissementComptables = {EtablissementComptables.Count} AnaelCis = {AnaelCis.Count} FredCis = {FredCis.Count}")]

    public class ImportFournisseurSocieteContext
    {
        /// <summary>
        /// la societe de fournisseur
        /// </summary>
        public SocieteEnt Societe { get; internal set; }

        public List<FournisseurFredModel> AnaelFournisseurs { get; internal set; }
        /// <summary>
        /// les fournisseurs Fred
        /// </summary>
        public List<FournisseurEnt> FredFournisseurs { get; internal set; } = new List<FournisseurEnt>();
    }
}
