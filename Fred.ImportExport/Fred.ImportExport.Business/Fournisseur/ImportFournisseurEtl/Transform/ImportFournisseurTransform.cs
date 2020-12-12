using System.Collections.Generic;
using System.Linq;
using Fred.Business.Referential;
using Fred.Entities.Referential;
using Fred.ImportExport.Business.Fournisseur.Etl.Result;
using Fred.ImportExport.Framework.Etl.Input;
using Fred.ImportExport.Framework.Etl.Result;
using Fred.ImportExport.Framework.Etl.Transform;
using Fred.ImportExport.Models;

namespace Fred.ImportExport.Business.Fournisseur.Etl.Transform
{
    /// <summary>
    /// Processus etl : Transformation du resultat de la requête Anael
    /// </summary>
    public class ImportFournisseurTransform : IEtlTransform<FournisseurModel, FournisseurFredModel>
    {
        private readonly IPaysManager paysMgr;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="paysMgr">Le gestionnaire des pays</param>
        public ImportFournisseurTransform(IPaysManager paysMgr)
        {
            this.paysMgr = paysMgr;
        }


        /// <summary>
        /// Appelé par l'ETL
        /// Execute le process de transformation 
        /// </summary>
        /// <param name="input">données d'entrée de l'etl</param>
        /// <param name="result">données de sortie de l'etl</param>
        public void Execute(IEtlInput<FournisseurModel> input, ref IEtlResult<FournisseurFredModel> result)
        {
            if (result == null)
            {
                result = new ImportFournisseurResult();
            }

            if (input?.Items.Count > 0)
            {
                List<PaysEnt> pays = paysMgr.GetList().ToList();

                foreach (var modelAnael in input.Items)
                {
                    var entity = ConvertToModel(modelAnael, pays);
                    result.Items.Add(entity);
                }
            }
        }

        private FournisseurFredModel ConvertToModel(FournisseurModel fournisseur, List<PaysEnt> pays)
        {
            int? paysId = pays.Find(x => x.Code.Trim() == fournisseur.CodePays)?.PaysId;

            return new FournisseurFredModel
            {
                Code = fournisseur.Code,
                TypeSequence = fournisseur.TypeSequence,
                Libelle = fournisseur.Libelle,
                Adresse = fournisseur.Adresse,
                CodePostal = fournisseur.CodePostal,
                Ville = fournisseur.Ville,
                Telephone = fournisseur.Telephone,
                Fax = fournisseur.Fax,
                Email = fournisseur.Email,
                SIRET = fournisseur.SIRET,
                SIREN = fournisseur.SIREN,
                ModeReglement = fournisseur.ModeReglement,
                RegleGestion = fournisseur.RegleGestion,
                CodePays = fournisseur.CodePays,
                DateOuverture = fournisseur.DateOuverture,
                DateCloture = fournisseur.DateCloture,
                TypeTiers = fournisseur.TypeTiers,
                PaysId = paysId,
                CodeSociete = fournisseur.SocieteCode,
                IsProfessionLiberale = fournisseur.IsProfessionLiberale
            };
        }
    }
}
