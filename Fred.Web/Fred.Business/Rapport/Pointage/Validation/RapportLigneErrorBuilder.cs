using System.Collections.Generic;
using Fred.Business.Rapport.Pointage.Validation.Interfaces;
using Fred.DataAccess.Interfaces;
using Fred.Entities.Rapport;

namespace Fred.Business.Rapport.Pointage.Validation
{
    public class RapportLigneErrorBuilder : IRapportLigneErrorBuilder
    {
        private readonly IPointageRepository pointageRepository;
        private readonly IPointageValidator pointageValidator;

        public RapportLigneErrorBuilder(
            IPointageRepository pointageRepository,
            IPointageValidator pointageValidator)
        {
            this.pointageRepository = pointageRepository;
            this.pointageValidator = pointageValidator;
        }

        /// <summary>
        /// Rajoutes les message d'erreurs a un rapportLigne. UTILISER L AUTRE API SI PLUSIEURS RAPPORT LIGNES !!!!!!!! 
        /// </summary>
        /// <param name="validationGlobalData">validationGlobalData</param>
        /// <param name="rapportLigne">rapportLigne</param>
        public void IncludeErrorsMessages(GlobalDataForValidationPointage validationGlobalData, RapportLigneEnt rapportLigne)
        {
            PointageValidator pointageValidator = this.pointageValidator as PointageValidator;
            IncludeErrorsMessagesForRapportLigne(validationGlobalData, pointageValidator, rapportLigne);
        }

        /// <summary>
        /// Rajoutes les message d'erreurs a des rapportLignes
        /// </summary>
        /// <param name="validationGlobalData">validationGlobalData</param>
        /// <param name="rapportLignes">rapportLignes</param>
        public void IncludeErrorsMessages(GlobalDataForValidationPointage validationGlobalData, IEnumerable<RapportLigneEnt> rapportLignes)
        {
            PointageValidator pointageValidator = this.pointageValidator as PointageValidator;

            foreach (RapportLigneEnt rapportLigne in rapportLignes)
            {
                IncludeErrorsMessagesForRapportLigne(validationGlobalData, pointageValidator, rapportLigne);
            }
        }

        /// <summary>
        /// Rajoutes les message d'erreurs a des rapportLignes pour le materiel uniquement
        /// </summary>
        /// <param name="rapportLignes">rapportLignes</param>
        public void IncludeErrorsMessagesMaterielOnly(IEnumerable<RapportLigneEnt> rapportLignes)
        {
            foreach (RapportLigneEnt rapportLigne in rapportLignes)
            {
                rapportLigne.ListErreurs = new List<string>();
                this.pointageValidator.CheckPointageMateriel(rapportLigne);
            }
        }
        private void IncludeErrorsMessagesForRapportLigne(GlobalDataForValidationPointage validationGlobalData, PointageValidator pointageValidator, RapportLigneEnt rapportLigne)
        {
            rapportLigne.ListErreurs = new List<string>();
            pointageValidator.CheckPointage(rapportLigne, validationGlobalData);

        }
    }
}
