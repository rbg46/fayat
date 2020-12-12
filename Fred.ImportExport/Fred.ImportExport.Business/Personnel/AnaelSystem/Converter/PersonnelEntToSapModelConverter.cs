using System.Collections.Generic;
using System.Linq;
using Fred.Entities.Personnel;
using Fred.Entities.Referential;
using Fred.ImportExport.Business.Personnel.AnaelSystem.Context.Common;
using Fred.ImportExport.Models.Personnel;

namespace Fred.ImportExport.Business.Personnel.AnaelSystem.Converter
{
    /// <summary>
    /// Convertisseur d'une entité fred en model SAP
    /// </summary>
    public class PersonnelEntToSapModelConverter
    {
        /// <summary>
        /// Convertie un PersonnelEnt en un PersonnelStormModel
        /// </summary>
        /// <typeparam name="T">Parametre d'entrée</typeparam>
        /// <param name="context">context</param>
        /// <param name="societeContext">Le context de la societe</param>
        /// <returns>Une liste de PersonnelStormModel</returns>
        public List<PersonnelStormModel> ConvertPersonnelEntToPersonnelSapModels<T>(ImportPersonnelContext<T> context, ImportPersonnelSocieteContext societeContext) where T : class
        {
            List<PersonnelStormModel> result = new List<PersonnelStormModel>();

            if (societeContext.FredPersonnels.Any())
            {
                foreach (PersonnelEnt personnelFred in societeContext.FredPersonnels)
                {
                    result.Add(ConvertPersonnelEntToPersonnelStormModel(context, societeContext, personnelFred));
                }
            }

            return result;
        }

        private PersonnelStormModel ConvertPersonnelEntToPersonnelStormModel<T>(ImportPersonnelContext<T> context, ImportPersonnelSocieteContext societeContext, PersonnelEnt personnelFred) where T : class
        {
            EtablissementPaieEnt etablissementPaye = societeContext.EtablissementsPaies.FirstOrDefault(x => x.EtablissementPaieId == personnelFred.EtablissementPaieId);

            string codePays = context.PersonnelPays.FirstOrDefault(x => x.PaysId == personnelFred.PaysId)?.Code;

            return new PersonnelStormModel
            {
                CodeSociete = societeContext.Societe.CodeSocieteComptable,
                CodeEtablissement = etablissementPaye?.Code,
                Matricule = personnelFred.Matricule,
                Nom = personnelFred.Nom,
                Prenom = personnelFred.Prenom,
                CategoriePerso = personnelFred.CategoriePerso,
                Statut = personnelFred.Statut,
                CodeEmploi = personnelFred.CodeEmploi,
                DateEntree = personnelFred.DateEntree,
                DateSortie = personnelFred.DateSortie,
                NomRue = personnelFred.Adresse1,
                CodePostal = personnelFred.CodePostal,
                Ville = personnelFred.Ville,
                PaysCode = codePays,
                DateModification = personnelFred.DateModification,
                Email = personnelFred.Email,
                IsPersonnelNonPointable = etablissementPaye != null ? etablissementPaye.IsPersonnelsNonPointables :  false
            };
        }
    }
}
