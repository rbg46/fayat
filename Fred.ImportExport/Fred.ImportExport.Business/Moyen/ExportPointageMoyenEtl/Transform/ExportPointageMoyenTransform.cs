using System;
using CommonServiceLocator;
using Fred.Business.Rapport.Pointage;
using Fred.Entities.Rapport;
using Fred.ImportExport.Business.Etl.Process;
using Fred.ImportExport.DataAccess.ExternalService.ExportMoyenPointageProxy;
using Fred.ImportExport.Framework.Etl.Input;
using Fred.ImportExport.Framework.Etl.Result;
using Fred.ImportExport.Framework.Etl.Transform;
using Fred.ImportExport.Framework.Exceptions;
using Fred.ImportExport.Models.Moyen;
using Fred.Web.Shared.Extentions;

namespace Fred.ImportExport.Business.Moyen.ExportPointageMoyenEtl.Transform
{
    /// <summary>
    /// Processus etl : Transformation du resultat.
    /// </summary>
    public class ExportPointageMoyenTransform : IEtlTransform<ExportPointageMoyenModel, GestionMoyenInRecord>
    {
        /// <summary>
        /// Etl execution logger
        /// </summary>
        private readonly EtlExecutionLogger etlExecutionLogger;

        /// <summary>
        /// Pointage manager
        /// </summary>
        private readonly Lazy<IPointageManager> pointageManager = new Lazy<IPointageManager>(() =>
        {
            return ServiceLocator.Current.GetInstance<IPointageManager>();
        });

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="etlExecutionLogger">etlExecutionLogger</param>  
        public ExportPointageMoyenTransform(EtlExecutionLogger etlExecutionLogger)
        {
            this.etlExecutionLogger = etlExecutionLogger;
        }

        /// <summary>
        /// Exécution de la transformation
        /// </summary>
        /// <param name="input">Object input</param>
        /// <param name="result">Résultat de la transformation</param>
        public void Execute(IEtlInput<ExportPointageMoyenModel> input, ref IEtlResult<GestionMoyenInRecord> result)
        {
            if (result == null)
            {
                result = new ExportPointageMoyenResult();
            }

            DateTime startDate = input.Items[0].StartDate;
            DateTime endDate = input.Items[0].EndDate;
            ExportTibcoRapportLigneModel[] rapportLignes = pointageManager.Value.GetPointageMoyenBetweenDates(startDate, endDate);

            if (rapportLignes.IsNullOrEmpty())
            {
                throw new FredIeBusinessException($"Aucun pointage à envoyer entre ces deux dates : {startDate.Date.ToString()} - {endDate.Date.ToString()}");
            }

            etlExecutionLogger.Log("Transform : Conversion des lignes de rapports à des GestionMoyenInRecord");

            // Le webservice soap attends un array , c'est pourquoi une seule conversion en array de GestionMoyenInRecord a été utilisée
            GestionMoyenInRecord[] recordsToSend = Array.ConvertAll(rapportLignes, x => ToGestionMoyenInRecord(x));

            result.Items = recordsToSend;
        }

        /// <summary>
        /// Conversion d'une ligne de rapport à un type GestionMoyenInRecord => le type attendu par le webservice SAOP
        /// </summary>
        /// <param name="model">Ligne de rapport</param>
        /// <returns>GestionMoyenInRecord => le type attendu par le webservice SAOP</returns>
        private GestionMoyenInRecord ToGestionMoyenInRecord(ExportTibcoRapportLigneModel model)
        {
            if (model == null)
            {
                return null;
            }

            GestionMoyenInRecord result = new GestionMoyenInRecord();

            result.DatePointage = model.DatePointage;
            result.Annee = model.Annee.ToString();
            result.Mois = model.Mois.ToString();

            result.SocieteCi = model.SocieteCi ?? string.Empty;
            result.SocieteCode = model.SocieteCode ?? string.Empty;

            result.MoyenCode = model.MoyenCode;
            result.CiCode = model.CiCode ?? string.Empty;

            result.EtablissementComptableCi = model.EtablissementComptableCi ?? string.Empty;
            result.EtablissementComptableCode = model.EtablissementComptableCode ?? string.Empty;

            // dans le cas d’une affectation sur une location créée par l’utilisateur, 
            // le numéro d’immatriculation est stocké dans la table FRED_MATERIEL_LOCATION.
            result.Immatriculation = model.Immatriculation ?? string.Empty;

            result.ConducteurSociete = model.ConducteurSociete ?? string.Empty;
            result.ConducteurMatricule = model.ConducteurMatricule ?? string.Empty;
            result.ConducteurNom = model.ConducteurNom ?? string.Empty;
            result.ConducteurPrenom = model.ConducteurPrenom ?? string.Empty;

            result.PersonnelSociete = model.PersonnelSociete ?? string.Empty;
            result.PersonnelMatricule = model.PersonnelMatricule ?? string.Empty;
            result.PersonnelNom = model.PersonnelNom ?? string.Empty;
            result.PersonnelPrenom = model.PersonnelPrenom ?? string.Empty;

            result.Commentaire = model.Commentaire ?? string.Empty;
            result.Quantite = model.Quantite;
            result.Unite = model.Unite;

            return result;
        }

    }
}
