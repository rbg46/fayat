using System;
using Fred.Business.Utilisateur;
using Fred.Entities.Journal;
using Fred.ImportExport.Business.JournauxComptable.Etl.Result;
using Fred.ImportExport.Framework.Etl.Input;
using Fred.ImportExport.Framework.Etl.Result;
using Fred.ImportExport.Framework.Etl.Transform;
using Fred.ImportExport.Models.JournauxComptable;

namespace Fred.ImportExport.Business.JournauxComptable.Etl.Transform
{
    /// <summary>
    /// Processus etl : Transformation du resultat de la requête Anael
    /// </summary>
    public class JournauxComptableTransform : IEtlTransform<JournauxComptableAnaelModel, JournalEnt>
    {
        private readonly IUtilisateurManager utilisateurManager;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="societeId">Société à qui attribuer les Journaux comptables</param>
        /// <param name="utilisateurManager">Manager des utilisateur</param>
        public JournauxComptableTransform(int? societeId, IUtilisateurManager utilisateurManager)
        {
            SocieteId = societeId;
            this.utilisateurManager = utilisateurManager;
        }

        /// <summary>
        /// Société à qui attribuer les Journaux comptables
        /// </summary>
        private int? SocieteId { get; }

        /// <summary>
        /// Appelé par l'ETL
        /// Execute le process de transformation 
        /// </summary>
        /// <param name="input">données d'entrée de l'etl</param>
        /// <param name="result">données de sortie de l'etl</param>
        public void Execute(IEtlInput<JournauxComptableAnaelModel> input, ref IEtlResult<JournalEnt> result)
        {
            if (result == null)
            {
                result = new JournauxComptableResult();
            }

            int auteurCreationId = utilisateurManager.GetByLogin("fred_ie").UtilisateurId;

            foreach (JournauxComptableAnaelModel modelAnael in input.Items)
            {
                JournalEnt entity = ConvertToEntity(modelAnael, SocieteId, auteurCreationId);
                result.Items.Add(entity);
            }
        }

        private JournalEnt ConvertToEntity(JournauxComptableAnaelModel modelAnael, int? societeId, int auteurCreationId)
        {
            return new JournalEnt()
            {
                Code = modelAnael.CodeJournal,
                Libelle = modelAnael.NomJournal,
                TypeJournal = modelAnael.TypeJournal,
                SocieteId = societeId.Value,
                DateCreation = DateTime.UtcNow,
                AuteurCreationId = auteurCreationId
            };
        }
    }
}
