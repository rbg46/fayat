using System;
using System.Collections.Generic;
using Fred.Entities.Personnel.Imports;
using Fred.ImportExport.Business.Common;
using Fred.ImportExport.Business.Personnel.Etl.Process;
using Fred.ImportExport.Business.Personnel.EtlFactory;
using Fred.ImportExport.DataAccess.ExternalService.SalarieFesProxy;
using Fred.ImportExport.DataAccess.ExternalService.Tibco.Personnel;
using Fred.ImportExport.Framework.Etl.Input;
using Fred.ImportExport.Framework.Exceptions;

namespace Fred.ImportExport.Business.Personnel.Etl.Input
{
    public class PersonnelInputFes : IEtlInput<PersonnelModel>
    {
        private readonly PersonnelEtlParameter personnelFluxParameter;

        public PersonnelInputFes(PersonnelEtlParameter personnelFluxParameter)
        {
            this.personnelFluxParameter = personnelFluxParameter;
        }

        /// <summary>
        /// Contient le resultat de l'importation Anael
        /// </summary>
        public IList<PersonnelModel> Items { get; set; }

        /// <inheritdoc/>
        /// Appelé par l'ETL
        public void Execute()
        {
            Items = GetPersonnelFromTibco();
        }

        /// <summary>
        ///   Récupération des personnels d'AS400 
        /// </summary>   
        /// <returns>Liste des personnels</returns>
        public IList<PersonnelModel> GetPersonnelFromTibco()
        {
            bool bypassDate = personnelFluxParameter.ByPassDate;
            DateTime? dateDerniereExecution = personnelFluxParameter.Flux.DateDerniereExecution;
            try
            {
                PersonnelTibcoRepositoryExterne personnelRepoExterne = new PersonnelTibcoRepositoryExterne();
                return ReadLine(personnelRepoExterne.GetSalarieFesFromTibco(dateDerniereExecution, bypassDate));
            }
            catch (Exception ex)
            {
                throw new FredIeBusinessException(string.Format(IEBusiness.FluxErreurRecuperation, this.personnelFluxParameter.CodeFlux), ex);
            }
        }

        protected IList<PersonnelModel> ReadLine(SalarieOutRecord[] tibcoResult)
        {
            List<PersonnelModel> personnelFes = new List<PersonnelModel>();
            if (tibcoResult != null && tibcoResult.Length > 0)
            {
                List<string> codeSocietePayes = PersonnelCodeSocietePayeSpliter.GetCodeSocietePayesInFlux(personnelFluxParameter.Flux);
                foreach (SalarieOutRecord item in tibcoResult)
                {
                    if (!string.IsNullOrEmpty(item.societe) && codeSocietePayes.Contains(item.societe.Trim()))
                    {
                        personnelFes.Add(new PersonnelModel()
                        {
                            CodeSocietePaye = item.societe?.Trim(),
                            CodeEtablissement = item.etablissement?.Trim(),
                            Matricule = item.matricule?.Trim(),
                            Nom = item.nom?.Trim(),
                            Prenom = item.prenom?.Trim(),
                            CategoriePerso = item.categorie_perso?.Trim(),
                            Statut = item.statut_categorie?.Trim(),
                            CodeEmploi = item.code_emploi?.Trim(),
                            DateEntree = item.date_embauche,
                            DateSortie = item.date_sortieSpecified ? item.date_sortie : default(DateTime?),
                            DateModification = item.date_modification,
                            Email = item.email?.Trim(),
                            Section = item.code_imputation?.Trim(),
                            Entreprise = item.code_imputation2?.Trim(),
                            SocieteComptable = item.societe_comptable_salarie?.Trim(),
                            EtablissementComptable = item.etabli_comptable_salarie?.Trim(),
                        }
                        );
                    }
                }
            }

            return personnelFes;
        }
    }
}
