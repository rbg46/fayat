using System.Collections.Generic;
using Fred.Entities.Organisation.Tree;
using Fred.Entities.Personnel;
using Fred.Entities.Referential;
using Fred.Entities.Societe;
using Fred.ImportExport.Entities.Transposition;
using Fred.ImportExport.Models.Personnel;

namespace Fred.ImportExport.Business.Personnel.AnaelSystem.Context.Common
{
    public class ImportPersonnelSocieteContext
    {
        /// <summary>
        /// la societe de personnel
        /// </summary>
        public SocieteEnt Societe { get; internal set; }
        public List<PersonnelAnaelModel> AnaelPersonnels { get; internal set; }
        /// <summary>
        /// le personnels Fred
        /// </summary>
        public List<PersonnelEnt> FredPersonnels { get; internal set; } = new List<PersonnelEnt>();
        /// <summary>
        /// les etablissements de paie
        /// </summary>
        public List<EtablissementPaieEnt> EtablissementsPaies { get; internal set; } = new List<EtablissementPaieEnt>();
        /// <summary>
        /// liste des Transpositions codeEmploi vers Ressource 
        /// </summary>
        public List<TranspoCodeEmploiToRessourceEnt> TranspoCodeEmploiRessourceList { get; internal set; } = new List<TranspoCodeEmploiToRessourceEnt>();
        /// <summary>
        /// le groupe parent de la societe 
        /// </summary>
        public OrganisationBase SocieteGroupeParent { get; internal set; }
    }
}
