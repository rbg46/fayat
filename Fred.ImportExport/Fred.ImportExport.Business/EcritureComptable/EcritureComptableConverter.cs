using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Fred.Entities.EcritureComptable;
using Fred.ImportExport.Models.EcritureComptable;
using Fred.Web.Shared.Models.Nature;

namespace Fred.ImportExport.Business.EcritureComptable
{
    /// <summary>
    /// Permet de convertir une écriture comptable
    /// </summary>
    public class EcritureComptableConverter
    {
        /// <summary>
        /// Converti une écriture comptable FAYAT TP (flux JSON) en model d'écriture comptable (FAYAT TP)
        /// </summary>
        /// <param name="ecritureComptableFayatTps">Liste de <see cref="EcritureComptableFayatTpImportModel"/> (JSON FAYAT TP)</param>
        /// <returns>Liste de <see cref="EcritureComptableFayatTpModel"/></returns>
        public List<EcritureComptableFayatTpModel> ConvertToEcritureComptableFayatTpModel(List<EcritureComptableFayatTpImportModel> ecritureComptableFayatTps)
        {
            List<EcritureComptableFayatTpModel> ecritureComptableFayatTpModels = new List<EcritureComptableFayatTpModel>();

            foreach (EcritureComptableFayatTpImportModel model in ecritureComptableFayatTps)
            {
                ecritureComptableFayatTpModels.Add(new EcritureComptableFayatTpModel
                {
                    DateCreation = model.CreationDateFormate,
                    DateComptable = model.DateComptableFormate,
                    GroupeCode = model.Groupe,
                    SocieteCode = model.SocieteCode,
                    Libelle = model.Libelle,
                    NatureAnalytique = model.NatureAnalytique,
                    MontantDeviseInterne = model.MontantDeviseInterne,
                    DeviseInterne = model.DeviseInterne,
                    MontantDeviseTransaction = model.MontantDeviseTransaction,
                    DeviseTransaction = model.DeviseTransaction,
                    CiCode = model.CiCode,
                    NumeroPiece = model.NumeroPiece,
                    NumeroCommande = model.NumeroCommande,
                    Ressource = model.Ressource,
                    Quantite = decimal.TryParse(model.Quantite, NumberStyles.Float, CultureInfo.CreateSpecificCulture("en-GB") ,out decimal number) ? number : default(decimal?),
                    Unite = model.UniteQuantite,
                    RapportLigneId = model.RapportLigneId,
                    Personne = model.Personne,
                    MaterielSocieteCode = model.MaterielSocieteCode,
                    MaterielCode = model.MaterielCode,
                    CodeRef = model.CodeRef
                });
            }
            return ecritureComptableFayatTpModels;
        }

        /// <summary>
        /// Converti un model d'écriture comptable FAYAT TP en écriture comptable DTO (FAYAT TP)
        /// </summary>
        /// <param name="ecritureComptableFayatTpModels">Liste de <see cref="EcritureComptableFayatTpModel"/> (JSON FAYAT TP)</param>
        /// <returns>Liste de <see cref="EcritureComptableFtpDto"/></returns>
        public List<EcritureComptableFtpDto> ConvertToEcritureComptableFtpDto(List<EcritureComptableFayatTpModel> ecritureComptableFayatTpModels)
        {
            List<EcritureComptableFtpDto> comptableFtpDtos = new List<EcritureComptableFtpDto>();

            foreach (EcritureComptableFayatTpModel model in ecritureComptableFayatTpModels)
            {
                comptableFtpDtos.Add(new EcritureComptableFtpDto
                {
                    CodeCi = model.CiCode,
                    CodeDevise = model.DeviseInterne,
                    CodeMateriel = model.MaterielCode,
                    CodeRef = model.CodeRef,
                    DateComptable = model.DateComptable.Value,
                    FamilleOperationDiverseId = model.FamilleOperationDiverseId,
                    FamilleOperationDiversesCode = model.CodeFamille,
                    Libelle = model.Libelle,
                    Montant = model.MontantDeviseInterne.Value,
                    MontantDevise = model.MontantDeviseTransaction.Value,
                    NumeroPiece = model.NumeroPiece,
                    PersonnelCode = model.Personne,
                    Quantite = model.Quantite == null ? 1 : model.Quantite.Value == 0 ? 1 : model.Quantite.Value,
                    RapportLigneId = model.RapportLigneId != null ? int.Parse(model.RapportLigneId) : default(int?),
                    RessourceCode = model.Ressource,
                    NatureAnalytique = model.CodeNature,
                    SocieteMaterielCode = model.SocieteCode,
                    NumeroCommande = model.NumeroCommande,
                    GroupeCode = model.GroupeCode,
                    SocieteCode = model.SocieteCode,
                    Unite = model.Unite,
                    NumFactureSAP = model.NumFactureSAP
                });
            }
            return comptableFtpDtos;
        }

