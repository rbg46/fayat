using System.Collections.Generic;
using System.Linq;
using Fred.Business.Referential;
using Fred.Entities.Referential;
using Fred.ImportExport.Models;

namespace Fred.ImportExport.Business.Fournisseur.AnaelSystem.Converter
{
    /// <summary>
    /// Convertisseur d'un model Anael en entité fred
    /// </summary>
    public class AnaelModelToFournisseurFredModelConverter
    {
        private readonly int siretLenght = 5;
        private readonly int sirenLenght = 9;
        private readonly char zeroPadChar = '0';
        private readonly string professionLiberale = "P";

        public AnaelModelToFournisseurFredModelConverter(IPaysManager paysManager)
        {
            PaysList = paysManager.GetList().ToList();
        }

        /// <summary>
        /// Liste des pays FRED
        /// </summary>
        private List<PaysEnt> PaysList { get; }

        /// <summary>
        /// Convertie une liste de model Anael en entité fred
        /// </summary>
        /// <param name="fournisseurAnaelModels">Lmodel a convertir</param>
        /// <returns>Liste d'entites fred</returns>
        public List<FournisseurFredModel> ConvertAnaelModelToFournisseurFredModel(List<FournisseurAnaelModel> fournisseurAnaelModels)
        {
            var result = new List<FournisseurFredModel>();

            // A quoi sert cette liste ?? A supprimer si quelqu'un maitrise les impacts
            List<FournisseurAnaelModel> excluded = new List<FournisseurAnaelModel>();

            foreach (var fournisseurAnaelModel in fournisseurAnaelModels)
            {
                SetTypeTiers(fournisseurAnaelModel, excluded);
                result.Add(ConvertAnaelModelToFournisseurFredModel(fournisseurAnaelModel));
            }

            return result;
        }


        private FournisseurFredModel ConvertAnaelModelToFournisseurFredModel(FournisseurAnaelModel fournisseurAnaelModel)
        {
            return new FournisseurFredModel
            {
                Code = fournisseurAnaelModel.Code,
                TypeSequence = fournisseurAnaelModel.TypeSequence,
                Libelle = fournisseurAnaelModel.Libelle,
                Adresse = fournisseurAnaelModel.Adresse,
                CodePostal = fournisseurAnaelModel.CodePostal,
                Ville = fournisseurAnaelModel.Ville,
                Telephone = fournisseurAnaelModel.Telephone,
                Fax = fournisseurAnaelModel.Fax,
                Email = fournisseurAnaelModel.Email,
                SIRET = fournisseurAnaelModel.SIRET.PadLeft(siretLenght, zeroPadChar),
                SIREN = fournisseurAnaelModel.SIREN.PadLeft(sirenLenght, zeroPadChar),
                ModeReglement = fournisseurAnaelModel.ModeReglement,
                RegleGestion = fournisseurAnaelModel.RegleGestion,
                CodePays = fournisseurAnaelModel.CodePays,
                DateOuverture = fournisseurAnaelModel.DateOuverture,
                DateCloture = fournisseurAnaelModel.DateCloture,
                CritereRecherche = fournisseurAnaelModel.CritereRecherche,
                IsoTVA = fournisseurAnaelModel.IsoTVA,
                NumeroTVA = fournisseurAnaelModel.NumeroTVA,
                TVA = fournisseurAnaelModel.IsoTVA.Replace(" ", string.Empty) + fournisseurAnaelModel.NumeroTVA.Replace(" ", string.Empty),
                TypeTiers = fournisseurAnaelModel.TypeTiers,
                PaysId = PaysList.Find(x => x.Code.Trim() == fournisseurAnaelModel.CodePays)?.PaysId,
                CodeSociete = fournisseurAnaelModel.CodeSociete,
                IsProfessionLiberale = fournisseurAnaelModel.IsProfessionLiberale == professionLiberale
            };
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
    }
}
