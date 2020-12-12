using System.Collections.Generic;
using System.Threading.Tasks;
using Fred.Business.Referential;
using Fred.Business.Societe;
using Fred.Entities.Referential;
using Fred.Entities.Societe;
using Fred.ImportExport.Framework.Etl.Output;
using Fred.ImportExport.Framework.Etl.Result;
using Fred.ImportExport.Models;

namespace Fred.ImportExport.Business.Fournisseur.Etl.Output
{
    /// <summary>
    /// Processus etl : Execution de la sortie de l'import des Fournisseurs
    /// Envoie dans Fred les fournisseurs
    /// </summary>
    internal class ImportFournisseurOuput : IEtlOutput<FournisseurFredModel>
    {
        private readonly IFournisseurManager fournisseurManager;
        private readonly ISocieteManager societeMgr;

        public ImportFournisseurOuput(IFournisseurManager fournisseurManager, ISocieteManager societeMgr)
        {
            this.fournisseurManager = fournisseurManager;
            this.societeMgr = societeMgr;
        }

        /// <summary>
        /// Appelé par l'ETL
        /// Envoie les fournisseurs à Fred
        /// </summary>
        /// <param name="result">liste des fournisseurs à envoyer à Fred</param>
        public async Task ExecuteAsync(IEtlResult<FournisseurFredModel> result)
        {
            await Task.Run(() =>
            {
                List<FournisseurEnt> fournisseurs = new List<FournisseurEnt>();

                if (result?.Items.Count > 0)
                {
                    // On récupére le groupe avec le 1er élément. (Un seul appel en base de données)
                    SocieteEnt societe = societeMgr.GetSocieteByCodeSocieteComptable(result.Items[0].CodeSociete);
                    int? groupeId = societe?.GroupeId;

                    foreach (var f in result.Items)
                    {
                        fournisseurs.Add(new FournisseurEnt
                        {
                            Code = f.Code,
                            TypeSequence = f.TypeSequence,
                            Libelle = f.Libelle,
                            Adresse = f.Adresse,
                            CodePostal = f.CodePostal,
                            Ville = f.Ville,
                            Telephone = f.Telephone,
                            Fax = f.Fax,
                            Email = f.Email,
                            SIRET = f.SIRET,
                            SIREN = f.SIREN,
                            ModeReglement = f.ModeReglement,
                            RegleGestion = f.RegleGestion,
                            PaysId = f.PaysId,
                            DateOuverture = f.DateOuverture,
                            DateCloture = f.DateCloture,
                            TypeTiers = f.TypeTiers
                        });
                    }

                    // Import vers FRED
                    fournisseurManager.ManageImportedFournisseurs(fournisseurs, societe?.Code, groupeId);
                }
            });
        }
    }
}