        /// <summary>
        /// Converti une liste de en <see cref="EcritureComptableFayatTpModel"/> liste de <see cref="EcritureComptableFtpSapModel"/>
        /// </summary>
        /// <param name="ecritureComptables">Liste de <see cref="EcritureComptableFayatTpModel"/> </param>
        /// <returns>Liste de <see cref="EcritureComptableFtpSapModel"/></returns>
        public IEnumerable<EcritureComptableFtpSapModel> ConvertToEcritureComptableFtpSapModel(List<EcritureComptableFayatTpModel> ecritureComptables)
        {
            List<EcritureComptableFtpSapModel> ecritures = new List<EcritureComptableFtpSapModel>();

            foreach (EcritureComptableFayatTpModel ecriture in ecritureComptables)
            {
                ecritures.Add(new EcritureComptableFtpSapModel
                {
                    NumeroPiece = ecriture.NumeroPiece,
                    Erreurs = ecriture.Errors,
                    Message = ecriture.Errors.Count == 0 ? "Import réussi " : "Erreur d'import"
                });
            }
            return ecritures;
        }

        /// <summary>
        /// Converti une liste de en <see cref="EcritureComptableFtpSapModel"/> liste de <see cref="EcritureComptableFtpDto"/>
        /// </summary>
        /// <param name="ecritureComptables">Liste de <see cref="EcritureComptableFtpDto"/></param>
        /// <returns>Liste de <see cref="EcritureComptableFtpSapModel"/></returns>
        public IEnumerable<EcritureComptableFtpSapModel> ConvertToEcritureComptableFtpSapModel(IEnumerable<EcritureComptableFtpDto> ecritureComptables)
        {
            List<EcritureComptableFtpSapModel> ecritures = new List<EcritureComptableFtpSapModel>();

            foreach (EcritureComptableFtpDto ecriture in ecritureComptables)
            {
                ecritures.Add(new EcritureComptableFtpSapModel
                {
                    NumeroPiece = ecriture.NumeroPiece,
                    Erreurs = ecriture.ErrorMessages,
                    Message = ecriture.ErrorMessages != null && ecriture.ErrorMessages.Count != 0 ? "Erreur d'import" : "Import réussi"
                });
            }
            return ecritures;
        }

        /// <summary>
        /// Permet de faire une jointure entre une liste de EcritureComptableFayatTpImportModel et de NatureFamilleOdModel en se basant sur la nature analytique et le code de la société
        /// </summary>
        /// <param name="ecritureComptableFayatTps"><see cref="EcritureComptableFayatTpImportModel"/></param>
        /// <param name="natureFamilleOds"><see cref="NatureFamilleOdModel"/></param>
        /// <returns><see cref="EcritureComptableFayatTpModel"/></returns>
        public List<EcritureComptableFayatTpModel> Join(List<EcritureComptableFayatTpImportModel> ecritureComptableFayatTps, List<NatureFamilleOdModel> natureFamilleOds)
        {
            return (from ecriture in ecritureComptableFayatTps
                    join nature in natureFamilleOds on new { ecriture.SocieteCode, ecriture.NatureAnalytique } equals new { nature.SocieteCode, nature.NatureAnalytique }
                    into results
                    from result in results.DefaultIfEmpty()
                    select new EcritureComptableFayatTpModel
                    {
                        CiCode = ecriture.CiCode,
                        CodeNature = result?.NatureAnalytique,
                        CodeRef = ecriture.CodeRef,
                        DateComptable = ecriture.DateComptableFormate,
                        DateCreation = ecriture.CreationDateFormate,
                        DeviseInterne = ecriture.DeviseInterne,
                        DeviseTransaction = ecriture.DeviseTransaction,
                        GroupeCode = ecriture.Groupe,
                        Libelle = ecriture.Libelle,
                        MaterielCode = ecriture.MaterielCode,
                        MaterielSocieteCode = ecriture.MaterielSocieteCode,
                        MontantDeviseInterne = ecriture.MontantDeviseInterne,
                        MontantDeviseTransaction = ecriture.MontantDeviseTransaction,
                        NatureAnalytique = ecriture.NatureAnalytique == null ? string.Empty : result == null ? "UNKNOW_" + ecriture.NatureAnalytique : ecriture.NatureAnalytique,
                        NumeroCommande = ecriture.NumeroCommande,
                        NumeroPiece = ecriture.NumeroPiece,
                        Personne = ecriture.Personne,
                        Quantite = decimal.TryParse(ecriture.Quantite, NumberStyles.Float, CultureInfo.CreateSpecificCulture("en-GB"), out decimal number) ? number : default(decimal?),
                        RapportLigneId = ecriture.RapportLigneId,
                        Ressource = ecriture.Ressource,
                        SocieteCode = ecriture.SocieteCode,
                        SocieteId = result != null ? 0 : -1,
                        Unite = ecriture.UniteQuantite,
                        ParentFamilyODWithOrder = result?.ParentFamilyODWithOrder ?? -1,
                        ParentFamilyODWithoutOrder = result?.ParentFamilyODWithoutOrder ?? -1,
                        CodeFamilleWithOrder = result != null ? result.CodeFamilleODWithOrder : string.Empty,
                        CodeFamilleWithoutOrder = result != null ? result.CodeFamilleODWhitoutOrder : string.Empty,
                        NumFactureSAP = ecriture.NumFactureSAP
                    }).ToList();
        }
    }
}
