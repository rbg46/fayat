using System.Collections.Generic;
using Fred.Entities.CI;
using Fred.Entities.Organisation;
using Fred.Entities.Personnel;
using Fred.Entities.Referential;
using Fred.Entities.Societe;
using Fred.ImportExport.Models.Ci;

namespace Fred.ImportExport.Business.CI.WebApi.Context.Models
{
    public class ImportCiByWebApiSocieteContext
    {
        /// <summary>
        /// La Societe 
        /// </summary>
        public SocieteEnt Societe { get; set; }
        /// <summary>
        /// Les EtablissementComptables de la societe
        /// </summary>
        public List<EtablissementComptableEnt> EtablissementComptables { get; set; } = new List<EtablissementComptableEnt>();
        /// <summary>
        /// le cis Fred
        /// </summary>
        public List<CIEnt> CisFoundInFredWithCode { get; set; } = new List<CIEnt>();

        /// <summary>
        /// Les ci web api
        /// </summary>
        public List<WebApiCiModel> CisToImport { get; set; }

        /// <summary>
        /// Le CodeSocieteComptable
        /// </summary>
        public string CodeSocieteComptable { get; set; }

        /// <summary>
        /// les societe des responsables
        /// </summary>
        public List<SocieteEnt> SocietesOfResponsableAffairesUsedInJson { get; set; }

        /// <summary>
        /// les responsables
        /// </summary>
        public List<PersonnelEnt> ResponsableAffairesUsedInJson { get; set; }

        //tous les type de ci
        public List<CITypeEnt> CiTypes { get; set; }

        /// <summary>
        /// les ci web api mappé en cieent
        /// </summary>
        public List<CIEnt> WebApiCisMappedToCiEnt { get; set; } = new List<CIEnt>();

        /// <summary>
        /// Tous les type d'organisations
        /// </summary>
        public List<TypeOrganisationEnt> TypeOrganisations { get; set; }
    }
}
