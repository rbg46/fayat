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
    public class FournisseurAnaelTransform : IEtlTransform<FournisseurAnaelModel, FournisseurFredModel>
    {
        private readonly int siretLenght = 5;
        private readonly int sirenLenght = 9;
        private readonly char zeroPadChar = '0';
        private readonly string professionLiberale = "P";

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="paysManager">Gestionnaire FRED des Pays</param>
        public FournisseurAnaelTransform(IPaysManager paysManager)
        {
            PaysList = paysManager.GetList().ToList();
        }

        /// <summary>
        /// Liste des pays FRED
        /// </summary>
        private List<PaysEnt> PaysList { get; }

        /// <summary>
        /// Appelé par l'ETL
        /// Execute le process de transformation 
        /// </summary>
        /// <param name="input">données d'entrée de l'etl</param>
        /// <param name="result">données de sortie de l'etl</param>
        public void Execute(IEtlInput<FournisseurAnaelModel> input, ref IEtlResult<FournisseurFredModel> result)
        {
            if (result == null)
            {
                result = new FournisseurAnaelResult();
            }

            // A quoi sert cette liste ?? A supprimer si quelqu'un maitrise les impacts
            List<FournisseurAnaelModel> excluded = new List<FournisseurAnaelModel>();
            foreach (var modelAnael in input.Items)
            {
                SetTypeTiers(modelAnael, excluded);
                var entity = ConvertToModel(modelAnael);
                result.Items.Add(entity);
            }
        }

        /// <summary>
        ///   Détermine et renseigne le type tiers fournisseur
        /// </summary>
        /// <param name="f">fournisseur</param>
        /// <param name="exluced">Liste des fournisseurs exclus</param>    
        private void SetTypeTiers(FournisseurAnaelModel f, List<FournisseurAnaelModel> exluced)
        {
            const string locatier = "L", interim = "I";
            const string ccInterim = "621110";
            const string ccLocatier = "6135";

            // RG_503_012
            if ((f.CC1 == ccInterim
                || f.CC2 == ccInterim
                || f.CC3 == ccInterim
                || f.CC4 == ccInterim
                || f.CC5 == ccInterim
                || f.CC6 == ccInterim
                || f.CC7 == ccInterim
                || f.CC8 == ccInterim)
                && (f.CC1.StartsWith(ccLocatier)
                || f.CC2.StartsWith(ccLocatier)
                || f.CC3.StartsWith(ccLocatier)
                || f.CC4.StartsWith(ccLocatier)
                || f.CC5.StartsWith(ccLocatier)
                || f.CC6.StartsWith(ccLocatier)
                || f.CC7.StartsWith(ccLocatier)
                || f.CC8.StartsWith(ccLocatier)))
            {
                exluced.Add(f);
            }
            else if (f.CC1 == ccInterim
                || f.CC2 == ccInterim
                || f.CC3 == ccInterim
                || f.CC4 == ccInterim
                || f.CC5 == ccInterim
                || f.CC6 == ccInterim
                || f.CC7 == ccInterim
                || f.CC8 == ccInterim)
            {
                f.TypeTiers = interim;
            }
            else if (f.CC1.StartsWith(ccLocatier)
                || f.CC2.StartsWith(ccLocatier)
                || f.CC3.StartsWith(ccLocatier)
                || f.CC4.StartsWith(ccLocatier)
                || f.CC5.StartsWith(ccLocatier)
                || f.CC6.StartsWith(ccLocatier)
                || f.CC7.StartsWith(ccLocatier)
                || f.CC8.StartsWith(ccLocatier))
            {
                f.TypeTiers = locatier;
            }
        }

        private FournisseurFredModel ConvertToModel(FournisseurAnaelModel f)
        {
            return new FournisseurFredModel
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
                SIRET = f.SIRET.PadLeft(siretLenght, zeroPadChar),
                SIREN = f.SIREN.PadLeft(sirenLenght, zeroPadChar),
                ModeReglement = f.ModeReglement,
                RegleGestion = f.RegleGestion,
                CodePays = f.CodePays,
                DateOuverture = f.DateOuverture,
                DateCloture = f.DateCloture,
                CritereRecherche = f.CritereRecherche,
                IsoTVA = f.IsoTVA,
                NumeroTVA = f.NumeroTVA,
                TVA = f.IsoTVA.Replace(" ", string.Empty) + f.NumeroTVA.Replace(" ", string.Empty),
                TypeTiers = f.TypeTiers,
                PaysId = PaysList.Find(x => x.Code.Trim() == f.CodePays)?.PaysId,
                CodeSociete = f.CodeSociete,
                IsProfessionLiberale = f.IsProfessionLiberale == professionLiberale
            };
        }
    }
}
